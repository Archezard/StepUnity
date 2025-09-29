using BasketballCards.Services;
using UnityEngine;

namespace BasketballCards.UI.Views
{
    public class PackShopView : ShopViewBase
    {
        // Реализация магазина паков
        private ShopService _shopService;
        
        public System.Action OnBackRequested;
        
        public void Initialize(ShopService shopService)
        {
            _shopService = shopService;
            // Инициализация магазина паков
        }
        
        // ПЕРЕОПРЕДЕЛЯЕМ метод для конкретной логики PackShopView
        protected override void OnBackButtonClicked()
        {
            OnBackRequested?.Invoke();
        }
    }
}