using BasketballCards.Models;
using BasketballCards.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class ShopItemElement : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _buyButton;
        
        private ShopItem _item;
        private System.Action<ShopItem> _onPurchase;
        
        public void Initialize(ShopItem item, System.Action<ShopItem> onPurchase)
        {
            _item = item;
            _onPurchase = onPurchase;
            
            _nameText.text = item.Name;
            _priceText.text = $"{item.Price} {item.Currency}";
            
            _buyButton.onClick.AddListener(OnBuyButtonClicked);
        }
        
        private void OnBuyButtonClicked()
        {
            _onPurchase?.Invoke(_item);
        }
    }
}