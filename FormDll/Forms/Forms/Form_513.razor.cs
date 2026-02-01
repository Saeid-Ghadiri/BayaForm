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
    public class Form_513Base : Form_513Peropeties
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
                var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();

                foreach (var Item in List)
                {
                    // نحوه تامین کالا
                    // Item.SupplyGoodsIsEnable = null;              
                }

                StateHasChanged();
            }
        }

        /// <summary>
        /// اعتبار سنجی فرم
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> FormValidator()
        {
            bool IsValid = true;

            var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();

            //Console.WriteLine("#Log FormValidator :");

            // دکمه ثبت و ادامه
            if (BtnWorkFlowId == "SequenceFlow_0wthill")
            {
                //Console.WriteLine("#Log FormValidator btn :");
                foreach (var Item in List)
                {
                    //Console.WriteLine("#Log FormValidator btn foreach :");

                    // تابع بررسی کد تحویل کالا
//                    IsValid = await CheckDeliveryCode(true);

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
        public async Task<bool> CheckFieldValidation(Entity.SCMPLATE_ProductRequestDetails Item)
        {
            bool IsValid = true;

            var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();

            // فیلد نوع درخواست
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

            //// - بررسی فیلدهای شماران - کالای جدید نباشد
            //if (Item.SH_DESC != null && Item.SH_DESC == "0000000000")
            //{
            //    IsValid = false;
            //    toastService.ShowError("كالاي جديد و کد کالای 0000000000 را نمیتوانید ثبت نمایید لطفا بر اساس آیتم جستجو تغییرات را اعمال کنید.",
            //        settings =>
            //        {
            //            settings.Timeout = 4;
            //            settings.ShowProgressBar = true;
            //            settings.PauseProgressOnHover = true;
            //        });
            //}

            // کد تحویل
            if (Item.Global_SCMRequestTypeId.ToString() == "a9c5df1c-d99c-ef11-8354-005056a02a64" || Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64" )
               
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
                if (Item.NumberofGoodsDelivery == null)
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

         

               // نقطه سفارش دارد؟	

            if(Item.Global_SCMRequestTypeId.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64" ||
                Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64")
            {
                if (Item.HasOrderPoint == null)
                {
                    IsValid = false;
    
                    toastService.ShowError("لطفا گزینه نقطه سفارش دارد؟ را تکمیل نمایید.",
                        settings =>
                        {
                            settings.Timeout = 4;
                            settings.ShowProgressBar = true;
                            settings.PauseProgressOnHover = true;
                        });
                }
            }
            

            // شرط پُر بودن فیلد نام کالا 
            if (Item.SH_DESC == null)
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
            if (Item.PlaceOfUse == null)
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
            if (Item.Global_PriorityId == null)
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
            if (Item.SupplyGoodsIsEnable == null)
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

        /// <summary>
        /// ثبت و تبدیل تاریخ تحویل کالا به درخواست دهنده
        /// </summary>
        /// <returns></returns>
        public async Task ChangeDateTime_Anbardar()
        {
            var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();
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

            var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();
            for (int i = 0; i < List.Count(); i++)
            {
                var Item = List[i];
                if (Item.DeliveryCode != Item.GetDeliveryCode)
                {
                    IsValid = false;
                    toastService.ShowError("کد تحویل وارد شده صحیح نیست!! لطفا کد صحیح را وارد نمایید.");
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
        public async Task<bool> CheckModeTahvilKharid(List<int> RequestTypeMode, Entity.SCMPLATE_ProductRequestDetails Item)
        {
            bool IsEmpty = (String.IsNullOrEmpty(Item.Global_SCMRequestTypeId.ToString()));

            foreach (var i in RequestTypeMode)
            {
                //Console.WriteLine(await Utility.JSON.ToJson(Item));

                switch (i)
                {
                    //تحویل است یا نه
                    case 1:

                        //Console.WriteLine("#Log Mode 1 ");

                        if (Item.Global_SCMRequestTypeId.ToString() == "a9c5df1c-d99c-ef11-8354-005056a02a64")
                        {
                            await TahvilIsVisible(true, true, Item);
                            await KharidIsVisible(false, false, Item);
                        }

                        break;
                    //خرید است یا نه
                    case 2:

                        //Console.WriteLine("#Log Mode 2 ");

                        if (Item.Global_SCMRequestTypeId.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64")
                        {
                            await TahvilIsVisible(false, false, Item);
                            await KharidIsVisible(true, true, Item);
                        }

                        break;
                    // تحویل و خرید با هم است
                    case 3:

                        //Console.WriteLine("#Log Mode 3 ");

                        if (Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64")
                        {
                            await TahvilKharidIsVisible(true, true, Item);
                        }

                        break;
                    // حالت پیش فرض
                    default:

                        //Console.WriteLine("#Log Mode 4 ");

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

        public async Task TahvilIsVisible(bool Visible, bool Value, Entity.SCMPLATE_ProductRequestDetails Item)
        {
            // تحویل
            Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(Visible);
            Item.GoodsDeliveryIsEnable = Value;

            // ثبت کد تحویل - notmapped
            Ref_InsertDeliveryCode.SetVisible(Visible);

             // تعداد یا مقدار واگذاری کالا -  NumberofGoodsDelivery
            Ref_SCMPLATE_ProductRequestDetails_NumberofGoodsDelivery.SetVisible(Visible);
            // field is double
            //Item.NumberofGoodsDelivery = Value;
            Ref_SCMPLATE_ProductRequestDetails_GetDeliveryCode?.SetVisible(Visible);

            Ref_SCMPLATE_ProductRequestDetails_OldBuyCount.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_OldBuyDate.SetVisible(Visible);
        }

        public async Task KharidIsVisible(bool Visible, bool Value, Entity.SCMPLATE_ProductRequestDetails Item)
        {
            // خرید
            Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(Visible);
            Item.DeficitSupplyIsEnable = Value;
            // تعداد تامین کسری	
            Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyNumber.SetVisible(Visible);

            // نقطه سفارش دارد
            Ref_SCMPLATE_ProductRequestDetails_HasOrderPoint.SetVisible(Visible);

            Ref_SCMPLATE_ProductRequestDetails_OldBuyCount.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_OldBuyDate.SetVisible(Visible);
        }

        public async Task TahvilKharidIsVisible(bool Visible, bool Value, Entity.SCMPLATE_ProductRequestDetails Item)
        {
            // تحویل
            Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(Visible);
            Item.GoodsDeliveryIsEnable = Value;
            // خرید
            Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(Visible);
            Item.DeficitSupplyIsEnable = Value;
            // تعداد تامین کسری
            Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyNumber.SetVisible(Visible);
            // نقطه سفارش دارد
            Ref_SCMPLATE_ProductRequestDetails_HasOrderPoint.SetVisible(Visible);
            // ثبت کد تحویل - notmapped
            Ref_InsertDeliveryCode.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_GetDeliveryCode?.SetVisible(Visible);
            // تعداد یا مقدار واگذاری کالا -  NumberofGoodsDelivery
            Ref_SCMPLATE_ProductRequestDetails_NumberofGoodsDelivery.SetVisible(Visible);
            // field is double
            //Item.NumberofGoodsDelivery = Value;

            Ref_SCMPLATE_ProductRequestDetails_OldBuyCount.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_OldBuyDate.SetVisible(Visible);
        }

        public async Task AnbardarIsVisible(bool Visible)
        {
            // تعداد یا مقدار واگذاری کالا -  NumberofGoodsDelivery
            Ref_SCMPLATE_ProductRequestDetails_NumberofGoodsDelivery.SetVisible(Visible);
            // دریافت کد تحویل کالا - GetDeliveryCode
            Ref_SCMPLATE_ProductRequestDetails_GetDeliveryCode.SetVisible(Visible);
        }

        #endregion / RequestTypeMode

        public async Task<bool> SCMPLATE_ProductRequestDetails_editmodelsaving(object e)
        {
            bool IsCancelled = false;

            var Item = (Entity.SCMPLATE_ProductRequestDetails)e;

            IsCancelled = !await CheckFieldValidation(Item);

            return IsCancelled;
        }

        public async Task SCMPLATE_ProductRequestDetails_afterrendermodal(Entity.SCMPLATE_ProductRequestDetails Item)
        {
            // Note: مخفی کردن فیلد تعداد تامین کسری بر اساس فیلد آیا تامین کسری دارد؟ و آیا نیاز به استعلام دارد یا خیر؟
            if (Item.DeficitSupplyIsEnable.HasValue && Item.DeficitSupplyIsEnable.Value)
            {
                Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true);
            }
            else
            {
                Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);
            }

            // فیلد نوع درخواست
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

            // نمایش جزئیات موجودی اول دوره و جای گذاری کالا در انبار
            if (Item.SH_PARTCODE != null)
            {
                Ref_SCMPLATE_ProductRequestDetails_SH_FirstAmountDTL.SetEntity(Item);
                Ref_SCMPLATE_ProductRequestDetails_SH_FirstAmountDTL.LoadData();
            }
        }

        public async Task DeficitSupplyIsEnable_oninput(ChangeEventArgs Selected, Entity.SCMPLATE_ProductRequestDetails Item)
        {
            // Note: مخفی کردن فیلد تعداد تامین کسری بر اساس فیلد آیا تامین کسری دارد؟       
            var Item1 = Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyNumber;
            if (Selected.Value.ToString() == "true")
            {
                Item1.Visible = true;
            }
            else
            {
                Item1.Visible = false;
            }
        }
        public async Task Global_SCMRequestTypeId_onitemselected(dynamic Selected, Entity.SCMPLATE_ProductRequestDetails Item)
        {
            // Console.WriteLine(await Utility.JSON.ToJson(Selected));

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

        public async Task InsertDeliveryCode_oninput(ChangeEventArgs Selected)
        {
            var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();
            foreach (var Item in List)
            {
                Item.GetDeliveryCode = int.Parse(Selected.Value.ToString());
            }
            StateHasChanged();
        }

        public async Task ProductSearch_NotMapped_onitemselected(dynamic Selected, Entity.SCMPLATE_ProductRequestDetails Item)
        {
            /// <summary>
            /// فیلدهای زیر فیلدهای اصلی برای نمایش در فرم هستند.
            ///
            ///</summary>

            //  نام کالا
            Item.SH_DESC = Selected.DESC;
            // کد کالا شماران
            Item.SH_PARTNO = Selected.PARTNO;
            // شناسه سیستمی کد کالا شماران
            Item.SH_PARTNO_GUID = Selected.PARTNO_GUID;
            // شماره کالا شماران
            Item.SH_PARTCODE = Selected.PARTCODE;
            // شناسه شماره کالا شماران
            Item.SH_PARTCODE_GUID = Selected.PARTCODE_GUID;
            // نام کالا شماران
            Item.SH_DESC = Selected.DESC;
            // واحد کالا شماران
            Item.SH_UNIT = Selected.UNIT;
            // کد دسته بندی فرعی کالا شماران
            Item.SH_SUBGRCODE = Selected.SUBGRCODE;
            // شناسه کد دسته بندی فرعی کالا شماران
            Item.SH_SUBGRCODE_GUID = Selected.SUBGRCODE_GUID;
            // کد دسته بندی اصلی شماران
            Item.SH_GRCODE = Selected.GRCODE;
            // شناسه کد دسته بندی اصلی کالا شماران
            Item.SH_GRCODE_GUID = Selected.GRCODE_GUID;
            // نام دسته بندی اصلی کالا شماران
            Item.SH_GroupName = Selected.GroupName;
            // نام دسته بندی فرعی کالا شماران
            Item.SH_SubGroupName = Selected.SubGroupName;
            // سال مالی کالا شماران
            Item.SH_YEAR = Selected.YEAR;
            // کالا موجود است یا خیر شماران
            Item.SH_IsExist = Selected.IsExist;
            // نام شرکت کالا شماران
            Item.SH_Factory = Selected.Factory;
            // کد گروه اصلی کالا شماران
            Item.SH_MapGroupCode = Selected.MapGroupCode;

            // موجودی کالا در شماران            
            if (Selected.Amount > -1)
            {
                Item.SH_Amount = (double)Selected.Amount;
            }

            // نمایش جزئیات موجودی اول دوره و جای گذاری کالا در انبار
            if (Item.SH_PARTCODE != null)
            {
                Ref_SCMPLATE_ProductRequestDetails_SH_FirstAmountDTL.SetEntity(Item);
                Ref_SCMPLATE_ProductRequestDetails_SH_FirstAmountDTL.LoadData();
            }
            StateHasChanged();
        }

        #endregion FunctionEvents

    }
}