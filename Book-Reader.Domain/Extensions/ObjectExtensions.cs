using System;
using System.Linq;

namespace Book_Reader.Domain.Extensions
{
    public static class ObjectExtensions
    {
        public static bool HasImplementedRawGeneric(this Type type, Type generic)
        {
            // Traverse all interfaces implemented by the type to determine whether there is an interface that is generic and is an instance of the original generic specified in the parameter.
            return type.GetInterfaces().Any(x => generic == (x.IsGenericType ? x.GetGenericTypeDefinition() : x));
        }
    }
}
