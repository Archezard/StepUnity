using BasketballCards.Models;
using UnityEngine;

namespace BasketballCards.Core
{
    public class UserDataManager : MonoBehaviour
    {
        public static UserDataManager Instance { get; private set; }
        
        public UserData CurrentUser { get; private set; }
        
        public event System.Action<UserData> OnUserDataUpdated;
        public event System.Action<int, int> OnCurrencyChanged;
        public event System.Action<int, int> OnDiamondsChanged;
        public event System.Action<int, int> OnTicketsChanged;
        
        // Флаг готовности
        public bool IsReady { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            IsReady = true; // Помечаем как готовый
        }
        
        public void UpdateUserData(UserData userData)
        {
            if (!IsReady) return;
            
            var oldGold = CurrentUser?.gold ?? 0;
            var oldDiamonds = CurrentUser?.diamonds ?? 0;
            var oldTickets = CurrentUser?.tickets ?? 0;
            
            CurrentUser = userData;
            
            OnUserDataUpdated?.Invoke(userData);
            
            if (oldGold != userData.gold)
                OnCurrencyChanged?.Invoke(oldGold, userData.gold);
                
            if (oldDiamonds != userData.diamonds)
                OnDiamondsChanged?.Invoke(oldDiamonds, userData.diamonds);
                
            if (oldTickets != userData.tickets)
                OnTicketsChanged?.Invoke(oldTickets, userData.tickets);
        }
    }
}