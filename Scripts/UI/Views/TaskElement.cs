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
        [SerializeField] private Image _backgroundImage;
        
        private BattlePassTask _task;
        
        public void Initialize(BattlePassTask task, System.Action<BattlePassTask> onClaim)
        {
            _task = task;
            
            _taskNameText.text = task.Name;
            _progressText.text = $"{task.Progress}/{task.Target}";
            _experienceText.text = $"+{task.Experience} опыта";
            _progressSlider.value = (float)task.Progress / task.Target;
            
            // Разный цвет для ежедневных и ежемесячных заданий
            _backgroundImage.color = task.IsDaily ? 
                new Color(0.2f, 0.4f, 0.8f, 0.3f) : 
                new Color(0.8f, 0.6f, 0.2f, 0.3f);
            
            _claimButton.interactable = task.IsCompleted && !task.IsClaimed;
            _claimButton.onClick.AddListener(() => onClaim?.Invoke(task));
            
            UpdateClaimButton();
        }
        
        private void UpdateClaimButton()
        {
            if (_task.IsClaimed)
            {
                _claimButton.interactable = false;
                _claimButton.GetComponentInChildren<TextMeshProUGUI>().text = "Получено";
            }
            else if (_task.IsCompleted)
            {
                _claimButton.interactable = true;
                _claimButton.GetComponentInChildren<TextMeshProUGUI>().text = "Получить";
            }
            else
            {
                _claimButton.interactable = false;
                _claimButton.GetComponentInChildren<TextMeshProUGUI>().text = "Не выполнено";
            }
        }
    }
    
    [System.Serializable]
    public class BattlePassTask
    {
        public string Name;
        public int Experience;
        public int Progress;
        public int Target;
        public bool IsDaily;
        public bool IsCompleted => Progress >= Target;
        public bool IsClaimed = false;
    }
}