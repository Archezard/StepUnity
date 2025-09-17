using System;
using Newtonsoft.Json;
using UnityEngine;

namespace BasketballCards.Models
{
    public enum CardType
    {
        Playable,
        Exclusive,
        Clip,
        ExclusiveClip
    }

    public enum Rarity
    {
        Bronze,
        Silver,
        Gold,
        Diamond,
        Legendary
    }

    [Serializable]
    public class CardData
    {
        [JsonProperty("card_id")] public string CardId;
        [JsonProperty("player_name")] public string PlayerName;
        [JsonProperty("team")] public string Team;
        [JsonProperty("type")] public CardType Type;
        [JsonProperty("rarity")] public Rarity Rarity;
        [JsonProperty("level")] public int Level = 1;
        [JsonProperty("max_level")] public int MaxLevel = 5;
        [JsonProperty("attack")] public int Attack;
        [JsonProperty("defense")] public int Defense;
        [JsonProperty("stamina")] public int Stamina;
        [JsonProperty("duplicates")] public int Duplicates = 0;
        [JsonProperty("image_url")] public string ImageUrl;
        
        // Дополнительные свойства для UI
        public Sprite CardImage { get; set; }
        public bool IsSelected { get; set; }
        
        public Color RarityColor
        {
            get
            {
                switch (Rarity)
                {
                    case Rarity.Bronze: return Color.gray;
                    case Rarity.Silver: return Color.white;
                    case Rarity.Gold: return Color.yellow;
                    case Rarity.Diamond: return Color.cyan;
                    case Rarity.Legendary: return Color.magenta;
                    default: return Color.white;
                }
            }
        }
    }
}