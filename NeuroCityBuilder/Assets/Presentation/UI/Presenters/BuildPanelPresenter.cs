using Domain.Messages;
using Domain.Gameplay;
using MessagePipe;
using Presentation.UI.Views;
using UnityEngine;

namespace Presentation.UI.Presenters
{
    public class BuildPanelPresenter : PanelPresenterBase<BuildPanelView>
    {
        private readonly IPublisher<BuildingSelectedMessage> _buildingSelectedPublisher;

        public BuildPanelPresenter(BuildPanelView view, IPublisher<BuildingSelectedMessage> buildingSelectedPublisher) : base(view)
        {
            this._buildingSelectedPublisher = buildingSelectedPublisher;
        }

        public override void Initialize()
        {
            this.view.onBuildSelected += (buildType) => this.HandleBuildingSelection(buildType);
        }

        private void HandleBuildingSelection(BuildingType buildingType)
        {
            Debug.Log($"HandleBuildingSelection: {buildingType}");

            this._buildingSelectedPublisher.Publish(new BuildingSelectedMessage
            {
                BuildingType = buildingType
            });
        }

        public override void Dispose()
        {
            this.view.onBuildSelected -= (buildType) => this.HandleBuildingSelection(buildType);
        }
    }
}

