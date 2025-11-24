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
    public class Form_994Base : Form_994Peropeties
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
        public override async Task<bool> FormValidator()
        {

            bool IsValid = true;


            var List = _Entity.SCM_ProductRequestDetails.Where(p => p.IsDelete == false).ToList();


            //var listCount = List.Count();

            // Console.WriteLine("#Log FormValidator :");

            // SequenceFlow_12trjct		==> 
            // SequenceFlow_0svdiko		==> 
            // SequenceFlow_17vu1l8		==> 

            foreach (var Item in List)
            {
                //Console.WriteLine("#Log FormValidator btn foreach :");
                IsValid = IsValid && await CheckFieldValidation(Item);
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
            Console.WriteLine("IsCancelled : " + IsCancelled);

            return IsCancelled;
        }

        public async Task SCM_ProductRequestDetails_afterrendermodal(Entity.SCM_ProductRequestDetails Item)
        {
             // آیا این خرید بعداً انجام می گردد؟                
            if (Item.IsPostponedPurchase != null)
            {
                toastService.ShowWarning("توجه: آیا این خرید بعداً انجام می گردد؟ قبلاً ثبت شده است — در صورت نیاز آن را اصلاح کنید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
            }
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

            // استعلام تدراکات 1- SCM_ProductRequestDetails_InquiryFirst
            // استعلام تدارکات 2- SCM_ProductRequestDetails_InquirySecondFile
            // استعلام تدارکات 3- SCM_ProductRequestDetails_InquiryThirdFile
            var Inq1IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryFirst;
            var Inq2IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquirySecondFile;
            var Inq3IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryThirdFile;
            // نمایش / عدم نمایش فیلد منتج از ITIL
            if (Item.InquiryTrueFalse.HasValue && Item.InquiryTrueFalse.Value)
            {
                Inq1IsVisible.SetVisible(true);
                Inq2IsVisible.SetVisible(true);
                Inq3IsVisible.SetVisible(true);
            }
            else
            {
                Inq1IsVisible.SetVisible(false);
                Inq2IsVisible.SetVisible(false);
                Inq3IsVisible.SetVisible(false);
            }

            // 82329c07-2aa7-ef11-8354-005056a02a64     یک مرحله
            // cc59710f-2aa7-ef11-8354-005056a02a64     دو مرحله
            // 13a46c17-2aa7-ef11-8354-005056a02a64     سه مرحله

            // **************************************************

            // تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات- TheNumberDeliveredByLogistics
            // تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 2- TheNumberDeliveredByLogistics2
            // تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 3- TheNumberDeliveredByLogistics3
            var NDL1IsVisible = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics;
            var NDL2IsVisible = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2;
            var NDL3IsVisible = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3;
            // نمایش تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 
            if (Item.SCM_NumTransfersGoodsWarehouseId.ToString() == "82329c07-2aa7-ef11-8354-005056a02a64")
            {
                NDL1IsVisible.SetVisible(true); 
            }
            else 
            {
                NDL1IsVisible.SetVisible(false);
            }

            if (Item.SCM_NumTransfersGoodsWarehouseId.ToString() == "cc59710f-2aa7-ef11-8354-005056a02a64")
            {
                NDL1IsVisible.SetVisible(true);
                NDL2IsVisible.SetVisible(true);
            }
            else
            {
                NDL1IsVisible.SetVisible(false);
                NDL2IsVisible.SetVisible(false);
            }

            if (Item.SCM_NumTransfersGoodsWarehouseId.ToString() == "13a46c17-2aa7-ef11-8354-005056a02a64")
            {
                NDL1IsVisible.SetVisible(true);
                NDL2IsVisible.SetVisible(true);
                NDL3IsVisible.SetVisible(true);
            }
            else
            {
                NDL1IsVisible.SetVisible(false);
                NDL2IsVisible.SetVisible(false);
                NDL3IsVisible.SetVisible(false);
            }

        }


        public async Task InquiryTrueFalse_oninput(ChangeEventArgs Selected, Entity.SCM_ProductRequestDetails Item)
        {
            // استعلام تدراکات 1- SCM_ProductRequestDetails_InquiryFirst
            // استعلام تدارکات 2- SCM_ProductRequestDetails_InquirySecondFile
            // استعلام تدارکات 3- SCM_ProductRequestDetails_InquiryThirdFile
            var Inq1IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryFirst;
            var Inq2IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquirySecondFile;
            var Inq3IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryThirdFile;
            // نمایش فایل های بارگذاری استعلام
            if (Selected.Value.ToString() == "true")
            {
                Inq1IsVisible.SetVisible(true);
                Inq2IsVisible.SetVisible(true);
                Inq3IsVisible.SetVisible(true);
            }
            else
            {
                Inq1IsVisible.SetVisible(false);
                Inq2IsVisible.SetVisible(false);
                Inq3IsVisible.SetVisible(false);
            }
        }



        public async Task SCM_NumTransfersGoodsWarehouseId_onitemselected(Entity.SCM_NumTransfersGoodsWarehouse Selected, Entity.SCM_ProductRequestDetails Item)
        {

            
            // 82329c07-2aa7-ef11-8354-005056a02a64     یک مرحله
            // cc59710f-2aa7-ef11-8354-005056a02a64     دو مرحله
            // 13a46c17-2aa7-ef11-8354-005056a02a64     سه مرحله

            // **************************************************

            // تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات- TheNumberDeliveredByLogistics
            // تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 2- TheNumberDeliveredByLogistics2
            // تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 3- TheNumberDeliveredByLogistics3

            var NDL1IsVisible = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics;
            var NDL2IsVisible = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2;
            var NDL3IsVisible = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3;

            // Console.WriteLine((Selected.Id.ToString() == "82329c07-2aa7-ef11-8354-005056a02a64").ToString());
            // Console.WriteLine((Selected.Id.ToString() == "cc59710f-2aa7-ef11-8354-005056a02a64").ToString());
            // Console.WriteLine((Selected.Id.ToString() == "13a46c17-2aa7-ef11-8354-005056a02a64").ToString());
            // نمایش تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 
            if (Selected.Id.ToString() == "82329c07-2aa7-ef11-8354-005056a02a64")
            {
                NDL1IsVisible.SetVisible(true); 
                NDL2IsVisible.SetVisible(false);
                NDL3IsVisible.SetVisible(false);
            }
            else if (Selected.Id.ToString() == "cc59710f-2aa7-ef11-8354-005056a02a64")
            {
                NDL1IsVisible.SetVisible(true);
                NDL2IsVisible.SetVisible(true); 
                NDL3IsVisible.SetVisible(false);
            }
            else if (Selected.Id.ToString() == "13a46c17-2aa7-ef11-8354-005056a02a64")
            {
                NDL1IsVisible.SetVisible(true);
                NDL2IsVisible.SetVisible(true);
                NDL3IsVisible.SetVisible(true);
            }

            

        }


        public async Task<bool> CheckFieldValidation(Entity.SCM_ProductRequestDetails Item)
        {
            bool IsValid = true;

            // آیا این خرید بعداً انجام می گردد؟
            if (Item.IsPostponedPurchase == null)
            {
                //IsValid = false;
                //Console.WriteLine("IsValid : " + IsValid);
                toastService.ShowError("لطفا گزینه آیا این خرید بعداً انجام می گردد؟ را تکمیل نمایید.",
                settings =>
                {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });

                return false;
            }
            Console.WriteLine("IsValid ref : " + Ref_SCM_ProductRequestDetails_IsPostponedPurchase.Value);
            bool IsPostponed = Convert.ToBoolean(Ref_SCM_ProductRequestDetails_IsPostponedPurchase.Value);
            if (IsPostponed)
            {
                return true;
            }

            // استعلام تدراکات 1
            if (Item.InquiryTrueFalse.HasValue && Item.InquiryTrueFalse.Value)
            {
                // SCM_ProductRequestDetails_InquiryFirst
                if (Item.SCM_ProductRequestDetails_InquiryFirst == null ||
                  Item.SCM_ProductRequestDetails_InquiryFirst.Count < 1)
                {
                    IsValid = false;
                    Console.WriteLine("IsValid : " + IsValid);
                    toastService.ShowError("لطفا استعلام 1 را بارگذاری نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
            }


            if (Item.InquiryTrueFalse.HasValue && !Item.InquiryTrueFalse.Value)
            {
                // if(Item.SCM_NumTransfersGoodsWarehouseId == null || Item.SCM_NumTransfersGoodsWarehouseId == Guid.Empty)
                if (Item.SCM_NumTransfersGoodsWarehouseId == null)
                {
                    IsValid = false;
                    Console.WriteLine("IsValid : " + IsValid);
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

            
            // نمایش تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 
            if (Item.SCM_NumTransfersGoodsWarehouseId.ToString() == "82329c07-2aa7-ef11-8354-005056a02a64")
            {
                if (Item.TheNumberDeliveredByLogistics == null)
                {
                    IsValid = false;

                    toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 1 را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
            }

            if (Item.SCM_NumTransfersGoodsWarehouseId.ToString() == "cc59710f-2aa7-ef11-8354-005056a02a64")
            {
                if (Item.TheNumberDeliveredByLogistics == null || Item.TheNumberDeliveredByLogistics2 == null)
                {
                    IsValid = false;

                    toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 1 و 2 را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
            }

            if (Item.SCM_NumTransfersGoodsWarehouseId.ToString() == "13a46c17-2aa7-ef11-8354-005056a02a64")
            {
                if (Item.TheNumberDeliveredByLogistics == null || Item.TheNumberDeliveredByLogistics2 == null || Item.TheNumberDeliveredByLogistics3 == null)
                {
                    IsValid = false;

                    toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 1 و 2 و 3 را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
            }



            // آیا خرید تعداد / مقدار مازاد دارد؟ - SurplusProductIsEnable
            if (Item.SurplusProductIsEnable == null)
            {
                IsValid = false;
                Console.WriteLine("IsValid : " + IsValid);
                toastService.ShowError("لطفا گزینه آیا خرید تعداد / مقدار مازاد دارد؟ را تکمیل نمایید.",
                settings =>
                {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }

            // آیا خرید تعداد / مقدار مازاد دارد؟
            if (Item.SurplusProductIsEnable.HasValue && Item.SurplusProductIsEnable.Value)
            {
                // تعداد / مقدار خرید مازاد
                if (Item.NumberOfSurplusProduct == null)
                {
                    IsValid = false;
                    Console.WriteLine("IsValid : " + IsValid);
                    toastService.ShowError("لطفا گزینه تعداد / مقدار خرید مازاد را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
            }

           
            Console.WriteLine("Return IsValid : " + IsValid);




          
            return IsValid;
        }

        #endregion FunctionEvents

    }
}
