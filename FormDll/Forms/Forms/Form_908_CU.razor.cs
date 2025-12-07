using Baya.Models.Utility;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Sitko.Blazor.CKEditor;
using System;
using System.Globalization;
using System.Net;

namespace Forms.Forms
{
    public class Form_908_CUBase : Form_908_CUPeropeties
    {



        /// <summary>
        /// آماده سازی فرم
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {

        }

        /// <summary>
        /// رندر شدن فرم
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //Console.WriteLine("Log Test 1");
            if (!_Entity.IsActive.HasValue)
            {
                //Console.WriteLine("Log Test 2");
                _Entity.IsActive = true;
                Ref_IsActive.AddAttribute("checked", "checked");

                StateHasChanged();
            }

            if (firstRender)
            {

            }
        }

        /// <summary>
        /// اعتبار سنجی فرم
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> FormValidator()
        {
            bool IsValid = true;

            // if (_Entity.ReqCount != 5)
            // {
            //     IsValid = false;
            //     SumaryMessage += "تعداد درخواست مخالف 5 باشد";
            // }

            return IsValid;
        }


        /// <summary>
        /// تابع قبل اجرا شدن ارسال داده
        /// </summary>
        /// <returns></returns>
        public override async Task<Result> BeforSubmit()
        {
            return new Result() { Status = HttpStatusCode.OK };
        }

        /// <summary>
        /// تابع بعد اجرا شدن ارسال داده
        /// </summary>
        /// <returns></returns>
        public override async Task AfterSubmit()
        {

        }

        /// <summary>
        /// تابع قبل دریافت داده
        /// </summary>
        /// <returns></returns>
        public override async Task BeforGetData()
        {

        }

        /// <summary>
        /// تابع بعد دریافت داده
        /// </summary>
        /// <returns></returns>
        public override async Task AfterGetData()
        {

        }


        #region FunctionEvents

        #endregion FunctionEvents

    }
}


namespace DateUtils
{
    /// <summary>
    /// کتابخانه جامع و بهینه برای مدیریت تاریخ‌های شمسی
    /// شامل تبدیل بین شمسی و میلادی، محاسبه اختلاف تاریخ، سن، روزهای کاری و تعطیلی،
    /// و قابلیت‌های کاربردی مانند تبدیل تاریخ به حروف و مدیریت تعطیلات سازمانی.
    /// </summary>
    public static class PersianDateUtils
    {
        private static readonly PersianCalendar _persianCalendar = new PersianCalendar();
        private static (int Min, int Max) _validYearRange = (1300, 1500);

        // لیست تعطیلات رسمی و سازمانی (ماه، روز) — پیش‌فرض: نوروز و چند مورد نمونه
        // قابل گسترش با متد AddOfficialHoliday
        private static readonly HashSet<(int Month, int Day)> _officialHolidays = new()
        {
            (1, 1), (1, 2), (1, 3), (1, 4), // نوروز — ۱ تا ۴ فروردین
            (12, 29), // شهادت حضرت فاطمه (مثال)
        };

        #region 🔧 تنظیمات سیستم

        /// <summary>
        /// تنظیم محدوده مجاز سال شمسی برای اعتبارسنجی ورودی‌ها.
        /// پیش‌فرض: 1300 تا 1500
        /// </summary>
        /// <param name="minYear">حداقل سال مجاز (شمسی)</param>
        /// <param name="maxYear">حداکثر سال مجاز (شمسی)</param>
        /// <exception cref="ArgumentException">اگر minYear >= maxYear باشد</exception>
        /// Example:
        /// PersianDateUtils.SetYearRange(1200, 1600);
        /// </summary>
        public static void SetYearRange(int minYear, int maxYear)
        {
            if (minYear >= maxYear)
                throw new ArgumentException("حداقل سال باید کوچکتر از حداکثر سال باشد");
            _validYearRange = (minYear, maxYear);
        }

