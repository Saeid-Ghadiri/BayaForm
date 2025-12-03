using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using System.Collections.Generic;

namespace Forms.Forms
{
    public class Form_784_CUBase : Form_784_CUPeropeties
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

        /// <summary>
        /// دیکشنری تبدیل شناسه نوع مدت قرارداد به تعداد روز معادل (هر ماه = 30 روز)
        /// </summary>
        private static readonly Dictionary<string, int> ContractTypeToDaysMap = new()
        {
            { "6844F943-DD99-F011-A50E-005056A2B6BD", 30 },   // 1 ماهه
            { "6944F943-DD99-F011-A50E-005056A2B6BD", 60 },   // 2 ماهه
            { "DE7ACE4E-DD99-F011-A50E-005056A2B6BD", 90 },   // 3 ماهه
            { "859A0C55-DD99-F011-A50E-005056A2B6BD", 120 },  // 4 ماهه
            { "AB1B7E0B-56AA-F011-A50E-005056A2B6BD", 150 },  // 5 ماهه
            { "934F1AC7-B3AC-F011-A50E-005056A2B6BD", 180 },  // 6 ماهه
            { "D7CF40CE-B3AC-F011-A50E-005056A2B6BD", 210 },  // 7 ماهه
            { "D8CF40CE-B3AC-F011-A50E-005056A2B6BD", 240 },  // 8 ماهه
            { "3D2D5FD8-B3AC-F011-A50E-005056A2B6BD", 270 },  // 9 ماهه
            { "5290C3DF-B3AC-F011-A50E-005056A2B6BD", 300 },  // 10 ماهه
            { "5390C3DF-B3AC-F011-A50E-005056A2B6BD", 330 },  // 11 ماهه
            { "73AF20E9-B3AC-F011-A50E-005056A2B6BD", 360 }   // 12 ماهه
        };

        /// <summary>
        /// محاسبه خودکار مدت قرارداد (ContractTime) بر اساس نوع مدت و تا تعداد.
        /// </summary>
        private void CalculateContractTime()
        {
            //if (_Entity == null) return;

            // اگر نوع مدت قرارداد انتخاب نشده باشد، مقدار را صفر قرار بده
            if (string.IsNullOrEmpty(_Entity.HR_ContractTimeTypeId?.ToString()))
            {
                _Entity.ContractTime = 0;
                return;
            }

            // دریافت روز معادل بر اساس شناسه نوع مدت
            if (!ContractTypeToDaysMap.TryGetValue(_Entity.HR_ContractTimeTypeId.ToString(), out var daysPerType))
            {
                _Entity.ContractTime = 0;
                return;
            }

            // محاسبه نهایی: مدت قرارداد = روز معادل × تا تعداد
            var toNumber = _Entity.ToNumber ?? 0;
            _Entity.ContractTime = daysPerType * toNumber;

            // اگر فیلدهای MonthToDays و ContractTotalDays در مدل وجود دارند:
            _Entity.MonthToDays = daysPerType;
            _Entity.ContractTotalDays = daysPerType * toNumber;
        }

        /// <summary>
        /// هنگام تایپ در فیلد "از تعداد"، مقدار ContractTime به‌روزرسانی می‌شود.
        /// (در حال حاضر FromNumber در محاسبه استفاده نمی‌شود، اما برای آینده آماده است)
        /// </summary>
        public async Task FromNumber_oninput(ChangeEventArgs args)
        {
            // اگر در آینده FromNumber در محاسبه دخیل شد، اینجا فعال می‌شود
            CalculateContractTime();
            await Task.CompletedTask; // فقط برای رفع هشدار async
        }

        /// <summary>
        /// هنگام تایپ در فیلد "تا تعداد"، مقدار ContractTime به‌صورت زنده محاسبه و نمایش داده می‌شود.
        /// </summary>
        public async Task ToNumber_oninput(ChangeEventArgs args)
        {
            CalculateContractTime();
            await Task.CompletedTask; // فقط برای رفع هشدار async
        }

        #endregion FunctionEvents
    }
}