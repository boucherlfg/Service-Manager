using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceManager.Builders;

namespace ServiceManager.Builders
{
    /// <summary>
    /// a builder that creates an instance from the default constructor
    /// </summary>
    public class DefaultConstructorBuilder : Builder
    {
        public DefaultConstructorBuilder() : base(typeof(object))
        {
        }

        public override object Build()
        {
            var ctor = Type.GetConstructor(new Type[] { });

            if (ctor != null) return ctor.Invoke(new object[] { });

            throw new Exception($"{Type.Name} doesn't contain a parameterless constructor");
        }
        public override bool CanBuild(Type type)
        {
            return base.CanBuild(type)
                && type.GetConstructor(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public, new Type[] { }) != null;
        }
    }
}
