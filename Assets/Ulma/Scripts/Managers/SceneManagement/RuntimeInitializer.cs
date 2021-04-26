using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Ulma.Util {
    public class RuntimeInitializer : MonoBehaviour {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadPermanentScene() {
            for (int i = 0; i < UnitySceneManager.sceneCount; i++) {
                if (UnitySceneManager.GetSceneAt(i).name == "Loading") return;
            }
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
        }
    }
}