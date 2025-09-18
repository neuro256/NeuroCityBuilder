using System;
using Domain.Gameplay;

namespace UseCases.Services
{
    public interface IResourceService
    {
        ResourceData Resources { get; }
        void AddGold(int amount);
        bool CanAfford(int buildingCost);
        void SpendGold(int amount);
    }
}
