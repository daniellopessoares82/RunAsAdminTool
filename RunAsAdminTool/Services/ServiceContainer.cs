using RunAsAdminTool.Services.Interfaces;

namespace RunAsAdminTool.Services;

/// <summary>
/// Simple service container for dependency injection.
/// </summary>
public class ServiceContainer
{
    private readonly Dictionary<Type, object> _services = [];

    /// <summary>
    /// Registers a service instance with the container.
    /// </summary>
    /// <typeparam name="TInterface">The interface type.</typeparam>
    /// <param name="implementation">The implementation instance.</param>
    public void RegisterSingleton<TInterface>(TInterface implementation) 
        where TInterface : class
    {
        _services[typeof(TInterface)] = implementation;
    }

    /// <summary>
    /// Registers a service factory with the container.
    /// </summary>
    /// <typeparam name="TInterface">The interface type.</typeparam>
    /// <param name="factory">The factory function to create instances.</param>
    public void RegisterSingleton<TInterface>(Func<ServiceContainer, TInterface> factory) 
        where TInterface : class
    {
        _services[typeof(TInterface)] = new Lazy<TInterface>(() => factory(this));
    }

    /// <summary>
    /// Retrieves a service instance from the container.
    /// </summary>
    /// <typeparam name="TInterface">The interface type.</typeparam>
    /// <returns>The service instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the service is not registered.</exception>
    public TInterface GetService<TInterface>() 
        where TInterface : class
    {
        if (!_services.TryGetValue(typeof(TInterface), out var service))
        {
            throw new InvalidOperationException($"Service of type {typeof(TInterface).Name} is not registered.");
        }

        return service switch
        {
            Lazy<TInterface> lazy => lazy.Value,
            TInterface instance => instance,
            _ => throw new InvalidOperationException($"Invalid service registration for type {typeof(TInterface).Name}.")
        };
    }

    /// <summary>
    /// Checks if a service is registered in the container.
    /// </summary>
    /// <typeparam name="TInterface">The interface type.</typeparam>
    /// <returns>True if the service is registered; otherwise, false.</returns>
    public bool IsRegistered<TInterface>() 
        where TInterface : class
    {
        return _services.ContainsKey(typeof(TInterface));
    }

    /// <summary>
    /// Disposes all disposable services in the container.
    /// </summary>
    public void DisposeServices()
    {
        foreach (var service in _services.Values)
        {
            switch (service)
            {
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
                case Lazy<IDisposable> lazyDisposable when lazyDisposable.IsValueCreated:
                    lazyDisposable.Value.Dispose();
                    break;
            }
        }
        _services.Clear();
    }
}