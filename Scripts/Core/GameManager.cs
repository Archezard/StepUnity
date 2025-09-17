using BasketballCards.Managers;
using BasketballCards.Models;
using BasketballCards.Services;
using UnityEngine;

namespace BasketballCards.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        public UserData CurrentUser { get; private set; }
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
        
        public void Initialize(ApiClient apiClient)
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            ApiClient = apiClient;
            InitializeServices();
            
            Debug.Log("GameManager: Initialized");
        }
        
        private void InitializeServices()
        {
            Debug.Log("GameManager: Initializing services...");
            
            UserService = new UserService(ApiClient);
            CardService = new CardService(ApiClient);
            ShopService = new ShopService(ApiClient);
            CraftService = new CraftService(ApiClient);
            TradeService = new TradeService(ApiClient);
            BattlePassService = new BattlePassService(ApiClient);
            ActivitiesService = new ActivitiesService(ApiClient);
            FiveOnFiveService = new FiveOnFiveService(ApiClient);
            AlbumService = new AlbumService(ApiClient);
            
            Debug.Log("GameManager: Services initialized");
        }
        
        public void SetCurrentUser(UserData userData)
        {
            CurrentUser = userData;
            Debug.Log($"GameManager: Current user set to {userData.username}");
            
            UIManager.Instance.UpdateUserData(userData);
        }
    }
}