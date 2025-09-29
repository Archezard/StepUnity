using BasketballCards.Services;
using UnityEngine;

namespace BasketballCards.UI.Views
{
    public class ThrowShopView : ShopViewBase
    {
        // Реализация магазина бросков
        private ShopService _shopService;
        
        public System.Action OnBackRequested;
        
        public void Initialize(ShopService shopService)
        {
            _shopService = shopService;
            // Инициализация магазина бросков
        }
        
        // ПЕРЕОПРЕДЕЛЯЕМ метод для конкретной логики ThrowShopView
        protected override void OnBackButtonClicked()
        {
            OnBackRequested?.Invoke();
        }
    }
}