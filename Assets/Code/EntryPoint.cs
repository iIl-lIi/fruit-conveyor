using Code.ScenesLoader;
using Code.UI;
using Code.UI.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private string _SceneToEntry = "Level";
    [SerializeField] private Canvas _UIControllerCanvas;
    [SerializeField] private UIWindowsElementsList _UIWindowsElementsList;

    private async void Awake() 
    {
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(_UIControllerCanvas.gameObject);
        UIController.Initialize(_UIControllerCanvas, _UIWindowsElementsList);
        var loadingWindow = UIController.LoadWindow<LoadingUIWindow>();
        loadingWindow.ShowImmediate();
        await loadingWindow.StartLoad(async () => await SceneLoader.Load(_SceneToEntry, false));
        
        await UniTask.Delay(1000);
        await loadingWindow.Hide();
        UIController.UnloadWindow(loadingWindow);
        await SceneLoader.CurrentScene.AbstractSceneInstance.Enter();
    }
}