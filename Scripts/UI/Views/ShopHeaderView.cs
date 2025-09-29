using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BasketballCards.Core;

namespace BasketballCards.UI.Views
{
    public class ShopHeaderView : BaseHeaderView
    {
        [Header("Shop Header Buttons")]
        [SerializeField] private Button _cardsButton;
        [SerializeField] private Button _throwsButton;
        [SerializeField] private Button _packsButton;
        [SerializeField] private Button _currencyButton;
        
        public System.Action<ShopSubScreen> OnSubScreenSelected;
        
        public override void Initialize()
        {
            _headerButtons = new Button[] { _cardsButton, _throwsButton, _packsButton, _currencyButton };
            
            _cardsButton.onClick.AddListener(() => 
            {
                OnSubScreenSelected?.Invoke(ShopSubScreen.Cards);
                SetActiveButton(_cardsButton);
            });
            
            _throwsButton.onClick.AddListener(() => 
            {
                OnSubScreenSelected?.Invoke(ShopSubScreen.Throws);
                SetActiveButton(_throwsButton);
            });
            
            _packsButton.onClick.AddListener(() => 
            {
                OnSubScreenSelected?.Invoke(ShopSubScreen.Packs);
                SetActiveButton(_packsButton);
            });
            
            _currencyButton.onClick.AddListener(() => 
            {
                OnSubScreenSelected?.Invoke(ShopSubScreen.Currency);
                SetActiveButton(_currencyButton);
            });
            
            // Устанавливаем начальное состояние
            SetActiveButton(_cardsButton);
        }
    }
}