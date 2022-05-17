using System;
using TMPro;
using UnityEngine;

namespace IngameConsole {
    [RequireComponent(typeof(TMP_InputField))]
    public class IgcInputField : MonoBehaviour {
        private readonly string[] saved = new string[32];

        public Action<string> OnSubmi2t { get; private set; }

        private TMP_InputField cmdInputField;


        private int savedCount = 0;
        private int currentIndex = -1;

        private void Awake() {
            cmdInputField = GetComponent<TMP_InputField>();
            cmdInputField.onSubmit.AddListener(OnSubmitCommandInputField);
        }

        private void Update() {
            if (!cmdInputField.isFocused) {
                return;
            }

            int dir = 0;
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                dir = 1;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                dir = -1;
            }

            if (dir == 0) {
                return;
            }

            int nextIdx = currentIndex + dir;
            if (nextIdx < 0 || savedCount <= nextIdx) {
                return;
            }

            currentIndex = nextIdx;
            cmdInputField.text = saved[nextIdx];
        }

        public void OnSubmitCommandInputField(string str) {
            if (cmdInputField.wasCanceled || string.IsNullOrEmpty(str)) {
                return;
            }

            cmdInputField.text = "";
            cmdInputField.ActivateInputField();

            currentIndex = -1;
            AddSaved(str);

            IgConsole.LogInput(str);
        }

        private void AddSaved(string str) {
            if (str.Equals(saved[0])) {
                return;
            }

            Array.Copy(saved, 0, saved, 1, saved.Length - 1);

            saved[0] = str;

            if (savedCount < saved.Length) {
                savedCount++;
            }
        }
    }
}