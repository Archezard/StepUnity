using BasketballCards.Models;
using BasketballCards.UI.Presenters;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class AlbumView : BaseView, IAlbumView
    {
        [Header("UI References")]
        [SerializeField] private Button _progressAlbumButton;
        [SerializeField] private Button _archiveAlbumButton;
        [SerializeField] private Button _myAlbumsButton;
        [SerializeField] private Transform _albumsContainer;
        [SerializeField] private GameObject _albumItemPrefab;
        [SerializeField] private TextMeshProUGUI _pageInfoText;
        [SerializeField] private Button _prevPageButton;
        [SerializeField] private Button _nextPageButton;
        [SerializeField] private Button _buyPageButton;

        private CollectionPresenter _presenter;
        private List<AlbumData> _currentAlbums;
        private AlbumType _currentAlbumType = AlbumType.Progress;
        private int _currentPage = 1;

        public void Initialize(CollectionPresenter presenter)
        {
            _presenter = presenter;
            
            _progressAlbumButton.onClick.AddListener(() => ShowAlbumType(AlbumType.Progress));
            _archiveAlbumButton.onClick.AddListener(() => ShowAlbumType(AlbumType.Archive));
            _myAlbumsButton.onClick.AddListener(() => ShowAlbumType(AlbumType.Custom));
            _prevPageButton.onClick.AddListener(OnPrevPage);
            _nextPageButton.onClick.AddListener(OnNextPage);
            _buyPageButton.onClick.AddListener(OnBuyPage);
            
            // Загружаем начальные данные
            LoadAlbums();
        }
        
        public void DisplayAlbums(List<AlbumData> albums)
        {
            _currentAlbums = albums;
            UpdateAlbumsDisplay();
        }
        
        public void ShowAlbumDetails(AlbumData album)
        {
            // Заглушка для отображения деталей альбома
            Debug.Log($"Showing album details: {album.Name}");
        }
        
        public void DisplayCards(List<CardData> cards)
        {
            // В альбоме отображаются не карточки напрямую, а альбомы
            // Этот метод может не использоваться в AlbumView
        }
        
        private void ShowAlbumType(AlbumType albumType)
        {
            _currentAlbumType = albumType;
            _currentPage = 1;
            LoadAlbums();
        }
        
        private void LoadAlbums()
        {
            // Заглушка - в реальности будет загрузка через сервис
            var albums = new List<AlbumData>
            {
                new AlbumData 
                { 
                    Id = _currentAlbumType.ToString().ToLower(), 
                    Name = GetAlbumTypeName(_currentAlbumType),
                    Type = _currentAlbumType,
                    Pages = new List<AlbumPage>(),
                    UnlockedPages = 1
                }
            };
            
            DisplayAlbums(albums);
        }
        
        private string GetAlbumTypeName(AlbumType albumType)
        {
            switch (albumType)
            {
                case AlbumType.Progress: return "Прогресс";
                case AlbumType.Archive: return "Архив";
                case AlbumType.Custom: return "Мои альбомы";
                default: return "Альбом";
            }
        }
        
        private void UpdateAlbumsDisplay()
        {
            // Очищаем контейнер
            foreach (Transform child in _albumsContainer)
            {
                Destroy(child.gameObject);
            }
            
            // Создаем элементы альбомов
            foreach (var album in _currentAlbums)
            {
                var albumItem = Instantiate(_albumItemPrefab, _albumsContainer);
                var albumButton = albumItem.GetComponent<Button>();
                var albumText = albumItem.GetComponentInChildren<TextMeshProUGUI>();
                
                if (albumText != null)
                {
                    albumText.text = album.Name;
                }
                
                if (albumButton != null)
                {
                    albumButton.onClick.AddListener(() => ShowAlbumDetails(album));
                }
            }
            
            UpdatePageInfo();
        }
        
        private void UpdatePageInfo()
        {
            _pageInfoText.text = $"Страница: {_currentPage}";
            _prevPageButton.interactable = _currentPage > 1;
            // _nextPageButton.interactable = ... // Зависит от общего количества страниц
        }
        
        private void OnPrevPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                UpdatePageInfo();
                // Загружаем данные для предыдущей страницы
            }
        }
        
        private void OnNextPage()
        {
            _currentPage++;
            UpdatePageInfo();
            // Загружаем данные для следующей страницы
        }
        
        private void OnBuyPage()
        {
            // Заглушка для покупки страницы
            Debug.Log("Buy page clicked");
        }
        
        public override void Show()
        {
            base.Show();
            LoadAlbums();
        }
    }
}