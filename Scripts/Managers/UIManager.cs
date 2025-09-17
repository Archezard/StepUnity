using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.UI.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        
        [Header("Canvas References")]
        [SerializeField] private Canvas _headerCanvas;
        [SerializeField] private Canvas _contentCanvas;
        [SerializeField] private Canvas _footerCanvas;
        [SerializeField] private Canvas _popupsCanvas;
        
        [Header("Header Elements")]
        [SerializeField] private TextMeshProUGUI _goldText;
        [SerializeField] private TextMeshProUGUI _diamondText;
        [SerializeField] private TextMeshProUGUI _ticketsText;
        [SerializeField] private Button _collectionTab;
        [SerializeField] private Button _shopTab;
        [SerializeField] private Button _activitiesTab;
        [SerializeField] private Button _fiveOnFiveTab;
        [SerializeField] private Button _battlePassTab;
        [SerializeField] private Button _profileTab;
        
        [Header("Footer Elements")]
        [SerializeField] private Button _collectionNavButton;
        [SerializeField] private Button _shopNavButton;
        [SerializeField] private Button _profileNavButton;
        
        [Header("Content Views")]
        [SerializeField] private CollectionView _collectionView;
        [SerializeField] private ShopView _shopView;
        [SerializeField] private ActivitiesView _activitiesView;
        //[SerializeField] private FiveOnFiveView _fiveOnFiveView;
        [SerializeField] private BattlePassView _battlePassView;
        [SerializeField] private ProfileView _profileView;
        
        [Header("Popup Views")]
        [SerializeField] private GameObject _cardViewerPopup;
        [SerializeField] private GameObject _craftConfirmPopup;
        [SerializeField] private GameObject _tradeConfirmPopup;
        [SerializeField] private GameObject _purchaseConfirmPopup;
        
        private GameManager _gameManager;
        
        public void Initialize(GameManager gameManager)
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            _gameManager = gameManager;
            
            SetupNavigation();
            HideAllContentViews();
            ShowCollection();
            
            Debug.Log("UIManager: Initialized");
        }
        
        private void SetupNavigation()
        {
            // Header tabs
            _collectionTab.onClick.AddListener(ShowCollection);
            _shopTab.onClick.AddListener(ShowShop);
            _activitiesTab.onClick.AddListener(ShowActivities);
            _fiveOnFiveTab.onClick.AddListener(ShowFiveOnFive);
            _battlePassTab.onClick.AddListener(ShowBattlePass);
            _profileTab.onClick.AddListener(ShowProfile);
            
            // Footer navigation
            _collectionNavButton.onClick.AddListener(ShowCollection);
            _shopNavButton.onClick.AddListener(ShowShop);
            _profileNavButton.onClick.AddListener(ShowProfile);
        }
        
        private void HideAllContentViews()
        {
            _collectionView.Hide();
            _shopView.Hide();
            _activitiesView.Hide();
            //_fiveOnFiveView.Hide();
            _battlePassView.Hide();
            _profileView.Hide();
        }
        
        public void ShowCollection()
        {
            HideAllContentViews();
            _collectionView.Show();
            UpdateHeaderTabs(_collectionTab);
        }

        public void ShowMainMenu()
        {
            // Показываем основной экран (Пока что это коллекция, но потом мб будет отдельный экран? Для этого сделано короче)
            ShowCollection();
        }
        
        public void ShowShop()
        {
            HideAllContentViews();
            _shopView.Show();
            UpdateHeaderTabs(_shopTab);
        }
        
        public void ShowActivities()
        {
            HideAllContentViews();
            _activitiesView.Show();
            UpdateHeaderTabs(_activitiesTab);
        }
        
        public void ShowFiveOnFive()
        {
            HideAllContentViews();
            //_fiveOnFiveView.Show();
            UpdateHeaderTabs(_fiveOnFiveTab);
        }
        
        public void ShowBattlePass()
        {
            HideAllContentViews();
            _battlePassView.Show();
            UpdateHeaderTabs(_battlePassTab);
        }
        
        public void ShowProfile()
        {
            HideAllContentViews();
            _profileView.Show();
            UpdateHeaderTabs(_profileTab);
        }
        
        private void UpdateHeaderTabs(Button activeTab)
        {
            // Сбросить все табы
            _collectionTab.interactable = true;
            _shopTab.interactable = true;
            _activitiesTab.interactable = true;
            _fiveOnFiveTab.interactable = true;
            _battlePassTab.interactable = true;
            _profileTab.interactable = true;
            
            // Деактивировать текущий таб
            activeTab.interactable = false;
        }
        
        public void UpdateUserData(UserData userData)
        {
            _goldText.text = userData.gold.ToString();
            _diamondText.text = userData.diamonds.ToString();
            _ticketsText.text = userData.tickets.ToString();
        }
        
        public void ShowCardViewer()
        {
            _cardViewerPopup.SetActive(true);
        }
        
        public void HideCardViewer()
        {
            _cardViewerPopup.SetActive(false);
        }
        
        public void ShowCraftConfirm()
        {
            _craftConfirmPopup.SetActive(true);
        }
        
        public void HideCraftConfirm()
        {
            _craftConfirmPopup.SetActive(false);
        }
        
        public void ShowTradeConfirm()
        {
            _tradeConfirmPopup.SetActive(true);
        }
        
        public void HideTradeConfirm()
        {
            _tradeConfirmPopup.SetActive(false);
        }
        
        public void ShowPurchaseConfirm()
        {
            _purchaseConfirmPopup.SetActive(true);
        }
        
        public void HidePurchaseConfirm()
        {
            _purchaseConfirmPopup.SetActive(false);
        }
    }
}