using BasketballCards.UI.Presenters;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class GetCardView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button _getCardButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private TextMeshProUGUI _attemptsText;
        [SerializeField] private GameObject _cardDisplay;
        [SerializeField] private Image _cardImage;
        [SerializeField] private TextMeshProUGUI _cardNameText;
        [SerializeField] private TextMeshProUGUI _cardRarityText;
        
        private ActivitiesPresenter _presenter;
        private Coroutine _timerCoroutine;
        
        public void Initialize(ActivitiesPresenter presenter)
        {
            _presenter = presenter;
            
            _getCardButton.onClick.AddListener(OnGetCardButtonClicked);
            _backButton.onClick.AddListener(OnBackButtonClicked);
            
            UpdateAttemptsText(3);
            StartTimer();
        }
        
        private void StartTimer()
        {
            if (_timerCoroutine != null)
                StopCoroutine(_timerCoroutine);
            
            _timerCoroutine = StartCoroutine(UpdateTimer());
        }
        
        private IEnumerator UpdateTimer()
        {
            int timeRemaining = 14400;
            
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
        
        public void DisplayCard(BasketballCards.Models.CardData card)
        {
            _cardDisplay.SetActive(true);
            _cardNameText.text = card.PlayerName;
            _cardRarityText.text = card.Rarity.ToString();
            _cardImage.color = card.RarityColor;
        }
        
        public void ShowError(string error)
        {
            Debug.LogError($"GetCard Error: {error}");
        }
        
        private void OnGetCardButtonClicked()
        {
            _presenter.GetFreeCard();
        }
        
        private void OnBackButtonClicked()
        {
            _presenter.ShowActivities();
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
            _cardDisplay.SetActive(false);
        }
    }
}