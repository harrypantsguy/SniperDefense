using UnityEngine;

namespace _Project.Codebase.UI
{
    public class RectPositionPropertyAssigner : ShaderPropertyAssigner
    {
        [SerializeField] private RectTransform _rectTransform;
        protected override void UpdateValue(in string propertyName, in Material material)
        {
            if (_rectTransform != null)
                material.SetVector(propertyName, _rectTransform.position);
        }
    }
}