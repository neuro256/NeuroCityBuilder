using NeuroCityBuilder.Application.Interfaces;
using UnityEngine;

namespace NeuroCityBuilder.Infrastructure
{
    public class GridPrefabProvider : MonoBehaviour, IGridPrefabProvider
    {
        [SerializeField] private GameObject _gridCell;

        public GameObject GridCell => this._gridCell;
    }
}

