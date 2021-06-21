using LedCubeAnimator.Model.Animations;
using LedCubeAnimator.Model.Animations.Data;
using LedCubeAnimator.Model.Undo;
using LedCubeAnimator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Animation = new Animation { Name = "Animation", Size = 4, MonoColor = Colors.White, FrameDuration = 1 };
            _filePath = null;
            _undo.Reset();
            _changedObject = null;
            _changedProperty = null;
            AnimationChanged?.Invoke(this, EventArgs.Empty);
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
                AnimationChanged?.Invoke(this, EventArgs.Empty);
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

        public void ExportMW(string path)
        {
            Exporter.ExportMW(path, Animation);
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

        public void SetTileProperty(Tile tile, string name, object newValue)
        {
            _undo.Group(() =>
            {
                if (tile is Frame frame && name == nameof(Frame.Voxels))
                {
                    SetFrameVoxels(frame, (Color[,,])newValue);
                }
                _undo.Set(tile, name, newValue);
                if (tile == Animation && name == nameof(Animation.ColorMode) && (ColorMode)newValue != ColorMode.RGB)
                {
                    SetGroupColorMode(Animation, (ColorMode)newValue);
                }
            }, _changedObject == tile && _changedProperty == name);
            _changedObject = tile;
            _changedProperty = name;
        }

        public void AddTile(Group group, Tile newTile)
        {
            _undo.Group(() => _undo.Add(group, nameof(Group.Children), group.Children, newTile));
            _changedObject = group;
            _changedProperty = nameof(Group.Children);
        }

        public void RemoveTile(Group group, Tile oldTile)
        {
            _undo.Group(() => _undo.Remove(group, nameof(Group.Children), group.Children, oldTile));
            _changedObject = group;
            _changedProperty = nameof(Group.Children);
        }

        public void SetVoxel(Frame frame, Color newColor, params int[] indices)
        {
            _undo.Group(() => _undo.ChangeArray(frame, nameof(Frame.Voxels), frame.Voxels, newColor, indices),
                _changedObject == frame && _changedProperty == nameof(Frame.Voxels));
            _changedObject = frame;
            _changedProperty = nameof(Frame.Voxels);
        }

        public event EventHandler<PropertiesChangedEventArgs> PropertiesChanged;

        public event EventHandler AnimationChanged;

        private void Undo_ActionExecuted(object sender, ActionExecutedEventArgs e)
        {
            var groupAction = (GroupAction)e.Action;
            var changes = groupAction.Actions.Cast<ObjectAction>().Select(a => new KeyValuePair<object, string>(a.Object, a.Property)).ToArray();
            PropertiesChanged?.Invoke(this, new PropertiesChangedEventArgs(changes));
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
                                            _undo.ChangeArray(f, nameof(Frame.Voxels), f.Voxels, oldColor.GetBrightness() > 127 ? Colors.White : Colors.Black, x, y, z);
                                            break;
                                        case ColorMode.MonoBrightness:
                                            _undo.ChangeArray(f, nameof(Frame.Voxels), f.Voxels, Colors.White.Multiply(oldColor.GetBrightness()).Opaque(), x, y, z);
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

        private static void SetFrameVoxels(Frame frame, Color[,,] voxels)
        {
            for (int x = 0; x < voxels.GetLength(0); x++)
            {
                for (int y = 0; y < voxels.GetLength(1); y++)
                {
                    for (int z = 0; z < voxels.GetLength(2); z++)
                    {
                        voxels[x, y, z] = x < frame.Voxels.GetLength(0)
                            && y < frame.Voxels.GetLength(1)
                            && z < frame.Voxels.GetLength(2)
                                ? frame.Voxels[x, y, z]
                                : Colors.Black;
                    }
                }
            }
        }
    }
}
