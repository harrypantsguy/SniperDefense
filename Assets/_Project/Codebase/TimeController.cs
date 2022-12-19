using UnityEngine;

namespace _Project.Codebase
{
    public class TimeController
    {
        private const float MIN_TIMESCALE = .125f;
        public static float TimeScale
        {
            get => Time.timeScale;
            set => Time.timeScale = Mathf.Max(value, MIN_TIMESCALE);
        }
    }
}