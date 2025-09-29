using BasketballCards.Models;
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
        [SerializeField] private Transform _3dCardTransform;
        
        [Header("UI References")]
        [SerializeField] private GameObject _cardDisplayPanel;
        [SerializeField] private Image _cardDisplayImage;
        [SerializeField] private Image _playerImage;
        [SerializeField] private TextMeshPro _playerNameText;
        [SerializeField] private TextMeshPro _attackText;
        [SerializeField] private TextMeshPro _defenseText;
        [SerializeField] private TextMeshPro _staminaText;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _resetRotationButton;
        
        [Header("Rotation Settings")]
        [SerializeField] private float _rotationSpeed = 0.5f;
        [SerializeField] private bool _invertX = false;
        [SerializeField] private bool _invertY = false;
        
        private bool _isDragging = false;
        private Vector2 _previousTouchPosition;
        
        public void Initialize()
        {
            _closeButton.onClick.AddListener(HideCard);
            _resetRotationButton.onClick.AddListener(ResetRotation);
            
            HideCard();
        }
        
        private void Start()
        {
            _closeButton.onClick.AddListener(HideCard);
            _resetRotationButton.onClick.AddListener(ResetRotation);
            
            // Скрытие элементов при старте. Потом мб делать это не тут?
            _cardDisplayPanel.SetActive(false);
            _cardCamera.gameObject.SetActive(false);
            _cardDisplayImage.gameObject.SetActive(false);
            _closeButton.gameObject.SetActive(false);
            _resetRotationButton.gameObject.SetActive(false);
        }
        
        public void ShowCard(CardData card)
        {
            _playerNameText.text = card.PlayerName;
            _attackText.text = $"Атака: {card.Attack}";
            _defenseText.text = $"Защита: {card.Defense}";
            _staminaText.text = $"Выносливость: {card.Stamina}";
            
            _cardDisplayPanel.SetActive(true);
            _cardCamera.gameObject.SetActive(true);
            
            ResetRotation();
            Setup3DCard(card);
        }
        
        private void LoadPlayerImage(string imageUrl)
        {
            // Здесь будет реализация загрузки изображения
            // Пока используем заглушку - случайный цвет
            _playerImage.color = new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
            );
        }
        
         private void Setup3DCard(CardData card)
        {
            Renderer renderer = _3dCardTransform.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = card.RarityColor;
            }
        }
        
        public void HideCard()
        {
            _cardDisplayPanel.SetActive(false);
            _cardCamera.gameObject.SetActive(false);
        }
        
        public void ResetRotation()
        {
            _3dCardTransform.rotation = Quaternion.identity;
        }
        
        // Обработка касаний через EventSystem
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
            if (!_isDragging) return;
            
            Vector2 delta = eventData.position - _previousTouchPosition;
            RotateCard(delta.x, delta.y);
            _previousTouchPosition = eventData.position;
        }
        
        private void RotateCard(float deltaX, float deltaY)
        {
            // Применяем инверсию, если вдруг будет нужно. По приколу сделал, ибо ну а нахуя... Хотя хз мб в настройках добавить
            float rotationX = _invertY ? -deltaY : deltaY;
            float rotationY = _invertX ? -deltaX : deltaX;
            
            // Вращаем карточку вокруг обеих осей
            _3dCardTransform.Rotate(rotationX * _rotationSpeed, 
                                   -rotationY * _rotationSpeed, 
                                   0, 
                                   Space.World);
        }
    }
}