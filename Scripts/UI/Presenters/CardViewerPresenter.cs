using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Views;
using UnityEngine;

namespace BasketballCards.UI.Presenters
{
    public class CardViewerPresenter : BasePresenter
    {
        [Header("View Reference")]
        [SerializeField] private CardViewer3D _cardViewer3D;

        private CardData _currentCard;

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            EventSystem.OnCardViewRequested += HandleCardViewRequested;
            EventSystem.OnCardUpgraded += HandleCardUpgraded;
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            EventSystem.OnCardViewRequested -= HandleCardViewRequested;
            EventSystem.OnCardUpgraded -= HandleCardUpgraded;
        }

        private void Start()
        {
            if (_cardViewer3D != null)
            {
                _cardViewer3D.Initialize();
                _cardViewer3D.OnUpgradeRequested += OnUpgradeRequested;
                _cardViewer3D.OnCloseRequested += OnCloseRequested;
            }
        }

        public override void Show()
        {
            // CardViewer3D
            if (_cardViewer3D != null)
            {
                _cardViewer3D.gameObject.SetActive(true);
            }
        }

        public override void Hide()
        {
            if (_cardViewer3D != null)
            {
                _cardViewer3D.HideCard();
                _cardViewer3D.gameObject.SetActive(false);
            }
        }

        private void HandleCardViewRequested(CardData card)
        {
            _currentCard = card;
            if (_cardViewer3D != null)
            {
                _cardViewer3D.ShowCard(card);
                Show();
            }
        }

        private void HandleCardUpgraded(CardData card)
        {
            // Если просматриваемая карточка была улучшена, обновляем её отображение
            if (_currentCard != null && _currentCard.CardId == card.CardId)
            {
                _currentCard = card;
                if (_cardViewer3D != null)
                {
                    _cardViewer3D.ShowCard(card);
                }
            }
        }

        private void OnUpgradeRequested()
        {
            if (_currentCard != null)
            {
                EventSystem.UpgradeCard(_currentCard);
            }
        }

        private void OnCloseRequested()
        {
            Hide();
        }
    }
}