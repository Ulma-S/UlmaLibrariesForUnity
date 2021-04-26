using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Ulma.Util {
    public class SceneManager : SingletonMonoBehaviour<SceneManager> {
        private CancellationToken m_token;

        private void Start() {
            m_token = gameObject.GetCancellationTokenOnDestroy();
        }

        /// <summary>
        /// Stage IDに対応したシーンをまとめてロードする.
        /// </summary>
        /// <param name="id">Stage ID</param>
        /// <returns></returns>
        public async UniTask LoadSceneAsync(EStageID id) {
            //常駐シーン以外をアンロード
            var taskList = new List<UniTask>();
            for (int i = 0; i < UnitySceneManager.sceneCount; i++) {
                var sceneName = UnitySceneManager.GetSceneAt(i).name;
                if (sceneName == "Loading") {
                    continue;
                }
                taskList.Add(UnitySceneManager.UnloadSceneAsync(sceneName)
                    .ToUniTask(cancellationToken: m_token));
            }
            await UniTask.WhenAll(taskList);
            taskList.Clear();

            //シーンをまとめてロード
            for (int i = 0; i < StageListProvider.Instance.GetSceneNames(id).Length; i++) {
                taskList.Add(UnitySceneManager.LoadSceneAsync(StageListProvider.Instance.GetSceneNames(id)[i], LoadSceneMode.Additive)
                    .ToUniTask(cancellationToken: m_token));
            }
            await UniTask.WhenAll(taskList);

            //アクティブシーンを設定
            for (int i = 0; i < UnitySceneManager.sceneCount; i++) {
                if (UnitySceneManager.GetSceneAt(i).name == StageListProvider.Instance.GetSceneNames(id)[0]) { 
                    UnitySceneManager.SetActiveScene(UnitySceneManager.GetSceneAt(i));
                }
            }
        }
    }
}