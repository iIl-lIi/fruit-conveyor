using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Windows
{
    public class LoadingUIWindow : UIWindow
    {
        [SerializeField] private TMP_Text _ProgressText;
        [SerializeField] private Image _ProgressImage;
        private bool _loadingProcess;

        public override void Initialize()
        { 
            base.Initialize();
            ResetProgress();
        }
        public void ResetProgress()
        {
            _ProgressText.text = $"0%";
            _ProgressImage.fillAmount = 0;
        }
        public async UniTask SetProgress(float value)
        {
            value = Mathf.Clamp01(value);
            _ProgressText.text = $"{(value * 100f):0.#}%";
            _ProgressImage.fillAmount = value;
            await UniTask.Yield();
        }
        public async UniTask StartLoad(params Action[] toLoad)
        {
            if (_loadingProcess) return;
            _loadingProcess = true;
            ResetProgress();
            SetAsLastSibling();
            if (State != ElementState.IsShown) await Show();
            for (int i = 0; i < toLoad.Length; i++)
            {
                toLoad[i]();
                await SetProgress(i / toLoad.Length);
            }
            await SetProgress(1);
            _loadingProcess = false;
        }
    }
}