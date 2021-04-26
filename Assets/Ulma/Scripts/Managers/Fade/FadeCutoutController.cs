using UnityEngine;
using UnityEngine.UI;

namespace Ulma.Util{
    [RequireComponent(typeof(Image))]
    public class FadeCutoutController : MonoBehaviour{
        [SerializeField] private RectTransform m_rectTransform;
        [SerializeField] private Image m_image;
        [SerializeField] public float Scale = 1.0f;
        private Material m_material;
        private static readonly int pScale = Shader.PropertyToID("_Scale");

        private void Start(){
            m_material = m_image.material;
            Scale = 10.0f;

            var aspect = Screen.height / Screen.width;
            var rect = m_rectTransform.rect;
            if (aspect < 16.0f / 9.0f) {
                rect.width = Screen.width;
                rect.height = Screen.width * 16.0f / 9.0f;
            }
            else {
                rect.height = Screen.height;
                rect.width = Screen.height * 9.0f / 16.0f;
            }

            m_rectTransform.sizeDelta = new Vector2(rect.width, rect.height);
        }

        private void Update(){
            m_material.SetFloat(pScale, Scale);
        }

        private void Reset(){
            m_rectTransform = GetComponent<RectTransform>();
            m_image = GetComponent<Image>();
        }
    }
}