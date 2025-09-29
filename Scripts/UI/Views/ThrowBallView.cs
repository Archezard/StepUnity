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
        [SerializeField] private TextMeshProUGUI _ballsCountText;
        [SerializeField] private GameObject _hoop;
        [SerializeField] private GameObject _ball;
        [SerializeField] private TextMeshProUGUI _scoreText;
        
        private ActivitiesService _activitiesService;
        private int _currentScore = 0;
        
        public System.Action OnBackRequested;
        
        public void Initialize(ActivitiesService activitiesService)
        {
            _activitiesService = activitiesService;
            
            _throwButton.onClick.AddListener(OnThrowButtonClicked);
            UpdateBallsCount(5);
            UpdateScore();
        }
        
        protected override void OnBackButtonClicked()
        {
            OnBackRequested?.Invoke();
        }
        
        private void UpdateBallsCount(int count)
        {
            _ballsCountText.text = $"Мячей: {count}";
            _throwButton.interactable = count > 0;
        }
        
        private void UpdateScore()
        {
            _scoreText.text = $"Счёт: {_currentScore}";
        }
        
        private void OnThrowButtonClicked()
        {
            EventSystem.RequestBallThrow();
        }
        
        public void ShowThrowResult(int score, int rewards)
        {
            _currentScore += score;
            UpdateScore();
            
            if (score > 0)
            {
                ShowSuccess($"Попадание! +{score} очков");
                if (rewards > 0)
                {
                    ShowSuccess($"Получено наград: {rewards}");
                }
            }
            else
            {
                ShowError("Промах!");
            }
        }
        
        public override void Show()
        {
            base.Show();
            _currentScore = 0;
            UpdateScore();
        }
    }
}