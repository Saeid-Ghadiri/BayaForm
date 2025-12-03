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
    public class Form_657Base : Form_657Peropeties
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
                // var List = _Entity.SCMICT_ProductRequestDetails.ToList();
                // foreach (var Item in List)
                // {

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

            var List = _Entity.SCMPETCO_ProductRequestDetails.ToList();

            //Console.WriteLine("#Log FormValidator :");

            // دکمه ثبت و ادامه
            if (BtnWorkFlowId == "SequenceFlow_1a229n7")
            {
                //Console.WriteLine("#Log FormValidator btn :");
                foreach (var Item in List)
                {
                    //Console.WriteLine("#Log FormValidator btn foreach :");

                    IsValid = IsValid && await CheckFieldValidation(Item);
                }
            }

            // لغو کلی درخواست
            // SequenceFlow_10eigms
            if (BtnWorkFlowId == "SequenceFlow_193t7ku")
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

            if (Item.KH_ORDERNO_GUID.Value == null)
            {
                IsValid = false;

                toastService.ShowError("لطفا گزینه جستجوی عطف را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
            }
            return IsValid;
        }
        
        // عطف خرید کالا در شماران سیستم
        public async Task KharidIsVisible(bool Visible)
        {
            // Console.WriteLine("#Log 2");
            // Console.WriteLine("#Log 2.1" + Ref_SCMPETCO_ProductRequestDetails_KH_Search_NotMapped.Value);
            await Task.Delay(100);

            Ref_SCMPETCO_ProductRequestDetails_KH_Search_NotMapped.SetVisible(Visible);

            Ref_SCMPETCO_ProductRequestDetails_KH_APPROVER.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_KH_CENTCODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_KH_CENTCODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_KH_CENTCODE_GUID.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_KH_PAYCENTName.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_KH_WUSER.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_KH_TEMPNO.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_KH_TempNoNum.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_KH_ORDERNO.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_KH_ORDERNO_GUID.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_KH_ORDERDATE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_KH_OKFACTDATE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_KH_INVCODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_KH_REQPERSON.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_KH_YEAR.SetVisible(Visible);
            // جزئیات خرید کالا
            Ref_SCMPETCO_ProductRequestDetails_SH_Kharid_DTL.SetVisible(Visible);

            Ref_SCMPETCO_ProductRequestDetails_KH_APPROVER.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_KH_CENTCODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_KH_CENTCODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_KH_CENTCODE_GUID.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_KH_PAYCENTName.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_KH_WUSER.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_KH_TEMPNO.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_KH_TempNoNum.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_KH_ORDERNO.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_KH_ORDERNO_GUID.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_KH_ORDERDATE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_KH_OKFACTDATE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_KH_INVCODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_KH_REQPERSON.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_KH_YEAR.SetDisabled(true);
        }

        public async Task KharidIsNull()
        {
            Ref_SCMPETCO_ProductRequestDetails_KH_Search_NotMapped = null;
            Ref_SCMPETCO_ProductRequestDetails_KH_APPROVER = null;
            Ref_SCMPETCO_ProductRequestDetails_KH_CENTCODE = null;
            Ref_SCMPETCO_ProductRequestDetails_KH_CENTCODE = null;
            Ref_SCMPETCO_ProductRequestDetails_KH_CENTCODE_GUID = null;
            Ref_SCMPETCO_ProductRequestDetails_KH_PAYCENTName = null;
            Ref_SCMPETCO_ProductRequestDetails_KH_WUSER = null;
            Ref_SCMPETCO_ProductRequestDetails_KH_TEMPNO = null;
            Ref_SCMPETCO_ProductRequestDetails_KH_TempNoNum = null;
            Ref_SCMPETCO_ProductRequestDetails_KH_ORDERNO = null;
            Ref_SCMPETCO_ProductRequestDetails_KH_ORDERNO_GUID = null;
            Ref_SCMPETCO_ProductRequestDetails_KH_ORDERDATE = null;
            Ref_SCMPETCO_ProductRequestDetails_KH_OKFACTDATE = null;
            Ref_SCMPETCO_ProductRequestDetails_KH_INVCODE = null;
            Ref_SCMPETCO_ProductRequestDetails_KH_REQPERSON = null;
            Ref_SCMPETCO_ProductRequestDetails_KH_YEAR = null;
            Ref_SCMPETCO_ProductRequestDetails_SH_Kharid_DTL = null;
        }

        public async Task KharidSetShomaran(dynamic Selected, Entity.SCMPETCO_ProductRequestDetails Item)
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
            Ref_SCMPETCO_ProductRequestDetails_SH_Kharid_DTL?.SetEntity(Item);
            // await Task.Delay(100);
            Ref_SCMPETCO_ProductRequestDetails_SH_Kharid_DTL?.LoadData();
        }

        public async Task<bool> SCMPETCO_ProductRequestDetails_editmodelsaving(object e)
        {
            bool IsCancelled = false;
            // Item
            var Item = (Entity.SCMPETCO_ProductRequestDetails)e;

            IsCancelled = !await CheckFieldValidation(Item);

            return IsCancelled;
        }

        public async Task SCMPETCO_ProductRequestDetails_afterrendermodal(Entity.SCMPETCO_ProductRequestDetails Item)
        {       
           // خرید
            if (Item.KH_ORDERNO_GUID.HasValue)
            {
                Ref_SCMPETCO_ProductRequestDetails_SH_Kharid_DTL.SetEntity(Item);
                Ref_SCMPETCO_ProductRequestDetails_SH_Kharid_DTL.LoadData();
            }
        }

        public async Task KH_Search_NotMapped_onitemselected(dynamic Selected, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            await KharidSetShomaran(Selected, Item);
        }


		public async Task  KH_TempNoNum_NotMapped_onitemselected(dynamic Selected   )
        {
            var List = _Entity.SCMPETCO_ProductRequestDetails.ToList();
			
			foreach (var Item in List)
			{
    			await KharidSetShomaran(Selected, Item);
 			} 
			
			_Entity.KH_ORDERNO_GUID = Selected.ORDERNO_GUID;
			Ref_KH_KharidDTL.SetEntity(_Entity); 
			Ref_KH_KharidDTL.LoadData();
            
			StateHasChanged();
        }

		#endregion FunctionEvents

    }
}
