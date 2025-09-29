using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BasketballCards.Core;

namespace BasketballCards.UI.Views
{
    public class CollectionHeaderView : BaseHeaderView
    {
        [Header("Collection Header Buttons")]
        [SerializeField] private Button _collectionButton;
        [SerializeField] private Button _workshopButton;
        [SerializeField] private Button _albumButton;
        [SerializeField] private Button _exchangeButton;
        
        public System.Action<CollectionSubScreen> OnSubScreenSelected;
        
        public override void Initialize()
        {
            _headerButtons = new Button[] { _collectionButton, _workshopButton, _albumButton, _exchangeButton };
            
            _collectionButton.onClick.AddListener(() => 
            {
                OnSubScreenSelected?.Invoke(CollectionSubScreen.Collection);
                SetActiveButton(_collectionButton);
            });
            
            _workshopButton.onClick.AddListener(() => 
            {
                OnSubScreenSelected?.Invoke(CollectionSubScreen.Workshop);
                SetActiveButton(_workshopButton);
            });
            
            _albumButton.onClick.AddListener(() => 
            {
                OnSubScreenSelected?.Invoke(CollectionSubScreen.Album);
                SetActiveButton(_albumButton);
            });
            
            _exchangeButton.onClick.AddListener(() => 
            {
                OnSubScreenSelected?.Invoke(CollectionSubScreen.Exchange);
                SetActiveButton(_exchangeButton);
            });
            
            // Устанавливаем начальное состояние
            SetActiveButton(_collectionButton);
        }
    }
}