using DanonsTools.DependencyInjectionLayer;

namespace DanonsTools.ServiceLayer
{
    public static class ServiceLocator
    {
        private static DependencyContainer _container;

        public static void Initialize() => _container = new DependencyContainer();

        public static void Bind<I, T>(in T implementation) where T : I => _container.Bind<I, T>(implementation);

        public static T Retrieve<T>() => _container.Resolve<T>();

        public static void ClearBindings() => _container.ClearBindings();
    }
}