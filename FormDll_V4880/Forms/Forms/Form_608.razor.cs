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
using Baya.Models.ORM;
using Baya.Models.Utility.Pagination.Pagings;
using Utility;


namespace Forms.Forms
{
    public class Form_608Base : Form_608Peropeties
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

            var List = _Entity.SCM_ProductRequestDetails.Where(p => p.IsDelete == false).ToList();

            var listCount = List.Count();

            // Console.WriteLine("#Log FormValidator :");

            // دکمه ثبت و ادامه           
            if (BtnWorkFlowId == "SequenceFlow_0gdypm2" || // ProcessID = 44
                BtnWorkFlowId == "SequenceFlow_1ggxm8y" || // ProcessID = 42
                BtnWorkFlowId == "SequenceFlow_19mh6os" || // ProcessID = 906
                BtnWorkFlowId == "SequenceFlow_02i9cg7" || // ProcessID = 905
                BtnWorkFlowId == "SequenceFlow_0m1875w" || // ProcessID = 902
                BtnWorkFlowId == "SequenceFlow_1rfvvl8" || // ProcessID = 901
                BtnWorkFlowId == "SequenceFlow_08ogln4" || // ProcessID = 900
                BtnWorkFlowId == "SequenceFlow_19mh6os" || // ProcessID = 898
                BtnWorkFlowId == "SequenceFlow_1g7x63k" ||  // ProcessID = 915
                BtnWorkFlowId == "SequenceFlow_07337k9"    // ProcessID = 922
                )
            {
                // Console.WriteLine("#Log FormValidator btn :");
                foreach (var Item in List)
                {
                    //Console.WriteLine("#Log FormValidator btn foreach :");
                    IsValid = IsValid && await CheckFieldValidation(Item);
                }
            }

            // // لغو کلی درخواست
            // if (BtnWorkFlowId == "SequenceFlow_12gc7zp" || // ProcessID = 44
            //     BtnWorkFlowId == "SequenceFlow_12qvs8o" || // ProcessID = 42
            //     BtnWorkFlowId == "SequenceFlow_0no30sl" || // ProcessID = 906
            //     BtnWorkFlowId == "SequenceFlow_1b66i99" || // ProcessID = 905
            //     BtnWorkFlowId == "SequenceFlow_17rw8et" || // ProcessID = 902
            //     BtnWorkFlowId == "SequenceFlow_0uq3ogg" || // ProcessID = 901
            //     BtnWorkFlowId == "SequenceFlow_0rk3edv" || // ProcessID = 900
            //     BtnWorkFlowId == "SequenceFlow_142kgx6" || // ProcessID = 898
            //     BtnWorkFlowId == "SequenceFlow_0gjizv4"    // ProcessID = 915
            //     )
            // {
            //     // Console.WriteLine("#Log:: 0");

            //     string htmlString =
            //         "<div>" +
            //             "<picture>" +
            //                 "<img src='https://File.workcv.ir/fa/api/v1/File/Get?FileID=6e5b6fb8-a5b2-490c-f83f-08dbea5b8061' class='' alt='لوگو پل‌فیلم' width='96px'>" +
            //             "</picture>" +
            //             "<hr class='hrdash border-success-subtle'>" +
            //         "</div>" +
            //         "<div class='fw-bold text-right'>" +
            //             "<div class='fs-6'>کاربر لغو کننده درخواست: " + _User.NAME + " " + _User.FAMILY + "</div>" +
            //             // "<div class='fs-6'>" + _Entity.CancellationAt + "</div>" +
            //             "<div class='fs-6'>دلیل لغو این درخواست:</div>" +
            //             "<textarea required id='InConfirmCancelationText' name='InConfirmCancelationText' " +
            //                 "style='padding: 8px; border: 1px solid #ddd; " +
            //                 "border-radius: 5px; resize: none; display: block; margin: 5px 0' " +
            //                 "width: 100%; height: 150px;' " +
            //                 "placeholder='دلیل لغو درخواست را وارد کنید...' " +
            //                 "oninput='document.getElementById(\"ConfirmCancelationText\").value=this.value'>" +
            //             "</textarea>" +
            //             "<div></div>" +
            //             "<div class='fs-6 text-secondary text-right'>" +
            //             "<i class='fal fa-exclamation-triangle px-2' style='font-size:24px; color:red'></i>" +
            //                 "کاربر محترم در نظر داشته باشید اطلاعات ثبت در این ناحیه در حال حاضر صرفاً در گزارش‌ها واحد سیستم‌ها و روش‌ها قابل نمایش است، همچنین کاربر درخواست‌دهنده از بخش درخواست‌های من می‌تواند وضعیت درخواست خود را بررسی نماید." +
            //             "</div>" +
            //         "</div>";

