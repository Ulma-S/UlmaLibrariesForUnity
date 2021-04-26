using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ulma.Util {
    public class StageListProvider : SingletonMonoBehaviour<StageListProvider> {
        [Serializable]
        public class StageSet {
            public EStageID ID;
            public string[] SceneNames;
        }

        [SerializeField] private List<StageSet> m_stageList = new List<StageSet>();

        /// <summary>
        /// IDに対応したシーン名の配列を返す.
        /// </summary>
        /// <param name="id">Stage ID</param>
        /// <returns></returns>
        public string[] GetSceneNames(EStageID id) {
            string[] sceneNames = {string.Empty};
            var set = m_stageList.Find(s => s.ID == id);
            if (set != null) {
                sceneNames = set.SceneNames;
            }
            return sceneNames;
        }
    }
}