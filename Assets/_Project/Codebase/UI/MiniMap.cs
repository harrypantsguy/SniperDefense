using System.Collections.Generic;
using _Project.Codebase.Enemies;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Codebase.UI
{
    public class MiniMap : CustomUI
    {
        [SerializeField] private Image _groundImage;
        [SerializeField] private Image _cameraBoundsImage;
        [SerializeField] private UILineRenderer _aimDirectionLine;
        [SerializeField] private GameObject _enemyIconPrefab;
        
        public float scaleFactor;
        public float globalIconScaleFactor = 1f;
        
        private readonly Dictionary<Transform, MiniMapIcon> _icons = new Dictionary<Transform, MiniMapIcon>();
        private Vector2 _miniMapOffset;
        private bool _zoomedLastFrame;
        private Vector2 _oldMousePos;
        private bool _dragging;
        private bool _settingCameraPos;
        private RectTransform _rectTransform;

        private Camera _camera;
        private World _world;
        
        private float PerfectFitScaleFactor =>  _rectTransform.sizeDelta.x / _world.width;
        
        protected override void Start()
        {
            base.Start();
            _rectTransform = GetComponent<RectTransform>();
            _camera = CameraController.Singleton.Camera;
            _world = World.Singleton;
            
            CenterMap();
            
            Enemy.NewEnemyEvent += AddNewEnemy;
            Enemy.RemoveEnemyEvent += RemoveEnemy;
        }

        private void OnDestroy()
        {
            Enemy.NewEnemyEvent -= AddNewEnemy;
            Enemy.RemoveEnemyEvent -= RemoveEnemy;
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            
            if (GameControls.CenterMap.IsPressed)
                CenterMap();

            if (GameControls.MoveMiniMap.IsPressed && MouseInside)
            {
                _dragging = true;
            }
            else if (!GameControls.MoveMiniMap.IsHeld && _dragging)
                _dragging = false;

            if (_dragging)
                _miniMapOffset += Player.Singleton.MouseDelta;

            float canvasScaleFactor = _groundImage.canvas.scaleFactor;
            
            Matrix4x4 transformationMatrix = Matrix4x4.TRS(_groundImage.rectTransform.position,
                Quaternion.identity, new Vector3(scaleFactor * canvasScaleFactor, 
                    scaleFactor * canvasScaleFactor, 1f));

            float scrollDelta = Input.mouseScrollDelta.y;
            float newScaleFactor = scaleFactor;
            if (Mathf.Abs(scrollDelta) > .0001f && MouseInside)
            {
                if (scrollDelta > 0f || GameControls.MinimapZoomIn.IsPressed)
                    newScaleFactor += 1f;
                if (scrollDelta < 0f || GameControls.MinimapZoomOut.IsPressed)
                    newScaleFactor -= 1f;
            }
            
            Vector2 oldLocalMousePos = GetRelativeMousePos();

            float oldScaleFactor = scaleFactor;
            scaleFactor = Mathf.Max(newScaleFactor, PerfectFitScaleFactor);
            UpdateGroundImage();

            Vector2 scaledOldMousePos = oldLocalMousePos * (1f - (scaleFactor - oldScaleFactor) / scaleFactor);
            //Vector2 newLocalMousePos = GetRelativeMousePos();
            Vector2 mousePosDelta = scaledOldMousePos - oldLocalMousePos;
            _miniMapOffset += mousePosDelta * new Vector2(_world.width * scaleFactor, _world.height * scaleFactor);

            UpdateGroundImage();

            Matrix4x4 cameraTranslate = transformationMatrix * _camera.transform.localToWorldMatrix;
            
            _cameraBoundsImage.rectTransform.position = cameraTranslate.GetPosition();
            _cameraBoundsImage.rectTransform.sizeDelta = new Vector2(cameraTranslate.lossyScale.x * 
                                                                     _camera.orthographicSize * _camera.aspect * 2f / canvasScaleFactor,
                cameraTranslate.lossyScale.y * _camera.orthographicSize * 2f / canvasScaleFactor);

            if (GameControls.MoveCameraToMiniMapLocation.IsPressed && MouseInside)
            {
                _settingCameraPos = true;
            }
            else if (!GameControls.MoveCameraToMiniMapLocation.IsHeld)
            {
                _settingCameraPos = false;
            }

            if (_settingCameraPos && MouseInside)
            {
                Vector2 worldMousePosOnMinimap = transformationMatrix.inverse
                    .MultiplyPoint(Input.mousePosition);
                
                CameraController.Singleton.ForceSetCameraTargetPos(worldMousePosOnMinimap);
            }

            if (_aimDirectionLine != null)
            {
                Vector2 minimapPlayerPos = (transformationMatrix * Player.Singleton.transform.localToWorldMatrix).GetPosition();
                Vector2 targetPos = Player.Singleton.gun.TargetPosAtRange;
                Vector2 minimapRangeEnd = transformationMatrix.MultiplyPoint(targetPos);
                _aimDirectionLine.points = new List<Vector2>
                {
                    minimapPlayerPos * canvasScaleFactor,
                    minimapRangeEnd * canvasScaleFactor
                };
                _aimDirectionLine.UpdateGraphic();
            }

            foreach ((Transform iconTransform, MiniMapIcon icon) in _icons)
            {
                icon.UpdateIcon(transformationMatrix, globalIconScaleFactor);
            }
        }

        public void CenterMap()
        {
            scaleFactor = PerfectFitScaleFactor;
            _miniMapOffset.y = 0f;//-rectTransform.sizeDelta.y / 2f;
            _miniMapOffset.x = 0f;
            UpdateGroundImage();
        }

        private void UpdateGroundImage()
        {
            _groundImage.rectTransform.sizeDelta = new Vector2(_world.width * scaleFactor,
                _world.height * scaleFactor);
            _groundImage.rectTransform.localPosition = _miniMapOffset;
        }

        private Vector2 GetRelativeMousePos()
        { 
            Vector2 localPos = (Input.mousePosition - _groundImage.rectTransform.position);
            Vector2 scaledLocalPos = localPos / new Vector2(_world.width * scaleFactor, _world.height * scaleFactor); 
            return scaledLocalPos;
        }

        private void AddNewEnemy(Enemy newEnemy)
        {
            MiniMapIcon newIcon = Instantiate(_enemyIconPrefab, _groundImage.transform).GetComponent<MiniMapIcon>();
            newIcon.targetTransform = newEnemy.transform;
            newIcon.scale = .0875f;
            _icons.Add(newEnemy.transform, newIcon);
        }

        private void RemoveEnemy(Enemy newEnemy)
        {
            MiniMapIcon icon = _icons[newEnemy.transform];
            Destroy(icon.gameObject);
            _icons.Remove(newEnemy.transform);
        }
    }
}