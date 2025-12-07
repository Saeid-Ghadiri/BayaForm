using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using DateUtils;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Forms.Forms
{
    public class Form_993Base : Form_993Peropeties
    {
        // تابع پیام تُــست
        public MSG _MSG { get; set; }


        /// <summary>
        /// آماده سازی فرم
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            ///Item.IsPostponedPurchase = null;
                    
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

            var List = _Entity.SCM_ProductRequestDetails.ToList();

            foreach (var Item in List)
            {
                // IsValid = IsValid && await CheckFieldValidation(Item);                

                if (!await CheckFieldValidation(Item))
                {
                    IsValid = false;
                    // نمایش خطاها درون CheckFieldValidation انجام می‌شود
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

        private async Task<bool> CheckFieldValidation(Entity.SCM_ProductRequestDetails item)
        {
            bool isValid = true;

            // 1. آیا این خرید بعداً انجام می‌گردد؟ — همیشه الزامی
            if (item.IsPostponedPurchase == null)
            {
                isValid = false;
                await _MSG.ShowError("لطفاً گزینه «آیا این خرید بعداً انجام می‌گردد؟» را انتخاب کنید.");
            }

            // 2. آیا خرید مازاد دارد؟ — همیشه الزامی (مستقل از IsPostponedPurchase)
            if (item.SurplusProductIsEnable == null)
            {
                isValid = false;
                await _MSG.ShowError("لطفاً مشخص کنید «آیا خرید تعداد / مقدار مازاد دارد؟»");
            }
            else if (item.SurplusProductIsEnable == true && item.NumberOfSurplusProduct == null)
            {
                isValid = false;
                await _MSG.ShowError("لطفاً «تعداد / مقدار خرید مازاد» را وارد کنید.");
            }

            // اگر خرید به بعد موکول شده باشد (بله)، دیگر نیازی به چک کردن مراحل تحویل نیست
            if (item.IsPostponedPurchase == true)
            {
                return isValid; // فقط مازاد چک شد، تحویل نادیده گرفته شد
            }

            // از اینجا به بعد: فقط وقتی IsPostponedPurchase == false است

            // 3. تعداد مراحل واگذاری کالا — الزامی
            if (item.SCM_NumTransfersGoodsWarehouseId == null)
            {
                isValid = false;
                await _MSG.ShowError("لطفاً «تعداد مراحل واگذاری کالا به انبار» را انتخاب کنید.");
            }

            // 4. اعتبارسنجی مقادیر تحویل بر اساس مراحل انتخاب‌شده
            var step1 = "82329c07-2aa7-ef11-8354-005056a02a64";
            //var step1 = Guid.Parse("82329c07-2aa7-ef11-8354-005056a02a64");
            var step2 = "cc59710f-2aa7-ef11-8354-005056a02a64";
            //var step2 = Guid.Parse("cc59710f-2aa7-ef11-8354-005056a02a64");
            var step3 = "13a46c17-2aa7-ef11-8354-005056a02a64";
            //var step3 = Guid.Parse("13a46c17-2aa7-ef11-8354-005056a02a64");

            string selectedStep = item.SCM_NumTransfersGoodsWarehouseId?.ToString();

            if (selectedStep == step1)
            {
                if (item.TheNumberDeliveredByLogistics == null)
                {
                    isValid = false;
                    await _MSG.ShowError("لطفاً «مقدار واگذاری کالا در مرحله 1» را وارد کنید.");
                }
            }
            else if (selectedStep == step2)
            {
                if (item.TheNumberDeliveredByLogistics == null || item.TheNumberDeliveredByLogistics2 == null)
                {
                    isValid = false;
                    await _MSG.ShowError("لطفاً مقادیر واگذاری کالا در مراحل 1 و 2 را وارد کنید.");
                }
            }
            else if (selectedStep == step3)
            {
                if (item.TheNumberDeliveredByLogistics == null ||
                    item.TheNumberDeliveredByLogistics2 == null ||
                    item.TheNumberDeliveredByLogistics3 == null)
                {
                    isValid = false;
                    await _MSG.ShowError("لطفاً مقادیر واگذاری کالا در مراحل 1، 2 و 3 را وارد کنید.");
                }
            }
            // اگر selectedStep هیچ‌کدام از موارد بالا نبود (مثلاً null یا مقدار نامعتبر)،
            // قبلاً با چک `SCM_NumTransfersGoodsWarehouseId == null` خطا داده شده است.

            return isValid;
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
            // آیا این خرید بعداً انجام می گردد؟                
            if (Item.IsPostponedPurchase != null)
            {
                await _MSG.ShowWarning("توجه: آیا این خرید بعداً انجام می گردد؟ قبلاً ثبت شده است — در صورت نیاز آن را اصلاح کنید.");
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

		#endregion FunctionEvents

    }
}