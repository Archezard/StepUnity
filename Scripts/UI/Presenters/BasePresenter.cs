using UnityEngine;

namespace BasketballCards.UI.Presenters
{
    public abstract class BasePresenter : MonoBehaviour
    {
        protected virtual void Awake()
        {
            // Базовая инициализация
        }
        
        protected virtual void Start()
        {
            // Подписка на события
        }
        
        protected virtual void OnDestroy()
        {
            // Отписка от событий
        }
    }
}