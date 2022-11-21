using UnityEngine;

namespace _Project.Codebase
{
    public class Player : MonoSingleton<Player>
    {
        [SerializeField] private Gun _gun;
        private void Update()
        {
            _gun.targetPos = Utils.WorldMousePos;
        }
    }
}