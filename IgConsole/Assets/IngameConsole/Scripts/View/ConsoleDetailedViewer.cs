using TMPro;
using UnityEngine;

namespace IngameConsole {

    public class ConsoleDetailedViewer : MonoBehaviour {
        public const string TIME_FORMAT = "[HH:mm:ss]";

        [SerializeField] private TextMeshProUGUI text;

        public void Show(string str) {
            text.text = str;

            gameObject.SetActive(true);
        }
    }
}