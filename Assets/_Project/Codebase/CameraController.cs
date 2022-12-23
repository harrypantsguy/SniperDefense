using _Project.Codebase.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Codebase
{
    public class CameraController : MonoSingleton<CameraController>
    {
        [SerializeField] private Vector2 _worldRangeExtension;
        public Camera Camera { get; private set; }
        private World _world;

        public Vector2 moveInput;
        
        private Vector3 _desiredPosition;
        private float _desiredZoom;

        private const float DEFAULT_MOVE_SPEED = 25f;
        private const float FAST_CAM_MULTIPLIER = 1.6f;
        private const float LERP_SPEED = 10f;
        private const float ZOOM_LERP_SPEED = 6f;
        private const float ZOOM_CHANGE_SPEED = 1f;
        private const float MAX_ZOOM = 25f;
        private const float MIN_ZOOM = 2f;

        protected override void Awake()
        {
            base.Awake();
            Camera = GetComponent<Camera>();
        }

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
            Vector3 moveVector = new Vector3(moveInput.x * speed * Time.unscaledDeltaTime, 
                moveInput.y * speed * Time.unscaledDeltaTime);
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
            Camera.orthographicSize = Mathf.Lerp(Camera.orthographicSize, _desiredZoom, 
                ZOOM_LERP_SPEED * Time.unscaledDeltaTime);
        }

        public void ForceSetCameraTargetPos(Vector2 target)
        {
            _desiredPosition = target.SetZ(_desiredPosition.z);
        }
    }
}