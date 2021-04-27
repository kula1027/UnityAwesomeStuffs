using TMPro;
using UnityEngine;

namespace AwesomeSnippets {
    [RequireComponent(typeof(TextMeshPro))]
    public class TextFloat : MonoBehaviour {
        [SerializeField] private SomeModelData someModelData;

        private TextMeshPro tmpText;

        private void Awake() {
            tmpText = GetComponent<TextMeshPro>();
        }

        private void OnEnable() {
            someModelData.FloatData.OnChange += OnFloatChange;
        }

        private void OnDisable() {
            someModelData.FloatData.OnChange -= OnFloatChange;
        }

        private void OnFloatChange(float f) {
            tmpText.text = f.ToString("F2");
        }
    }
}