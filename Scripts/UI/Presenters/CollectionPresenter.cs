using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Views;
using System.Collections.Generic;
using UnityEngine;

namespace BasketballCards.UI.Presenters
{
    public class CollectionPresenter : BasePresenter
    {
        [Header("Header Reference")]
        [SerializeField] private CollectionHeaderView _headerView;
        
        [Header("SubScreen Views")]
        [SerializeField] private CollectionView _collectionView;
        [SerializeField] private WorkshopView _workshopView;
        [SerializeField] private AlbumView _albumView;
        [SerializeField] private ExchangeView _exchangeView;
        
        private List<BaseView> _subViews = new List<BaseView>();
        private BaseView _currentSubView;
        private CollectionSubScreen _currentSubScreen = CollectionSubScreen.Collection;
        
        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            
            EventSystem.OnCollectionSubScreenChanged += HandleSubScreenChanged;
            EventSystem.OnCardUpgraded += HandleCardUpgraded;
            EventSystem.OnCardsCrafted += HandleCardsCrafted;
            EventSystem.OnCardsDisassembled += HandleCardsDisassembled;
            EventSystem.OnErrorOccurred += HandleError;
            
            if (UserDataManager.Instance != null)
            {
                UserDataManager.Instance.OnUserDataUpdated += HandleUserDataUpdated;
                UserDataManager.Instance.OnCurrencyChanged += HandleCurrencyChanged;
            }
            else
            {
                Debug.LogWarning("CollectionPresenter: UserDataManager.Instance is null during subscription");
            }
        }
        
        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            
            EventSystem.OnCollectionSubScreenChanged -= HandleSubScreenChanged;
            EventSystem.OnCardUpgraded -= HandleCardUpgraded;
            EventSystem.OnCardsCrafted -= HandleCardsCrafted;
            EventSystem.OnCardsDisassembled -= HandleCardsDisassembled;
            EventSystem.OnErrorOccurred -= HandleError;
            
