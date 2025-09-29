using NeuroCityBuilder.Application.Interfaces;
using NeuroCityBuilder.Domain.Gameplay;
using UnityEngine;

namespace NeuroCityBuilder.Infrastructure
{
    public class BuildingPrefabProvider : MonoBehaviour, IBuildingPrefabProvider
    {
        [Header("Building Prefabs")]
        [SerializeField] private GameObject _housePrefab;
        [SerializeField] private GameObject _farmPrefab;
        [SerializeField] private GameObject _minePrefab;

        [Header("Ghost Prefabs")]
        [SerializeField] private GameObject _houseGhostPrefab;
        [SerializeField] private GameObject _farmGhostPrefab;
        [SerializeField] private GameObject _mineGhostPrefab;

        public Transform Parent => this.transform;

        public GameObject GetBuildingPrefab(BuildingType type) => type switch
        {
            BuildingType.House => this._housePrefab,
            BuildingType.Farm => this._farmPrefab,
            BuildingType.Mine => this._minePrefab,
            _ => null
        };

        public GameObject GetGhostPrefab(BuildingType type) => type switch
        {
            BuildingType.House => this._houseGhostPrefab,
            BuildingType.Farm => this._farmGhostPrefab,
            BuildingType.Mine => this._mineGhostPrefab,
            _ => null
        };
    }
}

