using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Views;
using System.Collections.Generic;
using UnityEngine;

namespace BasketballCards.UI.Presenters
{
    public class CollectionPresenter : MonoBehaviour
    {
        [Header("View References")]
        [SerializeField] private CollectionView _collectionView;
        
        [Header("Service References")]
        private GameManager _gameManager;
        private List<CardData> _userCards;
        private List<CardData> _selectedCards = new List<CardData>();
        
        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            _collectionView.Initialize(this);
            
            LoadUserCards();
        }
        
        private void LoadUserCards()
        {
            _gameManager.CardService.GetUserCards(
                cards => {
                    _userCards = cards;
                    _collectionView.DisplayCards(cards);
                },
                error => {
                    Debug.LogError("Failed to load user cards: " + error);
                }
            );
        }
        
        public void OnCardSelected(CardData card)
        {
            _collectionView.ShowCardDetails(card);
        }
        
        public void OnCardToggleSelected(CardData card, bool isSelected)
        {
            if (isSelected && !_selectedCards.Contains(card))
            {
                _selectedCards.Add(card);
            }
            else if (!isSelected && _selectedCards.Contains(card))
            {
                _selectedCards.Remove(card);
            }
            
            _collectionView.UpdateSelectionCount(_selectedCards.Count);
        }
        
        public void OnCraftRequested()
        {
            if (_selectedCards.Count < 10)
            {
                _collectionView.ShowError("Нужно 10 карточек для крафта");
                return;
            }
            
            var cardIds = _selectedCards.ConvertAll(c => c.CardId);
            _gameManager.CraftService.CraftCards(cardIds,
                craftedCard => {
                    _userCards.Add(craftedCard);
                    _collectionView.OnCardCrafted(craftedCard);
                    _selectedCards.Clear();
                    _collectionView.UpdateSelectionCount(0);
                    _collectionView.ClearSelection();
                    
                    _gameManager.UserService.GetUserData(_gameManager.CurrentUser.username, 
                        userData => _gameManager.SetCurrentUser(userData), null);
                },
                error => {
                    _collectionView.ShowError(error);
                });
        }
        
        public void OnDisassembleRequested()
        {
            if (_selectedCards.Count == 0)
            {
                _collectionView.ShowError("Выберите карточки для разбора");
                return;
            }
            
            var cardIds = _selectedCards.ConvertAll(c => c.CardId);
            _gameManager.CraftService.DisassembleCards(cardIds,
                result => {
                    // Обновляем коллекцию, удаляя разобранные карточки
                    _userCards.RemoveAll(c => cardIds.Contains(c.CardId));
                    _collectionView.DisplayCards(_userCards);
                    _selectedCards.Clear();
                    _collectionView.UpdateSelectionCount(0);
                    _collectionView.ClearSelection();
                    
                    _gameManager.UserService.GetUserData(_gameManager.CurrentUser.username, 
                        userData => _gameManager.SetCurrentUser(userData), null);
                },
                error => {
                    _collectionView.ShowError(error);
                });
        }
        
        public void Show()
        {
            _collectionView.Show();
        }
        
        public void Hide()
        {
            _collectionView.Hide();
        }
    }
}