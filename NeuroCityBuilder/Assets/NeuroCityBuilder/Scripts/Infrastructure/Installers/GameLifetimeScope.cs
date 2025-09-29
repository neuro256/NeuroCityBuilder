using NeuroCityBuilder.Domain.Messages;
using MessagePipe;
using NeuroCityBuilder.Presentation.System;
using NeuroCityBuilder.Presentation.UI;
using NeuroCityBuilder.Presentation.UI.Presenters;
using NeuroCityBuilder.Presentation.UI.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using NeuroCityBuilder.Application.Services;
using NeuroCityBuilder.Application;
using NeuroCityBuilder.Infrastructure.Camera;

namespace NeuroCityBuilder.Infrastructure.Installers
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private int _gridWidth = 32;
        [SerializeField] private int _gridHeight = 32;

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
            builder.Register<IGameSaveService, GameSaveService>(Lifetime.Singleton);
            builder.Register(resolver =>
            {
                return new GridManager(this._gridWidth, this._gridHeight);
            }, Lifetime.Singleton).AsSelf();

            //Компоненты сцены
            builder.RegisterComponentInHierarchy<BuildingFactory>().AsSelf();
            builder.RegisterComponentInHierarchy<GridView>().AsSelf();
            builder.RegisterComponentInHierarchy<CameraController>().AsSelf();
            builder.RegisterComponentInHierarchy<BuildingSystemsRunner>().AsSelf();
            builder.RegisterComponentInHierarchy<BuildPanelView>().As<IBuildPanelView>();
            builder.RegisterComponentInHierarchy<BuildingActionPanelView>().As<IBuildingActionPanelView>();
            builder.RegisterComponentInHierarchy<BuildingInfoPanelView>().As<IBuildingInfoPanelView>();
            builder.RegisterComponentInHierarchy<ResourcePanelView>().As<IResourcePanelView>();
            builder.RegisterComponentInHierarchy<SaveLoadPanelView>().As<ISaveLoadPanelView>();
            builder.RegisterComponentInHierarchy<GridView>().As<IGridView>();

            //Презентеры и системы
            builder.Register<BuildPanelPresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<BuildingActionPanelPresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<BuildingInteractionSystem>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<BuildingInfoPanelPresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<BuildingMoveSystem>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<ResourcePanelPresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SaveLoadPanelPresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register(resolver =>
            {
                IGridView view = resolver.Resolve<GridView>();
                return new GridPresenter(view, this._gridWidth, this._gridHeight);
            }, Lifetime.Singleton)
                .As<IGridPresenter>()
                .As<IInitializable>()
                .AsSelf();

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
