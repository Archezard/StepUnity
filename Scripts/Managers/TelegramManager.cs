using BasketballCards.Core;
using UnityEngine;

namespace BasketballCards.Managers
{
    public class TelegramManager : MonoBehaviour
    {
        public static TelegramManager Instance { get; private set; }
        
        private GameManager _gameManager;
        
        public void Initialize(GameManager gameManager)
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            _gameManager = gameManager;
            Debug.Log("TelegramManager: Initialized");
        }
        
        public void StartApplication()
        {
            Debug.Log("TelegramManager: Starting application...");
            
            // Получение данных из Telegram
            string username = GetUsernameFromTelegram();
            
            // Загрузка данных пользователя
            _gameManager.UserService.GetUserData(
                username,
                userData => {
                    _gameManager.SetCurrentUser(userData);
                    Debug.Log("TelegramManager: User data loaded successfully");
                    
                    // После загрузки данных показываем основной интерфейс
                    OnUserDataLoaded();
                },
                error => {
                    Debug.LogError("TelegramManager: Failed to load user data: " + error);
                }
            );
        }
        
        private void OnUserDataLoaded()
        {
            Debug.Log("TelegramManager: User data loaded, showing UI...");
            
            // Здесь надо будет потом дополнить и показать основной интерфейс
            UIManager.Instance.ShowProfile();
        }
        
        private string GetUsernameFromTelegram()
        {
            // Здесь будет код для получения данных из Telegram Web App
            // Временная заглушка для тестирования короче
            return "test_user";
        }
    }
}