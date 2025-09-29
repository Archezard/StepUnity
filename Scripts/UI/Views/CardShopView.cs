using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.Services;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class CardShopView : ShopViewBase
    {
        [Header("UI References")]
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private ScrollRect _scrollRect;
        
        private ShopService _shopService;
        private List<ShopItemElement> _itemElements = new List<ShopItemElement>();
        
        public System.Action OnBackRequested;
        
        public void Initialize(ShopService shopService)
        {
            _shopService = shopService;
            LoadShopItems();
        }
        
        // ПЕРЕОПРЕДЕЛЯЕМ метод для конкретной логики CardShopView
        protected override void OnBackButtonClicked()
        {
            OnBackRequested?.Invoke();
        }
        
        private void LoadShopItems()
        {
            _shopService.GetShopItems(ShopCategory.Cards,
                items => {
                    DisplayItems(items);
                },
                error => {
                    ShowError($"Failed to load shop items: {error}");
                });
        }
        
        private void DisplayItems(List<ShopItem> items)
        {
            ClearItems();
            
            foreach (var item in items)
            {
                var itemObject = Instantiate(_itemPrefab, _itemsContainer);
                var shopItemElement = itemObject.GetComponent<ShopItemElement>();
                shopItemElement.Initialize(item, OnPurchaseItem);
                _itemElements.Add(shopItemElement);
            }
        }
        
        private void ClearItems()
        {
            foreach (var element in _itemElements)
            {
                Destroy(element.gameObject);
            }
            _itemElements.Clear();
        }
        
        private void OnPurchaseItem(ShopItem item)
        {
            EventSystem.PurchaseShopItem(item);
        }
    }
}