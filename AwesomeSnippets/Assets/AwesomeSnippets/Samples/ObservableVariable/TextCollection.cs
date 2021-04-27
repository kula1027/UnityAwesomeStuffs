using System.Collections.Specialized;
using TMPro;
using UnityEngine;

namespace AwesomeSnippets {
    [RequireComponent(typeof(TextMeshPro))]
    public class TextCollection : MonoBehaviour {
        [SerializeField] private SomeModelData someModelData;

        private TextMeshPro tmpText;

        private void Awake() {
            tmpText = GetComponent<TextMeshPro>();
        }

        private void OnEnable() {
            someModelData.StringCollection.CollectionChanged += OnChange;
        }

        private void OnDisable() {
            someModelData.StringCollection.CollectionChanged -= OnChange;
        }

        private void OnChange(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems.Count > 0) {
                        tmpText.text += $"{e.NewItems[0]}.";
                    }

                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Remove:
                    break;

                case NotifyCollectionChangedAction.Replace:
                    break;

                case NotifyCollectionChangedAction.Reset:
                    break;
            }
        }
    }
}