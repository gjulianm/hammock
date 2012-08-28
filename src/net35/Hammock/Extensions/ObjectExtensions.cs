using System;
using System.Reflection;

#if NETCF || METRO
using System.Linq;
#endif

namespace Hammock.Extensions
{
    internal static class ObjectExtensions
    {

        public static bool Implements(this object instance, Type interfaceType)
        {
#if !METRO
            return interfaceType.IsGenericTypeDefinition
                       ? instance.ImplementsGeneric(interfaceType)
                       : interfaceType.IsAssignableFrom(instance.GetType());
#else
            return interfaceType.GetTypeInfo().IsGenericTypeDefinition
                ? instance.ImplementsGeneric(interfaceType)
                : interfaceType.GetTypeInfo().IsAssignableFrom(instance.GetType().GetTypeInfo());
#endif
        }

        private static bool ImplementsGeneric(this Type type, Type targetType)
        {
#if METRO
            var interfaceTypes = type.GetTypeInfo().ImplementedInterfaces;
#else
            var interfaceTypes = type.GetInterfaces();
#endif
            foreach (var interfaceType in interfaceTypes)
            {
#if METRO
                if (!interfaceType.GetTypeInfo().IsGenericType)
#else
                if(!interfaceType.IsGenericType)
#endif
                {
                    continue;
                }

                if (interfaceType.GetGenericTypeDefinition() == targetType)
                {
                    return true;
                }
            }

#if METRO
            var baseType = type.GetTypeInfo().BaseType;
#else
            var baseType = type.BaseType;
#endif
            if (baseType == null)
            {
                return false;
            }

#if METRO
            return baseType.GetTypeInfo().IsGenericType
#else
            return baseType.IsGenericType
#endif
                       ? baseType.GetGenericTypeDefinition() == targetType
                       : baseType.ImplementsGeneric(targetType);
        }

        private static bool ImplementsGeneric(this object instance, Type targetType)
        {
            return instance.GetType().ImplementsGeneric(targetType);
        }

        public static Type GetDeclaredTypeForGeneric(this object instance, Type interfaceType)
        {
            return instance.GetType().GetDeclaredTypeForGeneric(interfaceType);
        }

        public static Type GetDeclaredTypeForGeneric(this Type baseType, Type interfaceType)
        {
            var type = default(Type);

            if (baseType.ImplementsGeneric(interfaceType))
            {
#if NETCF
                var generic = baseType.GetInterfaces()
                    .Single(i => i.FullName.Equals(interfaceType.FullName));
#elif METRO
                var generic = baseType.GetTypeInfo().ImplementedInterfaces.
                    Single(x => x.FullName.Equals(interfaceType.FullName)).GetTypeInfo();
#else
                var generic = baseType.GetInterface(interfaceType.FullName, true);
#endif
                if (generic.IsGenericType)
                {
#if !METRO
                    var args = generic.GetGenericArguments();
#else
                    var args = generic.GenericTypeParameters;
#endif
                    if (args.Length == 1)
                    {
                        type = args[0];
                    }
                }
            }

            return type;
        }
    }
}