using Code.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelUIWindow : UIWindow
{
    [SerializeField] private TMP_Text _Text;
    [SerializeField] private Image _Background;

    private GrabbingTask _currentTask;

    public void SetGrabbingTaskInfo(GrabbingTask task)
    {
        SaveUnsubscribe();
        UpdateText(task.count, task.taskId);
        task.CompletedSted += OnCompletedStep;
        _currentTask = task;

        _Background.DOFade(0.5f, 1).SetEase(Ease.OutCirc);
        _Text.transform.DOScale(Vector2.one, 1).SetEase(Ease.OutBack);
    }

    private void UpdateText(int count, string id)
    {
        _Text.text = $"Collect <color=red>{count}</color> <color=yellow>{id}</color>";
    }
    private void OnCompletedStep(int count, int max)
    {
        UpdateText(max - count, _currentTask.taskId);
    }
    private void SaveUnsubscribe()
    {
        if (_currentTask != null)
            _currentTask.CompletedSted -= OnCompletedStep;
    }
    private void OnDestroy() => SaveUnsubscribe();
}