using TMPro;
using UnityEngine;

namespace AwesomeSnippets {

    [RequireComponent(typeof(TextMeshPro))]
    public class TextInteger : MonoBehaviour {
        [SerializeField] private SomeModelData someModelData;

        private TextMeshPro tmpText;

        private void Awake() {
            tmpText = GetComponent<TextMeshPro>();
        }

        private void OnEnable() {
            someModelData.IntegerData.OnChange += OnIntegerChange;
        }

        private void OnDisable() {
            someModelData.IntegerData.OnChange -= OnIntegerChange;
        }

        private void OnIntegerChange(int i) {
            tmpText.text = i.ToString();
        }
    }
}