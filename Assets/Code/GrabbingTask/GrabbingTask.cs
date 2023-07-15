using System;
using UnityEngine;

public class GrabbingTask
{
    public readonly string taskId;
    public readonly int count;

    public bool IsCompleted { get; private set; }
    
    private int _counter;
    public event Action<int, int> CompletedSted;

    public GrabbingTask(string id, int count)
    {
        this.taskId = id;
        this.count = count;
    }
    public bool CompleteStep()
    {
        if (IsCompleted || ++_counter != count)
        {
            CompletedSted?.Invoke(_counter, count);
            return IsCompleted;
        }
        GrabbingTaskEvents.CompletedTask.SafeInvoke(this);
        IsCompleted = true;
        return true;
    }
}