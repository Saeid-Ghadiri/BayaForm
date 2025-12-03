using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms.Domain.Utility
{
    public static class EntityHelper
    {
        /// <summary>
        /// مقداردهی یک پراپرتی از طریق نام آن با Reflection
        /// </summary>
        public static void SetPropertyValue<T>(T target, string propertyName, object value)
        {
            if (target == null || string.IsNullOrWhiteSpace(propertyName))
                return;

            var prop = typeof(T).GetProperty(propertyName);
            if (prop != null && prop.CanWrite)
            {
                // اگر نوع‌ها سازگار نباشند (مثلاً Guid vs object)
                var convertedValue = Convert.ChangeType(value, prop.PropertyType);
                prop.SetValue(target, convertedValue);
            }
        }
        public static bool HasValidId(object? idValue)
        {
            return idValue switch
            {
                null => false,
                Guid guidValue => guidValue != Guid.Empty,
                string strValue => !string.IsNullOrWhiteSpace(strValue),
                int intValue => intValue != 0,
                long longValue => longValue != 0,
                short shortValue => shortValue != 0,
                decimal decValue => decValue != 0m,
                double dblValue => dblValue != 0d,
                float fltValue => fltValue != 0f,
                _ => true // سایر نوع‌ها به‌صورت پیش‌فرض معتبر در نظر گرفته می‌شوند
            };
        }

    }
}
