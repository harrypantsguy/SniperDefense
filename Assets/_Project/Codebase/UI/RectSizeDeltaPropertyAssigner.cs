using UnityEngine;

namespace _Project.Codebase.UI
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class RectSizeDeltaPropertyAssigner : ShaderPropertyAssigner
    {
        [SerializeField] private RectTransform _rectTransform;
        
        protected override void UpdateValue(in string propertyName, in Material material)
        {
            if (_rectTransform != null)
                material.SetVector(propertyName, _rectTransform.sizeDelta);
        }
    }
}