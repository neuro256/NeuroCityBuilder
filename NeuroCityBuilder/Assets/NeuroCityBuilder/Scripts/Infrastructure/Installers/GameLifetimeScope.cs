using NeuroCityBuilder.Infrastructure.Camera;
using NeuroCityBuilder.Presentation.System;
using NeuroCityBuilder.Presentation.UI;
using NeuroCityBuilder.Presentation.UI.Presenters;
using NeuroCityBuilder.Presentation.UI.Views;
using VContainer;
using VContainer.Unity;

namespace NeuroCityBuilder.Infrastructure.Installers
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            //Компоненты сцены
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
            builder.Register<GridPresenter>(Lifetime.Singleton)
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
