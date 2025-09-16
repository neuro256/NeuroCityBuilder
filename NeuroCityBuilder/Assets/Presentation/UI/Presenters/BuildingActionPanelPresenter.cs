using Domain.Gameplay;
using Presentation.UI.Views;
using System;
using UnityEngine;
using UseCases.Services;

namespace Presentation.UI.Presenters
{
    public class BuildingActionPanelPresenter : PanelPresenterBase<BuildingActionPanelView>
    {
        private readonly IBuildingService _buildingService;
        private Building _selectedBuilding;

        public BuildingActionPanelPresenter(BuildingActionPanelView view) : base(view) { }  

        public override void Initialize()
        {
            this.view.onMoveClicked += this.HandleMoveClicked;
            this.view.onUpgradeClicked += this.HandleUpgradeClicked;
            this.view.onDeleteClicked += this.HandleDeleteClicked;

            //this.view.HidePanel();
        }

        private void HandleDeleteClicked()
        {
            
        }

        private void HandleUpgradeClicked()
        {
            throw new NotImplementedException();
        }

        private void HandleMoveClicked()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            this.view.onMoveClicked -= this.HandleMoveClicked;
            this.view.onUpgradeClicked -= this.HandleUpgradeClicked;
            this.view.onDeleteClicked -= this.HandleDeleteClicked;
        }
    }
}

