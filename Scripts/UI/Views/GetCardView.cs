using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.Services;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class GetCardView : BaseView
    {
        [Header("UI References")]
        [SerializeField] private Button _getCardButton;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private TextMeshProUGUI _attemptsText;
        [SerializeField] private GameObject _cardDisplay;
        [SerializeField] private Image _cardImage;
        [SerializeField] private TextMeshProUGUI _cardNameText;
        [SerializeField] private TextMeshProUGUI _cardRarityText;
        
        private ActivitiesService _activitiesService;
        private Coroutine _timerCoroutine;
        
        public System.Action OnBackRequested;
        
        public void Initialize(ActivitiesService activitiesService)
        {
            _activitiesService = activitiesService;
            
            _getCardButton.onClick.AddListener(OnGetCardButtonClicked);
            UpdateAttemptsText(3);
            StartTimer();
        }
        
        protected override void OnBackButtonClicked()
        {
            OnBackRequested?.Invoke();
        }
        
        private void StartTimer()
        {
            if (_timerCoroutine != null)
                StopCoroutine(_timerCoroutine);
            
            _timerCoroutine = StartCoroutine(UpdateTimer());
        }
        
        private IEnumerator UpdateTimer()
        {
            int timeRemaining = 14400; // 4 часа в секундах
            
            while (timeRemaining > 0)
            {
                System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(timeRemaining);
                _timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", 
                    timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
                
                yield return new WaitForSeconds(1f);
                timeRemaining--;
            }
            
            _timerText.text = "Готово!";
            _getCardButton.interactable = true;
        }
        
        private void UpdateAttemptsText(int attempts)
        {
            _attemptsText.text = $"Попыток: {attempts}";
        }
        
        public void DisplayCard(CardData card)
        {
            _cardDisplay.SetActive(true);
            _cardNameText.text = card.PlayerName;
            _cardRarityText.text = card.Rarity.ToString();
            _cardImage.color = card.RarityColor;
        }
        
        private void OnGetCardButtonClicked()
        {
            EventSystem.RequestFreeCard();
        }
        
        public override void Show()
        {
            base.Show();
            _cardDisplay.SetActive(false);
        }
    }
}