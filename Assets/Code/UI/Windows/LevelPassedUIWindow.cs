using Code.ScenesLoader;
using Code.UI;
using Code.UI.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public class LevelPassedUIWindow : UIWindow
{
    [SerializeField] private TMP_Text _Text;
    [SerializeField] private Button _Button;
    private bool _reloaded;

    public override void Initialize()
    {
        base.Initialize();
        StartShowed += OnStartShowed;
    }
    public async void ReloadLevel(string sceneName)
    {
        if (_reloaded) return;
        _reloaded = true;
        await HideLabels();
        var loadingWindow = UIController.LoadWindow<LoadingUIWindow>();
        await loadingWindow.StartLoad(
            () => UIController.HideAllWindowsImmediate(loadingWindow),
            async () => await SceneLoader.Load(sceneName, false));
        
        await UniTask.Delay(1000);
        await loadingWindow.Hide();
        await SceneLoader.CurrentScene.AbstractSceneInstance.Enter();
        UIController.UnloadWindow(loadingWindow);
        _reloaded = false;
    }

    private void OnStartShowed(UIWindow window) => ShowLabels();
    
    private async void ShowLabels()
    {
        var task1 = _Text.transform
            .DOScale(Vector3.one, 1)
            .SetEase(Ease.OutBack)
            .AsyncWaitForCompletion();

        var task2 = _Text.transform
            .DORotate(Vector3.zero, 1)
            .SetEase(Ease.OutBack)
            .AsyncWaitForCompletion();

        var task3 = _Button.transform
            .DOScale(Vector3.one, 1)
            .SetEase(Ease.OutBack)
            .AsyncWaitForCompletion();

        await Task.WhenAll(task1, task2, task3);
    }
    private async Task HideLabels()
    {
        var task1 = _Text.transform
            .DOScale(Vector3.zero, 1)
            .SetEase(Ease.InBack)
            .AsyncWaitForCompletion();

        var task2 = _Text.transform
            .DORotate(Vector3.forward * 180, 1)
            .SetEase(Ease.InBack)
            .AsyncWaitForCompletion();

        var task3 = _Button.transform
            .DOScale(Vector3.zero, 1)
            .SetEase(Ease.InBack)
            .AsyncWaitForCompletion();

        await Task.WhenAll(task1, task2, task3);
    }

    private void OnDestroy() 
    {
        StartShowed -= OnStartShowed;
    }
}