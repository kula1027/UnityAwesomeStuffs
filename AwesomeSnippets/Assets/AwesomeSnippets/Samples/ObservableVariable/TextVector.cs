using TMPro;
using UnityEngine;

namespace AwesomeSnippets {
    [RequireComponent(typeof(TextMeshPro))]
    public class TextVector : MonoBehaviour {
        [SerializeField] private SomeModelData someModelData;

        private TextMeshPro tmpText;

        private void Awake() {
            tmpText = GetComponent<TextMeshPro>();
        }

        private void OnEnable() {
            someModelData.VectorData.OnChange += OnVectorChange;
        }

        private void OnDisable() {
            someModelData.VectorData.OnChange -= OnVectorChange;
        }

        private void OnVectorChange(Vector2 v) {
            tmpText.text = v.ToString();
        }
    }
}