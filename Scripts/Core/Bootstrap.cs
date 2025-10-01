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
            
            // Инициализация в правильном порядке
            InitializeCoreSystems();
            InitializeServices();
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
                   _navigationService != null && _telegramManager != null;
        }
        
        private void InitializeCoreSystems()
        {
            Debug.Log("Bootstrap: Initializing core systems...");
            
            // 1. Сначала гарантируем, что UserDataManager готов
            if (_userDataManager != null)
            {
                Debug.Log("Bootstrap: UserDataManager verified");
            }
            
            // 2. Затем инициализируем AppCoordinator
            if (_appCoordinator != null)
            {
                _appCoordinator.Initialize();
            }
            
            // 3. Затем остальные системы
            _performanceProfiler?.Initialize();
            _telegramManager?.Initialize();
        }
        
        private void InitializeServices()
        {
            Debug.Log("Bootstrap: Setting up service dependencies...");
            
            if (_telegramManager != null && _appCoordinator != null)
            {
                _telegramManager.SetAppCoordinator(_appCoordinator);
            }
        }
        
        private void StartApplication()
        {
            Debug.Log("Bootstrap: Starting application...");
            
            // Проверяем, что AppCoordinator полностью инициализирован
            if (_appCoordinator != null && _appCoordinator.IsInitialized())
            {
                if (_telegramManager != null)
                {
                    _telegramManager.StartApplication();
                }
                else
                {
                    _appCoordinator.StartApplication();
                }
            }
            else
            {
                Debug.LogError("Bootstrap: AppCoordinator is not initialized!");
                // Альтернативный запуск с задержкой
                StartCoroutine(DelayedStart());
            }
        }
        
        private System.Collections.IEnumerator DelayedStart()
        {
            Debug.LogWarning("Bootstrap: Retrying application start...");
            yield return new WaitForSeconds(1f);
            
            if (_appCoordinator != null && _appCoordinator.IsInitialized())
            {
                _appCoordinator.StartApplication();
            }
            else
            {
                Debug.LogError("Bootstrap: Failed to start application after retry!");
            }
        }
    }
}