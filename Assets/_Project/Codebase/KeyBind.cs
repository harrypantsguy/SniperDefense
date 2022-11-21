using UnityEngine;

namespace _Project.Codebase
{
    public struct KeyBind
    {
        public KeyCode primaryKey;
        public KeyCode altKey;
        public bool IsPressed => Input.GetKeyDown(primaryKey) || Input.GetKeyDown(altKey);
        public bool IsHeld => Input.GetKey(primaryKey) || Input.GetKey(altKey);
        public bool IsReleased => Input.GetKeyUp(primaryKey) || Input.GetKey(altKey);

        public KeyBind(KeyCode primaryKey, KeyCode altKey = KeyCode.None)
        {
            this.primaryKey = primaryKey;
            this.altKey = altKey;
        }
    }
}