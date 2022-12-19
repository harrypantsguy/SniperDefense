
namespace _Project.Codebase
{
    public class Player : MonoSingleton<Player>
    {
        public Gun gun;
        public bool fullAuto;
        private void Update()
        {
            if (GameControls.DecreaseGameSpeed.IsPressed)
                TimeController.TimeScale -= .125f;
            if (GameControls.IncreaseGameSpeed.IsPressed)
                TimeController.TimeScale += .125f;
            
            gun.targetPos = Utils.WorldMousePos;

            if (GameControls.Fire.IsPressed && !fullAuto || GameControls.Fire.IsHeld && fullAuto)
            {
                gun.Fire();
            }
            
            if (GameControls.Reload.IsPressed)
                gun.StartReload();
        }
    }
}