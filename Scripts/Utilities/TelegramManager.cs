using BasketballCards.Core;
using UnityEngine;

namespace BasketballCards.Managers
{
    public class TelegramManager : MonoBehaviour
    {
        public static TelegramManager Instance { get; private set; }
        
        public void Initialize()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            Debug.Log("TelegramManager: Initialized");
        }
        
        public void StartApplication()
        {
            Debug.Log("TelegramManager: Starting application...");
            
            // Получение данных из Telegram
            string username = GetUsernameFromTelegram();
            
            // Загрузка данных пользователя
            AppCoordinator.Instance.UserService.GetUserData(
                username,
                userData => {
                    UserDataManager.Instance.UpdateUserData(userData);
                    Debug.Log("TelegramManager: User data loaded successfully");
                    
                    // Переход к основному экрану
                    EventSystem.NavigateTo(AppScreen.Collection);
                },
                error => {
                    Debug.LogError("TelegramManager: Failed to load user data: " + error);
                    EventSystem.ShowError("Failed to load user data");
                }
            );
        }
        
        private string GetUsernameFromTelegram()
        {
            // Здесь будет код для получения данных из Telegram Web App
            // Временная заглушка для тестирования
            return "test_user";
        }
    }
}