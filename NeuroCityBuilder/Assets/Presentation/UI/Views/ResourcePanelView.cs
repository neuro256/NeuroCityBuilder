using UnityEngine;
using UnityEngine.UIElements;

namespace Presentation.UI.Views
{
    public class ResourcePanelView : PanelViewBase, IResourcePanelView
    {
        private Label _goldLabel;

        public override void Awake()
        {
            base.Awake();

            this._goldLabel = this.root.Q<Label>("gold-label");
        }

        public void UpdateGold(int value)
        {
            this._goldLabel.text = $"Gold: {value}";
        }
    }
}

