using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BasketballCards.Models
{
    [Serializable]
    public class BattlePassProgress
    {
        [JsonProperty("level")] public int Level;
        [JsonProperty("experience")] public int Experience;
        [JsonProperty("premium_unlocked")] public bool PremiumUnlocked;
        [JsonProperty("claimed_rewards")] public List<int> ClaimedRewards;
        
        // Тут награды по уровням
        [JsonProperty("rewards")] public Dictionary<int, BattlePassReward> Rewards;
    }

    [Serializable]
    public class BattlePassReward
    {
        [JsonProperty("gold")] public int Gold;
        [JsonProperty("diamonds")] public int Diamonds;
        [JsonProperty("tokens")] public Dictionary<Rarity, int> Tokens;
        [JsonProperty("cards")] public List<CardData> Cards;
    }
}