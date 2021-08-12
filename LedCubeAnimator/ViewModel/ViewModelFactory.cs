using GalaSoft.MvvmLight.Messaging;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using LedCubeAnimator.ViewModel.DataViewModels;
using System;
using System.ComponentModel;

namespace LedCubeAnimator.ViewModel
{
    public class ViewModelFactory : IViewModelFactory
    {
        public ViewModelFactory(IModelManager manager, IMessenger messenger)
        {
            _manager = manager;
            _messenger = messenger;
        }

        private readonly IModelManager _manager;
        private readonly IMessenger _messenger;
        
        public INotifyPropertyChanged Create(object model, params object[] args)
        {
            switch (model)
            {
                case Animation animation:
                    return new AnimationViewModel(animation, _manager, _messenger, this);
                case Frame frame:
                    return new FrameViewModel(frame, _manager, _messenger, (GroupViewModel)args[0]);
                case Group group:
                    return new GroupViewModel(group, _manager, _messenger, (GroupViewModel)args[0], this);
                case GradientEffect gradientEffect:
                    return new GradientEffectViewModel(gradientEffect, _manager, _messenger, (GroupViewModel)args[0]);
                case MoveEffect moveEffect:
                    return new MoveEffectViewModel(moveEffect, _manager, _messenger, (GroupViewModel)args[0]);
                case RotateEffect rotateEffect:
                    return new RotateEffectViewModel(rotateEffect, _manager, _messenger, (GroupViewModel)args[0]);
                case ScaleEffect scaleEffect:
                    return new ScaleEffectViewModel(scaleEffect, _manager, _messenger, (GroupViewModel)args[0]);
                case ShearEffect shearEffect:
                    return new ShearEffectViewModel(shearEffect, _manager, _messenger, (GroupViewModel)args[0]);
                case LinearDelay linearDelay:
                    return new LinearDelayViewModel(linearDelay, _manager, _messenger, (GroupViewModel)args[0]);
                case RadialDelay radialDelay:
                    return new RadialDelayViewModel(radialDelay, _manager, _messenger, (GroupViewModel)args[0]);
                case SphericalDelay sphericalDelay:
                    return new SphericalDelayViewModel(sphericalDelay, _manager, _messenger, (GroupViewModel)args[0]);
                case AngularDelay angularDelay:
                    return new AngularDelayViewModel(angularDelay, _manager, _messenger, (GroupViewModel)args[0]);
                default:
                    throw new ArgumentException("Could not find suitable viewModel for this type of model", "model");
            }
        }
    }
}
