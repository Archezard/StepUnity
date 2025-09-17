using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.UI.Views
{
    public class TaskElement : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _taskNameText;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private TextMeshProUGUI _experienceText;
        [SerializeField] private Button _claimButton;
        [SerializeField] private Slider _progressSlider;
        
        public void Initialize(BattlePassTask task)
        {
            _taskNameText.text = task.Name;
            _progressText.text = $"{task.Progress}/{task.Target}";
            _experienceText.text = $"+{task.Experience} опыта";
            _progressSlider.value = (float)task.Progress / task.Target;
            
            _claimButton.interactable = task.IsCompleted;
            _claimButton.onClick.AddListener(OnClaimButtonClicked);
        }
        
        private void OnClaimButtonClicked()
        {
            // Запрос на получение награды за задание
            Debug.Log("Claiming task reward");
        }
    }
}