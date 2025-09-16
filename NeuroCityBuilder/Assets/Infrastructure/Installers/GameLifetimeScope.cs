using Domain.Messages;
using Infrastructure.Camera;
using MessagePipe;
using Presentation;
using Presentation.UI;
using Presentation.UI.Presenters;
using Presentation.UI.Views;
using UnityEngine;
using UseCases;
using UseCases.Services;
using VContainer;
using VContainer.Unity;
//using Presentation.UI.Views;

namespace Infrastructure.Installers
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            MessagePipeOptions options = builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<BuildingTypeSelectedMessage>(options);
            builder.RegisterMessageBroker<BuildingPlacedMessage>(options);
            builder.RegisterMessageBroker<BuildingDeletedMessage>(options);
            builder.RegisterMessageBroker<BuildingDeselectedMessage>(options);
            builder.RegisterMessageBroker<BuildingDeleteRequestMessage>(options);
            builder.RegisterMessageBroker<BuildingSelectedMessage>(options);
            builder.RegisterMessageBroker<BuildingUpgradedMessage>(options);

            //Сервисы
            builder.Register<IBuildingService, BuildingService>(Lifetime.Singleton);

            //Компоненты сцены
            //builder.RegisterComponentInHierarchy<SaveLoadPanelView>().AsSelf();
            //builder.Register<SaveLoadPanelPresenter>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<BuildingFactory>().AsSelf();
            builder.RegisterComponentInHierarchy<GridManager>().AsSelf();
            builder.RegisterComponentInHierarchy<GridView>().AsSelf();
            builder.RegisterComponentInHierarchy<BuildPanelView>().AsSelf();
            builder.RegisterComponentInHierarchy<BuildingActionPanelView>().AsSelf();
            builder.RegisterComponentInHierarchy<BuildingInfoPanelView>().AsSelf();
            builder.RegisterComponentInHierarchy<CameraController>().AsSelf();
            builder.RegisterComponentInHierarchy<BuildingPlacementRunner>().AsSelf();

            //Презентеры и системы
            builder.Register<BuildPanelPresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<BuildingActionPanelPresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<BuildingInteractionSystem>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<BuildingInfoPanelPresenter>(Lifetime.Singleton).AsImplementedInterfaces();

            //Input
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
