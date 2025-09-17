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
        public void GetUserAlbums(Action<List<AlbumData>> onSuccess, Action<string> onError = null)
        {
            Debug.Log("AlbumService: Getting user albums (stub)");
            
            var albums = new List<AlbumData>
            {
                new AlbumData { Id = "progress", Name = "Прогресс", Type = AlbumType.Progress },
                new AlbumData { Id = "archive", Name = "Архив", Type = AlbumType.Archive },
                new AlbumData { Id = "custom1", Name = "Мой альбом", Type = AlbumType.Custom }
            };
            
            onSuccess?.Invoke(albums);
        }
    }

    public class AlbumData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public AlbumType Type { get; set; }
    }

    public enum AlbumType
    {
        Progress,
        Archive,
        Custom
    }
}