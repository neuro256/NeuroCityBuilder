using UnityEngine;

namespace NeuroCityBuilder.Application.Services
{
    public interface IGameSaveService
    {
        void SaveGame();
        void LoadGame();
    }
}

