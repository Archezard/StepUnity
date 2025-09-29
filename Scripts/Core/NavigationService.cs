using UnityEngine;

namespace BasketballCards.Core
{
    public class NavigationService : MonoBehaviour
    {
        public static NavigationService Instance { get; private set; }
        
        private AppScreen _currentScreen;
        private AppScreen _previousScreen;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Подписка на навигационные события
            EventSystem.OnNavigationRequested += HandleNavigationRequest;
            EventSystem.OnBackNavigationRequested += HandleBackNavigation;
        }
        
        private void OnDestroy()
        {
            EventSystem.OnNavigationRequested -= HandleNavigationRequest;
            EventSystem.OnBackNavigationRequested -= HandleBackNavigation;
        }
        
        private void HandleNavigationRequest(AppScreen screen)
        {
            _previousScreen = _currentScreen;
            _currentScreen = screen;
            
            // Здесь будет логика переключения между экранами
            Debug.Log($"Navigation: {_previousScreen} -> {_currentScreen}");
            
            // Вызываем события для презентеров
            switch (screen)
            {
                case AppScreen.Collection:
                    // CollectionPresenter.Show();
                    break;
                case AppScreen.Shop:
                    // ShopPresenter.Show();
                    break;
                case AppScreen.Activities:
                    // ActivitiesPresenter.Show();
                    break;
                case AppScreen.FiveOnFive:
                    // FiveOnFivePresenter.Show();
                    break;
                case AppScreen.BattlePass:
                    // BattlePassPresenter.Show();
                    break;
                case AppScreen.Profile:
                    // ProfilePresenter.Show();
                    break;
            }
        }
        
        private void HandleBackNavigation()
        {
            if (_previousScreen != _currentScreen)
            {
                HandleNavigationRequest(_previousScreen);
            }
        }
        
        public AppScreen GetCurrentScreen() => _currentScreen;
        public AppScreen GetPreviousScreen() => _previousScreen;
    }
}