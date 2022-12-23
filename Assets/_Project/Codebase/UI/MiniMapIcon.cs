using UnityEngine;
using UnityEngine.UI;

namespace _Project.Codebase.UI
{
    public class MiniMapIcon : MonoBehaviour
    {
        public Transform targetTransform;
        public Image _image;
        public float scale;

        public void UpdateIcon(Matrix4x4 miniMapMatrix, float sizeMultiplier)
        {
            Matrix4x4 translate = miniMapMatrix * targetTransform.localToWorldMatrix;
            _image.transform.position = translate.GetPosition();
            _image.rectTransform.localScale = translate.lossyScale * (scale * sizeMultiplier);
        }
    }
}