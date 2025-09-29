using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BasketballCards.UI.Views
{
    public abstract class BaseHeaderView : MonoBehaviour
    {
        [Header("Header Settings")]
        [SerializeField] protected Color _activeColor = new Color(1f, 0.5f, 0f); // Orange
        [SerializeField] protected Color _inactiveColor = Color.white;
        
        protected Button[] _headerButtons;
        
        public abstract void Initialize();
        
        protected virtual void SetActiveButton(Button activeButton)
        {
            foreach (var button in _headerButtons)
            {
                var text = button.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null)
                {
                    text.color = (button == activeButton) ? _activeColor : _inactiveColor;
                }
                
                // Также можно изменить цвет фона кнопки
                var colors = button.colors;
                colors.normalColor = (button == activeButton) ? _activeColor : Color.white;
                button.colors = colors;
            }
        }
    }
}