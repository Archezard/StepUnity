using BasketballCards.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public abstract class BaseView : MonoBehaviour
    {
        [Header("Common UI Elements")]
        [SerializeField] protected Button _backButton;
        [SerializeField] protected TextMeshProUGUI _titleText;
        
        protected virtual void Awake()
        {
            if (_backButton != null)
            {
                _backButton.onClick.AddListener(OnBackButtonClicked);
            }
        }
        
        protected virtual void OnBackButtonClicked()
        {
            EventSystem.NavigateBack();
        }
        
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
        
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public virtual void ShowError(string message)
        {
            Debug.LogError($"{GetType().Name} Error: {message}");
            // Здесь можно добавить показ UI ошибки
        }
        
        public virtual void ShowSuccess(string message)
        {
            Debug.Log($"{GetType().Name} Success: {message}");
            // Здесь можно добавить показ UI успешного сообщения
        }
    }
}