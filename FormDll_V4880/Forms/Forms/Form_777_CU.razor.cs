using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using Blazored.Toast.Services;
using DateUtils;

namespace Forms.Forms
{
    public class Form_777_CUBase : Form_777_CUPeropeties
    {
        // تابع پیام تست
        public MSG _MSG { get; set; }

        /// <summary>
        /// آماده سازی فرم
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            // اگر نیاز به مقداردهی اولیه خاصی دارید، اینجا اضافه کنید
            await base.OnInitializedAsync();
        }

        /// <summary>
        /// رندر شدن فرم
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _MSG = new MSG(toastService);
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        /// اعتبار سنجی فرم
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> FormValidator()
        {
            bool IsValid = true;

            if (string.IsNullOrWhiteSpace(_Entity.StartDate_Fa))
            {
                IsValid = false;
                await _MSG.ShowError("لطفاً تاریخ شروع محاسبه را وارد کنید.");
            }
            else if (!PersianDateUtils.TryParseDateString(_Entity.StartDate_Fa, out _))
            {
                IsValid = false;
                await _MSG.ShowError("تاریخ شروع محاسبه وارد شده معتبر نیست.");
            }

            return IsValid;
        }

        /// <summary>
        /// تابع قبل اجرا شدن ارسال داده
        /// </summary>
        /// <returns></returns>
        public override async Task<Result> BeforSubmit()
        {
            // تبدیل تاریخ شمسی به میلادی و استخراج اجزای Y/M/D قبل از ارسال
            PrepareStartDateForSubmit();

            // تأیید نهایی صحت تاریخ برای جلوگیری از ارسال داده ناقص
            if (string.IsNullOrWhiteSpace(_Entity.StartDate_Fa) || _Entity.StartDate == null)
            {
                await _MSG.ShowError("تاریخ شروع محاسبه نامعتبر است و قابل تبدیل به تاریخ میلادی نیست.");
                return new Result { Status = HttpStatusCode.BadRequest, Message = "تاریخ نامعتبر" };
            }

            return new Result { Status = HttpStatusCode.OK };
        }

        /// <summary>
        /// تابع بعد اجرا شدن ارسال داده
        /// </summary>
        /// <returns></returns>
        public override async Task AfterSubmit()
        {
            // اگر نیاز به اقدام خاصی پس از ذخیره دارید، اینجا بنویسید
        }

        /// <summary>
        /// تابع قبل دریافت داده
        /// </summary>
        /// <returns></returns>
        public override async Task BeforGetData()
        {
            // اگر نیاز به پیش‌پردازش قبل از لود داده دارید
        }

        /// <summary>
        /// تابع بعد دریافت داده
        /// </summary>
        /// <returns></returns>
        public override async Task AfterGetData()
        {
            // اگر نیاز به پس‌پردازش پس از لود داده دارید
        }

        #region FunctionEvents

        /// <summary>
        /// تبدیل تاریخ شمسی به میلادی و استخراج سال، ماه، روز شمسی
        /// </summary>
        private void PrepareStartDateForSubmit()
        {
            if (!string.IsNullOrWhiteSpace(_Entity.StartDate_Fa))
            {
                if (PersianDateUtils.TryParseDateString(_Entity.StartDate_Fa, out var parts))
                {
                    // 1. تبدیل به میلادی
                    _Entity.StartDate = PersianDateUtils.ToGregorian(_Entity.StartDate_Fa);

                    // 2. استخراج سال، ماه، روز شمسی
                    _Entity.CalculationStartYear = parts.year.ToString();
                    _Entity.CalculationStartMonth = parts.month.ToString("00");
                    _Entity.CalculationStartDay = parts.day.ToString("00");
                }
                else
                {
                    // تاریخ نامعتبر → میلادی null و فیلدها خالی
                    _Entity.StartDate = null;
                    _Entity.CalculationStartYear = null;
                    _Entity.CalculationStartMonth = null;
                    _Entity.CalculationStartDay = null;
                }
            }
            else
            {
                _Entity.StartDate = null;
                _Entity.CalculationStartYear = null;
                _Entity.CalculationStartMonth = null;
                _Entity.CalculationStartDay = null;
            }
        }

        #endregion FunctionEvents
    }
}