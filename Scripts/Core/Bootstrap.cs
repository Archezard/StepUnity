using BasketballCards.Managers;
using BasketballCards.Services;
using BasketballCards.UI.Presenters;
using UnityEngine;

namespace BasketballCards.Core
{
    public class Bootstrap : MonoBehaviour
    {
        [Header("Core Managers")]
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private TelegramManager _telegramManager;
        [SerializeField] private PerformanceProfiler _performanceProfiler;
        
        [Header("Service References")]
        [SerializeField] private ApiClient _apiClient;
        
        [Header("Presenter References")]
        [SerializeField] private CollectionPresenter _collectionPresenter;
        [SerializeField] private ShopPresenter _shopPresenter;
        [SerializeField] private ActivitiesPresenter _activitiesPresenter;
        [SerializeField] private FiveOnFivePresenter _fiveOnFivePresenter;
        [SerializeField] private BattlePassPresenter _battlePassPresenter;
        [SerializeField] private ProfilePresenter _profilePresenter;
        
        private void Awake()
        {
            Debug.Log("Bootstrap: Initializing application...");
            
            if (!ValidateReferences())
            {
                Debug.LogError("Bootstrap: Missing required references!");
                return;
            }
            
            InitializeSystems();
            Debug.Log("Bootstrap: Initialization complete!");
        }
        
        private bool ValidateReferences()
        {
            if (_gameManager == null) _gameManager = FindFirstObjectByType<GameManager>();
            if (_uiManager == null) _uiManager = FindFirstObjectByType<UIManager>();
            if (_telegramManager == null) _telegramManager = FindFirstObjectByType<TelegramManager>();
            if (_performanceProfiler == null) _performanceProfiler = FindFirstObjectByType<PerformanceProfiler>();
            if (_apiClient == null) _apiClient = FindFirstObjectByType<ApiClient>();
            
            // Проверка презентеров, если вдруг обосрамс и кто-нибудь забудет ручками подключить
            if (_collectionPresenter == null) _collectionPresenter = FindFirstObjectByType<CollectionPresenter>();
            if (_shopPresenter == null) _shopPresenter = FindFirstObjectByType<ShopPresenter>();
            if (_activitiesPresenter == null) _activitiesPresenter = FindFirstObjectByType<ActivitiesPresenter>();
            if (_fiveOnFivePresenter == null) _fiveOnFivePresenter = FindFirstObjectByType<FiveOnFivePresenter>();
            if (_battlePassPresenter == null) _battlePassPresenter = FindFirstObjectByType<BattlePassPresenter>();
            if (_profilePresenter == null) _profilePresenter = FindFirstObjectByType<ProfilePresenter>();
            
            return _gameManager != null && _uiManager != null && 
                   _telegramManager != null && _performanceProfiler != null &&
                   _apiClient != null && _collectionPresenter != null &&
                   _shopPresenter != null && _activitiesPresenter != null &&
                   _fiveOnFivePresenter != null && _battlePassPresenter != null &&
                   _profilePresenter != null;
        }
        
        private void InitializeSystems()
        {
            _gameManager.Initialize(_apiClient);
            _uiManager.Initialize(_gameManager);
            _performanceProfiler.Initialize();
            
            // Инициализация презентеров
            _collectionPresenter.Initialize(_gameManager);
            _shopPresenter.Initialize(_gameManager);
            _activitiesPresenter.Initialize(_gameManager);
            //_fiveOnFivePresenter.Initialize(_gameManager);
            _battlePassPresenter.Initialize(_gameManager);
            _profilePresenter.Initialize(_gameManager);
            
            // Запуск приложения
            _telegramManager.Initialize(_gameManager);
        }
    }
}