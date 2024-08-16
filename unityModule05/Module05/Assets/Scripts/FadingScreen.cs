using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Module05
{
    public class FadingScreen : MonoBehaviour
    {
        [SerializeField] private Image _image;
        
        private float _fadeDuration = 1.5f;
        public Action OnFadeComplete;

        private void Awake()
        {
            _image.canvasRenderer.SetAlpha(0.0f);
        }

        public void Fade(bool fadeIn)
        {
            StartCoroutine(FadeCoroutine(fadeIn));
        }
        
        private IEnumerator FadeCoroutine(bool fadeIn)
        {
            _image.CrossFadeAlpha(fadeIn ? 1 : 0, _fadeDuration, true);
            yield return new WaitForSeconds(_fadeDuration);
            OnFadeComplete?.Invoke();
        }
    }
}