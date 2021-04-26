using Cysharp.Threading.Tasks;
using Ulma.Util;
using UnityEngine;
using Zenject;

public class Test : MonoBehaviour {
    [Inject] private IInputProvider m_inputProvider;

    private void Start() {
        //BgmManager.Instance.Play(EBgmID.Title);
    }

    private async void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            await UIFadeManager.Instance.FadeOut();
            await SceneManager.Instance.LoadSceneAsync(EStageID.Game);
            await UIFadeManager.Instance.FadeIn();
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            BgmManager.Instance.FadeOut().Forget();
        }else if (Input.GetKeyDown(KeyCode.K)) {
            BgmManager.Instance.FadeIn(EBgmID.Title, 5.0f).Forget();
        }

        Debug.Log(m_inputProvider.HorizontalInput);
    }
}
