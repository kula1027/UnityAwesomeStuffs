using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AwesomeSnippets {

    public class ThreadSafeSingletonUnityNullCheck<T> : MonoBehaviour where T : Component {
        private static T instance;
        private static object lockObject = new Object();

        public static T Instance {
            get {
                if (instance == null) {
                    lock (lockObject) {
                        if (instance == null) {
                            instance = FindObjectOfType<T>();
                            if (instance == null) {
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