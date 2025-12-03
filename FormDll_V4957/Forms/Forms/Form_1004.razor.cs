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
using CurrieTechnologies.Razor.SweetAlert2;

namespace Forms.Forms
{
    public class Form_1004Base : Form_1004Peropeties
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

            var List = _Entity.SCM_ProductRequestDetails.Where(p => p.IsDelete == false).ToList();

            //Console.WriteLine("#Log FormValidator :");

            // دکمه ثبت و ادامه
            if (BtnWorkFlowId == "SequenceFlow_0fgh34q")
            {
                //Console.WriteLine("#Log FormValidator btn :");
                foreach (var Item in List)
                {
                    //Console.WriteLine("#Log FormValidator btn foreach :");
                    IsValid = IsValid && await CheckFieldValidation(Item);
                }
            }

            // لغو کلی درخواست
            if (BtnWorkFlowId == "SequenceFlow_04e1yn5") //904
            {
                string sValue = null;

                string htmlString =
                    "<div>" +
                        "<picture>" +
                            "<img src='https://File.workcv.ir/fa/api/v1/File/Get?FileID=6e5b6fb8-a5b2-490c-f83f-08dbea5b8061' class='' alt='لوگو پل فیلم' width='96px'>" +
                        "</picture>" +
                        "<hr class='hrdash border-success-subtle'>" +
                    "</div>" +
                    "<div class='fw-bold text-right'>" +
                        "<div class='fs-6'>*</div>" +
                        "<div class='fs-6'>" + "</div>" +
                        "<div class='fs-6'>دلیل لغو این درخواست:</div>" +
                        "<textarea required id='InConfirmCancelationText' name='InConfirmCancelationText' " +
                            "class='form-control my-3'" +
                            "rows='4'" +
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
                    StateHasChanged();

                    // در بخش Razor فیلد input hidden همین فیلد وجود دارد و از طریق آن داده به فیلد اصلی داده می شود.
                    sValue = await JS.InvokeAsync<string>("eval", "document.getElementById('ConfirmCancelationText')?.value || ''");
                    //await Task.Delay(300);
                    Console.WriteLine(sValue);
                    Console.WriteLine(_User.UserID.ToString());

                    _Entity.CancelledBy = _User.UserID.ToString();
                    _Entity.CancellationReason = sValue;

                    IsValid = true;
                }

                if (string.IsNullOrEmpty(sValue))
                {
                    toastService.ShowError("لطفا علت لغو را وارد کنید");
                    return false;
                }

                return IsValid;
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
            // var List = _Entity.SCM_ProductRequestDetails.ToList();

            // //Console.WriteLine("#Log FormValidator :");
            // //Console.WriteLine("#Log FormValidator btn :");
            // foreach (var Item in List)
            // {
            //     // بررسی داده های خرید برای عدم ثبت داده تکراری
            //     if (Item.KH_TempNoNum != null)
            //     {
            //         SweetAlertResult result = await Swal.FireAsync(new SweetAlertOptions
            //         {
            //             Title = "بررسی داده",
            //             Text = "شما قبلا با این کد پیگیری یکبار عطف ثبت کرده اید، در صورتی که ردیف هایی از این درخواست ممکن است به صورت بدون استعلام ثبت شده باشد، لطفا مجدد این عطف    درشماران را ویرایش کرده و ردیف جدید کالا را برای صدور درخواست خرید صادر نمایید.",
            //             Icon = SweetAlertIcon.Warning,
            //             ConfirmButtonText = "متوجه شدم!!",
            //             ShowCancelButton = false,
            //             CancelButtonText = ""
            //         });
            //     }
            // }

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

            // - نام کالا
            if (Item.ProductNameText == null)
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

            // 
            // if(!Item.KH_CENTCODE_GUID.HasValue)
            // {
            //     IsCancelled = true;

            //     toastService.ShowError("لطفا گزینه جستجوی عطف را تکمیل نمایید.",
            //         settings =>
            //         {
            //             settings.Timeout = 4;
            //             settings.ShowProgressBar = true;
            //             settings.PauseProgressOnHover = true;
            //         });
            // }

            // شرط پُر بودن فیلد نام کالا دیزیبل
            if (Item.KH_TempNoNum == null)
            {
                IsValid = false;

                toastService.ShowError("لطفا گزینه شماره عطف ثبت شده در بخش خرید شماران را تکمیل نمایید",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
            }

            return IsValid;
        }

