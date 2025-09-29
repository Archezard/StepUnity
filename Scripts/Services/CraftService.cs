using BasketballCards.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BasketballCards.Services
{
    public class CraftService
    {
        private readonly ApiClient _apiClient;
        private readonly System.Random _random = new System.Random();

        public CraftService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public void CraftCards(List<string> cardIds, Action<CardData> onSuccess, Action<string> onError = null)
        {
            // потом заменить на API
            Debug.Log($"Crafting with {cardIds.Count} cards (stub)");
            
            // Имитация обработки на сервере
            var success = _random.NextDouble() < 0.8;
            
            if (success)
            {
                Rarity[] possibleRarities = { Rarity.Bronze, Rarity.Silver, Rarity.Gold, Rarity.Diamond };
                double[] probabilities = { 0.5, 0.3, 0.15, 0.05 };
                
                Rarity resultRarity = GetRandomRarity(probabilities, possibleRarities);
                
                var craftedCard = new CardData
                {
                    CardId = Guid.NewGuid().ToString(),
                    PlayerName = GetRandomPlayerName(),
                    Rarity = resultRarity,
                    Type = CardType.Playable,
                    Level = 1,
                    MaxLevel = 5,
                    Attack = GetStatByRarity(resultRarity),
                    Defense = GetStatByRarity(resultRarity),
                    Stamina = GetStatByRarity(resultRarity),
                    Duplicates = 0
                };

                onSuccess?.Invoke(craftedCard);
            }
            else
            {
                onError?.Invoke("Крафт не удался");
            }
        }
        
        public void DisassembleCards(List<string> cardIds, Action<DisassembleResult> onSuccess, Action<string> onError = null)
        {
            // После тут будет запрос к API
            Debug.Log($"Disassembling {cardIds.Count} cards (stub)");
            
            // Симуляция разбора
            var result = new DisassembleResult
            {
                Gold = cardIds.Count * 100,
                Dust = cardIds.Count * 50
            };

            onSuccess?.Invoke(result);
        }
        
        private Rarity GetRandomRarity(double[] probabilities, Rarity[] rarities)
        {
            double randomValue = _random.NextDouble();
            double cumulative = 0.0;
            
            for (int i = 0; i < probabilities.Length; i++)
            {
                cumulative += probabilities[i];
                if (randomValue < cumulative)
                {
                    return rarities[i];
                }
            }
            
            return rarities[rarities.Length - 1];
        }
        
        private string GetRandomPlayerName()
        {
            string[] firstNames = { "LeBron", "Stephen", "Kevin", "Giannis", "Luka", "James", "Michael", "Kobe" };
            string[] lastNames = { "James", "Curry", "Durant", "Antetokounmpo", "Doncic", "Harden", "Jordan", "Bryant" };
            
            return $"{firstNames[_random.Next(firstNames.Length)]} {lastNames[_random.Next(lastNames.Length)]}";
        }
        
        private int GetStatByRarity(Rarity rarity)
        {
            switch (rarity)
            {
                case Rarity.Bronze: return _random.Next(60, 75);
                case Rarity.Silver: return _random.Next(75, 85);
                case Rarity.Gold: return _random.Next(85, 92);
                case Rarity.Diamond: return _random.Next(92, 97);
                case Rarity.Legendary: return _random.Next(97, 100);
                default: return 70;
            }
        }
    }

    public class DisassembleResult
    {
        public int Gold { get; set; }
        public int Dust { get; set; }
    }
}