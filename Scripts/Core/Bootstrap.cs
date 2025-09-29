using BasketballCards.Managers;
using UnityEngine;

namespace BasketballCards.Core
{
    public class Bootstrap : MonoBehaviour
    {
        [Header("Core Systems")]
        [SerializeField] private AppCoordinator _appCoordinator;
        [SerializeField] private UserDataManager _userDataManager;
        [SerializeField] private NavigationService _navigationService;
        [SerializeField] private TelegramManager _telegramManager;
        [SerializeField] private PerformanceProfiler _performanceProfiler;
        
        private void Awake()
        {
            Debug.Log("Bootstrap: Initializing application...");
            
            if (!ValidateReferences())
            {
                Debug.LogError("Bootstrap: Missing required references!");
                return;
            }
            
            InitializeSystems();
            StartApplication();
            
            Debug.Log("Bootstrap: Initialization complete!");
        }
        
        private bool ValidateReferences()
        {
            if (_appCoordinator == null) _appCoordinator = FindFirstObjectByType<AppCoordinator>();
            if (_userDataManager == null) _userDataManager = FindFirstObjectByType<UserDataManager>();
            if (_navigationService == null) _navigationService = FindFirstObjectByType<NavigationService>();
            if (_telegramManager == null) _telegramManager = FindFirstObjectByType<TelegramManager>();
            if (_performanceProfiler == null) _performanceProfiler = FindFirstObjectByType<PerformanceProfiler>();
            
            return _appCoordinator != null && _userDataManager != null && 
                   _navigationService != null && _telegramManager != null &&
                   _performanceProfiler != null;
        }
        
        private void InitializeSystems()
        {
            _performanceProfiler.Initialize();
            _telegramManager.Initialize();
        }
        
        private void StartApplication()
        {
            // Запуск через Telegram Manager или напрямую
            if (_telegramManager != null)
            {
                _telegramManager.StartApplication();
            }
            else
            {
                _appCoordinator.StartApplication();
            }
        }
    }
}