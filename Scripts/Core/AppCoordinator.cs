using BasketballCards.Services;
using UnityEngine;

namespace BasketballCards.Core
{
    public class AppCoordinator : MonoBehaviour
    {
        public static AppCoordinator Instance { get; private set; }
        
        // Сервисы
        public ApiClient ApiClient { get; private set; }
        public UserService UserService { get; private set; }
        public CardService CardService { get; private set; }
        public ShopService ShopService { get; private set; }
        public CraftService CraftService { get; private set; }
        public TradeService TradeService { get; private set; }
        public BattlePassService BattlePassService { get; private set; }
        public ActivitiesService ActivitiesService { get; private set; }
        public FiveOnFiveService FiveOnFiveService { get; private set; }
        public AlbumService AlbumService { get; private set; }

        // Флаг инициализации
        private bool _isInitialized = false;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            InitializeServices();
            SetupEventHandlers();
            
            _isInitialized = true;
            Debug.Log("AppCoordinator: Initialization complete");
        }
        
        private void InitializeServices()
        {
            Debug.Log("AppCoordinator: Initializing services...");
            
            if (ApiClient == null)
            {
                ApiClient = gameObject.GetComponent<ApiClient>();
                if (ApiClient == null)
                {
                    ApiClient = gameObject.AddComponent<ApiClient>();
                }
            }
            
            UserService = new UserService(ApiClient);
            CardService = new CardService(ApiClient);
            ShopService = new ShopService(ApiClient);
            CraftService = new CraftService(ApiClient);
            TradeService = new TradeService(ApiClient);
            BattlePassService = new BattlePassService(ApiClient);
            ActivitiesService = new ActivitiesService(ApiClient);
            FiveOnFiveService = new FiveOnFiveService(ApiClient);
            AlbumService = new AlbumService(ApiClient);
            
            Debug.Log("AppCoordinator: Services initialized");
        }
        
        private void SetupEventHandlers()
        {
            // Подписка на системные события
            EventSystem.OnErrorOccurred += HandleError;
            EventSystem.OnSuccessMessage += HandleSuccessMessage;
        }
        
        private void HandleError(string errorMessage)
        {
            Debug.LogError($"App Error: {errorMessage}");
            // Здесь добавить показ UI ошибки
        }
        
        private void HandleSuccessMessage(string message)
        {
            Debug.Log($"App Success: {message}");
            // Здесь добавить показ UI успешного сообщения
        }
        
        public void StartApplication()
        {
            if (!_isInitialized)
            {
                Debug.LogError("AppCoordinator: Cannot start application - not initialized!");
                return;
            }

            Debug.Log("AppCoordinator: Starting application...");
            
            // Проверяем, что UserService инициализирован
            if (UserService == null)
            {
                Debug.LogError("AppCoordinator: UserService is null!");
                return;
            }
            
            // Загрузка данных пользователя
            UserService.GetUserData("test_user", 
                userData => {
                    if (UserDataManager.Instance != null)
                    {
                        UserDataManager.Instance.UpdateUserData(userData);
                        EventSystem.NavigateTo(AppScreen.Collection);
                    }
                    else
                    {
                        Debug.LogError("AppCoordinator: UserDataManager.Instance is null!");
                        CreateDefaultUser();
                    }
                },
                error => {
                    EventSystem.ShowError($"Failed to load user data: {error}");
                    // Создаем пользователя по умолчанию в случае ошибки, но лучше тут сделать что-то другое
                    CreateDefaultUser();
                });
        }
        
        private void CreateDefaultUser()
        {
            var defaultUser = new BasketballCards.Models.UserData
            {
                user_id = 1,
                username = "Player1",
                gold = 1000,
                diamonds = 100,
                tickets = 5
            };
            
            if (UserDataManager.Instance != null)
            {
                UserDataManager.Instance.UpdateUserData(defaultUser);
                EventSystem.NavigateTo(AppScreen.Collection);
            }
            else
            {
                Debug.LogError("AppCoordinator: Cannot create default user - UserDataManager is null!");
            }
        }

        // Метод для проверки готовности
        public bool IsInitialized()
        {
            return _isInitialized;
        }
    }
}