using BasketballCards.Models;
using UnityEngine;

namespace BasketballCards.Core
{
    public class UserDataManager : MonoBehaviour
    {
        public static UserDataManager Instance { get; private set; }
        
        public UserData CurrentUser { get; private set; }
        
        public event System.Action<UserData> OnUserDataUpdated;
        public event System.Action<int, int> OnCurrencyChanged; // oldGold, newGold
        public event System.Action<int, int> OnDiamondsChanged; // oldDiamonds, newDiamonds
        public event System.Action<int, int> OnTicketsChanged; // oldTickets, newTickets
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        public void UpdateUserData(UserData userData)
        {
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