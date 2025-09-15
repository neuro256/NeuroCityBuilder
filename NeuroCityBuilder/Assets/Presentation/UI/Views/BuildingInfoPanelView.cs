using UnityEngine;
using UnityEngine.UIElements;

namespace Presentation.UI.Views
{
    public class BuildingInfoPanelView : PanelViewBase
    {
        private Label _levelLabel;
        private Label _incomeLabel;

        public override void Awake()
        {
            base.Awake();

            this._levelLabel = this.root.Q<Label>("building-level");
            this._incomeLabel = this.root.Q<Label>("building-income");
        }

        public void ShowInfo(int level, int income)
        {
            this._levelLabel.text = $"Level: {level}";
            this._incomeLabel.text = $"Income: {income}";
        }

        public void HideInfo()
        {
            this._levelLabel.text = "Level: -";
            this._incomeLabel.text = "Income: -";
        }
    }
}

