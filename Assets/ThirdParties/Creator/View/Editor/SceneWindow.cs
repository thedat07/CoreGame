using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Creator
{
    public class SceneWindow : EditorWindow
    {
        enum State
        {
            IDLE,
            GENERATING,
            COMPILING,
            COMPILING_AGAIN,
            ASK,
            YES,
            NO,
        }

        public string sceneName;
        public string sceneDirectoryPath;
        public string sceneTemplateFile;
        public bool fullScreen;

        string scenePath;
        string controllerPath;
        State state = State.IDLE;

        [MenuItem("SS/Scene Manager/Generate Scene")]
        public static void ShowWindow()
        {
            SceneWindow win = ScriptableObject.CreateInstance<SceneWindow>();

            win.minSize = new Vector2(400, 200);
            win.maxSize = new Vector2(400, 200);

            win.fullScreen = true;

            win.ResetParams();
            win.ShowUtility();

            win.LoadPrefs();
        }

        [MenuItem("SS/Tools/Take Screenshot %p")] // Phím tắt: Ctrl + P (Windows) hoặc Cmd + P (Mac)
        public static void TakeScreenshot()
        {
            string folderPath = Application.dataPath + "/Screenshots/";
            System.IO.Directory.CreateDirectory(folderPath); // Tạo thư mục nếu chưa có
            string filename = folderPath + "Screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
            ScreenCapture.CaptureScreenshot(filename);
            UnityEngine.Console.Log("SS Tools", "Screenshot saved to: " + filename);
        }

        void ResetParams()
        {
            sceneName = string.Empty;
        }

        void LoadPrefs()
        {
            sceneDirectoryPath = EditorPrefs.GetString("SS_VIEW_SCENE_DIRECTORY_PATH", "Scenes/");
            sceneTemplateFile = EditorPrefs.GetString("SS_VIEW_SCENE_TEMPLATE_FILE", "TemplateScene.unity");
        }

        void SavePrefs()
        {
            EditorPrefs.SetString("SS_VIEW_SCENE_DIRECTORY_PATH", sceneDirectoryPath);
            EditorPrefs.SetString("SS_VIEW_SCENE_TEMPLATE_FILE", sceneTemplateFile);
        }

        void OnGUI()
        {
            GUILayout.Label("Scene Generator", EditorStyles.boldLabel);
            sceneName = EditorGUILayout.TextField("Scene Name", sceneName);
            sceneDirectoryPath = EditorGUILayout.TextField("Scene Directory Path", sceneDirectoryPath);
            sceneTemplateFile = EditorGUILayout.TextField("Scene Template File", sceneTemplateFile);
            fullScreen = EditorGUILayout.Toggle("Fullscreen", fullScreen);

            switch (state)
            {
                case State.IDLE:
                    if (GUILayout.Button("Generate"))
                    {
                        if (GenerateScene())
                        {
                            state = State.GENERATING;
                        }
                    }
                    break;
                case State.GENERATING:
                    if (EditorApplication.isCompiling)
                    {
                        state = State.COMPILING;
                    }
                    break;
                case State.COMPILING:
                    if (EditorApplication.isCompiling)
                    {
                        EditorUtility.DisplayProgressBar("Compiling Scripts", "Wait for a few seconds...", 0.33f);
                    }
                    else
                    {
                        EditorUtility.ClearProgressBar();
                        SetupScene();
                        state = State.COMPILING_AGAIN;
                    }
                    break;
                case State.COMPILING_AGAIN:
                    if (EditorApplication.isCompiling)
                    {
                        EditorUtility.DisplayProgressBar("Compiling Scripts", "Wait for a few seconds...", 0.66f);
                    }
                    else
                    {
                        EditorUtility.ClearProgressBar();
                        SaveScene();
                        state = State.ASK;

                        if (EditorUtility.DisplayDialog("Successful!", "Scene was generated. Do you want to add it to Build Settings", "Yes", "No"))
                        {
                            state = State.YES;
                        }
                        else
                        {
                            state = State.NO;
                        }
                    }
                    break;
                case State.ASK:
                    break;
                case State.YES:
                    AddToBuildSettings();
                    ResetParams();
                    state = State.IDLE;
                    break;
                case State.NO:
                    ResetParams();
                    state = State.IDLE;
                    break;
            }
        }

        bool GenerateScene()
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                UnityEngine.Console.LogWarning("SS Tools", "You have to input an unique name to 'Scene Name'");
                return false;
            }

            string targetRelativePath = System.IO.Path.Combine(sceneDirectoryPath, sceneName + "/" + sceneName + ".unity");
            string targetFullPath = SS.IO.Path.GetAbsolutePath(targetRelativePath);

            if (System.IO.File.Exists(targetFullPath))
            {
                UnityEngine.Console.LogWarning("SS Tools", "This scene file is already exist!");
                return false;
            }

            if (string.IsNullOrEmpty(sceneTemplateFile))
            {
                UnityEngine.Console.LogWarning("SS Tools", "You have to input scene template file!");
                return false;
            }

            SavePrefs();
            if (!CreateScene())
            {
                UnityEngine.Console.LogWarning("SS Tools", "Scene template file is not exist!");
                return false;
            }
            CreateController();
            return true;
        }

        bool CreateScene()
        {
            string targetRelativePath = System.IO.Path.Combine(sceneDirectoryPath, sceneName + "/" + sceneName + ".unity");
            string targetFullPath = SS.IO.File.Copy(sceneTemplateFile, targetRelativePath);

            if (targetFullPath == null)
            {
                return false;
            }

            scenePath = SS.IO.Path.GetRelativePathWithAssets(targetRelativePath);

            AssetDatabase.ImportAsset(scenePath);

            SS.Tool.Scene.OpenScene(targetFullPath);

            return true;
        }

        void CreateController()
        {
            string baseScenePath = System.IO.Path.Combine(sceneDirectoryPath, sceneName);
            string scriptsFolder = System.IO.Path.Combine(baseScenePath, "Scripts");
            string prefabsFolder = System.IO.Path.Combine(baseScenePath, "Prefabs");

            // Tạo các thư mục nếu chưa tồn tại
            if (!System.IO.Directory.Exists(scriptsFolder))
                System.IO.Directory.CreateDirectory(scriptsFolder);

            if (!System.IO.Directory.Exists(prefabsFolder))
                System.IO.Directory.CreateDirectory(prefabsFolder);

            // Tạo Controller trong thư mục Scripts
            string targetRelativePath = System.IO.Path.Combine(scriptsFolder, sceneName + "Controller.cs");
            string targetFullPath = SS.IO.File.Copy("TemplateController.cs", targetRelativePath);

            SS.IO.File.ReplaceFileContent(targetFullPath, "TEMPLATE_SCENE_NAME", sceneName.ToUpper() + "_SCENE_NAME");
            SS.IO.File.ReplaceFileContent(targetFullPath, "Template", sceneName);

            controllerPath = SS.IO.Path.GetRelativePathWithAssets(targetRelativePath);

            AssetDatabase.ImportAsset(controllerPath);
        }

        void SetupScene()
        {
            Controller c = GameObject.FindObjectOfType<Controller>();
            GameObject go = c.gameObject;

            go.name = sceneName;
            if (c != null)
            {
                DestroyImmediate(c);
            }

            var canvas = c.Canvas;
            var camera = c.Camera;

            var type = GetAssemblyType(sceneName + "Controller");
            go.AddComponent(type);

            c = go.GetComponent<Controller>();

            c.Canvas = canvas;
            c.Camera = camera;
            c.FullScreen = fullScreen;

            AssetDatabase.ImportAsset(controllerPath);
        }

        void SaveScene()
        {
            SS.Tool.Scene.MarkCurrentSceneDirty();
            SS.Tool.Scene.SaveScene();
        }

        void AddToBuildSettings()
        {
            if (!string.IsNullOrEmpty(scenePath))
            {
                List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

                for (int i = 0; i < editorBuildSettingsScenes.Count; i++)
                {
                    if (string.Compare(editorBuildSettingsScenes[i].path, scenePath) == 0)
                    {
                        editorBuildSettingsScenes.RemoveAt(i);
                        break;
                    }
                }

                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));

                EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();

                AssetDatabase.SaveAssets();
            }
        }

        Type GetAssemblyType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null)
            {
                return type;
            }

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }
    }
}