        /// <summary>
        /// افزودن یک تاریخ تعطیل رسمی یا سازمانی به لیست تعطیلات.
        /// این تاریخ‌ها در محاسبه روزهای کاری و استراحت لحاظ می‌شوند.
        /// </summary>
        /// <param name="month">ماه شمسی (1 تا 12)</param>
        /// <param name="day">روز ماه (1 تا 31)</param>
        /// Example:
        /// PersianDateUtils.AddOfficialHoliday(3, 14); // شهادت حضرت علی
        /// </summary>
        public static void AddOfficialHoliday(int month, int day)
        {
            _officialHolidays.Add((month, day));
        }

        #endregion

        #region 🔄 تبدیل تاریخ شمسی ↔ میلادی (با و بدون زمان)

        /// <summary>
        /// تبدیل یک تاریخ شمسی (بدون زمان) به تاریخ میلادی.
        /// فرمت ورودی: yyyy/MM/dd یا yyyy-MM-dd
        /// </summary>
        /// <param name="persianDate">تاریخ شمسی به صورت رشته</param>
        /// <returns>تاریخ میلادی معادل (زمان صفر)</returns>
        /// <exception cref="ArgumentException">اگر تاریخ نامعتبر باشد</exception>
        /// <exception cref="FormatException">اگر فرمت تاریخ اشتباه باشد</exception>
        /// Example:
        /// DateTime dt = PersianDateUtils.ToGregorian("1404/01/01"); // 2025-03-21
        /// </summary>
        public static DateTime ToGregorian(string persianDate)
        {
            var (y, m, d) = ParseDateString(persianDate);
            return _persianCalendar.ToDateTime(y, m, d, 0, 0, 0, 0);
        }

        /// <summary>
        /// تبدیل یک تاریخ میلادی به تاریخ شمسی (بدون زمان).
        /// خروجی با فرمت yyyy/MM/dd
        /// </summary>
        /// <param name="date">تاریخ میلادی ورودی</param>
        /// <returns>تاریخ شمسی معادل</returns>
        /// Example:
        /// string shamsi = PersianDateUtils.ToPersian(DateTime.Now);
        /// </summary>
        public static string ToPersian(DateTime date)
        {
            int y = _persianCalendar.GetYear(date);
            int m = _persianCalendar.GetMonth(date);
            int d = _persianCalendar.GetDayOfMonth(date);
            return $"{y:0000}/{m:00}/{d:00}";
        }

        /// <summary>
        /// تبدیل یک تاریخ و زمان شمسی به تاریخ و زمان میلادی.
        /// فرمت ورودی: yyyy/MM/dd HH:mm:ss
        /// </summary>
        /// <param name="persianDateTime">تاریخ و زمان شمسی</param>
        /// <returns>DateTime میلادی معادل</returns>
        /// <exception cref="ArgumentException">اگر زمان نامعتبر باشد</exception>
        /// Example:
        /// DateTime dt = PersianDateUtils.ToGregorianWithTime("1404/01/01 14:30:00");
        /// </summary>
        public static DateTime ToGregorianWithTime(string persianDateTime)
        {
            var parts = persianDateTime.Split(' ');
            var datePart = parts[0];
            var timePart = parts.Length > 1 ? parts[1] : "00:00:00";
            var date = ToGregorian(datePart);
            var timeParts = timePart.Split(':');
            int h = int.Parse(timeParts[0]);
            int min = timeParts.Length > 1 ? int.Parse(timeParts[1]) : 0;
            int s = timeParts.Length > 2 ? int.Parse(timeParts[2]) : 0;
            ValidateTime(h, min, s);
            return new DateTime(date.Year, date.Month, date.Day, h, min, s);
        }

