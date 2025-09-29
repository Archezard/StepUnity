using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Views;
using UnityEngine;

namespace BasketballCards.UI.Presenters
{
    public class FiveOnFivePresenter : BasePresenter
    {
        [Header("View References")]
        [SerializeField] private FiveOnFiveView _fiveOnFiveView;
        
        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            
            //EventSystem.OnTeamSetupRequested += HandleTeamSetupRequested;
            //EventSystem.OnTacticsChanged += HandleTacticsChanged;
            
            // Подписка на события данных пользователя
            UserDataManager.Instance.OnUserDataUpdated += HandleUserDataUpdated;
        }
        
        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            
            //EventSystem.OnTeamSetupRequested -= HandleTeamSetupRequested;
            //EventSystem.OnTacticsChanged -= HandleTacticsChanged;
            
            if (UserDataManager.Instance != null)
            {
                UserDataManager.Instance.OnUserDataUpdated -= HandleUserDataUpdated;
            }
        }
        
        private void Start()
        {
            // Инициализация View
            if (_fiveOnFiveView != null)
            {
                _fiveOnFiveView.Initialize();
                _fiveOnFiveView.OnStartGame += OnStartGameRequested;
                _fiveOnFiveView.OnTeamSetup += OnTeamSetupRequested;
                _fiveOnFiveView.OnTactics += OnTacticsRequested;
                _fiveOnFiveView.OnLeaderboard += OnLeaderboardRequested;
            }
        }
        
        public override void Show()
        {
            if (_fiveOnFiveView != null)
            {
                _fiveOnFiveView.Show();
            }
        }
        
        public override void Hide()
        {
            if (_fiveOnFiveView != null)
            {
                _fiveOnFiveView.Hide();
            }
        }
        
        protected override void HandleUserDataUpdated(UserData userData)
        {
            // Обновление данных, если необходимо
            Debug.Log($"FiveOnFivePresenter: User data updated");
        }
        
        private void HandleTeamSetupRequested()
        {
            // Обработка настройки команды
            EventSystem.ShowSuccess("Настройка команды");
        }
        
        private void HandleTacticsChanged()
        {
            // Обработка изменения тактики
            EventSystem.ShowSuccess("Тактика изменена");
        }
        
        // Методы для работы с игрой 5 на 5
        private void OnStartGameRequested()
        {
            EventSystem.ShowSuccess("Начинаем игру 5 на 5!");
        }
        
        private void OnTeamSetupRequested()
        {
            //EventSystem.RequestTeamSetup();
        }
        
        private void OnTacticsRequested()
        {
            //EventSystem.ChangeTactics();
        }
        
        private void OnLeaderboardRequested()
        {
            EventSystem.ShowSuccess("Таблица лидеров");
        }
    }
}