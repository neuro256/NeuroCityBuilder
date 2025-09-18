using Cysharp.Threading.Tasks;
using Domain.Gameplay;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace UseCases.Services
{
    /// <summary>
    /// Сервис для начисления дохода с построенных зданий
    /// </summary>
    public class IncomeService : IIncomeService
    {
        private const float _incomeDelay = 5f;

        private readonly IBuildingService _buildingService;
        private readonly IResourceService _resourceService;

        private CancellationTokenSource _cancellationTokenSource;
        private bool _isRunning;

        public IncomeService(IBuildingService buildingService, IResourceService resourceService)
        {
            this._buildingService = buildingService;
            this._resourceService = resourceService;
        }

        /// <summary>
        /// Начинает процесс генерации дохода с построенных зданий
        /// </summary>
        public void StartIncomeGeneration()
        {
            if (this._isRunning) return;

            this._isRunning = true;
            this._cancellationTokenSource = new CancellationTokenSource();
            this.GenerateIncomeLoop(this._cancellationTokenSource.Token).Forget();
            Debug.Log("Income generation started");
        }

        /// <summary>
        /// Останавливает процесс генерации дохода с построенных зданий
        /// </summary>
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
                        this._resourceService.AddGold(totalIncome);
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
            try
            {
                List<Building> buildings = this._buildingService?.GetAllBuildings();
                if (buildings == null) return 0;

                int totalIncome = 0;

                foreach (Building building in buildings)
                {
                    if (building != null)
                    {
                        BuildingLevel levelInfo = building.GetCurrentLevelInfo();
                        if (levelInfo != null)
                        {
                            totalIncome += levelInfo.Income;
                        }
                    }
                }

                return totalIncome;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error calculating income: {ex.Message}");
                return 0;
            }
        }

        public void Dispose()
        {
            this.StopIncomeGeneration();
            this._cancellationTokenSource?.Dispose();
        }
    }
}

