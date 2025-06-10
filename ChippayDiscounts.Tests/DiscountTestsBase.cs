using System.Reflection;
using ChippayDiscounts.Core;

namespace ChippayDiscounts.Tests;

public class DiscountTestsBase
{
    protected static IEnumerable<IDiscountStrategy> GetDiscountStrategies()
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(SafeGetTypes)
            .Where<Type>(t => typeof(IDiscountStrategy).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })
            .Select(TryCreate)!
            .Where<object>(x => x != null!)!
            .Cast<IDiscountStrategy>();
    }

    private static IEnumerable<Type> SafeGetTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException err)
        {
            // Get the types that are actually loaded
            return err.Types.Where(t => t != null)!;
        }
    }

    private static object? TryCreate(Type type)
    {
        var constructor = type.GetConstructor(Type.EmptyTypes);
        return constructor != null ? constructor.Invoke(null) : null;
    }

}