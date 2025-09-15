using Presentation.UI.Views;
using System;
using UnityEngine;
using VContainer.Unity;

namespace Presentation.UI.Presenters
{
    public abstract class PanelPresenterBase<TView> : IInitializable, IDisposable where TView : PanelViewBase
    {
        protected TView view;

        public virtual void Initialize()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}

