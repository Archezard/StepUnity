using BasketballCards.Managers;
using BasketballCards.UI.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class ShopView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button _cardShopButton;
        [SerializeField] private Button _throwShopButton;
        [SerializeField] private Button _packShopButton;
        [SerializeField] private Button _currencyShopButton;
        [SerializeField] private Button _backButton;
        
        private ShopPresenter _presenter;
        
        public void Initialize(ShopPresenter presenter)
        {
            _presenter = presenter;
            
            _cardShopButton.onClick.AddListener(OnCardShopButtonClicked);
            _throwShopButton.onClick.AddListener(OnThrowShopButtonClicked);
            _packShopButton.onClick.AddListener(OnPackShopButtonClicked);
            _currencyShopButton.onClick.AddListener(OnCurrencyShopButtonClicked);
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }
        
        private void OnCardShopButtonClicked()
        {
            _presenter.ShowCardShop();
        }
        
        private void OnThrowShopButtonClicked()
        {
            _presenter.ShowThrowShop();
        }
        
        private void OnPackShopButtonClicked()
        {
            _presenter.ShowPackShop();
        }
        
        private void OnCurrencyShopButtonClicked()
        {
            _presenter.ShowCurrencyShop();
        }
        
        private void OnBackButtonClicked()
        {
            UIManager.Instance.ShowMainMenu();
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}