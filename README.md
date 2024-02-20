The idea is to have a class that can dependency inject classes throughout your codebase. 

How to use : 

```c#
ServiceManager.Instance.Get<YourClassHere>(optionalConstructorFunction);
```

If the class you want to dependency inject doesn't exist yet, it will be created by the service manager. If it does exist, then the existing instance will be returned. 

The ServiceManager class depends on the Singleton<T> class, that is also reusable.

This codebase is reliant on the UnityEngine library.
