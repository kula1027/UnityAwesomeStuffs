using System;
using System.Collections.Generic;
using UnityEngine;

namespace IngameConsole {

    public partial class ConsoleViewer : ConsoleViewInterface {

        public static readonly Color BgBlack = new Color(0.3f, 0.3f, 0.3f, 0.15f);
        public static readonly Color BgWhite = new Color(0.7f, 0.7f, 0.7f, 0.15f);

        [SerializeField] private bool showHelpOnStartUp;

        [SerializeField] private IgcScrollView igcScrollView;
        [SerializeField] private FilterBox filterBox;

        private Func<List<ConsoleData>> getFilteredData;

        public override bool IsVisible => gameObject.activeInHierarchy;

        public Action OnShow { get; set; }
        public Action OnHide { get; set; }


        private void OnEnable() {
            if (IgConsole.Instance) {
                ResetData();
            }
        }

        public override void Initialize(Func<List<ConsoleData>> getFilteredData) {
            SetupDefaultCommands();

            this.getFilteredData = getFilteredData;

            filterBox.Initialize();
            igcScrollView.Initialize();

            if (showHelpOnStartUp) {
                IgConsole.Log(HELP_MSG, HELP_DETAIL);
            }
        }

        public override void Show() {
            OnShow?.Invoke();
            gameObject.SetActive(true);
        }

        public override void Hide() {
            OnHide?.Invoke();
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
    }
}