using BasketballCards.Models;
using System;
using UnityEngine;

namespace BasketballCards.Services
{
    public class UserService
    {
        private readonly ApiClient _apiClient;

        public UserService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public void GetUserData(string username, Action<UserData> onSuccess, Action<string> onError = null)
        {
            // Заглушка для получения данных пользователя
            var userData = new UserData
            {
                user_id = 1,
                username = username,
                gold = 1000,
                diamonds = 100,
                tickets = 5
            };

            onSuccess?.Invoke(userData);
        }

        public void SaveUserData(UserData userData, Action onSuccess, Action<string> onError = null)
        {
            // Заглушка для сохранения данных пользователя
            Debug.Log("User data saved");
            onSuccess?.Invoke();
        }
    }
}