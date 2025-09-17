using BasketballCards.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BasketballCards.Services
{
    public class TradeService
    {
        private readonly ApiClient _apiClient;
        
        public TradeService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        public void ProposeTrade(string targetUserId, List<CardData> offeredCards, List<CardData> requestedCards, Action<bool> onSuccess, Action<string> onError = null)
        {
            // Заглушка для демонстрации
            Debug.Log($"Proposing trade to user {targetUserId}");
            Debug.Log($"Offered {offeredCards.Count} cards, requested {requestedCards.Count} cards");
            
            // После тут будет запрос к API
            onSuccess?.Invoke(true);
        }
        
        public void GetTradeOffers(Action<List<TradeOffer>> onSuccess, Action<string> onError = null)
        {
            // Заглушка для демонстрации
            Debug.Log("Getting trade offers");
            
            // После тут будет запрос к API
            var offers = new List<TradeOffer>
            {
                new TradeOffer
                {
                    OfferId = "1",
                    FromUserId = "user2",
                    FromUsername = "TraderJoe",
                    OfferedCards = new List<CardData>(),
                    RequestedCards = new List<CardData>(),
                    Status = TradeStatus.Pending
                }
            };
            
            onSuccess?.Invoke(offers);
        }
        
        public void RespondToTrade(string offerId, bool accept, Action<bool> onSuccess, Action<string> onError = null)
        {
            // Заглушка для демонстрации
            Debug.Log($"Responding to trade {offerId}: {(accept ? "accept" : "reject")}");
            
            // После тут будет запрос к API
            onSuccess?.Invoke(true);
        }
    }
    
    public class TradeOffer
    {
        public string OfferId { get; set; }
        public string FromUserId { get; set; }
        public string FromUsername { get; set; }
        public List<CardData> OfferedCards { get; set; }
        public List<CardData> RequestedCards { get; set; }
        public TradeStatus Status { get; set; }
    }
    
    public enum TradeStatus
    {
        Pending,
        Accepted,
        Rejected,
        Cancelled
    }
}