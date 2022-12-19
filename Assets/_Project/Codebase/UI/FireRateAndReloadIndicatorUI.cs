using UnityEngine;

namespace _Project.Codebase.UI
{
    public class FireRateAndReloadIndicatorUI : MonoBehaviour
    {
        [SerializeField] private FillableBarUI _reloadBar;
        [SerializeField] private FillableBarUI _fireRateBar;
        [SerializeField] private FillableBarUI _ammoBar;

        private Player _player;

        private void Start()
        {
            _player = Player.Singleton;
        }

        private void Update()
        {
            float ammoBarFillAmount = _player.gun.reloading ? (_player.gun.ReloadProgress) : 
                (float) _player.gun.bulletsInMag / _player.gun.magSize;
            _ammoBar.SetFillAmount(ammoBarFillAmount);

            _reloadBar.ImageTransform.position = Input.mousePosition;
            _reloadBar.SetFillAmount(_player.gun.FireDelayProgress);
            //_fireRateBar.SetFillAmount(_player.gun.FireDelayProgress);
            //_reloadBar.BarEnabled = _player.gun.reloading;
            //_reloadBar.SetFillAmount(_player.gun.ReloadProgress);
        }
    }
}