        // فیلد ارتباط با شماران
        // Id                                       عنوان
        // **************************************************
        // a5b1bc7b-8bb7-ef11-a4fa-005056a2b6bd     حواله مصرف
        // 09cf6986-8bb7-ef11-a4fa-005056a2b6bd     خرید کالا  
        // 0acf6986-8bb7-ef11-a4fa-005056a2b6bd     رسید انبار
        // f79ea98c-b6ee-ef11-a4fb-005056a2b6bd     حواله مصرف و رسید انبار

        // عطف خرید کالا در شماران سیستم
        public async Task KharidIsVisible(bool Visible)
        {
            // Console.WriteLine("#Log 2");
            // Console.WriteLine("#Log 2.1" + Ref_SCM_ProductRequestDetails_KH_Search_NotMapped.Value);
            await Task.Delay(200);

            // Ref_SCM_ProductRequestDetails_KH_Search_NotMapped.SetVisible(Visible);

            Ref_SCM_ProductRequestDetails_KH_APPROVER.SetVisible(Visible);
            Ref_SCM_ProductRequestDetails_KH_CENTCODE.SetVisible(Visible);
            Ref_SCM_ProductRequestDetails_KH_CENTCODE.SetVisible(Visible);
            Ref_SCM_ProductRequestDetails_KH_CENTCODE_GUID.SetVisible(Visible);
            Ref_SCM_ProductRequestDetails_KH_PAYCENTName.SetVisible(Visible);
            Ref_SCM_ProductRequestDetails_KH_WUSER.SetVisible(Visible);
            Ref_SCM_ProductRequestDetails_KH_TEMPNO.SetVisible(Visible);
            Ref_SCM_ProductRequestDetails_KH_TempNoNum.SetVisible(Visible);
            Ref_SCM_ProductRequestDetails_KH_ORDERNO.SetVisible(Visible);
            Ref_SCM_ProductRequestDetails_KH_ORDERNO_GUID.SetVisible(Visible);
            Ref_SCM_ProductRequestDetails_KH_ORDERDATE.SetVisible(Visible);
            Ref_SCM_ProductRequestDetails_KH_OKFACTDATE.SetVisible(Visible);
            Ref_SCM_ProductRequestDetails_KH_INVCODE.SetVisible(Visible);
            Ref_SCM_ProductRequestDetails_KH_REQPERSON.SetVisible(Visible);
            Ref_SCM_ProductRequestDetails_KH_YEAR.SetVisible(Visible);
            // جزئیات خرید کالا
            // Ref_SCM_ProductRequestDetails_SH_Kharid_DTL.SetVisible(Visible);

            Ref_SCM_ProductRequestDetails_KH_APPROVER.SetDisabled(true);
            Ref_SCM_ProductRequestDetails_KH_CENTCODE.SetDisabled(true);
            Ref_SCM_ProductRequestDetails_KH_CENTCODE.SetDisabled(true);
            Ref_SCM_ProductRequestDetails_KH_CENTCODE_GUID.SetDisabled(true);
            Ref_SCM_ProductRequestDetails_KH_PAYCENTName.SetDisabled(true);
            Ref_SCM_ProductRequestDetails_KH_WUSER.SetDisabled(true);
            Ref_SCM_ProductRequestDetails_KH_TEMPNO.SetDisabled(true);
            Ref_SCM_ProductRequestDetails_KH_TempNoNum.SetDisabled(true);
            Ref_SCM_ProductRequestDetails_KH_ORDERNO.SetDisabled(true);
            Ref_SCM_ProductRequestDetails_KH_ORDERNO_GUID.SetDisabled(true);
            Ref_SCM_ProductRequestDetails_KH_ORDERDATE.SetDisabled(true);
            Ref_SCM_ProductRequestDetails_KH_OKFACTDATE.SetDisabled(true);
            Ref_SCM_ProductRequestDetails_KH_INVCODE.SetDisabled(true);
            Ref_SCM_ProductRequestDetails_KH_REQPERSON.SetDisabled(true);
            Ref_SCM_ProductRequestDetails_KH_YEAR.SetDisabled(true);
        }

