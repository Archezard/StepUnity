using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Views;
using UnityEngine;

namespace BasketballCards.UI.Presenters
{
    public class BattlePassPresenter : MonoBehaviour
    {
        [Header("View References")]
        [SerializeField] private BattlePassView _battlePassView;
        [SerializeField] private TasksView _tasksView;
        [SerializeField] private RewardsView _rewardsView;
        
        [Header("Service References")]
        [SerializeField] private BattlePassService _battlePassService;
        
        private GameManager _gameManager;
        private BattlePassProgress _currentProgress;
        
        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            _battlePassService = _gameManager.BattlePassService;
            
            // Инициализация View
            _battlePassView.Initialize(this);
            _tasksView.Initialize(this, _battlePassService);
            _rewardsView.Initialize(this, _battlePassService);
            
            // Загрузка прогресса
            LoadBattlePassProgress();
            
            // Скрываем все подразделы, показываем только основной
            HideAllSubsections();
            _battlePassView.Show();
            
            Debug.Log("BattlePassPresenter: Initialized");
        }
        
        public void ShowBattlePass()
        {
            HideAllSubsections();
            _battlePassView.Show();
        }
        
        public void ShowTasks()
        {
            HideAllSubsections();
            _tasksView.Show();
        }
        
        public void ShowRewards()
        {
            HideAllSubsections();
            _rewardsView.Show();
        }
        
        private void HideAllSubsections()
        {
            _battlePassView.Hide();
            _tasksView.Hide();
            _rewardsView.Hide();
        }
        
        private void LoadBattlePassProgress()
        {
            _battlePassService.GetBattlePassProgress(
                progress => {
                    _currentProgress = progress;
                    _battlePassView.DisplayProgress(progress);
                    _rewardsView.DisplayRewards(progress);
                },
                error => {
                    Debug.LogError("Failed to load battle pass progress: " + error);
                });
        }
        
        public void OnRewardClaimed(int level, bool isPremium)
        {
            _battlePassService.ClaimReward(level, isPremium,
                reward => {
                    // Обновляем данные пользователя
                    _gameManager.UserService.GetUserData(_gameManager.CurrentUser.username, 
                        userData => {
                            _gameManager.SetCurrentUser(userData);
                            LoadBattlePassProgress(); // Перезагружаем прогресс
                        },
                        error => {
                            Debug.LogError("Failed to update user data: " + error);
                        });
                },
                error => {
                    Debug.LogError("Failed to claim reward: " + error);
                });
        }
        
        public void OnPremiumPurchased()
        {
            _battlePassService.PurchasePremium(
                success => {
                    if (success)
                    {
                        LoadBattlePassProgress(); // Перезагружаем прогресс
                    }
                    else
                    {
                        Debug.LogError("Failed to purchase premium battle pass");
                    }
                },
                error => {
                    Debug.LogError("Failed to purchase premium battle pass: " + error);
                });
        }
    }
}