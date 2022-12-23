using System;
using System.Collections.Generic;

namespace DanonsTools.DependencyInjectionLayer
{
    public sealed class DependencyContainer
    {
        private readonly Dictionary<Type, object> _bindings = new Dictionary<Type, object>();

        public void Bind<I, T>(in T implementation) where T : I
        {
            var bindInterface = typeof(I);

            if (_bindings.ContainsKey(bindInterface))
                throw new Exception($"Cannot bind {bindInterface} multiple times.");
            
            _bindings.Add(bindInterface, implementation);
        }

        public I Resolve<I>()
        {
            var bindInterface = typeof(I);

            if (!_bindings.ContainsKey(bindInterface))
                throw new Exception($"No implementation bound to {bindInterface}.");

            return (I)_bindings[bindInterface];
        }

        public void ClearBindings()
        {
            _bindings.Clear();
        }
    }
}