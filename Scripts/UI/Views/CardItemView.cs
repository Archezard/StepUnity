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
        
        private CardData _cardData;
        private System.Action<CardData> _onSelectAction;
        private System.Action<CardData, bool> _onToggleAction;
        private bool _isSelected = false;
        
        public string CardId => _cardData?.CardId;
        
        public void Initialize(CardData cardData, System.Action<CardData> onSelect, System.Action<CardData, bool> onToggle)
        {
            _cardData = cardData;
            _onSelectAction = onSelect;
            _onToggleAction = onToggle;
            
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
            _selectionBorder.gameObject.SetActive(_isSelected);
        }
        
        public void SetSelected(bool selected)
        {
            _isSelected = selected;
            _selectionBorder.gameObject.SetActive(selected);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            // карточка открывается в 3D просмотре
            BasketballCards.Core.EventSystem.RequestCardView(_cardData);
        }
    }
}