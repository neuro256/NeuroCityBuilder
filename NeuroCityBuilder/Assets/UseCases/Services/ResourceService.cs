using Domain.Gameplay;
using Domain.Messages;
using MessagePipe;
using System;
using UnityEngine;

namespace UseCases.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IPublisher<GoldAddedMessage> _goldAddedPublisher;

        public ResourceData Resources { get; private set; }

        public ResourceService(IPublisher<GoldAddedMessage> goldAddedPublisher)
        {
            this._goldAddedPublisher = goldAddedPublisher;

            this.Resources = new ResourceData()
            {
                Gold = 1000
            };
        }

        public void AddGold(int amount)
        {
            this.ChangeGold(amount);
        }

        public bool CanAfford(int buildingCost)
        {
            return this.Resources.Gold >= buildingCost;
        }

        public void SpendGold(int amount)
        {
            this.ChangeGold(-amount);
        }

        private void ChangeGold(int amount)
        {
            this.Resources.Gold += amount;
            this.Resources.Gold = Math.Max(0, this.Resources.Gold);

            this._goldAddedPublisher.Publish(new GoldAddedMessage
            {
                Amount = amount,
                Total = this.Resources.Gold
            });

            Debug.Log($"Gold added: +{amount}, Total: {this.Resources.Gold}");
        }
    }
}
