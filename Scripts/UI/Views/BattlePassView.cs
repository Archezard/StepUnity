using BasketballCards.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class BattlePassView : BaseView
    {
        [Header("UI References")]
        [SerializeField] private Button _tasksButton;
        [SerializeField] private Button _rewardsButton;
        [SerializeField] private Button _purchasePremiumButton;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _experienceText;
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private TextMeshProUGUI _premiumStatusText;
        
        public System.Action OnTasksSelected;
        public System.Action OnRewardsSelected;
        public System.Action OnPremiumPurchaseSelected;
        
        public void Initialize()
        {
            _tasksButton.onClick.AddListener(() => OnTasksSelected?.Invoke());
            _rewardsButton.onClick.AddListener(() => OnRewardsSelected?.Invoke());
            _purchasePremiumButton.onClick.AddListener(() => OnPremiumPurchaseSelected?.Invoke());
        }
        
        public void DisplayProgress(BattlePassProgress progress)
        {
            _levelText.text = $"Уровень: {progress.Level}";
            _experienceText.text = $"Опыт: {progress.Experience}/1000";
            _progressSlider.value = (float)progress.Experience / 1000f;
            
            _premiumStatusText.text = progress.PremiumUnlocked ? "Премиум активирован" : "Бесплатная версия";
            _purchasePremiumButton.interactable = !progress.PremiumUnlocked;
            _purchasePremiumButton.GetComponentInChildren<TextMeshProUGUI>().text = 
                progress.PremiumUnlocked ? "Активировано" : "Купить премиум";
        }
    }
}