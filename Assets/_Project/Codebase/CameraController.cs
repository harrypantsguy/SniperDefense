using System.Collections.Generic;
using _Project.Codebase.UI;
using UnityEngine;
namespace _Project.Codebase
{
    public class CameraController : MonoSingleton<CameraController>
    {
        [SerializeField] private Vector2 _worldRangeExtension;
        [SerializeField] private List<Camera> _syncCameras = new List<Camera>();
        [field: SerializeField] public Camera Camera { get; private set; }
        [field: SerializeField] public Camera WorldCamera { get; private set; }
        private World _world;

        public Vector2 moveInput;
        public Vector2 WorldMousePos { get; private set; }
        
        [SerializeField] private Vector3 _desiredPosition;
        private float _desiredZoom;
        
        private const float DEFAULT_MOVE_SPEED = 25f;
        private const float FAST_CAM_MULTIPLIER = 1.6f;
        private const float LERP_SPEED = 10f;
        private const float ZOOM_LERP_SPEED = 6f;
        private const float ZOOM_CHANGE_SPEED = 1f;
        private const float MAX_ZOOM = 25f;
        private const float MIN_ZOOM = 2f;

        private void Start()
        {
            _desiredPosition = transform.position;
            _desiredZoom = Camera.orthographicSize;

            _world = World.Singleton;
        }

        private void Update()
        {
            Vector2 cameraSize = new Vector2(Camera.orthographicSize * Camera.aspect, Camera.orthographicSize);
            float speed = (DEFAULT_MOVE_SPEED * (GameControls.FastCam.IsHeld ? FAST_CAM_MULTIPLIER : 1f));
            Vector2 input = moveInput.normalized;
            Vector3 moveVector = new Vector3(input.x * speed * Time.unscaledDeltaTime, 
                input.y * speed * Time.unscaledDeltaTime);
            _desiredPosition += moveVector;

            float minX = -_world.WidthExtents - _worldRangeExtension.x;
            float maxX = _world.WidthExtents + _worldRangeExtension.x;
            float minY = -_world.HeightExtents - _worldRangeExtension.y;
            float maxY = _world.HeightExtents + _worldRangeExtension.y;
            
            float minCamX = minX + cameraSize.x;
            float maxCamX = maxX - cameraSize.x;
            float minCamY = minY + cameraSize.y;
            float maxCamY = maxY - cameraSize.y;
            
            _desiredPosition = new Vector3(Mathf.Clamp(_desiredPosition.x, minCamX, maxCamX),
                Mathf.Clamp(_desiredPosition.y, minCamY, maxCamY), _desiredPosition.z);
            
            transform.position = Vector3.Lerp(transform.position, _desiredPosition, LERP_SPEED * Time.unscaledDeltaTime);
            
            if (!CustomUI.MouseOverUI)
                _desiredZoom += -Input.mouseScrollDelta.y * ZOOM_CHANGE_SPEED;
            float verticalRange = (maxY - minY) / 2f;
            float horizontalRange = (maxX - minX) / 2f;
            _desiredZoom = Mathf.Clamp(_desiredZoom, 
                MIN_ZOOM, Mathf.Min
            (MAX_ZOOM,verticalRange));
            float orthographicSize = Mathf.Lerp(Camera.orthographicSize, _desiredZoom, 
                ZOOM_LERP_SPEED * Time.unscaledDeltaTime);

            Camera.orthographicSize = orthographicSize;

            foreach (Camera camera in _syncCameras)
            {
                camera.orthographicSize = orthographicSize;
            }
            
            WorldMousePos = Camera.ScreenToWorldPoint(Input.mousePosition);
        }

        public void ForceSetCameraTargetPos(Vector2 target)
        {
            _desiredPosition = target.SetZ(_desiredPosition.z);
        }

        public bool IsPointOnScreen(Vector2 point, bool isWorldSpacePoint)
        {
            Vector2 screenPoint = isWorldSpacePoint ? Utils.WorldtoScreenPoint(point) : point;
            return screenPoint.x >= 0f && screenPoint.x <= Screen.width && screenPoint.y >= 0f && screenPoint.y
                <= Screen.height;
        }
        
        public bool IsRectOnScreen(Vector2 position, Vector2 size, bool isWorldSpace)
        {
            Vector2 screenPosition = isWorldSpace ? Utils.WorldtoScreenPoint(position) : position;
            Camera cam = Camera.main;
            Vector2 screenSize = isWorldSpace
                ? new Vector2( Screen.width * size.x / (cam.aspect * cam.orthographicSize * 2f), Screen.height * size.y /
                                                                                                (cam.orthographicSize * 2f))
                : size;

            Rect rect = new Rect(Vector2.zero, screenSize);
            rect.center = screenPosition;
            Rect screenRect = new Rect(Vector2.zero, 
                new Vector2(Screen.width, Screen.height));
            
            return rect.Overlaps(screenRect);
        }
    }
}