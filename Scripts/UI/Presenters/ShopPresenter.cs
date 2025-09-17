using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Views;
using UnityEngine;

namespace BasketballCards.UI.Presenters
{
    public class ShopPresenter : MonoBehaviour
    {
        [Header("View References")]
        [SerializeField] private ShopView _shopView;
        [SerializeField] private CardShopView _cardShopView;
        [SerializeField] private ThrowShopView _throwShopView;
        [SerializeField] private PackShopView _packShopView;
        [SerializeField] private CurrencyShopView _currencyShopView;
        
        private GameManager _gameManager;
        private ShopService _shopService;
        
        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            _shopService = _gameManager.ShopService;
            
            // Инициализация View
            _shopView.Initialize(this);
            _cardShopView.Initialize(this, _shopService);
            //_throwShopView.Initialize(this, _shopService);
            //_packShopView.Initialize(this, _shopService);
            //_currencyShopView.Initialize(this, _shopService);
            
            // Скрываем все подразделы, показываем только основной
            HideAllSubsections();
            _shopView.Show();
            
            Debug.Log("ShopPresenter: Initialized");
        }
        
        public void ShowShop()
        {
            HideAllSubsections();
            _shopView.Show();
        }
        
        public void ShowCardShop()
        {
            HideAllSubsections();
            _cardShopView.Show();
        }
        
        public void ShowThrowShop()
        {
            HideAllSubsections();
            _throwShopView.Show();
        }
        
        public void ShowPackShop()
        {
            HideAllSubsections();
            _packShopView.Show();
        }
        
        public void ShowCurrencyShop()
        {
            HideAllSubsections();
            _currencyShopView.Show();
        }
        
        private void HideAllSubsections()
        {
            _shopView.Hide();
            _cardShopView.Hide();
            _throwShopView.Hide();
            _packShopView.Hide();
            _currencyShopView.Hide();
        }
        
        public void OnPurchaseSuccess()
        {
            // Обновляем данные пользователя после покупки
            _gameManager.UserService.GetUserData(_gameManager.CurrentUser.username, 
                userData => {
                    _gameManager.SetCurrentUser(userData);
                },
                error => {
                    Debug.LogError("Failed to update user data: " + error);
                });
        }
    }
}