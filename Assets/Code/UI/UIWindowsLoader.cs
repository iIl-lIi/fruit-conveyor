using Code.UI;
using UnityEngine;

public static class UIWindowsLoader
{
    public static UIWindow Load(UIWindowElement container, Transform parent)
    {
        if (container.Instance != null) return container.Instance;
        container.Instance = Object.Instantiate(container.Prefab, parent.transform);
        var window = container.Instance.GetComponent<UIWindow>();
        window.SetAsFirstSibling();
        window.Initialize();
        container.Instance = window;
        return window;
    }
    public static bool Unload(UIWindowElement container)
    {
        if(container.Instance == null) return false;
        Object.Destroy(container.Instance.gameObject);
        return true;
    }
}