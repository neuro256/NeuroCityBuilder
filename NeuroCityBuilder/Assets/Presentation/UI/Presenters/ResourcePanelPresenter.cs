using Presentation.UI.Views;
using System;
using UnityEngine;
using VContainer.Unity;

namespace Presentation.UI.Presenters
{
    public class ResourcePanelPresenter : PanelPresenterBase<ResourcePanelView>
    {
        public ResourcePanelPresenter(ResourcePanelView view) : base(view) { } 

        public override void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

