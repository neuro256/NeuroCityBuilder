using Domain.Gameplay;
using UnityEngine;

namespace Domain.Messages
{
    public struct BuildingTypeSelectedMessage
    {
        public BuildingType BuildingType;
    }

    public struct BuildingSelectedMessage
    {
        public Building Building;
    }

    public struct BuildingPlacedMessage
    {
        public Building Building;
        public GridPos Position;
    }

    public struct BuildingPlacementCanceledMessage { }
    public struct BuildingDeselectedMessage { }

    public struct BuildingDeletedMessage
    {
        public Building Building;
        public GridPos Position;
    }
}

