using BasketballCards.UI.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class ThrowBallView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button _throwButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private TextMeshProUGUI _ballsCountText;
        [SerializeField] private GameObject _hoop;
        [SerializeField] private GameObject _ball;
        
        private ActivitiesPresenter _presenter;
        
        public void Initialize(ActivitiesPresenter presenter)
        {
            _presenter = presenter;
            
            _throwButton.onClick.AddListener(OnThrowButtonClicked);
            _backButton.onClick.AddListener(OnBackButtonClicked);
            
            UpdateBallsCount(5);
        }
        
        private void UpdateBallsCount(int count)
        {
            _ballsCountText.text = $"Мячей: {count}";
            _throwButton.interactable = count > 0;
        }
        
        private void OnThrowButtonClicked()
        {
            _presenter.ThrowBall();
        }
        
        private void OnBackButtonClicked()
        {
            _presenter.ShowActivities();
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public void ShowError(string error)
        {
            Debug.LogError($"ThrowBall Error: {error}");
        }
    }
}