            //     var options = new ConfirmDialogOptions
            //     {
            //         YesButtonText = "لغو درخواست",
            //         YesButtonColor = ButtonColor.Success,
            //         NoButtonText = "انصراف",
            //         NoButtonColor = ButtonColor.Danger
            //     };

            //     var confirmation = await Confirm.ShowAsync(
            //         title: "",
            //         message1: htmlString,
            //         confirmDialogOptions: options);

            //     if (!confirmation)
            //     {
            //         IsValid = false;
            //     }
            //     else
            //     {
            //         // کاربر لغو کننده
            //         _Entity.CancelledBy = _User.UserID.ToString();
            //         // در بخش Razor فیلد input hidden همین فیلد وجود دارد و از طریق آن داده به فیلد اصلی داده می شود.
            //         string Value = await JS.InvokeAsync<string>("eval", "document.getElementById('ConfirmCancelationText')?.value || ''");
            //         _Entity.CancellationReason = Value;
            //     }
            // }

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

        // FutureActionTrueFalseTrueFalse - آیا تامین کسری انجام شود؟
        // ProductDelivery - آیا فرآیند تحویل اجرا گردد؟
        // NumberofProductDelivery - تعداد یا مقدار واگذاری کالا
        // GetDeliveryCode - دریافت کد تحویل کالا
        // DeficitSupplyNumber - تعداد تامین کسری
        public async Task TahvilIsVisible(bool Visible, bool Value, Entity.SCM_ProductRequestDetails Item)
        {
            // تحویل
            Ref_SCM_ProductRequestDetails_ProductDelivery.SetVisible(Visible);
            Item.ProductDelivery = Value;
            // تعداد یا مقدار واگذاری کالا
            Ref_SCM_ProductRequestDetails_NumberofProductDelivery.SetVisible(Visible);
            // دریافت کد تحویل کالا
            Ref_SCM_ProductRequestDetails_GetDeliveryCode.SetVisible(Visible);
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
            // تعداد یا مقدار واگذاری کالا
            Ref_SCM_ProductRequestDetails_NumberofProductDelivery.SetVisible(Visible);
            // دریافت کد تحویل کالا
            Ref_SCM_ProductRequestDetails_GetDeliveryCode.SetVisible(Visible);
        }

        public async Task AnbardarIsVisible(bool Visible)
        {
            // تعداد یا مقدار واگذاری کالا
            Ref_SCM_ProductRequestDetails_NumberofProductDelivery.SetVisible(Visible);
            // دریافت کد تحویل کالا
            Ref_SCM_ProductRequestDetails_GetDeliveryCode.SetVisible(Visible);
        }

        /// <summary>
        /// بررسی مُدهای تحویل، خرید
        /// </summary>
        /// <param name="RequestTypeMode"></param>
        /// <param name="Item"></param>
        /// Global_SCMRequestTypeId - نوع درخواست
        /// <returns></returns>
        public async Task<bool> CheckModeTahvilKharid(List<int> RequestTypeMode, Entity.SCM_ProductRequestDetails Item)
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

