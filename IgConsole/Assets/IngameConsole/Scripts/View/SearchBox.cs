using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IngameConsole {

    public class SearchBox : MonoBehaviour {
        [SerializeField] private TMP_InputField searchInputField;
        [SerializeField] private Toggle toggleRegex;

        public void Initialize() {
            searchInputField.onSubmit.AddListener(OnSubmitFilterInputField);
        }

        public void OnSubmitFilterInputField(string str) {
            if (searchInputField.wasCanceled) {
                return;
            }
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void FindNext() {
        }

        public void FindPrev() {
        }
    }
}