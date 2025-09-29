using BasketballCards.Core;
using BasketballCards.Services;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI
{
    public class NavigationButtonHandler : MonoBehaviour
    {
        [Header("Button Configuration")]
        [SerializeField] private AppScreen _targetScreen;
        [SerializeField] private ShopCategory _shopCategory; // Если кнопка для магазина
        
        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            if (_button != null)
            {
                _button.onClick.AddListener(OnButtonClicked);
            }
        }
        
        private void OnButtonClicked()
        {
            EventSystem.NavigateTo(_targetScreen);
            
            // Если это кнопка для конкретной категории магазина
            if (_targetScreen == AppScreen.Shop && _shopCategory != ShopCategory.Cards)
            {
                EventSystem.ChangeShopCategory(_shopCategory);
            }
        }
        
        private void OnDestroy()
        {
            if (_button != null)
            {
                _button.onClick.RemoveListener(OnButtonClicked);
            }
        }
    }
}