using UnityEngine;

namespace _Project.Codebase.UI
{
    public class CustomUI : MonoBehaviour
    {
        protected RectTransform rectTransform;

        public static int MouseOverUICount;
        public static bool MouseOverUI => MouseOverUICount > 0;
        protected bool MouseInside { get; private set; }

        protected virtual void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetMouseOverUICount()
        {
            MouseOverUICount = 0;
        }
        
        protected virtual void Update() {}

        protected virtual void LateUpdate()
        {
            bool newMouseInside = CheckMouseInside();
            if (newMouseInside && !MouseInside)
            {
                MouseOverUICount++;
            }
            else if (MouseInside && !newMouseInside)
            {
                MouseOverUICount--;
            }

            MouseInside = newMouseInside;
        }
        
        private bool CheckMouseInside() => RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition);
    }
}