using Domain.Messages;
using Infrastructure.Camera;
using MessagePipe;
using Presentation.System;
using Presentation.Systems;
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
            builder.RegisterMessageBroker<BuildingMoveRequestMessage>(options);
            builder.RegisterMessageBroker<BuildingMoveStartMessage>(options);
            builder.RegisterMessageBroker<BuildingMoveCompleteMessage>(options);
            builder.RegisterMessageBroker<BuildingMoveCancelMessage>(options);
            builder.RegisterMessageBroker<GoldAddedMessage>(options);   

            //Сервисы
            builder.Register<IBuildingService, BuildingService>(Lifetime.Singleton);
            builder.Register<IResourceService, ResourceService>(Lifetime.Singleton);
            builder.Register<IIncomeService, IncomeService>(Lifetime.Singleton);

            //Компоненты сцены
            //builder.RegisterComponentInHierarchy<SaveLoadPanelView>().AsSelf();
            builder.RegisterComponentInHierarchy<BuildingFactory>().AsSelf();
            builder.RegisterComponentInHierarchy<GridManager>().AsSelf();
            builder.RegisterComponentInHierarchy<GridView>().AsSelf();
            builder.RegisterComponentInHierarchy<CameraController>().AsSelf();
            builder.RegisterComponentInHierarchy<BuildingSystemsRunner>().AsSelf();
            builder.RegisterComponentInHierarchy<BuildPanelView>().As<IBuildPanelView>();
            builder.RegisterComponentInHierarchy<BuildingActionPanelView>().As<IBuildingActionPanelView>();
            builder.RegisterComponentInHierarchy<BuildingInfoPanelView>().As<IBuildingInfoPanelView>();
            builder.RegisterComponentInHierarchy<ResourcePanelView>().As<IResourcePanelView>();

            //Презентеры и системы
            builder.Register<BuildPanelPresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<BuildingActionPanelPresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<BuildingInteractionSystem>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<BuildingInfoPanelPresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<BuildingMoveSystem>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<ResourcePanelPresenter>(Lifetime.Singleton).AsImplementedInterfaces();

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
