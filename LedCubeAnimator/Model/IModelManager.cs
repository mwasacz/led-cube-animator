using LedCubeAnimator.Model.Animations.Data;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace LedCubeAnimator.Model
{
    public interface IModelManager
    {
        Animation Animation { get; }

        void New();
        bool Open(string path);
        bool Save();
        void SaveAs(string path);
        string Copy(ICollection<Tile> tiles);
        bool Paste(Group group, string str, bool mergeAllowed);
        void Export(string path);
        void ExportMW(string path);

        Color[,,] Render(int time);

        bool CanUndo { get; }
        bool CanRedo { get; }
        void Undo();
        void Redo();

        bool MergeAllowed { get; set; }
        void Group(Action action);
        void SetTileProperty(Tile tile, string name, object newValue);
        void AddTile(Group group, Tile newTile);
        void RemoveTile(Group group, Tile oldTile);
        void SetVoxel(Frame frame, Color newColor, params int[] indices);

        event EventHandler AnimationChanged;
        event EventHandler<PropertiesChangedEventArgs> PropertiesChanged;
    }
}