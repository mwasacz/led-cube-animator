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
        private string _filePath;

        public Animation Animation { get; private set; }

        public void New()
        {
            var group = new Group { Name = "MainGroup" };
            Animation = new Animation { MainGroup = group, Size = 4, MonoColor = Colors.White };
            _filePath = null;
            _undo.Reset();
        }

        public bool Open(string path)
        {
            var animation = FileReaderWriter.Open(path);
            if (animation != null)
            {
                Animation = animation;
                _filePath = path;
                _undo.Reset();
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
        public void Undo() => _undo.Undo();
        public void Redo() => _undo.Redo();

        public void SetAnimationProperties(int size, ColorMode colorMode, Color monoColor, int frameDuration)
        {
            var group = _undo.Group(false);
            _undo.Set(Animation, nameof(Animation.Size), size, true);
            _undo.Set(Animation, nameof(Animation.ColorMode), colorMode, true);
            _undo.Set(Animation, nameof(Animation.MonoColor), monoColor, true);
            _undo.Set(Animation, nameof(Animation.Size), size, true);
            group.Finish();
        }

        public void SetTileProperty(Tile tile, string name, object newValue) => _undo.Set(tile, name, newValue, false);

        public void AddTile(Group group, Tile newTile)
        {
            _undo.Add(group.Children, newTile, false);
        }

        public void RemoveTile(Group group, Tile oldTile)
        {
            _undo.Remove(group.Children, oldTile, false);
        }

        public void SetVoxel(Frame frame, Color newColor, params int[] indices)
        {
            _undo.Group(true);
            _undo.ChangeArray(frame.Voxels, newColor, true, indices);
        }

        public event EventHandler<PropertiesChangedEventArgs> PropertiesChanged;

        private void Undo_ActionExecuted(object sender, ActionExecutedEventArgs e)
        {
            var actions = (e.Action as GroupAction)?.Actions ?? new[] { e.Action };
            PropertiesChanged?.Invoke(this, new PropertiesChangedEventArgs(actions
                .Select(a => new PropertiesChangedEventArgs.Change(GetChangedObject(a), GetChangedProperty(a)))
                .ToArray()));
        }

        private object GetChangedObject(IAction action)
        {
            switch (action)
            {
                case PropertyChangeAction propertyChange:
                    return propertyChange.Object;
                case CollectionChangeAction<Tile> collectionChange:
                    return FindTile(Animation.MainGroup, t => t is Group g && g.Children == collectionChange.Collection);
                case ArrayChangeAction<Color> arrayChange:
                    return FindTile(Animation.MainGroup, t => t is Frame f && f.Voxels == arrayChange.Array);
                default:
                    throw new Exception(); // ToDo
            }
        }

        private static string GetChangedProperty(IAction action)
        {
            switch (action)
            {
                case PropertyChangeAction propertyChange:
                    return propertyChange.Property;
                case CollectionChangeAction<Tile> collectionChange:
                    return nameof(Group.Children);
                case ArrayChangeAction<Color> arrayChange:
                    return nameof(Frame.Voxels);
                default:
                    throw new Exception(); // ToDo
            }
        }

        private static Tile FindTile(Tile tile, Predicate<Tile> predicate)
        {
            if (predicate(tile))
            {
                return tile;
            }
            if (tile is Group g)
            {
                foreach (var t in g.Children)
                {
                    var ret = FindTile(t, predicate);
                    if (ret != null)
                    {
                        return ret;
                    }
                }
            }
            return null;
        }
    }
}
