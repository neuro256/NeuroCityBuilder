using Presentation.UI.Views;
using System;
using UnityEngine;
using VContainer.Unity;

namespace Presentation.UI.Presenters
{
    public class SaveLoadPanelPresenter : PanelPresenterBase<SaveLoadPanelView>
    {
        public SaveLoadPanelPresenter(SaveLoadPanelView view) : base(view) { }

        public override void Initialize()
        {
            this.view.onSaveClicked += this.HandleSaveClicked;
            this.view.onLoadClicked += this.HandleLoadClicked;
        }

        private void HandleSaveClicked()
        {
            //this._saveService.SaveGame();
            Debug.Log("Game saved");
        }

        private void HandleLoadClicked()
        {
            //_saveService.LoadGame();
            Debug.Log("Game loaded");
        }

        public override void Dispose()
        {
            this.view.onSaveClicked -= this.HandleSaveClicked;
            this.view.onLoadClicked -= this.HandleLoadClicked;
        }
    }
}

