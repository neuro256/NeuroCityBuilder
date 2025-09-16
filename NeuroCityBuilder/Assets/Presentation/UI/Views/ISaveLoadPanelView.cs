using System;

namespace Presentation.UI.Views
{
    public interface ISaveLoadPanelView : IPanelView
    {
        event Action onSaveClicked;
        event Action onLoadClicked;

        void EnableSaveButton(bool value);
        void EnableLoadButton(bool value);
    }
}

