using BasketballCards.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BasketballCards.Services
{
    public class ActivitiesService
    {
        private readonly ApiClient _apiClient;
        private readonly System.Random _random = new System.Random();
        
        // Счетчики для системы гарантов
        private Dictionary<Rarity, int> _pityCounters = new Dictionary<Rarity, int>();
        
        public ActivitiesService(ApiClient apiClient)
        {
            _apiClient = apiClient;
            
            // Инициализация счетчиков гарантов. Это временная хуйня через ИИ сделано, потом надо будет это переделать, ибо я хз как это вообще должно работать
            foreach (Rarity rarity in Enum.GetValues(typeof(Rarity)))
            {
                _pityCounters[rarity] = 0;
            }
        }
        
        public void GetFreeCard(Action<CardData> onSuccess, Action<string> onError = null)
        {
            // зАгЛуШкА: потом заменить на API
            Debug.Log("Getting free card with pity system");
            
            // Обновляем счетчики гарантов
            foreach (var rarity in _pityCounters.Keys)
            {
                _pityCounters[rarity]++;
            }
            
            // Проверяем гаранты
            Rarity cardRarity = CheckPitySystem();
            
            // Сбрасываем счетчик для полученной редкости
            _pityCounters[cardRarity] = 0;
            
            var card = GenerateRandomCard(cardRarity);
            onSuccess?.Invoke(card);
        }
        
        private Rarity CheckPitySystem()
        {
            // Проверяем гаранты согласно ТЗ
            if (_pityCounters[Rarity.Legendary] >= 4000) return Rarity.Legendary;
            if (_pityCounters[Rarity.Diamond] >= 1330) return Rarity.Diamond;
            if (_pityCounters[Rarity.Gold] >= 23) return Rarity.Gold;
            if (_pityCounters[Rarity.Silver] >= 5) return Rarity.Silver;
            
            // Если гаранты не сработали, используем обычные вероятности
            float roll = (float)_random.NextDouble();
            
            if (roll < 0.75f) return Rarity.Bronze;
            if (roll < 0.95f) return Rarity.Silver;
            if (roll < 0.995f) return Rarity.Gold;
            if (roll < 0.9995f) return Rarity.Diamond;
            return Rarity.Legendary;
        }
        
        private CardData GenerateRandomCard(Rarity rarity)
        {
            return new CardData
            {
                CardId = Guid.NewGuid().ToString(),
                PlayerName = GetRandomPlayerName(),
                Rarity = rarity,
                Attack = GetStatByRarity(rarity),
                Defense = GetStatByRarity(rarity),
                Stamina = GetStatByRarity(rarity),
                Level = 1,
                MaxLevel = 5,
                Duplicates = 0
            };
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
        
        public void ThrowBall(Action<int, int> onSuccess, Action<string> onError = null)
        {
            // потом заменить на API
            Debug.Log("Throwing ball");
            
            int score = UnityEngine.Random.Range(0, 4);
            int rewards = score > 0 ? 1 : 0;
            
            onSuccess?.Invoke(score, rewards);
        }
        
        public void OpenPack(string packId, Action<List<CardData>> onSuccess, Action<string> onError = null)
        {
            // потом заменить на API
            Debug.Log($"Opening pack {packId}");
            
            var cards = new List<CardData>();
            int cardCount = 5;
            
            for (int i = 0; i < cardCount; i++)
            {
                float rarityRoll = (float)_random.NextDouble();
                Rarity rarity;
                
                if (rarityRoll < 0.75f) rarity = Rarity.Bronze;
                else if (rarityRoll < 0.95f) rarity = Rarity.Silver;
                else if (rarityRoll < 0.995f) rarity = Rarity.Gold;
                else if (rarityRoll < 0.9995f) rarity = Rarity.Diamond;
                else rarity = Rarity.Legendary;
                
                cards.Add(GenerateRandomCard(rarity));
            }
            
            onSuccess?.Invoke(cards);
        }
    }
}