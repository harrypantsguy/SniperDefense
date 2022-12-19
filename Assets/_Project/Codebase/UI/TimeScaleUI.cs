using System.Collections;
using TMPro;
using UnityEngine;

namespace _Project.Codebase.UI
{
    public class TimeScaleUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private Coroutine _routine;

        private void Start()
        {
            SetValue(TimeController.TimeScale, false);
            _text.color = _text.color.SetAlpha(0f);
        }

        private void Update()
        {
            SetValue(TimeController.TimeScale);
        }

        public void SetValue(in float value, bool lerpAlpha = true)
        {
            string newText = $"{value:0.###}";
            if (lerpAlpha && _text.text != newText)
            {
                if (_routine != null)
                    StopCoroutine(_routine);
                _routine = StartCoroutine(FadeRoutine());
            }

            _text.text = newText;
        }

        private IEnumerator FadeRoutine()
        {
            var t = 0f;
            const float prefadeTime = 1f;
            const float fadeTime = .75f;

            while (t < prefadeTime + fadeTime)
            {
                t += Time.unscaledDeltaTime;

                var col = _text.color;
                if (t <= prefadeTime)
                    col.a = 1f;
                else
                    col.a = 1f - (t - prefadeTime) / fadeTime;
                _text.color = col;

                yield return null;
            }

            _routine = null;
        }
    }
}