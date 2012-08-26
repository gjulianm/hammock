using System.Collections.Generic;
using System.Reflection;
using System.Linq;
namespace Hammock.Extensions
{
    internal static class ReflectionExtensions
    {
        public static IEnumerable<T> GetCustomAttributes<T>(this PropertyInfo info, bool inherit)
            where T : class
        {
            var attributes = info.GetCustomAttributes(typeof (T), inherit);
#if !METRO
            return attributes.ToEnumerable<T>();
#else
            return attributes.OfType<T>();
#endif
        }

        public static object GetValue(this object instance, string property)
        {
#if !METRO
            var info = instance.GetType().GetProperty(property);
#else
            var info = instance.GetType().GetRuntimeProperty(property);
#endif
            if (info != null)
            {
                var value = info.GetValue(instance, null);
                return value;
            }
            return null; 
        }

        public static void SetValue(this object instance, string property, object value)
        {
#if !METRO
            var info = instance.GetType().GetProperty(property);
#else
            var info = instance.GetType().GetRuntimeProperty(property);
#endif
            info.SetValue(instance, value, null);
        }
    }
}