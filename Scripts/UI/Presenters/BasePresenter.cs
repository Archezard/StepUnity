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
        
        // ИСПРАВЛЕНО: Добавлен параметр UserData
        protected virtual void HandleUserDataUpdated(UserData userData)
        {
            // Базовая реализация - может быть переопределена
            // По умолчанию ничего не делаем, но метод должен соответствовать делегату
        }
        
        public abstract void Show();
        public abstract void Hide();
    }
}