using System.Linq;
using UnityEngine;

namespace IngameConsole {

    public class IgConsoleSample : MonoBehaviour {
        [SerializeField] private Sprite someRandomSprite;

        private void Start() {
            Debug.Log("Hello Console");
            Debug.LogWarning("Warning!");
            Debug.LogError("Error!");
            Debug.LogException(new System.Exception("Exception!"));
            Debug.Assert(false, "Assert Failed!");

            IgConsole.Log(someRandomSprite, "Random Sprite");
            IgConsole.Log("click here to see details", "Random Long Strings:\n"
                + GenerateRandomString(10000));

            Debug.Log("");

            //////////// Add Command ///////////////
            IgConsole.OnSubmit += (strInput) => {
                if (string.Equals("hi", strInput)) {
                    Debug.Log("Hello");
                }
            };

            IgConsole.Filter.CustomFilter += (x) => {
                return true;
            };

            for (int loop = 0; loop < 5000; loop++) {
                Debug.Log(GenerateRandomString(Random.Range(50, 300)));
            }
        }

        private string GenerateRandomString(int length) {
            const string chars = "    abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Range(0, s.Length)]).ToArray());
        }
    }
}