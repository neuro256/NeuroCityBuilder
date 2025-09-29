namespace NeuroCityBuilder.Presentation.UI.Views
{
    public interface IBuildingInfoPanelView : IPanelView
    {
        void ShowInfo(int level, int income);
        void HideInfo();
    }
}

