using System;
using System.Reflection;

namespace TitansMVC.Helpers
{
    public static class EnumExtension
    {
        public static string GetEnumDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);

            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field == null) return null;
                var customAttributeNamedArguments = field.GetCustomAttributesData()[0].NamedArguments;
                if (customAttributeNamedArguments == null) return null;
                var attr = customAttributeNamedArguments[0].TypedValue.Value.ToString();
                return attr;
            }

            return null;
        }
    }
}