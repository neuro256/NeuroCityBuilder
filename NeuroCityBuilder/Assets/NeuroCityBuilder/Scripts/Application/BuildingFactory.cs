using NeuroCityBuilder.Domain.Gameplay;
using System.Collections.Generic;
using UnityEngine;

namespace NeuroCityBuilder.Application
{
    public class BuildingFactory : MonoBehaviour
    {
        private const float _ghostY = 1.5f;
        private const float _buildZ = 0.5f;

        [Header("Building Prefabs")]
        [SerializeField] private GameObject _housePrefab;
        [SerializeField] private GameObject _farmPrefab;
        [SerializeField] private GameObject _minePrefab;
        [SerializeField] private Transform _buildingsParent;

        [Header("Ghost Prefabs")]
        [SerializeField] private GameObject _houseGhostPrefab;
        [SerializeField] private GameObject _farmGhostPrefab;
        [SerializeField] private GameObject _mineGhostPrefab;

        private readonly Dictionary<GridPos, GameObject> _buildingInstances = new();
        private GameObject _ghostBuilding;

        private void OnDestroy()
        {
            this.ClearAllBuildings();
        }

        public Building CreateBuilding(BuildingType type, GridPos position)
        {
            GameObject prefab = this.GetPrefabByType(type);
            if (prefab == null)
            {
                Debug.LogError($"CreateBuilding prefab is null");
                return null;
            }

            Vector3 worldPosition = new Vector3(position.X, _buildZ, position.Y);
            GameObject buildingGO = Object.Instantiate(prefab, worldPosition, Quaternion.identity, this._buildingsParent);

            Building building = new Building
            {
                Type = type,
                Position = position,
                CurrentLevel = 0,
                Levels = this.GetBuildingLevels(type)
            };

            this._buildingInstances[position] = buildingGO;

            return building;
        }

        public GameObject CreateGhostBuilding(BuildingType type, Vector3 position)
        {
            this.DestroyGhostBuilding();

            GameObject ghostPrefab = this.GetGhostPrefabByType(type);
            if (ghostPrefab == null)
            {
                Debug.LogError($"Ghost prefab is null for type: {type}");
                return null;
            }

            Vector3 ghostPosition = new Vector3(position.x, _ghostY, position.z);
            this._ghostBuilding = Object.Instantiate(ghostPrefab, ghostPosition, Quaternion.identity);
            this._ghostBuilding.name = $"{type}Ghost";

            return this._ghostBuilding;
        }

        public void DestroyGhostBuilding()
        {
            if (this._ghostBuilding != null)
            {
                Destroy(this._ghostBuilding);
                this._ghostBuilding = null;
            }
        }

        public void UpdateGhostBuildingPosition(Vector3 position)
        {
            if (this._ghostBuilding != null)
            {
                this._ghostBuilding.transform.position = new Vector3(position.x, _ghostY, position.z);
            }
        }

        public void RemoveBuildingVisual(GridPos position)
        {
            if (this._buildingInstances.TryGetValue(position, out GameObject buildingGO))
            {
                Destroy(buildingGO);
                this._buildingInstances.Remove(position);
                Debug.Log($"Building visual removed from {position.X},{position.Y}");
            }
        }

        private GameObject GetPrefabByType(BuildingType type)
        {
            Debug.Log($"GetPrefabByType: {type}");
            return type switch
            {
                BuildingType.House => this._housePrefab,
                BuildingType.Farm => this._farmPrefab,
                BuildingType.Mine => this._minePrefab,
                _ => null
            };
        }

        private GameObject GetGhostPrefabByType(BuildingType type)
        {
            return type switch
            {
                BuildingType.House => this._houseGhostPrefab,
                BuildingType.Farm => this._farmGhostPrefab,
                BuildingType.Mine => this._mineGhostPrefab,
                _ => null
            };
        }

        public void ClearAllBuildings()
        {
            foreach (GameObject buildingGO in this._buildingInstances.Values)
            {
                if (buildingGO != null)
                    Destroy(buildingGO);
            }
            this._buildingInstances.Clear();
        }

        public BuildingLevel[] GetBuildingLevels(BuildingType type)
        {
            switch (type)
            {
                case BuildingType.House:
                    return new[]
                    {
                        new BuildingLevel { Level = 1, Cost = 100, Income = 10 },
                        new BuildingLevel { Level = 2, Cost = 200, Income = 25 },
                        new BuildingLevel { Level = 3, Cost = 300, Income = 35 }
            };
                case BuildingType.Farm:
                    return new[]
                    {
                        new BuildingLevel { Level = 1, Cost = 150, Income = 15 },
                        new BuildingLevel { Level = 2, Cost = 300, Income = 35 },
                        new BuildingLevel { Level = 3, Cost = 500, Income = 45 }
            };
                case BuildingType.Mine:
                    return new[]
                    {
                        new BuildingLevel { Level = 1, Cost = 200, Income = 20 },
                        new BuildingLevel { Level = 2, Cost = 400, Income = 50 },
                        new BuildingLevel { Level = 3, Cost = 600, Income = 80 }
            };
                default:
                    return new BuildingLevel[0];
            }
        }
    }
}

