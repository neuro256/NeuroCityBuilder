using Presentation.UI.Views;
using System;
using UnityEngine;
using UseCases.Services;
using VContainer.Unity;

namespace Presentation.UI.Presenters
{
    public class SaveLoadPanelPresenter : PanelPresenterBase<ISaveLoadPanelView>
    {
        private readonly IGameSaveService _saveService;

        public SaveLoadPanelPresenter(ISaveLoadPanelView view, IGameSaveService saveService) : base(view) 
        {
            this._saveService = saveService;
        }

        public override void Initialize()
        {
            this.view.onSaveClicked += this.HandleSaveClicked;
            this.view.onLoadClicked += this.HandleLoadClicked;
        }

        private void HandleSaveClicked()
        {
            this._saveService.SaveGame();
            Debug.Log("Game saved");
        }

        private void HandleLoadClicked()
        {
            this._saveService.LoadGame();
            Debug.Log("Game loaded");
        }

        public override void Dispose()
        {
            this.view.onSaveClicked -= this.HandleSaveClicked;
            this.view.onLoadClicked -= this.HandleLoadClicked;
        }
    }
}

