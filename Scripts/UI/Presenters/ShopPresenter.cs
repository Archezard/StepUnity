using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Views;
using System.Collections.Generic;
using UnityEngine;

namespace BasketballCards.UI.Presenters
{
    public class ShopPresenter : BasePresenter
    {
        [Header("View References")]
        [SerializeField] private ShopView _shopView;
        [SerializeField] private CardShopView _cardShopView;
        [SerializeField] private ThrowShopView _throwShopView;
        [SerializeField] private PackShopView _packShopView;
        [SerializeField] private CurrencyShopView _currencyShopView;
        
        private List<ShopViewBase> _allViews = new List<ShopViewBase>();
        private ShopViewBase _currentView;
        
        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            
            EventSystem.OnShopItemPurchased += HandleShopItemPurchased;
            EventSystem.OnShopCategoryChanged += HandleShopCategoryChanged;
            EventSystem.OnErrorOccurred += HandleError;
            
            // Подписка на события данных пользователя
            UserDataManager.Instance.OnUserDataUpdated += HandleUserDataUpdated;
            UserDataManager.Instance.OnCurrencyChanged += HandleCurrencyChanged;
            UserDataManager.Instance.OnDiamondsChanged += HandleDiamondsChanged;
        }
        
        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            
            EventSystem.OnShopItemPurchased -= HandleShopItemPurchased;
            EventSystem.OnShopCategoryChanged -= HandleShopCategoryChanged;
            EventSystem.OnErrorOccurred -= HandleError;
            
            if (UserDataManager.Instance != null)
            {
                UserDataManager.Instance.OnUserDataUpdated -= HandleUserDataUpdated;
                UserDataManager.Instance.OnCurrencyChanged -= HandleCurrencyChanged;
                UserDataManager.Instance.OnDiamondsChanged -= HandleDiamondsChanged;
            }
        }
        
        private void Start()
        {
            // Собираем все View
            _allViews.Add(_shopView);
            _allViews.Add(_cardShopView);
            _allViews.Add(_throwShopView);
            _allViews.Add(_packShopView);
            _allViews.Add(_currencyShopView);
            
            // Инициализация View
            InitializeViews();
            
            // Скрываем все View при старте
            HideAllViews();
        }
        
        private void InitializeViews()
        {
            // Инициализация основных View
            if (_shopView != null)
            {
                _shopView.Initialize();
                _shopView.OnCardShopSelected += () => ShowSubView(_cardShopView);
                _shopView.OnThrowShopSelected += () => ShowSubView(_throwShopView);
                _shopView.OnPackShopSelected += () => ShowSubView(_packShopView);
                _shopView.OnCurrencyShopSelected += () => ShowSubView(_currencyShopView);
            }
            
            // Инициализация под-View
            if (_cardShopView != null)
            {
                _cardShopView.Initialize(AppCoordinator.Instance.ShopService);
                _cardShopView.OnBackRequested += () => ShowSubView(_shopView);
            }
            
            if (_throwShopView != null)
            {
                _throwShopView.Initialize(AppCoordinator.Instance.ShopService);
                _throwShopView.OnBackRequested += () => ShowSubView(_shopView);
            }
            
            if (_packShopView != null)
            {
                _packShopView.Initialize(AppCoordinator.Instance.ShopService);
                _packShopView.OnBackRequested += () => ShowSubView(_shopView);
            }
            
            if (_currencyShopView != null)
            {
                _currencyShopView.Initialize(AppCoordinator.Instance.ShopService);
                _currencyShopView.OnBackRequested += () => ShowSubView(_shopView);
            }
        }
        
        public override void Show()
        {
            ShowSubView(_shopView);
        }
        
        public override void Hide()
        {
            HideAllViews();
        }
        
        private void ShowSubView(ShopViewBase view)
        {
            if (view == null) return;
            
            // Скрываем текущее View
            if (_currentView != null)
            {
                _currentView.Hide();
            }
            
            // Показываем новое View
            _currentView = view;
            _currentView.Show();
        }
        
        private void HideAllViews()
        {
            foreach (var view in _allViews)
            {
                if (view != null)
                {
                    view.Hide();
                }
            }
            _currentView = null;
        }
        
        protected override void HandleUserDataUpdated(UserData userData)
        {
            // Обновление UI при изменении данных пользователя
            Debug.Log($"ShopPresenter: User data updated - Gold: {userData.gold}, Diamonds: {userData.diamonds}");
        }
        
        private void HandleShopItemPurchased(ShopItem item)
        {
            // Покупка товара через сервис
            AppCoordinator.Instance.ShopService.PurchaseItem(item.Id,
                result => {
                    if (result.Success)
                    {
                        EventSystem.ShowSuccess($"Успешно куплено: {item.Name}");
                        
                        // Обновляем данные пользователя
                        AppCoordinator.Instance.UserService.GetUserData(
                            UserDataManager.Instance.CurrentUser.username,
                            userData => UserDataManager.Instance.UpdateUserData(userData),
                            error => EventSystem.ShowError("Failed to update user data")
                        );
                    }
                    else
                    {
                        EventSystem.ShowError($"Purchase failed: {result.Message}");
                    }
                },
                error => {
                    EventSystem.ShowError($"Purchase error: {error}");
                });
        }
        
        private void HandleShopCategoryChanged(ShopCategory category)
        {
            // Переключение категорий магазина
            switch (category)
            {
                case ShopCategory.Cards:
                    ShowSubView(_cardShopView);
                    break;
                case ShopCategory.Throws:
                    ShowSubView(_throwShopView);
                    break;
                case ShopCategory.Packs:
                    ShowSubView(_packShopView);
                    break;
                case ShopCategory.Currency:
                    ShowSubView(_currencyShopView);
                    break;
            }
        }
        
        private void HandleCurrencyChanged(int oldGold, int newGold)
        {
            // Обновление UI при изменении золота
        }
        
        private void HandleDiamondsChanged(int oldDiamonds, int newDiamonds)
        {
            // Обновление UI при изменении алмазов
        }
        
        private void HandleError(string error)
        {
            // Обработка ошибок, специфичных для магазина
            if (_currentView != null)
            {
                _currentView.ShowError(error);
            }
        }
    }
}