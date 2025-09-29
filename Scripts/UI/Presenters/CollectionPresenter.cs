using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Views;
using System.Collections.Generic;
using UnityEngine;

namespace BasketballCards.UI.Presenters
{
    public class CollectionPresenter : BasePresenter
    {
        [Header("View References")]
        [SerializeField] private CollectionView _collectionView;
        
        private List<CardData> _userCards;
        private List<CardData> _selectedCards = new List<CardData>();
        
        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            
            EventSystem.OnCardViewRequested += HandleCardViewRequested;
            EventSystem.OnCardSelected += HandleCardSelected;
            EventSystem.OnCardUpgraded += HandleCardUpgraded;
            EventSystem.OnCardsCrafted += HandleCardsCrafted;
            EventSystem.OnCardsDisassembled += HandleCardsDisassembled;
            EventSystem.OnErrorOccurred += HandleError;
            
            // Подписка на события данных пользователя
            UserDataManager.Instance.OnUserDataUpdated += HandleUserDataUpdated;
            UserDataManager.Instance.OnCurrencyChanged += HandleCurrencyChanged;
        }
        
        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            
            EventSystem.OnCardViewRequested -= HandleCardViewRequested;
            EventSystem.OnCardSelected -= HandleCardSelected;
            EventSystem.OnCardUpgraded -= HandleCardUpgraded;
            EventSystem.OnCardsCrafted -= HandleCardsCrafted;
            EventSystem.OnCardsDisassembled -= HandleCardsDisassembled;
            EventSystem.OnErrorOccurred -= HandleError;
            
