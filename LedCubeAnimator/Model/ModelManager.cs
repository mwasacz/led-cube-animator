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

        public string Copy(ICollection<Tile> tiles) => FileReaderWriter.Serialize(tiles);

        public bool Paste(Group group, string str, bool mergeAllowed)
        {
            var tiles = FileReaderWriter.Deserialize(str)?.Where(t => !(t is Animation)).ToArray();
            if (tiles?.Length > 0)
            {
                var prevMergeAllowed = MergeAllowed;
                MergeAllowed = mergeAllowed;
                Group(() =>
                {
                    foreach (var tile in tiles)
                    {
                        AddTile(group, tile);
                    }
                });
                MergeAllowed = prevMergeAllowed;
                return true;
            }
            return false;
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
                if (MergeAllowed)
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

        private List<ObjectAction> _lastChanges;

        private void Undo_ActionExecuted(object sender, ActionExecutedEventArgs e)
        {
            var changes = ((GroupAction)e.Action).Actions.Cast<ObjectAction>(); // ToDo: perform correction before finishing group
            if (_lastChanges == null)
            {
                var list = changes.ToList();
                _lastChanges = list;
                CorrectData(list);
                _lastChanges = null;
                var args = new PropertiesChangedEventArgs(list.Select(a => new KeyValuePair<object, string>(a.Object, a.Property)).ToArray());
                PropertiesChanged?.Invoke(this, args);
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

        private void CorrectData(List<ObjectAction> changes)
        {
            var movedTiles = new HashSet<Tile>();
            foreach (var change in changes)
            {
                var tile = (Tile)change.Object;
                string prop = change.Property;
                if (change is PropertyChangeAction)
                {
                    if (prop == nameof(Tile.Start) || prop == nameof(Tile.End) || prop == nameof(Tile.Channel) || prop == nameof(Tile.Hierarchy))
                    {
                        movedTiles.Add(tile);
                    }
                    else if (tile == Animation && prop == nameof(Animations.Data.Animation.ColorMode) && Animation.ColorMode != ColorMode.RGB)
                    {
                        CorrectFrameVoxels(Animation);
                    }
                }
                else if (change is CollectionChangeAction<Tile> collectionChange && tile is Group group && prop == nameof(Animations.Data.Group.Children))
                {
                    if (collectionChange.Added)
                    {
                        movedTiles.Add(collectionChange.Item);
                    }
                    else
                    {
                        foreach (var t in group.Children)
                        {
                            movedTiles.Add(t);
                        }
                    }
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
                                            _undo.Group(() => _undo.ChangeArray(f, nameof(Frame.Voxels), f.Voxels, oldColor.GetBrightness() > 127 ? Colors.White : Colors.Black, x, y, z), true);
                                            break;
                                        case ColorMode.MonoBrightness:
                                            _undo.Group(() => _undo.ChangeArray(f, nameof(Frame.Voxels), f.Voxels, Colors.White.Multiply(oldColor.GetBrightness()).Opaque(), x, y, z), true);
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
        }
    }
}
