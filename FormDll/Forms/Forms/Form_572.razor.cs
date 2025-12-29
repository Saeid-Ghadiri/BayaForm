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
    public class Form_572Base : Form_572Peropeties
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

            var List = _Entity.SCMPETCO_ProductRequestDetails.ToList();

            var List1 = _Entity.SCMPETCO_ProductRequestDetails.Where(p => p.IsDelete == false).ToList();

            int ListCount = List.Count();

            // Console.WriteLine("#Log FormValidator :");
            // دکمه ثبت و ادامه           
            if ((BtnWorkFlowId == "SequenceFlow_0j93bvr") || (BtnWorkFlowId == "SequenceFlow_1jsth7e"))
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
            if ((BtnWorkFlowId == "SequenceFlow_1o3ih8y") || (BtnWorkFlowId == "SequenceFlow_02c1lkm"))
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

            // نحوه تامین کالا
            if (Item.ForeignMachineryProduct == null)
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

            return IsValid;
        }

        public async Task ITILVisible(bool Visible)
        {
            Ref_SCMPETCO_ProductRequestDetails_ResultingFromITIL.SetVisible(Visible);
        }

        public async Task ITILDetailsVisible(bool Visible)
        {
            Ref_SCMPETCO_ProductRequestDetails_RequestIdITIL.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_RequestIdITIL.SetDisabled(true);

            Ref_SCMPETCO_ProductRequestDetails_RequesterUserITIL.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_RequesterUserITIL.SetDisabled(true);

            Ref_SCMPETCO_ProductRequestDetails_CreatedAtITIL.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_CreatedAtITIL.SetDisabled(true);

            Ref_SCMPETCO_ProductRequestDetails_ITILDetails.SetVisible(Visible);
        }

        public async Task TahvilIsVisible(bool Visible, bool Value, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            await Task.Delay(100);
            // تحویل
            Ref_SCMPETCO_ProductRequestDetails_ProductDelivery.SetVisible(Visible);
            Item.ProductDelivery = Value;
        }

        public async Task KharidIsVisible(bool Visible, bool Value, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            await Task.Delay(100);
            // خرید
            Ref_SCMPETCO_ProductRequestDetails_FutureAction.SetVisible(Visible);
            Item.FutureAction = Value;
            // تعداد تامین کسری	
            Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber.SetVisible(Visible);
        }

        public async Task TahvilKharidIsVisible(bool Visible, bool Value, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            await Task.Delay(100);
            // تحویل
            Ref_SCMPETCO_ProductRequestDetails_ProductDelivery.SetVisible(Visible);
            Item.ProductDelivery = Value;
            // خرید
            Ref_SCMPETCO_ProductRequestDetails_FutureAction.SetVisible(Visible);
            Item.FutureAction = Value;
            // تعداد تامین کسری
            Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber.SetVisible(Visible);
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

        public async Task<bool> SCMPETCO_ProductRequestDetails_editmodelsaving(object e)
        {

            bool IsCancelled = false;

            var Item = (Entity.SCMPETCO_ProductRequestDetails)e;
            IsCancelled = !await CheckFieldValidation(Item);


            return IsCancelled;
        }

        public async Task SCMPETCO_ProductRequestDetails_afterrendermodal(Entity.SCMPETCO_ProductRequestDetails Item)
        {
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

            // // Show ITIL
            // if (Item.ITILCodeIsEnable.HasValue && Item.ITILCodeIsEnable.Value)
            // {
            //     await ITILVisible(true); 
            // }
            // else
            // {
            //     await ITILVisible(false);
            // }

            // Show ITIL Details
            if (!string.IsNullOrEmpty(Item.ResultingFromITIL))
            {
                await ITILDetailsVisible(true);
            }
            else
            {
                await ITILDetailsVisible(false);
            }

            // نمایش جزئیات ITIL
            if (Item.ResultingFromITIL != null)
            {
                Ref_SCMPETCO_ProductRequestDetails_ITILDetails.SetEntity(Item);
                Ref_SCMPETCO_ProductRequestDetails_ITILDetails.LoadData();
            }
        }

        // public async Task  ITILCodeIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCMPETCO_ProductRequestDetails Item  )
        // {
        // 	// نمایش / عدم نمایش فیلد منتج از ITIL
        // 	if (Selected.Value.ToString() == "true")
        // 	{
        // 	    await ITILVisible(true);
        // 	}
        // 	else
        // 	{
        // 	    await ITILVisible(false); 
        // 	}
        // }

        public async Task ResultingFromITIL_onitemselected(dynamic Selected, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            // نمایش / عدم نمایش فیلد ITIL Detail
            await ITILDetailsVisible(true);

            if (Item.ResultingFromITIL != null)
            {

                Item.RequestIdITIL = Selected.RequestID;
                Item.RequesterUserITIL = Selected.UserName;
                Item.CreatedAtITIL = Selected.CreateDate;

                await Task.Delay(100);
                Ref_SCMPETCO_ProductRequestDetails_ITILDetails.SetEntity(Item);
                Ref_SCMPETCO_ProductRequestDetails_ITILDetails.LoadData();
            }
            StateHasChanged();
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
