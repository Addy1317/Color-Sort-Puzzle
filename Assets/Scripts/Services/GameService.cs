using SlowpokeStudio.Audio;
using SlowpokeStudio.Event;
using SlowpokeStudio.Generic;
using SlowpokeStudio.UI;
using System.Collections.Generic;
using UnityEngine;

namespace SlowpokeStudio.Services
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        [Header("Service")]
        [SerializeField] internal AudioManager audioManager;
        [SerializeField] internal EventManager eventManager;
        [SerializeField] internal UIManager uiManager;

        protected override void Awake()
        {
            base.Awake();
            if (Instance == this)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            InitializeServicesCheck();
        }

        private void InitializeServicesCheck()
        {
            var services = new Dictionary<string, Object>
            {
            { "AudioManager", audioManager },
            { "EventManager", eventManager },
            { "UIManager", uiManager },
            };

            foreach (var service in services)
            {
                if (service.Value == null)
                {
                    Debug.LogError($"{service.Key} failed to initialize.");
                }
                else
                {
                    Debug.Log($"{service.Key} is initialized.");
                }
            }
        }
    }
}

