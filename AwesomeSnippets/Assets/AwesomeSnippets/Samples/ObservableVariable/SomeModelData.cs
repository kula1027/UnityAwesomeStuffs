using System.Collections.ObjectModel;
using UnityEngine;

namespace AwesomeSnippets {
    public class SomeModelData : MonoBehaviour {
        public ObservableVariable<int> IntegerData { get; } = new ObservableVariable<int>();
        public ObservableVariable<float> FloatData { get; } = new ObservableVariable<float>();
        public ObservableVariable<Vector2> VectorData { get; } = new ObservableVariable<Vector2>();

        public ObservableCollection<string> StringCollection { get; } = new ObservableCollection<string>();
    }
}