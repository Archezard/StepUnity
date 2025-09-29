using BasketballCards.Models;
using BasketballCards.Services;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BasketballCards.Core
{
    public static class EventSystem
    {
        // Навигационные события
        public static event Action<AppScreen> OnNavigationRequested;
        public static event Action OnBackNavigationRequested;

        public static event Action<UserData> OnUserDataUpdated;
        
        // События карточек
        public static event Action<CardData> OnCardViewRequested;
        public static event Action<CardData> OnCardUpgraded;
        public static event Action<List<CardData>> OnCardsCrafted;
        public static event Action<List<CardData>> OnCardsDisassembled;
        public static event Action<CardData> OnCardReceived;

        // События магазина
        public static event Action<ShopItem> OnShopItemPurchased;
        public static event Action<ShopCategory> OnShopCategoryChanged;
        
        // События активностей
        public static event Action OnFreeCardRequested;
        public static event Action OnBallThrowRequested;
        public static event Action<string> OnPackOpenRequested;

        // События внутренней навигации
        public static event Action<CollectionSubScreen> OnCollectionSubScreenChanged;
        public static event Action<ActivitiesSubScreen> OnActivitiesSubScreenChanged;
        public static event Action<ShopSubScreen> OnShopSubScreenChanged;
        public static event Action<BattlePassSubScreen> OnBattlePassSubScreenChanged;
        
        // События баттл-пасса
        public static event Action<int, bool> OnBattlePassRewardClaimed;
        public static event Action OnBattlePassPremiumPurchased;
        
        // Системные события
        public static event Action<string> OnErrorOccurred;
        public static event Action<string> OnSuccessMessage;
        
        // Методы для вызова событий
        public static void NavigateTo(AppScreen screen) => OnNavigationRequested?.Invoke(screen);
        public static void NavigateBack() => OnBackNavigationRequested?.Invoke();

        public static void UpdateUserData(UserData userData) => OnUserDataUpdated?.Invoke(userData);
        
        public static void RequestCardView(CardData card) => OnCardViewRequested?.Invoke(card);
        public static void UpgradeCard(CardData card) => OnCardUpgraded?.Invoke(card);
        public static void CraftCards(List<CardData> cards) => OnCardsCrafted?.Invoke(cards);
        public static void DisassembleCards(List<CardData> cards) => OnCardsDisassembled?.Invoke(cards);
        public static void ReceiveCard(CardData card) => OnCardReceived?.Invoke(card);
        
        public static void PurchaseShopItem(ShopItem item) => OnShopItemPurchased?.Invoke(item);
        public static void ChangeShopCategory(ShopCategory category) => OnShopCategoryChanged?.Invoke(category);
        
        public static void RequestFreeCard() => OnFreeCardRequested?.Invoke();
        public static void RequestBallThrow() => OnBallThrowRequested?.Invoke();
        public static void RequestPackOpen(string packId) => OnPackOpenRequested?.Invoke(packId);

        // Методы для внутренней навигации
        public static void ChangeCollectionSubScreen(CollectionSubScreen subScreen) => OnCollectionSubScreenChanged?.Invoke(subScreen);
        public static void ChangeActivitiesSubScreen(ActivitiesSubScreen subScreen) => OnActivitiesSubScreenChanged?.Invoke(subScreen);
        public static void ChangeShopSubScreen(ShopSubScreen subScreen) => OnShopSubScreenChanged?.Invoke(subScreen);
        public static void ChangeBattlePassSubScreen(BattlePassSubScreen subScreen) => OnBattlePassSubScreenChanged?.Invoke(subScreen);
        
        // Методы для баттл-пасса
        public static void ClaimBattlePassReward(int level, bool isPremium) => OnBattlePassRewardClaimed?.Invoke(level, isPremium);
        public static void PurchaseBattlePassPremium() => OnBattlePassPremiumPurchased?.Invoke();
        
        public static void ShowError(string message) => OnErrorOccurred?.Invoke(message);
        public static void ShowSuccess(string message) => OnSuccessMessage?.Invoke(message);
    }
    
    public enum AppScreen
    {
        Collection,
        Shop,
        Activities,
        FiveOnFive,
        BattlePass,
        Profile
    }

    public enum CollectionSubScreen
    {
        Collection,
        Workshop,
        Album,
        Exchange
    }

    public enum ActivitiesSubScreen
    {
        GetCard,
        ThrowBall,
        OpenPack
    }

    public enum ShopSubScreen
    {
        Cards,
        Throws,
        Packs,
        Currency
    }

    public enum BattlePassSubScreen
    {
        Tasks,
        Rewards
    }
}