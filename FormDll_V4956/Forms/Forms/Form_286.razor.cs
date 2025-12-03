using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using Blazored.Toast.Services;

namespace Forms.Forms
{
    public class Form_286Base : Form_286Peropeties
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

                // var List = _Entity.SCM_ProductRequestDetails.ToList();
                // foreach (var Item in List)
                // {
                // }

                //  StateHasChanged();

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


            var listCount = List.Count();

            // Console.WriteLine("#Log FormValidator :");

            // SequenceFlow_12trjct		==> 42
            // SequenceFlow_0svdiko		==> 42
            // SequenceFlow_17vu1l8	==> 42
            // SequenceFlow_00joxth ==> منابع انسانی44
            //SequenceFlow_1sza1sd  ==> منابع انسانی44
            //SequenceFlow_1btbf1k ==> 51
            //SequenceFlow_0edtaa1 ==> 51
            //SequenceFlow_1ph49xj ==> 51
            //SequenceFlow_0wct1nv ==> 51
            //SequenceFlow_143i2j1 ==> 898 سرپرست کنترل کیفیت
            //SequenceFlow_0opecd2 ==> 898 سرپرست کنترل کیفیت
            //SequenceFlow_0d43lyl ==> 900 رئیس برنامه ریزی
            //SequenceFlow_1fmzt5u ==> 901 سرپرست تدارکات
            //SequenceFlow_1n7fceh ==> 902
            //SequenceFlow_02tfebw ==> 902
            //SequenceFlow_0reqsvd ==> 901 سرپرست تدارکات
            //SequenceFlow_07cwp29 ==> 902
            //SequenceFlow_144n0z9 ==> 902 
            //SequenceFlow_0kevspa ==> 902
            //SequenceFlow_1moeabf ==> 902
            //SequenceFlow_0z9yvqz ==> 904
          
            //SequenceFlow_0nji6xz ==> 905
            
            //SequenceFlow_0pwsfo7 ==> 906
            //SequenceFlow_0j6g9nr ==> 915
            //SequenceFlow_1rv867z ==> 915
            //
            // دکمه ثبت و ادامه           
            
            if ((BtnWorkFlowId == "SequenceFlow_12trjct") || (BtnWorkFlowId == "SequenceFlow_0svdiko") || (BtnWorkFlowId == "SequenceFlow_17vu1l8")|| (BtnWorkFlowId == "SequenceFlow_00joxth")|| (BtnWorkFlowId == "SequenceFlow_1btbf1k")|| (BtnWorkFlowId == "SequenceFlow_0edtaa1")|| (BtnWorkFlowId == "SequenceFlow_1ph49xj")|| (BtnWorkFlowId == "SequenceFlow_0wct1nv")|| (BtnWorkFlowId == "SequenceFlow_0opecd2")|| (BtnWorkFlowId == "SequenceFlow_0d43lyl")|| (BtnWorkFlowId == "SequenceFlow_0reqsvd")|| (BtnWorkFlowId == "SequenceFlow_07cwp29")|| (BtnWorkFlowId == "SequenceFlow_144n0z9")|| (BtnWorkFlowId == "SequenceFlow_0kevspa")|| (BtnWorkFlowId == "SequenceFlow_1moeabf")|| (BtnWorkFlowId == "SequenceFlow_1n7fceh")|| (BtnWorkFlowId == "SequenceFlow_02tfebw")|| (BtnWorkFlowId == "SequenceFlow_0z9yvqz")|| (BtnWorkFlowId == "SequenceFlow_0pwsfo7 ")|| (BtnWorkFlowId == "SequenceFlow_1sza1sd")|| (BtnWorkFlowId == "SequenceFlow_143i2j1")|| (BtnWorkFlowId == "SequenceFlow_0nji6xz ")|| (BtnWorkFlowId == "SequenceFlow_0j6g9nr")|| (BtnWorkFlowId == "SequenceFlow_1rv867z"))
            {
                // Console.WriteLine("#Log FormValidator btn :");
                foreach (var Item in List)
                {
                    //Console.WriteLine("#Log FormValidator btn foreach :");
                    IsValid = IsValid && await CheckFieldValidation(Item);
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

        #region FunctionEvent

        // /// <summary>
        // /// بررسی نوع درخواست انتخابی توسط انباردار
        // ///
        // /// Id										    Title
        // /// *********************************************************
        // /// a9c5df1c-d99c-ef11-8354-005056a02a64		تحویل کالا
        // /// 3b9e934d-d99c-ef11-8354-005056a02a64		خرید کالا
        // /// 73f6c459-d99c-ef11-8354-005056a02a64		تحویل و خرید کالا
        // /// 
        // /// بررسی ضرورت تکمیل فیلدها از این تابع انجام می شود.
        // /// </summary>
        // /// <param name="Item"></param>
        // /// <returns></returns>
        public async Task<bool> CheckFieldValidation(Entity.SCM_ProductRequestDetails Item)
        {
            bool IsValid = true;

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

            // شرط پُر بودن فیلد نام کالا دیزیبل
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

            // TDS فایل
            if (Item.ProductDataSheetTrueFalse.HasValue && Item.ProductDataSheetTrueFalse.Value)
            {
                if (Item.SCM_ProductRequestDetails_ProductDataSheetFile == null)
                {
                    IsValid = true;
                    toastService.ShowError("لطفا فایل TDS را تکمیل نمایید.",
                        settings =>
                        {
                            settings.Timeout = 4;
                            settings.ShowProgressBar = true;
                            settings.PauseProgressOnHover = true;
                        });
                }
            }

            // شرط نوع درخواست بر اساس خرید کالا
            if (Item.Global_SCMRequestTypeId.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64")
            {
                await TahvilIsVisible(false, false, Item);
                await KharidIsVisible(true, true, Item);
            }

            // ForeignMachineryProductTrueFasle - نحوه تامین کالا
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


        public async Task TahvilIsVisible(bool Visible, bool Value, Entity.SCM_ProductRequestDetails Item)
        {
            // تحویل
            Ref_SCM_ProductRequestDetails_ProductDelivery.SetVisible(Visible);
            Item.ProductDelivery = Value;
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
            await Task.Delay(100);
            // Note: فیلد آیا تامین کسری دارد؟
            if (Item.FutureActionTrueFalse.HasValue && Item.FutureActionTrueFalse.Value)
            {
                Ref_SCM_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true);
            }
            else
            {
                Ref_SCM_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);
            }

            // Note: DTS
            if (Item.ProductDataSheetTrueFalse.HasValue && Item.ProductDataSheetTrueFalse.Value)
            {
                Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductDataSheetFile.SetVisible(true);
            }
            else
            {
                Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductDataSheetFile.SetVisible(false);
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