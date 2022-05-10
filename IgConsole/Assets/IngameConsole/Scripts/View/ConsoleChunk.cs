using System;

namespace IngameConsole {

    public class ConsoleChunk {
        public const int MaxCountPerChunk = 8;

        private readonly ConsoleData[] consoleDatas = new ConsoleData[MaxCountPerChunk];
        private readonly ConsoleText[] consoleTexts = new ConsoleText[MaxCountPerChunk];
        private int consoleDataCount = 0;
        private Func<ConsoleData, float, ConsoleText> funcCreateText;
        private Func<ConsoleData, float> funcGetPreferredHeight;

        public ConsoleChunk(
            float top,
            Func<ConsoleData, float, ConsoleText> funcCreateText,
            Func<ConsoleData, float> funcGetPreferredHeight) {
            this.Top = top;
            this.Bottom = top;
            this.funcCreateText = funcCreateText;
            this.funcGetPreferredHeight = funcGetPreferredHeight;
        }

        public ConsoleData[] ConsoleDatas { get => consoleDatas; }
        public float Top { get; } = 0;
        public float Bottom { get; private set; } = 0;
        public bool IsActive { get; private set; } = false;
        public bool IsFull { get => MaxCountPerChunk <= consoleDataCount; }
        public bool IsVisible { get; private set; }

        public void Enable() {
            if (IsActive == true) {
                return;
            }

            IsActive = true;

            Bottom = Top;

            for (int loop = 0; loop < consoleDatas.Length; loop++) {
                if (consoleDatas[loop] != null) {
                    consoleTexts[loop] = funcCreateText(consoleDatas[loop], Bottom);
                    consoleTexts[loop].BgColor =
                        loop % 2 == 0 ? ConsoleViewer.BgBlack : ConsoleViewer.BgWhite;

                    Bottom -= consoleTexts[loop].Height;
                }
            }
        }

        public void Disable() {
            if (IsActive == false) {
                return;
            }

            IsActive = false;

            for (int loop = 0; loop < consoleTexts.Length; loop++) {
                if (consoleTexts[loop] != null) {
                    consoleTexts[loop].ReturnSelf();
                }
            }
        }

        public void Add(ConsoleData data) {
            consoleDatas[consoleDataCount] = data;

            if (IsActive) {
                consoleTexts[consoleDataCount] = funcCreateText(data, Bottom);
                consoleTexts[consoleDataCount].BgColor =
                    consoleDataCount % 2 == 0 ? ConsoleViewer.BgBlack : ConsoleViewer.BgWhite;
                Bottom -= consoleTexts[consoleDataCount].Height;
            } else {
                Bottom -= funcGetPreferredHeight(data);
            }

            consoleDataCount++;
        }

        public bool VisibilityCheck(float visibleTop, float visibleBottom) {
            if (visibleTop + 50 <= Bottom || visibleBottom - 50 >= Top) {
                IsVisible = false;
            } else {
                IsVisible = true;
            }

            return IsVisible;
        }

        public void UpdateArray(ConsoleData[] arrData) {
            arrData.CopyTo(consoleDatas, 0);

            consoleDataCount = 0;
            for (int loop = 0; loop < arrData.Length; loop++) {
                if (arrData[loop] != null) {
                    Bottom -= funcGetPreferredHeight(arrData[loop]);
                    consoleDataCount++;
                }
            }
        }
    }
}