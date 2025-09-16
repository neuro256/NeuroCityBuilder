using Cysharp.Threading.Tasks;
using Domain.Gameplay;
using Domain.Messages;
using MessagePipe;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading;
using UnityEngine;

namespace UseCases.Services
{
    public class ResourceService : IResourceService
    {
        private const float _incomeDelay = 5f;

        private readonly IPublisher<GoldAddedMessage> _goldAddedPublisher;

        public ResourceData Resources { get; private set; }

        private readonly IBuildingService _buildingService;

        private CancellationTokenSource _cancellationTokenSource;
        private bool _isRunning;

        public ResourceService(IBuildingService buildingService, IPublisher<GoldAddedMessage> goldAddedPublisher)
        {
            this._buildingService = buildingService;
            this._goldAddedPublisher = goldAddedPublisher;

            this.Resources = new ResourceData()
            {
                Gold = 1000
            };
        }

        public void StartIncomeGeneration()
        {
            if (this._isRunning) return;

            this._isRunning = true;
            this._cancellationTokenSource = new CancellationTokenSource();
            this.GenerateIncomeLoop(this._cancellationTokenSource.Token).Forget();
            Debug.Log("Income generation started");
        }

        public void StopIncomeGeneration()
        {
            if (!this._isRunning) return;

            this._isRunning = false;
            this._cancellationTokenSource?.Cancel();
            Debug.Log("Income generation stopped");
        }

        private async UniTaskVoid GenerateIncomeLoop(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(_incomeDelay), cancellationToken: cancellationToken);

                    int totalIncome = this.CalculateTotalIncome();
                    if (totalIncome > 0)
                    {
                        this.AddGold(totalIncome);
                        Debug.Log($"Income generated: +{totalIncome} gold");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Income loop cancelled");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error in income loop: {ex.Message}");
            }
        }

        private int CalculateTotalIncome()
        {
            List<Building> buildings = this._buildingService.GetAllBuildings();

            if (buildings == null)
                return 0;

            int totalIncome = 0;

            foreach (Building building in buildings)
            {
                BuildingLevel levelInfo = building?.GetCurrentLevelInfo();
                if (levelInfo != null)
                {
                    totalIncome += levelInfo.Income;
                }
            }

            return totalIncome;
        }

        private void AddGold(int amount)
        {
            if (amount <= 0) return;

            this.Resources.Gold += amount;
            this._goldAddedPublisher.Publish(new GoldAddedMessage
            {
                Amount = amount,
                Total = this.Resources.Gold
            });

            Debug.Log($"Gold added: +{amount}, Total: {this.Resources.Gold}");
        }

        public void Dispose()
        {
            this.StopIncomeGeneration();
            this._cancellationTokenSource?.Dispose();
        }
    }
}
