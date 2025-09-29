using BasketballCards.Core;
using BasketballCards.UI.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public abstract class ShopViewBase : BaseView
    {
        
        protected ShopPresenter _presenter;
        
        public virtual void Initialize(ShopPresenter presenter)
        {
            _presenter = presenter;
        }
        
        protected override void OnBackButtonClicked()
        {
            // Базовая реализация для магазина - потом переделать
            EventSystem.NavigateBack();
        }
        
        public override void Show()
        {
            gameObject.SetActive(true);
        }
        
        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}