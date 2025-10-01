using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BasketballCards.Models
{
    [Serializable]
    public class AlbumData
    {
        [JsonProperty("id")] public string Id;
        [JsonProperty("name")] public string Name;
        [JsonProperty("type")] public AlbumType Type;
        [JsonProperty("pages")] public List<AlbumPage> Pages;
        [JsonProperty("unlocked_pages")] public int UnlockedPages;
    }

    [Serializable]
    public class AlbumPage
    {
        [JsonProperty("page_number")] public int PageNumber;
        [JsonProperty("slots")] public List<AlbumSlot> Slots;
    }

    [Serializable]
    public class AlbumSlot
    {
        [JsonProperty("slot_index")] public int SlotIndex;
        [JsonProperty("card_id")] public string CardId;
        [JsonProperty("card_data")] public CardData CardData;
    }

    public enum AlbumType
    {
        Progress,
        Archive,
        Custom
    }
}