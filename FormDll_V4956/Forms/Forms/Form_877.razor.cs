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
    public class Form_877Base : Form_877Peropeties
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
                // حذف Defualt Value ها
                // var List = _Entity.SCMPETCO_ProductRequestDetails.ToList();
                // foreach (var Item in List)
                // {
                // 	// // آیا تامین کسری انجام شود؟
                // 	// Item.FutureAction = null;
                // 	// // آیا فرآیند تحویل اجرا گردد؟
                // 	// Item.ProductDelivery = null;

                // 	// نحوه تامین کالا
                // 	// Item.ForeignMachineryProduct=null;
                // }
                // StateHasChanged();
            }
        }

        /// <summary>
        /// اعتبار سنجی فرم
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> FormValidator()
        {
            bool IsValid = true;

            var List = _Entity.SCMPETCO_ProductRequestDetails.Where(p=>p.IsDelete == false).ToList();

            var listCount = List.Count();

            // Console.WriteLine("#Log FormValidator :");

            // دکمه ثبت و ادامه           
            if (BtnWorkFlowId == "SequenceFlow_0wthill")
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
            if (BtnWorkFlowId == "SequenceFlow_10eigms")
            {
                Console.WriteLine("#Log:: 0");

                // string htmlString = 
        	    // "<div>"+
        	    //     "<picture>"+
        	    //         "<img src='https://file.workcv.ir/fa/api/v1/File/Get?FileID=a18190d9-4b1c-45e3-db13-08dc25546b46' class='' alt='لوگو پتکو' width='96px;'>"+
        	    //     "</picture>"+
        	    //     "<hr class='hrdash border-success-subtle'>"+
        	    // "</div>"+
        	    // "<div class='fw-bold text-right'>" + 
        	    // "<span class='fs-6'>کاربر لغو کننده درخواست: </span>" + 
				// "<span class='fs-6'>" + _Entity.CancelledBy + "</span><div>" +
				// "<span class='fs-6'>" + _Entity.CancellationAt + "</span><div>" +
        	    // "<span class='fs-6'>دلیل لغو این درخواست: </span>" + 
				// "<span class='fs-6'>  <input type=\"text\" id=\"InConfirmCancelationText\" name=\"InConfirmCancelationText\" oninput=\"document.getElementById('ConfirmCancelationText').value=document.getElementById('InConfirmCancelationText').value;\"></span><div>" +
        	    // "<span><i class='fal fa-exclamation-triangle' style='font-size:24px; color:red;'></i>&nbsp;</span>" +
				// "<span class='fs-6 text-secondary text-right'>کاربر محترم در نظر داشته باشید اطلاعات ثبت در این ناحیه در حال حاضر صرفاً در گزارش ها واحد سیستم ها و روش ها قابل نمایش است، همچنین کاربر درخواست دهنده از بخش درخواست های من می تواند وضعیت درخواست خود را بررسی نماید.<span></div>" +
        	    // "</div>";


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
                        "<textarea id='InConfirmCancelationText' name='InConfirmCancelationText' " +
                            "style='padding: 8px; border: 1px solid #ddd; " +
                            "border-radius: 5px; resize: none; display: block; margin: 5px 0' " +
                            //"maxlength='500' placeholder='دلیل لغو درخواست را وارد کنید...' " +
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
                    _Entity.CancellationReason=Value;
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
            // ثبت و تبدیل تاریخ تحویل کالا به درخواست دهنده
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
        public async Task<bool> CheckFieldValidation(Entity.SCMPETCO_ProductRequestDetails Item)
        {
            bool IsValid = true;

            var List = _Entity.SCMPETCO_ProductRequestDetails.ToList();

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

            // - بررسی فیلدهای شماران - کالای جدید نباشد
            if (Item.ProductNameText != null && Item.ProductCodeText == "0000000000")
            {
                IsValid = false;
                toastService.ShowError("كالاي جديد و کد کالای 0000000000 را نمیتوانید ثبت نمایید لطفا بر اساس آیتم جستجو تغییرات را اعمال کنید.",
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

            // // Note نوع درخواست	
            // if (Item.Global_SCMRequestTypeId == null)
            // {
            //     IsValid = false;

            //     toastService.ShowError("لطفا گزینه  نوع درخواست را تکمیل نمایید.",
            //         settings =>
            //         {
            //             settings.Timeout = 4;
            //             settings.ShowProgressBar = true;
            //             settings.PauseProgressOnHover = true;
            //         });
            // }

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

            //  نحوه تامین کالا		
            if (Item.ForeignMachineryProduct == null)
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
        /// ثبت تغییر توسط انباردار - تبدیل تاریخ و تکمیل فیلد
        /// </summary>
        /// <returns></returns>
        public async Task ChangeDateTime_Anbardar()
        {
            var List = _Entity.SCMPETCO_ProductRequestDetails.ToList();

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
        /// 
        /// </summary>
        /// <param name="RequestTypeMode"></param>
        /// <param name="Item"></param>
        /// 
        /// <returns></returns>
        public async Task<bool> CheckModeTahvilKharid(List<int> RequestTypeMode, Entity.SCMPETCO_ProductRequestDetails Item)
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

        public async Task TahvilIsVisible(bool Visible, bool Value, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            // تحویل
            Ref_SCMPETCO_ProductRequestDetails_ProductDelivery.SetVisible(Visible);
            Item.ProductDelivery = Value;
            // تعداد یا مقدار واگذاری کالا
            Ref_SCMPETCO_ProductRequestDetails_NumberofProductDelivery.SetVisible(Visible);
            // دریافت کد تحویل کالا
            Ref_SCMPETCO_ProductRequestDetails_GetDeliveryCode.SetVisible(Visible);
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
            // تعداد یا مقدار واگذاری کالا
            Ref_SCMPETCO_ProductRequestDetails_NumberofProductDelivery.SetVisible(Visible);
            // دریافت کد تحویل کالا
            Ref_SCMPETCO_ProductRequestDetails_GetDeliveryCode.SetVisible(Visible);
        }

        public async Task AnbardarIsVisible(bool Visible)
        {
            // تعداد یا مقدار واگذاری کالا
            Ref_SCMPETCO_ProductRequestDetails_NumberofProductDelivery.SetVisible(Visible);
            // دریافت کد تحویل کالا
            Ref_SCMPETCO_ProductRequestDetails_GetDeliveryCode.SetVisible(Visible);
        }


        public async Task ProductName_NotMapped_onitemselected(dynamic Selected, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            /// <summary>
            /// فیلدهای زیر فیلدهای اصلی برای نمایش در فرم هستند.
            ///</summary>

            //  نام کالا
            Item.ProductNameText = Selected.DESC;
            //Console.WriteLine(Selected.DESC);
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
        }

        public async Task<bool> SCMPETCO_ProductRequestDetails_editmodelsaving(object e)
        {
            bool IsCancelled = false;

            var Item = (Entity.SCMPETCO_ProductRequestDetails)e;

            IsCancelled = !await CheckFieldValidation(Item);

            return IsCancelled;
        }

        public async Task SCMPETCO_ProductRequestDetails_afterrendermodal(Entity.SCMPETCO_ProductRequestDetails Item)
        {
            // Note: مخفی کردن فیلد تعداد تامین کسری بر اساس فیلد آیا تامین کسری دارد؟
            if (Item.FutureAction.HasValue && Item.FutureAction.Value)
            {
                Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true);
            }
            else
            {
                Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);
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

          public async Task Global_SCMRequestTypeId_onitemselected(Entity.Global_SCMRequestType Selected, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            Console.WriteLine(await Utility.JSON.ToJson(Selected));

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
            // تکمیل فیلد کد تحویل کالا در همه ردیف های گرید
            var List = _Entity.SCMPETCO_ProductRequestDetails.ToList();

            foreach (var Item in List)
            {
                Item.GetDeliveryCode = int.Parse(Selected.Value.ToString());
            }

            StateHasChanged();

        }

       	public async Task  FutureAction_oninput(ChangeEventArgs Selected ,Entity.SCMPETCO_ProductRequestDetails Item  )
        {
            // Note: مخفی کردن فیلد تعداد تامین کسری بر اساس فیلد آیا تامین کسری دارد؟       
            var OkItem = Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber;
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
