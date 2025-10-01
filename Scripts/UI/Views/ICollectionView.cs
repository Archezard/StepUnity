using BasketballCards.Models;
using BasketballCards.Services;
using System.Collections.Generic;

namespace BasketballCards.UI.Views
{
    public interface ICollectionView
    {
        void DisplayCards(List<CardData> cards);
        void ShowError(string message);
    }

    public interface IWorkshopView : ICollectionView
    {
        void UpdateSelectionCount(int count);
        void ClearSelection();
        void OnCraftSuccess(CardData craftedCard);
        void OnDisassembleSuccess(int gold, int dust);
    }

    public interface IAlbumView : ICollectionView
    {
        void DisplayAlbums(List<AlbumData> albums);
        void ShowAlbumDetails(AlbumData album);
    }

    public interface IExchangeView : ICollectionView
    {
        void DisplayTradeOffers(List<TradeOffer> offers);
        void ShowTradeDetails(TradeOffer offer);
    }
}