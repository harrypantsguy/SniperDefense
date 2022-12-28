using UnityEngine;

namespace _Project.Codebase.UI
{
    public class ProceduralUI : MonoBehaviour
    {
        private RectTransform _rectTransform;
        
        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            
            //Material material = new Material("");
        }
    }

    public class UIReference<T> 
    {
        public string propertyName;
        public GameObject gameObject;
    }
}