        /// <summary>
        /// تبدیل یک تاریخ و زمان میلادی به تاریخ و زمان شمسی.
        /// خروجی با فرمت: yyyy/MM/dd HH:mm:ss
        /// </summary>
        /// <param name="date">تاریخ و زمان میلادی</param>
        /// <returns>تاریخ و زمان شمسی معادل</returns>
        /// Example:
        /// string shamsi = PersianDateUtils.ToPersianWithTime(DateTime.Now);
        /// </summary>
        public static string ToPersianWithTime(DateTime date)
        {
            return $"{_persianCalendar.GetYear(date):0000}/{_persianCalendar.GetMonth(date):00}/{_persianCalendar.GetDayOfMonth(date):00} {date.Hour:00}:{date.Minute:00}:{date.Second:00}";
        }

        #endregion

        #region 📊 محاسبه اختلاف تاریخ‌ها و سن

        /// <summary>
        /// ساختار نتیجه محاسبه اختلاف بین دو تاریخ شمسی.
        /// شامل سال، ماه، روز، کل روزها و تعداد روزهای کاری.
        /// </summary>
        public struct DateDifference
        {
            public int Years { get; set; }
            public int Months { get; set; }
            public int Days { get; set; }
            public int TotalDays { get; set; }
            public int WorkDays { get; set; } // روزهای کاری (جمعه و تعطیلات حذف شده‌اند)

            /// <summary>
            /// نمایش خوانا از نتیجه اختلاف.
            /// </summary>
            /// <param name="showZeros">نمایش اجزای صفر</param>
            /// <returns>رشته خوانا</returns>
            /// Example:
            /// var diff = PersianDateUtils.GetDifference("1404/01/01", "1404/03/31");
            /// Console.WriteLine(diff.ToReadableString()); // "2 ماه و 30 روز (کل: 90 روز، روز کاری: 64)"
            /// </summary>
            public string ToReadableString(bool showZeros = false)
            {
                var parts = new List<string>();
                if (showZeros || Years > 0) parts.Add($"{Years} سال");
                if (showZeros || Months > 0) parts.Add($"{Months} ماه");
                if (showZeros || Days > 0 || parts.Count == 0) parts.Add($"{Days} روز");
                return $"{string.Join(" و ", parts)} (کل: {TotalDays} روز، روز کاری: {WorkDays})";
            }
        }

        /// <summary>
        /// محاسبه اختلاف بین دو تاریخ شمسی به صورت سال، ماه، روز، کل روزها و روزهای کاری.
        /// </summary>
        /// <param name="startDate">تاریخ شروع (شمسی)</param>
        /// <param name="endDate">تاریخ پایان (شمسی)</param>
        /// <param name="inclusive">آیا روز پایانی در محاسبه لحاظ شود؟</param>
        /// <returns>ساختار DateDifference</returns>
        /// <exception cref="ArgumentException">اگر startDate > endDate</exception>
        /// Example:
        /// var diff = PersianDateUtils.GetDifference("1404/01/01", "1404/03/31", true);
        /// Console.WriteLine($"روز کاری: {diff.WorkDays}");
        /// </summary>
        public static DateDifference GetDifference(string startDate, string endDate, bool inclusive = true)
        {
            var start = ToGregorian(startDate);
            var end = ToGregorian(endDate);
            if (start > end) throw new ArgumentException("تاریخ شروع نمی‌تواند بعد از تاریخ پایان باشد");
            int totalDays = (end - start).Days + (inclusive ? 1 : 0);
            var adjustedEnd = inclusive ? end.AddDays(1) : end;
            int y1 = _persianCalendar.GetYear(start);
            int m1 = _persianCalendar.GetMonth(start);
            int d1 = _persianCalendar.GetDayOfMonth(start);
            int y2 = _persianCalendar.GetYear(adjustedEnd);
            int m2 = _persianCalendar.GetMonth(adjustedEnd);
            int d2 = _persianCalendar.GetDayOfMonth(adjustedEnd);
            int years = y2 - y1;
            int months = m2 - m1;
            int days = d2 - d1;
            if (days < 0)
            {
                months--;
                int prevMonth = m2 == 1 ? 12 : m2 - 1;
                int prevYear = m2 == 1 ? y2 - 1 : y2;
                days += _persianCalendar.GetDaysInMonth(prevYear, prevMonth);
            }
            if (months < 0)
            {
                years--;
                months += 12;
            }

            int workDays = CountWorkDays(startDate, endDate, inclusive);

            return new DateDifference
            {
                Years = years,
                Months = months,
                Days = days,
                TotalDays = totalDays,
                WorkDays = workDays
            };
        }

