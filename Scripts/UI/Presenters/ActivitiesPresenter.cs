using BasketballCards.Core;
using BasketballCards.Models;
using BasketballCards.Services;
using BasketballCards.UI.Views;
using System.Collections.Generic;
using UnityEngine;

namespace BasketballCards.UI.Presenters
{
    public class ActivitiesPresenter : MonoBehaviour
    {
        [Header("View References")]
        [SerializeField] private ActivitiesView _activitiesView;
        [SerializeField] private GetCardView _getCardView;
        [SerializeField] private ThrowBallView _throwBallView;
        [SerializeField] private OpenPackView _openPackView;
        
        private GameManager _gameManager;
        private ActivitiesService _activitiesService;
        
        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            _activitiesService = _gameManager.ActivitiesService;
            
            _activitiesView.Initialize(this);
            _getCardView.Initialize(this);
            _throwBallView.Initialize(this);
            _openPackView.Initialize(this);
            
            HideAllSubsections();
            _activitiesView.Show();
        }
        
        public void GetFreeCard()
        {
            _activitiesService.GetFreeCard(
                card => {
                    _getCardView.DisplayCard(card);
                    OnCardReceived(card);
                },
                error => {
                    _getCardView.ShowError(error);
                });
        }
        
        public void ThrowBall()
        {
            _activitiesService.ThrowBall(
                (score, rewards) => {
                    // Обработка результата броска
                    Debug.Log($"Бросок! Очков: {score}, Наград: {rewards}");
                },
                error => {
                    _throwBallView.ShowError(error);
                });
        }
        
        public void OpenPack(string packId)
        {
            _activitiesService.OpenPack(packId,
                cards => {
                    _openPackView.DisplayCards(cards);
                    OnPackOpened(cards);
                },
                error => {
                    _openPackView.ShowError(error);
                });
        }
        
        public void OnCardReceived(CardData card)
        {
            _gameManager.UserService.GetUserData(_gameManager.CurrentUser.username, 
                userData => {
                    _gameManager.SetCurrentUser(userData);
                },
                error => {
                    Debug.LogError("Failed to update user data: " + error);
                });
        }
        
        public void OnPackOpened(List<CardData> cards)
        {
            _gameManager.UserService.GetUserData(_gameManager.CurrentUser.username, 
                userData => {
                    _gameManager.SetCurrentUser(userData);
                },
                error => {
                    Debug.LogError("Failed to update user data: " + error);
                });
        }
        
        public void ShowActivities()
        {
            HideAllSubsections();
            _activitiesView.Show();
        }
        
        public void ShowGetCard()
        {
            HideAllSubsections();
            _getCardView.Show();
        }
        
        public void ShowThrowBall()
        {
            HideAllSubsections();
            _throwBallView.Show();
        }
        
        public void ShowOpenPack()
        {
            HideAllSubsections();
            _openPackView.Show();
        }
        
        private void HideAllSubsections()
        {
            _activitiesView.Hide();
            _getCardView.Hide();
            _throwBallView.Hide();
            _openPackView.Hide();
        }
    }
}