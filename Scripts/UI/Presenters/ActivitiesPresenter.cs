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
        [Header("View References")]
        [SerializeField] private ActivitiesView _activitiesView;
        [SerializeField] private GetCardView _getCardView;
        [SerializeField] private ThrowBallView _throwBallView;
        [SerializeField] private OpenPackView _openPackView;
        
        private List<BaseView> _allViews = new List<BaseView>();
        private BaseView _currentView;
        
        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            
            EventSystem.OnFreeCardRequested += HandleFreeCardRequested;
            EventSystem.OnBallThrowRequested += HandleBallThrowRequested;
            EventSystem.OnPackOpenRequested += HandlePackOpenRequested;
            EventSystem.OnCardReceived += HandleCardReceived;
            EventSystem.OnErrorOccurred += HandleError;
            
            // Подписка на события данных пользователя
            UserDataManager.Instance.OnUserDataUpdated += HandleUserDataUpdated;
            UserDataManager.Instance.OnCurrencyChanged += HandleCurrencyChanged;
            UserDataManager.Instance.OnTicketsChanged += HandleTicketsChanged;
        }
        
        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            
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
            // Собираем все View
            _allViews.Add(_activitiesView);
            _allViews.Add(_getCardView);
            _allViews.Add(_throwBallView);
            _allViews.Add(_openPackView);
            
            // Инициализация View
            InitializeViews();
            
            // Скрываем все View при старте
            HideAllViews();
        }
        
        private void InitializeViews()
        {
            if (_activitiesView != null)
            {
                _activitiesView.Initialize();
                _activitiesView.OnGetCardSelected += () => ShowSubView(_getCardView);
                _activitiesView.OnThrowBallSelected += () => ShowSubView(_throwBallView);
                _activitiesView.OnOpenPackSelected += () => ShowSubView(_openPackView);
            }
            
            if (_getCardView != null)
            {
                _getCardView.Initialize(AppCoordinator.Instance.ActivitiesService);
                _getCardView.OnBackRequested += () => ShowSubView(_activitiesView);
            }
            
            if (_throwBallView != null)
            {
                _throwBallView.Initialize(AppCoordinator.Instance.ActivitiesService);
                _throwBallView.OnBackRequested += () => ShowSubView(_activitiesView);
            }
            
            if (_openPackView != null)
            {
                _openPackView.Initialize(AppCoordinator.Instance.ActivitiesService);
                _openPackView.OnBackRequested += () => ShowSubView(_activitiesView);
            }
        }
        
        public override void Show()
        {
            ShowSubView(_activitiesView);
        }
        
        public override void Hide()
        {
            HideAllViews();
        }
        
        private void ShowSubView(BaseView view)
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
            // Обновление UI на основе новых данных пользователя
            Debug.Log($"ActivitiesPresenter: User data updated - Gold: {userData.gold}, Diamonds: {userData.diamonds}, Tickets: {userData.tickets}");
        }
        
        private void HandleFreeCardRequested()
        {
            AppCoordinator.Instance.ActivitiesService.GetFreeCard(
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
            AppCoordinator.Instance.ActivitiesService.ThrowBall(
                (score, rewards) => {
                    // Показываем результат в ThrowBallView
                    if (_currentView is ThrowBallView throwBallView)
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
                    
                    // Обновляем данные пользователя
                    UpdateUserData();
                },
                error => {
                    EventSystem.ShowError($"Throw failed: {error}");
                });
        }
        
        private void HandlePackOpenRequested(string packId)
        {
            AppCoordinator.Instance.ActivitiesService.OpenPack(packId,
                cards => {
                    // Показываем карты в OpenPackView
                    if (_currentView is OpenPackView openPackView)
                    {
                        openPackView.DisplayCards(cards);
                    }
                    
                    EventSystem.ShowSuccess($"Открыт пак! Получено карт: {cards.Count}");
                    
                    // Обновляем данные пользователя
                    UpdateUserData();
                },
                error => {
                    EventSystem.ShowError($"Failed to open pack: {error}");
                });
        }
        
        private void HandleCardReceived(CardData card)
        {
            // Логика обработки полученной карты
            if (_currentView is GetCardView getCardView)
            {
                getCardView.DisplayCard(card);
            }
        }
        
        private void UpdateUserData()
        {
            AppCoordinator.Instance.UserService.GetUserData(
                UserDataManager.Instance.CurrentUser.username,
                userData => UserDataManager.Instance.UpdateUserData(userData),
                error => EventSystem.ShowError("Failed to update user data")
            );
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
            if (_currentView != null)
            {
                _currentView.ShowError(error);
            }
        }
    }
}