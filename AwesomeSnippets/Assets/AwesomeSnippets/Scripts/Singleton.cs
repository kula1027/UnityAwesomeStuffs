using UnityEngine;

namespace AwesomeSnippets {
    public abstract class Singleton<T> : MonoBehaviour where T : Component {
        private static T instance;

        public static T Instance {
            get {
                if (ReferenceEquals(instance, null)) {
                    instance = FindObjectOfType<T>();
                    if (ReferenceEquals(instance, null)) {
                        GameObject go = new GameObject(typeof(T).Name);
                        instance = go.AddComponent<T>();
                    }
                }

                return instance;
            }
        }
    }
}