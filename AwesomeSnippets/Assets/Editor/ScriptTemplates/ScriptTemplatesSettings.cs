using System.Collections.Generic;
using UnityEngine;

namespace AwesomeSnippets {
    public class ScriptTemplatesSettings : ScriptableObject {
        [SerializeField] private List<TextAsset> templates = new List<TextAsset> {null};

        public List<TextAsset> Templates {
            get => templates;
            set => templates = value;
        }
    }
}