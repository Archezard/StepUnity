using System;
using Newtonsoft.Json;
using UnityEngine;

namespace BasketballCards.Models
{
    [Serializable]
    public class UserData
    {
        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? user_id;

        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string username;

        [JsonProperty("gold", NullValueHandling = NullValueHandling.Ignore)]
        public int gold = 1000;

        [JsonProperty("diamonds", NullValueHandling = NullValueHandling.Ignore)]
        public int diamonds = 100;

        [JsonProperty("tickets", NullValueHandling = NullValueHandling.Ignore)]
        public int tickets = 5;

        [JsonProperty("count_get", NullValueHandling = NullValueHandling.Ignore)]
        public int? count_get;

        [JsonProperty("last_ticket_time", NullValueHandling = NullValueHandling.Ignore)]
        public string last_ticket_time;

        [JsonProperty("cards", NullValueHandling = NullValueHandling.Ignore)]
        public string cards;

        [JsonProperty("throw_count", NullValueHandling = NullValueHandling.Ignore)]
        public int? throw_count;

        [JsonProperty("subscribed_channels", NullValueHandling = NullValueHandling.Ignore)]
        public string subscribed_channels;

        [JsonProperty("used_channels", NullValueHandling = NullValueHandling.Ignore)]
        public string used_channels;

        [JsonProperty("reffer", NullValueHandling = NullValueHandling.Ignore)]
        public string reffer;

        [JsonProperty("additional_try", NullValueHandling = NullValueHandling.Ignore)]
        public int? additional_try;

        [JsonProperty("sub_date", NullValueHandling = NullValueHandling.Ignore)]
        public string sub_date;
    }
}