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
        private string _filePath;
        private bool _mergeAllowed = true;
        private bool _canMergeNext;
        private bool _groupAction;

        public Animation Animation { get; private set; }

        public void New()
        {
            Animation = new Animation { Name = "Animation", Size = 4, MonoColor = Colors.White, FrameDuration = 1 };
            _filePath = null;
            _undo.Reset();
            _canMergeNext = false;
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
                _canMergeNext = false;
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

        public void Export(string path) => Exporter.Export(path, Animation);

        public void ExportMW(string path) => Exporter.ExportMW(path, Animation);

        public Color[,,] Render(int time) => Renderer.Render(Animation, time, true);

        public bool CanUndo => _undo.CanUndo;

        public bool CanRedo => _undo.CanRedo;

        public void Undo()
        {
            _undo.Undo();
            _canMergeNext = false;
        }

        public void Redo()
        {
            _undo.Redo();
            _canMergeNext = false;
        }

        public bool MergeAllowed
        {
            get => _mergeAllowed;
            set
            {
                _mergeAllowed = value;
                if (!_mergeAllowed)
                {
                    _canMergeNext = false;
                }
            }
        }

        public void Group(Action action)
        {
            if (_groupAction)
            {
                action();
            }
            else
            {
                _groupAction = true;
                _undo.Group(action, _canMergeNext);
                _groupAction = false;
                if (_mergeAllowed)
                {
                    _canMergeNext = true;
                }
            }
        }

        public void SetTileProperty(Tile tile, string property, object newValue) => Group(() => _undo.Set(tile, property, CoerceValue(tile, property, newValue)));

        public void AddTile(Group group, Tile newTile) => Group(() => _undo.Add(group, nameof(Animations.Data.Group.Children), group.Children, newTile));

        public void RemoveTile(Group group, Tile oldTile) => Group(() => _undo.Remove(group, nameof(Animations.Data.Group.Children), group.Children, oldTile));

        public void SetVoxel(Frame frame, Color newColor, params int[] indices) => Group(() => _undo.ChangeArray(frame, nameof(Frame.Voxels), frame.Voxels, newColor, indices));

        public event EventHandler<PropertiesChangedEventArgs> PropertiesChanged;

        public event EventHandler AnimationChanged;

        private List<KeyValuePair<object, string>> _lastChanges;

        private void Undo_ActionExecuted(object sender, ActionExecutedEventArgs e)
        {
            var actions = ((GroupAction)e.Action).Actions.Cast<ObjectAction>();
            var changes = actions.Select(a => new KeyValuePair<object, string>(a.Object, a.Property)); // ToDo: perform correction before finishing group
            if (_lastChanges == null)
            {
                var list = changes.ToList();
                _lastChanges = list;
                CorrectData(list);
                _lastChanges = null;
                PropertiesChanged?.Invoke(this, new PropertiesChangedEventArgs(list));
            }
            else
            {
                _lastChanges.AddRange(changes);
            }
        }

        private static object CoerceValue(Tile tile, string property, object newValue)
        {
            if ((property == nameof(Tile.Start) || property == nameof(Tile.End) || property == nameof(Tile.Channel) || property == nameof(Tile.Hierarchy))
                && (int)newValue < 0)
            {
                newValue = 0;
            }
            else if (tile is Frame frame
                && property == nameof(Frame.Voxels))
            {
                CopyFrameVoxels(frame, (Color[,,])newValue);
            }
            else if (tile is Effect
                && property == nameof(Effect.RepeatCount)
                && (int)newValue < 1)
            {
                newValue = 1;
            }
            else if (tile is Animation
                && (property == nameof(Animations.Data.Animation.Size) || property == nameof(Animations.Data.Animation.FrameDuration))
                && (int)newValue < 1)
            {
                newValue = 1;
            }

            return newValue;
        }

        private static void CopyFrameVoxels(Frame frame, Color[,,] voxels)
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

        private void CorrectData(List<KeyValuePair<object, string>> changes)
        {
            var movedTiles = new HashSet<Tile>();
            int changeCount = changes.Count;
            for (int i = 0; i < changeCount; i++)
            {
                var tile = (Tile)changes[i].Key;
                string property = changes[i].Value;
                if (property == nameof(Tile.Start) || property == nameof(Tile.End) || property == nameof(Tile.Channel) || property == nameof(Tile.Hierarchy))
                {
                    movedTiles.Add(tile);
                }
                else if (tile == Animation && property == nameof(Animations.Data.Animation.ColorMode) && Animation.ColorMode != ColorMode.RGB)
                {
                    CorrectFrameVoxels(Animation);
                }
            }
            if (movedTiles.Count > 0)
            {
                CorrectGroupChildren(Animation, movedTiles);
            }
        }

        private void CorrectGroupChildren(Group group, HashSet<Tile> movedTiles)
        {
            bool moved = false;
            foreach (var c in group.Children)
            {
                if (c is Group g)
                {
                    CorrectGroupChildren(g, movedTiles);
                }
                if (movedTiles.Contains(c))
                {
                    moved = true;
                }
            }
            if (moved)
            {
                foreach (var channel in group.Children.GroupBy(c => c.Channel))
                {
                    int maxHierarchy = channel.Max(c => c.Hierarchy);
                    var levels = Enumerable.Range(0, maxHierarchy + 1)
                        .Select(h => channel.Where(c => c.Hierarchy == h).OrderBy(t => movedTiles.Contains(t)).ToList())
                        .ToList();
                    for (int i = 0; i < levels.Count; i++)
                    {
                        for (int x = 0; x < levels[i].Count; x++)
                        {
                            var tile = levels[i][x];
                            int j = i;
                            if (levels[j].All(t => t == tile || movedTiles.Contains(t) || t.End < tile.Start || t.Start > tile.End))
                            {
                                while (j > 0 && levels[j - 1].All(t => t == tile || t.End < tile.Start || t.Start > tile.End))
                                {
                                    j--;
                                }
                                if (j == i && !levels[j].All(t => t == tile || t.End < tile.Start || t.Start > tile.End))
                                {
                                    j++;
                                }
                            }
                            else
                            {
                                j++;
                            }
                            if (j != i)
                            {
                                levels[i].RemoveAt(x);
                                x--;
                                if (j == levels.Count)
                                {
                                    levels.Add(new List<Tile>());
                                }
                                int y = levels[j].Count;
                                while (y > 0 && movedTiles.Contains(levels[j][y - 1]))
                                {
                                    y--;
                                }
                                levels[j].Insert(y, tile);
                                _undo.Group(() => _undo.Set(tile, nameof(Tile.Hierarchy), j), true);
                            }
                        }
                    }
                }
            }
        }

        private void CorrectFrameVoxels(Group group)
        {
            _undo.Group(() =>
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
                                        var oldColor = f.Voxels[x, y, z];
                                        switch (Animation.ColorMode)
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
                            CorrectFrameVoxels(g);
                            break;
                    }
                }
            }, true);
        }
    }
}
