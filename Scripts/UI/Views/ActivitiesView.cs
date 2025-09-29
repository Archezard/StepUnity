using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class ActivitiesView : BaseView
    {
        [Header("Activity Buttons")]
        [SerializeField] private Button _getCardButton;
        [SerializeField] private Button _throwBallButton;
        [SerializeField] private Button _openPackButton;
        
        public System.Action OnGetCardSelected;
        public System.Action OnThrowBallSelected;
        public System.Action OnOpenPackSelected;
        
        public void Initialize()
        {
            _getCardButton.onClick.AddListener(() => OnGetCardSelected?.Invoke());
            _throwBallButton.onClick.AddListener(() => OnThrowBallSelected?.Invoke());
            _openPackButton.onClick.AddListener(() => OnOpenPackSelected?.Invoke());
        }
    }
}