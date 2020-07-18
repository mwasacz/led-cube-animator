using LedCubeAnimator.Model.Undo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LedCubeAnimator.Model
{
    public class ModelManager : IModelManager
    {
        public ModelManager()
        {
            _undo.ActionExecuted += Undo_ActionExecuted;
            New();
        }

        private readonly UndoManager _undo = new UndoManager();
        private object _changedObject;
        private string _changedProperty;
        private string _filePath;

        public Animation Animation { get; private set; }

        public void New()
        {
            var group = new Group { Name = "MainGroup" };
            Animation = new Animation { MainGroup = group, Size = 4, MonoColor = Colors.White };
            _filePath = null;
            _undo.Reset();
            _changedObject = null;
            _changedProperty = null;
        }

        public bool Open(string path)
        {
            var animation = FileReaderWriter.Open(path);
            if (animation != null)
            {
                Animation = animation;
                _filePath = path;
                _undo.Reset();
                _changedObject = null;
                _changedProperty = null;
                return true;
            }
            return false;
        }

        public bool Save()
        {
            if (_filePath != null)
            {
                FileReaderWriter.Save(_filePath, Animation);
                return true;
            }
            return false;
        }

        public void SaveAs(string path)
        {
            _filePath = path;
            FileReaderWriter.Save(_filePath, Animation);
        }

        public void Export(string path)
        {
            Exporter.Export(path, Animation);
        }

        public Color[,,] Render(int time) => Renderer.Render(Animation, time, true);

        public bool CanUndo => _undo.CanUndo;

        public bool CanRedo => _undo.CanRedo;

        public void Undo()
        {
            _undo.Undo();
            _changedObject = null;
            _changedProperty = null;
        }

        public void Redo()
        {
            _undo.Redo();
            _changedObject = null;
            _changedProperty = null;
        }

        public void SetAnimationProperties(int size, ColorMode colorMode, Color monoColor, int frameDuration)
        {
            _undo.Group(() =>
            {
                _undo.Set(Animation, nameof(Animation.Size), size);
                _undo.Set(Animation, nameof(Animation.ColorMode), colorMode);
                _undo.Set(Animation, nameof(Animation.MonoColor), monoColor);
                _undo.Set(Animation, nameof(Animation.FrameDuration), frameDuration);
                if (colorMode != ColorMode.RGB)
                {
                    SetGroupColorMode(Animation.MainGroup, colorMode);
                }
            });
            _changedObject = Animation;
            _changedProperty = null;
        }

        public void SetTileProperty(Tile tile, string name, object newValue)
        {
            _undo.Group(() => _undo.Set(tile, name, newValue), _changedObject == tile && _changedProperty == name);
            _changedObject = tile;
            _changedProperty = name;
        }

        public void AddTile(Group group, Tile newTile)
        {
            _undo.Group(() => _undo.Add(group.Children, newTile));
            _changedObject = group;
            _changedProperty = nameof(Group.Children);
        }

        public void RemoveTile(Group group, Tile oldTile)
        {
            _undo.Group(() => _undo.Remove(group.Children, oldTile));
            _changedObject = group;
            _changedProperty = nameof(Group.Children);
        }

        public void SetVoxel(Frame frame, Color newColor, params int[] indices)
        {
            _undo.Group(() => _undo.ChangeArray(frame.Voxels, newColor, indices), _changedObject == frame && _changedProperty == nameof(Frame.Voxels));
            _changedObject = frame;
            _changedProperty = nameof(Frame.Voxels);
        }

        public event EventHandler<PropertiesChangedEventArgs> PropertiesChanged;

        private void Undo_ActionExecuted(object sender, ActionExecutedEventArgs e)
        {
            var groups = new Dictionary<ICollection<Tile>, Group>();
            var frames = new Dictionary<Array, Frame>();
            MapGroupsAndFrames(Animation.MainGroup, groups, frames);
            var changes = ((GroupAction)e.Action).Actions.Select(a => GetChange(a, groups, frames)).ToArray();

            PropertiesChanged?.Invoke(this, new PropertiesChangedEventArgs(changes));
        }

        private static void MapGroupsAndFrames(Group group, Dictionary<ICollection<Tile>, Group> groups, Dictionary<Array, Frame> frames)
        {
            groups.Add(group.Children, group);
            foreach (var t in group.Children)
            {
                switch (t)
                {
                    case Frame f:
                        frames.Add(f.Voxels, f);
                        break;
                    case Group g:
                        MapGroupsAndFrames(g, groups, frames);
                        break;
                }
            }
        }

        private static KeyValuePair<object, string> GetChange(IAction action, Dictionary<ICollection<Tile>, Group> groups, Dictionary<Array, Frame> frames)
        {
            switch (action)
            {
                case PropertyChangeAction propertyChange:
                    return new KeyValuePair<object, string>(propertyChange.Object, propertyChange.Property);
                case CollectionChangeAction<Tile> collectionChange:
                    return new KeyValuePair<object, string>(groups[collectionChange.Collection], nameof(Group.Children));
                case ArrayChangeAction<Color> arrayChange:
                    return new KeyValuePair<object, string>(frames[arrayChange.Array], nameof(Frame.Voxels));
                default:
                    throw new Exception(); // ToDo
            }
        }

        private void SetGroupColorMode(Group group, ColorMode colorMode)
        {
            foreach (var t in group.Children)
            {
                switch (t)
                {
                    case Frame f:
                        for (int x = 0; x < f.Voxels.GetLength(0); x++)
                        {
                            for (int y = 0; y < f.Voxels.GetLength(1); y++)
                            {
                                for (int z = 0; z < f.Voxels.GetLength(2); z++)
                                {
                                    Color oldColor = f.Voxels[x, y, z];
                                    switch (colorMode)
                                    {
                                        case ColorMode.Mono:
                                            _undo.ChangeArray(f.Voxels, oldColor.GetBrightness() > 127 ? Colors.White : Colors.Black, x, y, z);
                                            break;
                                        case ColorMode.MonoBrightness:
                                            _undo.ChangeArray(f.Voxels, Colors.White.Multiply(oldColor.GetBrightness()).Opaque(), x, y, z);
                                            break;
                                    }
                                }
                            }
                        }
                        break;
                    case Group g:
                        SetGroupColorMode(g, colorMode);
                        break;
                }
            }
        }
    }
}
