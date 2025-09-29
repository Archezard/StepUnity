using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BasketballCards.Core;

namespace BasketballCards.UI.Views
{
    public class ActivitiesHeaderView : BaseHeaderView
    {
        [Header("Activities Header Buttons")]
        [SerializeField] private Button _getCardButton;
        [SerializeField] private Button _throwBallButton;
        [SerializeField] private Button _openPackButton;
        
        public System.Action<ActivitiesSubScreen> OnSubScreenSelected;
        
        public override void Initialize()
        {
            _headerButtons = new Button[] { _getCardButton, _throwBallButton, _openPackButton };
            
            _getCardButton.onClick.AddListener(() => 
            {
                OnSubScreenSelected?.Invoke(ActivitiesSubScreen.GetCard);
                SetActiveButton(_getCardButton);
            });
            
            _throwBallButton.onClick.AddListener(() => 
            {
                OnSubScreenSelected?.Invoke(ActivitiesSubScreen.ThrowBall);
                SetActiveButton(_throwBallButton);
            });
            
            _openPackButton.onClick.AddListener(() => 
            {
                OnSubScreenSelected?.Invoke(ActivitiesSubScreen.OpenPack);
                SetActiveButton(_openPackButton);
            });
            
            // Устанавливаем начальное состояние
            SetActiveButton(_getCardButton);
        }
    }
}