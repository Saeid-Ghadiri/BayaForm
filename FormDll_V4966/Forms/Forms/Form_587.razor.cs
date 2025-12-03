using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using System.Globalization;
using System.Threading.Tasks;
using BlazorBootstrap;
using Blazored.Toast.Services;


namespace Forms.Forms
{
    public class Form_587Base : Form_587Peropeties
    {

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

            var List = _Entity.SCM_ProductRequestDetails.Where(p=>p.IsDelete == false).ToList();
            var listCount = List.Count();

            //Console.WriteLine("#Log FormValidator :");

            // دکمه ثبت و ادامه
            if (BtnWorkFlowId == "SequenceFlow_0w2q635")
            {
                //Console.WriteLine("#Log FormValidator btn :");
                foreach (var Item in List)
                {
                    //Console.WriteLine("#Log FormValidator btn foreach :");

                    // تابع بررسی کد تحویل کالا توسط درخواست دهنده به انباردار
                    IsValid = await CheckDeliveryCode(true);

                    IsValid = IsValid && await CheckFieldValidation(Item);
                }
            }

	        // لغو کلی درخواست
            // SequenceFlow_1frbv74
            if (BtnWorkFlowId == "SequenceFlow_0336llv")// آیدی دکمه چک شود
            {
                // Console.WriteLine("#Log:: 0");

                 string htmlString =
                    "<div>" +
                        "<picture>" +
                            "<img src='https://File.workcv.ir/fa/api/v1/File/Get?FileID=6e5b6fb8-a5b2-490c-f83f-08dbea5b8061' class='' alt='لوگو پل‌فیلم' width='96px'>" +
                        "</picture>" +
                        "<hr class='hrdash border-success-subtle'>" +
                    "</div>" +
                    "<div class='fw-bold text-right'>" +
                        "<div class='fs-6'>کاربر لغو کننده درخواست: " + _User.NAME + " "+ _User.FAMILY + "</div>" +
                        // "<div class='fs-6'>" + _Entity.CancellationAt + "</div>" +
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
                    _Entity.CancelledBy =  _User.UserID.ToString();
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
        await ChangeDateTime_Anbardar();

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

            // - فیلد نوع درخواست
            if (Item.Global_SCMRequestTypeId == null)
            {
                IsValid = false;
                toastService.ShowError("لطفا گزینه نوع درخواست را تکمیل نمایید",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
            }

            // کد تحویل
            if (Item.Global_SCMRequestTypeId.ToString() == "a9c5df1c-d99c-ef11-8354-005056a02a64" ||
                Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64")
            {
                if (Item.DeliveryCode != Item.GetDeliveryCode)
                {
                    IsValid = false;

                    toastService.ShowError("کد تحویل وارد شده صحیح نیست!! لطفا کد صحیح را وارد نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
            }

            // تعداد یا مقدار واگذاری کالا
            if (Item.Global_SCMRequestTypeId.ToString() == "a9c5df1c-d99c-ef11-8354-005056a02a64" ||
                Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64")
            {
                if (Item.NumberofProductDelivery == null)
                {
                    IsValid = false;

                    toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا را تکمیل  نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
            }

            // Note نحوه تامین کالا		
            if (Item.Global_SCMRequestTypeId == null)
            {
                IsValid = false;

                toastService.ShowError("لطفا گزینه  نوع درخواست را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
            }

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

            // شرط پُر بودن فیلد اولویت
            if (Item.SCM_PriorityId == null)
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

            //  نحوه تامین کالا		
            if (Item.ForeignMachineryProductTrueFasle == null)
            {
                IsValid = false;

                toastService.ShowError("لطفا گزینه  نحوه تامین کالا تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
            }

            return IsValid;
        }

        /// <summary>
        /// تبدیل تاریخ در زمان ثبت کد تحویل کالا توسط انباردار
        /// </summary>
        /// <returns></returns>
        public async Task ChangeDateTime_Anbardar()
        {
            var List = _Entity.SCM_ProductRequestDetails.ToList();
            // تبدیل تاریخ و تکمیل فیلد
            for (int i = 0; i < List.Count; i++)
            {
                var item = List[i];
                if (item.GetDeliveryCode != null)
                {
                    // تبدیل تاریخ شمسی به میلادی            
                    System.Globalization.PersianCalendar PC = new System.Globalization.PersianCalendar();

                    var DateNow = DateTime.Now;
                    // تاریخ شمسی پر می شود
                    item.DateTimeDeliveryCode = PC.GetYear(DateNow) + "/" + PC.GetMonth(DateNow).ToString("0#") + "/" + PC.GetDayOfMonth(DateNow).ToString("0#");
                }
            }
        }

        /// <summary>
		/// بررسی کد تحویل وارد شده توسط انباردار
		/// </summary>
		/// <param name="IsValid"></param>
		/// <returns></returns>
        public async Task<bool> CheckDeliveryCode(bool IsValid)
        {
            IsValid = true;

            var List = _Entity.SCM_ProductRequestDetails.ToList();
            foreach(var Item in List)
            {
                if (Item.Global_SCMRequestTypeId.ToString() != "3b9e934d-d99c-ef11-8354-005056a02a64")
                {
                    if (Item.DeliveryCode != Item.GetDeliveryCode)
                    {
                        IsValid = false;
                        toastService.ShowError("کد تحویل وارد شده صحیح نیست!! لطفا کد صحیح را وارد نمایید.",
                        settings =>
                        {
                            settings.Timeout = 4;
                            settings.ShowProgressBar = true;
                            settings.PauseProgressOnHover = true;
                        });
                    }
                }
            }

            return IsValid;
        }

        #region RequestTypeMode

        /// <summary>
        /// بررسی حالت های داپ دان تحویل، خرید و تحویل و خرید
        /// </summary>
        /// <param name="RequestTypeMode"></param>
        /// <param name="Item"></param>
        /// 
        /// <returns></returns>
        public async Task<bool> CheckModeTahvilKharid(List<int> RequestTypeMode, Entity.SCM_ProductRequestDetails Item)
        {
            bool IsEmpty = (String.IsNullOrEmpty(Item.Global_SCMRequestTypeId.ToString()));

            foreach (var i in RequestTypeMode)
            {
                Console.WriteLine(await Utility.JSON.ToJson(Item));

                switch (i)
                {
                    //تحویل است یا نه
                    case 1:

                        Console.WriteLine("#Log Mode 1 ");

                        if (Item.Global_SCMRequestTypeId.ToString() == "a9c5df1c-d99c-ef11-8354-005056a02a64")
                        {
                            await TahvilIsVisible(true, true, Item);
                            await KharidIsVisible(false, false, Item);
                        }

                        break;
                    //خرید است یا نه
                    case 2:

                        Console.WriteLine("#Log Mode 2 ");

                        if (Item.Global_SCMRequestTypeId.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64")
                        {
                            await TahvilIsVisible(false, false, Item);
                            await KharidIsVisible(true, true, Item);
                        }

                        break;
                    // تحویل و خرید با هم است
                    case 3:

                        Console.WriteLine("#Log Mode 3 ");

                        if (Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64")
                        {
                            await TahvilKharidIsVisible(true, true, Item);
                        }

                        break;
                    // حالت پیش فرض
                    default:

                        Console.WriteLine("#Log Mode 4 ");

                        // فیلد نوع درخواست
                        if (Item.Global_SCMRequestTypeId == null)
                        {
                            await TahvilKharidIsVisible(false, false, Item);
                        }

                        break;
                }
            }

            return IsEmpty;
        }
        public async Task TahvilIsVisible(bool Visible, bool Value, Entity.SCM_ProductRequestDetails Item)
        {
            // تحویل
            Ref_SCM_ProductRequestDetails_ProductDelivery.SetVisible(Visible);
            Item.ProductDelivery = Value;

            Ref_SCM_ProductRequestDetails_GetDeliveryCode.SetVisible(Visible);

            Ref_SCM_ProductRequestDetails_NumberofProductDelivery.SetVisible(Visible);
        }

        public async Task KharidIsVisible(bool Visible, bool Value, Entity.SCM_ProductRequestDetails Item)
        {
            // خرید
            Ref_SCM_ProductRequestDetails_FutureActionTrueFalse.SetVisible(Visible);
            Item.FutureActionTrueFalse = Value;
            // تعداد تامین کسری	
            Ref_SCM_ProductRequestDetails_DeficitSupplyNumber.SetVisible(Visible);

        }

        public async Task TahvilKharidIsVisible(bool Visible, bool Value, Entity.SCM_ProductRequestDetails Item)
        {
            // تحویل
            Ref_SCM_ProductRequestDetails_ProductDelivery.SetVisible(Visible);
            Item.ProductDelivery = Value;
            // خرید
            Ref_SCM_ProductRequestDetails_FutureActionTrueFalse.SetVisible(Visible);
            Item.FutureActionTrueFalse = Value;
            // تعداد تامین کسری
            Ref_SCM_ProductRequestDetails_DeficitSupplyNumber.SetVisible(Visible);

            Ref_SCM_ProductRequestDetails_GetDeliveryCode.SetVisible(Visible);

            Ref_SCM_ProductRequestDetails_NumberofProductDelivery.SetVisible(Visible);
        }

        #endregion / RequestTypeMode
        public async Task ProductName_NotMapped_onitemselected(dynamic Selected, Entity.SCM_ProductRequestDetails Item)
        {
            /// <summary>
            /// فیلدهای زیر فیلدهای اصلی برای نمایش در فرم هستند.
            ///
            ///</summary>

            //Console.WriteLine("start");
            //  نام کالا
            Item.ProductNameText = Selected.DESC;
            //Console.WriteLine(Selected.DESC);
            //  نام دسته بندی فرعی
            Item.ProductSubCategoryText = Selected.SubGroupName;
            //Console.WriteLine(Selected.SubGroupName);
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
            //کالا موجود است یا خیر
            Item.IsExistText = Selected.IsExist;
            // کد اصلی گروه کالا شماران 
            Item.MapGroupCodeNum = Selected.MapGroupCode;
            // موجودی کالا در شماران
            if (Selected.Amount > -1)
            {
                Item.ProductInventoryText = (double)Selected.Amount;
            }
            //Console.WriteLine("End");            
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
            // Note: مخفی کردن فیلد تعداد تامین کسری بر اساس فیلد آیا تامین کسری دارد؟
            if (Item.FutureActionTrueFalse.HasValue && Item.FutureActionTrueFalse.Value)
            {
                Ref_SCM_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true);
            }
            else
            {
                Ref_SCM_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);
            }
            
            
            
            // فیلد نوع درخواست
            if (Item.Global_SCMRequestTypeId == null)
            {
                await TahvilKharidIsVisible(false, false, Item);
            }
            else
            {
                // 3 حالت سویچ طراحی شده بررسی خواهد شد.
                await CheckModeTahvilKharid([1, 2, 3], Item);
            }


        }

        public async Task Global_SCMRequestTypeId_onitemselected(Entity.Global_SCMRequestType Selected, Entity.SCM_ProductRequestDetails Item)
        {
            //Console.WriteLine(await Utility.JSON.ToJson(Selected));

            try
            {
                // 3 حالت سویچ طراحی شده بررسی خواهد شد.
                await CheckModeTahvilKharid([1, 2, 3], Item);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task DeliveryCode_NotMapped_oninput(ChangeEventArgs Selected)
        {
            var List = _Entity.SCM_ProductRequestDetails.ToList();
            foreach (var Item in List)
            {
                Item.GetDeliveryCode = int.Parse(Selected.Value.ToString());
            }

            StateHasChanged();
        }


        public async Task  FutureActionTrueFalse_oninput(ChangeEventArgs Selected ,Entity.SCM_ProductRequestDetails Item  )
        {
            // Note: مخفی کردن فیلد تعداد تامین کسری بر اساس فیلد آیا تامین کسری دارد؟       
            var OkItem = Ref_SCM_ProductRequestDetails_DeficitSupplyNumber;
            if (Selected.Value.ToString() == "true")
            {
                OkItem.SetVisible(true);
            }
            else
            {
                OkItem.SetVisible(false);
            }
        }

		#endregion FunctionEvents

    }
}