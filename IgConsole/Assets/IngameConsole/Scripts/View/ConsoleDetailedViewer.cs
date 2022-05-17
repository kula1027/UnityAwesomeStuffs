using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IngameConsole {

    public class ConsoleDetailedViewer : MonoBehaviour {
        private const string TIME_FORMAT = "[HH:mm:ss]";

        [SerializeField] private TextMeshProUGUI text;

        [SerializeField] private Image dtImage;

        internal void Show(ConsoleData consoleData) {
            if (consoleData.HasSprite) {
                dtImage.gameObject.SetActive(true);

                dtImage.type = Image.Type.Simple;
                dtImage.preserveAspect = true;
                dtImage.sprite = consoleData.Sprite;
            } else {
                dtImage.gameObject.SetActive(false);
            }

            string strTime = IgConsole.LogStartTime.AddSeconds(consoleData.RealtimeSinceStartup).ToString(TIME_FORMAT);
            text.text =
               consoleData.Msg + "\n\n" +
               consoleData.Detailed + "\n" +
               strTime;

            gameObject.SetActive(true);
        }
    }
}