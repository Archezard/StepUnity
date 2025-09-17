using BasketballCards.Models;
using BasketballCards.UI.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class ProfileView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _usernameText;
        [SerializeField] private TextMeshProUGUI _goldText;
        [SerializeField] private TextMeshProUGUI _diamondsText;
        [SerializeField] private TextMeshProUGUI _ticketsText;
        [SerializeField] private Button _editProfileButton;
        [SerializeField] private Button _settingsButton;
        
        private ProfilePresenter _presenter;
        
        public void Initialize(ProfilePresenter presenter)
        {
            _presenter = presenter;
            
            _editProfileButton.onClick.AddListener(OnEditProfileButtonClicked);
            _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        }
        
        public void DisplayProfile(UserData userData)
        {
            _usernameText.text = userData.username;
            _goldText.text = userData.gold.ToString();
            _diamondsText.text = userData.diamonds.ToString();
            _ticketsText.text = userData.tickets.ToString();
        }
        
        private void OnEditProfileButtonClicked()
        {
            // Редактирование профиля
        }
        
        private void OnSettingsButtonClicked()
        {
            // Настройки
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}