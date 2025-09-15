using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Presentation.UI.Views
{
    public class BuildingActionPanelView : PanelViewBase
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

            this._moveButton.clicked += () => onMoveClicked?.Invoke();
            this._upgradeButton.clicked += () => onUpgradeClicked?.Invoke();
            this._deleteButton.clicked += () => onDeleteClicked?.Invoke();
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
            this.gameObject.SetActive(true);
        }

        public void HidePanel()
        {
            this.gameObject.SetActive(false);
        }
    }
}

