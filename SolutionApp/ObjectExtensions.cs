using System;

namespace SolutionApp
{
    static class ObjectExtensions
    {
        public static T SetNullToEmpty<T>(this T sourceObject)
        {
            var propertyInfo = sourceObject.GetType().GetProperties();
            foreach (var property in propertyInfo)
            {
                switch (property.PropertyType)
                {
                    case { } _type when _type == typeof(string) && property.GetValue(sourceObject) == null:
                        property.SetValue(sourceObject, string.Empty);
                        break;

                    case { IsClass: true }:
                        object instance = property.GetValue(sourceObject) ?? Activator.CreateInstance(property.PropertyType);

                        property.SetValue(sourceObject, instance);
                        instance.SetNullToEmpty();
                        break;
                }
            }

            return sourceObject;
        }
    }
}
