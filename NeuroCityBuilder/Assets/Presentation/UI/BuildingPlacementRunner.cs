using UnityEngine;
using VContainer;

namespace Presentation.UI
{
    public class BuildingPlacementRunner : MonoBehaviour
    {
        private BuildingInteractionSystem _placementSystem;

        [Inject]
        public void Construct(BuildingInteractionSystem placementSystem)
        {
            this._placementSystem = placementSystem;
        }

        private void Update()
        {
            this._placementSystem.Update();
        }
    }
}

