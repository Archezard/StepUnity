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
        [Header("Header Reference")]
        [SerializeField] private BattlePassHeaderView _headerView;
        
        [Header("SubScreen Views")]
        [SerializeField] private TasksView _tasksView;
        [SerializeField] private RewardsView _rewardsView;
        
        private List<BaseView> _subViews = new List<BaseView>();
        private BaseView _currentSubView;
        private BattlePassSubScreen _currentSubScreen = BattlePassSubScreen.Tasks;
        private BattlePassProgress _currentProgress;
        
        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            
            EventSystem.OnBattlePassSubScreenChanged += HandleSubScreenChanged;
            EventSystem.OnBattlePassRewardClaimed += HandleRewardClaimed;
            EventSystem.OnBattlePassPremiumPurchased += HandlePremiumPurchased;
            EventSystem.OnErrorOccurred += HandleError;
            
            // ИСПРАВЛЕНИЕ: Добавляем проверку на null для UserDataManager
            if (UserDataManager.Instance != null)
            {
                UserDataManager.Instance.OnUserDataUpdated += HandleUserDataUpdated;
                UserDataManager.Instance.OnCurrencyChanged += HandleCurrencyChanged;
                UserDataManager.Instance.OnDiamondsChanged += HandleDiamondsChanged;
            }
            else
            {
                Debug.LogWarning("BattlePassPresenter: UserDataManager.Instance is null during subscription");
            }
        }
        
        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            
            EventSystem.OnBattlePassSubScreenChanged -= HandleSubScreenChanged;
            EventSystem.OnBattlePassRewardClaimed -= HandleRewardClaimed;
            EventSystem.OnBattlePassPremiumPurchased -= HandlePremiumPurchased;
            EventSystem.OnErrorOccurred -= HandleError;
            
            // ИСПРАВЛЕНИЕ: Добавляем проверку на null для UserDataManager
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
            
            // Загрузка прогресса
            LoadBattlePassProgress();
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
                Debug.LogError("BattlePassPresenter: HeaderView reference is null!");
            }
        }
        
        private void InitializeSubViews()
        {
            if (_tasksView != null)
            {
                _tasksView.Initialize(AppCoordinator.Instance?.BattlePassService);
                _subViews.Add(_tasksView);
            }
            
            if (_rewardsView != null)
            {
                _rewardsView.Initialize(AppCoordinator.Instance?.BattlePassService);
                _rewardsView.OnRewardClaimed += OnRewardClaimRequested;
                _subViews.Add(_rewardsView);
            }
            
            // Показываем начальное подпредставление
            ShowSubView(GetViewForSubScreen(_currentSubScreen));
        }
        
        public override void Show()
        {
            // Показываем хедер
            if (_headerView != null)
                _headerView.gameObject.SetActive(true);
            
            // Показываем текущее подпредставление
            if (_currentSubView != null)
                _currentSubView.Show();
            
            // Обновляем данные при показе
            LoadBattlePassProgress();
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
        
        private void HandleHeaderSubScreenSelected(BattlePassSubScreen subScreen)
        {
            EventSystem.ChangeBattlePassSubScreen(subScreen);
        }
        
        private void HandleSubScreenChanged(BattlePassSubScreen subScreen)
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
            
            // Если показываем rewards view, обновляем данные
            if (view is RewardsView rewardsView && _currentProgress != null)
            {
                rewardsView.DisplayRewards(_currentProgress);
            }
        }
        
        private BaseView GetViewForSubScreen(BattlePassSubScreen subScreen)
        {
            switch (subScreen)
            {
                case BattlePassSubScreen.Tasks: return _tasksView;
                case BattlePassSubScreen.Rewards: return _rewardsView;
                default: return _tasksView;
            }
        }
        
        protected override void HandleUserDataUpdated(UserData userData)
        {
            // При обновлении данных пользователя перезагружаем прогресс бп
            LoadBattlePassProgress();
        }
        
        private void LoadBattlePassProgress()
        {
            var battlePassService = AppCoordinator.Instance?.BattlePassService;
            if (battlePassService == null)
            {
                Debug.LogError("BattlePassPresenter: BattlePassService is not available");
                return;
            }
            
            battlePassService.GetBattlePassProgress(
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
            if (_currentProgress != null)
            {
                // Обновляем данные в текущем подпредставлении
                if (_currentSubView is RewardsView rewardsView)
                {
                    rewardsView.DisplayRewards(_currentProgress);
                }
            }
        }
        
        private void OnRewardClaimRequested(int level, bool isPremium)
        {
            EventSystem.ClaimBattlePassReward(level, isPremium);
        }
        
        private void HandleRewardClaimed(int level, bool isPremium)
        {
            var battlePassService = AppCoordinator.Instance?.BattlePassService;
            if (battlePassService == null)
            {
                Debug.LogError("BattlePassPresenter: BattlePassService is not available");
                return;
            }
            
            battlePassService.ClaimReward(level, isPremium,
                reward => {
                    EventSystem.ShowSuccess($"Награда уровня {level} получена!");
                    
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
                    
                    // Перезагружаем прогресс
                    LoadBattlePassProgress();
                },
                error => {
                    EventSystem.ShowError($"Failed to claim reward: {error}");
                });
        }
        
        private void HandlePremiumPurchased()
        {
            var battlePassService = AppCoordinator.Instance?.BattlePassService;
            if (battlePassService == null)
            {
                Debug.LogError("BattlePassPresenter: BattlePassService is not available");
                return;
            }
            
            battlePassService.PurchasePremium(
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
            // Обработка ошибок, специфичных для Бпшки
            if (_currentSubView != null)
            {
                _currentSubView.ShowError(error);
            }
        }
    }
}