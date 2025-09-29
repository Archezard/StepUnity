using BasketballCards.Models;
using BasketballCards.UI.Presenters;
using UnityEngine;

namespace BasketballCards.UI.Views
{
    public class AlbumView : BaseView, ICollectionView
    {
        private CollectionPresenter _presenter;
        
        public void Initialize(CollectionPresenter presenter)
        {
            _presenter = presenter;
        }
        
        // ICollectionView implementation
        public void DisplayCards(System.Collections.Generic.List<CardData> cards) { }
        public void ShowCardDetails(CardData card) { }
        public void UpdateSelectionCount(int count) { }
        public void OnCardUpgraded(CardData card) { }
        public void OnCardCrafted(CardData card) { }
        public void ClearSelection() { }
    }
}