            if (UserDataManager.Instance != null)
            {
                UserDataManager.Instance.OnUserDataUpdated -= HandleUserDataUpdated;
                UserDataManager.Instance.OnCurrencyChanged -= HandleCurrencyChanged;
            }
        }
        
        private void Start()
        {
            // Инициализация View
            if (_collectionView != null)
            {
                _collectionView.Initialize(this);
            }
            
            // Загрузка карточек пользователя
            LoadUserCards();
        }

        private void HandleCardViewRequested(CardData card)
        {
            // Открываем 3D просмотр карточки
            if (_collectionView != null)
            {
                _collectionView.ShowCardDetails(card);
            }
        }
        
        public override void Show()
        {
            if (_collectionView != null)
            {
                _collectionView.Show();
            }
        }
        
        public override void Hide()
        {
            if (_collectionView != null)
            {
                _collectionView.Hide();
            }
        }
        
        protected override void HandleUserDataUpdated(UserData userData)
        {
            // При обновлении данных пользователя перезагружаем карточки
            LoadUserCards();
        }
        
        private void LoadUserCards()
        {
            AppCoordinator.Instance.CardService.GetUserCards(
                cards => {
                    _userCards = cards;
                    if (_collectionView != null)
                    {
                        _collectionView.DisplayCards(cards);
                    }
                },
                error => {
                    EventSystem.ShowError($"Failed to load user cards: {error}");
                });
        }
        
        private void HandleCardSelected(CardData card)
        {
            // Показ деталей карточки
            if (_collectionView != null)
            {
                _collectionView.ShowCardDetails(card);
            }
        }
        
        private void HandleCardUpgraded(CardData card)
        {
            // Обновление карточки после улучшения
            var index = _userCards.FindIndex(c => c.CardId == card.CardId);
            if (index >= 0)
            {
                _userCards[index] = card;
                if (_collectionView != null)
                {
                    _collectionView.OnCardUpgraded(card);
                }
            }
        }
        
        private void HandleCardsCrafted(List<CardData> cards)
        {
            // Добавление скрафченных карточек в коллекцию
            _userCards.AddRange(cards);
            if (_collectionView != null)
            {
                _collectionView.OnCardCrafted(cards[0]); // В текущей реализации крафт одной карты
            }
        }
        
        private void HandleCardsDisassembled(List<CardData> cards)
        {
            // Удаление разобранных карточек из коллекции
            foreach (var card in cards)
            {
                _userCards.RemoveAll(c => c.CardId == card.CardId);
            }
            if (_collectionView != null)
            {
                _collectionView.DisplayCards(_userCards);
            }
        }
        
        private void HandleCurrencyChanged(int oldGold, int newGold)
        {
            // Обновление UI при изменении валюты
        }
        
        private void HandleError(string error)
        {
            if (_collectionView != null)
            {
                _collectionView.ShowError(error);
            }
        }
        
        // Методы, вызываемые из View
        public void OnCardSelectedInView(CardData card)
        {
            EventSystem.SelectCard(card);
        }
        
        public void OnCardToggleSelectedInView(CardData card, bool isSelected)
        {
            if (isSelected && !_selectedCards.Contains(card))
            {
                _selectedCards.Add(card);
            }
            else if (!isSelected && _selectedCards.Contains(card))
            {
                _selectedCards.Remove(card);
            }
            
            if (_collectionView != null)
            {
                _collectionView.UpdateSelectionCount(_selectedCards.Count);
            }
        }
        
        public void OnCraftRequestedInView()
        {
            if (_selectedCards.Count < 10)
            {
                EventSystem.ShowError("Нужно 10 карточек для крафта");
                return;
            }
            
            var cardIds = _selectedCards.ConvertAll(c => c.CardId);
            AppCoordinator.Instance.CraftService.CraftCards(cardIds,
                craftedCard => {
                    // В текущей реализации крафт возвращает одну карту
                    EventSystem.CraftCards(new List<CardData> { craftedCard });
                    
                    // Обновляем данные пользователя
                    AppCoordinator.Instance.UserService.GetUserData(
                        UserDataManager.Instance.CurrentUser.username,
                        userData => UserDataManager.Instance.UpdateUserData(userData),
                        error => EventSystem.ShowError("Failed to update user data")
                    );
                    
                    // Сбрасываем выбранные карточки
                    _selectedCards.Clear();
                    if (_collectionView != null)
                    {
                        _collectionView.UpdateSelectionCount(0);
                        _collectionView.ClearSelection();
                    }
                },
                error => {
                    EventSystem.ShowError(error);
                });
        }
        
        public void OnDisassembleRequestedInView()
        {
            if (_selectedCards.Count == 0)
            {
                EventSystem.ShowError("Выберите карточки для разбора");
                return;
            }
            
            var cardIds = _selectedCards.ConvertAll(c => c.CardId);
            AppCoordinator.Instance.CraftService.DisassembleCards(cardIds,
                result => {
                    // Временная заглушка: мы не получаем список разобранных карт, поэтому используем выбранные
                    EventSystem.DisassembleCards(_selectedCards);
                    
                    // Обновляем данные пользователя
                    AppCoordinator.Instance.UserService.GetUserData(
                        UserDataManager.Instance.CurrentUser.username,
                        userData => UserDataManager.Instance.UpdateUserData(userData),
                        error => EventSystem.ShowError("Failed to update user data")
                    );
                    
                    // Сбрасываем выбранные карточки
                    _selectedCards.Clear();
                    if (_collectionView != null)
                    {
                        _collectionView.UpdateSelectionCount(0);
                        _collectionView.ClearSelection();
                    }
                },
                error => {
                    EventSystem.ShowError(error);
                });
        }
        
        public void OnUpgradeCardRequestedInView(CardData card)
        {
            AppCoordinator.Instance.CardService.UpgradeCard(card.CardId,
                upgradedCard => {
                    EventSystem.UpgradeCard(upgradedCard);
                    
                    // Обновляем данные пользователя
                    AppCoordinator.Instance.UserService.GetUserData(
                        UserDataManager.Instance.CurrentUser.username,
                        userData => UserDataManager.Instance.UpdateUserData(userData),
                        error => EventSystem.ShowError("Failed to update user data")
                    );
                },
                error => {
                    EventSystem.ShowError(error);
                });
        }
    }
}