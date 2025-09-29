using BasketballCards.Core;
using BasketballCards.Models;
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
            EventSystem.OnUserDataUpdated += HandleUserDataUpdated;
        }
        
        protected virtual void UnsubscribeFromEvents()
        {
            EventSystem.OnNavigationRequested -= HandleNavigationRequest;
            EventSystem.OnUserDataUpdated -= HandleUserDataUpdated;
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
            return gameObject.activeInHierarchy;
        }
    }
}