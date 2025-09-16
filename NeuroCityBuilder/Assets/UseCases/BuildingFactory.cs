using Domain.Gameplay;
using System.Collections.Generic;
using UnityEngine;

namespace UseCases
{
    public class BuildingFactory : MonoBehaviour
    {
        [SerializeField] private GameObject _housePrefab;
        [SerializeField] private GameObject _farmPrefab;
        [SerializeField] private GameObject _minePrefab;
        [SerializeField] private Transform _buildingsParent;

        private readonly Dictionary<GridPos, GameObject> _buildingInstances = new();

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

            Vector3 worldPosition = new Vector3(position.X, 0.5f, position.Y);
            GameObject buildingGO = Object.Instantiate(prefab, worldPosition, Quaternion.identity, this._buildingsParent);

            Building building = new Building
            {
                Type = type,
                Position = position,
                CurrentLevel = 0
            };

            this._buildingInstances[position] = buildingGO;

            return building;
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

        public GameObject GetBuildingVisual(GridPos position)
        {
            this._buildingInstances.TryGetValue(position, out GameObject buildingGO);
            return buildingGO;
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

        public void ClearAllBuildings()
        {
            foreach (GameObject buildingGO in this._buildingInstances.Values)
            {
                if (buildingGO != null)
                    Destroy(buildingGO);
            }
            this._buildingInstances.Clear();
        }
    }
}

