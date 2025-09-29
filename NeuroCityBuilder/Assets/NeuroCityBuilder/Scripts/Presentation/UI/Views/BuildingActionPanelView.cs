using System;
using UnityEngine.UIElements;

namespace NeuroCityBuilder.Presentation.UI.Views
{
    public class BuildingActionPanelView : PanelViewBase, IBuildingActionPanelView
    {
        private Button _moveButton;
        private Button _upgradeButton;
        private Button _deleteButton;

        public event Action onMoveClicked;
        public event Action onUpgradeClicked;
        public event Action onDeleteClicked;

        public override void Awake()
        {
            base.Awake();

            this._moveButton = this.root.Q<Button>("move-button");
            this._upgradeButton = this.root.Q<Button>("upgrade-button");
            this._deleteButton = this.root.Q<Button>("delete-button");

            this._moveButton.clicked += this.OnMoveButtonClicked;
            this._upgradeButton.clicked += this.OnUpgradeButtonClicked;
            this._deleteButton.clicked += this.OnDeleteButtonClicked;

            this.HidePanel();
        }

        private void OnMoveButtonClicked()
        {
            onMoveClicked?.Invoke();
        }

        private void OnUpgradeButtonClicked()
        {
            onUpgradeClicked?.Invoke();
        }

        private void OnDeleteButtonClicked()
        {
            onDeleteClicked?.Invoke();
        }

        private void OnDestroy()
        {
            if (this._moveButton != null)
                this._moveButton.clicked -= this.OnMoveButtonClicked;
            if (this._upgradeButton != null)
                this._upgradeButton.clicked -= this.OnUpgradeButtonClicked;
            if (this._deleteButton != null)
                this._deleteButton.clicked -= this.OnDeleteButtonClicked;
        }

        public void EnableMoveButton(bool enabled)
        {
            this._moveButton.SetEnabled(enabled);
        }

        public void EnableUpgradeButton(bool enabled)
        {
            this._upgradeButton.SetEnabled(enabled);
        }

        public void EnableDeleteButton(bool enabled)
        {
            this._deleteButton.SetEnabled(enabled);
        }

        public void ShowPanel()
        {
            this.root.style.display = DisplayStyle.Flex;
        }

        public void HidePanel()
        {
            this.root.style.display = DisplayStyle.None;
        }
    }
}

