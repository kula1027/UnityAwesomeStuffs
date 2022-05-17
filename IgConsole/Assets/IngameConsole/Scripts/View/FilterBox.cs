using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IngameConsole {

    public class FilterBox : MonoBehaviour {
        [SerializeField] private TMP_InputField filterLogLevel;
        [SerializeField] private TMP_InputField filterInputField;
        [SerializeField] private TextMeshProUGUI textSubmittedInput;
        [SerializeField] private Toggle toggleRegex;

        public void Initialize() {
            filterInputField.onSubmit.AddListener(OnSubmitFilterInputField);
            IgConsole.Filter.OnValueChanged += (filter) => {
                filterLogLevel.text = filter.LogLevel.ToString();
                filterInputField.text = filter.FilterString;
                toggleRegex.isOn = filter.UseRegexForFilter;
            };
        }

        public void OnSubmitFilterInputField(string str) {
            if (filterInputField.wasCanceled) {
                return;
            }

            IgConsole.Filter.FilterString = str ?? "";

            textSubmittedInput.text = str;
        }

        public void OnToggleRegex(bool value) {
            IgConsole.Filter.UseRegexForFilter = value;
        }

        public void OnLogLevelChanged(string logLevel) {
            if (string.IsNullOrEmpty(logLevel)) {
                return;
            }

            if (int.TryParse(logLevel, out int result)) {
                IgConsole.Filter.LogLevel = result;
            } else {
                Debug.LogWarning($"Invalid input {logLevel}, Failed int.TryParse");
            }
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        public void Show() {
            gameObject.SetActive(true);
        }
    }
}