        // Id										Title
        // **************************************************
        // a9c5df1c-d99c-ef11-8354-005056a02a64		تحویل کالا
        // 3b9e934d-d99c-ef11-8354-005056a02a64		خرید کالا
        // 73f6c459-d99c-ef11-8354-005056a02a64		تحویل و خرید کالا
        // **************************************************
        public async Task<bool> CheckFieldValidation(Entity.SCM_ProductRequestDetails Item)
        {
            bool IsValid = true;

            var List = _Entity.SCM_ProductRequestDetails.Where(p => p.IsDelete == false).ToList();

            // تابع بررسی کد تحویل کالا توسط درخواست دهنده به انباردار
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

            // فیلد: تعداد یا مقدار واگذاری کالا به درخواست دهنده
            if ((Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64" ||
                Item.Global_SCMRequestTypeId.ToString() == "a9c5df1c-d99c-ef11-8354-005056a02a64"))
            {
                if (Item.NumberofProductDelivery == null)
                {
                    IsValid = false;

                    toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به درخواست دهنده را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });

                }
            }

            // شرایط روال خرید
            if (Item.Global_SCMRequestTypeId.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64" ||
                Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64")
            {
                if (Item.DeficitSupplyNumber == null)
                {
                    IsValid = false;

                    toastService.ShowError("لطفا تعداد تامین کسری را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });

                }
            }

            // شرط پُر بودن فیلد نوع درخواست 
            if (Item.Global_SCMRequestTypeId == null)
            {
                IsValid = false;
                toastService.ShowError("لطفا گزینه نوع درخواست  را تکمیل نمایید",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });

            }
            return IsValid;
        }

        // تبدیل تاریخ در زمان ثبت کد تحویل کالا توسط انباردار
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

        public async Task DeliveryCode_NotMapped_oninput(ChangeEventArgs Selected)
        {
            var List = _Entity.SCM_ProductRequestDetails.ToList();
            foreach (var Item in List)
            {
                Item.GetDeliveryCode = int.Parse(Selected.Value.ToString());
            }
            StateHasChanged();
        }

        public async Task<bool> GridSCM_ProductRequestId_182_editmodelsaving(object e)
        {
            bool IsCancelled = false;

            var Item = (Entity.SCM_ProductRequestDetails)e;

            IsCancelled = !await CheckFieldValidation(Item);

            return IsCancelled;
        }
        public async Task GridSCM_ProductRequestId_182_afterrendermodal(Entity.SCM_ProductRequestDetails Item)
        {
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

        // دکمه تست حواله
        public async Task submit_onclick(MouseEventArgs Selected)
        {

            Table table = new Table()
            {
                Name = "SH_PolFilm_ProductDelivery",
                Column = new List<Coulmn>()
                {
                    new Coulmn() { Name = "Id" },
                    new Coulmn() { Name = "RequestID" },
                    new Coulmn() { Name = "CreateUser" },
                    new Coulmn() { Name = "UpdateUser" },
                    new Coulmn() { Name = "CreateDate" },
                    new Coulmn() { Name = "UpdateDate" },
                    new Coulmn() { Name = "IsDelete" },
                    new Coulmn() { Name = "CENTCODE" },
                    new Coulmn() { Name = "CENTCODE2" },
                    new Coulmn() { Name = "FORMCODE" },
                    new Coulmn() { Name = "FORMYEAR" },
                    new Coulmn() { Name = "ORDERNO" },
                    new Coulmn() { Name = "ORDERYEAR" },
                    new Coulmn() { Name = "CREATOR" },
                    new Coulmn() { Name = "FACTDATE" },
                    new Coulmn() { Name = "FACTNO" },
                    new Coulmn() { Name = "H_KIND" },
                    new Coulmn() { Name = "INVCODE" },
                    new Coulmn() { Name = "MAIN_MNT" },
                    new Coulmn() { Name = "TEMPNO" },
                    new Coulmn() { Name = "NOTE" },
                    new Coulmn() { Name = "WUSER" },
                    new Coulmn() { Name = "Year" },
                },
                Relation = new List<Table>()
                {
                    new Table()
                    {
                        Name = "SH_PolFilm_ProductDeliveryDetail",
                        Column = new List<Coulmn>()
                        {
                            new Coulmn() { Name = "Id" },
                            new Coulmn() { Name = "RequestID" },
                            new Coulmn() { Name = "CreateUser" },
                            new Coulmn() { Name = "UpdateUser" },
                            new Coulmn() { Name = "CreateDate" },
                            new Coulmn() { Name = "UpdateDate" },
                            new Coulmn() { Name = "IsDelete" },
                            new Coulmn() { Name = "AMOUNT" },
                            new Coulmn() { Name = "AMOUNT2" },
                            new Coulmn() { Name = "CODE" },
                            new Coulmn() { Name = "PARTCODE" },
                            new Coulmn() { Name = "RADYABI" },
                            new Coulmn() { Name = "SEFARESH" },
                            new Coulmn() { Name = "ROW_ID" },
                            new Coulmn() { Name = "Year" }
                        },
                        //ModeErtebat = ModeErtebat._N1
                    }
                },
            };
            var dataList = new List<Entity.SH_PolFilm_ProductDeliveryDetail>();
            var data = new Entity.SH_PolFilm_ProductDelivery();
            data.CENTCODE2 = " ";
        
        // باید فیلتر شود بر اساس فقط دیتاهای تحویل
            foreach (var item in _Entity.SCM_ProductRequestDetails.Where(p=>p.Global_SCMRequestTypeId.ToString() == "a9c5df1c-d99c-ef11-8354-005056a02a64"))// تحویل
            {
                Console.WriteLine(item.PARTCODE);
                var d = new Entity.SH_PolFilm_ProductDeliveryDetail();
                d.PARTCODE = item.PARTCODE;
                d.CODE = " ";
                d.RADYABI = " ";
                d.SEFARESH = " ";
                d.AMOUNT = Convert.ToDecimal(item.ProductRequestingQTY);
                //d. = item.;
                dataList.Add(d);
            }
            data.SH_PolFilm_ProductDeliveryDetail = dataList;
            string sData = await JSON.ToJson(data);
            var DataResult = await ApiServer.External.Services.Data.Put(sData, "SH_PolFilm_ProductDelivery", table,null,_User.UserId);


            //** خرید

            Table table2 = new Table()
            {
                Name = "SH_PolFilm_Anbord",
                Column = new List<Coulmn>()
                {
                    new Coulmn() { Name = "Id" },
                    new Coulmn() { Name = "RequestID" },
                    new Coulmn() { Name = "CreateUser" },
                    new Coulmn() { Name = "UpdateUser" },
                    new Coulmn() { Name = "CreateDate" },
                    new Coulmn() { Name = "UpdateDate" },
                    new Coulmn() { Name = "IsDelete" },
                    new Coulmn() { Name = "CENTCODE" },
                    new Coulmn() { Name = "CREATOR" },
                    new Coulmn() { Name = "CLOSED" },
                    new Coulmn() { Name = "FORMCODE" },
                    new Coulmn() { Name = "FORMYEAR" },
                    new Coulmn() { Name = "REQPERSON" },
                    new Coulmn() { Name = "ORDERNO" },
                    new Coulmn() { Name = "INVCODE" },
                    new Coulmn() { Name = "MAIN_MNT" },
                    new Coulmn() { Name = "NDATE" },
                    new Coulmn() { Name = "ORDERDATE" },
                    new Coulmn() { Name = "OKFACTDATE" },
                    new Coulmn() { Name = "ORDWANTNO" },
                    new Coulmn() { Name = "PSOURCE" },
                    new Coulmn() { Name = "TEMPNO" },
                    new Coulmn() { Name = "WUSER" },
                    new Coulmn() { Name = "ORDERYEAR" },
                    new Coulmn() { Name = "NOTE" },
                    new Coulmn() { Name = "YEAR" },
                    new Coulmn() { Name = "Description" },
                    new Coulmn() { Name = "MOJ" },
                    new Coulmn() { Name = "OKAMOUNT" },
                    new Coulmn() { Name = "ORDAMOUNT" },
                    new Coulmn() { Name = "DESC1" },
                    
                },
                Relation = new List<Table>()
                {
                    new Table()
                    {
                        Name = "SH_PolFilm_AnbordDetail",
                        Column = new List<Coulmn>()
                        {
                            new Coulmn() { Name = "Id" },
                            new Coulmn() { Name = "RequestID" },
                            new Coulmn() { Name = "CreateUser" },
                            new Coulmn() { Name = "UpdateUser" },
                            new Coulmn() { Name = "CreateDate" },
                            new Coulmn() { Name = "UpdateDate" },
                            new Coulmn() { Name = "IsDelete" },
                            new Coulmn() { Name = "OKAMOUNT" },
                            new Coulmn() { Name = "ORDAMOUNT" },
                            new Coulmn() { Name = "ORDAMOUNT1" },
                            new Coulmn() { Name = "PARTCODE" },
                            new Coulmn() { Name = "RADYABI" },
                            new Coulmn() { Name = "SEFARESH" },
                            new Coulmn() { Name = "RowId" },
                            new Coulmn() { Name = "Year" },
                            new Coulmn() { Name = "MOJ" },
                            new Coulmn() { Name = "DESC1" }
                        },
                        //ModeErtebat = ModeErtebat._N1
                    }
                },
            };
            var dataList2 = new List<Entity.SH_PolFilm_AnbordDetail>();
            var data2 = new Entity.SH_PolFilm_Anbord();
            data2.CENTCODE = " ";//_Entity.CENTCODE;

            // باید فیلتر شود بر اساس فقط دیتاهای تحویل
            foreach (var item in _Entity.SCM_ProductRequestDetails.Where(p => p.Global_SCMRequestTypeId.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64"))// خرید
            {
                Console.WriteLine(item.PARTCODE);
                var d = new Entity.SH_PolFilm_AnbordDetail();
                d.PARTCODE = item.PARTCODE;
                d.RADYABI = " ";
                d.SEFARESH = " ";
                d.OKAMOUNT = Convert.ToDecimal(item.ProductRequestingQTY);
                //d. = item.;
                dataList2.Add(d);
            }
            data2.SH_PolFilm_AnbordDetail = dataList2;
            string sData2 = await JSON.ToJson(data2);
            var DataResult2 = await ApiServer.External.Services.Data.Put(sData2, "SH_PolFilm_Anbord", table2, null, _User.UserId);

        }

		

		#endregion FunctionEvents
    }
}