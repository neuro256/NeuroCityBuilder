using MessagePipe;
using NeuroCityBuilder.Application;
using NeuroCityBuilder.Application.Interfaces;
using NeuroCityBuilder.Application.Services;
using NeuroCityBuilder.Domain.Messages;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace NeuroCityBuilder.Infrastructure.Installers
{
    public class CoreLifetimeScope : LifetimeScope
    {
        [SerializeField] private GridConfig _gridConfig;
        [SerializeField] private BuildingLevelsConfig _buildingLevelsConfig;

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

            builder.RegisterComponentInHierarchy<GridPrefabProvider>().As<IGridPrefabProvider>();
            builder.RegisterComponentInHierarchy<BuildingPrefabProvider>().As<IBuildingPrefabProvider>();

            // Фабрики и утилиты
            builder.Register<IBuildingFactory, BuildingFactory>(Lifetime.Singleton);
            builder.Register<IGridCellFactory, GridCellFactory>(Lifetime.Singleton);
            builder.Register<IObjectDestroyer, ObjectDestroyer>(Lifetime.Singleton);

            //Сервисы
            builder.Register<GridManager>(Lifetime.Singleton).AsSelf();
            builder.Register<IBuildingService, BuildingService>(Lifetime.Singleton);
            builder.Register<IResourceService, ResourceService>(Lifetime.Singleton);
            builder.Register<IIncomeService, IncomeService>(Lifetime.Singleton);
            builder.Register<IGameSaveService, GameSaveService>(Lifetime.Singleton);

            // Конфиги
            builder.RegisterInstance(this._buildingLevelsConfig).As<IBuildingLevelProvider>();
            builder.RegisterInstance(this._gridConfig).As<IGridConfig>();
        }
    }
}

