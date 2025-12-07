using Baya.Models.Utility;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Sitko.Blazor.CKEditor;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using DateUtils;

namespace Forms.Forms
{
    public class Form_775_CUBase : Form_775_CUPeropeties
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
        /// قبل از ارسال داده — فقط فراخوانی PrepareForSubmit
        /// </summary>
        public override async Task<Result> BeforSubmit()
        {
            await PrepareForSubmit();
            return new Result() { Status = HttpStatusCode.OK };
        }

        /// <summary>
        /// بعد از دریافت داده — فقط فراخوانی PrepareForDisplay
        /// </summary>
        public override async Task AfterGetData()
        {
            await PrepareForDisplay();
        }

        #region FunctionEvents

        /// <summary>
        /// اعتبارسنجی فیلدها — فقط تاریخ شروع محاسبه
        /// </summary>
        public async Task<bool> CheckFieldValidation(Entity.HR_CVR_JobSalaryRank Item)
        {
            bool IsValid = true;

            if (string.IsNullOrWhiteSpace(Item.StartDateJobSalaryRank_Fa))
            {
                IsValid = false;
                await _MSG.ShowError("لطفاً گزینه تاریخ شروع محاسبه مزد شغل و رتبه را تکمیل نمایید.");
            }
            else if (!PersianDateUtils.TryParseDateString(Item.StartDateJobSalaryRank_Fa, out _))
            {
                IsValid = false;
                await _MSG.ShowError("تاریخ شمسی وارد شده معتبر نیست. لطفاً به فرمت صحیح (مثال: 1404/01/01) وارد کنید.");
            }

            return IsValid;
        }

        /// <summary>
        /// آماده‌سازی برای ارسال — تبدیل شمسی → میلادی + استخراج سال/ماه/روز
        /// </summary>
        private async Task PrepareForSubmit()
        {
            if (!string.IsNullOrWhiteSpace(_Entity.StartDateJobSalaryRank_Fa))
            {
                if (PersianDateUtils.TryParseDateString(_Entity.StartDateJobSalaryRank_Fa, out var parts))
                {
                    // ✅ تبدیل به میلادی — برای ذخیره در دیتابیس
                    _Entity.StartDateJobSalaryRank = PersianDateUtils.ToGregorian(_Entity.StartDateJobSalaryRank_Fa);

                    // ✅ استخراج و ذخیره سال/ماه/روز — حتی اگر قبلاً پر بودند
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
        /// آماده‌سازی برای نمایش — تبدیل میلادی → شمسی + پر کردن فیلدهای سال/ماه/روز
        /// </summary>
        private async Task PrepareForDisplay()
        {
            // ✅ اگر تاریخ میلادی وجود داشت — تبدیل به شمسی
            if (_Entity.StartDateJobSalaryRank.HasValue)
            {
                _Entity.StartDateJobSalaryRank_Fa = PersianDateUtils.ToPersian(_Entity.StartDateJobSalaryRank.Value);
            }

            // ✅ اگر تاریخ شمسی وجود داشت — پر کردن سه فیلد
            if (!string.IsNullOrWhiteSpace(_Entity.StartDateJobSalaryRank_Fa) && PersianDateUtils.TryParseDateString(_Entity.StartDateJobSalaryRank_Fa, out var parts))
            {
                _Entity.CalculationStartYear = parts.year.ToString();
                _Entity.CalculationStartMonth = parts.month.ToString("00");
                _Entity.CalculationStartDay = parts.day.ToString("00");
            }
        }

        /// <summary>
        /// هنگام تغییر تاریخ شمسی — به‌روزرسانی سه فیلد سال/ماه/روز
        /// </summary>
        public async Task StartDateJobSalaryRank_Fa_oninput(ChangeEventArgs e)
        {
            var persianDate = e?.Value?.ToString();

            if (!string.IsNullOrWhiteSpace(persianDate) && PersianDateUtils.TryParseDateString(persianDate, out var parts))
            {
                // ✅ به‌روزرسانی سه فیلد — جایگزین مقادیر قبلی
                _Entity.CalculationStartYear = parts.year.ToString();
                _Entity.CalculationStartMonth = parts.month.ToString("00");
                _Entity.CalculationStartDay = parts.day.ToString("00");

                // ✅ به‌روزرسانی UI
                StateHasChanged();
            }
            else
            {
                // ❌ اگر تاریخ نامعتبر بود — پاک کردن فیلدها
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
                    // ✅ به‌روزرسانی تاریخ شمسی — جایگزین مقدار قبلی
                    _Entity.StartDateJobSalaryRank_Fa = $"{parts.year}/{parts.month:00}/{parts.day:00}";
                    StateHasChanged();
                }
            }
        }

        public async Task CalculationStartYear_oninput(ChangeEventArgs Selected)
        {
            await UpdatePersianDateFromParts();
        }

        public async Task CalculationStartMonth_oninput(ChangeEventArgs Selected)
        {
            await UpdatePersianDateFromParts();
        }

        public async Task CalculationStartDay_oninput(ChangeEventArgs Selected)
        {
            await UpdatePersianDateFromParts();
        }

        #endregion FunctionEvents
    }
}