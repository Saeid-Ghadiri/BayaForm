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
    public class Form_525Base : Form_525Peropeties
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

            var List = _Entity.SCM_ProductRequestDetails.Where(p => p.IsDelete == false).ToList();

            var listCount = List.Count();

            // دکمه ثبت و ادامه           
            if (BtnWorkFlowId == "SequenceFlow_05e9n6q")
            {
                foreach (var Item in List)
                {
                    IsValid = IsValid && await CheckFieldValidation(Item);
                }
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

            // آیا خرید تعداد / مقدار مازاد دارد؟
            if (Item.SurplusProductIsEnable == null)
            {
                IsValid = false;
                toastService.ShowError("لطفا گزینه آیا خرید تعداد / مقدار مازاد دارد؟ را تکمیل نمایید.",
                settings =>
                {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }

            if (Item.InquiryTrueFalse.HasValue && Item.InquiryTrueFalse.Value)
            {
                // if(Item.SCM_NumTransfersGoodsWarehouseId == null || Item.SCM_NumTransfersGoodsWarehouseId == Guid.Empty)
                if (Item.SCM_NumTransfersGoodsWarehouseId == null)
                {
                    IsValid = false;
                    toastService.ShowError("لطفا گزینه تعداد مراحل واگذاری را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
            }

            // 82329c07-2aa7-ef11-8354-005056a02a64     یک مرحله
            // cc59710f-2aa7-ef11-8354-005056a02a64     دو مرحله
            // 13a46c17-2aa7-ef11-8354-005056a02a64     سه مرحله

            // if(Item.SCM_NumTransfersGoodsWarehouseId.ToString() =="82329c07-2aa7-ef11-8354-005056a02a64")
            // {
            //     // TheNumberDeliveredByLogistics
            //     //if(String.IsNullOrEmpty(Item.TheNumberDeliveredByLogistics))                 
            //     if(Item.TheNumberDeliveredByLogistics == null)
            //     {
            //     	IsValid = false;
            //     	toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری 1 را تکمیل نمایید.",
            //     	settings => {
            //     		settings.Timeout = 4;
            //     		settings.ShowProgressBar = true;
            //     		settings.PauseProgressOnHover = true;
            //     	});
            //     }
            // }

            // خرید مرحله 1
            if (Item.SCM_NumTransfersGoodsWarehouseId.ToString() == "82329c07-2aa7-ef11-8354-005056a02a64")
            {
                //Console.WriteLine("#Log0-01:: Marahel Vagozari: 11");
                if (Item.TheNumberDeliveredByLogistics == null)
                {
                    //Console.WriteLine("#Log0-02:: Marahel Vagozari: 11");
                    IsValid = false;
                    toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
            }
            else if (Item.SCM_NumTransfersGoodsWarehouseId.ToString() == "cc59710f-2aa7-ef11-8354-005056a02a64")
            {
                if (Item.TheNumberDeliveredByLogistics == null)
                {
                    IsValid = false;
                    toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }

                if (Item.TheNumberDeliveredByLogistics2 == null)
                {
                    IsValid = false;
                    toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 2 را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }


            }
            else if (Item.SCM_NumTransfersGoodsWarehouseId.ToString() == "13a46c17-2aa7-ef11-8354-005056a02a64")
            
            {

                if (Item.TheNumberDeliveredByLogistics == null)
                {
                    IsValid = false;
                    toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }

                if (Item.TheNumberDeliveredByLogistics2 == null)
                {
                    IsValid = false;
                    toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 2 را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
                if (Item.TheNumberDeliveredByLogistics3 == null)
                {
                    IsValid = false;
                    toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 3 را تکمیل نمایید.",
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


        public async Task InquiryIsVisible(bool Visible)
        {
            // استعلام شماره 1
            Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryFirst.SetVisible(Visible);
            // استعلام شماره 2
            Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquirySecondFile.SetVisible(Visible);
            // استعلام شماره 3
            Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryThirdFile.SetVisible(Visible);
        }

        public async Task LogisticDeliveryIsVisible(bool Visible)
        {
            // تعداد مراحل واگذاری کالا به انباردار
            Ref_SCM_ProductRequestDetails_SCM_NumTransfersGoodsWarehouseId.SetVisible(Visible);
            // تعداد یا مقدار واگذاری کالا 1
            Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics.SetVisible(Visible);
            // تعداد یا مقدار واگذاری کالا 2
            Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2.SetVisible(Visible);
            // تعداد یا مقدار واگذاری کالا 3
            Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3.SetVisible(Visible);
        }


        public async Task LogisticDeliveryOkIsVisible(Entity.SCM_ProductRequestDetails Item)
        {
            if (Item.SCM_NumTransfersGoodsWarehouseId == null || Item.SCM_NumTransfersGoodsWarehouseId == Guid.Empty)
            {
                //Console.WriteLine("log 2 SCM_NumTransfersGoodsWarehouseId");
                // تعداد یا مقدار واگذاری کالا 1
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics.SetVisible(false);
                // تعداد یا مقدار واگذاری کالا 2
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2.SetVisible(false);
                // تعداد یا مقدار واگذاری کالا 3
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3.SetVisible(false);
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
                }
            }

        }

        public async Task SurplusProductIsEnable_oninput(ChangeEventArgs Selected, Entity.SCM_ProductRequestDetails Item)
        {
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

        public async Task RepeatDeliveryProductIsEnable_oninput(ChangeEventArgs Selected, Entity.SCM_ProductRequestDetails Item)
        {
            var Item2 = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2;

            if (Selected.Value.ToString() == "true")
            {
                Item2.SetVisible(true);
            }
            else
            {
                Item2.SetVisible(false);
            }
        }

        public async Task ProductDataSheetTrueFalse_oninput(ChangeEventArgs Selected, Entity.SCM_ProductRequestDetails Item)
        {
            // Note: مخفی کردن فیلد بارگذاری فایل تی دی اس، بر اساس فیلد آیا دیتاشیت دارد یا خیر؟
            var Item2 = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductDataSheetFile;

            if (Selected.Value.ToString() == "true")
            {
                Item2.SetVisible(true);
            }
            else
            {
                Item2.SetVisible(false);
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
            // Note: مخفی کردن فیلد بارگذاری فایل تی دی اس، بر اساس فیلد آیا دیتاشیت دارد یا خیر؟ 
            if (Item.ProductDataSheetTrueFalse.HasValue && Item.ProductDataSheetTrueFalse.Value)
            {
                Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductDataSheetFile.SetVisible(true);
            }
            else
            {
                Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductDataSheetFile.SetVisible(false);
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

            // // آیا نیاز به استعلام دارد؟
            // Item.InquiryTrueFalse = false;
            // وضعیت فایل های بارگذاری استعلام
            if (Item.InquiryTrueFalse.HasValue && Item.InquiryTrueFalse.Value)
            {
                // // استعلام شماره 1
                // Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryFirst.SetVisible(true);				
                // // استعلام شماره 2
                // Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquirySecondFile.SetVisible(true);                
                // // استعلام شماره 3
                // Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryThirdFile.SetVisible(true); 

                // 
                await InquiryIsVisible(true);
                // Ref_SCM_ProductRequestDetails_SCM_NumTransfersGoodsWarehouseId.SetVisible(false);
                //await LogisticDeliveryOkIsVisible(Item);
                await LogisticDeliveryIsVisible(true);

            }
            else
            {
                // // استعلام شماره 1
                // Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryFirst.SetVisible(false);				
                // // استعلام شماره 2
                // Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquirySecondFile.SetVisible(false);                
                // // استعلام شماره 3
                // Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryThirdFile.SetVisible(false);

                // 
                await InquiryIsVisible(false);
                //Ref_SCM_ProductRequestDetails_SCM_NumTransfersGoodsWarehouseId.SetVisible(true);

                // await LogisticDeliveryOkIsVisible(Item);

            }

            await LogisticDeliveryOkIsVisible(Item);

        }

        public async Task InquiryTrueFalse_oninput(ChangeEventArgs Selected, Entity.SCM_ProductRequestDetails Item)
        {
            // استعلام شماره 1
            var InqueryF1IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryFirst;
            // استعلام شماره 2
            var InqueryF2IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquirySecondFile;
            // استعلام شماره 3
            var InqueryF3IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryThirdFile;

            // var MarahelTahvilIsVisible  = Ref_SCM_ProductRequestDetails_SCM_NumTransfersGoodsWarehouseId;

            // وضعیت فایل های بارگذاری استعلام
            if (Selected.Value.ToString() == "true")
            {
                // استعلام شماره 1
                InqueryF1IsVisible.SetVisible(true);
                // استعلام شماره 2
                InqueryF2IsVisible.SetVisible(true);
                // استعلام شماره 3
                InqueryF3IsVisible.SetVisible(true);

                // MarahelTahvilIsVisible.SetVisible(false); 
                // await LogisticDeliveryOkIsVisible(Item);
                await LogisticDeliveryIsVisible(true);
            }
            else
            {
                // استعلام شماره 1
                InqueryF1IsVisible.SetVisible(false);
                // استعلام شماره 2
                InqueryF2IsVisible.SetVisible(false);
                // استعلام شماره 3
                InqueryF3IsVisible.SetVisible(false);

                // MarahelTahvilIsVisible.SetVisible(true);   			
            }

        }

        public async Task SCM_NumTransfersGoodsWarehouseId_onitemselected(Entity.SCM_NumTransfersGoodsWarehouse Selected, Entity.SCM_ProductRequestDetails Item)
        {
            // شرط یک مرحله ای
            if (Selected.code.ToString() == "1001")
            {
                // تعداد یا مقدار واگذاری کالا 1
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics.SetVisible(true);
                // تعداد یا مقدار واگذاری کالا 2
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2.SetVisible(false);
                // تعداد یا مقدار واگذاری کالا 3
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3.SetVisible(false);
            }

            // شرط دو مرحله ای
            if (Selected.code.ToString() == "1002")
            {
                // تعداد یا مقدار واگذاری کالا 1
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics.SetVisible(true);
                // تعداد یا مقدار واگذاری کالا 2
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2.SetVisible(true);
                // تعداد یا مقدار واگذاری کالا 3
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3.SetVisible(false);
            }

            // شرط سه مرحله ای
            if (Selected.code.ToString() == "1003")
            {
                // تعداد یا مقدار واگذاری کالا 1
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics.SetVisible(true);
                // تعداد یا مقدار واگذاری کالا 2
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2.SetVisible(true);
                // تعداد یا مقدار واگذاری کالا 3
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3.SetVisible(true);
            }
        }
    }

    #endregion FunctionEvents
}