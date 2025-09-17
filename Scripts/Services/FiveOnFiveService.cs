using BasketballCards.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BasketballCards.Services
{
    public class FiveOnFiveService
    {
        private readonly ApiClient _apiClient;
        
        public FiveOnFiveService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }
    }
}