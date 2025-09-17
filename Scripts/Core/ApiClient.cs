using System.Text;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using BasketballCards.Models;

public class ApiClient : MonoBehaviour
{
    private string apiUrl = "https://step-app.ru/api.php";
    private string authToken = ""; // Токен будет устанавливаться после авторизации

    private JsonSerializerSettings JsonSettings = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
        DefaultValueHandling = DefaultValueHandling.Ignore
    };

    public void SetAuthToken(string token)
    {
        authToken = token;
    }

    public IEnumerator PostRequest<T>(string endpoint, object data, System.Action<T> onSuccess, System.Action<string> onError = null)
    {
        string jsonRequest = JsonConvert.SerializeObject(data, JsonSettings);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequest);

        using (UnityWebRequest webRequest = new UnityWebRequest($"{apiUrl}/{endpoint}", "POST"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            
            if (!string.IsNullOrEmpty(authToken))
            {
                webRequest.SetRequestHeader("Authorization", $"Bearer {authToken}");
            }

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                string error = $"Network error: {webRequest.error}";
                Debug.LogError(error);
                onError?.Invoke(error);
                yield break;
            }

            string jsonResponse = webRequest.downloadHandler.text;
            Debug.Log($"API Response: {jsonResponse}");

            try
            {
                T response = JsonConvert.DeserializeObject<T>(jsonResponse);
                onSuccess?.Invoke(response);
            }
            catch (System.Exception ex)
            {
                string error = $"JSON parse error: {ex.Message}";
                Debug.LogError(error);
                onError?.Invoke(error);
            }
        }
    }

    // Специализированные методы для различных операций
    public void CraftCards(List<string> cardIds, System.Action<CraftResponse> onSuccess, System.Action<string> onError = null)
    {
        var request = new CraftRequest { CardIds = cardIds };
        StartCoroutine(PostRequest<CraftResponse>("craft", request, onSuccess, onError));
    }

    public void UpgradeCard(string cardId, System.Action<UpgradeResponse> onSuccess, System.Action<string> onError = null)
    {
        var request = new UpgradeRequest { CardId = cardId };
        StartCoroutine(PostRequest<UpgradeResponse>("upgrade", request, onSuccess, onError));
    }
}

// Модели для запросов и ответов API
public class CraftRequest
{
    public List<string> CardIds { get; set; }
}

public class CraftResponse
{
    public bool Success { get; set; }
    public CardData Card { get; set; }
    public string Error { get; set; }
}

public class UpgradeRequest
{
    public string CardId { get; set; }
}

public class UpgradeResponse
{
    public bool Success { get; set; }
    public CardData Card { get; set; }
    public string Error { get; set; }
}