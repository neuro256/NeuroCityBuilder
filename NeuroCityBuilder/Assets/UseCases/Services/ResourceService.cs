using Cysharp.Threading.Tasks;
using Domain.Gameplay;
using Domain.Messages;
using MessagePipe;
using System;
using System.Threading;
using UnityEngine;

namespace UseCases.Services
{
    public class ResourceService : IResourceService
    {
        private const float _incomeDelay = 5f;

        private readonly IPublisher<GoldAddedMessage> _goldAddedPublisher;

        public ResourceData Resources { get; private set; }

        private CancellationTokenSource _cancellationTokenSource;
        private IDisposable _subscription;
        private bool _isRunning;

        public ResourceService(IPublisher<GoldAddedMessage> goldAddedPublisher)
        {
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

        public bool CanAfford(int buildingCost)
        {
            return this.Resources.Gold >= buildingCost;
        }

        public void SpendGold(int amount)
        {
            this.ChangeGold(-amount);
        }

        private async UniTaskVoid GenerateIncomeLoop(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(_incomeDelay), cancellationToken: cancellationToken);

                    int totalIncome = 10; //Для отладки, нашел баг
                    if (totalIncome > 0)
                    {
                        this.ChangeGold(totalIncome);
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

        public void Dispose()
        {
            this.StopIncomeGeneration();
            this._cancellationTokenSource?.Dispose();
        }
    }
}
