using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BasketballCards.Models
{
    [Serializable]
    public class TradeOffer
    {
        [JsonProperty("offer_id")] public string OfferId;
        [JsonProperty("from_user_id")] public string FromUserId;
        [JsonProperty("from_username")] public string FromUsername;
        [JsonProperty("to_user_id")] public string ToUserId;
        [JsonProperty("to_username")] public string ToUsername;
        [JsonProperty("offered_cards")] public List<CardData> OfferedCards;
        [JsonProperty("requested_cards")] public List<CardData> RequestedCards;
        [JsonProperty("status")] public TradeStatus Status;
        [JsonProperty("created_at")] public string CreatedAt;
        [JsonProperty("diamond_cost")] public int DiamondCost;
    }

    public enum TradeStatus
    {
        Pending,
        Accepted,
        Rejected,
        Cancelled
    }
}