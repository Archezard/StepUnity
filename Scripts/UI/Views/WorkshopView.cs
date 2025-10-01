using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.UI.Presenters;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class WorkshopView : BaseView, IWorkshopView
    {
        [Header("UI References")]
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GridLayoutGroup _cardsGrid;
        [SerializeField] private GameObject _cardPrefab;
        [SerializeField] private Button _craftButton;
        [SerializeField] private Button _disassembleButton;
        [SerializeField] private TextMeshProUGUI _selectionCountText;
        [SerializeField] private TextMeshProUGUI _craftChanceText;
        [SerializeField] private Button _selectAllButton;
        [SerializeField] private Button _clearSelectionButton;

        [Header("Craft/Disassemble Results")]
        [SerializeField] private GameObject _resultPanel;
        [SerializeField] private TextMeshProUGUI _resultText;
        [SerializeField] private Button _resultCloseButton;

        private CollectionPresenter _presenter;
        private List<CardItemView> _cardViews = new List<CardItemView>();
        private List<CardData> _currentCards;
        private List<CardData> _selectedCards = new List<CardData>();
        private WorkshopMode _currentMode = WorkshopMode.Craft;

        public void Initialize(CollectionPresenter presenter)
        {
            _presenter = presenter;
            
            _craftButton.onClick.AddListener(OnCraftButtonClicked);
            _disassembleButton.onClick.AddListener(OnDisassembleButtonClicked);
            _selectAllButton.onClick.AddListener(OnSelectAllButtonClicked);
            _clearSelectionButton.onClick.AddListener(OnClearSelectionButtonClicked);
            _resultCloseButton.onClick.AddListener(OnResultCloseButtonClicked);
            
            _resultPanel.SetActive(false);
            UpdateSelectionUI();
        }
        
        public void DisplayCards(List<CardData> cards)
        {
            _currentCards = cards;
            ClearCards();
            
            foreach (var card in cards)
            {
                var cardObject = Instantiate(_cardPrefab, _cardsGrid.transform);
                var cardView = cardObject.GetComponent<CardItemView>();
                if (cardView != null)
                {
                    cardView.Initialize(card, OnCardSelected, OnCardToggled);
                    _cardViews.Add(cardView);
                }
            }
        }
        
        private void OnCardSelected(CardData card)
        {
            // В мастерской при одиночном нажатии - просмотр
            EventSystem.RequestCardView(card);
        }
        
        private void OnCardToggled(CardData card, bool isSelected)
        {
            if (isSelected)
            {
                if (!_selectedCards.Contains(card))
                    _selectedCards.Add(card);
            }
            else
            {
                _selectedCards.Remove(card);
            }
            
            UpdateSelectionUI();
        }
        
        private void UpdateSelectionUI()
        {
            _selectionCountText.text = $"Выбрано: {_selectedCards.Count}";
            
            // Обновляем текст шанса крафта (заглушка)
            if (_currentMode == WorkshopMode.Craft)
            {
                float chance = CalculateCraftChance();
                _craftChanceText.text = $"Шанс успеха: {chance:P0}";
                _craftButton.interactable = _selectedCards.Count >= 3;
            }
            else
            {
                _craftChanceText.text = "";
                _disassembleButton.interactable = _selectedCards.Count > 0;
            }
        }
        
        private float CalculateCraftChance()
        {
            // Заглушка для расчета шанса крафта
            if (_selectedCards.Count < 3) return 0f;
            return Mathf.Min(0.8f + (_selectedCards.Count - 3) * 0.05f, 0.95f);
        }
        
        private void OnCraftButtonClicked()
        {
            if (_selectedCards.Count < 3)
            {
                ShowError("Для крафта нужно выбрать минимум 3 карты");
                return;
            }
            
            var cardIds = new List<string>();
            foreach (var card in _selectedCards)
            {
                cardIds.Add(card.CardId);
            }
            
            _presenter?.CraftCards(cardIds);
        }
        
        private void OnDisassembleButtonClicked()
        {
            if (_selectedCards.Count == 0)
            {
                ShowError("Выберите карты для разбора");
                return;
            }
            
            var cardIds = new List<string>();
            foreach (var card in _selectedCards)
            {
                cardIds.Add(card.CardId);
            }
            
            _presenter?.DisassembleCards(cardIds);
        }
        
        private void OnSelectAllButtonClicked()
        {
            foreach (var cardView in _cardViews)
            {
                cardView.SetSelected(true);
            }
            
            _selectedCards.Clear();
            _selectedCards.AddRange(_currentCards);
            UpdateSelectionUI();
        }
        
        private void OnClearSelectionButtonClicked()
        {
            ClearSelection();
        }
        
        private void OnResultCloseButtonClicked()
        {
            _resultPanel.SetActive(false);
            ClearSelection();
        }
        
        public void UpdateSelectionCount(int count)
        {
            // Наверное уже не надо...
        }
        
        public void ClearSelection()
        {
            foreach (var cardView in _cardViews)
            {
                cardView.SetSelected(false);
            }
            _selectedCards.Clear();
            UpdateSelectionUI();
        }
        
        public void OnCraftSuccess(CardData craftedCard)
        {
            _resultText.text = $"Крафт успешен!\nПолучена карта: {craftedCard.PlayerName} ({craftedCard.Rarity})";
            _resultPanel.SetActive(true);
            
            // Обновляем список карт
            _presenter?.LoadUserCards();
        }
        
        public void OnDisassembleSuccess(int gold, int dust)
        {
            _resultText.text = $"Разбор успешен!\nПолучено: {gold} золота, {dust} пыли";
            _resultPanel.SetActive(true);
            
            // Обновляем список карт
            _presenter?.LoadUserCards();
        }
        
        public override void Show()
        {
            base.Show();
            _resultPanel.SetActive(false);
            ClearSelection();
            _presenter?.LoadUserCards();
        }
        
        private void ClearCards()
        {
            foreach (var cardView in _cardViews)
            {
                Destroy(cardView.gameObject);
            }
            _cardViews.Clear();
        }
        
        public void SetMode(WorkshopMode mode)
        {
            _currentMode = mode;
            _craftButton.gameObject.SetActive(mode == WorkshopMode.Craft);
            _disassembleButton.gameObject.SetActive(mode == WorkshopMode.Disassemble);
            UpdateSelectionUI();
        }
    }

    public enum WorkshopMode
    {
        Craft,
        Disassemble
    }
}