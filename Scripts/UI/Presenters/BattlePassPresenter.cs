using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Views;
using System.Collections.Generic;
using UnityEngine;

namespace BasketballCards.UI.Presenters
{
    public class BattlePassPresenter : BasePresenter
    {
        [Header("View References")]
        [SerializeField] private BattlePassView _battlePassView;
        [SerializeField] private TasksView _tasksView;
        [SerializeField] private RewardsView _rewardsView;
        
        private List<BaseView> _allViews = new List<BaseView>();
        private BaseView _currentView;
        private BattlePassProgress _currentProgress;
        
        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            
            EventSystem.OnBattlePassRewardClaimed += HandleRewardClaimed;
            EventSystem.OnBattlePassPremiumPurchased += HandlePremiumPurchased;
            EventSystem.OnErrorOccurred += HandleError;
            
            // Подписка на события данных пользователя
            UserDataManager.Instance.OnUserDataUpdated += HandleUserDataUpdated;
            UserDataManager.Instance.OnCurrencyChanged += HandleCurrencyChanged;
            UserDataManager.Instance.OnDiamondsChanged += HandleDiamondsChanged;
        }
        
        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            
            EventSystem.OnBattlePassRewardClaimed -= HandleRewardClaimed;
            EventSystem.OnBattlePassPremiumPurchased -= HandlePremiumPurchased;
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
            _allViews.Add(_battlePassView);
            _allViews.Add(_tasksView);
            _allViews.Add(_rewardsView);
            
            // Инициализация View
            InitializeViews();
            
            // Скрываем все View при старте
            HideAllViews();
            
            // Загрузка прогресса
            LoadBattlePassProgress();
        }
        
        private void InitializeViews()
        {
            if (_battlePassView != null)
            {
                _battlePassView.Initialize();
                _battlePassView.OnTasksSelected += () => ShowSubView(_tasksView);
                _battlePassView.OnRewardsSelected += () => ShowSubView(_rewardsView);
                _battlePassView.OnPremiumPurchaseSelected += OnPremiumPurchaseRequested;
            }
            
            if (_tasksView != null)
            {
                _tasksView.Initialize(AppCoordinator.Instance.BattlePassService);
                _tasksView.OnBackRequested += () => ShowSubView(_battlePassView);
            }
            
            if (_rewardsView != null)
            {
                _rewardsView.Initialize(AppCoordinator.Instance.BattlePassService);
                _rewardsView.OnBackRequested += () => ShowSubView(_battlePassView);
                _rewardsView.OnRewardClaimed += OnRewardClaimRequested;
            }
        }
        
        public override void Show()
        {
            ShowSubView(_battlePassView);
            
            // Обновляем данные при показе
            LoadBattlePassProgress();
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
            
            // Если показываем rewards view, обновляем данные
            if (view is RewardsView rewardsView && _currentProgress != null)
            {
                rewardsView.DisplayRewards(_currentProgress);
            }
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
            // При обновлении данных пользователя перезагружаем прогресс баттл-пасса
            LoadBattlePassProgress();
        }
        
        private void LoadBattlePassProgress()
        {
            AppCoordinator.Instance.BattlePassService.GetBattlePassProgress(
                progress => {
                    _currentProgress = progress;
                    UpdateBattlePassUI();
                },
                error => {
                    EventSystem.ShowError($"Failed to load battle pass progress: {error}");
                });
        }
        
        private void UpdateBattlePassUI()
        {
            if (_battlePassView != null && _currentProgress != null)
            {
                _battlePassView.DisplayProgress(_currentProgress);
            }
            
            if (_rewardsView != null && _currentProgress != null)
            {
                _rewardsView.DisplayRewards(_currentProgress);
            }
        }
        
        private void OnRewardClaimRequested(int level, bool isPremium)
        {
            EventSystem.ClaimBattlePassReward(level, isPremium);
        }
        
        private void OnPremiumPurchaseRequested()
        {
            EventSystem.PurchaseBattlePassPremium();
        }
        
        private void HandleRewardClaimed(int level, bool isPremium)
        {
            AppCoordinator.Instance.BattlePassService.ClaimReward(level, isPremium,
                reward => {
                    EventSystem.ShowSuccess($"Награда уровня {level} получена!");
                    
                    // Обновляем данные пользователя
                    AppCoordinator.Instance.UserService.GetUserData(
                        UserDataManager.Instance.CurrentUser.username,
                        userData => UserDataManager.Instance.UpdateUserData(userData),
                        error => EventSystem.ShowError("Failed to update user data")
                    );
                    
                    // Перезагружаем прогресс
                    LoadBattlePassProgress();
                },
                error => {
                    EventSystem.ShowError($"Failed to claim reward: {error}");
                });
        }
        
        private void HandlePremiumPurchased()
        {
            AppCoordinator.Instance.BattlePassService.PurchasePremium(
                success => {
                    if (success)
                    {
                        EventSystem.ShowSuccess("Премиум баттл-пасс активирован!");
                        LoadBattlePassProgress(); // Перезагружаем прогресс
                    }
                    else
                    {
                        EventSystem.ShowError("Failed to purchase premium battle pass");
                    }
                },
                error => {
                    EventSystem.ShowError($"Failed to purchase premium battle pass: {error}");
                });
        }
        
        private void HandleCurrencyChanged(int oldGold, int newGold)
        {
            // Обновление UI при изменении валюты
        }
        
        private void HandleDiamondsChanged(int oldDiamonds, int newDiamonds)
        {
            // Обновление UI при изменении алмазов
        }
        
        private void HandleError(string error)
        {
            // Обработка ошибок, специфичных для баттл-пасса
            if (_currentView != null)
            {
                _currentView.ShowError(error);
            }
        }
    }
}