using System.Collections;
using _Project.Content.Prefabs;
using DanonsTools.ContentLayer;
using DanonsTools.ServiceLayer;
using TMPro;
using UnityEngine;

namespace _Project.Codebase.UI
{
    public class FlyingText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private AnimationCurve _sizeCurve;
        private RectTransform _rectTransform;
        private bool _isScreenSpace;

        private float _textSize;
        private float _defaultTextSize;
        private Vector2 _travelDirection;
        private Vector2 _startPosition;
        private Vector2 _finalPosition;

        public const float WORLDSPACE_DEFAULT_SIZE = .8f;
        public const float WORLDSPACE_MAX_TRAVEL_SIZE = 2f;
        public const float SCREENSPACE_DEFAULT_SIZE = 25f;
        public const float LIFETIME = 1f;
        private const float FADE_OUT_START_TIME = .8f;
        
        public string Text
        {
            get => _text.text;
            set => _text.text = value;
        }

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _defaultTextSize = _isScreenSpace ? SCREENSPACE_DEFAULT_SIZE : WORLDSPACE_DEFAULT_SIZE;
            UpdateRectTransform();
            
            _startPosition = transform.position;
            _finalPosition = _startPosition + _travelDirection * 
                Random.Range(WORLDSPACE_MAX_TRAVEL_SIZE / 2f, WORLDSPACE_MAX_TRAVEL_SIZE);
            
            StartCoroutine(LifetimeRoutine());
        }
        
        private void UpdateRectTransform()
        {
            _rectTransform.sizeDelta = new Vector2(_textSize, _textSize);
        }
        
        private IEnumerator LifetimeRoutine()
        {
            float t = 0;
            while (t < LIFETIME)
            {
                _textSize = _sizeCurve.Evaluate(t / LIFETIME) * _defaultTextSize;
                UpdateRectTransform();
                float exponentComp = 1f - Mathf.Exp(-t / (LIFETIME / 6f));
                transform.position = Vector3.Lerp(_startPosition, _finalPosition, exponentComp);
                if (t > FADE_OUT_START_TIME)
                {
                    _text.alpha = Mathf.Exp(-(t - FADE_OUT_START_TIME) / ((LIFETIME - FADE_OUT_START_TIME) / 3f));
                }
                yield return null;
                t += Time.unscaledDeltaTime;
            }
            
            Destroy(gameObject);
        }

        public static void SpawnFlyingText(string text, Vector2 position, Vector2 travelDirection, bool screenSpace)
        {
            var contentService = ServiceLocator.Retrieve<IContentService>();

            GameObject prefab = contentService.GetCachedAsset<GameObject>(PrefabAssetGroup.FLYING_TEXT);
            FlyingText spawnedFlyingText = Instantiate(prefab, 
                (screenSpace ? 
                    RuntimeRefs.Singleton.ScreenSpaceCanvas : 
                    RuntimeRefs.Singleton.WorldSpaceCanvas).transform).GetComponent<FlyingText>();
            
            spawnedFlyingText.transform.position = position;
            spawnedFlyingText.name = $"{text} flying text";
            spawnedFlyingText._text.text = text;
            spawnedFlyingText._travelDirection = travelDirection;
            spawnedFlyingText._isScreenSpace = screenSpace;
        }
    }
}