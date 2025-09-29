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
            
            // Инициализация счетчиков гарантов
            InitializePityCounters();
        }
        
        private void InitializePityCounters()
        {
            _pityCounters[Rarity.Bronze] = 0;
            _pityCounters[Rarity.Silver] = 0;
            _pityCounters[Rarity.Gold] = 0;
            _pityCounters[Rarity.Diamond] = 0;
            _pityCounters[Rarity.Legendary] = 0;
        }
        
        public void GetFreeCard(Action<CardData> onSuccess, Action<string> onError = null)
        {
            // Заглушка: потом заменить на API
            Debug.Log("Getting free card with pity system");
            
            // Обновляем счетчики гарантов для всех редкостей
            UpdatePityCounters();
            
            // Проверяем гаранты
            Rarity cardRarity = CheckPitySystem();
            
            // Сбрасываем счетчик для полученной редкости
            _pityCounters[cardRarity] = 0;
            
            var card = GenerateRandomCard(cardRarity);
            onSuccess?.Invoke(card);
        }
        
        private void UpdatePityCounters()
        {
            // Увеличиваем счетчики только для тех редкостей, которые еще не были получены
            foreach (var rarity in _pityCounters.Keys)
            {
                _pityCounters[rarity]++;
            }
        }
        
        private Rarity CheckPitySystem()
        {
            // Проверяем гаранты согласно ТЗ в порядке от самой редкой к самой частой
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
                Team = GetRandomTeam(),
                Rarity = rarity,
                Type = GetCardTypeByRarity(rarity),
                Attack = GetStatByRarity(rarity),
                Defense = GetStatByRarity(rarity),
                Stamina = GetStatByRarity(rarity),
                Level = 1,
                MaxLevel = 5,
                Duplicates = 0
            };
        }
        
        private CardType GetCardTypeByRarity(Rarity rarity)
        {
            
            float clipChance = 0.004f;
            
            if (_random.NextDouble() < clipChance)
            {
                return CardType.Clip;
            }
            
            return CardType.Playable;
        }
        
        private string GetRandomPlayerName()
        {
            string[] firstNames = { "LeBron", "Stephen", "Kevin", "Giannis", "Luka", "James", "Michael", "Kobe" };
            string[] lastNames = { "James", "Curry", "Durant", "Antetokounmpo", "Doncic", "Harden", "Jordan", "Bryant" };
            
            return $"{firstNames[_random.Next(firstNames.Length)]} {lastNames[_random.Next(lastNames.Length)]}";
        }
        
        private string GetRandomTeam()
        {
            string[] teams = { "Lakers", "Warriors", "Nets", "Bucks", "Mavericks", "Bulls", "Heat", "Celtics" };
            return teams[_random.Next(teams.Length)];
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
            // Заглушка: потом заменить на API
            Debug.Log("Throwing ball");
            
            int score = UnityEngine.Random.Range(0, 4);
            int rewards = score > 0 ? 1 : 0;
            
            onSuccess?.Invoke(score, rewards);
        }
        
        public void OpenPack(string packId, Action<List<CardData>> onSuccess, Action<string> onError = null)
        {
            // Заглушка: потом заменить на API
            Debug.Log($"Opening pack {packId}");
            
            var cards = new List<CardData>();
            int cardCount = GetCardCountByPack(packId);
            
            for (int i = 0; i < cardCount; i++)
            {
                Rarity rarity = GetRarityForPack(packId, i);
                cards.Add(GenerateRandomCard(rarity));
            }
            
            onSuccess?.Invoke(cards);
        }
        
        private int GetCardCountByPack(string packId)
        {
            switch (packId)
            {
                case "weekly_pack": return 5;
                case "bronze_pack": return 3;
                case "silver_pack": return 4;
                case "gold_pack": return 5;
                default: return 3;
            }
        }
        
        private Rarity GetRarityForPack(string packId, int cardIndex)
        {
            // Логика распределения редкостей в зависимости от пака
            float roll = (float)_random.NextDouble();
            
            switch (packId)
            {
                case "weekly_pack":
                    // Еженедельный пак согласно ТЗ
                    if (cardIndex == 0) return Rarity.Bronze; // 100% бронза
                    if (cardIndex == 1) return roll < 0.9f ? Rarity.Bronze : Rarity.Silver; // 90% бронза, 10% серебро
                    if (cardIndex == 2) return roll < 0.9f ? Rarity.Bronze : (roll < 0.99f ? Rarity.Silver : Rarity.Gold); // 90% бронза, 9% серебро, 1% золото
                    if (cardIndex == 3) return roll < 0.2f ? Rarity.Diamond : Rarity.Bronze; // 20% алмаз
                    // Для клипа возвращаем базовую редкость, тип будет определен в GenerateRandomCard
                    return roll < 0.1f ? Rarity.Silver : Rarity.Bronze; // 10% клип (временно используем Silver как базовую редкость)
                    
                default:
                    // Обычная логика для других паков
                    if (roll < 0.75f) return Rarity.Bronze;
                    if (roll < 0.95f) return Rarity.Silver;
                    if (roll < 0.995f) return Rarity.Gold;
                    if (roll < 0.9995f) return Rarity.Diamond;
                    return Rarity.Legendary;
            }
        }
        
        // Метод для сброса счетчиков (Если будет нужен)
        public void ResetPityCounters()
        {
            InitializePityCounters();
        }
        
        // Метод для получения текущих счетчиков (для отладки)
        public Dictionary<Rarity, int> GetPityCounters()
        {
            return new Dictionary<Rarity, int>(_pityCounters);
        }
    }
}