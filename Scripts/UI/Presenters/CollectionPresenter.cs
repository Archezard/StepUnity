using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Views;
using System.Collections;
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
        
        // Сервисы
        private CardService _cardService;
        private CraftService _craftService;
        private AlbumService _albumService;
        private TradeService _tradeService;
        
        // Флаги инициализации
        private bool _areServicesInitialized = false;
        private bool _isWaitingForServices = false;
        
        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            
            EventSystem.OnCollectionSubScreenChanged += HandleSubScreenChanged;
            EventSystem.OnCardUpgraded += HandleCardUpgraded;
            EventSystem.OnCardsCrafted += HandleCardsCrafted;
            EventSystem.OnCardsDisassembled += HandleCardsDisassembled;
            EventSystem.OnErrorOccurred += HandleError;
        }
        
        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            
            EventSystem.OnCollectionSubScreenChanged -= HandleSubScreenChanged;
            EventSystem.OnCardUpgraded -= HandleCardUpgraded;
            EventSystem.OnCardsCrafted -= HandleCardsCrafted;
            EventSystem.OnCardsDisassembled -= HandleCardsDisassembled;
            EventSystem.OnErrorOccurred -= HandleError;
        }
        
        protected override void OnUserDataManagerReady()
        {
            InitializeServices();
        }
        
        private void InitializeServices()
        {
            if (_areServicesInitialized) return;
            
            var appCoordinator = AppCoordinator.Instance;
            if (appCoordinator == null || !appCoordinator.IsInitialized())
            {
                Debug.LogWarning("CollectionPresenter: AppCoordinator not ready, waiting...");
                StartCoroutine(WaitForAppCoordinator());
                return;
            }
            
            // Получаем сервисы
            _cardService = appCoordinator.CardService;
            _craftService = appCoordinator.CraftService;
            _albumService = appCoordinator.AlbumService;
            _tradeService = appCoordinator.TradeService;
            
            if (_cardService == null)
            {
                Debug.LogError("CollectionPresenter: CardService is null after AppCoordinator initialization!");
                return;
            }
            
            _areServicesInitialized = true;
            Debug.Log("CollectionPresenter: Services initialized successfully");
            
            // Загружаем данные если мы активны
            if (IsActive())
            {
                LoadDataForCurrentSubScreen();
            }
        }
        
        private IEnumerator WaitForAppCoordinator()
        {
            if (_isWaitingForServices) yield break;
            
            _isWaitingForServices = true;
            int maxAttempts = 50; // 5 секунд максимум
            int attempts = 0;
            
            while (attempts < maxAttempts)
            {
                attempts++;
                var appCoordinator = AppCoordinator.Instance;
                
                if (appCoordinator != null && appCoordinator.IsInitialized())
                {
                    _isWaitingForServices = false;
                    InitializeServices();
                    yield break;
                }
                
                yield return new WaitForSeconds(0.1f);
            }
            
            _isWaitingForServices = false;
            Debug.LogError("CollectionPresenter: Timeout waiting for AppCoordinator initialization!");
        }
        
        private void Start()
        {
            InitializeHeader();
            InitializeSubViews();
        }
        
        private void InitializeHeader()
        {
            if (_headerView != null)
            {
                _headerView.Initialize();
                _headerView.OnSubScreenSelected += HandleHeaderSubScreenSelected;
            }
        }
        
        private void InitializeSubViews()
        {
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
            
            ShowSubView(GetViewForSubScreen(_currentSubScreen));
        }
        
        public override void Show()
        {
            if (_headerView != null)
                _headerView.gameObject.SetActive(true);
            
            if (_currentSubView != null)
                _currentSubView.Show();
            
            // Инициализируем сервисы если еще не инициализированы
            if (!_areServicesInitialized)
            {
                InitializeServices();
            }
            else
            {
                // Если сервисы уже инициализированы, загружаем данные
                LoadDataForCurrentSubScreen();
            }
        }
        
        public override void Hide()
        {
            if (_headerView != null)
                _headerView.gameObject.SetActive(false);
            
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
            
            // Загружаем данные для нового подраздела
            LoadDataForCurrentSubScreen();
        }
        
        private void ShowSubView(BaseView view)
        {
            if (view == null) return;
            
            if (_currentSubView != null)
            {
                _currentSubView.Hide();
            }
            
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
        
        private void LoadDataForCurrentSubScreen()
        {
            if (!_areServicesInitialized)
            {
                Debug.LogWarning("CollectionPresenter: Services not initialized, skipping data load");
                return;
            }
            
            switch (_currentSubScreen)
            {
                case CollectionSubScreen.Collection:
                case CollectionSubScreen.Workshop:
                    LoadUserCards();
                    break;
                case CollectionSubScreen.Album:
                    LoadAlbums();
                    break;
                case CollectionSubScreen.Exchange:
                    LoadTradeOffers();
                    break;
            }
        }
        
        // PUBLIC METHODS CALLED FROM VIEWS
        
        public void LoadUserCards()
        {
            if (!_areServicesInitialized || _cardService == null)
            {
                Debug.LogError("CollectionPresenter: CardService is not available");
                return;
            }
            
            _cardService.GetUserCards(
                cards => {
                    if (_currentSubView is ICollectionView collectionView)
                    {
                        collectionView.DisplayCards(cards);
                    }
                },
                error => {
                    EventSystem.ShowError($"Failed to load user cards: {error}");
                });
        }
        
        public void CraftCards(List<string> cardIds)
        {
            if (!_areServicesInitialized || _craftService == null)
            {
                Debug.LogError("CollectionPresenter: CraftService is not available");
                return;
            }
            
            _craftService.CraftCards(cardIds,
                craftedCard => {
                    if (_currentSubView is IWorkshopView workshopView)
                    {
                        workshopView.OnCraftSuccess(craftedCard);
                    }
                    EventSystem.ShowSuccess($"Успешно скрафчена карта: {craftedCard.PlayerName}");
                    
                    // Обновляем данные пользователя
                    UpdateUserData();
                },
                error => {
                    EventSystem.ShowError($"Крафт не удался: {error}");
                });
        }
        
        public void DisassembleCards(List<string> cardIds)
        {
            if (!_areServicesInitialized || _craftService == null)
            {
                Debug.LogError("CollectionPresenter: CraftService is not available");
                return;
            }
            
            _craftService.DisassembleCards(cardIds,
                result => {
                    if (_currentSubView is IWorkshopView workshopView)
                    {
                        workshopView.OnDisassembleSuccess(result.Gold, result.Dust);
                    }
                    EventSystem.ShowSuccess($"Разбор успешен! Получено: {result.Gold} золота");
                    
                    // Обновляем данные пользователя
                    UpdateUserData();
                },
                error => {
                    EventSystem.ShowError($"Разбор не удался: {error}");
                });
        }
        
        public void LoadAlbums()
        {
            if (!_areServicesInitialized || _albumService == null)
            {
                Debug.LogError("CollectionPresenter: AlbumService is not available");
                return;
            }
            
            _albumService.GetUserAlbums(
                albums => {
                    // Конвертируем AlbumInfo в AlbumData для View
                    var albumDataList = new List<AlbumData>();
                    foreach (var albumInfo in albums)
                    {
                        albumDataList.Add(new AlbumData 
                        { 
                            Id = albumInfo.Id, 
                            Name = albumInfo.Name, 
                            Type = albumInfo.Type 
                        });
                    }
                    
                    if (_currentSubView is IAlbumView albumView)
                    {
                        albumView.DisplayAlbums(albumDataList);
                    }
                },
                error => {
                    EventSystem.ShowError($"Failed to load albums: {error}");
                });
        }
        
        public void LoadTradeOffers()
        {
            if (!_areServicesInitialized || _tradeService == null)
            {
                Debug.LogError("CollectionPresenter: TradeService is not available");
                return;
            }
            
            _tradeService.GetTradeOffers(
                offersData => {
                    // Конвертируем TradeOfferData в TradeOffer для View
                    var tradeOffers = new List<TradeOffer>();
                    foreach (var offerData in offersData)
                    {
                        tradeOffers.Add(new TradeOffer
                        {
                            OfferId = offerData.OfferId,
                            FromUserId = offerData.FromUserId,
                            FromUsername = offerData.FromUsername,
                            OfferedCards = offerData.OfferedCards,
                            RequestedCards = offerData.RequestedCards,
                            Status = offerData.Status
                        });
                    }
                    
                    if (_currentSubView is IExchangeView exchangeView)
                    {
                        exchangeView.DisplayTradeOffers(tradeOffers);
                    }
                },
                error => {
                    EventSystem.ShowError($"Failed to load trade offers: {error}");
                });
        }
        
        public void OnCardSelectedInView(CardData card)
        {
            EventSystem.RequestCardView(card);
        }
        
        public void OnUpgradeCardRequestedInView(CardData card)
        {
            if (!_areServicesInitialized || _cardService == null) return;
            
            _cardService.UpgradeCard(card.CardId,
                upgradedCard => {
                    EventSystem.UpgradeCard(upgradedCard);
                    UpdateUserData();
                },
                error => {
                    EventSystem.ShowError(error);
                });
        }
        
        // EVENT HANDLERS
        
        private void HandleCardUpgraded(CardData card)
        {
            // При улучшении карты перезагружаем коллекцию
            if (_areServicesInitialized)
            {
                LoadUserCards();
            }
        }
        
        private void HandleCardsCrafted(List<CardData> cards)
        {
            // При успешном крафте перезагружаем коллекцию
            if (_areServicesInitialized)
            {
                LoadUserCards();
            }
        }
        
        private void HandleCardsDisassembled(List<CardData> cards)
        {
            // При разборе перезагружаем коллекцию
            if (_areServicesInitialized)
            {
                LoadUserCards();
            }
        }
        
        private void HandleError(string error)
        {
            if (_currentSubView != null)
            {
                _currentSubView.ShowError(error);
            }
        }
        
        private void UpdateUserData()
        {
            var userService = AppCoordinator.Instance?.UserService;
            if (userService != null && UserDataManager.Instance != null)
            {
                userService.GetUserData(
                    UserDataManager.Instance.CurrentUser.username,
                    userData => UserDataManager.Instance.UpdateUserData(userData),
                    error => EventSystem.ShowError("Failed to update user data")
                );
            }
        }
        
        // Вспомогательные методы
        public bool AreServicesReady()
        {
            return _areServicesInitialized;
        }
    }
}