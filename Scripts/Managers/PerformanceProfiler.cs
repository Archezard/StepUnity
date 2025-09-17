using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketballCards.Managers
{
    public class PerformanceProfiler : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _fpsText;
        [SerializeField] private TextMeshProUGUI _memoryText;
        [SerializeField] private GameObject _profilerPanel;
        
        [Header("Settings")]
        [SerializeField] private bool _enableProfiler = false;
        [SerializeField] private float _updateInterval = 1.0f;
        
        private float _deltaTime = 0.0f;
        private float _nextUpdateTime = 0.0f;
        
        public void Initialize()
        {
            if (!_enableProfiler)
            {
                _profilerPanel.SetActive(false);
                return;
            }
            
            _profilerPanel.SetActive(true);
            Debug.Log("PerformanceProfiler: Initialized");
        }
        
        private void Update()
        {
            if (!_enableProfiler) return;
            
            // Calculate FPS
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
            
            // Update at intervals
            if (Time.unscaledTime >= _nextUpdateTime)
            {
                UpdateProfilerInfo();
                _nextUpdateTime = Time.unscaledTime + _updateInterval;
            }
        }
        
        private void UpdateProfilerInfo()
        {
            // Update FPS
            float fps = 1.0f / _deltaTime;
            _fpsText.text = $"FPS: {fps:0.}";
            
            // Update memory
            long memory = System.GC.GetTotalMemory(false) / 1024 / 1024;
            _memoryText.text = $"Memory: {memory} MB";
            
            // Add other performance metrics as needed
        }
        
        public void ToggleProfiler()
        {
            _enableProfiler = !_enableProfiler;
            _profilerPanel.SetActive(_enableProfiler);
        }
    }
}