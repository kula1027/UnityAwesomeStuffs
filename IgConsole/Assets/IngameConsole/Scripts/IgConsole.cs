using IngameConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Ingame Console
/// </summary>
public partial class IgConsole : MonoBehaviour {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private static IgConsole instance;

    #region Unity SerializeFields

    [Header("Options")]
    [SerializeField] private bool printLogLevel = true;

    [SerializeField] private int logLevelDefault = 0;
    [SerializeField] private int logLevelWarning = 1;
    [SerializeField] private int logLevelError = 2;
    [SerializeField] private int logLevelInput = 3;

    [Header("Colors")]
    [SerializeField] private Color colorLog;
    [SerializeField] private Color colorWarning;
    [SerializeField] private Color colorError;

    [Header("Viewer")]
    [SerializeField] private KeyCode keyBind0;
    [SerializeField] private KeyCode keyBind1;
    [SerializeField] private ConsoleViewInterface viewInterface;

    #endregion Unity SerializeFields    

    private int nextLogId;
    private readonly List<ConsoleData> logs = new List<ConsoleData>(512);
    private readonly ConsoleFilter filter = new ConsoleFilter();
    private Action<string> onSubmitCommand;

    private DateTime logStartTime;

    private void Awake() {
        if (ReferenceEquals(instance, null)) {
            Initialize();
        }
    }

    private void Update() {
        if (Input.GetKey(keyBind0) && Input.GetKeyDown(keyBind1)) {
            if (viewInterface.IsVisible) {
                Hide();
            } else {
                Show();
            }
        }
    }

    private void Initialize() {
        instance = this;

        logStartTime = DateTime.Now;

        DontDestroyOnLoad(gameObject);

        if (!ReferenceEquals(viewInterface, null)) {
            viewInterface.Initialize(GetFilteredConsoleData);
        }

        filter.OnValueChanged += UpdateConsole;

        Application.logMessageReceived += LogMessageReceived;
        Debug.developerConsoleVisible = false;
    }

    private void OnDestroy() {
        Application.logMessageReceived -= LogMessageReceived;

        instance = null;
    }

    private void LogMessageReceived(string logString, string stackTrace, LogType type) {
        switch (type) {
            case LogType.Error:
                PrintError(logString, stackTrace);
                break;

            case LogType.Assert:
                PrintError(logString, stackTrace);
                break;

            case LogType.Warning:
                PrintWarning(logString, stackTrace);
                break;

            case LogType.Log:
                PrintLog(logString, stackTrace);
                break;

            case LogType.Exception:
                PrintError(logString, stackTrace);
                break;
        }
    }

    private void Add(ConsoleData data) {

        logs.Add(data);

        if (filter.Check(data) && !ReferenceEquals(viewInterface, null)) {
            viewInterface.Add(data);
        }
    }

    private void ClearData() {
        logs.Clear();
        nextLogId = 0;

        if (!ReferenceEquals(viewInterface, null)) {
            viewInterface.ResetData();
        }
    }

    private void UpdateConsole(ConsoleFilter filter) {
        if (!ReferenceEquals(viewInterface, null)) {
            viewInterface.ResetData();
        }
    }

    private List<ConsoleData> GetFilteredConsoleData() {
        return logs.AsParallel()
            .AsOrdered()
            .Where(x => filter.Check(x)).ToList();
    }

    private void Print(int logLevel, string msg, string detailed) {
        string strMsg = msg ?? "Null";

        if (printLogLevel) {
            strMsg = $"[{logLevel}]{strMsg}";
        }

        ConsoleData item = new ConsoleData(
                                Time.realtimeSinceStartup,
                                strMsg,
                                detailed,
                                logLevel,
                                IgLogType.Log
                            );
        Add(item);
    }

    private void PrintSprite(int logLevel, string msg, Sprite sprite) {
        string strMsg = msg ?? "Null";

        if (printLogLevel) {
            strMsg = $"[{logLevel}]{strMsg}";
        }

        ConsoleData item = new ConsoleData(
                                Time.realtimeSinceStartup,
                                sprite,
                                strMsg,
                                "",
                                logLevel,
                                IgLogType.Log
                            );
        Add(item);
    }

    private void PrintLog(string msg, string detailed) {
        ConsoleData item = new ConsoleData(
                        Time.realtimeSinceStartup,
                        msg,
                        detailed,
                        logLevelDefault,
                        IgLogType.Log
                    );

        Add(item);
    }

    private void PrintWarning(string msg, string detailed) {
        ConsoleData item = new ConsoleData(
                        Time.realtimeSinceStartup,
                        msg,
                        detailed,
                        logLevelWarning,
                        IgLogType.Warning
                    );

        Add(item);
    }

    private void PrintError(string msg, string detailed) {
        ConsoleData item = new ConsoleData(
                        Time.realtimeSinceStartup,
                        msg,
                        detailed,
                        logLevelError,
                        IgLogType.Error
                    );

        Add(item);
    }

    private void PrintInput(string msg) {
        ConsoleData item = new ConsoleData(
                                Time.realtimeSinceStartup,
                                msg,
                                "",
                                logLevelInput,
                                IgLogType.Input
                            );
        Add(item);

        OnSubmit?.Invoke(msg);
    }
#else
    private void Awake() {
        Destroy(gameObject);
    }
#endif
}