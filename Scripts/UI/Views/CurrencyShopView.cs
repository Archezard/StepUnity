using BasketballCards.Services;
using UnityEngine;

namespace BasketballCards.UI.Views
{
    public class CurrencyShopView : ShopViewBase
    {
        // Реализация магазина валюты
        private ShopService _shopService;
        
        public System.Action OnBackRequested;
        
        public void Initialize(ShopService shopService)
        {
            _shopService = shopService;
            // Инициализация магазина валюты
        }
        
        // ПЕРЕОПРЕДЕЛЯЕМ метод для конкретной логики CurrencyShopView
        protected override void OnBackButtonClicked()
        {
            OnBackRequested?.Invoke();
        }
    }
}