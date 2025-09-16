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

    public struct BuildingDeleteRequestMessage 
    {
        public Building Building;
    }

    public struct BuildingDeletedMessage
    {
        public Building Building;
        public GridPos Position;
    }

    public struct BuildingUpgradedMessage
    {
        public Building Building;
    }

    public struct BuildingUpgradeRequestMessage
    {
        public Building Building;
    }

    public struct BuildingMoveRequestMessage
    {
        public Building Building;
    }

    public struct BuildingMoveStartMessage
    {
        public Building Building;
    }

    public struct BuildingMoveCompleteMessage
    {
        public Building Building;
        public GridPos NewPosition;
    }

    public struct BuildingMoveCancelMessage
    {
        public Building Building;
    }

    public struct GoldAddedMessage
    {
        public int Amount;
        public int Total;
    }
}

