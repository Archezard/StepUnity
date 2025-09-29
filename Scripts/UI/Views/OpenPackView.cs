using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.Services;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class OpenPackView : BaseView
    {
        [Header("UI References")]
        [SerializeField] private Button _openPackButton;
        [SerializeField] private TextMeshProUGUI _packsCountText;
        [SerializeField] private GameObject _packDisplay;
        [SerializeField] private Transform _cardsContainer;
        [SerializeField] private GameObject _cardPrefab;
        [SerializeField] private Button _confirmButton;
        
        private ActivitiesService _activitiesService;
        private List<CardData> _currentCards;
        
        public void Initialize(ActivitiesService activitiesService)
        {
            _activitiesService = activitiesService;
            
            _openPackButton.onClick.AddListener(OnOpenPackButtonClicked);
            _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
            UpdatePacksCount(2);
        }
        
        private void UpdatePacksCount(int count)
        {
            _packsCountText.text = $"Паков: {count}";
            _openPackButton.interactable = count > 0;
        }
        
        public void DisplayCards(List<CardData> cards)
        {
            _currentCards = cards;
            _packDisplay.SetActive(true);
            _openPackButton.gameObject.SetActive(false);
            _confirmButton.gameObject.SetActive(true);
            
            ClearCards();
            
            foreach (var card in cards)
            {
                var cardObject = Instantiate(_cardPrefab, _cardsContainer);
                var cardUI = cardObject.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.Initialize(card, OnCardSelected);
                }
            }
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
            // Просмотр карточки в 3D
            EventSystem.RequestCardView(card);
        }
        
        private void OnOpenPackButtonClicked()
        {
            EventSystem.RequestPackOpen("weekly_pack");
        }

        private void OnConfirmButtonClicked()
        {
            _packDisplay.SetActive(false);
            _openPackButton.gameObject.SetActive(true);
            _confirmButton.gameObject.SetActive(false);
            ClearCards();

            // Обновляем количество паков
            UpdatePacksCount(1); // Уменьшаем на 1 после открытия
            //Заглушка
        }
        
        public override void Show()
        {
            base.Show();
            _packDisplay.SetActive(false);
            _openPackButton.gameObject.SetActive(true);
            _confirmButton.gameObject.SetActive(false);
        }
    }
}