using BasketballCards.Models;
using BasketballCards.Services;
using UnityEngine;

namespace BasketballCards.UI.Views
{
    public class RewardsView : BaseView
    {
        private BattlePassService _battlePassService;
        
        public System.Action<int, bool> OnRewardClaimed;
        
        public void Initialize(BattlePassService battlePassService)
        {
            _battlePassService = battlePassService;
        }
        
        public void DisplayRewards(BattlePassProgress progress)
        {
            // Отображение наград баттл-пасса
            // Здесь будет логика отображения списка наград
        }
    }
}