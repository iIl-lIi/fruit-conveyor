using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Code.ScenesLoader
{
    [Serializable]
    public class SceneContainer
    {
        public string SceneName { get; private set; }
        public AbstractScene AbstractSceneInstance;

        public bool IsLoaded { get; private set; }
        public Scene SceneInstance { get; private set; }

        public async UniTask<AbstractScene> Load(string sceneName, bool enterToScene,
            Action<AbstractScene> preInitialization = default)
        {
            if (AbstractSceneInstance) await AbstractSceneInstance.Exit();

            SceneName = sceneName;
            var operation = SceneManager.LoadSceneAsync(sceneName);
            await operation;

            SceneInstance = SceneManager.GetSceneByName(sceneName);
            var objects = SceneInstance.GetRootGameObjects();
            AbstractSceneInstance = null;
            objects.FirstOrDefault(x => x.TryGetComponent<AbstractScene>(out AbstractSceneInstance));
            if (AbstractSceneInstance == null) throw new Exception($"{nameof(AbstractScene)} not find...");
            
            preInitialization?.Invoke(AbstractSceneInstance);
            await AbstractSceneInstance.Initialize();
            IsLoaded = true;
            if (enterToScene) await AbstractSceneInstance.Enter();
            return AbstractSceneInstance;
        }
    }
}