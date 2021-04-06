using UnityEngine;

namespace AwesomeSnippets {

    public class UnityExtenstionsSample : MonoBehaviour {

        private void Start() {
            Debug.Log($"FindDescendant(\"Lvl4\"): {transform.FindDescendant("Lvl4").name}");

            Debug.Log($"FindDescendantsAll(\"Lvl3\"): {transform.FindDescendantsAll("Lvl3").Count}");

            Debug.Log($"GetDescendantAll: {transform.GetDescendantAll().Count}");
        }
    }
}