using BasketballCards.Core;
using BasketballCards.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class ProfileView : BaseView
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _usernameText;
        [SerializeField] private TextMeshProUGUI _goldText;
        [SerializeField] private TextMeshProUGUI _diamondsText;
        [SerializeField] private TextMeshProUGUI _ticketsText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _collectionSizeText;
        [SerializeField] private Button _editProfileButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _friendsButton;
        
        public void Initialize()
        {
            _editProfileButton.onClick.AddListener(OnEditProfileButtonClicked);
            _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            _friendsButton.onClick.AddListener(OnFriendsButtonClicked);
        }
        
        public void DisplayProfile(UserData userData)
        {
            _usernameText.text = userData.username ?? "Игрок";
            _goldText.text = userData.gold.ToString();
            _diamondsText.text = userData.diamonds.ToString();
            _ticketsText.text = userData.tickets.ToString();
            
            // Дополнительная информация (заглушки)
            _levelText.text = "Уровень: 15";
            _collectionSizeText.text = "Коллекция: 127 карт";
        }
        
        private void OnEditProfileButtonClicked()
        {
            EventSystem.ShowSuccess("Редактирование профиля");
            // Здесь будет открытие окна редактирования профиля
        }
        
        private void OnSettingsButtonClicked()
        {
            EventSystem.ShowSuccess("Настройки");
            // Здесь будет открытие настроек
        }
        
        private void OnFriendsButtonClicked()
        {
            EventSystem.ShowSuccess("Друзья");
            // Здесь будет открытие списка друзей
        }
    }
}