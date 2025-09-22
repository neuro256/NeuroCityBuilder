using Domain.Gameplay;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Overlays;
using UnityEngine;

namespace UseCases.Services
{
    /// <summary>
    /// Сервис для сохранения/загрузки состояния игры (местоположения и уровня зданий, ресурсов)
    /// </summary>
    public class GameSaveService : IGameSaveService
    {
        private readonly IBuildingService _buildingService;
        private readonly IResourceService _resourceService;
        private readonly string _savePath;

        public GameSaveService(IBuildingService buildingService, IResourceService resourceService)
        {
            this._buildingService = buildingService;
            this._resourceService = resourceService;
            this._savePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        }

        /// <summary>
        /// Сохранение текущего состояния построенных зданий и ресурсов
        /// </summary>
        public void SaveGame()
        {
            List<BuildingSaveData> buildings = this._buildingService.GetAllBuildings()
            .Select(b => new BuildingSaveData
            {
                Type = b.Type,
                Position = b.Position,
                CurrentLevel = b.CurrentLevel
            }).ToList();

            GameSaveData saveData = new GameSaveData
            {
                Buildings = buildings,
                Gold = this._resourceService.Resources.Gold
            };

            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(this._savePath, json);
            Debug.Log($"Game saved to {this._savePath}");
        }

        public void LoadGame()
        {
            if (!File.Exists(this._savePath))
            {
                Debug.LogWarning("Save file not found");
                return;
            }

            string json = File.ReadAllText(this._savePath);
            GameSaveData savedData = JsonUtility.FromJson<GameSaveData>(json);

            if (savedData == null)
            {
                Debug.LogError("Failed to deserialize save data");
                return;
            }

            try
            {
                this._buildingService.ClearAllBuildings();

                // Загружаем здания
                foreach (BuildingSaveData buildingData in savedData.Buildings)
                {
                    Building building = this._buildingService.PlaceBuilding(buildingData.Type, buildingData.Position, true);
                    if (building != null)
                    {
                        building.CurrentLevel = buildingData.CurrentLevel;
                    }
                }

                //Загружаем ресурсы
                this._resourceService.Resources.Gold = savedData.Gold;

                Debug.Log("Game loaded!");
            }
            catch(Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}

