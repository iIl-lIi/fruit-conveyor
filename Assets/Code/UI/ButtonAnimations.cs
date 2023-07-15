using UnityEngine;
using UnityEngine.UI;

namespace BaranovskyStudio
{
    [RequireComponent(typeof(Animator))]
    [AddComponentMenu("UI/Button Animations", 0)]
    public class ButtonAnimations : MonoBehaviour
    {
        private string _triggerName;
        private Animator _animator;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
            GetComponent<Button>().onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _animator.SetTrigger(_triggerName);
        }
    }
}