using NeuroCityBuilder.Domain.Gameplay;

namespace NeuroCityBuilder.Application.Services
{
    public interface IResourceService
    {
        ResourceData Resources { get; }
        void AddGold(int amount);
        bool CanAfford(int buildingCost);
        void SpendGold(int amount);
    }
}
