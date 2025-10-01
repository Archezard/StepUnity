using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Presenters;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class ExchangeView : BaseView, IExchangeView
    {
        [Header("UI References")]
        [SerializeField] private Button _exchangesButton;
        [SerializeField] private Button _requestsButton;
        [SerializeField] private TMP_InputField _searchUserInput;
        [SerializeField] private Transform _offersContainer;
        [SerializeField] private GameObject _tradeOfferPrefab;
        [SerializeField] private TextMeshProUGUI _diamondCostText;

        private CollectionPresenter _presenter;
        private List<TradeOffer> _currentOffers = new List<TradeOffer>();
        private bool _showExchanges = true; // true = обмены, false = запросы

        public void Initialize(CollectionPresenter presenter)
        {
            _presenter = presenter;
            
            _exchangesButton.onClick.AddListener(() => ShowExchanges(true));
            _requestsButton.onClick.AddListener(() => ShowExchanges(false));
            _searchUserInput.onValueChanged.AddListener(OnSearchUserInputChanged);
            
            LoadTradeOffers();
        }
        
        public void DisplayTradeOffers(List<TradeOffer> offers)
        {
            _currentOffers = offers;
            UpdateOffersDisplay();
        }
        
        public void ShowTradeDetails(TradeOffer offer)
        {
            // Заглушка для отображения деталей обмена
            Debug.Log($"Showing trade details: {offer.OfferId}");
        }
        
        public void DisplayCards(List<CardData> cards)
        {
            // В обмене отображаются не карточки напрямую, а предложения обмена
            // Этот метод может не использоваться в ExchangeView
        }
        
        private void ShowExchanges(bool showExchanges)
        {
            _showExchanges = showExchanges;
            LoadTradeOffers();
        }
        
        private void LoadTradeOffers()
        {
            // Заглушка - в реальности будет загрузка через сервис
            var offers = new List<TradeOffer>
            {
                new TradeOffer
                {
                    OfferId = "1",
                    FromUserId = "user123",
                    FromUsername = "Trader123",
                    OfferedCards = new List<CardData>(),
                    RequestedCards = new List<CardData>(),
                    Status = TradeStatus.Pending
                }
            };
            
            DisplayTradeOffers(offers);
        }
        
        private void UpdateOffersDisplay()
        {
            // Очищаем контейнер
            foreach (Transform child in _offersContainer)
            {
                Destroy(child.gameObject);
            }
            
            // Создаем элементы предложений обмена
            foreach (var offer in _currentOffers)
            {
                var offerItem = Instantiate(_tradeOfferPrefab, _offersContainer);
                var offerButton = offerItem.GetComponent<Button>();
                var offerText = offerItem.GetComponentInChildren<TextMeshProUGUI>();
                
                if (offerText != null)
                {
                    offerText.text = $"Обмен с {offer.FromUsername}";
                }
                
                if (offerButton != null)
                {
                    offerButton.onClick.AddListener(() => ShowTradeDetails(offer));
                }
            }
            
            UpdateDiamondCost();
        }
        
        private void UpdateDiamondCost()
        {
            // Заглушка для расчета стоимости алмазов
            _diamondCostText.text = "Стоимость обмена: 10 алмазов";
        }
        
        private void OnSearchUserInputChanged(string searchQuery)
        {
            // Заглушка для поиска пользователей
            if (!string.IsNullOrEmpty(searchQuery))
            {
                Debug.Log($"Searching for user: {searchQuery}");
            }
        }
        
        public override void Show()
        {
            base.Show();
            LoadTradeOffers();
        }
    }
}