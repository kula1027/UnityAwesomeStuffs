using System.Linq;
using UnityEngine;

namespace IngameConsole {

    public class IgConsoleSample : MonoBehaviour {
        [SerializeField] private Sprite someRandomSprite;

        private string filePath = Application.streamingAssetsPath + "/iglogs.txt";

        private void Start() {
            Debug.Log("Hello Console");
            Debug.LogWarning("Warning!");
            Debug.LogError("Error!");
            Debug.LogException(new System.Exception("Exception!"));
            Debug.Assert(false, "Assert Failed!");

            ///Log sprite
            IgConsole.Log(someRandomSprite, "Log sprite sample, click for detail");

            ///Logging with detailed strings
            IgConsole.Log(
                "click here to see details",
                "Random Long Strings:\n" + GenerateRandomString(10000));

            CustomizeCommands();
        }

        private void CustomizeCommands() {
            Debug.Log("[IgConsoleSample] type below sample commands\n" +
                "\thi\n" +
                "\tcustomfilter\n"
            );

            IgConsole.OnSubmit += (strInput) => {
                if (string.Equals("hi", strInput)) {
                    Debug.Log("Hello");
                }
            };

            IgConsole.OnSubmit += (strInput) => {
                if (string.Equals("customfilter", strInput)) {
                    ApplyCustomizedFilter();
                }
            };
        }

        private void ApplyCustomizedFilter() {
            IgConsole.Filter.CustomFilter += (ConsoleData x) => {
                if (x.Msg.Length < 15) {//filter logs to show only length less than 15
                    return true;
                } else {
                    return false;
                }
            };
        }

        private string GenerateRandomString(int length) {
            const string chars = "    abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Range(0, s.Length)]).ToArray());
        }
    }
}