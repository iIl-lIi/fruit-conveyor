using UnityEngine;
using UnityEngine.Events;

public class GrabbingTaskUnityEvents : MonoBehaviour
{
    [field: SerializeField] public string[] GrabbingObjectIds { get; private set; }
    [field: SerializeField] public UnityEvent<GrabbingTask> ComplettedTask { get; private set; }
    [field: SerializeField] public UnityEvent<GrabbingTask> GeneratedNewTask { get; private set; }

    private bool Allow(string id)
    {
        foreach (var objectId in GrabbingObjectIds)
        {
            if (objectId != id) continue;
            return true;
        }
        return false;
    } 
    private void OnGeneratedNewTask(GrabbingTask task)
    {
        if (!Allow(task.taskId)) return;
        GeneratedNewTask.Invoke(task);
    }
    private void OnCompletedTask(GrabbingTask task)
    {
        if (!Allow(task.taskId)) return;
        ComplettedTask.Invoke(task);
    }

    private void Awake()
    {
        GrabbingTaskEvents.CompletedTask.Event += OnCompletedTask; 
        GrabbingTaskEvents.GeneratedNewTask.Event += OnGeneratedNewTask; 
    }
    private void OnDestroy()
    {
        GrabbingTaskEvents.CompletedTask.Event -= OnCompletedTask;
        GrabbingTaskEvents.GeneratedNewTask.Event -= OnGeneratedNewTask;
    }
}