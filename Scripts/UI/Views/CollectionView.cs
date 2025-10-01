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
        [SerializeField] private Button _filterButton;
        [SerializeField] private TMP_InputField _searchInputField;
        [SerializeField] private TextMeshProUGUI _searchModeText;
        [SerializeField] private Button _searchModeButton;
        [SerializeField] private TextMeshProUGUI _loadingText;
        [SerializeField] private Button _retryButton;

        [Header("Filter UI")]
        [SerializeField] private GameObject _filterPanel;
        [SerializeField] private Button _allRaritiesButton;
        [SerializeField] private Button _bronzeFilterButton;
        [SerializeField] private Button _silverFilterButton;
        [SerializeField] private Button _goldFilterButton;
        [SerializeField] private Button _diamondFilterButton;
        [SerializeField] private Button _legendaryFilterButton;
        [SerializeField] private Button _sortByNameButton;
        [SerializeField] private Button _sortByLevelButton;

        private CollectionPresenter _presenter;
        private List<CardItemView> _cardViews = new List<CardItemView>();
        private List<CardData> _currentCards;
        private Rarity? _currentRarityFilter = null;
        private string _currentSearchQuery = "";
        private bool _searchByTeam = false;
        private bool _sortByName = true;

        public void Initialize(CollectionPresenter presenter)
        {
            _presenter = presenter;

            // Инициализация кнопок
            _filterButton.onClick.AddListener(OnFilterButtonClicked);
            _searchInputField.onValueChanged.AddListener(OnSearchInputChanged);
            _searchModeButton.onClick.AddListener(OnSearchModeButtonClicked);
            _retryButton.onClick.AddListener(OnRetryButtonClicked);

            // Инициализация фильтров
            InitializeFilters();

            // Скрываем панель фильтров
            _filterPanel.SetActive(false);

            UpdateSearchModeText();
            ShowLoadingState(true);
        }

        private void InitializeFilters()
        {
            _allRaritiesButton.onClick.AddListener(() => SetRarityFilter(null));
            _bronzeFilterButton.onClick.AddListener(() => SetRarityFilter(Rarity.Bronze));
            _silverFilterButton.onClick.AddListener(() => SetRarityFilter(Rarity.Silver));
            _goldFilterButton.onClick.AddListener(() => SetRarityFilter(Rarity.Gold));
            _diamondFilterButton.onClick.AddListener(() => SetRarityFilter(Rarity.Diamond));
            _legendaryFilterButton.onClick.AddListener(() => SetRarityFilter(Rarity.Legendary));

            _sortByNameButton.onClick.AddListener(() => SetSorting(true));
            _sortByLevelButton.onClick.AddListener(() => SetSorting(false));
        }

        public void DisplayCards(List<CardData> cards)
        {
            _currentCards = cards;
            ShowLoadingState(false);
            ApplyFiltersAndSearch();
        }

        private void ShowLoadingState(bool isLoading, string message = "Загрузка карточек...")
        {
            _loadingText.text = message;
            _loadingText.gameObject.SetActive(isLoading);
            _retryButton.gameObject.SetActive(!isLoading && _currentCards == null);
            _scrollRect.gameObject.SetActive(!isLoading && _currentCards != null);
        }

        private void OnRetryButtonClicked()
        {
            ShowLoadingState(true, "Повторная загрузка...");
            _presenter?.LoadUserCards();
        }

        private void ApplyFiltersAndSearch()
        {
            ClearCards();

            var filteredCards = _currentCards;

            // Применяем фильтр по редкости
            if (_currentRarityFilter.HasValue)
            {
                filteredCards = filteredCards.FindAll(c => c.Rarity == _currentRarityFilter.Value);
            }

            // Применяем поиск
            if (!string.IsNullOrEmpty(_currentSearchQuery))
            {
                if (_searchByTeam)
                {
                    filteredCards = filteredCards.FindAll(c =>
                        c.Team?.ToLower().Contains(_currentSearchQuery.ToLower()) == true);
                }
                else
                {
                    filteredCards = filteredCards.FindAll(c =>
                        c.PlayerName.ToLower().Contains(_currentSearchQuery.ToLower()));
                }
            }

            // Сортируем карточки
            if (_sortByName)
            {
                filteredCards.Sort((a, b) => string.Compare(a.PlayerName, b.PlayerName));
            }
            else
            {
                filteredCards.Sort((a, b) => b.Level.CompareTo(a.Level));
            }

            // Создаем карточки в UI
            foreach (var card in filteredCards)
            {
                var cardObject = Instantiate(_cardPrefab, _cardsGrid.transform);
                var cardView = cardObject.GetComponent<CardItemView>();
                if (cardView != null)
                {
                    cardView.Initialize(card, OnCardSelected, null); // В коллекции нет множественного выбора
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
            // В коллекции при выборе карточки сразу открываем просмотр
            EventSystem.RequestCardView(card);
        }

        private void OnFilterButtonClicked()
        {
            _filterPanel.SetActive(!_filterPanel.activeSelf);
        }

        private void SetRarityFilter(Rarity? rarity)
        {
            _currentRarityFilter = rarity;
            ApplyFiltersAndSearch();
            _filterPanel.SetActive(false);
        }

        private void OnSearchInputChanged(string searchQuery)
        {
            _currentSearchQuery = searchQuery;
            ApplyFiltersAndSearch();
        }

        private void OnSearchModeButtonClicked()
        {
            _searchByTeam = !_searchByTeam;
            UpdateSearchModeText();
            ApplyFiltersAndSearch(); // Переприменяем поиск с новым режимом
        }

        private void UpdateSearchModeText()
        {
            _searchModeText.text = _searchByTeam ? "По команде" : "По имени";
        }

        private void SetSorting(bool sortByName)
        {
            _sortByName = sortByName;
            ApplyFiltersAndSearch();
            _filterPanel.SetActive(false);
        }

        public override void ShowError(string message)
        {
            base.ShowError(message);
            ShowLoadingState(false, $"Ошибка: {message}");
        }

        public override void Show()
        {
            base.Show();

            // Сбрасываем фильтры и поиск при показе
            _currentRarityFilter = null;
            _currentSearchQuery = "";
            _searchInputField.text = "";
            _searchByTeam = false;
            _sortByName = true;

            UpdateSearchModeText();
            _filterPanel.SetActive(false);

            // Проверяем доступность сервисов перед загрузкой
            if (_presenter != null && _presenter.AreServicesReady())
            {
                ShowLoadingState(true);
                _presenter.LoadUserCards();
            }
            else
            {
                ShowLoadingState(false, "Сервисы не готовы...");
            }
        }

        public override void Hide()
        {
            base.Hide();
            _filterPanel.SetActive(false);
        }
    }
}