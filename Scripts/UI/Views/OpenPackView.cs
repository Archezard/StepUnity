using BasketballCards.Models;
using BasketballCards.UI.Presenters;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class OpenPackView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button _openPackButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private TextMeshProUGUI _packsCountText;
        [SerializeField] private GameObject _packDisplay;
        [SerializeField] private Transform _cardsContainer;
        [SerializeField] private GameObject _cardPrefab;
        
        private ActivitiesPresenter _presenter;
        
        public void Initialize(ActivitiesPresenter presenter)
        {
            _presenter = presenter;
            
            _openPackButton.onClick.AddListener(OnOpenPackButtonClicked);
            _backButton.onClick.AddListener(OnBackButtonClicked);
            
            UpdatePacksCount(2);
        }
        
        private void UpdatePacksCount(int count)
        {
            _packsCountText.text = $"Паков: {count}";
            _openPackButton.interactable = count > 0;
        }
        
        public void DisplayCards(List<CardData> cards)
        {
            _packDisplay.SetActive(true);
            ClearCards();
            
            foreach (var card in cards)
            {
                var cardObject = Instantiate(_cardPrefab, _cardsContainer);
                var cardView = cardObject.GetComponent<CardItemView>();
                cardView.Initialize(card, OnCardSelected, OnCardToggled);
            }
        }
        
        public void ShowError(string error)
        {
            Debug.LogError($"OpenPack Error: {error}");
        }
        
        private void ClearCards()
        {
            foreach (Transform child in _cardsContainer)
            {
                Destroy(child.gameObject);
            }
        }
        
        private void OnCardSelected(CardData card)
        {
            // Просмотр карточки
            // Здесь будет вызов CardViewer
            Debug.Log($"Selected card: {card.PlayerName}");
        }
        
        private void OnCardToggled(CardData card, bool isSelected)
        {
            // Не используется в этом View
        }
        
        private void OnOpenPackButtonClicked()
        {
            _presenter.OpenPack("weekly_pack");
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
            _packDisplay.SetActive(false);
        }
    }
}