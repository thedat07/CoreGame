#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public static class OnSceneSwitch
{
    static List<string> sceneNames;
    
    static int selectedSceneIndex = 0;

    static class ToolbarStyles
    {
        public static readonly GUIStyle commandButtonStyle;

        static ToolbarStyles()
        {
            commandButtonStyle = new GUIStyle("Command")
            {
                fontSize = 10,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Normal,
                fixedWidth = 70,
            };
        }
    }


    static OnSceneSwitch()
    {
        ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI_Right);
    }


    static void OnToolbarGUI_Right()
    {
        if (sceneNames == null)
        {
            LoadAllSceneNames();
        }

        if (sceneNames != null && sceneNames.Count > 0)
        {
            selectedSceneIndex = EditorGUILayout.Popup(selectedSceneIndex, sceneNames.ToArray(), GUILayout.Width(120));
            
            if (GUILayout.Button("Open", GUILayout.Width(50)))
            {
                SceneHelper.StartScene(sceneNames[selectedSceneIndex]);
            }
        }
        else
        {
            GUILayout.Label("No scenes found", GUILayout.Width(120));
        }
    }

    static void LoadAllSceneNames()
    {
        sceneNames = new List<string>();

        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                string path = scene.path;
                string name = System.IO.Path.GetFileNameWithoutExtension(path);
                sceneNames.Add(name);
            }
        }

        if (sceneNames.Count == 0)
        {
            sceneNames.Add("No scenes in build");
        }
    }

    static class SceneHelper
    {
        static string sceneToOpen;

        public static void StartScene(string sceneName)
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
            }

            sceneToOpen = sceneName;
            EditorApplication.update += OnUpdate;
        }

        static void OnUpdate()
        {
            if (sceneToOpen == null ||
                EditorApplication.isPlaying || EditorApplication.isPaused ||
                EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            EditorApplication.update -= OnUpdate;

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                // need to get scene via search because the path to the scene
                // file contains the package version so it'll change over time
                string[] guids = AssetDatabase.FindAssets("t:scene " + sceneToOpen, null);
                if (guids.Length == 0)
                {
                    Debug.LogWarning("Couldn't find scene file");
                }
                else
                {
                    string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
                    EditorSceneManager.OpenScene(scenePath);
                    //EditorApplication.isPlaying = true;
                }
            }
            sceneToOpen = null;
        }
    }
}

#endif