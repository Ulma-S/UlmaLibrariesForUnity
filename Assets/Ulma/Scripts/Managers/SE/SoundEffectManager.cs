using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ulma.Util {
    public class SoundEffectManager : SingletonMonoBehaviour<SoundEffectManager> {
        [Serializable]
        public class SoundEffectData {
            public ESoundEffectID ID;
            public AudioClip Clip;
        }

        [SerializeField] private int m_maxAudioSource = 5;
        [SerializeField] private List<SoundEffectData> m_soundEffectDatas = new List<SoundEffectData>();
        private readonly List<AudioSource> m_audioSources = new List<AudioSource>();

        private void Start() {
            for (int i = 0; i < m_maxAudioSource; i++) {
                var src = gameObject.AddComponent<AudioSource>();
                m_audioSources.Add(src);
            }
            
            m_audioSources.ForEach(src => {
                src.playOnAwake = false;
                src.loop = false;
            });
        }

        /// <summary>
        /// IDに対応するSEを再生.
        /// </summary>
        /// <param name="id">SE ID</param>
        public void Play(ESoundEffectID id) {
            var data = m_soundEffectDatas.Find(dt => dt.ID == id);
            if (data == null) {
                Debug.LogError(id + "に対応するSEデータが存在しません.");
                return;
            }
            
            var audioSource = m_audioSources.Find(src => !src.isPlaying);
            if (audioSource == null) {
                audioSource = m_audioSources[0];
            }
            audioSource.PlayOneShot(data.Clip);
        }
    }
}