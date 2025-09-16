using Domain.Gameplay;
using Domain.Messages;
using MessagePipe;
using Presentation.UI.Views;
using System;
using UnityEngine;
using UseCases.Services;

namespace Presentation.UI.Presenters
{
    public class BuildingActionPanelPresenter : PanelPresenterBase<IBuildingActionPanelView>
    {
        private readonly ISubscriber<BuildingSelectedMessage> _buildingSelectedSubscriber;
        private readonly ISubscriber<BuildingDeselectedMessage> _buildingDeselectedSubscriber;
        private readonly IPublisher<BuildingDeleteRequestMessage> _buildingDeleteRequestPublisher;
        private readonly IPublisher<BuildingUpgradeRequestMessage> _buildingUpgradeRequestPublisher;
        private readonly IPublisher<BuildingMoveRequestMessage> _buildingMoveRequestPublisher;

        private Building _selectedBuilding;
        private IDisposable _subscription;

        public BuildingActionPanelPresenter(IBuildingActionPanelView view, 
            ISubscriber<BuildingSelectedMessage> buildingSelectedSubscriber,
            ISubscriber<BuildingDeselectedMessage> buildingDeselectedSubscriber,
            IPublisher<BuildingDeleteRequestMessage> buildingDeleteRequestPublisher,
            IPublisher<BuildingUpgradeRequestMessage> buildingUpgradeRequestPublisher,
            IPublisher<BuildingMoveRequestMessage> buildingMoveRequestPublisher) : base(view) 
        {
            this._buildingSelectedSubscriber = buildingSelectedSubscriber;
            this._buildingDeselectedSubscriber = buildingDeselectedSubscriber;
            this._buildingDeleteRequestPublisher = buildingDeleteRequestPublisher;
            this._buildingUpgradeRequestPublisher = buildingUpgradeRequestPublisher;
            this._buildingMoveRequestPublisher = buildingMoveRequestPublisher;
        }  

        public override void Initialize()
        {
            this.view.onMoveClicked += this.HandleMoveClicked;
            this.view.onUpgradeClicked += this.HandleUpgradeClicked;
            this.view.onDeleteClicked += this.HandleDeleteClicked;

            DisposableBagBuilder bag = DisposableBag.CreateBuilder();
            this._buildingSelectedSubscriber.Subscribe(this.OnBuildingSelected).AddTo(bag);
            this._buildingDeselectedSubscriber.Subscribe(this.OnBuildingDeselected).AddTo(bag);
            this._subscription = bag.Build();
        }

        private void OnBuildingSelected(BuildingSelectedMessage message)
        {
            Debug.Log($"BuildingActionPanelPresenter: Building selected - {message.Building.Type}");
            this._selectedBuilding = message.Building;
            this.view.ShowPanel();
            this.UpdateUpgradeButtonState(message.Building);
        }

        private void OnBuildingDeselected(BuildingDeselectedMessage message)
        {
            Debug.Log("BuildingActionPanelPresenter: Building deselected");
            this._selectedBuilding = null;
            this.view.HidePanel();
        }

        private void HandleDeleteClicked()
        {
            Debug.Log("BuildingActionPanelPresenter: HandleDeleteClicked");

            if (this._selectedBuilding != null)
            {
                this._buildingDeleteRequestPublisher.Publish(new BuildingDeleteRequestMessage
                {
                    Building = this._selectedBuilding
                });
            }

            this.view.HidePanel();
        }

        private void HandleUpgradeClicked()
        {
            Debug.Log("BuildingActionPanelPresenter: HandleUpgradeClicked");

            if (this._selectedBuilding != null)
            {
                if (this._selectedBuilding.CurrentLevel < this._selectedBuilding.Levels.Length - 1)
                {
                    this._buildingUpgradeRequestPublisher.Publish(new BuildingUpgradeRequestMessage
                    {
                        Building = this._selectedBuilding
                    });
                }
                else
                {
                    this.view.EnableUpgradeButton(false);
                }
            }
        }

        private void HandleMoveClicked()
        {
            Debug.Log("BuildingActionPanelPresenter: HandleMoveClicked");

            if (this._selectedBuilding != null)
            {
                this._buildingMoveRequestPublisher.Publish(new BuildingMoveRequestMessage
                {
                    Building = this._selectedBuilding
                });

                this.view.HidePanel();
            }
        }

        private void UpdateUpgradeButtonState(Building building)
        {
            if (building != null)
            {
                bool canUpgrade = building.CurrentLevel < building.Levels.Length - 1;
                this.view.EnableUpgradeButton(canUpgrade);
            }
            else
            {
                this.view.EnableUpgradeButton(false);
            }
        }

        public override void Dispose()
        {
            this.view.onMoveClicked -= this.HandleMoveClicked;
            this.view.onUpgradeClicked -= this.HandleUpgradeClicked;
            this.view.onDeleteClicked -= this.HandleDeleteClicked;

            this._subscription?.Dispose();
        }
    }
}

