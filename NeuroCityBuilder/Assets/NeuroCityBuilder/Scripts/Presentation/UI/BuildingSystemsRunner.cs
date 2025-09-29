using NeuroCityBuilder.Presentation.System;
using UnityEngine;
using VContainer;

namespace NeuroCityBuilder.Presentation.UI
{
    public class BuildingSystemsRunner : MonoBehaviour
    {
        private BuildingInteractionSystem _interactionSystem;
        private BuildingMoveSystem _moveSystem;

        [Inject]
        public void Construct(BuildingInteractionSystem placementSystem, BuildingMoveSystem moveSystem)
        {
            this._interactionSystem = placementSystem;
            this._moveSystem = moveSystem;
        }

        private void Update()
        {
            this._interactionSystem?.Update();
            this._moveSystem?.Update();
        }
    }
}

