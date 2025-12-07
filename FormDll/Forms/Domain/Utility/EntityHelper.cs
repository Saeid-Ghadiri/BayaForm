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
         /// <summary>
         /// مقدار "خالی" مناسب رو بر اساس نوع ورودی برمی‌گردونه
         /// مثلاً Guid → Guid.Empty, string → null, int → 0 و ...
         /// </summary>
         public static object SetPropertyNull(object value)
         {
             if (value == null) return null;
        
             var type = value.GetType();
        
             // برای Guid
             if (type == typeof(Guid)) return Guid.Empty;
             if (type == typeof(Guid?)) return (Guid?)null;
        
             // برای string (یا string?)
             if (type == typeof(string)) return null; // یا Guid.Empty.ToString() اگه بخوای
        
             // برای اعداد
             if (type == typeof(int)) return 0;
             if (type == typeof(int?)) return (int?)null;
             if (type == typeof(long)) return 0L;
             if (type == typeof(long?)) return (long?)null;
             if (type == typeof(short)) return (short)0;
             if (type == typeof(short?)) return (short?)null;
             if (type == typeof(byte)) return (byte)0;
             if (type == typeof(byte?)) return (byte?)null;
             if (type == typeof(decimal)) return 0m;
             if (type == typeof(decimal?)) return (decimal?)null;
             if (type == typeof(float)) return 0f;
             if (type == typeof(float?)) return (float?)null;
             if (type == typeof(double)) return 0d;
             if (type == typeof(double?)) return (double?)null;
        
             // اگه نوع دیگه‌ای بود، null برگردون (یا می‌تونی throw کنی)
             return null;
         }
        
         // نسخه generic برای تایپ‌سیف بیشتر (اختیاری، اما بهتره از این استفاده کنی)
         public static T SetPropertyNull<T>(T value)
         {
             return (T)SetPropertyNull((object)value);
         }
    }
}
