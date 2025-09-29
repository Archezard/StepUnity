using BasketballCards.Core;
using UnityEngine;

namespace BasketballCards.Managers
{
    public class TelegramManager : MonoBehaviour
    {
        public static TelegramManager Instance { get; private set; }
        
        private AppCoordinator _appCoordinator;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        public void Initialize()
        {
            Debug.Log("TelegramManager: Initializing...");
            // Инициализация Telegram Web App
        }
        
        // Метод для установки ссылки на AppCoordinator
        public void SetAppCoordinator(AppCoordinator appCoordinator)
        {
            _appCoordinator = appCoordinator;
            Debug.Log("TelegramManager: AppCoordinator reference set");
        }
        
        public void StartApplication()
        {
            Debug.Log("TelegramManager: Starting application...");
            
            if (_appCoordinator == null)
            {
                _appCoordinator = FindFirstObjectByType<AppCoordinator>();
                
                if (_appCoordinator == null)
                {
                    Debug.LogError("TelegramManager: AppCoordinator is not initialized and not found in scene!");
                    return;
                }
            }

            if (!_appCoordinator.IsInitialized())
            {
                Debug.LogError("TelegramManager: AppCoordinator is not fully initialized!");
                // Можно попробовать подождать или перезапустить позже
                return;
            }
            
            // Запускаем приложение через AppCoordinator
            _appCoordinator.StartApplication();
        }
    }
}