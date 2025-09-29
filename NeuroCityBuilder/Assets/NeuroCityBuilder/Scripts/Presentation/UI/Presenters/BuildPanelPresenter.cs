using MessagePipe;
using NeuroCityBuilder.Domain.Gameplay;
using NeuroCityBuilder.Domain.Messages;
using NeuroCityBuilder.Presentation.UI.Views;
using UnityEngine;

namespace NeuroCityBuilder.Presentation.UI.Presenters
{
    public class BuildPanelPresenter : PanelPresenterBase<IBuildPanelView>
    {
        private readonly IPublisher<BuildingTypeSelectedMessage> _buildingSelectedPublisher;

        public BuildPanelPresenter(IBuildPanelView view, IPublisher<BuildingTypeSelectedMessage> buildingSelectedPublisher) : base(view)
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

            this._buildingSelectedPublisher.Publish(new BuildingTypeSelectedMessage
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

