using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class ShopView : ShopViewBase
    {
        [Header("Category Buttons")]
        [SerializeField] private Button _cardShopButton;
        [SerializeField] private Button _throwShopButton;
        [SerializeField] private Button _packShopButton;
        [SerializeField] private Button _currencyShopButton;
        
        public System.Action OnCardShopSelected;
        public System.Action OnThrowShopSelected;
        public System.Action OnPackShopSelected;
        public System.Action OnCurrencyShopSelected;
        
        public void Initialize()
        {
            _cardShopButton.onClick.AddListener(() => OnCardShopSelected?.Invoke());
            _throwShopButton.onClick.AddListener(() => OnThrowShopSelected?.Invoke());
            _packShopButton.onClick.AddListener(() => OnPackShopSelected?.Invoke());
            _currencyShopButton.onClick.AddListener(() => OnCurrencyShopSelected?.Invoke());
        }
    }
}