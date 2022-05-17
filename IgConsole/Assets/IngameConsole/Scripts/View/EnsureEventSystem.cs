using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace IngameConsole {
    public class EnsureEventSystem : MonoBehaviour {
        private void Start() {
            if (FindObjectOfType<EventSystem>() == null) {
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }

            SceneManager.sceneLoaded += (_, __) => {
                if (FindObjectOfType<EventSystem>() == null) {
                    new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                }
            };
        }
    }
}