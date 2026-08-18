using System;
using System.Reflection;

namespace UIFramework.Helpers
{
    public static class PropertyPathResolver
    {
        public static object GetPropertyValue(object source, string propertyPath)
        {
            if (source == null)
                return null;

            if (string.IsNullOrWhiteSpace(propertyPath))
                return null;

            object currentObject = source;

            var properties = propertyPath.Split('.');

            foreach (var propertyName in properties)
            {
                if (currentObject == null)
                    return null;

                var type = currentObject.GetType();
                var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

                if (property == null)
                    throw new InvalidOperationException(
                        $"Property '{propertyName}' not found on type '{type.Name}'");

                currentObject = property.GetValue(currentObject);
            }

            return currentObject;
        }
    }
}
