using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using CurrieTechnologies.Razor.SweetAlert2;
using BlazorBootstrap;
using Blazored.Toast.Services;


namespace Forms.Forms
{
    public class Form_338Base : Form_338Peropeties
    {

        // SweetAlert2
        [Inject]
        public SweetAlertService Swal { get; set; }

        // Toast  
        [Inject]
        public IToastService toastService { get; set; }


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

            var List = _Entity.SCMPETCO_ProductRequestDetails.Where(p => p.IsDelete == false).ToList();

            var listCount = List.Count();

            // Console.WriteLine("#Log FormValidator :");

            // دکمه ثبت و ادامه           
            if ((BtnWorkFlowId == "SequenceFlow_0hwcmh9") || //رئیس برق
            (BtnWorkFlowId == "SequenceFlow_1fcwlbp") || //مدیر فنی مهندسی
            (BtnWorkFlowId == "SequenceFlow_0nrj0jn") || //رئیس مکانیک
            (BtnWorkFlowId == "SequenceFlow_1gt5053") || //مکانیک دورنیر
            (BtnWorkFlowId == "SequenceFlow_0fj4bm7") || //مکنیک خط متالایز
            (BtnWorkFlowId == "SequenceFlow_0u1gr0v") || // رئیس تاسیسات
            (BtnWorkFlowId == "SequenceFlow_14jouda") || // رئیس تاسیسات عمومی
            (BtnWorkFlowId == "SequenceFlow_1wnor7f") || //مدیر مالی
            (BtnWorkFlowId == "SequenceFlow_0c02i7r") || // سرپرست کنترل کیفیت
            (BtnWorkFlowId == "SequenceFlow_0w0of28") || //مدیر تولید
            (BtnWorkFlowId == "SequenceFlow_13tqiws") || //مدیر برنامه ریزی
            (BtnWorkFlowId == "SequenceFlow_060czxz") || //معاون منابع انسانی
            (BtnWorkFlowId == "SequenceFlow_0aqbk9a") || //سرپرست hse
            (BtnWorkFlowId == "SequenceFlow_18trva8") || //مدیر hse
            (BtnWorkFlowId == "SequenceFlow_0w1jim7") || // مدیر عمران
            (BtnWorkFlowId == "SequenceFlow_01i0cq1") || //رئیس rdc
            (BtnWorkFlowId == "SequenceFlow_0l9fdiu") || //سرپرست اداری
            (BtnWorkFlowId == "SequenceFlow_0hwcmh9") || // معاون منابع انسانی
            (BtnWorkFlowId == "SequenceFlow_1xiiui0") || //مدیرکارخانه
            (BtnWorkFlowId == "SequenceFlow_1tp098c") || //دفتر مهندسی
            (BtnWorkFlowId == "SequenceFlow_1808562") // واحد فنی تایید وارجاع 5
            )
            {
                // Console.WriteLine("#Log FormValidator btn :");
                foreach (var Item in List)
                {
                    //Console.WriteLine("#Log FormValidator btn foreach :");
                    IsValid = IsValid && await CheckFieldValidation(Item);
                }
            }

