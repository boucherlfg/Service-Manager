using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManager.Builders
{
    /// <summary>
    /// a base class for creating builders for the service manager
    /// </summary>
    public abstract class Builder
    {
        /// <summary>
        /// the builder's target type, which is set in constructor
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// constructor for choosing target type for the builder
        /// </summary>
        /// <param name="type">target type of the builder</param>
        protected Builder(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// returns a new instance of an object of target type
        /// </summary>
        /// <returns>an instance of type Type</returns>
        public abstract object Build();

        /// <summary>
        /// checks if it is possible to build an object of given type with this builder
        /// </summary>
        /// <param name="type">the type upon which we want to check</param>
        /// <returns>true if we can build an instance of given type with this builder. False otherwise.</returns>
        public virtual bool CanBuild(Type type)
        {
            return type == Type || type.IsSubclassOf(Type);
        }
    }
}
