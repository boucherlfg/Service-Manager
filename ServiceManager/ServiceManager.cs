using System;
using System.Collections.Generic;

namespace ServiceManager
{
    public class ServiceManager
    {
        private readonly List<object> _services;
        public ServiceManager()
        {
            _services = new();
        }

        /// <summary>
        /// returns the service instance corresponding to given class type
        /// </summary>
        /// <typeparam name="T">a class type</typeparam>
        /// <param name="builder">an override for generating the type</param>
        /// <returns>an object of type T</returns>
        public T Get<T>() where T : class
        {
            return Get(GetDefaultConstructorObject<T>);
        }

        /// <summary>
        /// returns the service instance corresponding to given class type
        /// </summary>
        /// <typeparam name="T">a class type</typeparam>
        /// <param name="builder">an override for generating the type</param>
        /// <returns>an object of type T</returns>
        /// <exception cref="Exception">returns an error if there is no service of type T found, and createIfInexistant is false</exception>
        public T Get<T>(Func<T> builder) where T : class
        {
            var srv = _services.Find(t => t is T);
            if (srv != null) return (T)srv;

            return Add(builder);

            throw new Exception($"No service of type {typeof(T).Name} was found");
        }

        public T Add<T>() where T : class
        {
            return Add<T>(GetDefaultConstructorObject<T>);
        }

        public T Add<T>(Func<T> builderOverride) where T : class
        {
            var srv = _services.Find(t => t is T);
            if (srv != null) return (T)srv;

            srv = builderOverride.Invoke();
            _services.Add(srv);
            return (T)srv;
        }

        /// <summary>
        /// checks if a service instance of given type exists
        /// </summary>
        /// <typeparam name="T">a class type</typeparam>
        /// <returns>True if a service instance of type T exists. False otherwise</returns>
        public bool Has<T>() => _services.Exists(srv => srv is T);

        private T GetDefaultConstructorObject<T>()
        {
            var type = typeof(T);
            var ctor = type.GetConstructor(new Type[] { });

            if (ctor != null) return (T)ctor.Invoke(new object[] { });

            throw new Exception($"{type.Name} doesn't contain a parameterless constructor");
        }
    }
}