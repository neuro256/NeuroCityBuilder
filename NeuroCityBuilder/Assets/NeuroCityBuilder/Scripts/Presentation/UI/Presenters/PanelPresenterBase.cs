using NeuroCityBuilder.Presentation.UI.Views;

namespace NeuroCityBuilder.Presentation.UI.Presenters
{
    public abstract class PanelPresenterBase<TView> : IPanelPresenter where TView : IPanelView
    {
        protected TView view;

        public PanelPresenterBase(TView view)
        {
            this.view = view;
        }

        public virtual void Initialize()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}

