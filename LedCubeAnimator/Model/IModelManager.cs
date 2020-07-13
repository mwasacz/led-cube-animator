using System;
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
        void Export(string path);

        Color[,,] Render(int time);

        bool CanUndo { get; }
        bool CanRedo { get; }
        void Undo();
        void Redo();

        void StartGroupChange();
        void EndGroupChange();

        void SetAnimationProperty(string name, object newValue);
        void SetTileProperty(Tile tile, string name, object newValue);
        void AddTile(Group group, Tile newTile);
        void RemoveTile(Group group, Tile oldTile);
        void SetVoxel(Frame frame, Color newColor, params int[] indices);

        event EventHandler<PropertiesChangedEventArgs> PropertiesChanged;
    }
}