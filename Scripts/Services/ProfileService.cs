using BasketballCards.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BasketballCards.Services
{
    public class ProfileService
    {
        private readonly ApiClient _apiClient;
        
        public ProfileService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        public void GetUserProfile(string userId, Action<UserProfile> onSuccess, Action<string> onError = null)
        {
            // Заглушка для демонстрации
            Debug.Log("Getting user profile: " + userId);
            
            // После тут будет запрос к API
            var profile = new UserProfile
            {
                UserId = userId,
                Username = "Player" + userId,
                Level = 10,
                Experience = 2500,
                MatchesPlayed = 100,
                MatchesWon = 60,
                CollectionSize = 150
            };
            
            onSuccess?.Invoke(profile);
        }
        
        public void GetFriends(Action<List<Friend>> onSuccess, Action<string> onError = null)
        {
            // Заглушка для демонстрации
            Debug.Log("Getting friends list");
            
            // После тут будет запрос к API
            var friends = new List<Friend>();
            onSuccess?.Invoke(friends);
        }
        
        public void AddFriend(string userId, Action<bool> onSuccess, Action<string> onError = null)
        {
            // Заглушка для демонстрации
            Debug.Log("Adding friend: " + userId);
            
            // После тут будет запрос к API
            onSuccess?.Invoke(true);
        }
        
        public void UpdateProfile(UserProfile profile, Action<bool> onSuccess, Action<string> onError = null)
        {
            // Заглушка для демонстрации
            Debug.Log("Updating profile");
            
            // После тут будет запрос к API
            onSuccess?.Invoke(true);
        }
    }
    
    public class UserProfile
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int MatchesPlayed { get; set; }
        public int MatchesWon { get; set; }
        public int CollectionSize { get; set; }
    }
    
    public class Friend
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public int Level { get; set; }
        public bool IsOnline { get; set; }
    }
}