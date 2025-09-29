using BasketballCards.Services;
using UnityEngine;

namespace BasketballCards.UI.Views
{
    public class TasksView : BaseView
    {
        private BattlePassService _battlePassService;
        
        public void Initialize(BattlePassService battlePassService)
        {
            _battlePassService = battlePassService;
            // Инициализация представления заданий
        }
    }
}