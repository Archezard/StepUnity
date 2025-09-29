using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Views;
using System.Collections.Generic;
using UnityEngine;

namespace BasketballCards.UI.Presenters
{
    public class ActivitiesPresenter : BasePresenter
    {
        [Header("Header Reference")]
        [SerializeField] private ActivitiesHeaderView _headerView;
        
        [Header("SubScreen Views")]
        [SerializeField] private GetCardView _getCardView;
        [SerializeField] private ThrowBallView _throwBallView;
        [SerializeField] private OpenPackView _openPackView;
        
        private List<BaseView> _subViews = new List<BaseView>();
        private BaseView _currentSubView;
        private ActivitiesSubScreen _currentSubScreen = ActivitiesSubScreen.GetCard;
        
        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            
            EventSystem.OnActivitiesSubScreenChanged += HandleSubScreenChanged;
            EventSystem.OnFreeCardRequested += HandleFreeCardRequested;
            EventSystem.OnBallThrowRequested += HandleBallThrowRequested;
            EventSystem.OnPackOpenRequested += HandlePackOpenRequested;
            EventSystem.OnCardReceived += HandleCardReceived;
            EventSystem.OnErrorOccurred += HandleError;
            
            if (UserDataManager.Instance != null)
            {
                UserDataManager.Instance.OnUserDataUpdated += HandleUserDataUpdated;
                UserDataManager.Instance.OnCurrencyChanged += HandleCurrencyChanged;
                UserDataManager.Instance.OnTicketsChanged += HandleTicketsChanged;
            }
            else
            {
                Debug.LogWarning("ActivitiesPresenter: UserDataManager.Instance is null during subscription");
            }
        }
        
        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            
            EventSystem.OnActivitiesSubScreenChanged -= HandleSubScreenChanged;
            EventSystem.OnFreeCardRequested -= HandleFreeCardRequested;
            EventSystem.OnBallThrowRequested -= HandleBallThrowRequested;
            EventSystem.OnPackOpenRequested -= HandlePackOpenRequested;
            EventSystem.OnCardReceived -= HandleCardReceived;
            EventSystem.OnErrorOccurred -= HandleError;
            
