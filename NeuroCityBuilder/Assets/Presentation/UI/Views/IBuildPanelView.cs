using Domain.Gameplay;
using System;

namespace Presentation.UI.Views
{
    public interface IBuildPanelView : IPanelView
    {
        event Action<BuildingType> onBuildSelected;
    }
}

