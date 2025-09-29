using BasketballCards.Services;
using UnityEngine;

namespace BasketballCards.UI.Views
{
    public class PackShopView : ShopViewBase
    {
        private ShopService _shopService;
        
        public void Initialize(ShopService shopService)
        {
            _shopService = shopService;
            // Инициализация магазина паков
        }
    }
}