using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace AwesomeSnippets {

    [InitializeOnLoad]
    public class ScriptTemplatesWindow : EditorWindow {
        public const string Label_AwesomeSnippets = "AwesomeSnippets";
        private ScriptTemplatesSettings settings;

        private Vector2 scroll;

        private TextAsset previewTextAsset;

        public ScriptTemplatesSettings Settings {
            get => this.settings;
            set => this.settings = value;
        }

        [MenuItem("Window/Script Templates")]
        public static void ShowWindow() {
            ScriptTemplatesWindow window = GetWindow<ScriptTemplatesWindow>(false, "Script Templates");

            string[] vs = AssetDatabase.FindAssets($"t:{typeof(ScriptTemplatesSettings).Name}");
            if (vs.Length > 0) {
                window.Settings = AssetDatabase.LoadAssetAtPath<ScriptTemplatesSettings>(AssetDatabase.GUIDToAssetPath(vs[0]));
            }
        }

        public static DirectoryInfo FindWindowScriptAssetDirectory() {
            string[] mtw = AssetDatabase.FindAssets($"{typeof(ScriptTemplatesWindow).Name} l:{Label_AwesomeSnippets}");
            if (mtw.Length > 0) {
                return Directory.GetParent(AssetDatabase.GUIDToAssetPath(mtw[0]));
            } else {
                return null;
            }
        }

        private void DeleteExistingScript() {
            string[] vs = AssetDatabase.FindAssets($"{ScriptTemplatesMenuItemGenerator.Name_Generate_Class} l:{Label_AwesomeSnippets}");

            foreach (string foundAssetGUID in vs) {
                AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(foundAssetGUID));
            }
        }

        private void OnGUI_ButtonSaveAndUpdate() {
            EditorGUILayout.Space(50);

            Vector2 btnSize = new Vector2(180, 30);
            Rect rect = new Rect(
                new Vector2(EditorGUIUtility.currentViewWidth - btnSize.x - 10, 10),
                btnSize
            );

            GUILayout.BeginArea(rect);
            GUILayoutOption[] options = {
                GUILayout.Height(btnSize.y),
                GUILayout.Width(btnSize.x)
            };
            if (GUILayout.Button("Save & Update", options)) {
                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();

                DeleteExistingScript();
                ScriptTemplatesMenuItemGenerator.GenerateMenuItemScript(settings);
            }
            GUILayout.EndArea();
        }

        private void OnGUI() {
            minSize = new Vector2(320, 300);

            if (settings == null) {
                OnGUI_StartButton();
            } else {
                OnGUI_SetupTemplates();
            }
        }

        private void OnGUI_SetupTemplates() {
            EditorGUILayout.BeginVertical();
            {
                OnGUI_ButtonSaveAndUpdate();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.BeginVertical(GUILayout.Width(320));
                    {
                        OnGUI_SelectScript();
                    }
                    EditorGUILayout.EndVertical();

                    OnGUI_ScriptPreview();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        private void OnGUI_StartButton() {
            string[] vs = AssetDatabase.FindAssets($"t:{typeof(ScriptTemplatesSettings).Name}");

            if (vs.Length > 0) {
                settings = AssetDatabase.LoadAssetAtPath<ScriptTemplatesSettings>(AssetDatabase.GUIDToAssetPath(vs[0]));
            } else {
                Vector2 btnSize = new Vector2(300, 60);
                Rect rect = new Rect(
                    new Vector2((EditorGUIUtility.currentViewWidth - btnSize.x) / 2, btnSize.y),
                    btnSize
                );

                GUILayout.BeginArea(rect);
                GUILayoutOption[] options = {
                        GUILayout.Height(btnSize.y),
                        GUILayout.Width(btnSize.x)
                    };

                if (GUILayout.Button("Start Using Script Templates", options)) {
                    DirectoryInfo directoryInfo = FindWindowScriptAssetDirectory();
                    if (directoryInfo != null) {
                        AssetDatabase.CreateAsset(CreateInstance<ScriptTemplatesSettings>(), directoryInfo + "\\Script Templates Settings.asset");
                        AssetDatabase.SaveAssets();

                        settings = AssetDatabase.LoadAssetAtPath<ScriptTemplatesSettings>(directoryInfo + "\\Script Templates Settings.asset");
                    }
                }

                GUILayout.EndArea();
            }
        }

        private void OnGUI_SelectScript() {
            GUILayout.Label("Templates", EditorStyles.boldLabel);

            for (int loop = 0; loop < settings.Templates.Count; loop++) {
                EditorGUILayout.BeginHorizontal();

                TextAsset taBefore = settings.Templates[loop];
                settings.Templates[loop] = (TextAsset)EditorGUILayout.ObjectField(settings.Templates[loop], typeof(TextAsset), false);
                if (taBefore != settings.Templates[loop]) {
                    if (HasDuplicate(settings.Templates, loop)) {
                        settings.Templates[loop] = null;
                    } else {
                        previewTextAsset = settings.Templates[loop];
                    }
                }

                GUILayout.Space(5);

                if (GUILayout.Button(EditorGUIUtility.FindTexture("ViewToolOrbit"), GUILayout.Width(30))) {
                    previewTextAsset = settings.Templates[loop];
                }
                if (GUILayout.Button(EditorGUIUtility.FindTexture("Toolbar Minus"), GUILayout.Width(30))) {
                    if (previewTextAsset == settings.Templates[loop]) {
                        previewTextAsset = null;
                    }
                    settings.Templates.RemoveAt(loop);
                }

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add Template")) {
                settings.Templates.Add(null);
            }
        }

        private void OnGUI_ScriptPreview() {
            EditorGUILayout.BeginVertical();

            GUIStyle style = new GUIStyle(EditorStyles.textArea);
            style.wordWrap = true;
            style.stretchHeight = true;

            string strPreviewName = "";
            string strPreview = "";
            if (previewTextAsset != null) {
                strPreview = previewTextAsset.text;
                strPreviewName = previewTextAsset.name;
            }

            GUILayout.Label(strPreviewName, EditorStyles.boldLabel);
            scroll = EditorGUILayout.BeginScrollView(scroll);

            GUILayout.Label(strPreview, style);

            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();
        }

        private bool HasDuplicate(List<TextAsset> textAssets, int idx) {
            for (int loop = 0; loop < textAssets.Count; loop++) {
                if (loop == idx) {
                    continue;
                }

                if (textAssets[loop] == textAssets[idx]) {
                    return true;
                }
            }

            return false;
        }
    }
}