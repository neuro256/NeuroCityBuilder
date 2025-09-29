using MessagePipe;
using NeuroCityBuilder.Application.Services;
using NeuroCityBuilder.Domain.Messages;
using NeuroCityBuilder.Presentation.UI.Views;
using System;

namespace NeuroCityBuilder.Presentation.UI.Presenters
{
    public class ResourcePanelPresenter : PanelPresenterBase<IResourcePanelView>
    {
        private readonly IResourceService _resourceService;
        private readonly ISubscriber<GoldAddedMessage> _goldAddedSubscriber;
        private IDisposable _subscription;

        public ResourcePanelPresenter(IResourcePanelView view, 
            IResourceService resourceService, 
            ISubscriber<GoldAddedMessage> goldAddedSubscriber) : base(view) 
        { 
            this._resourceService = resourceService;
            this._goldAddedSubscriber = goldAddedSubscriber;    
        } 

        public override void Initialize()
        {
            DisposableBagBuilder bag = DisposableBag.CreateBuilder();
            this._goldAddedSubscriber.Subscribe(this.OnGoldAdded).AddTo(bag);
            this._subscription = bag.Build();
        }

        private void OnGoldAdded(GoldAddedMessage message)
        {
            this.OnGoldChanged(message.Total);
        }

        private void OnGoldChanged(int goldAmount)
        {
            this.view.UpdateGold(goldAmount);
        }

        public override void Dispose()
        {
            this._subscription?.Dispose();
        }
    }
}

