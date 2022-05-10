using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace IngameConsole {

    public class ConsoleViewer : ConsoleViewInterface {
        public static readonly Color BgBlack = new Color(0.3f, 0.3f, 0.3f, 0.15f);
        public static readonly Color BgWhite = new Color(0.7f, 0.7f, 0.7f, 0.15f);

        [SerializeField] private GameObject goNormalView;
        [SerializeField] private GameObject goMiniView;

        [SerializeField] private IgcScrollView igcScrollView;
        [SerializeField] private TMP_InputField commandInputField;
        [SerializeField] private FilterBox filterBox;

        private Func<List<ConsoleData>> getFilteredData;

        private void Awake() {
            commandInputField.onSubmit.AddListener(OnSubmitCommandInputField);
        }

        private void Start() {
            if (FindObjectOfType<EventSystem>() == null) {
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }

            SceneManager.sceneLoaded += (_, __) => {
                if (FindObjectOfType<EventSystem>() == null) {
                    new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                }
            };

            IgConsole.OnSubmit += (x => {
                switch (x) {
                    case "fbox":
                        filterBox.Show();
                        break;

                    case "rebuild":
                        igcScrollView.Rebuild();
                        break;
                }
            });
        }

        private void OnEnable() {
            if (IgConsole.Instance) {
                ResetData();
            }
        }

        public override void Initialize(Func<List<ConsoleData>> getFilteredData) {
            this.getFilteredData = getFilteredData;

            filterBox.Initialize();
            igcScrollView.Initialize();
        }

        public override void Show() {
            gameObject.SetActive(true);
        }

        public override void Hide() {
            gameObject.SetActive(false);
        }

        public override void Add(ConsoleData data) {
            if (gameObject.activeInHierarchy) {
                igcScrollView.Add(data);
            }
        }

        public override void ResetData() {
            if (gameObject.activeInHierarchy) {
                igcScrollView.ResetData(getFilteredData());
            }
        }

        public void Minimize() {
            goNormalView.SetActive(false);
            goMiniView.SetActive(true);

            goMiniView.GetComponent<RectTransform>().anchoredPosition = goNormalView.GetComponent<RectTransform>().anchoredPosition;
        }

        public void Maximize() {
            goNormalView.SetActive(true);
            goMiniView.SetActive(false);

            goNormalView.GetComponent<RectTransform>().anchoredPosition = goMiniView.GetComponent<RectTransform>().anchoredPosition;
        }

        public void OnSubmitCommandInputField(string str) {
            if (commandInputField.wasCanceled || string.IsNullOrEmpty(str)) {
                return;
            }

            IgConsole.LogInput(str);

            commandInputField.text = "";
            commandInputField.ActivateInputField();
        }
    }
}