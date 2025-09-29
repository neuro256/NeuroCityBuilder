using NeuroCityBuilder.Application.Interfaces;
using NeuroCityBuilder.Domain.Gameplay;
using System.Collections.Generic;
using UnityEngine;

namespace NeuroCityBuilder.Application
{
    public interface IBuildingFactory : System.IDisposable
    {
        Building CreateBuilding(BuildingType type, GridPos position);
        GameObject CreateGhostBuilding(BuildingType type, Vector3 position);
        void DestroyGhostBuilding();
        void UpdateGhostBuildingPosition(Vector3 position);
        void RemoveBuildingVisual(GridPos position);
    }

    public class BuildingFactory : IBuildingFactory
    {
        private const float _ghostY = 1.5f;
        private const float _buildZ = 0.5f;

        private readonly IBuildingPrefabProvider _prefabProvider;
        private readonly IBuildingLevelProvider _levelProvider;
        private readonly Dictionary<GridPos, GameObject> _buildingInstances = new();
        private GameObject _ghostBuilding;

        public BuildingFactory(IBuildingPrefabProvider prefabProvider, 
            IBuildingLevelProvider levelProvider)
        {
            this._prefabProvider = prefabProvider;
            this._levelProvider = levelProvider;
        }

        public Building CreateBuilding(BuildingType type, GridPos position)
        {
            GameObject prefab = this._prefabProvider.GetBuildingPrefab(type);

            if (prefab == null)
            {
                Debug.LogError($"CreateBuilding prefab is null");
                return null;
            }

            Vector3 worldPosition = new Vector3(position.X, _buildZ, position.Y);
            GameObject buildingGO = UnityEngine.Object.Instantiate(prefab, worldPosition, Quaternion.identity, this._prefabProvider.Parent);

            Building building = new Building
            {
                Type = type,
                Position = position,
                CurrentLevel = 0,
                Levels = this._levelProvider.GetLevels(type)
            };

            this._buildingInstances[position] = buildingGO;

            return building;
        }

        public GameObject CreateGhostBuilding(BuildingType type, Vector3 position)
        {
            this.DestroyGhostBuilding();

            GameObject ghostPrefab = this._prefabProvider.GetGhostPrefab(type);

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
                Object.Destroy(this._ghostBuilding);
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
                Object.Destroy(buildingGO);
                this._buildingInstances.Remove(position);
                Debug.Log($"Building visual removed from {position.X},{position.Y}");
            }
        }

        public void Dispose()
        {
            this.ClearAllBuildings();
        }

        private void ClearAllBuildings()
        {
            foreach (GameObject buildingGO in this._buildingInstances.Values)
            {
                if (buildingGO != null)
                    Object.Destroy(buildingGO);
            }
            this._buildingInstances.Clear();
        }
    }
}

