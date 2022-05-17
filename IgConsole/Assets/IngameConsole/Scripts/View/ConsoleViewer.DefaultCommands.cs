using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace IngameConsole {
    public partial class ConsoleViewer {
        private const string HELP_MSG = "click this message for basic commands";
        private const string CMD_HELP = "help";
        private const string CMD_FILTERBOX = "fbox";
        private const string CMD_REBUILD_LAYOUT = "rebuild";
        private const string CMD_HIDE = "hide";
        private const string CMD_CLEAR = "clear";
        private const string CMD_SCROLLDOWN = "sd";
        private const string CMD_LOGLEVEL = "lvl";
        private const string CMD_FILTER = "fil";
        private const string CMD_FILTER_USE_REGULAR_EXP = "filreg";
        private const string CMD_WRITEFILE = "writefile";

        private readonly string HELP_DETAIL =
            "First things to know:\n" +
            $" - {CMD_HELP} :  Print help message\n" +
            $" - {CMD_FILTERBOX} :  Show filter box\n" +
            $" - {CMD_HIDE} :  Hide console(deactivate view). LeftShift+O(default) to toggle visibility. modify key binding in IgConsole\n" +
            $" - {CMD_REBUILD_LAYOUT} :  Rebuild ugui layout. use it when the layout goes awry after resolution change\n" +
            "\n" +
            $"Better to know but not necessary:\n" +
            $" - {CMD_CLEAR} :  Clear logs\n" +
            $" - {CMD_SCROLLDOWN} :  Scroll down to the bottom\n" +
            $" - {CMD_LOGLEVEL} [logLevel] :  Set log level. (e.g., lvl 3)\n" +
            $" - {CMD_FILTER} [filterString] :  Set log filter string. (e.g., fil abcdef). Reset filter by applying no parameter\n" +
            $" - {CMD_FILTER_USE_REGULAR_EXP} [t/f] :  Toggle filter's regular expression. (e.g., filreg t)\n" +
            $" - {CMD_WRITEFILE} (path) :  Write logs to file. path is optional. default path -> UnityEngine.Application.streamingAssetsPath\n";

        private void SetupDefaultCommands() {
            IgConsole.OnSubmit += (x => {
                string[] splits = x.ToLower().Split(new[] {' '}, 2);

                if (splits.Length <= 0) {
                    return;
                }

                switch (splits[0]) {
                    case CMD_HELP:
                        IgConsole.Log(HELP_MSG, HELP_DETAIL);
                        break;

                    case CMD_FILTERBOX:
                        filterBox.Show();
                        break;

                    case CMD_REBUILD_LAYOUT:
                        igcScrollView.Rebuild();
                        break;

                    case CMD_HIDE:
                        Hide();
                        break;

                    ///////////////////////////////////////////////////////////

                    case CMD_CLEAR:
                        IgConsole.Clear();
                        break;

                    case CMD_SCROLLDOWN:
                        igcScrollView.ScrollRect.verticalNormalizedPosition = 0;
                        break;

                    case CMD_LOGLEVEL:
                        if (1 < splits.Length && int.TryParse(splits[1], out int lvl)) {
                            IgConsole.Filter.LogLevel = lvl;
                        }
                        break;

                    case CMD_FILTER:
                        if (1 < splits.Length) {
                            IgConsole.Filter.FilterString = splits[1];
                        } else {
                            IgConsole.Filter.FilterString = "";
                            IgConsole.Filter.CustomFilter = null;
                        }
                        break;

                    case CMD_FILTER_USE_REGULAR_EXP:
                        if (1 < splits.Length) {
                            if (splits[1].Equals("t")) {
                                IgConsole.Filter.UseRegexForFilter = true;
                            } else if (splits[1].Equals("f")) {
                                IgConsole.Filter.UseRegexForFilter = false;
                            }
                        }
                        break;

                    case CMD_WRITEFILE:
                        if (splits.Length == 1) {
                            FileCreateAndWriteLog(Application.persistentDataPath);
                        } else if (1 < splits.Length) {
                            FileCreateAndWriteLog(splits[1]);
                        }
                        break;
                }
            });
        }

        private void FileCreateAndWriteLog(string targetDirectory) {
            List<ConsoleData> logs = IgConsole.Logs; //Retrieve all logs
            StringBuilder stringBuilder = new StringBuilder();

            foreach (ConsoleData cd in logs) {
                string strTime = IgConsole.LogStartTime.AddSeconds(cd.RealtimeSinceStartup).ToString("HH:mm:ss");
                stringBuilder.Append($"[{strTime}][{cd.LogType}] {cd.Msg}\n");
                if (string.IsNullOrEmpty(cd.Detailed) == false) {
                    stringBuilder.Append($"Details: {cd.Detailed}\n");
                }
            }

            Directory.CreateDirectory(targetDirectory);

            string targetPath = $"{targetDirectory}/{DateTime.Now:yyyy_MM_dd_HH_mm}.txt";
            File.WriteAllText(targetPath, stringBuilder.ToString());

            Debug.Log(targetPath);
        }
    }
}