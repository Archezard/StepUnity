using BasketballCards.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BasketballCards.Services
{
    public class BattlePassService
    {
        private readonly ApiClient _apiClient;
        
        public BattlePassService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        public void GetBattlePassProgress(Action<BattlePassProgress> onSuccess, Action<string> onError = null)
        {
            // Заглушка для демонстрации с добавленными наградами
            Debug.Log("Getting battle pass progress");
            
            // Создаем словарь наград для каждого уровня
            var rewards = new Dictionary<int, BattlePassReward>();
            
            // Заполняем награды для уровней 1-30 согласно вашему описанию
            rewards[1] = new BattlePassReward { Gold = 1000, Diamonds = 0 };
            rewards[2] = new BattlePassReward { Tokens = new Dictionary<Rarity, int> { { Rarity.Bronze, 10 } } };
            rewards[3] = new BattlePassReward { Diamonds = 5 };
            // ... заполнить остальные уровни согласно таблице наград
            
            // потом заменить на API
            var progress = new BattlePassProgress
            {
                Level = 5,
                Experience = 750,
                PremiumUnlocked = false,
                ClaimedRewards = new List<int> { 1, 2, 3, 4 },
                Rewards = rewards
            };
            
            onSuccess?.Invoke(progress);
        }
        
        public void ClaimReward(int level, bool isPremium, Action<BattlePassReward> onSuccess, Action<string> onError = null)
        {
            // Заглушка
            Debug.Log($"Claiming battle pass reward for level {level}, premium: {isPremium}");
            
            // потом заменить на API
            var reward = new BattlePassReward
            {
                Gold = level * 100,
                Diamonds = isPremium ? level * 5 : 0,
                Cards = new List<CardData>(),
                Tokens = new Dictionary<Rarity, int>()
            };
            
            onSuccess?.Invoke(reward);
        }
        
        public void PurchasePremium(Action<bool> onSuccess, Action<string> onError = null)
        {
            // Заглушка
            Debug.Log("Purchasing premium battle pass");
            
            // потом заменить на API
            onSuccess?.Invoke(true);
        }
    }
}