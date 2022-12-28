using System.Collections.Generic;
using _Project.Codebase.Enemies;
using UnityEngine;

namespace _Project.Codebase
{
    public abstract class Entity : MonoBehaviour
    {
        public readonly List<EntityComponent> entityComponents = new List<EntityComponent>();

        protected virtual void Update()
        {
            foreach (EntityComponent component in entityComponents)
                component.Update(Time.deltaTime);
        }
    }
}