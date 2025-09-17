using BasketballCards.Models;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace BasketballCards.UI.Views
{
    public class CardUI : MonoBehaviour, IPointerClickHandler
    {
        [Header("UI References")]
        [SerializeField] private Image _cardImage;
        [SerializeField] private TextMeshProUGUI _playerNameText;
        [SerializeField] private TextMeshProUGUI _rarityText;
        [SerializeField] private Image _borderImage;
        
        private CardData _cardData;
        private System.Action<CardData> _onCardSelected;
        
        public void Initialize(CardData cardData, System.Action<CardData> onCardSelected)
        {
            _cardData = cardData;
            _onCardSelected = onCardSelected;
            
            UpdateUI();
        }
        
        private void UpdateUI()
        {
            _playerNameText.text = _cardData.PlayerName;
            _rarityText.text = $"Rarity: {_cardData.Rarity}";
            _borderImage.color = _cardData.RarityColor;
            
            // Здесь будет загрузка изображения карточки
            // Пока используем заглушку
            _cardImage.color = _cardData.RarityColor;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _onCardSelected?.Invoke(_cardData);
        }
        
        public void SetSelected(bool selected)
        {
            // Визуальное выделение выбранной карточки
            _borderImage.color = selected ? Color.green : _cardData.RarityColor;
        }
    }
}