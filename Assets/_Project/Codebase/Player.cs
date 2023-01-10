using _Project.Codebase.UI;
using UnityEngine;

namespace _Project.Codebase
{
    public class Player : MonoSingleton<Player>
    {
        public Gun gun;
        public bool fullAuto;

        public Vector2 MouseDelta { get; private set; }
        private bool _pressedFireNotOverUI;

        private Vector2 _oldMousePos;
        private void Update()
        {
            if (GameControls.DecreaseGameSpeed.IsPressed)
                TimeController.TimeScale -= .125f;
            if (GameControls.IncreaseGameSpeed.IsPressed)
                TimeController.TimeScale += .125f;

            gun.targetPos = CameraController.Singleton.WorldMousePos;

            if (!CustomUI.MouseOverUI && GameControls.Fire.IsPressed)
            {
                _pressedFireNotOverUI = true;
            }
            else if (!GameControls.Fire.IsHeld)
                _pressedFireNotOverUI = false;
            
            if (GameControls.Fire.IsPressed && !fullAuto && !CustomUI.MouseOverUI || GameControls.Fire.IsHeld && fullAuto && _pressedFireNotOverUI)
            {
                gun.Fire();
            }
            
            if (GameControls.Reload.IsPressed)
                gun.StartReload();

            bool movingCharacter = GameControls.SetMoveModeToCharacter.IsHeld;
            
            if (movingCharacter)
            {
                transform.position += new Vector3(0f, GameControls.DirectionalInput.y * 5f * Time.deltaTime);
                transform.position = transform.position.SetY(Mathf.Clamp(transform.position.y, -World.Singleton
                    .HeightExtents, World.Singleton.HeightExtents));
            }
            
            transform.position = transform.position.SetX(-(World.Singleton.WidthExtents + 1f));

            Vector2 moveInput = GameControls.DirectionalInput;
            moveInput = moveInput.SetY(movingCharacter ? 0f : moveInput.y);
            CameraController.Singleton.moveInput = moveInput.normalized;
            
            MouseDelta = (Vector2)Input.mousePosition - _oldMousePos;
        }

        private void LateUpdate()
        {
            _oldMousePos = Input.mousePosition;
        }
    }
}