using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AwesomeSnippets {

    public class ScriptTemplateModifier : UnityEditor.AssetModificationProcessor {
        public const string Prefs_ScriptTemplatePath = "Prefs_ScriptTemplatePath";

        [MenuItem("Assets/Create/C# Script Template", false, 80)]
        private static void CreateScriptFromTemplate() {
            string templatePath = EditorPrefs.GetString(Prefs_ScriptTemplatePath, "");

            if (string.IsNullOrEmpty(templatePath)) {
                Debug.LogWarning("you need to set script template in AwesomeSnippets/ScriptTemplate in order to create script template");
                return;
            }

            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewBehaviourScript.cs");
        }
    }
}