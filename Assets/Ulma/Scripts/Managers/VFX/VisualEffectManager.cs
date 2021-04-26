using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ulma.Util {
    public class VisualEffectManager : SingletonMonoBehaviour<VisualEffectManager> {
        [Serializable]
        public class VisualEffectData {
            public EVisualEffectID ID;
            public GameObject Sorce;
        }

        [SerializeField] private int m_maxEffect = 10;
        [SerializeField] private List<VisualEffectData> m_visualEffectDatas = new List<VisualEffectData>();

        /// <summary>
        /// 生成済みエフェクトのリスト.
        /// </summary>
        private readonly List<GameObject> m_instantiatedEffects = new List<GameObject>();
        
        /// <summary>
        /// 生成済みエフェクトのキュー.
        /// </summary>
        private readonly Queue<int> m_instantiatedEffectIdxQueue = new Queue<int>();
        
        private void Start() {
            //領域確保
            for (int i = 0; i < m_maxEffect; i++) {
                m_instantiatedEffects.Add(null);
            }
        }

        /// <summary>
        /// エフェクトを再生.
        /// </summary>
        /// <param name="id">VFX ID</param>
        /// <param name="position">再生する座標</param>
        public void Play(EVisualEffectID id, Vector3 position) {
            var data = m_visualEffectDatas.Find(dt => dt.ID == id);
            if (data == null) {
                Debug.LogError(id + "に対応するVFXデータが存在しません.");
                return;
            }

            var effect = Instantiate(data.Sorce, position, Quaternion.identity);

            //現在のエフェクト数が最大数と同値以上の場合は順次上書きする.
            if (GetCurrentInstantiatedEffectCount() >= m_maxEffect) {
                var idx = m_instantiatedEffectIdxQueue.Dequeue();
                
                m_instantiatedEffects[idx] = effect;
            }
            else {
                var idx = GetEmptyIdx();
                m_instantiatedEffectIdxQueue.Enqueue(idx);
                m_instantiatedEffects[idx] = effect;
            }
        }


        /// <summary>
        /// 現在シーン上に存在するエフェクト数を返す.
        /// </summary>
        /// <returns></returns>
        private int GetCurrentInstantiatedEffectCount() {
            var count = 0;
            foreach (var effect in m_instantiatedEffects) {
                if (effect != null) {
                    count++;
                }
            }
            return count;
        }
        

        /// <summary>
        /// 生成済みエフェクトリストから空いているインデックスを検索して返す.
        /// </summary>
        /// <returns></returns>
        private int GetEmptyIdx() {
            var idx = -1;
            for (int i = 0; i < m_maxEffect; i++) {
                if (m_instantiatedEffects[i] == null) {
                    idx = i;
                    break;
                }
            }
            return idx;
        }
    }
}