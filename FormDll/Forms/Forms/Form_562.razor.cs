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
using Microsoft.IdentityModel.Tokens;

namespace Forms.Forms
{
    public class Form_562Base : Form_562Peropeties
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

            var List = _Entity.SCMPETCO_OS_Details.ToList();

            int ListCount = List.Count();

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
                        "<img src='https://file.workcv.ir/fa/api/v1/File/Get?FileID=a18190d9-4b1c-45e3-db13-08dc25546b46' class='' alt='لوگو پتکو' width='96px;'>" +
                    "</picture>" +
                    "<hr class='hrdash border-success-subtle'>" +
                "</div>" +
                "<div class='fw-bold text-center'>" +
                "<span class='fs-5'>کد پیگیری این درخواست: </span>" +
                "<span class='fs-3' style='color: #1ba156'>" + _Entity.RequestTrakingCode + "</span><div>" +
                "<span><i class='fal fa-exclamation-triangle' style='font-size:24px; color:red;'></i>&nbsp;</span>" +
                "<span class='fs-6 text-secondary text-right'>تا کنون هیچ ردیف درخواستی تکمیل نشده است. لطفا برای ثبت و ادامه به مرحله بعد حداقل یک ردیف در درخواست خود ثبتنمایید.	<span></div>" +
                "</div>";
                var confirmation = await Confirm.ShowAsync(
                    title: "",
                    message1: htmlString,
                    confirmDialogOptions: options);
            }

            // SequenceFlow_1awhatg - ثبت و ادامه - مدیر فناوری اطلاعات

            if (BtnWorkFlowId == "SequenceFlow_1awhatg")
            {
                foreach (var Item in List)
                {
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

            if ((await BeforSubmit()).Status != HttpStatusCode.OK)
            {
                StateHasChanged();
                Result.Status = HttpStatusCode.InternalServerError;
                return Result;
            }

            _loadingService.Show();
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
            _loadingService.Hide();
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
        public async Task<bool> CheckFieldValidation(Entity.SCMPETCO_OS_Details Item)
        {
            bool IsValid = true;

            var List = _Entity.SCMPETCO_OS_Details.ToList();

            // OS_JobTitle - عنوان کار
            if (Item.OS_JobTitle == null)
            {
                IsValid = false;

                toastService.ShowError("لطفا گزینه عنوان کار را تکمیل نمایید.",
                settings =>
                {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }

            // Amount - تعداد 
            if (Item.Amount == null || Item.Amount == 0)
            {
                IsValid = false;

                toastService.ShowError("لطفا گزینه تعداد را تکمیل نمایید.",
                settings =>
                {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }

            // فیلد اولویت
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

            // فیلد واحد کالا
            if (Item.Global_UnitsId == null)
            {
                IsValid = false;

                toastService.ShowError("لطفا گزینه واحد کالا را تکمیل نمایید.",
                settings =>
                {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }

            // UploadFileIsEnable - آیا مدارک پیوست دارد؟
            if (Item.UploadFileIsEnable == null)
            {
                IsValid = false;

                toastService.ShowError("لطفا گزینه آیا مدارک پیوست دارد؟ را تکمیل نمایید.",
                settings =>
                {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }

            // فیلد: فایل مدارک پیوست
            if (Item.UploadFileIsEnable.HasValue && Item.UploadFileIsEnable.Value)
            {
                if (Item.SCMPETCO_OS_Details_UploadFiles == null)
                {
                    IsValid = false;

                    toastService.ShowError("لطفا گزینه فایل مدارک پیوست را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
            }

            // SCMPETCO_OS_Details_SampleGoodsFiles - آیا نمونه دارد؟>
            if (Item.IsEnableSampleGoods == null)
            {
                IsValid = false;

                toastService.ShowError("لطفا گزینه آیا نمونه دارد؟ را تکمیل نمایید.",
                settings =>
                {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }

            // فیلد: فایل نمونه؟
            if (Item.IsEnableSampleGoods.HasValue && Item.IsEnableSampleGoods.Value)
            {
                if (Item.SCMPETCO_OS_Details_SampleGoodsFiles == null)
                {
                    IsValid = false;

                    toastService.ShowError("لطفا گزینه آیا نمونه دارد؟ را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
            }

            // آیا گارانتی دارد؟ - GuaranteeIsEnable
            if (Item.GuaranteeIsEnable == null)
            {
                IsValid = false;
                toastService.ShowError("لطفا گزینه آیا گارانتی دارد؟ را تکمیل نمایید.",
                settings =>
                {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }

            // سابقه تعمیرات دارد؟ - HistoryRepairsIsEnable
            if (Item.HistoryRepairsIsEnable == null)
            {
                IsValid = false;
                toastService.ShowError("لطفا گزینه سابقه تعمیرات دارد؟ را تکمیل نمایید.",
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
            Ref_SCMPETCO_OS_Details_ResultingFromITIL.SetVisible(Visible);
            Ref_SCMPETCO_OS_Details_ResultingFromITIL.SetDisabled(true);
        }

        public async Task ITILDetailsVisible(bool Visible)
        {
            Ref_SCMPETCO_OS_Details_RequestIdITIL.SetVisible(Visible);
            Ref_SCMPETCO_OS_Details_RequestIdITIL.SetDisabled(true);

            Ref_SCMPETCO_OS_Details_RequesterUserITIL.SetVisible(Visible);
            Ref_SCMPETCO_OS_Details_RequesterUserITIL.SetDisabled(true);

            Ref_SCMPETCO_OS_Details_CreatedAtITIL.SetVisible(Visible);
            Ref_SCMPETCO_OS_Details_CreatedAtITIL.SetDisabled(true);

            Ref_SCMPETCO_OS_Details_ITILDetails.SetVisible(Visible);
            Ref_SCMPETCO_OS_Details_ITILDetails.SetDisabled(true);

        }

        public async Task ResultingFrom_Code13(bool Visible, bool Value, Entity.SCMPETCO_OS_Details Item)
        {
            // IsEnableDemolitionAndRenovation
            Ref_SCMPETCO_OS_Details_IsEnableDemolitionAndRenovation.SetVisible(Visible);
            Item.IsEnableDemolitionAndRenovation = Value;
        }



        public async Task<bool> SCMPETCO_OS_Details_editmodelsaving(object e)
        {
            bool IsCancelled = false;

            var Item = (Entity.SCMPETCO_OS_Details)e;

            IsCancelled = !await CheckFieldValidation(Item);


            return IsCancelled;
        }

        public async Task SCMPETCO_OS_Details_afterrendermodal(Entity.SCMPETCO_OS_Details Item)
        {
            // 
            if (Item.UploadFileIsEnable.HasValue && Item.UploadFileIsEnable.Value)
            {
                Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_UploadFiles.SetVisible(true);
            }
            else
            {
                Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_UploadFiles.SetVisible(false);
            }


            // فیلد: منتج از پیمانکار با Code 13
            if (_Entity.SCMPETCO_OS_ResultingFromId.HasValue && _Entity.SCMPETCO_OS_ResultingFromId.ToString() == "2a087a09-9fbe-ef11-a4fa-005056a2b6bd")
            {

                await ResultingFrom_Code13(true, true, Item);
            }
            else
            {

                await ResultingFrom_Code13(false, false, Item);
            }

            // فایل نمونه
            if (Item.IsEnableSampleGoods.HasValue && Item.IsEnableSampleGoods.Value)
            {
                Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_SampleGoodsFiles.SetVisible(true);
            }
            else
            {
                Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_SampleGoodsFiles.SetVisible(false);
            }

            // Show ITIL
            if (Item.ITILCodeIsEnable.HasValue && Item.ITILCodeIsEnable.Value)
            {
                // تابع ITIL
                await ITILVisible(true);
            }
            else
            {
                // تابع ITIL
                await ITILVisible(false);
            }

            // Show ITIL Details
            if (!string.IsNullOrEmpty(Item.ResultingFromITIL))
            {
                // تابع ITILDetails
                await ITILDetailsVisible(true);
            }
            else
            {
                // تابع ITILDetails
                await ITILDetailsVisible(false);
            }

            // 
            if (Item.ResultingFromITIL != null)
            {
                Ref_SCMPETCO_OS_Details_ITILDetails.SetEntity(Item);
                Ref_SCMPETCO_OS_Details_ITILDetails.LoadData();
            }
        }
        public async Task SCMPETCO_OS_ResultingFromId_onitemselected(Entity.SCMPETCO_OS_ResultingFrom Selected)
        {
        }

        public async Task IsEnableSampleGoods_oninput(ChangeEventArgs Selected, Entity.SCMPETCO_OS_Details Item)
        {
            // مخفی کردن فیلد فایل نمونه
            var SampleFileIsVisible = Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_SampleGoodsFiles;

            if (Selected.Value.ToString() == "true")
            {
                SampleFileIsVisible.SetVisible(true);
            }
            else
            {
                SampleFileIsVisible.SetVisible(false);
            }
        }

        public async Task UploadFileIsEnable_oninput(ChangeEventArgs Selected, Entity.SCMPETCO_OS_Details Item)
        {
        }


        public async Task ITILCodeIsEnable_oninput(ChangeEventArgs Selected, Entity.SCMPETCO_OS_Details Item)
        {
            // نمایش / عدم نمایش فیلد منتج از ITIL
            if (Selected.Value.ToString() == "true")
            {
                await ITILVisible(true);
            }
            else
            {
                await ITILVisible(false);
            }
        }

        public async Task ResultingFromITIL_onitemselected(dynamic Selected, Entity.SCMPETCO_OS_Details Item)
        {
            // نمایش / عدم نمایش فیلد ITIL Detail
            await ITILDetailsVisible(true);

            if (Item.ResultingFromITIL != null)
            {
                Item.RequestIdITIL = Selected.RequestID;
                Item.RequesterUserITIL = Selected.UserName;
                Item.CreatedAtITIL = Selected.CreateDate;

                Ref_SCMPETCO_OS_Details_ITILDetails.SetEntity(Item);
                Ref_SCMPETCO_OS_Details_ITILDetails.LoadData();
            }
        }

        #endregion FunctionEvents

    }
}