            if (UserDataManager.Instance != null)
            {
                UserDataManager.Instance.OnUserDataUpdated -= HandleUserDataUpdated;
                UserDataManager.Instance.OnCurrencyChanged -= HandleCurrencyChanged;
                UserDataManager.Instance.OnTicketsChanged -= HandleTicketsChanged;
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
                Debug.LogError("ActivitiesPresenter: HeaderView reference is null!");
            }
        }
        
        private void InitializeSubViews()
        {
            // Все подпредставления
            if (_getCardView != null)
            {
                _getCardView.Initialize(AppCoordinator.Instance?.ActivitiesService);
                _subViews.Add(_getCardView);
            }
            
            if (_throwBallView != null)
            {
                _throwBallView.Initialize(AppCoordinator.Instance?.ActivitiesService);
                _subViews.Add(_throwBallView);
            }
            
            if (_openPackView != null)
            {
                _openPackView.Initialize(AppCoordinator.Instance?.ActivitiesService);
                _subViews.Add(_openPackView);
            }
            
            // Показываем начальное подпредставление
            ShowSubView(GetViewForSubScreen(_currentSubScreen));
        }
        
        public override void Show()
        {
            // Хедер
            if (_headerView != null)
                _headerView.gameObject.SetActive(true);
            
            // Показываем текущее подпредставление
            if (_currentSubView != null)
                _currentSubView.Show();
        }
        
        public override void Hide()
        {
            // Скрываем хедер
            if (_headerView != null)
                _headerView.gameObject.SetActive(false);
            
            // Скрываем все подпредставления
            foreach (var view in _subViews)
            {
                if (view != null)
                    view.Hide();
            }
        }
        
        private void HandleHeaderSubScreenSelected(ActivitiesSubScreen subScreen)
        {
            EventSystem.ChangeActivitiesSubScreen(subScreen);
        }
        
        private void HandleSubScreenChanged(ActivitiesSubScreen subScreen)
        {
            _currentSubScreen = subScreen;
            var targetView = GetViewForSubScreen(subScreen);
            ShowSubView(targetView);
        }
        
        private void ShowSubView(BaseView view)
        {
            if (view == null) return;
            
            // Скрываем текущее подпредставление
            if (_currentSubView != null)
            {
                _currentSubView.Hide();
            }
            
            // Показываем новое подпредставление
            _currentSubView = view;
            _currentSubView.Show();
        }
        
        private BaseView GetViewForSubScreen(ActivitiesSubScreen subScreen)
        {
            switch (subScreen)
            {
                case ActivitiesSubScreen.GetCard: return _getCardView;
                case ActivitiesSubScreen.ThrowBall: return _throwBallView;
                case ActivitiesSubScreen.OpenPack: return _openPackView;
                default: return _getCardView;
            }
        }
        
        protected override void HandleUserDataUpdated(UserData userData)
        {
            // Обновление UI на основе новых данных пользователя
            Debug.Log($"ActivitiesPresenter: User data updated - Gold: {userData.gold}, Diamonds: {userData.diamonds}, Tickets: {userData.tickets}");
        }
        
        private void HandleFreeCardRequested()
        {
            var activitiesService = AppCoordinator.Instance?.ActivitiesService;
            if (activitiesService == null)
            {
                Debug.LogError("ActivitiesPresenter: ActivitiesService is not available");
                return;
            }
            
            activitiesService.GetFreeCard(
                card => {
                    EventSystem.ReceiveCard(card);
                    EventSystem.ShowSuccess($"Получена карта: {card.PlayerName}");
                    
                    // Обновляем данные пользователя
                    UpdateUserData();
                },
                error => {
                    EventSystem.ShowError($"Failed to get free card: {error}");
                });
        }
        
        private void HandleBallThrowRequested()
        {
            var activitiesService = AppCoordinator.Instance?.ActivitiesService;
            if (activitiesService == null)
            {
                Debug.LogError("ActivitiesPresenter: ActivitiesService is not available");
                return;
            }
            
            activitiesService.ThrowBall(
                (score, rewards) => {
                    // Показываем результат в ThrowBallView
                    if (_currentSubView is ThrowBallView throwBallView)
                    {
                        throwBallView.ShowThrowResult(score, rewards);
                    }
                    
                    if (score > 0)
                    {
                        EventSystem.ShowSuccess($"Попадание! Очков: {score}");
                    }
                    else
                    {
                        EventSystem.ShowError("Промах!");
                    }
                    
                    UpdateUserData();
                },
                error => {
                    EventSystem.ShowError($"Throw failed: {error}");
                });
        }
        
        private void HandlePackOpenRequested(string packId)
        {
            var activitiesService = AppCoordinator.Instance?.ActivitiesService;
            if (activitiesService == null)
            {
                Debug.LogError("ActivitiesPresenter: ActivitiesService is not available");
                return;
            }
            
            activitiesService.OpenPack(packId,
                cards => {
                    // Показываем карты в OpenPackView
                    if (_currentSubView is OpenPackView openPackView)
                    {
                        openPackView.DisplayCards(cards);
                    }
                    
                    EventSystem.ShowSuccess($"Открыт пак! Получено карт: {cards.Count}");
                    
                    UpdateUserData();
                },
                error => {
                    EventSystem.ShowError($"Failed to open pack: {error}");
                });
        }
        
        private void HandleCardReceived(CardData card)
        {
            // Логика обработки полученной карты
            if (_currentSubView is GetCardView getCardView)
            {
                getCardView.DisplayCard(card);
            }
        }
        
        private void UpdateUserData()
        {
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
        
        private void HandleCurrencyChanged(int oldGold, int newGold)
        {
            // Обновление UI при изменении валюты
        }
        
        private void HandleTicketsChanged(int oldTickets, int newTickets)
        {
            // Обновление UI при изменении билетов
        }
        
        private void HandleError(string error)
        {
            // Обработка ошибок, специфичных для активностей
            if (_currentSubView != null)
            {
                _currentSubView.ShowError(error);
            }
        }
    }
}