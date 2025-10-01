using BasketballCards.Models;
using BasketballCards.Core;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace BasketballCards.UI.Views
{
    public class CardItemView : MonoBehaviour, IPointerClickHandler
    {
        [Header("UI References")]
        [SerializeField] private Image _cardBackground;
        [SerializeField] private Image _playerImage;
        [SerializeField] private TextMeshProUGUI _playerNameText;
        [SerializeField] private Image _rarityIndicator;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _duplicatesText;
        [SerializeField] private Image _selectionBorder;
        [SerializeField] private GameObject _levelStarsContainer;
        [SerializeField] private Image[] _levelStars;
        
        private CardData _cardData;
        private System.Action<CardData> _onSelectAction;
        private System.Action<CardData, bool> _onToggleAction;
        private bool _isSelected = false;
        private bool _allowMultipleSelection = false;
        
        public string CardId => _cardData?.CardId;
        
        public void Initialize(CardData cardData, System.Action<CardData> onSelect, System.Action<CardData, bool> onToggle)
        {
            _cardData = cardData;
            _onSelectAction = onSelect;
            _onToggleAction = onToggle;
            _allowMultipleSelection = onToggle != null;
            
            UpdateUI();
        }
        
        public void UpdateCardData(CardData cardData)
        {
            _cardData = cardData;
            UpdateUI();
        }
        
        private void UpdateUI()
        {
            _playerNameText.text = _cardData.PlayerName;
            _rarityIndicator.color = _cardData.RarityColor;
            _levelText.text = $"Ур. {_cardData.Level}";
            _duplicatesText.text = _cardData.Duplicates > 0 ? $"x{_cardData.Duplicates}" : "";
            
            _cardBackground.color = _cardData.RarityColor;
            _selectionBorder.gameObject.SetActive(_isSelected && _allowMultipleSelection);
            
            // Обновляем звезды уровня
            UpdateLevelStars();
        }
        
        private void UpdateLevelStars()
        {
            if (_levelStarsContainer == null || _levelStars == null) return;
            
            for (int i = 0; i < _levelStars.Length; i++)
            {
                if (_levelStars[i] != null)
                {
                    _levelStars[i].gameObject.SetActive(i < _cardData.Level);
                }
            }
        }
        
        public void SetSelected(bool selected)
        {
            if (!_allowMultipleSelection) return;
            
            _isSelected = selected;
            _selectionBorder.gameObject.SetActive(selected);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_allowMultipleSelection)
            {
                // В режиме множественного выбора - переключаем выделение
                _isSelected = !_isSelected;
                _selectionBorder.gameObject.SetActive(_isSelected);
                _onToggleAction?.Invoke(_cardData, _isSelected);
            }
            else
            {
                // В режиме одиночного выбора - сразу открываем просмотр
                _onSelectAction?.Invoke(_cardData);
            }
        }
    }
}