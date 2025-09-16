using UnityEngine;
using UseCases.Services;
using VContainer.Unity;

namespace Infrastructure
{
    public class GameEntryPoint : IStartable
    {
        private readonly IResourceService _resourceService;

        public GameEntryPoint(IResourceService resourceService)
        {
            this._resourceService = resourceService;
        }

        public void Start()
        {
            Debug.Log("Game started");

            this._resourceService.StartIncomeGeneration();
        }
    }
}