        /// <summary>
        /// محاسبه سن یک فرد بر اساس تاریخ تولد شمسی.
        /// </summary>
        /// <param name="birthDate">تاریخ تولد (شمسی)</param>
        /// <returns>سن به صورت DateDifference</returns>
        /// Example:
        /// var age = PersianDateUtils.CalculateAge("1370/05/20");
        /// Console.WriteLine($"سن: {age.Years} سال و {age.Months} ماه");
        /// </summary>
        public static DateDifference CalculateAge(string birthDate)
        {
            return GetDifference(birthDate, ToPersian(DateTime.Now));
        }

        #endregion

        #region 🛌 روزهای استراحت (جمعه + تعطیلات) و روزهای کاری

        /// <summary>
        /// بررسی اینکه آیا یک تاریخ شمسی، روز استراحت (جمعه یا تعطیل رسمی) است یا خیر.
        /// در فرهنگ ایران، جمعه = DayOfWeek.Friday = 5
        /// </summary>
        /// <param name="persianDate">تاریخ شمسی</param>
        /// <returns>true اگر روز استراحت باشد</returns>
        /// Example:
        /// bool isRest = PersianDateUtils.IsRestDay("1404/01/01"); // true — چون تعطیل نوروز است
        /// </summary>
        public static bool IsRestDay(string persianDate)
        {
            var greg = ToGregorian(persianDate);
            var (y, m, d) = ParseDateString(persianDate);
            // جمعه = Friday = 5 در .NET
            return (int)greg.DayOfWeek == 5 || _officialHolidays.Contains((m, d));
        }

        /// <summary>
        /// شمارش تعداد روزهای استراحت (جمعه + تعطیلات رسمی) بین دو تاریخ شمسی.
        /// </summary>
        /// <param name="startDate">تاریخ شروع</param>
        /// <param name="endDate">تاریخ پایان</param>
        /// <param name="inclusive">آیا روز پایانی لحاظ شود؟</param>
        /// <returns>تعداد روزهای استراحت</returns>
        /// Example:
        /// int restDays = PersianDateUtils.CountRestDays("1404/01/01", "1404/01/10", true);
        /// </summary>
        public static int CountRestDays(string startDate, string endDate, bool inclusive = true)
        {
            var start = ToGregorian(startDate);
            var end = ToGregorian(endDate);
            if (start > end) throw new ArgumentException("تاریخ شروع نمی‌تواند بعد از تاریخ پایان باشد");
            int count = 0;
            var current = start;
            var endInclusive = inclusive ? end.AddDays(1) : end;
            while (current < endInclusive)
            {
                if (IsRestDay(ToPersian(current))) count++;
                current = current.AddDays(1);
            }
            return count;
        }

        /// <summary>
        /// شمارش تعداد روزهای کاری (غیرجمعه و غیرتعطیل) بین دو تاریخ شمسی.
        /// </summary>
        /// <param name="startDate">تاریخ شروع</param>
        /// <param name="endDate">تاریخ پایان</param>
        /// <param name="inclusive">آیا روز پایانی لحاظ شود؟</param>
        /// <returns>تعداد روزهای کاری</returns>
        /// Example:
        /// int workDays = PersianDateUtils.CountWorkDays("1404/01/01", "1404/01/10", true);
        /// </summary>
        public static int CountWorkDays(string startDate, string endDate, bool inclusive = true)
        {
            var start = ToGregorian(startDate);
            var end = ToGregorian(endDate);
            if (start > end) throw new ArgumentException("تاریخ شروع نمی‌تواند بعد از تاریخ پایان باشد");
            int totalDays = (end - start).Days + (inclusive ? 1 : 0);
            int restDays = CountRestDays(startDate, endDate, inclusive);
            return Math.Max(0, totalDays - restDays);
        }

