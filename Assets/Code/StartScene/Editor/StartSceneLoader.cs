using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EditorTools.StartScene.Editor
{
    [InitializeOnLoad]
    internal static class StartSceneLoader
    {
        private const string EDITOR_LOAD_START_SCENE_ON_PLAY = "StartSceneLoader.LoadStartSceneOnPlay";
        private const string EDITOR_START_SCENE = "StartSceneLoader.InitializeScene";
        private const string EDITOR_PREVIOUS_SCENE = "StartSceneLoader.PreviousScene";

        private static bool LoadStartSceneOnPlay
        {
            get => EditorPrefs.GetBool(EDITOR_LOAD_START_SCENE_ON_PLAY + Application.dataPath, false);
            set => EditorPrefs.SetBool(EDITOR_LOAD_START_SCENE_ON_PLAY + Application.dataPath, value);
        }
        private static string StartScene
        {
            get => EditorPrefs.GetString(EDITOR_START_SCENE + Application.dataPath, "Start.unity");
            set => EditorPrefs.SetString(EDITOR_START_SCENE + Application.dataPath, value);
        }
        private static string[] PreviousScenes
        {
            get { return EditorPrefs.GetString(EDITOR_PREVIOUS_SCENE + Application.dataPath).Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries); }
            set => EditorPrefs.SetString(EDITOR_PREVIOUS_SCENE + Application.dataPath, string.Join("|", value));
        }
        private static string[] CurrentScenesPath => EditorSceneManager.GetSceneManagerSetup().Select(x => x.path).ToArray();

        private static bool isLoaded;
        
        static StartSceneLoader()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }
        
        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (!LoadStartSceneOnPlay) return;

            if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                PreviousScenes = CurrentScenesPath;
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    try
                    {
                        EditorSceneManager.OpenScene(StartScene);
                        isLoaded = false;
                    }
                    catch (Exception)
                    {
                        Debug.LogError($"Start scene <color=yellow>not found!</color>");
                        EditorApplication.isPlaying = false;
                    }
                }
                else
                {
                    EditorApplication.isPlaying = false;
                }
            }

            if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode || isLoaded) return;
            
            for (var i = 0; i < PreviousScenes.Length; i++)
                EditorSceneManager.OpenScene(PreviousScenes[i], i == 0 ? OpenSceneMode.Single : OpenSceneMode.Additive);

            isLoaded = true;
        }

        [MenuItem("Start Scene/Open Start Scene")]
        public static void OpenCurrentStartScene()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            
            try
            {
                EditorSceneManager.OpenScene(StartScene);
            }
            catch (Exception)
            {
                Debug.LogError($"Start scene <color=yellow>not found!</color>");
            }
        }
        [MenuItem("Start Scene/Select Start Scene...")]
        public static void SelectStartScene()
        {
            var startScene = EditorUtility.OpenFilePanel("Select start scene", Application.dataPath, "unity");
        
            if (!string.IsNullOrEmpty(startScene))
            {
                StartScene = startScene;
                LoadStartSceneOnPlay = true;
                Debug.Log("Select start scene: " + startScene);
            }
        }

        [MenuItem("Start Scene/Load Start Scene On Play", true)]
        public static bool ShowLoadStartSceneOnPlay()
        {
            return !LoadStartSceneOnPlay;
        }
        [MenuItem("Start Scene/Load Start Scene On Play")]
        public static void EnableLoadStartSceneOnPlay()
        {
            LoadStartSceneOnPlay = true;
        }
        
        [MenuItem("Start Scene/Don't Load Start Scene On Play", true)]
        public static bool ShowDontLoadStartSceneOnPlay()
        {
            return LoadStartSceneOnPlay;
        }
        [MenuItem("Start Scene/Don't Load Start Scene On Play")]
        public static void DisableLoadStartSceneOnPlay()
        {
            LoadStartSceneOnPlay = false;
        }
    }
}