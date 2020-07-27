using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using LedCubeAnimator.ViewModel.DataViewModels;
using System;
using System.ComponentModel;

namespace LedCubeAnimator.ViewModel
{
    public class ViewModelFactory : IViewModelFactory
    {
        public ViewModelFactory(IModelManager manager)
        {
            _manager = manager;
        }

        private readonly IModelManager _manager;
        
        public INotifyPropertyChanged Create(object model)
        {
            switch (model)
            {
                case Frame frame:
                    return new FrameViewModel(frame, _manager);
                case Group group:
                    return new GroupViewModel(group, _manager);
                case GradientEffect gradientEffect:
                    return new GradientEffectViewModel(gradientEffect, _manager);
                case MoveEffect moveEffect:
                    return new MoveEffectViewModel(moveEffect, _manager);
                case RotateEffect rotateEffect:
                    return new RotateEffectViewModel(rotateEffect, _manager);
                case ScaleEffect scaleEffect:
                    return new ScaleEffectViewModel(scaleEffect, _manager);
                case ShearEffect shearEffect:
                    return new ShearEffectViewModel(shearEffect, _manager);
                case LinearDelay linearDelay:
                    return new LinearDelayViewModel(linearDelay, _manager);
                case RadialDelay radialDelay:
                    return new RadialDelayViewModel(radialDelay, _manager);
                case SphericalDelay sphericalDelay:
                    return new SphericalDelayViewModel(sphericalDelay, _manager);
                case AngularDelay angularDelay:
                    return new AngularDelayViewModel(angularDelay, _manager);
                default:
                    throw new ArgumentException("Could not find suitable viewModel for this type of model", "model");
            }
        }
    }
}