        public async Task KharidIsNull()
        {
            await Task.Delay(100);

            // Ref_SCM_ProductRequestDetails_KH_Search_NotMapped = null;
            Ref_SCM_ProductRequestDetails_KH_APPROVER = null;
            Ref_SCM_ProductRequestDetails_KH_CENTCODE = null;
            Ref_SCM_ProductRequestDetails_KH_CENTCODE = null;
            Ref_SCM_ProductRequestDetails_KH_CENTCODE_GUID = null;
            Ref_SCM_ProductRequestDetails_KH_PAYCENTName = null;
            Ref_SCM_ProductRequestDetails_KH_WUSER = null;
            Ref_SCM_ProductRequestDetails_KH_TEMPNO = null;
            Ref_SCM_ProductRequestDetails_KH_TempNoNum = null;
            Ref_SCM_ProductRequestDetails_KH_ORDERNO = null;
            Ref_SCM_ProductRequestDetails_KH_ORDERNO_GUID = null;
            Ref_SCM_ProductRequestDetails_KH_ORDERDATE = null;
            Ref_SCM_ProductRequestDetails_KH_OKFACTDATE = null;
            Ref_SCM_ProductRequestDetails_KH_INVCODE = null;
            Ref_SCM_ProductRequestDetails_KH_REQPERSON = null;
            Ref_SCM_ProductRequestDetails_KH_YEAR = null;
            // Ref_SCM_ProductRequestDetails_SH_Kharid_DTL = null;
        }

        public async Task KharidSetShomaran(dynamic Selected, Entity.SCM_ProductRequestDetails Item)
        {
            //Console.WriteLine("#Log 3");
            //Console.WriteLine(await Utility.JSON.ToJson(Selected));

            Item.KH_TEMPNO = Selected.TEMPNO;
            Item.KH_PAYCENTName = Selected.PAYCENTName;
            Item.KH_CENTCODE = Selected.CENTCODE;
            Item.KH_CENTCODE_GUID = Selected.CENTCODE_GUID;
            Item.KH_REQPERSON = Selected.REQPERSON;
            Item.KH_WUSER = Selected.WUSER;
            Item.KH_APPROVER = Selected.APPROVER;
            Item.KH_ORDERDATE = Selected.ORDERDATE;
            Item.KH_OKFACTDATE = Selected.OKFACTDATE;
            Item.KH_ORDERNO = Selected.ORDERNO;
            Item.KH_ORDERNO_GUID = Selected.ORDERNO_GUID;
            Item.KH_TempNoNum = Selected.TempNoNum;
            Item.KH_YEAR = Selected.YEAR;
            Item.KH_INVCODE = Selected.INVCODE;

            // فراخوانی داده از dropdown TempNoNum برای گرید داده های آن
            // Ref_SCM_ProductRequestDetails_SH_Kharid_DTL?.SetEntity(Item);
            // await Task.Delay(100);
            // Ref_SCM_ProductRequestDetails_SH_Kharid_DTL?.LoadData();
        }

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

        public async Task<bool> GridSCM_ProductRequestId_editmodelsaving(object e)
        {
            bool IsCancelled = false;

            var Item = (Entity.SCM_ProductRequestDetails)e;

            IsCancelled = !await CheckFieldValidation(Item);

            return IsCancelled;
        }

        public async Task GridSCM_ProductRequestId_afterrendermodal(Entity.SCM_ProductRequestDetails Item)
        {
            // // خرید
            // if (Item.KH_ORDERNO_GUID.HasValue)
            // {
            //     Ref_SCM_ProductRequestDetails_SH_Kharid_DTL.SetEntity(Item);

            //     await Task.Delay(100);
            //     Ref_SCM_ProductRequestDetails_SH_Kharid_DTL.LoadData();
            // }
        }

        public async Task KH_Search_NotMapped_onitemselected(dynamic Selected, Entity.SCM_ProductRequestDetails Item)
        {
            await KharidSetShomaran(Selected, Item);
        }

        public async Task KH_TempNoNum_NotMapped_onitemselected(dynamic Selected)
        {
            var List = _Entity.SCM_ProductRequestDetails.ToList();

            foreach (var Item in List)
            {
                await KharidSetShomaran(Selected, Item);
            }

            _Entity.KH_ORDERNO_GUID = Selected.ORDERNO_GUID;
           // Ref_KH_KharidDTL.SetEntity(_Entity);
           // Ref_KH_KharidDTL.LoadData();

            StateHasChanged();
        }

		#endregion FunctionEvents

    }
}