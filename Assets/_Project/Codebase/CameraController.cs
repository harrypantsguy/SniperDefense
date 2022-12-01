using UnityEngine;

namespace _Project.Codebase
{
    public class CameraController : MonoBehaviour
    {
        private Camera _camera;

        private Vector3 _desiredPosition;
        
        private const float DEFAULT_MOVE_SPEED = 25f;
        private const float LERP_SPEED = 10f;
        
        private void Start()
        {
            _camera = GetComponent<Camera>();
            _desiredPosition = transform.position;
        }

        private void Update()
        {
            Vector2 inputs = GameControls.DirectionalInput;
            Vector3 moveVector = new Vector3(inputs.x * DEFAULT_MOVE_SPEED * Time.deltaTime, 0f);
            _desiredPosition += moveVector;
            transform.position = Vector3.Lerp(transform.position, _desiredPosition, LERP_SPEED * Time.deltaTime);
        }
    }
}