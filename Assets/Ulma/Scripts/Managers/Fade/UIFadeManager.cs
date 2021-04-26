using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Ulma.Util{
    public class UIFadeManager : SingletonMonoBehaviour<UIFadeManager>{
        [SerializeField] private FadeCutoutController m_cutoutController;
        [SerializeField] private float m_maxScale = 3.0f;
        [SerializeField] private float m_fadeTime = 1.0f;
        [SerializeField] private float m_waitTime = 0.8f;
        private CancellationToken m_token;

        private void Start() {
            m_token = gameObject.GetCancellationTokenOnDestroy();
        }

        /// <summary>
        /// 画面のフェードイン.
        /// </summary>
        /// <returns></returns>
        public async UniTask FadeIn() {
            await DOVirtual.Float(0.0f, m_maxScale, m_fadeTime, value => {
                m_cutoutController.Scale = value;
            }).SetEase(Ease.InExpo).WithCancellation(m_token);
        }

        /// <summary>
        /// 画面のフェードアウト.
        /// </summary>
        /// <returns></returns>
        public async UniTask FadeOut() {
            await DOVirtual.Float(m_maxScale, 0.0f, m_fadeTime, value => {
                m_cutoutController.Scale = value;
            }).SetEase(Ease.OutExpo).WithCancellation(m_token);
            
            await UniTask.Delay((int) (m_waitTime * 1000), cancellationToken: m_token);
        }
    }
}