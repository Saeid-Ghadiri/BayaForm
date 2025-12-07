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
    public class Form_990_CUBase : Form_990_CUPeropeties
    {
        // تابع پیام تست
        public MSG _MSG { get; set; }

        /// <summary>
        /// آماده‌سازی فرم
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
        }

        /// <summary>
        /// رندر شدن فرم
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _MSG = new MSG(toastService);
            }
        }

        /// <summary>
        /// اعتبارسنجی فرم
        /// </summary>
        public override async Task<bool> FormValidator()
        {
            bool IsValid = true;

            // اعتبارسنجی تاریخ شروع محاسبه (شمسی)
            if (!string.IsNullOrWhiteSpace(_Entity.StartDate_Fa))
            {
                if (!PersianDateUtils.TryParseDateString(_Entity.StartDate_Fa, out _))
                {
                    IsValid = false;
                    await _MSG.ShowError("تاریخ شروع محاسبه وارد شده معتبر نیست.");
                }
            }
            else
            {
                IsValid = false;
                await _MSG.ShowError("لطفاً تاریخ شروع محاسبه را وارد کنید.");
            }

            return IsValid;
        }

        /// <summary>
        /// قبل از ارسال داده: تبدیل تاریخ شمسی به میلادی و استخراج سال/ماه/روز
        /// </summary>
        public override async Task<Result> BeforSubmit()
        {
            // تبدیل تاریخ شمسی به میلادی
            PrepareStartDateForSubmit();

            return new Result() { Status = HttpStatusCode.OK };
        }

        /// <summary>
        /// بعد از ارسال داده
        /// </summary>
        public override async Task AfterSubmit()
        {
        }

        /// <summary>
        /// قبل از دریافت داده
        /// </summary>
        public override async Task BeforGetData()
        {
        }

        /// <summary>
        /// بعد از دریافت داده
        /// </summary>
        public override async Task AfterGetData()
        {
            
        }

        #region FunctionEvents

        /// <summary>
        /// تبدیل تاریخ شمسی به میلادی و استخراج سال/ماه/روز شمسی
        /// </summary>
        private void PrepareStartDateForSubmit()
        {
            if (!string.IsNullOrWhiteSpace(_Entity.StartDate_Fa))
            {
                if (PersianDateUtils.TryParseDateString(_Entity.StartDate_Fa, out var parts))
                {
                    // 1. تبدیل به میلادی
                    _Entity.StartDate = PersianDateUtils.ToGregorian(_Entity.StartDate_Fa);

                    // 2. استخراج سال، ماه، روز شمسی به فیلدهای جداگانه
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