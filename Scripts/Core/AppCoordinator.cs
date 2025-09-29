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
        }
        
        private void InitializeServices()
        {
            Debug.Log("AppCoordinator: Initializing services...");
            
            ApiClient = gameObject.AddComponent<ApiClient>();
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
            // Здесь можно добавить показ UI ошибки
        }
        
        private void HandleSuccessMessage(string message)
        {
            Debug.Log($"App Success: {message}");
            // Здесь можно добавить показ UI успешного сообщения
        }
        
        public void StartApplication()
        {
            Debug.Log("AppCoordinator: Starting application...");
            
            // Загрузка данных пользователя
            UserService.GetUserData("test_user", 
                userData => {
                    UserDataManager.Instance.UpdateUserData(userData);
                    EventSystem.NavigateTo(AppScreen.Collection);
                },
                error => {
                    EventSystem.ShowError($"Failed to load user data: {error}");
                });
        }
    }
}