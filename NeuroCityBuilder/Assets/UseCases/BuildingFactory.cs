using Domain.Gameplay;
using UnityEngine;

namespace UseCases
{
    public class BuildingFactory : MonoBehaviour
    {
        [SerializeField] private GameObject _housePrefab;
        [SerializeField] private GameObject _farmPrefab;
        [SerializeField] private GameObject _minePrefab;
        [SerializeField] private Transform _buildingsParent;


        public Building CreateBuilding(BuildingType type, GridPos position)
        {
            GameObject prefab = this.GetPrefabByType(type);
            if (prefab == null)
            {
                Debug.LogError($"CreateBuilding prefab is null");
                return null;
            }

            Vector3 worldPosition = this.GridToWorld(position);
            GameObject buildingGO = Object.Instantiate(prefab, worldPosition, Quaternion.identity, this._buildingsParent);

            Building building = new Building
            {
                Type = type,
                Position = position,
                CurrentLevel = 0
            };

            return building;
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

        private Vector3 GridToWorld(GridPos gridPos)
        {
            float offsetX = -(32) / 2f; 
            return new Vector3(gridPos.X + offsetX, 0.5f, gridPos.Y);
        }
    }
}

