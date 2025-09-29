using NeuroCityBuilder.Application.Interfaces;
using UnityEngine;

namespace NeuroCityBuilder.Presentation.UI
{
    public class GridCell : MonoBehaviour, IGridCell
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _highlightValidMaterial;
        [SerializeField] private Material _highlightInvalidMaterial;

        private void Awake()
        {
            if (this._renderer == null)
                this._renderer = this.GetComponent<Renderer>();
        }

        public void SetDefault()
        {
            if (this._renderer != null && this._defaultMaterial != null)
            {
                this._renderer.material = this._defaultMaterial;
            }
        }

        public void SetHighlightValid()
        {
            if (this._renderer != null && this._highlightValidMaterial != null)
            {
                this._renderer.material = this._highlightValidMaterial;
            }
        }

        public void SetHighlightInvalid()
        {
            if (this._renderer != null && this._highlightInvalidMaterial != null)
            {
                this._renderer.material = this._highlightInvalidMaterial;
            }
        }
    }
}

