using UnityEngine;

namespace Presentation.UI
{
    public class GridCell : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _highlightValidMaterial;
        [SerializeField] private Material _highlightInvalidMaterial;

        private Material _currentMaterial;

        public Material GetCurrentMaterial() => this._currentMaterial;

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
                this._currentMaterial = this._defaultMaterial;
            }
        }

        public void SetHighlightValid()
        {
            if (this._renderer != null && this._highlightValidMaterial != null)
            {
                this._renderer.material = this._highlightValidMaterial;
                this._currentMaterial = this._highlightValidMaterial;
            }
        }

        public void SetHighlightInvalid()
        {
            if (this._renderer != null && this._highlightInvalidMaterial != null)
            {
                this._renderer.material = this._highlightInvalidMaterial;
                this._currentMaterial = this._highlightInvalidMaterial;
            }
        }
    }
}

