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
                Debug.LogWarning("TelegramManager: AppCoordinator is not fully initialized, waiting...");
                // Ждём инициализации
                StartCoroutine(WaitForAppCoordinator());
                return;
            }
            
            // Запускаем приложение через AppCoordinator
            _appCoordinator.StartApplication();
        }
        
        private System.Collections.IEnumerator WaitForAppCoordinator()
        {
            int attempts = 0;
            while (!_appCoordinator.IsInitialized() && attempts < 10)
            {
                attempts++;
                yield return new WaitForSeconds(0.5f);
            }
            
            if (_appCoordinator.IsInitialized())
            {
                _appCoordinator.StartApplication();
            }
            else
            {
                Debug.LogError("TelegramManager: AppCoordinator initialization timeout!");
            }
        }
    }
}