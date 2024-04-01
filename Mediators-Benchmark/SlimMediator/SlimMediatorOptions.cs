using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public class SlimMediatorOptions
{
    public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;
    internal List<Assembly> AssembliesToRegister { get; } = [];

    public SlimMediatorOptions RegisterServicesFromAssembly(Assembly assembly)
    {
        AssembliesToRegister.Add(assembly);
        return this;
    }
}