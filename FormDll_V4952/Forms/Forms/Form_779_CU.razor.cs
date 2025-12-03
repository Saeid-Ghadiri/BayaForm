using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using DateUtils;

namespace Forms.Forms
{
    public class Form_779_CUBase : Form_779_CUPeropeties
    {
        // تابع پیام تُــست
        public MSG _MSG { get; set; }

        /// <summary>
        /// آماده سازی فرم
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
        /// اعتبار سنجی فرم — فقط فراخوانی CheckFieldValidation
        /// </summary>
        public override async Task<bool> FormValidator()
        {
            return await CheckFieldValidation(_Entity);
        }

        /// <summary>
        /// قبل از ارسال داده — تبدیل شمسی → میلادی
        /// </summary>
        public override async Task<Result> BeforSubmit()
        {
            await PrepareForSubmit();
            return new Result() { Status = HttpStatusCode.OK };
        }

        /// <summary>
        /// بعد از دریافت داده — تبدیل میلادی → شمسی + پر کردن سه فیلد
        /// </summary>
        public override async Task AfterGetData()
        {
            await PrepareForDisplay();
        }

        #region FunctionEvents

        /// <summary>
        /// اعتبارسنجی فیلدها — فقط تاریخ شمسی
        /// </summary>
        public async Task<bool> CheckFieldValidation(Entity.HR_CVR_ApprovalsMinistryLaborGroup Item)
        {
            bool IsValid = true;

            if (string.IsNullOrWhiteSpace(Item.StartDate_Fa))
            {
                IsValid = false;
                await _MSG.ShowError("لطفاً گزینه تاریخ شروع محاسبه را تکمیل نمایید.");
            }
            else if (!PersianDateUtils.TryParseDateString(Item.StartDate_Fa, out _))
            {
                IsValid = false;
                await _MSG.ShowError("تاریخ شمسی وارد شده معتبر نیست. لطفاً به فرمت صحیح (مثال: 1404/01/01) وارد کنید.");
            }

            return IsValid;
        }

        /// <summary>
        /// آماده‌سازی برای ارسال — تبدیل شمسی → میلادی
        /// </summary>
        private async Task PrepareForSubmit()
        {
            if (!string.IsNullOrWhiteSpace(_Entity.StartDate_Fa))
            {
                if (PersianDateUtils.TryParseDateString(_Entity.StartDate_Fa, out var parts))
                {
                    // تبدیل به میلادی — برای ذخیره در دیتابیس
                    _Entity.StartDate = PersianDateUtils.ToGregorian(_Entity.StartDate_Fa);

                    // ذخیره سه فیلد — حتی اگر قبلاً مقدار داشته باشند
                    _Entity.CalculationStartYear = parts.year.ToString();
                    _Entity.CalculationStartMonth = parts.month.ToString("00");
                    _Entity.CalculationStartDay = parts.day.ToString("00");
                }
                else
                {
                    await _MSG.ShowError("تاریخ شمسی وارد شده معتبر نیست.");
                }
            }
        }

        /// <summary>
        /// آماده‌سازی برای نمایش — تبدیل میلادی → شمسی + پر کردن سه فیلد
        /// </summary>
        private async Task PrepareForDisplay()
        {
            // اگر تاریخ میلادی وجود داشت — تبدیل به شمسی
            if (_Entity.StartDate.HasValue)
            {
                _Entity.StartDate_Fa = PersianDateUtils.ToPersian(_Entity.StartDate.Value);
            }

            // اگر تاریخ شمسی وجود داشت — پر کردن سه فیلد
            if (!string.IsNullOrWhiteSpace(_Entity.StartDate_Fa) && PersianDateUtils.TryParseDateString(_Entity.StartDate_Fa, out var parts))
            {
                _Entity.CalculationStartYear = parts.year.ToString();
                _Entity.CalculationStartMonth = parts.month.ToString("00");
                _Entity.CalculationStartDay = parts.day.ToString("00");
            }
        }

        /// <summary>
        /// هنگام تغییر تاریخ شمسی — به‌روزرسانی سه فیلد سال/ماه/روز
        /// </summary>
        public async Task StartDate_Fa_oninput(ChangeEventArgs e)
        {
            var persianDate = e?.Value?.ToString();

            if (!string.IsNullOrWhiteSpace(persianDate) && PersianDateUtils.TryParseDateString(persianDate, out var parts))
            {
                // به‌روزرسانی سه فیلد — جایگزین مقادیر قبلی
                _Entity.CalculationStartYear = parts.year.ToString();
                _Entity.CalculationStartMonth = parts.month.ToString("00");
                _Entity.CalculationStartDay = parts.day.ToString("00");
                StateHasChanged();
            }
            else
            {
                // اگر تاریخ نامعتبر بود — پاک کردن فیلدها
                _Entity.CalculationStartYear = null;
                _Entity.CalculationStartMonth = null;
                _Entity.CalculationStartDay = null;
                StateHasChanged();
            }
        }

        /// <summary>
        /// هنگام تغییر هر یک از فیلدهای سال/ماه/روز — به‌روزرسانی تاریخ شمسی
        /// </summary>
        private async Task UpdatePersianDateFromParts()
        {
            if (!string.IsNullOrWhiteSpace(_Entity.CalculationStartYear) &&
                !string.IsNullOrWhiteSpace(_Entity.CalculationStartMonth) &&
                !string.IsNullOrWhiteSpace(_Entity.CalculationStartDay))
            {
                var persianDate = $"{_Entity.CalculationStartYear}/{_Entity.CalculationStartMonth}/{_Entity.CalculationStartDay}";

                if (PersianDateUtils.TryParseDateString(persianDate, out var parts))
                {
                    // به‌روزرسانی تاریخ شمسی — جایگزین مقدار قبلی
                    _Entity.StartDate_Fa = $"{parts.year}/{parts.month:00}/{parts.day:00}";
                    StateHasChanged();
                }
            }
        }

        public async Task CalculationStartYear_oninput(ChangeEventArgs e)
        {
            await UpdatePersianDateFromParts();
        }

        public async Task CalculationStartMonth_oninput(ChangeEventArgs e)
        {
            await UpdatePersianDateFromParts();
        }

        public async Task CalculationStartDay_oninput(ChangeEventArgs e)
        {
            await UpdatePersianDateFromParts();
        }

        #endregion FunctionEvents
    }
}