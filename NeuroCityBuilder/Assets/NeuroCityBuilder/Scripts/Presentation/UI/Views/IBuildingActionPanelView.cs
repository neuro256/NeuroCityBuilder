using System;

namespace NeuroCityBuilder.Presentation.UI.Views
{
    public interface IBuildingActionPanelView : IPanelView
    {
        event Action onMoveClicked;
        event Action onUpgradeClicked;
        event Action onDeleteClicked;

        void EnableUpgradeButton(bool enabled);
        void ShowPanel();
        void HidePanel();
    }
}

