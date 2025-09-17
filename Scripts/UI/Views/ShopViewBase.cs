using BasketballCards.UI.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public abstract class ShopViewBase : MonoBehaviour
    {
        [SerializeField] protected Button _backButton;
        
        protected ShopPresenter _presenter;
        
        public virtual void Initialize(ShopPresenter presenter)
        {
            _presenter = presenter;
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }
        
        protected virtual void OnBackButtonClicked()
        {
            _presenter.ShowShop();
        }
        
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
        
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}