            // لغو کلی درخواست
            // SequenceFlow_10eigms
            if ((BtnWorkFlowId == "SequenceFlow_1ts3rnd") || //رئیس برق
             (BtnWorkFlowId == "SequenceFlow_1fk2bdh") || //مدیر فنی مهندسی
            (BtnWorkFlowId == "SequenceFlow_13bvwqa") || //رئیس مکانیک
            (BtnWorkFlowId == "SequenceFlow_02mi9mp") || //مکانیک دورنیر
            (BtnWorkFlowId == "SequenceFlow_0w8dmju") || //مکنیک خط متالایز
            (BtnWorkFlowId == "SequenceFlow_199j76b") || // رئیس تاسیسات
            (BtnWorkFlowId == "SequenceFlow_0afrqny") || // رئیس تاسیسات عمومی
            (BtnWorkFlowId == "SequenceFlow_1hiqsen") || //مدیر مالی
            (BtnWorkFlowId == "SequenceFlow_0kr2g6g") || // سرپرست کنترل کیفیت
            (BtnWorkFlowId == "SequenceFlow_1vjm8ql") || //مدیر تولید
            (BtnWorkFlowId == "SequenceFlow_1vjm8ql") || //مدیر برنامه ریزی
            (BtnWorkFlowId == "SequenceFlow_15ztx26") || //معاون منابع انسانی
            (BtnWorkFlowId == "SequenceFlow_1ig0fb6") || //سرپرست hse
            (BtnWorkFlowId == "SequenceFlow_1l6tbdz") || //مدیر hse
            (BtnWorkFlowId == "SequenceFlow_04ivfti") || // مدیر عمران
            (BtnWorkFlowId == "SequenceFlow_03cqthd") || //رئیس rdc
            (BtnWorkFlowId == "SequenceFlow_0pqou45") || //سرپرست اداری
            (BtnWorkFlowId == "SequenceFlow_16lycxb") || // معاون منابع انسانی
            (BtnWorkFlowId == "SequenceFlow_16475ed") || //مدیرکارخانه
            (BtnWorkFlowId == "SequenceFlow_16jnf37") //دفتر مهندسی
            )
            {
                // Console.WriteLine("#Log:: 0");

                string htmlString =
                    "<div>" +
                        "<picture>" +
                            "<img src='https://file.workcv.ir/fa/api/v1/File/Get?FileID=a18190d9-4b1c-45e3-db13-08dc25546b46' class='' alt='لوگو پتکو' width='96px'>" +
                        "</picture>" +
                        "<hr class='hrdash border-success-subtle'>" +
                    "</div>" +
                    "<div class='fw-bold text-right'>" +
                        "<div class='fs-6'>کاربر لغو کننده درخواست: " + _Entity.CancelledBy + "</div>" +
                        "<div class='fs-6'>" + _Entity.CancellationAt + "</div>" +
                        "<div class='fs-6'>دلیل لغو این درخواست:</div>" +
                        "<textarea required id='InConfirmCancelationText' name='InConfirmCancelationText' " +
                            "style='padding: 8px; border: 1px solid #ddd; " +
                            "border-radius: 5px; resize: none; display: block; margin: 5px 0' " +
                            "width: 100%; height: 150px;' " +
                            "placeholder='دلیل لغو درخواست را وارد کنید...' " +
                            "oninput='document.getElementById(\"ConfirmCancelationText\").value=this.value'>" +
                        "</textarea>" +
                        "<div></div>" +
                        "<div class='fs-6 text-secondary text-right'>" +
                        "<i class='fal fa-exclamation-triangle px-2' style='font-size:24px; color:red'></i>" +
                            "کاربر محترم در نظر داشته باشید اطلاعات ثبت در این ناحیه در حال حاضر صرفاً در گزارش‌ها واحد سیستم‌ها و روش‌ها قابل نمایش است، همچنین کاربر درخواست‌دهنده از بخش درخواست‌های من می‌تواند وضعیت درخواست خود را بررسی نماید." +
                        "</div>" +
                    "</div>";

                var options = new ConfirmDialogOptions
                {
                    YesButtonText = "لغو درخواست",
                    YesButtonColor = ButtonColor.Success,
                    NoButtonText = "انصراف",
                    NoButtonColor = ButtonColor.Danger
                };

                var confirmation = await Confirm.ShowAsync(
                    title: "",
                    message1: htmlString,
                    confirmDialogOptions: options);

                if (!confirmation)
                {
                    IsValid = false;
                }
                else
                {
                    // در بخش Razor فیلد input hidden همین فیلد وجود دارد و از طریق آن داده به فیلد اصلی داده می شود.
                    string Value = await JS.InvokeAsync<string>("eval", "document.getElementById('ConfirmCancelationText')?.value || ''");
                    _Entity.CancellationReason = Value;
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

        public async Task<bool> CheckFieldValidation(Entity.SCMPETCO_ProductRequestDetails Item)
        {
            bool IsValid = true;

            var List = _Entity.SCMPETCO_ProductRequestDetails.ToList();

            // شرط پُر بودن فیلد نام کالا 
            if (Item.ProductNameText == null)
            {
                IsValid = false;

                toastService.ShowError("لطفا گزینه نام کالا را تکمیل نمایید",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
            }

            // شرط پُر بودن فیلد تعداد یا مقدار درخواستی
            if (Item.ProductRequestingQTY == null || Item.ProductRequestingQTY == 0)
            {
                IsValid = false;

                toastService.ShowError("لطفا گزینه تعداد یا مقدار درخواستی را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
            }

            // شرط پُر بودن فیلد اولویت
            if (Item.SCMPETCO_PriorityId == null)
            {
                IsValid = false;

                toastService.ShowError("لطفا گزینه اولویت را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
            }

            // شرط پُر بودن فیلد محل مصرف
            if (Item.PlaceOfUseProduct == null)
            {
                IsValid = false;

                toastService.ShowError("لطفا گزینه محل مصرف تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
            }

            return IsValid;
        }
        public async Task TahvilIsVisible(bool Visible, bool Value, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            // تحویل
            Ref_SCMPETCO_ProductRequestDetails_ProductDelivery.SetVisible(Visible);
            Item.ProductDelivery = Value;
        }

        public async Task KharidIsVisible(bool Visible, bool Value, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            // خرید
            Ref_SCMPETCO_ProductRequestDetails_FutureAction.SetVisible(Visible);
            Item.FutureAction = Value;
            // تعداد تامین کسری	
            Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber.SetVisible(Visible);
        }

        public async Task TahvilKharidIsVisible(bool Visible, bool Value, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            // تحویل
            Ref_SCMPETCO_ProductRequestDetails_ProductDelivery.SetVisible(Visible);
            Item.ProductDelivery = Value;
            // خرید
            Ref_SCMPETCO_ProductRequestDetails_FutureAction.SetVisible(Visible);
            Item.FutureAction = Value;
            // تعداد تامین کسری
            Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber.SetVisible(Visible);
        }



        public async Task<bool> GridSCMPETCO_ProductRequestId_editmodelsaving(object e)
        {
            bool IsCancelled = false;

            var Item = (Entity.SCMPETCO_ProductRequestDetails)e;

            IsCancelled = !await CheckFieldValidation(Item);

            return IsCancelled;
        }

        public async Task GridSCMPETCO_ProductRequestId_afterrendermodal(Entity.SCMPETCO_ProductRequestDetails Item)
        {
            // Note: مخفی کردن فیلد بارگذاری فایل تی دی اس، بر اساس فیلد آیا دیتاشیت دارد یا خیر؟ 
            if (Item.ProductDataSheet.HasValue && Item.ProductDataSheet.Value)
            {
                Ref_SCMPETCO_ProductRequestDetails_SCMPETCO_ProductRequestDetails_ProductDataSheetFile.SetVisible(true);
            }
            else
            {
                Ref_SCMPETCO_ProductRequestDetails_SCMPETCO_ProductRequestDetails_ProductDataSheetFile.SetVisible(false);
            }

            // فیلد نوع درخواست
            if (Item.Global_SCMRequestTypeId == null)
            {
                await TahvilKharidIsVisible(false, false, Item);
            }
            else
            {
                // شرط اینکه آیا فرآیند تحویل اجرا گردد؟
                if (Item.Global_SCMRequestTypeId.ToString() == "a9c5df1c-d99c-ef11-8354-005056a02a64")
                {
                    await TahvilIsVisible(true, true, Item);
                    await KharidIsVisible(false, false, Item);
                }

                // شرط نوع درخواست بر اساس خرید کالا
                if (Item.Global_SCMRequestTypeId.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64")
                {
                    await TahvilIsVisible(false, false, Item);
                    await KharidIsVisible(true, true, Item);
                }

                // شرط نوع درخواست بر اساس تحویل و خرید کالا
                if (Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64")
                {
                    await TahvilKharidIsVisible(true, true, Item);
                }
            }
        }
        public async Task ProductName_NotMapped_onitemselected(dynamic Selected, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            /// <summary>
            /// فیلدهای زیر فیلدهای اصلی برای نمایش در فرم هستند.
            ///
            ///</summary>

            // Console.WriteLine("start");
            //  نام کالا
            Item.ProductNameText = Selected.DESC;
            //  نام دسته بندی فرعی
            Item.ProductSubCategoryText = Selected.SubGroupName;
            // کد کالا
            Item.ProductCodeText = Selected.PARTNO;
            // واحد کالا
            Item.ProductUnitText = Selected.UNIT;
            //دسته بندی اصلی کالا
            Item.ProductMainCategoryText = Selected.GroupName;
            // شناسه دسته بندی اصلی کالا
            Item.ProductMainCategoryIdText = Selected.GRCODE;
            // شناسه دسته بندی فرعی
            Item.ProductSubCategoryIdText = Selected.SUBGRCODE;
            // سال مالی شماران
            Item.ShomaranFiscalYearText = Selected.YEAR;
            // موجودی کالا در شماران        
            if (Selected.Amount > -1)
            {
                Item.ProductInventoryText = (double)Selected.Amount;
            }
            // Console.WriteLine("End");
        }
        public async Task ProductDataSheet_oninput(ChangeEventArgs Selected, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            // Note: مخفی کردن فیلد بارگذاری فایل تی دی اس، بر اساس فیلد آیا دیتاشیت دارد یا خیر؟
            var Item2 = Ref_SCMPETCO_ProductRequestDetails_SCMPETCO_ProductRequestDetails_ProductDataSheetFile;

            if (Selected.Value.ToString() == "true")
            {
                Item2.SetVisible(true);
            }
            else
            {
                Item2.SetVisible(false);
            }
        }
        public async Task Global_SCMRequestTypeId_onitemselected(Entity.Global_SCMRequestType Selected, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            // Console.WriteLine(await Utility.JSON.ToJson(Selected));

            // شرط اینکه آیا فرآیند تحویل اجرا گردد؟
            if (Item.Global_SCMRequestTypeId.ToString() == "a9c5df1c-d99c-ef11-8354-005056a02a64")
            {
                await TahvilIsVisible(true, true, Item);
                await KharidIsVisible(false, false, Item);
            }

            // شرط نوع درخواست بر اساس خرید کالا
            if (Item.Global_SCMRequestTypeId.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64")
            {
                await TahvilIsVisible(false, false, Item);
                await KharidIsVisible(true, true, Item);
            }

            // شرط نوع درخواست بر اساس تحویل و خرید کالا
            if (Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64")
            {
                await TahvilKharidIsVisible(true, true, Item);
            }
        }

        #endregion FunctionEvents

    }
}

