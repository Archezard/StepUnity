using BasketballCards.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BasketballCards.Services
{
    public class ShopService
    {
        private readonly ApiClient _apiClient;
        
        public ShopService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        public void GetShopItems(ShopCategory category, Action<List<ShopItem>> onSuccess, Action<string> onError = null)
        {
            // Заглушка для демонстрации
            Debug.Log($"Getting shop items for category: {category}");
            
            // После тут будет запрос к API
            var items = new List<ShopItem>();
            
            switch (category)
            {
                case ShopCategory.Cards:
                    items.Add(new ShopItem { Id = "card1", Name = "Бронзовая карта", Price = 100, Currency = Currency.Gold });
                    items.Add(new ShopItem { Id = "card2", Name = "Серебряная карта", Price = 500, Currency = Currency.Gold });
                    break;
                case ShopCategory.Throws:
                    items.Add(new ShopItem { Id = "throw1", Name = "1 бросок", Price = 50, Currency = Currency.Gold });
                    items.Add(new ShopItem { Id = "throw5", Name = "5 бросков", Price = 200, Currency = Currency.Gold });
                    break;
                case ShopCategory.Packs:
                    items.Add(new ShopItem { Id = "pack_bronze", Name = "Бронзовый пак", Price = 300, Currency = Currency.Gold });
                    items.Add(new ShopItem { Id = "pack_silver", Name = "Серебряный пак", Price = 1000, Currency = Currency.Gold });
                    break;
                case ShopCategory.Currency:
                    items.Add(new ShopItem { Id = "gold1000", Name = "1000 золота", Price = 100, Currency = Currency.Diamonds });
                    items.Add(new ShopItem { Id = "gold5000", Name = "5000 золота", Price = 450, Currency = Currency.Diamonds });
                    break;
            }
            
            onSuccess?.Invoke(items);
        }
        
        public void PurchaseItem(string itemId, Action<PurchaseResult> onSuccess, Action<string> onError = null)
        {
            // Заглушка для демонстрации
            Debug.Log($"Purchasing item: {itemId}");
            
            // После тут будет запрос к API
            var result = new PurchaseResult
            {
                Success = true,
                Message = "Purchase successful",
                Items = new List<CardData>() // Проверить, как будто не так как надо работает
            };
            
            onSuccess?.Invoke(result);
        }
    }
    
    public enum ShopCategory
    {
        Cards,
        Throws,
        Packs,
        Currency
    }
    
    public enum Currency
    {
        Gold,
        Diamonds
    }
    
    public class ShopItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public Currency Currency { get; set; }
        public string Description { get; set; }
    }
    
    public class PurchaseResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<CardData> Items { get; set; }
    }
}