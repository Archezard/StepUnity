using BasketballCards.Models;
using System;
using UnityEngine;

namespace BasketballCards.Core
{
    public class UserDataManager : MonoBehaviour
    {
        public static UserDataManager Instance { get; private set; }
        
        public UserData CurrentUser { get; private set; }
        
        // События для обновления данных
        public event Action<UserData> OnUserDataUpdated;
        public event Action<int, int> OnCurrencyChanged; // oldGold, newGold
        public event Action<int, int> OnDiamondsChanged; // oldDiamonds, newDiamonds
        public event Action<int, int> OnTicketsChanged; // oldTickets, newTickets
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Инициализация пустыми данными
            CurrentUser = new UserData
            {
                user_id = 0,
                username = "Guest",
                gold = 1000,
                diamonds = 100,
                tickets = 5
            };
        }
        
        public void UpdateUserData(UserData newData)
        {
            var oldGold = CurrentUser?.gold ?? 0;
            var oldDiamonds = CurrentUser?.diamonds ?? 0;
            var oldTickets = CurrentUser?.tickets ?? 0;
            
            CurrentUser = newData;
            
            // Вызываем события изменений
            if (oldGold != newData.gold)
                OnCurrencyChanged?.Invoke(oldGold, newData.gold);
                
            if (oldDiamonds != newData.diamonds)
                OnDiamondsChanged?.Invoke(oldDiamonds, newData.diamonds);
                
            if (oldTickets != newData.tickets)
                OnTicketsChanged?.Invoke(oldTickets, newData.tickets);
            
            // Вызываем общее событие через EventSystem
            EventSystem.UpdateUserData(newData);
            OnUserDataUpdated?.Invoke(newData);
        }
        
        public bool HasEnoughGold(int amount) => CurrentUser.gold >= amount;
        public bool HasEnoughDiamonds(int amount) => CurrentUser.diamonds >= amount;
        public bool HasEnoughTickets(int amount) => CurrentUser.tickets >= amount;
        
        public void AddGold(int amount)
        {
            if (CurrentUser != null)
            {
                var oldGold = CurrentUser.gold;
                CurrentUser.gold += amount;
                OnCurrencyChanged?.Invoke(oldGold, CurrentUser.gold);
                OnUserDataUpdated?.Invoke(CurrentUser);
            }
        }
        
        public void SpendGold(int amount)
        {
            if (HasEnoughGold(amount))
            {
                var oldGold = CurrentUser.gold;
                CurrentUser.gold -= amount;
                OnCurrencyChanged?.Invoke(oldGold, CurrentUser.gold);
                OnUserDataUpdated?.Invoke(CurrentUser);
            }
        }
        
        // Аналогичные методы для алмазов и билетов...
    }
}