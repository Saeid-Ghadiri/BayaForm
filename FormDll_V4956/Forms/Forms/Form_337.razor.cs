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
    public class Form_337Base : Form_337Peropeties
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
                // var List = _Entity.SCMPETCO_ProductRequestDetails.ToList();
                // foreach (var Item in List)
                // {
                // 	// Note: توضیحات انباردار
                // 	//Item.ProductDataSheet == false;

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

            var List = _Entity.SCMPETCO_ProductRequestDetails.ToList();

            Console.WriteLine("#Log FormValidator :");

            // دکمه ثبت و ادامه
            if (BtnWorkFlowId == "SequenceFlow_0v1pluq")
            {
                Console.WriteLine("#Log FormValidator btn :");
                foreach (var Item in List)
                {
                    Console.WriteLine("#Log FormValidator btn foreach :");

                    IsValid = IsValid && await CheckFieldValidation(Item);
                }
            }
            // لغو کلی درخواست
            // SequenceFlow_1awlmjn
            if (BtnWorkFlowId == "SequenceFlow_1awlmjn")
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

            // توضیحات انباردار
            if (Item.DescriptionOfTechnicalOffice == null)
            {
                IsValid = false;
                toastService.ShowError("لطفا گزینه توضیحات دفتر فنی را تکمیل نمایید",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
            }

            return IsValid;
        }


        public async Task SCMPETCO_ProductRequestDetails_afterrendermodal(Entity.SCMPETCO_ProductRequestDetails Item)
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

        public async Task<bool> SCMPETCO_ProductRequestDetails_editmodelsaving(object e)
        {
            bool IsCancelled = false;

            var Item = (Entity.SCMPETCO_ProductRequestDetails)e;

            IsCancelled = !await CheckFieldValidation(Item);

            return IsCancelled;
        }

        #endregion FunctionEvents

    }
}