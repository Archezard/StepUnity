🏗️ Архитектура проекта
Проект построен по принципам MVP (Model-View-Presenter), что обеспечивает четкое разделение ответственности и упрощает тестирование и поддержку кода.

Основные компоненты архитектуры:
Model - данные и бизнес-логика (сервисы, модели данных)

View - отображение и пользовательский ввод (UI компоненты)

Presenter - посредник между Model и View, обработка логики

🚀 Настройки

Unity 6.1 6000.1.13f1 или новее

WebGL билд

🛠️ Как добавить новый модуль (Также полезно почитать, если надо что-либо редактировать)
1. Создание сервиса (Model)
Создайте новый класс сервиса в папке Scripts/Services/:
```
using BasketballCards.Models;
using System;
using UnityEngine;

namespace BasketballCards.Services
{
    public class NewFeatureService
    {
        private readonly ApiClient _apiClient;

        public NewFeatureService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public void GetData(Action<DataModel> onSuccess, Action<string> onError = null)
        {
            // ЗАГЛУШКА: В реальности здесь будет запрос к API
            Debug.Log("NewFeatureService: Getting data (stub)");
            
            // Временные данные для демонстрации
            var data = new DataModel
            {
                Id = "1",
                Name = "Test Data"
            };

            onSuccess?.Invoke(data);
        }
    }
}
```

2. Регистрация сервиса в GameManager

```
public class GameManager : MonoBehaviour
{
    // ... существующие сервисы
    
    public NewFeatureService NewFeatureService { get; private set; }
    
    private void InitializeServices()
    {
        Debug.Log("GameManager: Initializing services...");
        
        // ... существующие сервисы
        NewFeatureService = new NewFeatureService(ApiClient);
        
        Debug.Log("GameManager: Services initialized");
    }
}
```

3. Создание презентера (Presenter)
Создать новый презентер в папке Scripts/Presenters/:

```
using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Views;
using UnityEngine;

namespace BasketballCards.UI.Presenters
{
    public class NewFeaturePresenter : MonoBehaviour
    {
        [Header("View References")]
        [SerializeField] private NewFeatureView _view;
        
        private GameManager _gameManager;
        private NewFeatureService _service;
        
        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            _service = _gameManager.NewFeatureService;
            
            _view.Initialize(this);
            
            LoadData();
        }
        
        private void LoadData()
        {
            _service.GetData(
                data => {
                    _view.DisplayData(data);
                },
                error => {
                    _view.ShowError(error);
                });
        }
        
        public void OnActionRequested(string parameter)
        {
            // Обработка действий пользователя
            Debug.Log($"Action requested with parameter: {parameter}");
        }
        
        public void Show()
        {
            _view.Show();
        }
        
        public void Hide()
        {
            _view.Hide();
        }
    }
}
```

4. Создание представления (View)
Создать новое представление в папке Scripts/Views/:

```
using BasketballCards.UI.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class NewFeatureView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button _actionButton;
        [SerializeField] private Text _dataText;
        [SerializeField] private Button _backButton;
        
        private NewFeaturePresenter _presenter;
        
        public void Initialize(NewFeaturePresenter presenter)
        {
            _presenter = presenter;
            
            _actionButton.onClick.AddListener(OnActionButtonClicked);
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }
        
        public void DisplayData(DataModel data)
        {
            _dataText.text = data.Name;
        }
        
        public void ShowError(string error)
        {
            Debug.LogError($"NewFeature Error: {error}");
        }
        
        private void OnActionButtonClicked()
        {
            _presenter.OnActionRequested("button_click");
        }
        
        private void OnBackButtonClicked()
        {
            UIManager.Instance.ShowMainMenu();
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
}
```
5. Создание UI элементов
Создайть новый Canvas для вашего модуля

Добавить необходимые UI элементы (кнопки, тексты, изображения)

Настроить анимации и переходы

Привязать View к UI элементам через Inspector

6. Регистрация в Bootstrap
Добавить ссылку на ваш презентер в Bootstrap.cs:

```
public class Bootstrap : MonoBehaviour
{
    // ... существующие ссылки
    
    [Header("New Feature References")]
    [SerializeField] private NewFeaturePresenter _newFeaturePresenter;
    
    private void InitializeSystems()
    {
        // ... существующая инициализация
        
        // Инициализация нового презентера
        _newFeaturePresenter.Initialize(_gameManager);
    }
}
```

7. Добавление навигации
Добавить кнопки навигации в UIManager.cs:

```
public class UIManager : MonoBehaviour
{
    // ... существующие методы
    
    public void ShowNewFeature()
    {
        HideAllContentViews();
        _newFeatureView.Show();
        UpdateHeaderTabs(_newFeatureTab);
    }
}
```

Работа с API
Все запросы к серверу выполняются через ApiClient.cs. Для добавления нового API-метода:

Добавить модель запроса и ответа:

```
public class NewFeatureRequest
{
    public string Action { get; set; }
    public string Data { get; set; }
}

public class NewFeatureResponse
{
    public bool Success { get; set; }
    public string Result { get; set; }
    public string Error { get; set; }
}
```

Добавить метод в ApiClient:

```
public void NewFeatureAction(string data, Action<NewFeatureResponse> onSuccess, Action<string> onError = null)
{
    var request = new NewFeatureRequest { Action = "new_action", Data = data };
    StartCoroutine(PostRequest<NewFeatureResponse>("new_feature", request, onSuccess, onError));
}
```

Использовать в сервисе:

```
public void PerformAction(string data, Action<string> onSuccess, Action<string> onError = null)
{
    _apiClient.NewFeatureAction(data,
        response => {
            if (response.Success)
            {
                onSuccess?.Invoke(response.Result);
            }
            else
            {
                onError?.Invoke(response.Error);
            }
        },
        error => {
            onError?.Invoke(error);
        });
}
```

Настройка UI
Добавление новой кнопки навигации
Добавить кнопку в соответствующий Canvas

Настройте внешний вид (цвет, шрифт, анимации)

Добавить обработчик в скрипт:

```
_button.onClick.AddListener(OnButtonClicked);

private void OnButtonClicked()
{
    UIManager.Instance.ShowNewFeature();
}
```

Создание анимированных переходов
Создать анимацию в Unity

Добавить Animation Controller

Настроить переходы между состояниями

Использовать в коде:

```
public void ShowWithAnimation()
{
    gameObject.SetActive(true);
    _animator.SetTrigger("Show");
}

public void HideWithAnimation()
{
    _animator.SetTrigger("Hide");
    StartCoroutine(DeactivateAfterAnimation());
}

private IEnumerator DeactivateAfterAnimation()
{
    yield return new WaitForSeconds(0.5f); // Длительность анимации
    gameObject.SetActive(false);
}
```

🐛 Известные проблемы

Тут потом можно будет проблемы добавлять? Хз надо ли вообще









