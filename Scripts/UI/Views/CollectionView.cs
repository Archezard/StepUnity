using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.UI.Presenters;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class CollectionView : BaseView, ICollectionView
    {
        [Header("UI References")]
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GridLayoutGroup _cardsGrid;
        [SerializeField] private GameObject _cardPrefab;
        [SerializeField] private TextMeshProUGUI _selectionInfoText;
        [SerializeField] private Button _craftButton;
        [SerializeField] private Button _disassembleButton;
        [SerializeField] private Button _filterButton;
        [SerializeField] private TMP_InputField _searchInputField;
        
        [Header("Filter UI")]
        [SerializeField] private GameObject _filterPanel;
        [SerializeField] private Button _allCategoriesButton;
        [SerializeField] private Button _bronzeFilterButton;
        [SerializeField] private Button _silverFilterButton;
        [SerializeField] private Button _goldFilterButton;
        [SerializeField] private Button _diamondFilterButton;
        [SerializeField] private Button _legendaryFilterButton;

        private CollectionPresenter _presenter;
        private List<CardItemView> _cardViews = new List<CardItemView>();
        private List<CardData> _currentCards;
        private Rarity? _currentFilter = null;
        private string _currentSearchQuery = "";
        
        public void Initialize(CollectionPresenter presenter)
        {
            _presenter = presenter;
            
            // Инициализация кнопок
            _craftButton.onClick.AddListener(OnCraftButtonClicked);
            _disassembleButton.onClick.AddListener(OnDisassembleButtonClicked);
            _filterButton.onClick.AddListener(OnFilterButtonClicked);
            _searchInputField.onValueChanged.AddListener(OnSearchInputChanged);
            
            // Инициализация фильтров
            InitializeFilters();
            
            // Скрываем панель фильтров
            if (_filterPanel != null)
            {
                _filterPanel.SetActive(false);
            }
            
            UpdateSelectionCount(0);
        }
        
        private void InitializeFilters()
        {
            _allCategoriesButton.onClick.AddListener(() => SetFilter(null));
            _bronzeFilterButton.onClick.AddListener(() => SetFilter(Rarity.Bronze));
            _silverFilterButton.onClick.AddListener(() => SetFilter(Rarity.Silver));
            _goldFilterButton.onClick.AddListener(() => SetFilter(Rarity.Gold));
            _diamondFilterButton.onClick.AddListener(() => SetFilter(Rarity.Diamond));
            _legendaryFilterButton.onClick.AddListener(() => SetFilter(Rarity.Legendary));
        }
        
        public void DisplayCards(List<CardData> cards)
        {
            _currentCards = cards;
            ApplyFiltersAndSearch();
        }
        
    
        public void ShowCardDetails(CardData card)
        {
            // Прямой вызов CardViewerPresenter через презентер
            _presenter.OnCardSelectedInView(card);
        }
        
        public void UpdateSelectionCount(int count)
        {
            _selectionInfoText.text = $"Выбрано: {count}";
            _craftButton.interactable = count >= 10;
            _disassembleButton.interactable = count > 0;
        }
        
        public void OnCardUpgraded(CardData card)
        {
            // Находим View этой карточки и обновляем ее
            var cardView = _cardViews.Find(c => c.CardId == card.CardId);
            if (cardView != null)
            {
                cardView.UpdateCardData(card);
            }
            
            ShowSuccess($"Карточка улучшена до уровня {card.Level}!");
        }
        
        public void OnCardCrafted(CardData card)
        {
            // Добавляем новую карточку в коллекцию
            _currentCards.Add(card);
            ApplyFiltersAndSearch();
            
            // Показываем новую карточку
            ShowCardDetails(card);
            
            ShowSuccess($"Успешно скрафчена карточка: {card.PlayerName}!");
        }
        
        public void ClearSelection()
        {
            foreach (var cardView in _cardViews)
            {
                cardView.SetSelected(false);
            }
            UpdateSelectionCount(0);
        }
        
        private void ApplyFiltersAndSearch()
        {
            ClearCards();
            
            var filteredCards = _currentCards;
            
            // Применяем фильтр по редкости
            if (_currentFilter.HasValue)
            {
                filteredCards = filteredCards.FindAll(c => c.Rarity == _currentFilter.Value);
            }
            
            // Применяем поиск
            if (!string.IsNullOrEmpty(_currentSearchQuery))
            {
                filteredCards = filteredCards.FindAll(c => 
                    c.PlayerName.ToLower().Contains(_currentSearchQuery.ToLower()) ||
                    (c.Team != null && c.Team.ToLower().Contains(_currentSearchQuery.ToLower())));
            }
            
            // Сортируем карточки (по имени, по умолчанию)
            filteredCards.Sort((a, b) => string.Compare(a.PlayerName, b.PlayerName));
            
            // Создаем карточки в UI
            foreach (var card in filteredCards)
            {
                var cardView = Instantiate(_cardPrefab, _cardsGrid.transform).GetComponent<CardItemView>();
                if (cardView != null)
                {
                    cardView.Initialize(card, OnCardSelected, OnCardToggled);
                    _cardViews.Add(cardView);
                }
            }
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
            _presenter.OnCardSelectedInView(card);
        }
        
        private void OnCardToggled(CardData card, bool isSelected)
        {
            // Обработка выбора карточек для крафта/разбора
            // TODO: Реализовать логику выбора
        }
        
        private void OnCraftButtonClicked()
        {
            // TODO: Реализовать логику крафта
        }
        
        private void OnDisassembleButtonClicked()
        {
            // TODO: Реализовать логику разбора
        }
        
        private void OnFilterButtonClicked()
        {
            if (_filterPanel != null)
            {
                _filterPanel.SetActive(!_filterPanel.activeSelf);
            }
        }
        
        private void SetFilter(Rarity? rarity)
        {
            _currentFilter = rarity;
            ApplyFiltersAndSearch();
            
            // Скрываем панель фильтров
            if (_filterPanel != null)
            {
                _filterPanel.SetActive(false);
            }
        }
        
        private void OnSearchInputChanged(string searchQuery)
        {
            _currentSearchQuery = searchQuery;
            ApplyFiltersAndSearch();
        }
        
        public override void Show()
        {
            base.Show();
            
            // Сбрасываем фильтры и поиск при показе
            _currentFilter = null;
            _currentSearchQuery = "";
            _searchInputField.text = "";
            
            // Скрываем дополнительные панели
            if (_filterPanel != null)
            {
                _filterPanel.SetActive(false);
            }
            
            // Очищаем выделение
            ClearSelection();
        }
        
        public override void Hide()
        {
            base.Hide();
            
            // Скрываем все дополнительные панели
            if (_filterPanel != null)
            {
                _filterPanel.SetActive(false);
            }
        }
    }
}