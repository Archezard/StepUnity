using BasketballCards.UI.Presenters;
using UnityEngine;

namespace BasketballCards.Core
{
    public class NavigationService : MonoBehaviour
    {
        public static NavigationService Instance { get; private set; }
        
        private AppScreen _currentScreen;
        private AppScreen _previousScreen;
        
        // Ссылки на презентеры для управления их видимостью
        private CollectionPresenter _collectionPresenter;
        private ShopPresenter _shopPresenter;
        private ActivitiesPresenter _activitiesPresenter;
        private FiveOnFivePresenter _fiveOnFivePresenter;
        private BattlePassPresenter _battlePassPresenter;
        private ProfilePresenter _profilePresenter;
        private CardViewerPresenter _cardViewerPresenter;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            FindPresenters();
            
            EventSystem.OnNavigationRequested += HandleNavigationRequest;
            EventSystem.OnBackNavigationRequested += HandleBackNavigation;
        }
        
        private void OnDestroy()
        {
            EventSystem.OnNavigationRequested -= HandleNavigationRequest;
            EventSystem.OnBackNavigationRequested -= HandleBackNavigation;
        }
        
        private void FindPresenters()
        {
            _collectionPresenter = FindFirstObjectByType<CollectionPresenter>();
            _shopPresenter = FindFirstObjectByType<ShopPresenter>();
            _activitiesPresenter = FindFirstObjectByType<ActivitiesPresenter>();
            _fiveOnFivePresenter = FindFirstObjectByType<FiveOnFivePresenter>();
            _battlePassPresenter = FindFirstObjectByType<BattlePassPresenter>();
            _profilePresenter = FindFirstObjectByType<ProfilePresenter>();
            _cardViewerPresenter = FindFirstObjectByType<CardViewerPresenter>();
        }
        
        private void HandleNavigationRequest(AppScreen screen)
        {
            _previousScreen = _currentScreen;
            _currentScreen = screen;
            
            // Скрываем все презентеры
            HideAllPresenters();
            
            // Показываем запрошенный презентер
            switch (screen)
            {
                case AppScreen.Collection:
                    _collectionPresenter?.Show();
                    break;
                case AppScreen.Shop:
                    _shopPresenter?.Show();
                    break;
                case AppScreen.Activities:
                    _activitiesPresenter?.Show();
                    break;
                case AppScreen.FiveOnFive:
                    _fiveOnFivePresenter?.Show();
                    break;
                case AppScreen.BattlePass:
                    _battlePassPresenter?.Show();
                    break;
                case AppScreen.Profile:
                    _profilePresenter?.Show();
                    break;
            }
            
            Debug.Log($"Navigation: {_previousScreen} -> {_currentScreen}");
        }
        
        private void HandleBackNavigation()
        {
            // Если CardViewerPresenter активен, скрываем его
            if (_cardViewerPresenter != null && _cardViewerPresenter.isActiveAndEnabled)
            {
                _cardViewerPresenter.Hide();
                return;
            }
            
            // Иначе возвращаемся к предыдущему экрану
            if (_previousScreen != _currentScreen)
            {
                HandleNavigationRequest(_previousScreen);
            }
        }
        
        private void HideAllPresenters()
        {
            _collectionPresenter?.Hide();
            _shopPresenter?.Hide();
            _activitiesPresenter?.Hide();
            _fiveOnFivePresenter?.Hide();
            _battlePassPresenter?.Hide();
            _profilePresenter?.Hide();
            _cardViewerPresenter?.Hide();
        }
        
        public AppScreen GetCurrentScreen() => _currentScreen;
        public AppScreen GetPreviousScreen() => _previousScreen;
    }
}