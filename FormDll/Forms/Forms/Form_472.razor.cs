using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using BlazorBootstrap;
using Blazored.Toast.Services;

namespace Forms.Forms
{
    public class Form_472Base : Form_472Peropeties
    {


        // Toast  
        [Inject]
        public IToastService toastService { get; set; }

        

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
                // var List = _Entity.SCM_ProductRequestDetails.ToList();

                // for (int i = 0; i < List.Count(); i++)
                // {
                // 	var Item = List[i];
                // 	// آیا تامین کسری انجام شود؟
                // 	Item.FutureActionTrueFalse = null;
                // 	// آیا فرآیند تحویل اجرا گردد؟
                // 	Item.ProductDelivery = null;
                // 	//آیا کالا دارای (Technical Data Sheet - (TDS است؟ 
                // 	Item.ProductDataSheetTrueFalse = null;
                // }
            }
        }

        /// <summary>
        /// اعتبار سنجی فرم
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> FormValidator()
        {
            bool IsValid = true;

            var List1 = _Entity.SCM_ProductRequestDetails.ToList();
            var List = _Entity.SCM_ProductRequestDetails.Where(p => p.IsDelete == false).ToList();
            int ListCount = List1.Count();

            if (ListCount == 0)
            {

                IsValid = false;

                var options = new ConfirmDialogOptions
                {
                    YesButtonText = "بازگشت به درخواست",
                    YesButtonColor = ButtonColor.Danger,
                    NoButtonText = "",
                };

                string htmlString =
                "<div>" +
                    "<picture>" +
                        "<img src='https://file.workcv.ir/fa/api/v1/File/Get?FileID=6e5b6fb8-a5b2-490c-f83f-08dbea5b8061' class='' alt='لوگو پل فیلم' width='96px;'>" +
                    "</picture>" +
                    "<hr class='hrdash border-success-subtle'>" +
                "</div>" +
                "<div class='fw-bold text-center'>" +
                "<span class='fs-5'>کد پیگیری این درخواست: </span>" +
                "<span class='fs-3' style='color: #1ba156'>" + _Entity.RequestTrakingCode + "</span><div>" +
                "<span><i class='fal fa-exclamation-triangle' style='font-size:24px; color:red;'></i>&nbsp;</span>" +
                "<span class='fs-6 text-secondary text-right'>تا کنون هیچ ردیف درخواستی تکمیل نشده است. لطفا برای ثبت و ادامه به مرحله بعد حداقل یک ردیف در درخواست خود ثبت نمایید.	<span></div>" +
                "</div>";

                var confirmation = await Confirm.ShowAsync(
                    title: "",
                    message1: htmlString,
                    confirmDialogOptions: options);
            }

            foreach (var Item in List)
            {
                //Console.WriteLine("#Log FormValidator btn foreach :");
                IsValid = IsValid && await CheckFieldValidation(Item);
            }

            // لغو کلی درخواست
            if (BtnWorkFlowId == "SequenceFlow_0f8i8mq") // ProcessID = 922
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
                        "<div class='fs-6'>کاربر لغو کننده درخواست: " + _User.NAME + " " + _User.FAMILY + "</div>" +
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
                    // کاربر لغو کننده
                    _Entity.CancelledBy = _User.UserID.ToString();
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
            // 
			await AcceptDeliveryCode();

            #region DeliveryCode Confirm
			// نمایش کد تحویل کالا در زمان ثبت درخواست
			var options = new ConfirmDialogOptions
			{
				YesButtonText = "ادامه فرآیند",
				YesButtonColor = ButtonColor.Success,
				NoButtonText = "",
			};

			string htmlString = 
            "<div class='text-center'>" + 
            "<span>کد پیگیری درخواست: </span>" + 
			"<span class='fs-4 fw-bold'>" + _Entity.RequestTrakingCode + "</span><hr class='hrdash'><div>"+
			"<div><span class='fw-normal'>کد تحویل کالا: </span>" + 
			"<span class='fs-3 text-green fw-bold'>" + RandomDeliveryCode + "</span>" +
            "<span class='btn btn-yellow-light btn-sm mx-2 mb-2' onclick=\"navigator.clipboard.writeText('" + RandomDeliveryCode + "'); $('#copylable').removeClass('d-none');\"><i class='fal fa-copy'></i><span class='px-1'>کپی</span></span></div>" +
            "<p id='copylable' class='d-none text-green mt-3'>کد تحویل کالا کپی شد.</p>" +  
            "<div><hr class='hrdash'><span class='fal fa-exclamation-circle text-red btnicon mx-1' style='font-size: 24px;'></span>"+
			"<span>لطفا در زمان دریافت کالا از انبار، کد فوق را به انباردار اعلام نمایید.</span>" +
            "</div>";

			// MarkupString htmlContent = new MarkupString(htmlString);

			var confirmation = await Confirm.ShowAsync(
				title: "",
				message1: htmlString,
				confirmDialogOptions: options);

			#endregion / DeliveryCode Confirm

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
        /// کد تحویل رندوم 
        /// </summary>
        /// <returns></returns>
        public int RandomDeliveryCode { get; set; }

        /// <summary>
        /// آماده سازی فرم
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            //شش رقمی
            RandomDeliveryCode = new Random().Next(100000, 999999);
        }

        /// <summary>
		/// پرکردن فیلد کد رهگیری
		/// </summary>
		public async Task AcceptDeliveryCode()
		{
			var List = _Entity.SCM_ProductRequestDetails.ToList();
			foreach(var Item in List)
			{
				if(!Item.DeliveryCode.HasValue)
            	{
                	Item.DeliveryCode = RandomDeliveryCode;
            	}
			}
		}


        public async Task<bool> CheckFieldValidation(Entity.SCM_ProductRequestDetails Item)
        {
            bool IsValid = true;

            var List = _Entity.SCM_ProductRequestDetails.Where(p => p.IsDelete == false).ToList();

            // Note: شرط پُر بودن فیلد نام کالا دیزیبل
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

            // Note: شرط پُر بودن فیلد تعداد یا مقدار درخواستی
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


            //	ProductDataSheetTrueFalse
            if (Item.ProductDataSheetTrueFalse == null)
            {
                IsValid = false;

                toastService.ShowError("لطفا گزینه آیا TDS دارد یا خیر؟ را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
            }

            // Global_SCMRequestTypeId - مشخص کردن وضعیت یک درخواست زنجیره تامین 
            if (Item.Global_SCMRequestTypeId == null)
            {
                IsValid = false;
                toastService.ShowError("یک نوع درخواست انتخاب کنید.",
                settings =>
                {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }

            // Note: شرط پُر بودن فیلد تعداد یا مقدار درخواستی
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

            // Note: شرط پُر بودن فیلد اولویت
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

            // Note: شرط پُر بودن فیلد محل مصرف
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

            // ForeignMachineryProductTrueFasle - نحوه تامین کالا
            if (Item.ForeignMachineryProductTrueFasle == null)
            {
                IsValid = false;

                toastService.ShowError("لطفا گزینه نحوه تامین کالا تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
            }

            // Global_SCMRequestTypeId - مشخص کردن وضعیت یک درخواست زنجیره تامین 
            if (Item.Global_SCMRequestTypeId.ToString() == ("73f6c459-d99c-ef11-8354-005056a02a64") ||
                Item.Global_SCMRequestTypeId.ToString() == ("3b9e934d-d99c-ef11-8354-005056a02a64"))
            {
                // DeficitSupplyNumber - تعداد تامین کسری
                if (Item.DeficitSupplyNumber == null)
                {
                    IsValid = false;

                    toastService.ShowError("لطفا گزینه تعداد تامین کسری تکمیل نمایید.",
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

        public async Task ProductName_NotMapped_onitemselected(dynamic Selected, Entity.SCM_ProductRequestDetails Item)
        {
            /// <summary>
            /// فیلدهای زیر فیلدهای اصلی برای نمایش در فرم هستند.
            ///
            ///</summary>

            Console.WriteLine("start");
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

             Console.WriteLine("Selected.PARTCODE : " + Selected.PARTCODE);
             Item.PARTCODE = Selected.PARTCODE;
        }

        public async Task<bool> SCM_ProductRequestDetails_editmodelsaving(object e)
        {
            bool IsCancelled = false;

            var ListCount = _Entity.SCM_ProductRequestDetails.Count();
            if (ListCount > 4)
            {
                IsCancelled = true;

                toastService.ShowError("تعداد ردیف‌ها نمی‌تواند بیشتر از 5 باشد",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });

                return IsCancelled;
            }

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

            // Note: مخفی کردن فیلد بارگذاری فایل تی دی اس، بر اساس فیلد آیا دیتاشیت دارد یا خیر؟ 
            if (Item.ProductDataSheetTrueFalse.HasValue && Item.ProductDataSheetTrueFalse.Value)
            {

                Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductDataSheetFile.SetVisible(true);

            }
            else
            {
                Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductDataSheetFile.SetVisible(false);
            }

            // در حالت نال هر دو فقط مخفی باشد
            if (Item.Global_SCMRequestTypeId == null)
            {
                Ref_SCM_ProductRequestDetails_ProductDelivery.SetVisible(false);
                Ref_SCM_ProductRequestDetails_FutureActionTrueFalse.SetVisible(false);
                Ref_SCM_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);
            }
            else
            {
                // شرط اینکه آیا فرآیند تحویل اجرا گردد؟
                if (Item.Global_SCMRequestTypeId.ToString() == "a9c5df1c-d99c-ef11-8354-005056a02a64")
                {
                    Ref_SCM_ProductRequestDetails_ProductDelivery.SetVisible(true);
                    Ref_SCM_ProductRequestDetails_FutureActionTrueFalse.SetVisible(false);
                }
                // شرط نوع درخواست بر اساس خرید کالا
                if (Item.Global_SCMRequestTypeId.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64")
                {
                    Ref_SCM_ProductRequestDetails_ProductDelivery.SetVisible(false);
                    Ref_SCM_ProductRequestDetails_FutureActionTrueFalse.SetVisible(true);
                }
                // شرط نوع درخواست بر اساس تحویل و خرید کالا
                if (Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64")
                {
                    Ref_SCM_ProductRequestDetails_ProductDelivery.SetVisible(true);
                    Ref_SCM_ProductRequestDetails_FutureActionTrueFalse.SetVisible(true);
                }
            }
        }

        public async Task SCM_ProductRequestDetails_customizeeditmodel(GridCustomizeEditModelEventArgs e)
        {
            // var Item = (Entity.SCM_ProductRequestDetails)e.EditModel;
            // if (e.IsNew)
            // {
            // 	// آیا تامین کسری انجام شود؟
            // 	Item.FutureActionTrueFalse = null;
            // 	//آیا کالا دارای (Technical Data Sheet - (TDS است؟	
            // 	Item.ProductDataSheetTrueFalse = null;
            // 	// آیا فرآیند تحویل اجرا گردد؟
            // 	Item.ProductDelivery = null;
            // }
        }

        public async Task FutureActionTrueFalse_oninput(ChangeEventArgs Selected, Entity.SCM_ProductRequestDetails Item)
        {
            // Note: مخفی کردن فیلد تعداد تامین کسری بر اساس فیلد آیا تامین کسری دارد؟       
            var Item1 = Ref_SCM_ProductRequestDetails_DeficitSupplyNumber;

            if (Selected.Value.ToString() == "true")
            {
                Item1.SetVisible(true);
            }
            else
            {
                Item1.SetVisible(false);
            }
        }

        public async Task ProductDataSheetTrueFalse_oninput(ChangeEventArgs Selected, Entity.SCM_ProductRequestDetails Item)
        {
            // Note: مخفی کردن فیلد بارگذاری فایل تی دی اس، بر اساس فیلد آیا دیتاشیت دارد یا خیر؟

            var Item2 = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductDataSheetFile;

            if (Selected.Value.ToString() == "true")
            // if(Item.FutureActionTrueFalse.Value == null || !Item.ProductDataSheetTrueFalse.Value)
            {
                Item2.SetVisible(true);
            }
            else
            {
                Item2.SetVisible(false);
            }
        }

        public async Task Global_SCMRequestTypeId_onitemselected(Entity.Global_SCMRequestType Selected, Entity.SCM_ProductRequestDetails Item)
        {
            //Console.WriteLine(await Utility.JSON.ToJson(Selected));

            // شرط نوع درخواست بر اساس تحویل کالا
            if (Selected.Id.ToString() == "a9c5df1c-d99c-ef11-8354-005056a02a64")
            {
                // تحویل کالا
                //Ref_SCM_ProductRequestDetails_ProductDelivery.SetVisible(true);
                Ref_SCM_ProductRequestDetails_ProductDelivery.Value = true;
                // خرید کالا
                //Ref_SCM_ProductRequestDetails_FutureActionTrueFalse.SetVisible(false);
                Ref_SCM_ProductRequestDetails_FutureActionTrueFalse.Value = false;
                Ref_SCM_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);
            }

            // شرط نوع درخواست بر اساس خرید کالا
            if (Selected.Id.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64")
            {
                // تحویل کالا
                //Ref_SCM_ProductRequestDetails_ProductDelivery.SetVisible(false);
                Ref_SCM_ProductRequestDetails_ProductDelivery.Value = false;
                // خرید کالا
                //Ref_SCM_ProductRequestDetails_FutureActionTrueFalse.SetVisible(true);              
                Ref_SCM_ProductRequestDetails_FutureActionTrueFalse.Value = true;
                if (Ref_SCM_ProductRequestDetails_FutureActionTrueFalse.Value.HasValue && Ref_SCM_ProductRequestDetails_FutureActionTrueFalse.Value.Value)
                {
                    Ref_SCM_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true);
                }
                else
                {
                    Ref_SCM_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);
                }
            }

            // شرط نوع درخواست بر اساس تحویل و خرید کالا
            if (Selected.Id.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64")
            {
                // تحویل کالا
                //Ref_SCM_ProductRequestDetails_ProductDelivery.SetVisible(true);    
                Ref_SCM_ProductRequestDetails_ProductDelivery.Value = true;
                // خرید کالا
                //Ref_SCM_ProductRequestDetails_FutureActionTrueFalse.SetVisible(true);       
                Ref_SCM_ProductRequestDetails_FutureActionTrueFalse.Value = true;

                if (Ref_SCM_ProductRequestDetails_FutureActionTrueFalse.Value.HasValue && Ref_SCM_ProductRequestDetails_FutureActionTrueFalse.Value.Value)
                {
                    Ref_SCM_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true);
                }
                else
                {
                    Ref_SCM_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);
                }
            }
        }

        



		public async Task <bool> GridSCM_ProductRequestId_182_editmodelsaving(object e   )
        {
            bool IsCancelled = false;

            var ListCount = _Entity.SCM_ProductRequestDetails.Count();
            if (ListCount > 4)
            {
                IsCancelled = true;

                toastService.ShowError("تعداد ردیف‌ها نمی‌تواند بیشتر از 5 باشد",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });

                return IsCancelled;
            }

            var Item = (Entity.SCM_ProductRequestDetails)e;

            IsCancelled = !await CheckFieldValidation(Item);

            return IsCancelled;
        }

		#endregion FunctionEvents

    }
}