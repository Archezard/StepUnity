using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Presenters;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class TasksView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button _backButton;
        [SerializeField] private Transform _tasksContainer;
        [SerializeField] private GameObject _taskPrefab;
        [SerializeField] private ScrollRect _scrollRect;
        
        private BattlePassPresenter _presenter;
        private BattlePassService _battlePassService;
        
        public void Initialize(BattlePassPresenter presenter, BattlePassService battlePassService)
        {
            _presenter = presenter;
            _battlePassService = battlePassService;
            
            _backButton.onClick.AddListener(OnBackButtonClicked);
            
            LoadTasks();
        }
        
        private void LoadTasks()
        {
            // Заглушка для загрузки заданий
            var tasks = new List<BattlePassTask>
            {
                new BattlePassTask { Name = "Выиграть матч в 5 на 5", Experience = 200, Progress = 0, Target = 1 },
                new BattlePassTask { Name = "Кинуть мячик", Experience = 200, Progress = 0, Target = 1 },
                new BattlePassTask { Name = "Скрафтить карточку", Experience = 200, Progress = 0, Target = 1 },
                new BattlePassTask { Name = "Получить 5 карт", Experience = 200, Progress = 0, Target = 5 },
                new BattlePassTask { Name = "Получить серебряную карту", Experience = 200, Progress = 0, Target = 1 }
            };
            
            DisplayTasks(tasks);
        }
        
        private void DisplayTasks(List<BattlePassTask> tasks)
        {
            ClearTasks();
            
            foreach (var task in tasks)
            {
                var taskObject = Instantiate(_taskPrefab, _tasksContainer);
                var taskElement = taskObject.GetComponent<TaskElement>();
                taskElement.Initialize(task);
            }
        }
        
        private void ClearTasks()
        {
            foreach (Transform child in _tasksContainer)
            {
                Destroy(child.gameObject);
            }
        }
        
        private void OnBackButtonClicked()
        {
            _presenter.ShowBattlePass();
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
    
    public class BattlePassTask
    {
        public string Name { get; set; }
        public int Experience { get; set; }
        public int Progress { get; set; }
        public int Target { get; set; }
        public bool IsCompleted => Progress >= Target;
    }
}