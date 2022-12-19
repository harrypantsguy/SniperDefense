using UnityEngine;
using UnityEngine.UI;

namespace _Project.Codebase.UI
{
    public class FillableBarUI : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public bool BarEnabled
        {
            get => _image.enabled;
            set => _image.enabled = value;
        }

        public Transform ImageTransform => _image.transform;
        public bool lerpFill;
        public float lerpSpeed;
        private float _targetFill;
        public float FillAmount => _image.fillAmount;

        public Color Color
        {
            get => _image.color;
            set => _image.color = value;
        }

        private void Start()
        {
            _image.fillAmount = 1f;
            _targetFill = _image.fillAmount;
        }

        public void SetFillAmount(float amount)
        {
            _targetFill = amount;
            if (!lerpFill)
                _image.fillAmount = amount;
        }
        
        private void Update()
        {
            if (lerpFill)
            {
                _image.fillAmount = Mathf.Lerp(_image.fillAmount, _targetFill, lerpSpeed * Time.unscaledDeltaTime);
            }
        }
    }
}