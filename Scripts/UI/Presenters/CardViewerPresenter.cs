using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Views;
using UnityEngine;

namespace BasketballCards.UI.Presenters
{
    public class CardViewerPresenter : MonoBehaviour
    {
        [Header("View References")]
        [SerializeField] private CardViewer3D _cardViewer3D;
        
        private void Start()
        {
            if (_cardViewer3D != null)
            {
                _cardViewer3D.Initialize();
            }
            
            SetupEventHandlers();
        }
        
        private void SetupEventHandlers()
        {
            EventSystem.OnCardSelected += HandleCardSelected;
            EventSystem.OnCardUpgraded += HandleCardUpgraded;
        }
        
        private void OnDestroy()
        {
            EventSystem.OnCardSelected -= HandleCardSelected;
            EventSystem.OnCardUpgraded -= HandleCardUpgraded;
        }
        
        private void HandleCardSelected(CardData card)
        {
            if (_cardViewer3D != null)
            {
                _cardViewer3D.ShowCard(card);
            }
        }
        
        private void HandleCardUpgraded(CardData card)
        {
            // Если просматриваемая карточка была улучшена, обновляем её отображение
            if (_cardViewer3D != null)
            {
                // Можно добавить логику проверки, что это та же карточка
                _cardViewer3D.ShowCard(card);
            }
        }
    }
}