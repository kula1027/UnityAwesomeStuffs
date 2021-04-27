using UnityEngine;

namespace AwesomeSnippets {
    public class SingletonUnityNullCheck<T> : MonoBehaviour where T : Component {
        private static T instance;

        public static T Instance {
            get {
                if (instance == null) {
                    instance = FindObjectOfType<T>();
                    if (instance == null) {
                        GameObject go = new GameObject(typeof(T).Name);
                        instance = go.AddComponent<T>();
                    }
                }

                return instance;
            }
        }
    }
}