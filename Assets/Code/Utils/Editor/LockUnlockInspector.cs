using UnityEditor;

public static class LockUnlockInspector
{
    [MenuItem("Utils/Inspector Lock #w")]
    public static void ToggleInspectorLock()
    {
        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
        ActiveEditorTracker.sharedTracker.ForceRebuild();
    }
}