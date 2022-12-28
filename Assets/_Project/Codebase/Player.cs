using _Project.Codebase.UI;
using UnityEngine;

namespace _Project.Codebase
{
    public class Player : MonoSingleton<Player>
    {
        public Gun gun;
        public bool fullAuto;

        public Vector2 MouseDelta { get; private set; }

        private Vector2 _oldMousePos;
        private void Update()
        {
            if (GameControls.DecreaseGameSpeed.IsPressed)
                TimeController.TimeScale -= .125f;
            if (GameControls.IncreaseGameSpeed.IsPressed)
                TimeController.TimeScale += .125f;
            
            gun.targetPos = Utils.WorldMousePos;
            
            if ((GameControls.Fire.IsPressed && !fullAuto || GameControls.Fire.IsHeld && fullAuto) && !CustomUI.MouseOverUI)
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

            Vector2 moveInput = GameControls.DirectionalInput;
            moveInput = moveInput.SetY(movingCharacter ? 0f : moveInput.y);
            CameraController.Singleton.moveInput = moveInput;
            
            MouseDelta = (Vector2)Input.mousePosition - _oldMousePos;
        }

        private void LateUpdate()
        {
            _oldMousePos = Input.mousePosition;
        }
    }
}