// Scripts/UI/Presenters/BasePresenter.cs - ИСПРАВЛЕННАЯ ВЕРСИЯ
using BasketballCards.Core;
using BasketballCards.Models;
using System.Collections;
using UnityEngine;

namespace BasketballCards.UI.Presenters
{
    public abstract class BasePresenter : MonoBehaviour
    {
        [SerializeField] protected AppScreen _screenType;
        
        protected virtual void OnEnable()
        {
            SubscribeToEvents();
        }
        
        protected virtual void OnDisable()
        {
            UnsubscribeFromEvents();
        }
        
        protected virtual void SubscribeToEvents()
        {
            EventSystem.OnNavigationRequested += HandleNavigationRequest;
            
            // Безопасная подписка на UserDataManager
            if (UserDataManager.Instance != null && UserDataManager.Instance.IsReady)
            {
                UserDataManager.Instance.OnUserDataUpdated += HandleUserDataUpdated;
            }
            else
            {
                // Если UserDataManager ещё не готов, отложим подписку
                StartCoroutine(WaitForUserDataManager());
            }
        }
        
        protected virtual void UnsubscribeFromEvents()
        {
            EventSystem.OnNavigationRequested -= HandleNavigationRequest;
            
            if (UserDataManager.Instance != null)
            {
                UserDataManager.Instance.OnUserDataUpdated -= HandleUserDataUpdated;
            }
        }
        
        private IEnumerator WaitForUserDataManager()
        {
            // Ждём пока UserDataManager станет доступен
            int attempts = 0;
            int maxAttempts = 50; // 5 секунд максимум
            
            while (attempts < maxAttempts)
            {
                attempts++;
                if (UserDataManager.Instance != null && UserDataManager.Instance.IsReady)
                {
                    UserDataManager.Instance.OnUserDataUpdated += HandleUserDataUpdated;
                    OnUserDataManagerReady();
                    yield break;
                }
                yield return new WaitForSeconds(0.1f);
            }
            
            Debug.LogError($"{GetType().Name}: Timeout waiting for UserDataManager!");
        }
        
        protected virtual void OnUserDataManagerReady()
        {
            // Переопределить в наследниках при необходимости
        }
        
        protected virtual void HandleNavigationRequest(AppScreen screen)
        {
            if (screen == _screenType)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
        
        protected virtual void HandleUserDataUpdated(UserData userData)
        {
            // Базовая реализация
        }
        
        public abstract void Show();
        public abstract void Hide();
        
        // Вспомогательный метод для проверки активности
        public bool IsActive()
        {
            return gameObject.activeInHierarchy && isActiveAndEnabled;
        }
    }
}