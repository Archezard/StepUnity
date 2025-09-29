using BasketballCards.Models;
using BasketballCards.Services;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class TasksView : BaseView
    {
        [Header("UI References")]
        [SerializeField] private Transform _tasksContainer;
        [SerializeField] private GameObject _taskPrefab;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private TextMeshProUGUI _dailyTasksTitle;
        [SerializeField] private TextMeshProUGUI _monthlyTasksTitle;
        
        private BattlePassService _battlePassService;
        private List<TaskElement> _taskElements = new List<TaskElement>();
        
        public System.Action OnBackRequested;
        
        public void Initialize(BattlePassService battlePassService)
        {
            _battlePassService = battlePassService;
            
            LoadTasks();
        }
        
        protected override void OnBackButtonClicked()
        {
            OnBackRequested?.Invoke();
        }
        
        private void LoadTasks()
        {
            // Загрузка заданий из сервиса
            // Временная заглушка
            var dailyTasks = new List<BattlePassTask>
            {
                new BattlePassTask { Name = "Выиграть матч в 5 на 5", Experience = 200, Progress = 0, Target = 1, IsDaily = true },
                new BattlePassTask { Name = "Кинуть мячик", Experience = 200, Progress = 0, Target = 1, IsDaily = true },
                new BattlePassTask { Name = "Скрафтить карточку", Experience = 200, Progress = 0, Target = 1, IsDaily = true }
            };
            
            var monthlyTasks = new List<BattlePassTask>
            {
                new BattlePassTask { Name = "Получить 50 карт", Experience = 1000, Progress = 25, Target = 50, IsDaily = false },
                new BattlePassTask { Name = "Получить легендарную карту", Experience = 2000, Progress = 0, Target = 1, IsDaily = false },
                new BattlePassTask { Name = "Открыть 3 пака", Experience = 800, Progress = 1, Target = 3, IsDaily = false }
            };
            
            DisplayTasks(dailyTasks, monthlyTasks);
        }
        
        private void DisplayTasks(List<BattlePassTask> dailyTasks, List<BattlePassTask> monthlyTasks)
        {
            ClearTasks();
            
            _dailyTasksTitle.gameObject.SetActive(true);
            foreach (var task in dailyTasks)
            {
                CreateTaskElement(task);
            }
            
            _monthlyTasksTitle.gameObject.SetActive(true);
            foreach (var task in monthlyTasks)
            {
                CreateTaskElement(task);
            }
        }
        
        private void CreateTaskElement(BattlePassTask task)
        {
            var taskObject = Instantiate(_taskPrefab, _tasksContainer);
            var taskElement = taskObject.GetComponent<TaskElement>();
            taskElement.Initialize(task, OnTaskClaimed);
            _taskElements.Add(taskElement);
        }
        
        private void ClearTasks()
        {
            foreach (var element in _taskElements)
            {
                Destroy(element.gameObject);
            }
            _taskElements.Clear();
        }
        
        private void OnTaskClaimed(BattlePassTask task)
        {
            // Запрос на получение награды за задание
            ShowSuccess($"Задание выполнено! Получено {task.Experience} опыта");
            // Здесь будет вызов сервиса для получения награды
        }
    }
}