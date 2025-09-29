using BasketballCards.Core;
using BasketballCards.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class ThrowBallView : BaseView
    {
        [Header("UI References")]
        [SerializeField] private Button _throwButton;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _ballsCountText;
        [SerializeField] private GameObject _resultPanel;
        [SerializeField] private TextMeshProUGUI _resultText;
        
        private ActivitiesService _activitiesService;
        
        public void Initialize(ActivitiesService activitiesService)
        {
            _activitiesService = activitiesService;
            
            _throwButton.onClick.AddListener(OnThrowButtonClicked);
            UpdateBallsCount(5);
            _resultPanel.SetActive(false);
        }
        
        private void UpdateBallsCount(int count)
        {
            _ballsCountText.text = $"Мячей: {count}";
            _throwButton.interactable = count > 0;
        }
        
        public void ShowThrowResult(int score, int rewards)
        {
            _resultPanel.SetActive(true);
            _resultText.text = $"Очков: {score}\nНаград: {rewards}";
            _scoreText.text = $"Счет: {score}";
            
            // Обновляем количество мячей
            UpdateBallsCount(4); // Уменьшаем на 1 после броска, тут желательно тоже через апишку
        }
        
        private void OnThrowButtonClicked()
        {
            EventSystem.RequestBallThrow();
        }
        
        public override void Show()
        {
            base.Show();
            _resultPanel.SetActive(false);
        }
    }
}