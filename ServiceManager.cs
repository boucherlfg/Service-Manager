using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceManager : Singleton<ServiceManager>
{
    public enum GetBehaviour
    {
        Create,
        Error
    }
    public GetBehaviour Behaviour { get; set; } = GetBehaviour.Create;

    private readonly List<object> _systems = new();

    public T Get<T>(Func<T> generator = null) where T : class
    {
        var sys = _systems.Find(t => t is T);
        if (sys != null) return sys as T;

        return Behaviour switch
        {
            GetBehaviour.Create => Add<T>(generator),
            GetBehaviour.Error => throw new Exception("This system was not previously registered"),
            _ => throw new NotImplementedException()
        };
    }

    public T Add<T>(Func<T> generator = null) where T : class
    {
        var sys = _systems.Find(t => t is T);
        if (sys != null) return sys as T;

        generator ??= GetDefaultObject<T>;
        sys = generator?.Invoke();
        _systems.Add(sys);
        return sys as T;
    }

    private T GetDefaultObject<T>() where T : class
    {
        var type = typeof(T);
        if (type.IsSubclassOf(typeof(Component)))
        {
            return GetDefaultFromGameObject(type) as T;
        }
        return GetDefaultFromCtor(type) as T;
    }

    private object GetDefaultFromGameObject(Type type)
    {
        Component component = UnityEngine.Object.FindObjectOfType(type) as Component;
        if (component != default) return component;
        return Behaviour switch
        {
            GetBehaviour.Create => CreateObject(),
            GetBehaviour.Error => throw new Exception("This component doesn't exist in this scene."),
            _ => throw new NotImplementedException()
        };

        Component CreateObject()
        {
            var gameObject = new GameObject(type.Name);
            var component = gameObject.AddComponent(type);
            _systems.Add(component);
            return component;
        }
    }

    private object GetDefaultFromCtor(Type type)
    {
        var ctor = type.GetConstructor(new Type[] { });
        return ctor == null
            ? throw new NullReferenceException($"{type.Name} doesn't contain a parameterless constructor")
            : ctor.Invoke(new object[] { });
    }
}
