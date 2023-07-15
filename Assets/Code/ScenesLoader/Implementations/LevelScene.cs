using Cysharp.Threading.Tasks;
using Code.UI;
using UnityEngine;
using DG.Tweening;

namespace Code.ScenesLoader.Implementations
{
    public class LevelScene : AbstractScene
    {
        [Header("Camera")]
        [SerializeField] private Transform _CameraTransform;
        [SerializeField] private Transform _CameraEndPoint;

        [Header("Conveyor Belt")]
        [SerializeField] private ConveyorBelt _ConveyorBelt;
        [SerializeField] private Transform _ConveyorBeltTransform;
        [SerializeField] private Transform _ConveyorBeltEndPoint;

        [Header("Grabbing Task")]
        [SerializeField] private GrabbingTaskGenerator _GrabbingTaskGenerator;
        [SerializeField] private ParticleSystem _CompleteParfticles;
        private LevelUIWindow _window;

        public override async UniTask Initialize()
        {
            _window = UIController.LoadWindow<LevelUIWindow>();
            _window.ShowImmediate();
        }
        public override async UniTask Enter()
        {
            var task = _GrabbingTaskGenerator.GenerateNewGrabbingTask();
            _window.SetGrabbingTaskInfo(task);
        }
        public override async UniTask Exit()
        {
            
        }

        public async void CompleteLevel()
        {
            _CameraTransform.DOMove(_CameraEndPoint.position, 2).SetEase(Ease.InOutCirc);
            _CameraTransform.DORotate(_CameraEndPoint.eulerAngles, 2).SetEase(Ease.InOutCirc);
            _ConveyorBeltTransform.DOMove(_ConveyorBeltEndPoint.position, 0.5f);
            foreach (var obj in _ConveyorBelt.objects)
                obj.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack);

            await _window.Hide();
            UIController.UnloadWindow(_window);
            LevelEvents.CompletedLevel.SafeInvoke();

            await UniTask.Delay(300);
            _CompleteParfticles.Play();
        }
    }
}