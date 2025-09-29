using NeuroCityBuilder.Domain.Gameplay;
using System;
using VContainer.Unity;

namespace NeuroCityBuilder.Presentation.UI.Presenters
{
    public interface IGridPresenter : IInitializable, IDisposable
    {
        void ShowHighlight(GridPos gridPos, bool isValid);
        void HideHighlight();
    }
}

