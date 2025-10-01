using BasketballCards.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BasketballCards.Services
{
    public class AlbumService
    {
        private readonly ApiClient _apiClient;

        public AlbumService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        // ЗАГЛУШКА: Методы для работы с альбомами
        public void GetUserAlbums(Action<List<AlbumInfo>> onSuccess, Action<string> onError = null)
        {
            Debug.Log("AlbumService: Getting user albums (stub)");
            
            var albums = new List<AlbumInfo>
            {
                new AlbumInfo { Id = "progress", Name = "Прогресс", Type = AlbumType.Progress },
                new AlbumInfo { Id = "archive", Name = "Архив", Type = AlbumType.Archive },
                new AlbumInfo { Id = "custom1", Name = "Мой альбом", Type = AlbumType.Custom }
            };
            
            onSuccess?.Invoke(albums);
        }
    }

    public class AlbumInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public AlbumType Type { get; set; }
    }
}