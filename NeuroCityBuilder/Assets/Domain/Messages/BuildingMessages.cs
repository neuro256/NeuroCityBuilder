using Domain.Gameplay;
using UnityEngine;

namespace Domain.Messages
{
    public struct BuildingSelectedMessage
    {
        public BuildingType BuildingType;
    }

    public struct BuildingPlacementRequestMessage
    {
        public BuildingType BuildingType;
        public GridPos Position;
    }

    public struct BuildingPlacedMessage
    {
        public Building Building;
        public GridPos Position;
    }

    public struct BuildingPlacementCanceledMessage { }
}

