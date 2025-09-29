using BasketballCards.Core;
using BasketballCards.UI.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public abstract class ShopViewBase : BaseView
    {
        // УБИРАЕМ дублирование поля _backButton, так как оно уже есть в BaseView
        // [SerializeField] protected Button _backButton;
        
        protected ShopPresenter _presenter;
        
        public virtual void Initialize(ShopPresenter presenter)
        {
            _presenter = presenter;
            // Кнопка назад уже инициализирована в BaseView
        }
        
        // ДОБАВЛЯЕМ override для метода OnBackButtonClicked
        protected override void OnBackButtonClicked()
        {
            // Базовая реализация для магазина - может быть переопределена
            EventSystem.NavigateBack();
        }
        
        public override void Show()
        {
            gameObject.SetActive(true);
        }
        
        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}