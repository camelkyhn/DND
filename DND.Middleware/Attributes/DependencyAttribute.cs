using System;

namespace DND.Middleware.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SingletonDependencyAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ScopedDependencyAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TransientDependencyAttribute : Attribute
    {
    }
}