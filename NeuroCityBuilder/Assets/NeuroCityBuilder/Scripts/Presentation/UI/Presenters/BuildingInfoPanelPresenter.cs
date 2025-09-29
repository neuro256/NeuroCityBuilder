using MessagePipe;
using NeuroCityBuilder.Domain.Gameplay;
using NeuroCityBuilder.Domain.Messages;
using NeuroCityBuilder.Presentation.UI.Views;
using System;
using UnityEngine;

namespace NeuroCityBuilder.Presentation.UI.Presenters
{
    public class BuildingInfoPanelPresenter : PanelPresenterBase<IBuildingInfoPanelView>
    {
        private readonly ISubscriber<BuildingSelectedMessage> _buildingSelectedSubscriber;
        private readonly ISubscriber<BuildingDeselectedMessage> _buildingDeselectedSubscriber;
        private readonly ISubscriber<BuildingDeletedMessage> _buildingDeletedSubscriber;
        private readonly ISubscriber<BuildingUpgradedMessage> _buildingUpgradedSubscriber;

        private IDisposable _subscription;

        public BuildingInfoPanelPresenter(IBuildingInfoPanelView view,
            ISubscriber<BuildingSelectedMessage> buildingSelectedSubscriber,
            ISubscriber<BuildingDeselectedMessage> buildingDeselectedSubscriber,
            ISubscriber<BuildingDeletedMessage> buildingDeletedSubscriber,
            ISubscriber<BuildingUpgradedMessage> buildingUpgradedSubscriber) : base(view)
        {
            this._buildingSelectedSubscriber = buildingSelectedSubscriber;
            this._buildingDeselectedSubscriber = buildingDeselectedSubscriber;
            this._buildingDeletedSubscriber = buildingDeletedSubscriber;
            this._buildingUpgradedSubscriber = buildingUpgradedSubscriber;
        }

        public override void Initialize()
        {
            DisposableBagBuilder bag = DisposableBag.CreateBuilder();
            this._buildingSelectedSubscriber.Subscribe(this.OnBuildingSelected).AddTo(bag);
            this._buildingDeselectedSubscriber.Subscribe(this.OnBuildingDeselected).AddTo(bag);
            this._buildingDeletedSubscriber.Subscribe(this.OnBuildingDeleted).AddTo(bag);
            this._buildingUpgradedSubscriber.Subscribe(this.OnBuildingUpgraded).AddTo(bag);
            this._subscription = bag.Build();
        }

        private void OnBuildingSelected(BuildingSelectedMessage message)
        {
            Debug.Log($"BuildingInfoPanelPresenter: Building selected - {message.Building.Type}");
            this.ShowBuildingInfo(message.Building);
        }

        private void OnBuildingDeselected(BuildingDeselectedMessage message)
        {
            Debug.Log("BuildingInfoPanelPresenter: Building deselected");
            this.view.HideInfo();
        }

        private void OnBuildingDeleted(BuildingDeletedMessage message)
        {
            Debug.Log("BuildingInfoPanelPresenter: Building deleted");
            this.view.HideInfo();
        }

        private void OnBuildingUpgraded(BuildingUpgradedMessage message)
        {
            Debug.Log($"BuildingInfoPanelPresenter: Building upgraded type: {message.Building.Type} level {message.Building.CurrentLevel}");
            this.ShowBuildingInfo(message.Building);
        }

        private void ShowBuildingInfo(Building building)
        {
            if (building != null && building.Levels != null && building.Levels.Length > 0)
            {
                BuildingLevel currentLevel = building.GetCurrentLevelInfo();
                if (currentLevel != null)
                {
                    this.view.ShowInfo(building.CurrentLevel + 1, currentLevel.Income);
                    Debug.Log($"BuildingInfoPanelPresenter: Showing info - Level: {building.CurrentLevel}, Income: {currentLevel.Income}");
                }
                else
                {
                    this.view.HideInfo();
                }
            }
            else
            {
                this.view.HideInfo();
            }
        }

        public override void Dispose()
        {
            this._subscription?.Dispose();
        }
    }
}

