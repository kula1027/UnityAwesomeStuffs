using IngameConsole;
using System;
using System.Collections.Generic;
using UnityEngine;

public partial class IgConsole {

#if UNITY_EDITOR || DEVELOPMENT_BUILD        
    public static IgConsole Instance {
        get {
            if (ReferenceEquals(instance, null)) {
                instance = FindObjectOfType<IgConsole>();

                if (ReferenceEquals(instance, null)) {
                    Debug.LogError($"{typeof(IgConsole).Name} not found.");
                } else {
                    instance.Initialize();
                }
            }

            return instance;
        }
    }

    public static Color LogColor(IgLogType type) {
        switch (type) {
            case IgLogType.Warning:
                return Instance.colorWarning;

            case IgLogType.Error:
                return Instance.colorError;

            case IgLogType.Log:
            case IgLogType.Input:
            default:
                return Instance.colorLog;
        }
    }

    public static int NextLogId
        => Instance.nextLogId++;

    public static List<ConsoleData> Logs
        => Instance.logs;

    public static ConsoleFilter Filter
        => Instance.filter;

    public static Action<string> OnSubmit {
        get => Instance.onSubmitCommand;
        set => Instance.onSubmitCommand = value;
    }

    public static DateTime LogStartTime
        => Instance.logStartTime;

    public static void Clear() {
        Instance.ClearData();
    }

    public static void Log(string msg, string detailed = "") {
        Instance.Print(Instance.logLevelDefault, msg, detailed);
    }

    public static void Log(int logLevel, string msg, string detailed = "") {
        Instance.Print(logLevel, msg, detailed);
    }

    public static void Log(Sprite sprite, string msg = "") {
        Instance.PrintSprite(Instance.logLevelDefault, msg, sprite);
    }

    public static void Log(int logLevel, Sprite sprite, string msg) {
        Instance.PrintSprite(logLevel, msg, sprite);
    }

    /// <summary>
    /// create input to console. invokes OnSubmit
    /// </summary>
    /// <param name="msg"></param>
    public static void LogInput(string msg) {
        Instance.PrintInput(msg);
    }

    /// <summary>
    /// disable viewer gameobject
    /// </summary>
    public static void Hide() {
        Instance.viewInterface.Hide();
    }

    /// <summary>
    /// enable viewer gameobject
    /// </summary>
    public static void Show() {
        Instance.viewInterface.Show();
    }

    public static bool IsVisible => Instance.viewInterface.IsVisible;
#else
    public static IgConsole Instance {
        get => null;
    }

    public static Color LogColor(IgLogType type) {
        return Color.white;
    }

    public static int NextLogId
        => 0;

    public static List<ConsoleData> Logs
        => new List<ConsoleData>();

    public static ConsoleFilter Filter {
        get => new ConsoleFilter();
    }

    public static Action<string> OnSubmit {
        get {
            return (x) => { };
        }

        set { /* do nothing */ }
    }

    public static DateTime LogStartTime
        => new DateTime();

    public static void Clear() { /* do nothing */ }


    //Log Msg
    public static void Log(string msg, string detailed = "") {
        /* do nothing */
    }

    public static void Log(int logLevel, string msg, string detailed = "") {
        /* do nothing */
    }

    public static void Log(Sprite sprite, string msg = "") {
        /* do nothing */
    }

    public static void Log(int logLevel, Sprite sprite, string msg) {
        /* do nothing */
    }

    public static void LogInput(string msg) {
        /* do nothing */
    }

    public static void Hide() {
        /* do nothing */
    }

    public static void Show() {
        /* do nothing */
    }

    public static bool IsVisible => false;
#endif
}