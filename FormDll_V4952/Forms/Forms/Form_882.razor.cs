using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using Blazored.Toast.Services;
using Entity;
using Newtonsoft.Json.Linq;
using Baya.Models.ORM;
using Utility;
using Baya.Models.Utility.Entity;
using System.Globalization;

namespace Forms.Forms
{
    public class Form_882Base : Form_882Peropeties
    {

        // پیام تُــست
        public MSG _MSG { get; set; }

        // تبدیل تاریخ
        public DateTimeConverter _DTC { get; set; }

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
                _MSG = new MSG(toastService);

                // تبدیل تاریخ
                _DTC = new DateTimeConverter();
            }
        }

        /// <summary>
        /// اعتبار سنجی فرم
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> FormValidator()
        {
            bool IsValid = true;

            //// بررسی تابع اعتبارسنجی فیلد ها
            //IsValid = await CheckFieldValidation(_Entity);

            return IsValid;
        }


        /// <summary>
        /// تابع قبل اجرا شدن ارسال داده
        /// </summary>
        /// <returns></returns>
        public override async Task<Result> BeforSubmit()
        {
            foreach (var item in _Entity.SCMPETCO_Partnerships_SupplierDetail)
            {
                if (item.StartDate != null)
                {
                    Console.WriteLine("#Log selected date:" + item.StartDate);
                    var datee = ConvertShamsiToMiladiWithTime(item.StartDate);
                    Console.WriteLine("#Log covert date:" + datee);
                    // var dt = _DTC.ConvertShamsiToMiladi(Item.StartDate);
                    // Console.WriteLine(dt);
                    item.StartDateEN = datee;
                }
            }
            

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

        //public async Task<bool> CheckFieldValidation(Entity.HR_EMP_Employees Item)
        //{
        //    bool IsValid = true;
        //    // **************************************************
        //    // فیلد شرکت
        //    if (Item.HR_Base_FactoryId == null)
        //    {
        //        IsValid = false;
        //        await _MSG.ShowError("لطفا گزینه شرکت را تکمیل نمایید.");
        //    }
        //    // **************************************************
        //    return IsValid;

        //}

        public async Task StartDate_onselect(EventArgs Selected, Entity.SCMPETCO_Partnerships_SupplierDetail Item)
        {
            // تبدیل تاریخ در زمان ثبت کد تحویل کالا توسط انباردار
            if (Item.StartDate != null)
            {
                Console.WriteLine("#Log selected date:"+ Item.StartDate);
                var datee = ConvertShamsiToMiladiWithTime(Item.StartDate);
                Console.WriteLine("#Log covert date:"+ datee);
                // var dt = _DTC.ConvertShamsiToMiladi(Item.StartDate);
                // Console.WriteLine(dt);
                 Item.StartDateEN = datee;

            }

        }

        public DateTime ConvertShamsiToMiladiWithTime(string shamsiDateTime)
        {
            var dateTimeParts = shamsiDateTime.Split(' ');
            var dateParts = dateTimeParts[0].Split('/');
            var timeParts = dateTimeParts.Length > 1 ? dateTimeParts[1].Split(':') : new[] { "0", "0" };

            int year = int.Parse(dateParts[0]);
            int month = int.Parse(dateParts[1]);
            int day = int.Parse(dateParts[2]);

            int hour = int.Parse(timeParts[0]);
            int minute = int.Parse(timeParts[1]);
            //int second = int.Parse(timeParts[2]);

            PersianCalendar pc = new PersianCalendar();
            return pc.ToDateTime(year, month, day, hour, minute, 0, 0);


            ////string shamsi = "1403/04/01 14:30:00";
            ////DateTime miladi = DateTimeConverter.ConvertShamsiToMiladi(shamsi);
            ////Console.WriteLine(miladi); // Output: 2024-06-21 14:30:00
        }
        #endregion FunctionEvents

    }
}
