using LedCubeAnimator.Model.Animations.Data;
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

        void SetAnimationProperties(int size, ColorMode colorMode, Color monoColor, int frameDuration);
        void SetTileProperty(Tile tile, string name, object newValue);
        void AddTile(Group group, Tile newTile);
        void RemoveTile(Group group, Tile oldTile);
        void SetVoxel(Frame frame, Color newColor, params int[] indices);

        event EventHandler AnimationChanged;
        event EventHandler<PropertiesChangedEventArgs> PropertiesChanged;
    }
}