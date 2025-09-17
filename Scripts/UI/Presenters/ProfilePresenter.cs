using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Views;
using UnityEngine;

namespace BasketballCards.UI.Presenters
{
    public class ProfilePresenter : MonoBehaviour
    {
        [Header("View References")]
        [SerializeField] private ProfileView _profileView;
        //[SerializeField] private FriendsView _friendsView;
        //[SerializeField] private SettingsView _settingsView;
        
        private GameManager _gameManager;
        private ProfileService _profileService;
        
        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            _profileService = new ProfileService(_gameManager.ApiClient);
            
            // Исправленная инициализация с одним параметром
            _profileView.Initialize(this);
            
            HideAllSubsections();
            _profileView.Show();
            
            Debug.Log("ProfilePresenter: Initialized");
        }
        
        public void ShowProfile()
        {
            HideAllSubsections();
            _profileView.Show();
        }
        
        public void ShowFriends()
        {
            HideAllSubsections();
            //_friendsView.Show();
        }
        
        public void ShowSettings()
        {
            HideAllSubsections();
            //_settingsView.Show();
        }
        
        private void HideAllSubsections()
        {
            _profileView.Hide();
            //_friendsView.Hide();
            //_settingsView.Hide();
        }
        
        public void OnProfileUpdated()
        {
            // Обновляем данные пользователя
            _gameManager.UserService.GetUserData(_gameManager.CurrentUser.username, 
                userData => {
                    _gameManager.SetCurrentUser(userData);
                },
                error => {
                    Debug.LogError("Failed to update user data: " + error);
                });
        }
    }
}