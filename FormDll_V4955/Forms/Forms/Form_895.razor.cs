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
    public class Form_895Base : Form_895Peropeties
    {

        // تابع پیام تُــست
        public MSG _MSG { get; set; }
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
                _DTC = new DateTimeConverter();

                // تکمیل ستون عناوین در گرید ارزیابی اولیه تامین کنندگان
                await PreAssessmentSupplier();

                // 
                //await PaymentMethodSupplier();
                await PaymentMethod_Supplier();

                //
                //await TransportationMethodSupplier();
                await TransportationMethod_Supplier();

                //
                //await InspectionMethodSupplier();
                await InspectionMethod_Supplier();
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



        /// <summary>
        /// گرید اصلی ارزیابی تامین کنندگان 
        /// تکمیل بخش عنوان
        /// </summary>
        /// <returns></returns>
        private async Task PreAssessmentSupplier()
        {
            //دریافت دیتای عنوان فرم
            var DataResult = await ApiServer.External.Services.Data.GetList("SCMPETCO_TitlePreAssessment_Supplier", 1000, 0, null, null);

            // Console.WriteLine("#Log Start :: ");
            // Console.WriteLine("#Log DataResult.Status : " + DataResult.Status);
            // Console.WriteLine("#Log DataResult.Content : " + DataResult.Content.ToString());

            List<Entity.SCMPETCO_TitlePreAssessment_Supplier> result =
                await JSON.ToObject<List<Entity.SCMPETCO_TitlePreAssessment_Supplier>>(DataResult.Content.ToString());

            if (_Entity.SCMPETCO_PreAssessment_Supplier.Count == 0)
            {
                // افزودن عنوان به هر ردیف از گرید
                foreach (var item in result)
                {
                    Entity.SCMPETCO_PreAssessment_Supplier PA_Supplier = new();

                    // Id از جدول خود عنوان هم باید باشد
                    PA_Supplier.SCMPETCO_TitlePreAssessment_SupplierId = item.Id;//شناسه
                    PA_Supplier.SCMPETCO_TitlePreAssessment_Supplier = item;//ابجکت
                    _Entity.SCMPETCO_PreAssessment_Supplier.Add(PA_Supplier);
                }
            }


            StateHasChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task InspectionMethod_Supplier()
        {
            //دریافت دیتای عنوان فرم
            var DataResult = await ApiServer.External.Services.Data.GetList("SCMPETCO_TitleInspectionMethod_Supplier", 1000, 0, null, null);

            List<Entity.SCMPETCO_TitleInspectionMethod_Supplier> result =
                await JSON.ToObject<List<Entity.SCMPETCO_TitleInspectionMethod_Supplier>>(DataResult.Content.ToString());

            if (_Entity.SCMPETCO_InspectionMethod_Supplier.Count == 0)
            {
                // افزودن عنوان به هر ردیف از گرید
                foreach (var item in result)
                {
                    Entity.SCMPETCO_InspectionMethod_Supplier InspectionMethod = new();

                    // Id از جدول خود عنوان هم باید باشد
                    InspectionMethod.SCMPETCO_TitleInspectionMethod_SupplierId = item.Id;//شناسه
                    InspectionMethod.SCMPETCO_TitleInspectionMethod_Supplier = item;//ابجکت
                    _Entity.SCMPETCO_InspectionMethod_Supplier.Add(InspectionMethod);
                }
            }


            StateHasChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task PaymentMethod_Supplier()
        {
            //دریافت دیتای عنوان فرم
            var DataResult = await ApiServer.External.Services.Data.GetList("SCMPETCO_TitlePaymentMethod_Supplier", 1000, 0, null, null);

            List<Entity.SCMPETCO_TitlePaymentMethod_Supplier> result =
                await JSON.ToObject<List<Entity.SCMPETCO_TitlePaymentMethod_Supplier>>(DataResult.Content.ToString());

            if (_Entity.SCMPETCO_PaymentMethod_Supplier.Count == 0)
            {
                // افزودن عنوان به هر ردیف از گرید
                foreach (var item in result)
                {
                    Entity.SCMPETCO_PaymentMethod_Supplier PaymentMethod_Supplier = new();

                    // Id از جدول خود عنوان هم باید باشد
                    PaymentMethod_Supplier.SCMPETCO_TitlePaymentMethod_SupplierId = item.Id;//شناسه
                    PaymentMethod_Supplier.SCMPETCO_TitlePaymentMethod_Supplier = item;//ابجکت
                    _Entity.SCMPETCO_PaymentMethod_Supplier.Add(PaymentMethod_Supplier);
                }
            }


            StateHasChanged();
        }

        private async Task TransportationMethod_Supplier()
        {
            //دریافت دیتای عنوان فرم
            var DataResult = await ApiServer.External.Services.Data.GetList("SCMPETCO_SCMPETCO_TitleTransportationMethod_Supplier", 1000, 0, null, null);

            List<Entity.SCMPETCO_SCMPETCO_TitleTransportationMethod_Supplier> result =
                await JSON.ToObject<List<Entity.SCMPETCO_SCMPETCO_TitleTransportationMethod_Supplier>>(DataResult.Content.ToString());

            if (_Entity.SCMPETCO_TransportationMethod_Supplier.Count == 0)
            {
                // افزودن عنوان به هر ردیف از گرید
                foreach (var item in result)
                {
                    Entity.SCMPETCO_TransportationMethod_Supplier Transportation_Supplier = new();
                    // Id از جدول خود عنوان هم باید باشد
                    Transportation_Supplier.SCMPETCO_SCMPETCO_TitleTransportationMethod_SupplierId = item.Id;//شناسه
                    Transportation_Supplier.SCMPETCO_SCMPETCO_TitleTransportationMethod_Supplier = item;//ابجکت
                    _Entity.SCMPETCO_TransportationMethod_Supplier.Add(Transportation_Supplier);
                }
            }


            StateHasChanged();
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //private async Task PaymentMethodSupplier()
        //{
        //    //دریافت دیتای عنوان فرم
        //    var DataResult = await ApiServer.External.Services.Data.GetList("SCMPETCO_PaymentMethod_Supplier", 1000, 0, null, null);

        //    List<Entity.SCMPETCO_PaymentMethod_Supplier> result =
        //        await JSON.ToObject<List<Entity.SCMPETCO_PaymentMethod_Supplier>>(DataResult.Content.ToString());

        //    if (_Entity.SCMPETCO_PaymentMethod_Supplier.Count == 0)
        //    {
        //        // افزودن عنوان به هر ردیف از گرید
        //        foreach (var item in result)
        //        {
        //            Entity.SCMPETCO_PaymentMethod_Supplier Payment_Supplier = new();
        //            Payment_Supplier.Title = item.Title;//ابجکت
        //            _Entity.SCMPETCO_PaymentMethod_Supplier.Add(Payment_Supplier);
        //        }
        //    }


        //    StateHasChanged();
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //private async Task TransportationMethodSupplier()
        //{
        //    //دریافت دیتای عنوان فرم
        //    var DataResult = await ApiServer.External.Services.Data.GetList("SCMPETCO_TransportationMethod_Supplier", 1000, 0, null, null);

        //    List<Entity.SCMPETCO_TransportationMethod_Supplier> result =
        //        await JSON.ToObject<List<Entity.SCMPETCO_TransportationMethod_Supplier>>(DataResult.Content.ToString());

        //    if (_Entity.SCMPETCO_TransportationMethod_Supplier.Count == 0)
        //    {
        //        // افزودن عنوان به هر ردیف از گرید
        //        foreach (var item in result)
        //        {
        //            Entity.SCMPETCO_TransportationMethod_Supplier Transportation_Supplier = new();
        //            Transportation_Supplier.Title = item.Title;//ابجکت
        //            _Entity.SCMPETCO_TransportationMethod_Supplier.Add(Transportation_Supplier);
        //        }
        //    }


        //    StateHasChanged();
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //private async Task InspectionMethodSupplier()
        //{
        //    //دریافت دیتای عنوان فرم
        //    var DataResult = await ApiServer.External.Services.Data.GetList("SCMPETCO_InspectionMethod_Supplier", 1000, 0, null, null);

        //    List<Entity.SCMPETCO_InspectionMethod_Supplier> result =
        //        await JSON.ToObject<List<Entity.SCMPETCO_InspectionMethod_Supplier>>(DataResult.Content.ToString());

        //    if (_Entity.SCMPETCO_InspectionMethod_Supplier.Count == 0)
        //    {
        //        // افزودن عنوان به هر ردیف از گرید
        //        foreach (var item in result)
        //        {
        //            Entity.SCMPETCO_InspectionMethod_Supplier Inspection_Supplier = new();
        //            Inspection_Supplier.Title = item.Title;//ابجکت
        //            _Entity.SCMPETCO_InspectionMethod_Supplier.Add(Inspection_Supplier);
        //        }
        //    }


        //    // روش دوم برای پر کردن گرید ولی همه فیلد ها - آقای کلهر
        //    // _Entity.SCMPETCO_InspectionMethod_Supplier = result;

        //    StateHasChanged();
        //}

        public async Task StartDate_onselect(EventArgs Selected, Entity.SCMPETCO_Partnerships_SupplierDetail Item)
        {
            // تبدیل تاریخ در زمان ثبت کد تحویل کالا توسط انباردار
            if (Item.StartDate != null)
            {
                var dt = _DTC.ConvertShamsiToMiladi(Item.StartDate);
                 Console.WriteLine(dt);
                Item.StartDateEN = dt;

            }

        }


        public void FinalScore()
        {

            var summ = _Entity.SCMPETCO_PreAssessment_Supplier.Sum(p => p.SCMPETCO_ScoringMethod_Supplier.ScoringNo);
            //return Convert.ToDecimal(summ);

            byte sum = 0;
            foreach (var item in _Entity.SCMPETCO_PreAssessment_Supplier)
            {
                if(item.SCMPETCO_ScoringMethod_Supplier.ScoringNo == null)
                {
                    //return 0;
                }
                var val = Convert.ToByte(item.SCMPETCO_ScoringMethod_Supplier.ScoringNo);
                sum += val;
            }

            Console.WriteLine("FinalScore " + sum);

            double emtiaz = (sum / 700) * 100;
            Console.WriteLine("emtiaz " + emtiaz);

            string grade = "";

            if (emtiaz >= 90)
                grade = "A";
            else if (emtiaz >= 80)
                grade = "B";
            else if (emtiaz >= 70)
                grade = "C";
            else if (emtiaz >= 60)
                grade = "D";
            else
                grade = "E";

            Console.WriteLine("Grade: " + grade);
        }


		#endregion FunctionEvents

    }
}