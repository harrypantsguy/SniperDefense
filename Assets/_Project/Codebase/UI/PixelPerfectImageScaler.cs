using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Codebase.UI
{
    public class PixelPerfectImageScaler : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private RawImage _rawImage;

        private CameraController _camera;

        public const float PPU = 32;

        private void Start()
        {
            _camera = CameraController.Singleton;
            _rectTransform = GetComponent<RectTransform>();
            _rawImage = GetComponent<RawImage>();
        }

        private void LateUpdate()
        {
           // _rectTransform.sizeDelta = new Vector2(_camera.Camera.orthographicSize * _camera.Camera.aspect * 
            //PPU, _camera.Camera.orthographicSize * PPU);
            
            _camera.Camera.targetTexture?.Release();

            Vector2 cameraWorldCenter = _camera.transform.position;
            Vector2 pixelWorldCenter = (cameraWorldCenter * PPU).Floor() / PPU;
            //_rectTransform.position = _camera.Camera.WorldToScreenPoint(pixelWorldCenter);
            
            Vector2Int newSize = new Vector2Int(Mathf.CeilToInt(
                PPU * _camera.WorldCamera.orthographicSize * (16f / 9f) * 2f), 
                Mathf.CeilToInt(PPU * _camera.Camera.orthographicSize * 2f));
            
            //Debug.Log($"{newSize}");
            RenderTexture _screenTexture = new RenderTexture(newSize.x, newSize.y, 0);
            _screenTexture.filterMode = FilterMode.Point;
            _camera.WorldCamera.targetTexture = _screenTexture;
            _rawImage.texture = _screenTexture;
        }
    }
}