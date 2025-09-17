using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.UI.Presenters;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class CollectionView : MonoBehaviour, ICollectionView
    {
        [Header("UI References")]
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GridLayoutGroup _cardsGrid;
        [SerializeField] private GameObject _cardPrefab;
        [SerializeField] private TextMeshProUGUI _selectionInfoText;
        [SerializeField] private Button _craftButton;
        [SerializeField] private Button _disassembleButton;
        [SerializeField] private GameObject _cardDetailsPanel;
        [SerializeField] private CardViewer3D _cardViewer;
        
        private CollectionPresenter _presenter;
        private List<CardItemView> _cardViews = new List<CardItemView>();
        
        public void Initialize(CollectionPresenter presenter)
        {
            _presenter = presenter;
            
            _craftButton.onClick.AddListener(OnCraftButtonClicked);
            _disassembleButton.onClick.AddListener(OnDisassembleButtonClicked);
            _cardViewer.Initialize();
        }
        
        public void DisplayCards(List<CardData> cards)
        {
            ClearCards();
            
            foreach (var card in cards)
            {
                var cardView = Instantiate(_cardPrefab, _cardsGrid.transform).GetComponent<CardItemView>();
                cardView.Initialize(card, OnCardSelected, OnCardToggled);
                _cardViews.Add(cardView);
            }
        }
        
        public void ShowCardDetails(CardData card)
        {
            _cardViewer.ShowCard(card);
        }
        
        public void UpdateSelectionCount(int count)
        {
            _selectionInfoText.text = $"Выбрано: {count}";
            _craftButton.interactable = count >= 10;
            _disassembleButton.interactable = count > 0;
        }
        
        public void OnCardUpgraded(CardData card)
        {
            var cardView = _cardViews.Find(c => c.CardId == card.CardId);
            if (cardView != null)
            {
                cardView.UpdateCardData(card);
            }
        }
        
        public void OnCardCrafted(CardData card)
        {
            var cardView = Instantiate(_cardPrefab, _cardsGrid.transform).GetComponent<CardItemView>();
            cardView.Initialize(card, OnCardSelected, OnCardToggled);
            _cardViews.Add(cardView);
        }
        
        public void ClearSelection()
        {
            foreach (var cardView in _cardViews)
            {
                cardView.SetSelected(false);
            }
        }
        
        public void ShowError(string message)
        {
            Debug.LogError($"Collection Error: {message}");
        }
        
        private void ClearCards()
        {
            foreach (var cardView in _cardViews)
            {
                Destroy(cardView.gameObject);
            }
            _cardViews.Clear();
        }
        
        private void OnCardSelected(CardData card)
        {
            _presenter.OnCardSelected(card);
        }
        
        private void OnCardToggled(CardData card, bool isSelected)
        {
            _presenter.OnCardToggleSelected(card, isSelected);
        }
        
        private void OnCraftButtonClicked()
        {
            _presenter.OnCraftRequested();
        }
        
        private void OnDisassembleButtonClicked()
        {
            _presenter.OnDisassembleRequested();
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