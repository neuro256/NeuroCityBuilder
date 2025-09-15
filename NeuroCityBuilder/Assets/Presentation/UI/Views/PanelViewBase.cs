using UnityEngine;
using UnityEngine.UIElements;

namespace Presentation.UI.Views
{
    [RequireComponent(typeof(UIDocument))]
    public abstract class PanelViewBase : MonoBehaviour
    {
        protected VisualElement root;
        protected UIDocument uiDocument;

        public virtual void Awake()
        {
            this.uiDocument = this.GetComponent<UIDocument>();
            this.root = this.uiDocument.rootVisualElement;
        }
    }
}

