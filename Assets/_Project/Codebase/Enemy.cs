using UnityEngine;

namespace _Project.Codebase
{
    public class Enemy : MonoBehaviour
    {
        private void FixedUpdate()
        {
            transform.position -= new Vector3(5f * Time.fixedDeltaTime, 0f, 0f);
        }
    }
}