            if (UserDataManager.Instance != null)
            {
                UserDataManager.Instance.OnUserDataUpdated -= HandleUserDataUpdated;
                UserDataManager.Instance.OnCurrencyChanged -= HandleCurrencyChanged;
            }
        }
        
        private void Start()
        {
            InitializeHeader();
            InitializeSubViews();
            
            // Загрузка начальных данных
            LoadUserCards();
        }
        
        private void InitializeHeader()
        {
            if (_headerView != null)
            {
                _headerView.Initialize();
                _headerView.OnSubScreenSelected += HandleHeaderSubScreenSelected;
            }
            else
            {
                Debug.LogError("CollectionPresenter: HeaderView reference is null!");
            }
        }
        
        private void InitializeSubViews()
        {
            // Собираем все подпредставления
            if (_collectionView != null)
            {
                _collectionView.Initialize(this);
                _subViews.Add(_collectionView);
            }
            
            if (_workshopView != null)
            {
                _workshopView.Initialize(this);
                _subViews.Add(_workshopView);
            }
            
            if (_albumView != null)
            {
                _albumView.Initialize(this);
                _subViews.Add(_albumView);
            }
            
            if (_exchangeView != null)
            {
                _exchangeView.Initialize(this);
                _subViews.Add(_exchangeView);
            }
            
            // Показываем начальное подпредставление
            ShowSubView(GetViewForSubScreen(_currentSubScreen));
        }
        
        public override void Show()
        {
            // Показываем хедер
            if (_headerView != null)
                _headerView.gameObject.SetActive(true);
            
            // Показываем текущее подпредставление
            if (_currentSubView != null)
                _currentSubView.Show();
        }
        
        public override void Hide()
        {
            // Скрываем хедер
            if (_headerView != null)
                _headerView.gameObject.SetActive(false);
            
            // Скрываем все подпредставления
            foreach (var view in _subViews)
            {
                if (view != null)
                    view.Hide();
            }
        }
        
        private void HandleHeaderSubScreenSelected(CollectionSubScreen subScreen)
        {
            EventSystem.ChangeCollectionSubScreen(subScreen);
        }
        
        private void HandleSubScreenChanged(CollectionSubScreen subScreen)
        {
            _currentSubScreen = subScreen;
            var targetView = GetViewForSubScreen(subScreen);
            ShowSubView(targetView);
        }
        
        private void ShowSubView(BaseView view)
        {
            if (view == null) return;
            
            // Скрываем текущее подпредставление
            if (_currentSubView != null)
            {
                _currentSubView.Hide();
            }
            
            // Показываем новое подпредставление
            _currentSubView = view;
            _currentSubView.Show();
        }
        
        private BaseView GetViewForSubScreen(CollectionSubScreen subScreen)
        {
            switch (subScreen)
            {
                case CollectionSubScreen.Collection: return _collectionView;
                case CollectionSubScreen.Workshop: return _workshopView;
                case CollectionSubScreen.Album: return _albumView;
                case CollectionSubScreen.Exchange: return _exchangeView;
                default: return _collectionView;
            }
        }
        
        protected override void HandleUserDataUpdated(UserData userData)
        {
            // При обновлении данных пользователя перезагружаем карточки
            LoadUserCards();
        }
        
        private void LoadUserCards()
        {
            var cardService = AppCoordinator.Instance?.CardService;
            if (cardService == null)
            {
                Debug.LogError("CollectionPresenter: CardService is not available");
                return;
            }
            
            cardService.GetUserCards(
                cards => {
                    if (_collectionView != null)
                    {
                        _collectionView.DisplayCards(cards);
                    }
                },
                error => {
                    EventSystem.ShowError($"Failed to load user cards: {error}");
                });
        }
        
        private void HandleCardUpgraded(CardData card)
        {
            // Обновление карточки после улучшения
            if (_currentSubView is ICollectionView collectionView)
            {
                collectionView.OnCardUpgraded(card);
            }
        }
        
        private void HandleCardsCrafted(List<CardData> cards)
        {
            // Добавление скрафченных карточек в коллекцию
            if (_currentSubView is ICollectionView collectionView)
            {
                collectionView.OnCardCrafted(cards[0]);
            }
        }
        
        private void HandleCardsDisassembled(List<CardData> cards)
        {
            // Удаление разобранных карточек из коллекции
            if (_currentSubView is ICollectionView collectionView)
            {
                // Здесь нужно обновить отображение коллекции
            }
        }
        
        private void HandleCurrencyChanged(int oldGold, int newGold)
        {
            // Обновление UI при изменении валюты
        }
        
        private void HandleError(string error)
        {
            if (_currentSubView != null)
            {
                _currentSubView.ShowError(error);
            }
        }
        
        // Методы, вызываемые из под-представлений
        public void OnCardSelectedInView(CardData card)
        {
            // Прямой вызов CardViewer через EventSystem
            EventSystem.RequestCardView(card);
        }
        
        public void OnUpgradeCardRequestedInView(CardData card)
        {
            var cardService = AppCoordinator.Instance?.CardService;
            if (cardService == null)
            {
                Debug.LogError("CollectionPresenter: CardService is not available");
                return;
            }
            
            cardService.UpgradeCard(card.CardId,
                upgradedCard => {
                    EventSystem.UpgradeCard(upgradedCard);
                    
                    // Обновляем данные пользователя
                    var userService = AppCoordinator.Instance?.UserService;
                    if (userService != null && UserDataManager.Instance != null)
                    {
                        userService.GetUserData(
                            UserDataManager.Instance.CurrentUser.username,
                            userData => UserDataManager.Instance.UpdateUserData(userData),
                            error => EventSystem.ShowError("Failed to update user data")
                        );
                    }
                },
                error => {
                    EventSystem.ShowError(error);
                });
        }
    }
}