#if UNITY_EDITOR
using UnityEngine;

namespace ServiceManagement
{
    internal class UnityComponentBuilder : Builder
    {
        public UnityComponentBuilder() : base(typeof(Component))
        {
        }

        public override object Build()
        {
            Component component = GameObject.FindObjectOfType(Type) as Component;
            if (component != default) return component;
            return CreateObject();

            Component CreateObject()
            {
                var gameObject = new GameObject(Type.Name);
                var component = gameObject.AddComponent(Type);
                return component;
            }
        }
    }
}
#endif