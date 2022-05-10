using UnityEngine;

namespace IngameConsole {

    public enum IgLogType {
        Log,
        Warning,
        Error,
        Input,
    }

    public class ConsoleData {

        public ConsoleData(float realtimeSinceStartup, string msg, string detailed, int logLevel, IgLogType logType) {
            this.LogId = IgConsole.NextLogId;

            this.RealtimeSinceStartup = realtimeSinceStartup;
            this.Msg = msg;
            this.Detailed = detailed;
            this.LogLevel = logLevel;
            this.LogType = logType;
        }

        public ConsoleData(float realtimeSinceStartup, Sprite sprite, string msg, string detailed, int logLevel, IgLogType logType) {
            this.LogId = IgConsole.NextLogId;

            this.RealtimeSinceStartup = realtimeSinceStartup;
            this.Sprite = sprite;
            this.HasSprite = true;
            this.Msg = msg;
            this.Detailed = detailed;
            this.LogLevel = logLevel;
            this.LogType = logType;
        }

        public int LogId { get; }
        public float RealtimeSinceStartup { get; }
        public Sprite Sprite { get; } = null;
        public bool HasSprite { get; } = false;
        public string Msg { get; }
        public string Detailed { get; private set; }
        public int LogLevel { get; }
        public IgLogType LogType { get; }
    }
}