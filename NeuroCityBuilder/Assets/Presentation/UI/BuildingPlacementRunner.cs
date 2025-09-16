using UnityEngine;
using VContainer;

namespace Presentation.UI
{
    public class BuildingPlacementRunner : MonoBehaviour
    {
        private BuildingPlacementSystem _placementSystem;

        [Inject]
        public void Construct(BuildingPlacementSystem placementSystem)
        {
            this._placementSystem = placementSystem;
        }

        private void Update()
        {
            this._placementSystem.Update();
        }
    }
}

