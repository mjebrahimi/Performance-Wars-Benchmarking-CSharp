using System.Dynamic;

namespace Utilities;

public static class ExpandoObjectExtensions
{
    public static void SetProperty(this ExpandoObject expandoObj, string propertyName, object propertyValue)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);

        var expandoDict = expandoObj as IDictionary<string, object>;
        expandoDict[propertyName] = propertyValue;
    }

    public static object GetProperty(this ExpandoObject expandoObj, string propertyName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);

        var expandoDict = expandoObj as IDictionary<string, object>;
        return expandoDict[propertyName];
    }

    public static void RemoveProperty(this ExpandoObject expandoObj, string propertyName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);

        var expandoDict = expandoObj as IDictionary<string, object>;
        expandoDict.Remove(propertyName);
    }

    public static void ChangeProperty(this ExpandoObject expandoObj, string fromPropertyName, string toPropertyName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fromPropertyName);
        ArgumentException.ThrowIfNullOrWhiteSpace(toPropertyName);

        if (fromPropertyName == toPropertyName)
            return;

        var expandoDict = expandoObj as IDictionary<string, object>;
        expandoDict[toPropertyName] = expandoDict[fromPropertyName];
        expandoDict.Remove(fromPropertyName);
    }

    public static void RemovePropertiesExcept(this ExpandoObject expandoObj, IEnumerable<string> propertyNames)
    {
        EnumerableGuard.ThrowIfNullOrEmpty(propertyNames);

        var expandoDict = expandoObj as IDictionary<string, object>;
        foreach (var propertyName in expandoDict.Keys.Except(propertyNames))
            expandoDict.Remove(propertyName);
    }
}
