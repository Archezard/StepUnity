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
        [Header("Header Reference")]
        [SerializeField] private ShopHeaderView _headerView;
        
        [Header("SubScreen Views")]
        [SerializeField] private CardShopView _cardShopView;
        [SerializeField] private ThrowShopView _throwShopView;
        [SerializeField] private PackShopView _packShopView;
        [SerializeField] private CurrencyShopView _currencyShopView;
        
        private List<ShopViewBase> _subViews = new List<ShopViewBase>();
        private ShopViewBase _currentSubView;
        private ShopSubScreen _currentSubScreen = ShopSubScreen.Cards;
        
        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            
            EventSystem.OnShopSubScreenChanged += HandleSubScreenChanged;
            EventSystem.OnShopItemPurchased += HandleShopItemPurchased;
            EventSystem.OnErrorOccurred += HandleError;
            
            if (UserDataManager.Instance != null)
            {
                UserDataManager.Instance.OnUserDataUpdated += HandleUserDataUpdated;
                UserDataManager.Instance.OnCurrencyChanged += HandleCurrencyChanged;
                UserDataManager.Instance.OnDiamondsChanged += HandleDiamondsChanged;
            }
            else
            {
                Debug.LogWarning("ShopPresenter: UserDataManager.Instance is null during subscription");
            }
        }
        
        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            
            EventSystem.OnShopSubScreenChanged -= HandleSubScreenChanged;
            EventSystem.OnShopItemPurchased -= HandleShopItemPurchased;
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
            InitializeHeader();
            InitializeSubViews();
        }
        
        private void InitializeHeader()
        {
            if (_headerView != null)
            {
                _headerView.Initialize();
                _headerView.OnSubScreenSelected += HandleHeaderSubScreenSelected;
            }
            else
            {
                Debug.LogError("ShopPresenter: HeaderView reference is null!");
            }
        }
        
        private void InitializeSubViews()
        {
            // Собираем все под-представления
            if (_cardShopView != null)
            {
                _cardShopView.Initialize(AppCoordinator.Instance?.ShopService);
                _subViews.Add(_cardShopView);
            }
            
            if (_throwShopView != null)
            {
                _throwShopView.Initialize(AppCoordinator.Instance?.ShopService);
                _subViews.Add(_throwShopView);
            }
            
            if (_packShopView != null)
            {
                _packShopView.Initialize(AppCoordinator.Instance?.ShopService);
                _subViews.Add(_packShopView);
            }
            
            if (_currencyShopView != null)
            {
                _currencyShopView.Initialize(AppCoordinator.Instance?.ShopService);
                _subViews.Add(_currencyShopView);
            }
            
            // Показываем начальное под-представление
            ShowSubView(GetViewForSubScreen(_currentSubScreen));
        }
        
        public override void Show()
        {
            // Показываем хедер
            if (_headerView != null)
                _headerView.gameObject.SetActive(true);
            
            // Показываем текущее под-представление
            if (_currentSubView != null)
                _currentSubView.Show();
        }
        
        public override void Hide()
        {
            // Скрываем хедер
            if (_headerView != null)
                _headerView.gameObject.SetActive(false);
            
            // Скрываем все под-представления
            foreach (var view in _subViews)
            {
                if (view != null)
                    view.Hide();
            }
        }
        
        private void HandleHeaderSubScreenSelected(ShopSubScreen subScreen)
        {
            EventSystem.ChangeShopSubScreen(subScreen);
        }
        
        private void HandleSubScreenChanged(ShopSubScreen subScreen)
        {
            _currentSubScreen = subScreen;
            var targetView = GetViewForSubScreen(subScreen);
            ShowSubView(targetView);
        }
        
        private void ShowSubView(ShopViewBase view)
        {
            if (view == null) return;
            
            // Скрываем текущее под-представление
            if (_currentSubView != null)
            {
                _currentSubView.Hide();
            }
            
            // Показываем новое под-представление
            _currentSubView = view;
            _currentSubView.Show();
        }
        
        private ShopViewBase GetViewForSubScreen(ShopSubScreen subScreen)
        {
            switch (subScreen)
            {
                case ShopSubScreen.Cards: return _cardShopView;
                case ShopSubScreen.Throws: return _throwShopView;
                case ShopSubScreen.Packs: return _packShopView;
                case ShopSubScreen.Currency: return _currencyShopView;
                default: return _cardShopView;
            }
        }
        
        protected override void HandleUserDataUpdated(UserData userData)
        {
            // Обновление UI при изменении данных пользователя
            Debug.Log($"ShopPresenter: User data updated - Gold: {userData.gold}, Diamonds: {userData.diamonds}");
        }
        
        private void HandleShopItemPurchased(ShopItem item)
        {
            var shopService = AppCoordinator.Instance?.ShopService;
            if (shopService == null)
            {
                Debug.LogError("ShopPresenter: ShopService is not available");
                return;
            }
            
            // Покупка товара через сервис
            shopService.PurchaseItem(item.Id,
                result => {
                    if (result.Success)
                    {
                        EventSystem.ShowSuccess($"Успешно куплено: {item.Name}");
                        
                        // Обновляем данные пользователя
                        var userService = AppCoordinator.Instance?.UserService;
                        if (userService != null && UserDataManager.Instance != null)
                        {
                            userService.GetUserData(
                                UserDataManager.Instance.CurrentUser.username,
                                userData => UserDataManager.Instance.UpdateUserData(userData),
                                error => EventSystem.ShowError("Failed to update user data")
                            );
                        }
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
            if (_currentSubView != null)
            {
                _currentSubView.ShowError(error);
            }
        }
    }
}