using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace NeuroCityBuilder.Presentation.UI.Views
{
    public class SaveLoadPanelView : PanelViewBase, ISaveLoadPanelView
    {
        public event Action onSaveClicked;
        public event Action onLoadClicked;

        private Button _saveButton;
        private Button _loadButton;

        public override void Awake()
        {
            base.Awake();

            this._saveButton = this.root.Q<Button>("save-button");
            this._loadButton = this.root.Q<Button>("load-button");

            this._saveButton.clicked += () => this.OnSaveClicked();
            this._loadButton.clicked += () => onLoadClicked?.Invoke();
        }

        public void EnableSaveButton(bool value)
        {
            this._saveButton.SetEnabled(value);
        }

        public void EnableLoadButton(bool value)
        {
            this._loadButton.SetEnabled(value);
        }

        private void OnSaveClicked()
        {
            onSaveClicked?.Invoke();
            Debug.Log("On Save Clicked");
        }
    }
}

