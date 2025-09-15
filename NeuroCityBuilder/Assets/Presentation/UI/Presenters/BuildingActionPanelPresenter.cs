using Presentation.UI.Views;
using System;
using UnityEngine;

namespace Presentation.UI.Presenters
{
    public class BuildingActionPanelPresenter : PanelPresenterBase<BuildingActionPanelView>
    {
        public BuildingActionPanelPresenter(BuildingActionPanelView view)
        {
            this.view = view;
        }

        public override void Initialize()
        {
            this.view.onMoveClicked += this.HandleMoveClicked;
            this.view.onUpgradeClicked += this.HandleUpgradeClicked;
            this.view.onDeleteClicked += this.HandleDeleteClicked;

            //this.view.HidePanel();
        }

        private void HandleDeleteClicked()
        {
            throw new NotImplementedException();
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

