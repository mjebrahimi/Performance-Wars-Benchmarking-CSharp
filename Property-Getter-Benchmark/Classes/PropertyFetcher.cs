using System.Reflection;

#region opentelemetry-dotnet edition
/// <summary>
/// PropertyFetcher fetches a property from an object.
/// https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry/DiagnosticSourceInstrumentation/PropertyFetcher.cs
/// </summary>
/// <typeparam name="T">The type of the property being fetched.</typeparam>
public class PropertyFetcher<T>
{
    private readonly string propertyName;
    private PropertyFetch innerFetcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyFetcher{T}"/> class.
    /// </summary>
    /// <param name="propertyName">Property name to fetch.</param>
    public PropertyFetcher(string propertyName)
    {
        this.propertyName = propertyName;
    }

    /// <summary>
    /// Fetch the property from the object.
    /// </summary>
    /// <param name="obj">Object to be fetched.</param>
    /// <returns>Property fetched.</returns>
    public T Fetch(object obj)
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));

        if (!TryFetch(obj, out T value, true))
        {
            throw new ArgumentException($"Unable to fetch property: '{nameof(obj)}'", nameof(obj));
        }

        return value;
    }

    /// <summary>
    /// Try to fetch the property from the object.
    /// </summary>
    /// <param name="obj">Object to be fetched.</param>
    /// <param name="value">Fetched value.</param>
    /// <param name="skipObjNullCheck">Set this to <see langword= "true"/> if we know <paramref name="obj"/> is not <see langword= "null"/>.</param>
    /// <returns><see langword= "true"/> if the property was fetched.</returns>
    public bool TryFetch(object obj, out T value, bool skipObjNullCheck = false)
    {
        if (!skipObjNullCheck && obj == null)
        {
            value = default;
            return false;
        }

        if (innerFetcher == null)
        {
            innerFetcher = PropertyFetch.Create(obj.GetType().GetTypeInfo(), propertyName);
        }

        return innerFetcher.TryFetch(obj, out value);
    }

    // see https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.DiagnosticSource/src/System/Diagnostics/DiagnosticSourceEventSource.cs
    private class PropertyFetch
    {
        public static PropertyFetch Create(TypeInfo type, string propertyName)
        {
            var property = type.DeclaredProperties.FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.InvariantCultureIgnoreCase));
            if (property == null)
            {
                property = type.GetProperty(propertyName);
            }

            return CreateFetcherForProperty(property);

            static PropertyFetch CreateFetcherForProperty(PropertyInfo propertyInfo)
            {
                if (propertyInfo == null || !typeof(T).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    // returns null on any fetch.
                    return new PropertyFetch();
                }

                var typedPropertyFetcher = typeof(TypedPropertyFetch<,>);
                var instantiatedTypedPropertyFetcher = typedPropertyFetcher.MakeGenericType(
                    typeof(T), propertyInfo.DeclaringType, propertyInfo.PropertyType);
                return (PropertyFetch)Activator.CreateInstance(instantiatedTypedPropertyFetcher, propertyInfo);
            }
        }

        public virtual bool TryFetch(object obj, out T value)
        {
            value = default;
            return false;
        }

        private sealed class TypedPropertyFetch<TDeclaredObject, TDeclaredProperty> : PropertyFetch
            where TDeclaredProperty : T
        {
            private readonly string propertyName;
            private readonly Func<TDeclaredObject, TDeclaredProperty> propertyFetch;

            private PropertyFetch innerFetcher;

            public TypedPropertyFetch(PropertyInfo property)
            {
                propertyName = property.Name;
                propertyFetch = (Func<TDeclaredObject, TDeclaredProperty>)property.GetMethod.CreateDelegate(typeof(Func<TDeclaredObject, TDeclaredProperty>));
            }

            public override bool TryFetch(object obj, out T value)
            {
                if (obj is TDeclaredObject o)
                {
                    value = propertyFetch(o);
                    return true;
                }

                innerFetcher ??= Create(obj.GetType().GetTypeInfo(), propertyName);

                return innerFetcher.TryFetch(obj, out value);
            }
        }
    }
}
#endregion
