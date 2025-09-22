using Domain.Gameplay;
using System;
using UnityEngine;
using VContainer.Unity;

namespace Presentation.UI.Presenters
{
    public interface IGridPresenter : IInitializable, IDisposable
    {
        void ShowHighlight(GridPos gridPos, bool isValid);
        void HideHighlight();
    }
}

