using Events;

public static class GrabbingTaskEvents
{
    public static EventHolder<GrabbingTask> GeneratedNewTask = new ();
    public static EventHolder<GrabbingTask> CompletedTask = new ();
}