using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.UI.Presenters;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class CollectionView : BaseView
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
        [SerializeField] private GameObject _cardDetailsPanel;
        [SerializeField] private CardViewer3D _cardViewer;
        
        [Header("Filter UI")]
        [SerializeField] private GameObject _filterPanel;
        [SerializeField] private Button _allCategoriesButton;
        [SerializeField] private Button _bronzeFilterButton;
        [SerializeField] private Button _silverFilterButton;
        [SerializeField] private Button _goldFilterButton;
        [SerializeField] private Button _diamondFilterButton;
        [SerializeField] private Button _legendaryFilterButton;

        [Header("3D Card Viewer")]
        [SerializeField] private CardViewer3D _cardViewer3D;
        
        private CollectionPresenter _presenter;
        private List<CardItemView> _cardViews = new List<CardItemView>();
        private List<CardData> _currentCards;
        private Rarity? _currentFilter = null;
        private string _currentSearchQuery = "";
        
        public void Initialize(CollectionPresenter presenter)
        {

            _presenter = presenter;
        
            // Инициализация CardViewer3D
            if (_cardViewer3D != null)
            {
                _cardViewer3D.Initialize();
            }

            _presenter = presenter;
            
            // Инициализация кнопок
            _craftButton.onClick.AddListener(OnCraftButtonClicked);
            _disassembleButton.onClick.AddListener(OnDisassembleButtonClicked);
            _filterButton.onClick.AddListener(OnFilterButtonClicked);
            _searchInputField.onValueChanged.AddListener(OnSearchInputChanged);
            
            // Инициализация фильтров
            InitializeFilters();
            
            // Инициализация CardViewer
            if (_cardViewer != null)
            {
                _cardViewer.Initialize();
            }
            
            // Скрываем панель деталей при старте
            if (_cardDetailsPanel != null)
            {
                _cardDetailsPanel.SetActive(false);
            }
            
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
            
            // Обновляем информацию о количестве
            UpdateCardsCount(filteredCards.Count, _currentCards.Count);
        }
        
        private void UpdateCardsCount(int filteredCount, int totalCount)
        {
            string countText = $"Показано: {filteredCount}";
            if (filteredCount != totalCount)
            {
                countText += $" из {totalCount}";
            }
            
            // Можно добавить TextMeshProUGUI для отображения количества
            Debug.Log(countText);
        }
        
        public void ShowCardDetails(CardData card)
        {
            // Используем CardViewer3D для просмотра карточки
            if (_cardViewer3D != null)
            {
                _cardViewer3D.ShowCard(card);
            }
            else
            {
                // Fallback: показываем обычную панель деталей
                if (_cardDetailsPanel != null)
                {
                    _cardDetailsPanel.SetActive(true);
                    // ... настройка панели ...
                }
            }
        }
        
        public void UpdateSelectionCount(int count)
        {
            _selectionInfoText.text = $"Выбрано: {count}";
            _craftButton.interactable = count >= 10;
            _disassembleButton.interactable = count > 0;
            
            // Обновляем текст на кнопках
            var craftText = _craftButton.GetComponentInChildren<TextMeshProUGUI>();
            if (craftText != null)
            {
                craftText.text = count >= 10 ? "Крафт (10)" : $"Крафт ({count}/10)";
            }
            
            var disassembleText = _disassembleButton.GetComponentInChildren<TextMeshProUGUI>();
            if (disassembleText != null)
            {
                disassembleText.text = count > 0 ? $"Разобрать ({count})" : "Разобрать";
            }
        }
        
        public void OnCardUpgraded(CardData card)
        {
            // Находим View этой карточки и обновляем ее
            var cardView = _cardViews.Find(c => c.CardId == card.CardId);
            if (cardView != null)
            {
                cardView.UpdateCardData(card);
            }
            
            // Скрываем панель деталей
            if (_cardDetailsPanel != null)
            {
                _cardDetailsPanel.SetActive(false);
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
            _presenter.OnCardToggleSelectedInView(card, isSelected);
        }
        
        private void OnCraftButtonClicked()
        {
            _presenter.OnCraftRequestedInView();
        }
        
        private void OnDisassembleButtonClicked()
        {
            _presenter.OnDisassembleRequestedInView();
        }
        
        private void OnUpgradeCardRequested(CardData card)
        {
            _presenter.OnUpgradeCardRequestedInView(card);
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
            
            // Обновляем визуальное состояние кнопок фильтров
            UpdateFilterButtonsState();
            
            // Скрываем панель фильтров
            if (_filterPanel != null)
            {
                _filterPanel.SetActive(false);
            }
        }
        
        private void UpdateFilterButtonsState()
        {
            // Сбрасываем все кнопки
            _allCategoriesButton.interactable = true;
            _bronzeFilterButton.interactable = true;
            _silverFilterButton.interactable = true;
            _goldFilterButton.interactable = true;
            _diamondFilterButton.interactable = true;
            _legendaryFilterButton.interactable = true;
            
            // Деактивируем текущий фильтр
            if (!_currentFilter.HasValue)
            {
                _allCategoriesButton.interactable = false;
            }
            else
            {
                switch (_currentFilter.Value)
                {
                    case Rarity.Bronze: _bronzeFilterButton.interactable = false; break;
                    case Rarity.Silver: _silverFilterButton.interactable = false; break;
                    case Rarity.Gold: _goldFilterButton.interactable = false; break;
                    case Rarity.Diamond: _diamondFilterButton.interactable = false; break;
                    case Rarity.Legendary: _legendaryFilterButton.interactable = false; break;
                }
            }
        }
        
        private void OnSearchInputChanged(string searchQuery)
        {
            _currentSearchQuery = searchQuery;
            ApplyFiltersAndSearch();
        }
        
        protected override void OnBackButtonClicked()
        {
            // Если открыта панель деталей, скрываем ее
            if (_cardDetailsPanel != null && _cardDetailsPanel.activeSelf)
            {
                _cardDetailsPanel.SetActive(false);
                return;
            }
            
            // Если открыта панель фильтров, скрываем ее
            if (_filterPanel != null && _filterPanel.activeSelf)
            {
                _filterPanel.SetActive(false);
                return;
            }
            
            // Иначе возвращаемся в главное меню
            EventSystem.NavigateBack();
        }
        
        public override void Show()
        {
            base.Show();
            
            // Сбрасываем фильтры и поиск при показе
            _currentFilter = null;
            _currentSearchQuery = "";
            _searchInputField.text = "";
            UpdateFilterButtonsState();
            
            // Скрываем дополнительные панели
            if (_cardDetailsPanel != null)
            {
                _cardDetailsPanel.SetActive(false);
            }
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
            if (_cardDetailsPanel != null)
            {
                _cardDetailsPanel.SetActive(false);
            }
            if (_filterPanel != null)
            {
                _filterPanel.SetActive(false);
            }
        }
    }
}