        #endregion

        #region 📅 متدهای کاربردی — روزهای گذشته و باقیمانده

        /// <summary>
        /// محاسبه تعداد روزهای گذشته از یک تاریخ شمسی تا امروز
        /// </summary>
        /// <param name="startDate">تاریخ شروع شمسی (مثال: "1404/01/01")</param>
        /// <param name="inclusive">آیا روز شروع محاسبه شود؟ (پیش‌فرض: true)</param>
        /// <returns>تعداد روزهای گذشته</returns>
        /// <exception cref="ArgumentException">اگر تاریخ نامعتبر باشد یا از امروز بزرگتر باشد</exception>
        /// Example:
        /// int passed = PersianDateUtils.DaysPassed("1404/01/01", true);
        /// </summary>
        public static int DaysPassed(string startDate, bool inclusive = true)
        {
            var start = ToGregorian(startDate);
            var today = DateTime.Today;

            if (start > today)
                throw new ArgumentException("تاریخ شروع نمی‌تواند از امروز بزرگتر باشد");

            return (today - start).Days + (inclusive ? 1 : 0);
        }

        /// <summary>
        /// محاسبه تعداد روزهای باقیمانده تا یک تاریخ شمسی از امروز
        /// </summary>
        /// <param name="endDate">تاریخ پایان شمسی (مثال: "1404/12/29")</param>
        /// <param name="inclusive">آیا روز پایان محاسبه شود؟ (پیش‌فرض: true)</param>
        /// <returns>تعداد روزهای باقیمانده (مثبت) یا منفی اگر تاریخ گذشته باشد</returns>
        /// <exception cref="ArgumentException">اگر تاریخ نامعتبر باشد</exception>
        /// Example:
        /// int remaining = PersianDateUtils.DaysRemaining("1404/12/29", true);
        /// </summary>
        public static int DaysRemaining(string endDate, bool inclusive = true)
        {
            var end = ToGregorian(endDate);
            var today = DateTime.Today;
            return (end - today).Days + (inclusive ? 1 : 0);
        }

        #endregion

        #region 🔧 متدهای کمکی داخلی

        /// <summary>
        /// پارس کردن یک رشته تاریخ شمسی به اجزای سال، ماه و روز.
        /// پشتیبانی از جداکننده‌های '/', '-', ' '
        /// </summary>
        /// <param name="s">رشته تاریخ شمسی مانند "1404/03/15"</param>
        /// <returns>ساختار (سال، ماه، روز)</returns>
        /// <exception cref="ArgumentException">اگر تاریخ خارج از محدوده معتبر باشد</exception>
        /// <exception cref="FormatException">اگر فرمت تاریخ اشتباه باشد یا شامل عدد نباشد</exception>
        /// Example:
        /// var (y, m, d) = PersianDateUtils.ParseDateString("1404/03/15");
        /// </summary>
        public static (int year, int month, int day) ParseDateString(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                throw new ArgumentException("تاریخ نمی‌تواند خالی یا فاصله باشد", nameof(s));

            var separators = new char[] { '/', '-', ' ' };
            var parts = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 3)
                throw new FormatException("فرمت تاریخ باید حداقل شامل سال، ماه و روز باشد (مثال: 1404/03/15)");

            if (!int.TryParse(parts[0], out int year))
                throw new FormatException("قسمت سال باید یک عدد معتبر باشد");
            if (!int.TryParse(parts[1], out int month))
                throw new FormatException("قسمت ماه باید یک عدد معتبر باشد");
            if (!int.TryParse(parts[2], out int day))
                throw new FormatException("قسمت روز باید یک عدد معتبر باشد");

