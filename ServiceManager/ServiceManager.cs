using System;
using System.Collections.Generic;
using ServiceManager.Builders;

namespace ServiceManager
{
    public class ServiceManager
    {
        private readonly List<object> _services;
        private readonly List<Builder> _builders;
        public ServiceManager()
        {
            _services = new();
            _builders = new();
        }

        /// <summary>
        /// a list of builders for creating service instances
        /// </summary>
        public List<Builder> Builders => _builders;

        /// <summary>
        /// returns the service instance corresponding to given class type
        /// </summary>
        /// <typeparam name="T">a class type</typeparam>
        /// <param name="builderOverride">an override for generating the type</param>
        /// <param name="createIfInexistant">specifies if the type should be created if it does not already exists</param>
        /// <returns>an object of type T</returns>
        /// <exception cref="Exception">returns an error if there is no service of type T found, and createIfInexistant is false</exception>
        public T Get<T>(Func<T> builderOverride = null, bool createIfInexistant = true) where T : class
        {
            var srv = _services.Find(t => t is T);
            if (srv != null) return (T)srv;

            if (createIfInexistant) return Add(builderOverride);

            throw new Exception($"No service of type {typeof(T).Name} was found");
        }

        /// <summary>
        /// creates and return a unique service instance of given class type
        /// </summary>
        /// <typeparam name="T">a class type</typeparam>
        /// <param name="builderOverride">an override for generating the type</param>
        /// <returns>an object of type T</returns>
        public T Add<T>(Func<T> builderOverride = null) where T : class
        {
            var srv = _services.Find(t => t is T);
            if (srv != null) return (T)srv;

            builderOverride ??= () => GetBuilderForType<T>();
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

        private T GetBuilderForType<T>() where T : class
        {
            var type = typeof(T);

            var builder = Builders.Find(builder => builder.CanBuild(type));
            if (builder != null) return (T)builder.Build();

            throw new Exception($"Cannot find a suitable Builder for type {type.Name}");
        }
    }
}