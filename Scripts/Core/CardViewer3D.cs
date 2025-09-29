using BasketballCards.Models;
using BasketballCards.UI.Views;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace BasketballCards.Core
{
    public class CardViewer3D : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {        
        [Header("3D References")]
        [SerializeField] private Camera _cardCamera;
        [SerializeField] private Transform _cardContainer; // Контейнер для 3D карточки
        
        [Header("Card Prefab")]
        [SerializeField] private GameObject _cardPrefab; // Префаб CardItemView
        
        [Header("UI References")]
        [SerializeField] private GameObject _cardDisplayPanel;
        [SerializeField] private TextMeshPro _playerNameText;
        [SerializeField] private TextMeshPro _rarityText;
        [SerializeField] private TextMeshPro _levelText;
        [SerializeField] private TextMeshPro _attackText;
        [SerializeField] private TextMeshPro _defenseText;
        [SerializeField] private TextMeshPro _staminaText;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _resetRotationButton;
        [SerializeField] private Button _upgradeButton;
        
        [Header("Rotation Settings")]
        [SerializeField] private float _rotationSpeed = 0.5f;
        [SerializeField] private bool _invertX = false;
        [SerializeField] private bool _invertY = false;
        
        private bool _isDragging = false;
        private Vector2 _previousTouchPosition;
        private GameObject _currentCardInstance;
        private CardData _currentCardData;

        public System.Action OnUpgradeRequested;
        public System.Action OnCloseRequested;
        
        public void Initialize()
        {
            _closeButton.onClick.AddListener(() => OnCloseRequested?.Invoke());
            _resetRotationButton.onClick.AddListener(ResetRotation);
            _upgradeButton.onClick.AddListener(() => OnUpgradeRequested?.Invoke());
            
            HideCard();
        }
        
        public void ShowCard(CardData card)
        {
            _currentCardData = card;
            
            // Создаем экземпляр карточки если его нет
            if (_currentCardInstance == null && _cardPrefab != null)
            {
                _currentCardInstance = Instantiate(_cardPrefab, _cardContainer);
                
                // Отключаем ненужные компоненты для 3D просмотра
                var cardItemView = _currentCardInstance.GetComponent<CardItemView>();
                if (cardItemView != null)
                {
                    // Сохраняем ссылки на нужные компоненты или настраиваем для 3D
                }
            }
            
            // Обновляем информацию о карточке
            UpdateCardInfo(card);
            
            _cardDisplayPanel.SetActive(true);
            _cardCamera.gameObject.SetActive(true);
            
            ResetRotation();
            SetupCardVisuals(card);
        }
        
        private void UpdateCardInfo(CardData card)
        {
            _playerNameText.text = card.PlayerName;
            _rarityText.text = $"Редкость: {card.Rarity}";
            _levelText.text = $"Уровень: {card.Level}/{card.MaxLevel}";
            _attackText.text = $"Атака: {card.Attack}";
            _defenseText.text = $"Защита: {card.Defense}";
            _staminaText.text = $"Выносливость: {card.Stamina}";
            
            // Настраиваем кнопку улучшения ХУЙНЯ КАКАЯ-ТО
            _upgradeButton.interactable = card.Level < card.MaxLevel;
            _upgradeButton.GetComponentInChildren<TextMeshPro>().text = 
                card.Level < card.MaxLevel ? $"Улучшить ({GetUpgradeCost(card)} золота)" : "Макс. уровень";
        }
        
        private int GetUpgradeCost(CardData card)
        {
            // Тут временно такая редкость, но потом это надо под Апи загнать
            switch (card.Rarity)
            {
                case Rarity.Bronze:
                    return card.Level == 1 ? 100 : card.Level == 2 ? 300 : 500;
                case Rarity.Silver:
                    return card.Level == 1 ? 500 : card.Level == 2 ? 1000 : 2000;
                case Rarity.Gold:
                    return card.Level == 1 ? 1500 : card.Level == 2 ? 3000 : 5000;
                case Rarity.Diamond:
                    return card.Level == 1 ? 5000 : card.Level == 2 ? 10000 : 20000;
                case Rarity.Legendary:
                    return card.Level == 1 ? 7500 : card.Level == 2 ? 15000 : 30000;
                default:
                    return 0;
            }
        }
        
        private void SetupCardVisuals(CardData card)
        {
            if (_currentCardInstance != null)
            {
                // Визуальное отображение карточки
                var renderer = _currentCardInstance.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = card.RarityColor;
                }
                
                // Нужно потом добавить дополнительные настройки для 3D отображения
            }
        }
        
        public void HideCard()
        {
            _cardDisplayPanel.SetActive(false);
            _cardCamera.gameObject.SetActive(false);
            
            // Очищаем текущую карточку
            _currentCardData = null;
        }
        
        public void ResetRotation()
        {
            if (_currentCardInstance != null)
            {
                _currentCardInstance.transform.rotation = Quaternion.identity;
            }
        }
        
        // Обработка вращения карточки
        public void OnPointerDown(PointerEventData eventData)
        {
            _isDragging = true;
            _previousTouchPosition = eventData.position;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            _isDragging = false;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragging || _currentCardInstance == null) return;
            
            Vector2 delta = eventData.position - _previousTouchPosition;
            RotateCard(delta.x, delta.y);
            _previousTouchPosition = eventData.position;
        }
        
        private void RotateCard(float deltaX, float deltaY)
        {
            float rotationX = _invertY ? -deltaY : deltaY;
            float rotationY = _invertX ? -deltaX : deltaX;
            
            _currentCardInstance.transform.Rotate(rotationX * _rotationSpeed, 
                                   -rotationY * _rotationSpeed, 
                                   0, 
                                   Space.World);
        }
        
        private void OnDestroy()
        {
            if (_currentCardInstance != null)
            {
                Destroy(_currentCardInstance);
            }
        }
    }
}