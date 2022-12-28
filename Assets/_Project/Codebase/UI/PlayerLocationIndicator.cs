using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Codebase.UI
{
    public class PlayerLocationIndicator : MonoBehaviour
    {
        [SerializeField] private RawImage _image;
        [SerializeField] private Image _arrowImage;

        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            bool onScreen = CameraController.Singleton.IsRectOnScreen(Player.Singleton.transform.position, new
                Vector2(2f, 1f), true);
            
            Vector2 screenSpacePlayerPos = Utils.WorldtoScreenPoint(Player.Singleton.transform.position);
            Vector2 screenSpaceGunTargetPos = Utils.WorldtoScreenPoint(Player.Singleton.gun.targetPos);
            Vector2 screenSpaceGunDir = (screenSpaceGunTargetPos - screenSpacePlayerPos).normalized;
            
            //transform.localPosition = new Vector3(-Screen.width / 2f + 100f, screenSpacePlayerPos.y - Screen.height / 2f);
            
            float distToScreenEdge = Vector2.Distance(screenSpacePlayerPos, new Vector2(0f, Screen.height / 2f));

            transform.localPosition = screenSpacePlayerPos + screenSpaceGunDir * distToScreenEdge - Utils.ScreenCenter;

            if (_arrowImage != null)
                _arrowImage.transform.right = screenSpaceGunDir;
            
            //Vector2 dirToPlayerFromCenter = (screenSpacePlayerPos - Utils.ScreenCenter).normalized;

            //transform.localPosition =
            //    new Vector2(dirToPlayerFromCenter.x * Screen.width / 2f, dirToPlayerFromCenter.y * Screen.height / 2f);
            
            _image.enabled = !onScreen;
        }
    }
}