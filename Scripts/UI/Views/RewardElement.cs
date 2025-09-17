using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class RewardElement : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _rewardDescriptionText;
        [SerializeField] private Button _claimButton;
        [SerializeField] private Button _claimPremiumButton;
        [SerializeField] private GameObject _claimedIndicator;
        
        private int _level;
        private bool _isPremium;
        
        public void Initialize(int level, bool isClaimed, bool canClaim, System.Action<int, bool> onClaimReward)
        {
            _level = level;
            
            _levelText.text = $"Уровень {level}";
            _rewardDescriptionText.text = GetRewardDescription(level);
            
            _claimButton.gameObject.SetActive(canClaim && !_isPremium);
            _claimPremiumButton.gameObject.SetActive(canClaim && _isPremium);
            _claimedIndicator.SetActive(isClaimed);
            
            _claimButton.onClick.AddListener(() => onClaimReward(level, false));
            _claimPremiumButton.onClick.AddListener(() => onClaimReward(level, true));
        }
        
        private string GetRewardDescription(int level)
        {
            // Описание наград согласно таблице
            switch (level)
            {
                case 1: return "1000 золота";
                case 2: return "10 бронзовых жетонов";
                case 3: return "5 алмазов";
                case 4: return "3 серебряных жетона";
                case 5: return "1500 золота";
                // ... остальные уровни
                default: return "Награда";
            }
        }
    }
}