using BasketballCards.Models;
using BasketballCards.Services;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class RewardsView : BaseView
    {
        [Header("UI References")]
        [SerializeField] private Transform _rewardsContainer;
        [SerializeField] private GameObject _rewardPrefab;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private TextMeshProUGUI _currentLevelText;
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private TextMeshProUGUI _freeRewardsTitle;
        [SerializeField] private TextMeshProUGUI _premiumRewardsTitle;
        
        private BattlePassService _battlePassService;
        private List<RewardElement> _rewardElements = new List<RewardElement>();
        
        public System.Action OnBackRequested;
        public System.Action<int, bool> OnRewardClaimed;
        
        public void Initialize(BattlePassService battlePassService)
        {
            _battlePassService = battlePassService;
        }
        
        protected override void OnBackButtonClicked()
        {
            OnBackRequested?.Invoke();
        }
        
        public void DisplayRewards(BattlePassProgress progress)
        {
            _currentLevelText.text = $"Текущий уровень: {progress.Level}";
            _progressSlider.value = (float)progress.Experience / 1000f;
            
            DisplayRewardsList(progress);
        }
        
        private void DisplayRewardsList(BattlePassProgress progress)
        {
            ClearRewards();
            
            // Создаем награды для уровней 1-30 согласно ТЗ
            for (int level = 1; level <= 30; level++)
            {
                CreateRewardElement(level, progress);
            }
        }
        
        private void CreateRewardElement(int level, BattlePassProgress progress)
        {
            var rewardObject = Instantiate(_rewardPrefab, _rewardsContainer);
            var rewardElement = rewardObject.GetComponent<RewardElement>();
            
            bool isClaimed = progress.ClaimedRewards.Contains(level);
            bool canClaim = progress.Level >= level && !isClaimed;
            
            rewardElement.Initialize(level, isClaimed, canClaim, progress.PremiumUnlocked, OnRewardClaimed);
            _rewardElements.Add(rewardElement);
        }
        
        private void ClearRewards()
        {
            foreach (var element in _rewardElements)
            {
                Destroy(element.gameObject);
            }
            _rewardElements.Clear();
        }
    }
}