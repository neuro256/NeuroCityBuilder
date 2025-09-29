using NeuroCityBuilder.Application.Services;
using UnityEngine;
using VContainer.Unity;

namespace NeuroCityBuilder.Infrastructure
{
    public class GameEntryPoint : IStartable
    {
        private readonly IIncomeService _incomeService;

        public GameEntryPoint(IIncomeService incomeService)
        {
            this._incomeService = incomeService;
        }

        public void Start()
        {
            Debug.Log("Game started");

            this._incomeService.StartIncomeGeneration();
        }
    }
}


