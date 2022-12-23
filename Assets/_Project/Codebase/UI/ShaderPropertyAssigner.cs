using JetBrains.Annotations;
using UnityEngine;

namespace _Project.Codebase.UI
{
    [ExecuteAlways]
    public abstract class ShaderPropertyAssigner : MonoBehaviour
    {
        [SerializeField] private string _propertyName;
        [SerializeField] private Material _material;

        [UsedImplicitly]
        private void LateUpdate()
        {
            if (_material != null)
                UpdateValue(_propertyName, _material);
        }
        
        protected abstract void UpdateValue(in string propertyName, in Material material);
    }
}