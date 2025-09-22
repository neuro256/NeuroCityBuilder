using UnityEngine;

namespace UseCases.Services
{
    public interface IGameSaveService
    {
        void SaveGame();
        void LoadGame();
    }
}

