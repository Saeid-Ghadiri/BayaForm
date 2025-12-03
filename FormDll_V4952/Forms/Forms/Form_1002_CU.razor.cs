using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using Baya.Models.Utility.Entity;
using Baya.Models.Utility.Menu;
using Blazored.Toast.Services;
using Newtonsoft.Json.Linq;
using Utility;
using Baya.Models.ORM;
using DateUtils;

namespace Forms.Forms
{
    public class Form_1002_CUBase : Form_1002_CUPeropeties
    {
        // تابع پیام تُــست
        public MSG _MSG { get; set; }

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
                // تٌست
                _MSG = new MSG(toastService);
            }
        }

        /// <summary>
        /// اعتبار سنجی فرم
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> FormValidator()
        {
            bool IsValid = true;


            //foreach (var Item in List)
            //{
            //    Console.WriteLine("#Log FormValidator btn foreach :");
            //    IsValid = IsValid && await CheckFieldValidation(Item);
            //}

            return IsValid;
        }


        /// <summary>
        /// تابع قبل اجرا شدن ارسال داده
        /// </summary>
        /// <returns></returns>
        public override async Task<Result> BeforSubmit()
        {
            // تبدیل تاریخ شمسی اجرای حکم به میلادی
            PrepareExecutionDateForSubmit();

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

        public async Task<bool> CheckFieldValidation(Entity.HR_CVR_VerdictRecruiting Item)
        {
            bool IsValid = true;

            // var List = _Entity.HR_CVR_VerdictRecruiting.ToList();

            // - تاریخ اجرای حکم
            // اعتبارسنجی تاریخ اجرای حکم (شمسی)
            if (!string.IsNullOrWhiteSpace(_Entity.ExecutionDateSentence_Fa))
            {
                if (!PersianDateUtils.TryParseDateString(_Entity.ExecutionDateSentence_Fa, out _))
                {
                    IsValid = false;
                    await _MSG.ShowWarning("تاریخ اجرای حکم وارد شده معتبر نیست.");
                }
            }
            else
            {
                // اگر فیلد اجباری است:
                IsValid = false;
                await _MSG.ShowError("لطفاً تاریخ اجرای حکم را وارد کنید.");
            }


            return IsValid;
        }


        public async Task HR_CVR_PersonnelContractId_onitemselected(Entity.HR_CVR_PersonnelContract Selected)
        {
        }

        /// <summary>
        /// پرکردن فیلد گروه شغل بر اساس شغل کارمند
        /// شغل کارمند از View گرفته شده است
        /// </summary>
        /// <param name="Selected"></param>
        /// <returns></returns>
        public async Task HR_CVR_JobId_onitemselected(dynamic Selected)
        {
            // HR_CVR_JobId => شغل کارمند
            // HR_CVR_JobGroupId => گروه شغلی کارمند

            Entity.HR_CVR_JobGroup JobGroup = new();
            JobGroup.Id = Selected.JobGroupId;
            JobGroup.Title = Selected.JobGroupTitle;

            // Console.WriteLine("#Log 1");

            Ref_HR_CVR_JobGroupId.SetEntity(JobGroup);

            // Console.WriteLine(await Utility.JSON.ToJson(JobGroup));
            Ref_HR_CVR_JobGroupId.ItemSelected(JobGroup);

            // Console.WriteLine("#Log 2");

            await Task.Delay(100);
            Ref_HR_CVR_JobGroupId.LoadData();
        }

        /// <summary>
        /// تبدیل تاریخ شمسی اجرای حکم به میلادی و ذخیره در مدل
        /// </summary>
        private void PrepareExecutionDateForSubmit()
        {
            if (!string.IsNullOrWhiteSpace(_Entity.ExecutionDateSentence_Fa))
            {
                if (PersianDateUtils.TryParseDateString(_Entity.ExecutionDateSentence_Fa, out _))
                {
                    // تبدیل به تاریخ میلادی (بدون زمان)
                    _Entity.ExecutionDateSentence = PersianDateUtils.ToGregorian(_Entity.ExecutionDateSentence_Fa);
                }
                else
                {
                    // در صورت نامعتبر بودن، مقدار میلادی را null کنید
                    _Entity.ExecutionDateSentence = null;
                }
            }
            else
            {
                _Entity.ExecutionDateSentence = null;
            }
        }

		#endregion FunctionEvents

    }
}