using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.ScenesLoader
{
    public abstract class AbstractScene : MonoBehaviour
    {
        [field: SerializeField] public string SceneName { get; private set; } = "Level";
        
        public abstract UniTask Initialize();
        public abstract UniTask Enter();
        public abstract UniTask Exit();
    }
}