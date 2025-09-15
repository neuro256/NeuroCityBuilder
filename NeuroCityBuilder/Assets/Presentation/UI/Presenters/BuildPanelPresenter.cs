using Domain.Gameplay;
using Presentation.UI.Views;
using System;
using UnityEngine;
using VContainer.Unity;

namespace Presentation.UI.Presenters
{
    public class BuildPanelPresenter : PanelPresenterBase<BuildPanelView>
    {
        public BuildPanelPresenter(BuildPanelView view)
        {
            this.view = view;
        }

        public override void Initialize()
        {
            this.view.onBuildSelected += (buildType) => this.HandleBuildingSelection(buildType);
        }

        private void HandleBuildingSelection(BuildingType buildType)
        {
            Debug.Log($"HandleBuildingSelection: {buildType}");
        }

        public override void Dispose()
        {
            this.view.onBuildSelected -= (buildType) => this.HandleBuildingSelection(buildType);
        }
    }
}

