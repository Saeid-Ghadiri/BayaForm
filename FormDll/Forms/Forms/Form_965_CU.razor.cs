using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using System.Collections.ObjectModel;
using Baya.Models.Utility.Entity;
using Baya.Models.Utility.Menu;
using Blazored.Toast.Services;
using Newtonsoft.Json.Linq;
using Utility;
using Baya.Models.ORM;

namespace Forms.Forms
{
    public class Form_965_CUBase : Form_965_CUPeropeties
    {

        // تابع پیام تُــست
        // public MSG _MSG { get; set; }


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
                // تعریف مدل پیام بر اساس تابع تعریف شده
                // _MSG = new MSG(toastService);
            }
        }

        /// <summary>
        /// اعتبار سنجی فرم
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> FormValidator()
        {
            bool IsValid = true;

            // بررسی تابع اعتبارسنجی فیلد ها
            //IsValid = await CheckFieldValidation(_Entity);

            return IsValid;
        }


        /// <summary>
        /// تابع قبل اجرا شدن ارسال داده
        /// </summary>
        /// <returns></returns>
        public override async Task<Result> BeforSubmit()
        {
            // var IsCancelled = !await ValidateEmployeeDetails(_Entity.HR_EMP_EmployeeInfos);
            // if(IsCancelled){
            //     return new Result() { Status = HttpStatusCode.BadRequest };
            // }
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


        // public async Task<bool> CheckFieldValidation(Entity.HR_EMP_Employees Item)
        // {
        //     bool IsValid = true;
        //     // **************************************************
        //     // // فیلد شرکت
        //     // if (Item.BaseInfo_ORG_CompaniesId == null || Item.BaseInfo_ORG_CompaniesId == Guid.Empty)
        //     // {
        //     //     IsValid = false;
        //     //     await _MSG.ShowError("لطفا گزینه شرکت را تکمیل نمایید.");
        //     // }

        //     // فیلد نام
        //     //if (MasterItem.FirstName == null)
        //     if (string.IsNullOrWhiteSpace(Item.FirstName))
        //     {
        //         IsValid = false;
        //         // await _MSG.ShowError("لطفا گزینه نام را تکمیل نمایید.");
        //     }

        //     // فیلد نام خانوادگی
        //     //if (MasterItem.FirstName == null)
        //     if (string.IsNullOrWhiteSpace(Item.LastName))
        //     {
        //         IsValid = false;
        //         // await _MSG.ShowError("لطفا گزینه نام خانوادگی را تکمیل نمایید.");
        //     }

        //     // فیلد وضعیت کارمند
        //     if (Item.HR_EMP_StatusId == null)
        //     {
        //         IsValid = false;
        //         // await _MSG.ShowError("لطفا گزینه وضعیت کارمند را تکمیل نمایید.");
        //     }

        //     // فیلد کد قدیم پرسنلی کارمند
        //     if (Item.EmployeeLastPersonelNo == null)
        //     {
        //         IsValid = false;
        //         // await _MSG.ShowError("لطفا گزینه کد قدیم پرسنلی کارمند را تکمیل نمایید.");
        //     }

        //     // فیلد کد کارمند
        //     if (Item.EmployeeNo == null)
        //     {
        //         IsValid = false;
        //         // await _MSG.ShowError("لطفا گزینه کد کارمند را تکمیل نمایید.");
        //     }

        //     // فیلد کد پرسنلی کارمند
        //     if (Item.EmployeePersonelNo == null)
        //     {
        //         IsValid = false;
        //         // await _MSG.ShowError("لطفا گزینه کد پرسنلی کارمند را تکمیل نمایید.");
        //     }

        //     // **************************************************

        //     // **************************************************
        //     // Details 
        //     // foreach (var item in _Entity.HR_EMP_EmployeeInfos)
        //     // {
        //     //     // فیلد نام پدر
        //     //     if (item.FatherName == null)
        //     //     {
        //     //         IsValid = false;
        //     //         await _MSG.ShowError("لطفا گزینه نام پدر را تکمیل نمایید.");
        //     //     }

        //     //     // فیلد کد ملی
        //     //     if (item.NationalCode == null)
        //     //     {
        //     //         IsValid = false;
        //     //         await _MSG.ShowError("لطفا گزینه کد ملی را تکمیل نمایید.");
        //     //     }
        //     // }


        //     // **************************************************


        //     return IsValid;
        // }

        /// <summary>
        ///  تابعی برای بررسی تعداد ردیف‌های جزئیات
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        // public async Task<bool> ValidateEmployeeDetails(ICollection<Entity.HR_EMP_EmployeeInfos> Item)
        // {
        //     if (Item != null && Item.Count > 1)
        //     {
        //         await _MSG.ShowError("شما مجاز به ثبت یک ردیف بیشتر نیستید!!");
        //         return false;
        //     }
        //     return true;
        // }

             

        
       
		#endregion FunctionEvents

    }
}