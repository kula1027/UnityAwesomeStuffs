using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AwesomeSnippets {

    [InitializeOnLoad]
    public class ScriptTemplateWindow : EditorWindow {
        public TextAsset templateText;
        private Vector2 scroll;
        private string textScript;

        [MenuItem("AwesomeSnippets/ScriptTemplate")]
        public static void ShowWindow() {
            GetWindow(typeof(ScriptTemplateWindow), false, "ScriptTemplate");
        }

        private void OnGUI() {
            OnGUI_SelectScript();

            EditorGUILayout.Space(30);

            OnGUI_ScriptPreview();
        }

        private void OnGUI_ScriptPreview() {
            scroll = EditorGUILayout.BeginScrollView(scroll);

            GUIStyle style = new GUIStyle(EditorStyles.textArea);
            style.wordWrap = true;
            style.stretchHeight = true;

            string strLabel = "";
            if (templateText) {
                if (textScript != templateText.text) {
                    textScript = templateText.text;
                }
                strLabel = textScript;
            }
            GUILayout.Label(strLabel, style);

            EditorGUILayout.EndScrollView();
        }

        private void OnGUI_SelectScript() {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Template", EditorStyles.boldLabel);

            string pathTemplate = EditorPrefs.GetString(ScriptTemplateModifier.Prefs_ScriptTemplatePath, "");
            if (string.IsNullOrEmpty(pathTemplate) == false) {
                templateText = (TextAsset)AssetDatabase.LoadAssetAtPath(pathTemplate, typeof(TextAsset));
            }

            templateText = (TextAsset)EditorGUILayout.ObjectField(templateText, typeof(TextAsset), false);

            string templateScriptPath = "";
            if (templateText) {
                templateScriptPath = AssetDatabase.GetAssetPath(templateText);
            }
            EditorPrefs.SetString(
                    ScriptTemplateModifier.Prefs_ScriptTemplatePath,
                    templateScriptPath);

            EditorGUILayout.EndHorizontal();
        }
    }
}