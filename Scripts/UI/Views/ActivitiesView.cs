using BasketballCards.Managers;
using BasketballCards.UI.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class ActivitiesView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button _getCardButton;
        [SerializeField] private Button _throwBallButton;
        [SerializeField] private Button _openPackButton;
        [SerializeField] private Button _backButton;
        
        private ActivitiesPresenter _presenter;
        
        public void Initialize(ActivitiesPresenter presenter)
        {
            _presenter = presenter;
            
            _getCardButton.onClick.AddListener(OnGetCardButtonClicked);
            _throwBallButton.onClick.AddListener(OnThrowBallButtonClicked);
            _openPackButton.onClick.AddListener(OnOpenPackButtonClicked);
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }
        
        private void OnGetCardButtonClicked()
        {
            _presenter.GetFreeCard(); // View вызывает метод Presenter'a
        }
        
        private void OnThrowBallButtonClicked()
        {
            _presenter.ShowThrowBall();
        }
        
        private void OnOpenPackButtonClicked()
        {
            _presenter.ShowOpenPack();
        }
        
        private void OnBackButtonClicked()
        {
            UIManager.Instance.ShowMainMenu();
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