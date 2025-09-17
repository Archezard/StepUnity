using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Presenters;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class RewardsView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button _backButton;
        [SerializeField] private Transform _rewardsContainer;
        [SerializeField] private GameObject _rewardPrefab;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Slider _progressSlider;
        
        private BattlePassPresenter _presenter;
        private BattlePassService _battlePassService;
        
        public void Initialize(BattlePassPresenter presenter, BattlePassService battlePassService)
        {
            _presenter = presenter;
            _battlePassService = battlePassService;
            
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }
        
        public void DisplayRewards(BattlePassProgress progress)
        {
            _levelText.text = $"Уровень: {progress.Level}";
            _progressSlider.value = progress.Experience % 1000 / 1000f;
            
            DisplayRewardsList(progress);
        }
        
        private void DisplayRewardsList(BattlePassProgress progress)
        {
            ClearRewards();
            
            for (int level = 1; level <= 30; level++)
            {
                var rewardObject = Instantiate(_rewardPrefab, _rewardsContainer);
                var rewardElement = rewardObject.GetComponent<RewardElement>();
                
                bool isClaimed = progress.ClaimedRewards.Contains(level);
                bool canClaim = progress.Level >= level && !isClaimed;
                
                rewardElement.Initialize(level, isClaimed, canClaim, OnClaimReward);
            }
        }
        
        private void ClearRewards()
        {
            foreach (Transform child in _rewardsContainer)
            {
                Destroy(child.gameObject);
            }
        }
        
        private void OnClaimReward(int level, bool isPremium)
        {
            _presenter.OnRewardClaimed(level, isPremium);
        }
        
        private void OnBackButtonClicked()
        {
            _presenter.ShowBattlePass();
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}