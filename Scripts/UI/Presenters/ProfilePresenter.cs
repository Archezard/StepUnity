using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Views;
using System.Collections.Generic;
using UnityEngine;

namespace BasketballCards.UI.Presenters
{
    public class ProfilePresenter : BasePresenter
    {
        [Header("View References")]
        [SerializeField] private ProfileView _profileView;
        //[SerializeField] private FriendsView _friendsView;
        //[SerializeField] private SettingsView _settingsView;
        
        private List<BaseView> _allViews = new List<BaseView>();
        private BaseView _currentView;
        
        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            
            // Подписка на события данных пользователя
            UserDataManager.Instance.OnUserDataUpdated += HandleUserDataUpdated;
            UserDataManager.Instance.OnCurrencyChanged += HandleCurrencyChanged;
            UserDataManager.Instance.OnDiamondsChanged += HandleDiamondsChanged;
            UserDataManager.Instance.OnTicketsChanged += HandleTicketsChanged;
        }
        
        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            
            if (UserDataManager.Instance != null)
            {
                UserDataManager.Instance.OnUserDataUpdated -= HandleUserDataUpdated;
                UserDataManager.Instance.OnCurrencyChanged -= HandleCurrencyChanged;
                UserDataManager.Instance.OnDiamondsChanged -= HandleDiamondsChanged;
                UserDataManager.Instance.OnTicketsChanged -= HandleTicketsChanged;
            }
        }
        
        private void Start()
        {
            // Собираем все View
            _allViews.Add(_profileView);
            //_allViews.Add(_friendsView);
            //_allViews.Add(_settingsView);
            
            // Инициализация View
            InitializeViews();
            
            // Скрываем все View при старте
            HideAllViews();
        }
        
        private void InitializeViews()
        {
            if (_profileView != null)
            {
                _profileView.Initialize();
                // Подписка на события ProfileView, если они появятся
            }
            
            // Инициализация других View когда они будут реализованы
            /*
            if (_friendsView != null)
            {
                _friendsView.Initialize();
                _friendsView.OnBackRequested += () => ShowSubView(_profileView);
            }
            
            if (_settingsView != null)
            {
                _settingsView.Initialize();
                _settingsView.OnBackRequested += () => ShowSubView(_profileView);
            }
            */
        }
        
        public override void Show()
        {
            ShowSubView(_profileView);
            
            // Обновляем данные при показе
            UpdateProfileData();
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
            UpdateProfileData();
        }
        
        private void UpdateProfileData()
        {
            if (_profileView != null && UserDataManager.Instance.CurrentUser != null)
            {
                _profileView.DisplayProfile(UserDataManager.Instance.CurrentUser);
            }
        }
        
        private void HandleCurrencyChanged(int oldGold, int newGold)
        {
            // Обновление UI при изменении золота
            UpdateProfileData();
        }
        
        private void HandleDiamondsChanged(int oldDiamonds, int newDiamonds)
        {
            // Обновление UI при изменении алмазов
            UpdateProfileData();
        }
        
        private void HandleTicketsChanged(int oldTickets, int newTickets)
        {
            // Обновление UI при изменении билетов
            UpdateProfileData();
        }
        
        // Методы для навигации между под-разделами профиля
        public void ShowFriends()
        {
            //HideAllSubsections();
            //_friendsView.Show();
        }
        
        public void ShowSettings()
        {
            //HideAllSubsections();
            //_settingsView.Show();
        }
    }
}