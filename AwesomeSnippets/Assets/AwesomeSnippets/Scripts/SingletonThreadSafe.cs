using UnityEngine;

namespace AwesomeSnippets {
    public abstract class SingletonThreadSafe<T> : MonoBehaviour where T : Component {
        private static T instance;
        private static readonly object lockObject = new Object();

        public static T Instance {
            get {
                if (ReferenceEquals(instance, null)) {
                    lock (lockObject) {
                        if (ReferenceEquals(instance, null)) {
                            instance = FindObjectOfType<T>();
                            if (ReferenceEquals(instance, null)) {
                                GameObject go = new GameObject(typeof(T).Name);
                                instance = go.AddComponent<T>();
                            }
                        }
                    }
                }

                return instance;
            }
        }
    }
}