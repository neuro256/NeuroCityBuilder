using VContainer;
using VContainer.Unity;
using MessagePipe;
using UnityEngine;
using UseCases.Services;
using Infrastructure.Camera;
//using Presentation.UI.Views;

namespace Infrastructure.Installers
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe();

            builder.Register<IBuildingService, BuildingService>(Lifetime.Singleton);

            //builder.RegisterComponentInHierarchy<SaveLoadPanelView>().AsSelf();

            //builder.Register<SaveLoadPanelPresenter>(Lifetime.Singleton);

            builder.RegisterComponentInHierarchy<CameraController>().AsSelf();

            builder.Register(resolver =>
            {
                PlayerInputActions controls = new PlayerInputActions();
                controls.Enable();
                return controls;
            }, Lifetime.Singleton);

            builder.Register<CameraInputHandler>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.RegisterEntryPoint<GameEntryPoint>(Lifetime.Singleton);
        }
    }
}
