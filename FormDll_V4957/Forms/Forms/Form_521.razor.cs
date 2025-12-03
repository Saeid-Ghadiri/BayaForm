using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
namespace Forms.Forms
{
    public class Form_521Base : Form_521Peropeties
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

            var List = _Entity.SCM_ProductRequestDetails.ToList();

            // دکمه ثبت و ادامه

            if (BtnWorkFlowId == "SequenceFlow_1lvyepc")
            {
                // Console.WriteLine("#Log FormValidator btn :");
                foreach (var Item in List)
                {
                    //Console.WriteLine("#Log FormValidator btn foreach :");

                    IsValid = IsValid && await CheckFieldValidation(Item);
                }
            }

            foreach (var Item in List)
            {

            }

            return IsValid;
        }

        /// <summary>
        /// ارسال داده
        /// </summary>
        /// <returns></returns>
        public override async Task<Baya.Models.Utility.Result> Submit()
        {
            SumaryMessage = "";
            Baya.Models.Utility.Result Result = new Baya.Models.Utility.Result();

            if (!await FormValidator())
            {
                StateHasChanged();
                Result.Status = HttpStatusCode.InternalServerError;
                return Result;
            }

            await BeforSubmit();

            var Resualt = await PostData();

            if (Resualt)
            {
                Result.Status = HttpStatusCode.OK;

                SumaryMessage = "داده ها با موفقیت ثبت شد";
            }
            else
            {
                Result.Status = HttpStatusCode.InternalServerError;
                SumaryMessage = "ذخیره داده با مشکل مواجه شد";
            }

            Result.Message = SumaryMessage;

            switch ((int)Result.Status)
            {
                case 200:
                    toastService.ShowSuccess(Result.Message);
                    break;
                case 500:
                    toastService.ShowError(Result.Message);
                    break;
                default:
                    break;
            }

            await AfterSubmit();

            return Result;
        }

        /// <summary>
        /// ارسال داده
        /// </summary>
        /// <returns></returns>
        private async Task<bool> PostData()
        {
            string Data = await Utility.JSON.ToJson(_Entity);

            bool IsOk = false;
            var Model = await ApiServer.External.Services.Data.Put(Data, TablePost.Name, TablePost, RequestID?.ToString(), _User.UserID.ToString());
            if (Model?.Status == HttpStatusCode.OK)
            {
                IsOk = true;
            }

            return IsOk;
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
        /// بررسی نوع درخواست انتخابی توسط انباردار
        ///
        /// Id										    Title
        /// *********************************************************
        /// a9c5df1c-d99c-ef11-8354-005056a02a64		تحویل کالا
        /// 3b9e934d-d99c-ef11-8354-005056a02a64		خرید کالا
        /// 73f6c459-d99c-ef11-8354-005056a02a64		تحویل و خرید کالا
        /// 
        /// بررسی ضرورت تکمیل فیلدها از این تابع انجام می شود.
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public async Task<bool> CheckFieldValidation(Entity.SCM_ProductRequestDetails Item)
        {
            bool IsValid = true;

            var List = _Entity.SCM_ProductRequestDetails.ToList();

            // تعداد یا مقدار واگذاری کالا 1
            if (Item.NumberofProductDelivery1 == null || Item.NumberofProductDelivery1.Value == 0)
            {
                IsValid = false;
                toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا 1 را تکمیل نمایید.",
                settings =>
                {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }

            //
            if (Item.TheNumberDeliveredByLogistics2.HasValue && Item.TheNumberDeliveredByLogistics2.Value != 0)
            {
                if (Item.NumberofProductDelivery2 == null || Item.NumberofProductDelivery2.Value == 0)
                {
                    IsValid = false;
                    toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا 2 را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
                // تعداد یا مقدار واگذاری کالا 1
                if (Item.NumberofProductDelivery1 == null || Item.NumberofProductDelivery1.Value == 0)
                {
                    IsValid = false;
                    toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا 1 را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
            }

            if (Item.TheNumberDeliveredByLogistics3.HasValue && Item.TheNumberDeliveredByLogistics3.Value != 0)
            {
                if (Item.NumberofProductDelivery3 == null || Item.NumberofProductDelivery3.Value == 0)
                {
                    IsValid = false;
                    toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا 3 را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
                if (Item.NumberofProductDelivery2 == null || Item.NumberofProductDelivery2.Value == 0)
                {
                    IsValid = false;
                    toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا 2 را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
                // تعداد یا مقدار واگذاری کالا 1
                if (Item.NumberofProductDelivery1 == null || Item.NumberofProductDelivery1.Value == 0)
                {
                    IsValid = false;
                    toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا 1 را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
            }

            return IsValid;
        }


        // public async Task InquiryIsVisible(bool Visible)
        // {
        //     // استعلام شماره 1
        //     Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryFirst.SetVisible(Visible);
        //     // استعلام شماره 2
        //     Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquirySecondFile.SetVisible(Visible);
        //     // استعلام شماره 3
        //     Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryThirdFile.SetVisible(Visible);
        // }

        public async Task LogisticDeliveryIsVisible1(bool Visible)
        {
            // تعداد مراحل واگذاری کالا به انباردار
            Ref_SCM_ProductRequestDetails_SCM_NumTransfersGoodsWarehouseId.SetVisible(Visible);
            // تعداد یا مقدار واگذاری کالا 1
            Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics.SetVisible(Visible);
            // تعداد یا مقدار واگذاری کالا 2
            Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2.SetVisible(Visible);
            // تعداد یا مقدار واگذاری کالا 3
            Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3.SetVisible(Visible);
            //انبار
            // تعداد یا مقدار واگذاری کالا 1
            Ref_SCM_ProductRequestDetails_NumberofProductDelivery1.SetVisible(Visible);
            // تعداد یا مقدار واگذاری کالا 2
            Ref_SCM_ProductRequestDetails_NumberofProductDelivery2.SetVisible(Visible);
            // تعداد یا مقدار واگذاری کالا 3
            Ref_SCM_ProductRequestDetails_NumberofProductDelivery3.SetVisible(Visible);
        }



        public async Task LogisticDeliveryOkIsVisible1(Entity.SCM_ProductRequestDetails Item)
        {
            //if (!(Item.SCM_NumTransfersGoodsWarehouseId == null || Item.SCM_NumTransfersGoodsWarehouseId == Guid.Empty))
             Console.WriteLine("log 2 SCM_NumTransfersGoodsWarehouseId " + Item.SCM_NumTransfersGoodsWarehouseId.ToString());
            // if (Item.SCM_NumTransfersGoodsWarehouseId != Guid.Empty) 
            if ((Item.SCM_NumTransfersGoodsWarehouseId == null || Item.SCM_NumTransfersGoodsWarehouseId == Guid.Empty))
            {
                Console.WriteLine("log 2 SCM_NumTransfersGoodsWarehouseId");
                // تعداد یا مقدار واگذاری کالا 1
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics.SetVisible(false);
                // تعداد یا مقدار واگذاری کالا 2
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2.SetVisible(false);
                // تعداد یا مقدار واگذاری کالا 3
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3.SetVisible(false);

                Console.WriteLine("#Log anbar");
                //انبار
                // تعداد یا مقدار واگذاری کالا 1
                Ref_SCM_ProductRequestDetails_NumberofProductDelivery1.SetVisible(false);
                Console.WriteLine("#Log anbar1");
                // تعداد یا مقدار واگذاری کالا 2
                Ref_SCM_ProductRequestDetails_NumberofProductDelivery2.SetVisible(false);
                // تعداد یا مقدار واگذاری کالا 3
                Ref_SCM_ProductRequestDetails_NumberofProductDelivery3.SetVisible(false);
            }
            else
            {
                // شرط یک مرحله ای    
                if (Item.SCM_NumTransfersGoodsWarehouse.code.ToString() == "1001")
                {
                    // تعداد یا مقدار واگذاری کالا 1
                    Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 2
                    Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2.SetVisible(false);
                    // تعداد یا مقدار واگذاری کالا 3
                    Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3.SetVisible(false);
                    //انبار
                    // تعداد یا مقدار واگذاری کالا 1
                    Ref_SCM_ProductRequestDetails_NumberofProductDelivery1.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 2
                    Ref_SCM_ProductRequestDetails_NumberofProductDelivery2.SetVisible(false);
                    // تعداد یا مقدار واگذاری کالا 3
                    Ref_SCM_ProductRequestDetails_NumberofProductDelivery3.SetVisible(false);
                }

                // شرط دو مرحله ای
                if (Item.SCM_NumTransfersGoodsWarehouse.code.ToString() == "1002")
                {
                    // تعداد یا مقدار واگذاری کالا 1
                    Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 2
                    Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 3
                    Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3.SetVisible(false);
                    //انبار
                    // تعداد یا مقدار واگذاری کالا 1
                    Ref_SCM_ProductRequestDetails_NumberofProductDelivery1.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 2
                    Ref_SCM_ProductRequestDetails_NumberofProductDelivery2.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 3
                    Ref_SCM_ProductRequestDetails_NumberofProductDelivery3.SetVisible(false);
                }

                // شرط سه مرحله ای
                if (Item.SCM_NumTransfersGoodsWarehouse.code.ToString() == "1003")
                {
                    // تعداد یا مقدار واگذاری کالا 1
                    Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 2
                    Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 3
                    Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3.SetVisible(true);
                    //انبار
                    // تعداد یا مقدار واگذاری کالا 1
                    Ref_SCM_ProductRequestDetails_NumberofProductDelivery1.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 2
                    Ref_SCM_ProductRequestDetails_NumberofProductDelivery2.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 3
                    Ref_SCM_ProductRequestDetails_NumberofProductDelivery3.SetVisible(true);

                }
            }

        }

        public async Task<bool> SCM_ProductRequestDetails_editmodelsaving(object e)
        {
            bool IsCancelled = false;
            var Item = (Entity.SCM_ProductRequestDetails)e;

            IsCancelled = !await CheckFieldValidation(Item);

            return IsCancelled;
        }

        public async Task SCM_ProductRequestDetails_afterrendermodal(Entity.SCM_ProductRequestDetails Item)
        {
            var SurplusProductIsVisible = Ref_SCM_ProductRequestDetails_SurplusProductIsEnable;

            if (Item.SurplusProductIsEnable.HasValue && Item.SurplusProductIsEnable.Value)
            {
                SurplusProductIsVisible.SetVisible(false);
            }
            else
            {
                SurplusProductIsVisible.SetVisible(true);
            }

            // مخفی کردن فیلد تعداد یا مقدار مازاد
            if (Item.SurplusProductIsEnable.HasValue && Item.SurplusProductIsEnable.Value)
            {
                Ref_SCM_ProductRequestDetails_NumberOfSurplusProduct.SetVisible(true);
            }
            else
            {
                Ref_SCM_ProductRequestDetails_NumberOfSurplusProduct.SetVisible(false);
            }

            if (!(Item.SCM_NumTransfersGoodsWarehouseId == null || Item.SCM_NumTransfersGoodsWarehouseId == Guid.Empty))
            {
                Console.WriteLine("#log 000::");

                await LogisticDeliveryIsVisible1(true);
                await LogisticDeliveryOkIsVisible1(Item);
            }
        }

        public async Task SurplusProductIsEnable_oninput(ChangeEventArgs Selected, Entity.SCM_ProductRequestDetails Item)
        {
            var SurplusProductIsVisible = Ref_SCM_ProductRequestDetails_SurplusProductIsEnable;

            //
            if (Selected.Value.ToString() == "true")
            {
                SurplusProductIsVisible.SetVisible(false);
            }
            else
            {
                SurplusProductIsVisible.SetVisible(true);
            }

            // Note: آیا خرید تعداد /مقدار مازاد دارد؟
            var Item2 = Ref_SCM_ProductRequestDetails_NumberOfSurplusProduct;

            if (Selected.Value.ToString() == "true")
            {
                Item2.SetVisible(true);
            }
            else
            {
                Item2.SetVisible(false);
            }
        }

        public async Task SCM_NumTransfersGoodsWarehouseId_onitemselected(Entity.SCM_NumTransfersGoodsWarehouse Selected, Entity.SCM_ProductRequestDetails Item)
        {
           await LogisticDeliveryOkIsVisible1(Item);
        }

        #endregion FunctionEvents

    }
}