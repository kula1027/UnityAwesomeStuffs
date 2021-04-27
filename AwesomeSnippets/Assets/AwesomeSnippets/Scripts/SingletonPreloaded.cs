using UnityEngine;

namespace AwesomeSnippets {
    /// <summary>
    ///     must be existed in scene and unique
    /// </summary>
    public abstract class SingletonPreloaded<T> : MonoBehaviour where T : Component {
        private static T instance;
        public static T Instance => instance;

        protected virtual void Awake() {
            if (instance == null) {
                instance = GetComponent<T>();
            } else {
                Debug.LogWarning("instance is already set: " + nameof(instance));
            }
        }
    }
}