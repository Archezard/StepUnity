using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class RewardElement : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _freeRewardText;
        [SerializeField] private TextMeshProUGUI _premiumRewardText;
        [SerializeField] private Button _claimFreeButton;
        [SerializeField] private Button _claimPremiumButton;
        [SerializeField] private GameObject _claimedIndicator;
        [SerializeField] private Image _backgroundImage;
        
        private int _level;
        private bool _isPremiumUnlocked;
        
        public void Initialize(int level, bool isClaimed, bool canClaim, bool isPremiumUnlocked, System.Action<int, bool> onClaimReward)
        {
            _level = level;
            _isPremiumUnlocked = isPremiumUnlocked;
            
            _levelText.text = $"Уровень {level}";
            _freeRewardText.text = GetFreeRewardDescription(level);
            _premiumRewardText.text = GetPremiumRewardDescription(level);
            
            // Настройка кнопок
            _claimFreeButton.gameObject.SetActive(canClaim && !isClaimed);
            _claimPremiumButton.gameObject.SetActive(canClaim && !isClaimed && isPremiumUnlocked);
            _claimedIndicator.SetActive(isClaimed);
            
            _claimFreeButton.onClick.AddListener(() => onClaimReward?.Invoke(level, false));
            _claimPremiumButton.onClick.AddListener(() => onClaimReward?.Invoke(level, true));
            
            // Разный цвет для премиумных наград
            _backgroundImage.color = isPremiumUnlocked ? 
                new Color(1f, 0.8f, 0f, 0.2f) : 
                new Color(0.5f, 0.5f, 0.5f, 0.2f);
        }
        
        private string GetFreeRewardDescription(int level)
        {
            // Описание бесплатных наград согласно таблице из ТЗ
            switch (level)
            {
                case 1: return "1000 золота";
                case 2: return "10 бронзовых жетонов";
                case 3: return "5 алмазов";
                case 4: return "3 серебряных жетона";
                case 5: return "1500 золота";
                case 6: return "15 бронзовых жетонов";
                case 7: return "10 алмазов";
                case 8: return "1500 золота";
                case 9: return "1 золотой жетон";
                case 10: return "4 броска";
                // ... остальные уровни
                default: return "Награда";
            }
        }
        
        private string GetPremiumRewardDescription(int level)
        {
            // Описание премиумных наград согласно таблице из ТЗ
            switch (level)
            {
                case 1: return "Клип карта";
                case 2: return "1 золотой жетон";
                case 3: return "15 алмазов";
                case 4: return "50 бронзовых жетонов";
                case 5: return "2000 золота";
                case 6: return "6 серебряных жетонов";
                case 7: return "15 алмазов";
                case 8: return "2500 золота";
                case 9: return "1 золотой жетон";
                case 10: return "8 бросков";
                // ... остальные уровни
                default: return "Премиум награда";
            }
        }
    }
}