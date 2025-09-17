using BasketballCards.Models;
using System.Collections.Generic;

namespace BasketballCards.UI.Views
{
    public interface ICollectionView
    {
        void DisplayCards(List<CardData> cards);
        void ShowCardDetails(CardData card);
        void UpdateSelectionCount(int count);
        void OnCardUpgraded(CardData card);
        void OnCardCrafted(CardData card);
        void ClearSelection();
        void ShowError(string message);
    }
}