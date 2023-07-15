using UnityEngine;

public class GrabbingTaskGenerator : MonoBehaviour
{
    [SerializeField] private ConveyorBeltObjectsList _TargetList;
    [SerializeField, Min(1)] private int _MaxGrabbingCount = 5;

    public GrabbingTask GenerateNewGrabbingTask()
    {
        var id = _TargetList.List[UnityEngine.Random.Range(0, _TargetList.List.Count)].Id;
        var task = new GrabbingTask(id, UnityEngine.Random.Range(1, _MaxGrabbingCount + 1));
        GrabbingTaskEvents.GeneratedNewTask.SafeInvoke(task);
        return task;
    }
}