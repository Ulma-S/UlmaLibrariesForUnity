using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Ulma.Util {
    public class BgmManager : SingletonMonoBehaviour<BgmManager> {
        /// <summary>
        /// BGMのデータを格納する.
        /// </summary>
        [Serializable]
        public class BgmData {
            public EBgmID ID;
            public AudioClip Clip;
        }

        [SerializeField] private AudioSource m_audioSource;
        [SerializeField] private List<BgmData> m_bgmDatas = new List<BgmData>();
        private bool m_isFade = false;
        private const float m_defaultVolume = 1.0f;
        private CancellationToken m_token;
        
        /// <summary>
        /// BGMのボリューム.
        /// </summary>
        public float Volume {
            get => m_audioSource.volume;
            set {
                if (!m_isFade) {
                    m_audioSource.volume = value;
                }
            }
        }

        /// <summary>
        /// 再生中か?
        /// </summary>
        public bool IsPlaying => m_audioSource.isPlaying;

        /// <summary>
        /// 現在再生中のBGM ID.
        /// </summary>
        public EBgmID CurrentBgmID { get; private set; } = EBgmID.None;
        
        
        private void Start() {
            m_token = gameObject.GetCancellationTokenOnDestroy();
            
            //ループ設定
            m_audioSource.loop = true;
        }

        /// <summary>
        /// IDに対応したBGMを再生する.
        /// </summary>
        /// <param name="id">BGM ID</param>
        public void Play(EBgmID id) {
            var data = m_bgmDatas.Find(dt => dt.ID == id);
            if (data == null) {
                Debug.LogError(id + "に対応するBGMデータが存在しません.");
                return;
            }
            m_audioSource.clip = data.Clip;
            m_audioSource.Play();
            CurrentBgmID = id;
        }

        /// <summary>
        /// BGMの再生を終了する.
        /// </summary>
        public void Stop() {
            m_audioSource.Stop();
            CurrentBgmID = EBgmID.None;
        }
        
       
        /// <summary>
        /// BGMをフェードインで再生.
        /// </summary>
        /// <param name="id">BGM ID</param>
        /// <param name="duration">フェード時間</param>
        public async UniTask FadeIn(EBgmID id, float duration = 1.0f) {
            m_isFade = true;
            m_audioSource.volume = 0.0f;
            Play(id);
            await m_audioSource.DOFade(m_defaultVolume, duration).ToUniTask(cancellationToken: m_token);
            m_isFade = false;
        }


        /// <summary>
        /// BGMをフェードアウト.
        /// </summary>
        /// <param name="duration">フェード時間</param>
        public async UniTask FadeOut(float duration = 1.0f) {
            if (CurrentBgmID == EBgmID.None || !IsPlaying) return;
            m_isFade = true;
            await m_audioSource.DOFade(0.0f, duration).ToUniTask(cancellationToken: m_token);
            Stop();
            m_isFade = false;
        }


        /// <summary>
        /// ミュートする.
        /// </summary>
        public void Mute() {
            m_audioSource.volume = 0.0f;
        }

        private void Reset() {
            m_audioSource = GetComponent<AudioSource>();
        }
    }
}