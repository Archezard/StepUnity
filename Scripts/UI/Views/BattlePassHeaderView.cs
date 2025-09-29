using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BasketballCards.Core;

namespace BasketballCards.UI.Views
{
    public class BattlePassHeaderView : BaseHeaderView
    {
        [Header("BattlePass Header Buttons")]
        [SerializeField] private Button _tasksButton;
        [SerializeField] private Button _rewardsButton;
        
        public System.Action<BattlePassSubScreen> OnSubScreenSelected;
        
        public override void Initialize()
        {
            _headerButtons = new Button[] { _tasksButton, _rewardsButton };
            
            _tasksButton.onClick.AddListener(() => 
            {
                OnSubScreenSelected?.Invoke(BattlePassSubScreen.Tasks);
                SetActiveButton(_tasksButton);
            });
            
            _rewardsButton.onClick.AddListener(() => 
            {
                OnSubScreenSelected?.Invoke(BattlePassSubScreen.Rewards);
                SetActiveButton(_rewardsButton);
            });
            
            // Устанавливаем начальное состояние
            SetActiveButton(_tasksButton);
        }
    }
}