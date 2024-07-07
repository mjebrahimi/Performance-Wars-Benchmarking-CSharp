using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

public static class Factory<T>
    where T : new()
{
    private static readonly Func<T> createInstanceFuncUsingExpression = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();
    private static readonly Func<T> createInstanceFuncUsingReflectionEmit = CreateCtorUsingReflectionEmit();
    private static readonly ConstructorInfo constructorInfo = typeof(T).GetConstructor(Type.EmptyTypes)!;

    public static T New() => new T();
    public static T CompiledExpression() => createInstanceFuncUsingExpression();
    public static T ActivatorCreateInstance() => Activator.CreateInstance<T>();
    public static T Reflection() => (T)constructorInfo.Invoke(null);
    public static T ReflectionEmit() => createInstanceFuncUsingReflectionEmit();

    private static Func<T> CreateCtorUsingReflectionEmit()
    {
        var type = typeof(T);
        ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes)!;
        var dynamicMethod = new DynamicMethod("CreateInstance", type, Type.EmptyTypes, true);
        ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
        ilGenerator.Emit(OpCodes.Nop);
        ilGenerator.Emit(OpCodes.Newobj, ctor);
        ilGenerator.Emit(OpCodes.Ret);
        return dynamicMethod.CreateDelegate<Func<T>>();
    }
}
