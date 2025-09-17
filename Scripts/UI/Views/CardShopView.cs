using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Presenters;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class CardShopView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button _backButton;
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private ScrollRect _scrollRect;
        
        private ShopPresenter _presenter;
        private ShopService _shopService;
        
        public void Initialize(ShopPresenter presenter, ShopService shopService)
        {
            _presenter = presenter;
            _shopService = shopService;
            
            _backButton.onClick.AddListener(OnBackButtonClicked);
            
            LoadShopItems();
        }
        
        private void LoadShopItems()
        {
            _shopService.GetShopItems(ShopCategory.Cards,
                items => {
                    DisplayItems(items);
                },
                error => {
                    Debug.LogError("Failed to load shop items: " + error);
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
            }
        }
        
        private void ClearItems()
        {
            foreach (Transform child in _itemsContainer)
            {
                Destroy(child.gameObject);
            }
        }
        
        private void OnPurchaseItem(ShopItem item)
        {
            _shopService.PurchaseItem(item.Id,
                result => {
                    if (result.Success)
                    {
                        Debug.Log("Purchase successful: " + result.Message);
                        _presenter.OnPurchaseSuccess();
                    }
                    else
                    {
                        Debug.LogError("Purchase failed: " + result.Message);
                    }
                },
                error => {
                    Debug.LogError("Purchase error: " + error);
                });
        }
        
        private void OnBackButtonClicked()
        {
            _presenter.ShowShop();
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}