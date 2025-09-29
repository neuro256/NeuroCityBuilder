using NeuroCityBuilder.Domain.Gameplay;
using System;

namespace NeuroCityBuilder.Presentation.UI.Views
{
    public interface IBuildPanelView : IPanelView
    {
        event Action<BuildingType> onBuildSelected;
    }
}

