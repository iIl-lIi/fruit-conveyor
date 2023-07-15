using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace Code.UI
{
    public static class UIController
    {
        private static Canvas _canvas;
        private static UIWindowsElementsList _windowsElementsList;

        public static void Initialize(Canvas canvas, UIWindowsElementsList containers)
        {
            _canvas = canvas;
            _windowsElementsList = containers;
            UnloadAllWindows();
            LayoutRebuilder.ForceRebuildLayoutImmediate(canvas.transform as RectTransform);
        }
        public static void UnloadAllWindows(params UIWindow[] exeptWindows)
        {
            var cont = false;
            foreach (var wc in _windowsElementsList.List)
            {
                cont = false;
                foreach (var ew in exeptWindows)
                {
                    if(ew != wc.Instance) continue;
                    cont = true;
                    break;
                }
                if(cont || wc.Instance == null) continue;
                UIWindowsLoader.Unload(wc);
            }
        }
        public static void HideAllWindowsImmediate(params UIWindow[] exeptWindows)
        {
            var cont = false;
            foreach (var wc in _windowsElementsList.List)
            {
                cont = false;
                foreach (var ew in exeptWindows)
                {
                    if(ew != wc.Instance) continue;
                    cont = true;
                    break;
                }
                if(cont || wc.Instance == null) continue;
                wc.Instance.HideImmediate();
            }
        }
        public static async UniTask HideAllWindows(params UIWindow[] exeptWindows)
        {
            var actions = new List<UniTask>();
            var cont = false;
            foreach (var wc in _windowsElementsList.List)
            {
                cont = false;
                foreach (var ew in exeptWindows)
                {
                    if(ew != wc.Instance) continue;
                    cont = true;
                    break;
                }
                if(cont || wc.Instance == null) continue;
                actions.Add(wc.Instance.Hide());
            }
            await UniTask.WhenAll(actions);
        }

        public static T LoadWindow <T>() where T : UIWindow
        {
            foreach (var wc in _windowsElementsList.List)
                if (wc.Prefab is T tWindow)
                    return UIWindowsLoader.Load(wc, _canvas.transform) as T;

            Debug.LogError($"Window '{nameof(T)}' is not exist!");
            return default;
        }
        public static T LoadWindowWithPrefab <T>(T prefab) where T : UIWindow
        {
            foreach (var wc in _windowsElementsList.List)
                if (wc.Prefab == prefab)
                    return UIWindowsLoader.Load(wc, _canvas.transform) as T;

            Debug.LogError($"Window '{nameof(T)}' is not exist!");
            return default;
        }
        public static T GetWindow<T>() where T : UIWindow
        {
            foreach (var wc in _windowsElementsList.List)
                if (wc.Instance is T tWindow)
                    return tWindow;

            Debug.LogError($"Window '{nameof(T)}' is not exist!");
            return default;
        }
        public static T GetWindowWithPrefab<T>(T prefab) where T : UIWindow
        {
            foreach (var wc in _windowsElementsList.List)
                if (wc.Prefab == prefab)
                    return UIWindowsLoader.Load(wc, _canvas.transform) as T;

            Debug.LogError($"Window '{nameof(T)}' is not exist!");
            return default;
        }
        public static bool UnloadWindow(UIWindow instance)
        {
            foreach (var wc in _windowsElementsList.List)
            {
                if (wc.Instance != instance) continue;
                return UIWindowsLoader.Unload(wc);
            }
            return false;
        }
        public static bool UnloadWindow<T>() where T : UIWindow
        {
            foreach (var wc in _windowsElementsList.List)
            {
                if (wc.Instance is not T tWindow) continue;
                return UIWindowsLoader.Unload(wc);
            }
            return false;
        }
        public static bool UnloadWindowWithPrefab<T>(T prefab) where T : UIWindow
        {
            foreach (var wc in _windowsElementsList.List)
            {
                if (wc.Prefab != prefab) continue;
                return UIWindowsLoader.Unload(wc);
            }
            return false;
        }
    }
}