using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("CustomTile", 1)]
    public class CustomTileViewModel : TileViewModel
    {
        public CustomTileViewModel(CustomTile customTile, IModelManager model, GroupViewModel parent) : base(customTile, model, parent) { }

        [Browsable(false)]
        public CustomTile CustomTile => (CustomTile)Tile;

        [Category("CustomTile")]
        [PropertyOrder(0)]
        public string Expression
        {
            get => CustomTile.Expression;
            set => Model.SetTileProperty(CustomTile, nameof(CustomTile.Expression), value);
        }

        public override void ModelPropertyChanged(object obj, string propertyName, out TileViewModel changedViewModel, out string changedProperty)
        {
            base.ModelPropertyChanged(obj, propertyName, out changedViewModel, out changedProperty);
            if (obj == CustomTile)
            {
                switch (propertyName)
                {
                    case nameof(CustomTile.Expression):
                        changedProperty = nameof(Expression);
                        break;
                    default:
                        return;
                }
                changedViewModel = this;
                RaisePropertyChanged(changedProperty);
            }
        }
    }
}
