using UnityEngine;

namespace Code.UI
{
    [System.Serializable]
    public class UIWindowElement
    {        
        [field: SerializeField] public UIWindow Prefab { get; private set; }
        public UIWindow Instance { get; set; }
    }
}