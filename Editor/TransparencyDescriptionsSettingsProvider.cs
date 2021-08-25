﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Appegy.Att.Localization.TransparencyDescriptions;

namespace Appegy.Att.Localization
{
    internal static class TransparencyDescriptionsSettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateDeviceSimulatorSettingsProvider()
        {
            var provider = new SettingsProvider($"Project/iOS ATT Localization", SettingsScope.Project)
            {
                guiHandler = (searchContext) =>
                {
                    EditorGUIUtility.labelWidth = 200;
                    EditorGUI.indentLevel++;

                    EnabledAutoXcodeUpdate = EditorGUILayout.Toggle(nameof(EnabledAutoXcodeUpdate).AddSpacesToSentence(), EnabledAutoXcodeUpdate);
                    var guiEnabled = GUI.enabled;
                    GUI.enabled = EnabledAutoXcodeUpdate;

                    PostprocessorOrder = EditorGUILayout.IntField(nameof(PostprocessorOrder).AddSpacesToSentence(), PostprocessorOrder);
                    DefaultDescription = EditorGUILayout.TextField($"{Default} [{Default.GetLocalCodeIOS()}] - Default", DefaultDescription);
                    if (string.IsNullOrEmpty(DefaultDescription))
                    {
                        EditorGUILayout.HelpBox($"You need to specify '{nameof(DefaultDescription).AddSpacesToSentence()}' to use translations. Otherwise it won't work", MessageType.Error);
                    }
                    GUI.enabled = EnabledAutoXcodeUpdate && !string.IsNullOrEmpty(DefaultDescription);
                    EditorGUILayout.Space();
                    foreach (SystemLanguage language in Enum.GetValues(typeof(SystemLanguage)))
                    {
                        switch (language)
                        {
                            case Default:
                            case SystemLanguage.Unknown:
                                continue;
                        }
                        if (language.ToString() == "Hugarian")
                        {
                            continue;
                        }

                        var translation = GetAttDescription(language);
                        var newValue = EditorGUILayout.TextField($"{language} [{language.GetLocalCodeIOS()}]", translation);
                        if (translation != newValue)
                        {
                            SetAttDescription(language, newValue);
                        }
                    }

                    GUI.enabled = guiEnabled;
                    EditorGUI.indentLevel--;
                },
                footerBarGuiHandler = () =>
                {
                    if (GUILayout.Button("Reset To Default", GUILayout.Width(200)))
                    {
                        ResetToDefault();
                    }
                },

                keywords = new HashSet<string>(new[] {"att", "ios"}),
            };

            return provider;
        }
    }
}