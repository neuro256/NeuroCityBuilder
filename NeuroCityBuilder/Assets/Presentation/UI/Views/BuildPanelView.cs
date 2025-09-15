using Domain.Gameplay;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Presentation.UI.Views
{
    public class BuildPanelView : PanelViewBase
    {
        private Button _houseButton;
        private Button _farmButton;
        private Button _mineButton;

        public event Action<BuildingType> onBuildSelected;

        public override void Awake()
        {
            base.Awake();

            this._houseButton = this.root.Q<Button>("house-button");
            this._farmButton = this.root.Q<Button>("farm-button");
            this._mineButton = this.root.Q<Button>("mine-button");

            this._houseButton.clicked += () => onBuildSelected?.Invoke(BuildingType.House);
            this._farmButton.clicked += () => onBuildSelected?.Invoke(BuildingType.Farm);
            this._mineButton.clicked += () => onBuildSelected?.Invoke(BuildingType.Mine);
        }
    }
}

