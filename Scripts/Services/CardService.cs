using BasketballCards.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BasketballCards.Services
{
    public class CardService
    {
        private readonly ApiClient _apiClient;
        private List<CardData> _userCardsCache;

        public CardService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public void GetUserCards(Action<List<CardData>> onSuccess, Action<string> onError = null)
        {
            // потом заменить на API
            Debug.Log("CardService: Getting user cards (stub)");
            
            if (_userCardsCache != null)
            {
                onSuccess?.Invoke(_userCardsCache);
                return;
            }
            
            // Временные данные для демонстрации
            _userCardsCache = new List<CardData>
            {
                new CardData
                {
                    CardId = "1",
                    PlayerName = "LeBron James",
                    Rarity = Rarity.Gold,
                    Type = CardType.Playable,
                    Level = 3,
                    MaxLevel = 5,
                    Attack = 95,
                    Defense = 88,
                    Stamina = 92,
                    Duplicates = 2
                },
                new CardData
                {
                    CardId = "2",
                    PlayerName = "Stephen Curry",
                    Rarity = Rarity.Gold,
                    Type = CardType.Playable,
                    Level = 2,
                    MaxLevel = 5,
                    Attack = 98,
                    Defense = 75,
                    Stamina = 85,
                    Duplicates = 1
                },
                new CardData
                {
                    CardId = "2",
                    PlayerName = "Stephen Curry",
                    Rarity = Rarity.Gold,
                    Type = CardType.Playable,
                    Level = 2,
                    MaxLevel = 5,
                    Attack = 98,
                    Defense = 75,
                    Stamina = 85,
                    Duplicates = 1
                },
                new CardData
                {
                    CardId = "2",
                    PlayerName = "Stephen Curry",
                    Rarity = Rarity.Gold,
                    Type = CardType.Playable,
                    Level = 2,
                    MaxLevel = 5,
                    Attack = 98,
                    Defense = 75,
                    Stamina = 85,
                    Duplicates = 1
                },
                new CardData
                {
                    CardId = "2",
                    PlayerName = "Stephen Curry",
                    Rarity = Rarity.Gold,
                    Type = CardType.Playable,
                    Level = 2,
                    MaxLevel = 5,
                    Attack = 98,
                    Defense = 75,
                    Stamina = 85,
                    Duplicates = 1
                },
                new CardData
                {
                    CardId = "2",
                    PlayerName = "Stephen Curry",
                    Rarity = Rarity.Gold,
                    Type = CardType.Playable,
                    Level = 2,
                    MaxLevel = 5,
                    Attack = 98,
                    Defense = 75,
                    Stamina = 85,
                    Duplicates = 1
                },
                new CardData
                {
                    CardId = "2",
                    PlayerName = "Stephen Curry",
                    Rarity = Rarity.Gold,
                    Type = CardType.Playable,
                    Level = 2,
                    MaxLevel = 5,
                    Attack = 98,
                    Defense = 75,
                    Stamina = 85,
                    Duplicates = 1
                },
                new CardData
                {
                    CardId = "2",
                    PlayerName = "Stephen Curry",
                    Rarity = Rarity.Gold,
                    Type = CardType.Playable,
                    Level = 2,
                    MaxLevel = 5,
                    Attack = 98,
                    Defense = 75,
                    Stamina = 85,
                    Duplicates = 1
                },
                new CardData
                {
                    CardId = "2",
                    PlayerName = "Stephen Curry",
                    Rarity = Rarity.Gold,
                    Type = CardType.Playable,
                    Level = 2,
                    MaxLevel = 5,
                    Attack = 98,
                    Defense = 75,
                    Stamina = 85,
                    Duplicates = 1
                },
                new CardData
                {
                    CardId = "2",
                    PlayerName = "Stephen Curry",
                    Rarity = Rarity.Gold,
                    Type = CardType.Playable,
                    Level = 2,
                    MaxLevel = 5,
                    Attack = 98,
                    Defense = 75,
                    Stamina = 85,
                    Duplicates = 1
                },
                new CardData
                {
                    CardId = "2",
                    PlayerName = "Stephen Curry",
                    Rarity = Rarity.Gold,
                    Type = CardType.Playable,
                    Level = 2,
                    MaxLevel = 5,
                    Attack = 98,
                    Defense = 75,
                    Stamina = 85,
                    Duplicates = 1
                },
                new CardData
                {
                    CardId = "2",
                    PlayerName = "Stephen Curry",
                    Rarity = Rarity.Gold,
                    Type = CardType.Playable,
                    Level = 2,
                    MaxLevel = 5,
                    Attack = 98,
                    Defense = 75,
                    Stamina = 85,
                    Duplicates = 1
                },
                new CardData
                {
                    CardId = "2",
                    PlayerName = "Stephen Curry",
                    Rarity = Rarity.Silver,
                    Type = CardType.Playable,
                    Level = 2,
                    MaxLevel = 5,
                    Attack = 98,
                    Defense = 75,
                    Stamina = 85,
                    Duplicates = 1
                }
            };

            onSuccess?.Invoke(_userCardsCache);
        }

        public void FilterCards(List<CardData> cards, CardType? type, Rarity? rarity, string searchQuery, Action<List<CardData>> onSuccess)
        {
            // ЗАГЛУШКА: Фильтрация на клиенте (будет на сервере, потом удалить)
            Debug.Log("CardService: Filtering cards (stub)");
            
            var filtered = cards;

            if (type.HasValue)
            {
                filtered = filtered.FindAll(c => c.Type == type.Value);
            }

            if (rarity.HasValue)
            {
                filtered = filtered.FindAll(c => c.Rarity == rarity.Value);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                filtered = filtered.FindAll(c => 
                    c.PlayerName.IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    c.Team?.IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            onSuccess?.Invoke(filtered);
        }
        
        public void UpgradeCard(string cardId, Action<CardData> onSuccess, Action<string> onError = null)
        {
            // потом заменить на API
            Debug.Log($"CardService: Upgrading card {cardId} (stub)");
            
            // Симуляция успешного улучшения
            var card = _userCardsCache.Find(c => c.CardId == cardId);
            if (card != null && card.Level < card.MaxLevel)
            {
                card.Level++;
                card.Attack += 5;
                card.Defense += 3;
                card.Stamina += 4;
                
                onSuccess?.Invoke(card);
            }
            else
            {
                onError?.Invoke("Cannot upgrade card");
            }
        }
    }
}