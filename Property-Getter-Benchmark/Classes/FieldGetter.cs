using System.Reflection;
using System.Reflection.Emit;

#region reflection emit edition
/// <summary>
/// https://github.com/dotnet/runtime/blob/main/src/libraries/System.Diagnostics.DiagnosticSource/src/System/Diagnostics/HttpHandlerDiagnosticListener.cs
/// </summary>
/// <typeparam name="TField"></typeparam>
public class FieldGetter<TField>
{
    public static Func<TClass, TField> Create<TClass>(string fieldName, BindingFlags flags) where TClass : class
    {
        FieldInfo field = typeof(TClass).GetField(fieldName, flags);
        if (field != null)
        {
            string methodName = field.ReflectedType.FullName + ".get_" + field.Name;
            DynamicMethod getterMethod = new DynamicMethod(methodName, typeof(TField), new[] { typeof(TClass) }, true);
            ILGenerator generator = getterMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, field);
            generator.Emit(OpCodes.Ret);
            return (Func<TClass, TField>)getterMethod.CreateDelegate(typeof(Func<TClass, TField>));
        }

        return null;
    }


    /// <summary>
    /// Creates getter for a field defined in private or internal type
    /// repesented with classType variable
    /// </summary>
    public static Func<object, TField> Create(Type classType, string fieldName, BindingFlags flags)
    {
        fieldName = "get_" + fieldName;
        MethodInfo field = classType.GetMethod(fieldName, flags);
        if (field != null)
        {
            string methodName = classType.FullName + fieldName;
            DynamicMethod getterMethod = new DynamicMethod(methodName, typeof(TField), new[] { typeof(object) }, true);
            ILGenerator generator = getterMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, classType);
            generator.Emit(OpCodes.Call, field);
            generator.Emit(OpCodes.Ret);

            return (Func<object, TField>)getterMethod.CreateDelegate(typeof(Func<object, TField>));
        }

        return null;
    }
}
#endregion