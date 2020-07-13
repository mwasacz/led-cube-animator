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
        private readonly Dictionary<ICollection<Tile>, Group> _groups = new Dictionary<ICollection<Tile>, Group>();
        private readonly Dictionary<Array, Frame> _frames = new Dictionary<Array, Frame>();
        private bool _groupChange;

        public Animation Animation { get; private set; }

        public void New()
        {
            var group = new Group { Name = "MainGroup" };
            Animation = new Animation { MainGroup = group, Size = 4, MonoColor = Colors.White };
            _filePath = null;
            _groups.Clear();
            _frames.Clear();
            _undo.Reset();
        }

        public bool Open(string path)
        {
            var animation = FileReaderWriter.Open(path);
            if (animation != null)
            {
                Animation = animation;
                _filePath = path;
                _groups.Clear();
                _frames.Clear();
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

        public void StartGroupChange()
        {
            if (!_groupChange)
            {
                _undo.Group();
                _groupChange = true;
            }
        }

        public void EndGroupChange()
        {
            _undo.FinishAction();
            _groupChange = false;
        }

        public void SetAnimationProperty(string name, object newValue) => _undo.Set(Animation, name, newValue, _groupChange);

        public void SetTileProperty(Tile tile, string name, object newValue) => _undo.Set(tile, name, newValue, _groupChange);

        public void AddTile(Group group, Tile newTile)
        {
            _groups[group.Children] = group;
            _undo.Add(group.Children, newTile, _groupChange);
        }

        public void RemoveTile(Group group, Tile oldTile)
        {
            _groups[group.Children] = group;
            if(oldTile is Group g)
            {
                _groups.Remove(g.Children);
            }
            else if (oldTile is Frame f)
            {
                _frames.Remove(f.Voxels);
            }
            _undo.Remove(group.Children, oldTile, _groupChange);
        }

        public void SetVoxel(Frame frame, Color newColor, params int[] indices)
        {
            _frames[frame.Voxels] = frame;
            _undo.ChangeArray(frame.Voxels, newColor, true, indices);
        }

        public event EventHandler<PropertiesChangedEventArgs> PropertiesChanged;

        private void Undo_ActionExecuted(object sender, ActionExecutedEventArgs e)
        {
            var actions = (e.Action as ActionGroup)?.Actions ?? new[] { e.Action };
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
                    return _groups[collectionChange.Collection];
                case ArrayChangeAction<Color> arrayChange:
                    return _frames[arrayChange.Array];
                default:
                    throw new Exception(); // ToDo
            }
        }

        private string GetChangedProperty(IAction action)
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
    }
}
