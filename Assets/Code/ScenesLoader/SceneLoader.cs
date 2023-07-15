using System;
using Cysharp.Threading.Tasks;

namespace Code.ScenesLoader
{
    public static class SceneLoader
    {
        public static event Action Loaded;
        public static event Action Unloaded;

        public static readonly SceneContainer CurrentScene = new ();
        public static bool IsBusy { get; private set; }
        
        public static void Initialize()
        {
            IsBusy = false;
        }
        public static async UniTask<AbstractScene> Load(string scene, bool enterToScene,
            Action<AbstractScene> preInitialization = default)
        {
            if (IsBusy) return default;
            IsBusy = true;
            var aScene = await CurrentScene.Load(scene, enterToScene, preInitialization);
            IsBusy = false;
            Loaded?.Invoke();
            return aScene;
        }
    }
}