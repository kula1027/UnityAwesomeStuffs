using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AwesomeSnippets {

    public class SingletonCaller : MonoBehaviour {

        private void Start() {
            Debug.Log(SomePreloadedSingleton.Instance.gameObject.name);
            Debug.Log(SomeSingleton.Instance.gameObject.name);
            Debug.Log(SomeThreadSafeSingleton.Instance.gameObject.name);
        }
    }
}