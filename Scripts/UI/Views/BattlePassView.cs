using BasketballCards.Managers;
using BasketballCards.Models;
using BasketballCards.UI.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class BattlePassView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button _tasksButton;
        [SerializeField] private Button _rewardsButton;
        [SerializeField] private Button _purchasePremiumButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Slider _progressSlider;
        
        private BattlePassPresenter _presenter;
        
        public void Initialize(BattlePassPresenter presenter)
        {
            _presenter = presenter;
            
            _tasksButton.onClick.AddListener(OnTasksButtonClicked);
            _rewardsButton.onClick.AddListener(OnRewardsButtonClicked);
            _purchasePremiumButton.onClick.AddListener(OnPurchasePremiumButtonClicked);
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }
        
        public void DisplayProgress(BattlePassProgress progress)
        {
            _levelText.text = $"Level: {progress.Level}";
            _progressSlider.value = progress.Experience % 1000 / 1000f; // Предполагаем, что каждый уровень требует 1000 опыта
            _purchasePremiumButton.interactable = !progress.PremiumUnlocked;
        }
        
        private void OnTasksButtonClicked()
        {
            _presenter.ShowTasks();
        }
        
        private void OnRewardsButtonClicked()
        {
            _presenter.ShowRewards();
        }
        
        private void OnPurchasePremiumButtonClicked()
        {
            _presenter.OnPremiumPurchased();
        }
        
        private void OnBackButtonClicked()
        {
            UIManager.Instance.ShowMainMenu();
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