            // اعتبارسنجی کامل تاریخ شمسی — شامل محدوده سال، ماه و روز
            ValidatePersianDate(year, month, day);

            return (year, month, day);
        }

        /// <summary>
        /// تلاش برای پارس کردن تاریخ شمسی بدون پرتاب Exception
        /// </summary>
        /// <param name="s">رشته تاریخ شمسی</param>
        /// <param name="result">خروجی (سال، ماه، روز)</param>
        /// <returns>true اگر پارس موفقیت‌آمیز بود</returns>
        /// Example:
        /// if (PersianDateUtils.TryParseDateString("1404/03/15", out var result))
        /// {
        ///     Console.WriteLine($"Parsed: {result.year}/{result.month}/{result.day}");
        /// }
        /// </summary>
        public static bool TryParseDateString(string s, out (int year, int month, int day) result)
        {
            try
            {
                result = ParseDateString(s);
                return true;
            }
            catch
            {
                result = (0, 0, 0);
                return false;
            }
        }

        /// <summary>
        /// اعتبارسنجی اجزای تاریخ شمسی (سال، ماه، روز)
        /// </summary>
        private static void ValidatePersianDate(int y, int m, int d)
        {
            if (y < _validYearRange.Min || y > _validYearRange.Max) throw new ArgumentException($"سال باید بین {_validYearRange.Min} تا {_validYearRange.Max} باشد");
            if (m < 1 || m > 12) throw new ArgumentException("ماه باید بین 1 تا 12 باشد");
            int daysInMonth = _persianCalendar.GetDaysInMonth(y, m);
            if (d < 1 || d > daysInMonth) throw new ArgumentException($"روز باید بین 1 تا {daysInMonth} باشد");
        }

        /// <summary>
        /// اعتبارسنجی اجزای زمان (ساعت، دقیقه، ثانیه)
        /// </summary>
        private static void ValidateTime(int h, int min, int s)
        {
            if (h < 0 || h > 23) throw new ArgumentException("ساعت باید بین 0 تا 23 باشد");
            if (min < 0 || min > 59) throw new ArgumentException("دقیقه باید بین 0 تا 59 باشد");
            if (s < 0 || s > 59) throw new ArgumentException("ثانیه باید بین 0 تا 59 باشد");
        }

        #endregion

        #region تبدیل تعداد روزهای سپری‌شده به فرمت خوانای شمسی: "X سال و Y ماه و Z روز"
        /// <summary>
        /// تبدیل تعداد روزهای سپری‌شده به فرمت خوانای شمسی: "X سال و Y ماه و Z روز"
        /// </summary>
        /// <param name="totalDays">تعداد کل روزها (مثبت)</param>
        /// <returns>رشته‌ای مانند "11 سال و 4 ماه و 23 روز"</returns>
        /// <exception cref="ArgumentException">اگر totalDays منفی باشد</exception>
        public static string ConvertDaysToPersianReadable(int totalDays)
        {
            if (totalDays < 0)
                throw new ArgumentException("تعداد روز نمی‌تواند منفی باشد.");

            // تاریخ پایه: امروز
            var endDate = DateTime.Today;
            var startDate = endDate.AddDays(-totalDays);

            // تبدیل به تاریخ شمسی
            var startPersian = ToPersian(startDate);
            var endPersian = ToPersian(endDate);

            // محاسبه اختلاف به صورت شمسی
            var diff = GetDifference(startPersian, endPersian, inclusive: false);

            // حذف بخش اضافی " (کل: ... روز، روز کاری: ...)"
            var readable = diff.ToReadableString(showZeros: false);
            int parenIndex = readable.IndexOf(" (کل:");
            if (parenIndex > 0)
                readable = readable.Substring(0, parenIndex);

            return readable;
        }
        #endregion
    }
}