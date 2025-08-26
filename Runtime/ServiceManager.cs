using System;
using System.Collections.Generic;

public class ServiceManager : Singleton<ServiceManager>
{
    private List<object> _services = new List<object>();

    public T Get<T>(Func<T> generator) where T : class, new()
    {
        generator ??= () => new T();
        var srv = _services.Find(x => x is T);
        if (srv != null) return srv as T;
        
        srv = generator();
        _services.Add(srv);
        return (T)srv;
    } 
}
