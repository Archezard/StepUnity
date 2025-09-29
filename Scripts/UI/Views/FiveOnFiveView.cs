using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class FiveOnFiveView : BaseView
    {
        [Header("UI References")]
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _teamSetupButton;
        [SerializeField] private Button _tacticsButton;
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private TextMeshProUGUI _teamRatingText;
        [SerializeField] private TextMeshProUGUI _matchesPlayedText;
        
        public System.Action OnStartGame;
        public System.Action OnTeamSetup;
        public System.Action OnTactics;
        public System.Action OnLeaderboard;
        
        public void Initialize()
        {
            _startGameButton.onClick.AddListener(() => OnStartGame?.Invoke());
            _teamSetupButton.onClick.AddListener(() => OnTeamSetup?.Invoke());
            _tacticsButton.onClick.AddListener(() => OnTactics?.Invoke());
            _leaderboardButton.onClick.AddListener(() => OnLeaderboard?.Invoke());
            
            // Заглушки для данных
            _teamRatingText.text = "Рейтинг команды: 1500";
            _matchesPlayedText.text = "Сыграно матчей: 25";
        }
        
        public void UpdateTeamInfo(int rating, int matchesPlayed)
        {
            _teamRatingText.text = $"Рейтинг команды: {rating}";
            _matchesPlayedText.text = $"Сыграно матчей: {matchesPlayed}";
        }
    }
}