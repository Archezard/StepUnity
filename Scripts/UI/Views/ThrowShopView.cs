using BasketballCards.Services;
using UnityEngine;

namespace BasketballCards.UI.Views
{
    public class ThrowShopView : ShopViewBase
    {
        private ShopService _shopService;
        
        public void Initialize(ShopService shopService)
        {
            _shopService = shopService;
            // Инициализация магазина бросков
        }
    }
}