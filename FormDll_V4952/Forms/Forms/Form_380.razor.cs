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
    public class Form_380Base : Form_380Peropeties
    {
        // Toast  
        [Inject]
        public IToastService toastService { get; set; }

        // تابع پیام تُــست
        public MSG _MSG { get; set; }

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
            _MSG = new MSG(toastService);

            if (firstRender)
            {
                var List = _Entity.SCM_OS_Details.ToList();

                foreach (var Item in List)
                {
                    // محل عملیات
                    Item.SCM_AreaOperationId = null;

                    // آیا مدارک دارد؟
                    Item.UploadFileIsEnable = null;

                    // آیا نیاز به استعلام دارد؟
                    Item.InquiryIsEnable = null;

                }
            }
        }

        /// <summary>
        /// اعتبار سنجی فرم
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> FormValidator()
        {
            bool IsValid = true;
            var List = _Entity.SCM_OS_Details.Where(p => p.IsDelete == false).ToList();

            var listCount = List.Count();
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

        #region FunctionEvents

        public async Task<bool> CheckFieldValidation(Entity.SCM_OS_Details Item)
        {
            bool IsValid = true;

            // Note: SCM_AreaOperationId - محل عملیات
            if (Item.SCM_AreaOperationId == null)
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه محل عملیات را تکمیل نمایید");
            }

            // Note: UploadFileIsEnable
            if (Item.UploadFileIsEnable == null)
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه آیا مدارک پیوست دارد؟ را تکمیل نمایید");
            }

            // مدارک پیوست
            if (Item.UploadFileIsEnable.HasValue && Item.UploadFileIsEnable.Value)
            {
                // SCM_OS_Details_UploadFile
                if (Item.SCM_OS_Details_UploadFile == null ||
                   Item.SCM_OS_Details_UploadFile.Count < 1)
                {
                    IsValid = false;
                    await _MSG.ShowError("لطفا مدارک پیوست را بارگذاری نمایید.");
                }
            }

            // استعلام تدراکات 1
            if (Item.InquiryIsEnable.HasValue && Item.InquiryIsEnable.Value)
            {
                // SCM_OS_Details_InquiryFile1
                if (Item.SCM_OS_Details_InquiryFile1 == null ||
                   Item.SCM_OS_Details_InquiryFile1.Count < 1)
                {
                    IsValid = false;
                    await _MSG.ShowError("لطفا استعلام 1 را بارگذاری نمایید.");
                }
            }

            // Note: InquiryIsEnable
            if (Item.InquiryIsEnable == null)
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه آیا نیاز به استعلام دارد؟ را تکمیل نمایید");
            }

            //  // Note: InquiryIsEnable
            // if (Item.DesignMakeNum== null)
            // {
            //     IsValid = false;
            //     await _MSG.ShowError("لطفا گزینه شماره طراحی و ساخت را تکمیل نمایید");
            // }


            // TO_ConfirmedInquiryNum 
            if (Item.InquiryIsEnable.HasValue && Item.InquiryIsEnable.Value)
            {
                // SCM_OS_Details_InquiryFile1
                if (Item.TO_ConfirmedInquiryNum == null)
                {
                    IsValid = false;
                    await _MSG.ShowError("لطفا استعلام تایید شده واحد و دفتر فنی را بارگذاری نمایید.");
                }
            }



            return IsValid;
        }



        public async Task<bool> SCM_OS_Details_editmodelsaving(object e)
        {
            bool IsCancelled = false;

            var Item = (Entity.SCM_OS_Details)e;
            IsCancelled = !await CheckFieldValidation(Item);


            return IsCancelled;
        }

        public async Task SCM_OS_Details_afterrendermodal(Entity.SCM_OS_Details Item)
        {
            if (_Entity.SCM_ResultingFromId == null)
            {
                Ref_SCM_OS_Details_DesignMakeNum.SetVisible(false);
            }
            else
            {
                // تعمیرات
                if (_Entity.SCM_ResultingFromId.ToString() == "6b3840b6-6a26-ef11-8351-005056a02a64")
                {
                    Ref_SCM_OS_Details_DesignMakeNum.SetVisible(false);
                }
                // طراحی و ساخت
                if (_Entity.SCM_ResultingFromId.ToString() == "6c3840b6-6a26-ef11-8351-005056a02a64")
                {
                    Ref_SCM_OS_Details_DesignMakeNum.SetVisible(true);
                }
                // اجرایی
                if (_Entity.SCM_ResultingFromId.ToString() == "07503dc3-6a26-ef11-8351-005056a02a64")
                {
                    Ref_SCM_OS_Details_DesignMakeNum.SetVisible(false);
                }
            }

            // آیا مدارک دارد یا خیر؟
            if (Item.UploadFileIsEnable.HasValue && Item.UploadFileIsEnable.Value)
            {
                Ref_SCM_OS_Details_SCM_OS_Details_UploadFile.SetVisible(true);
            }
            else
            {
                Ref_SCM_OS_Details_SCM_OS_Details_UploadFile.SetVisible(false);
            }

            // بررسی حالت استعلامی
            // آیا نیاز به استعلام دارد؟
            if (Item.InquiryIsEnable.HasValue && Item.InquiryIsEnable.Value)
            {
                // فایل های استعلام
                Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile1.SetVisible(true);
                Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile2.SetVisible(true);
                Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile3.SetVisible(true);
                // کدام استعلام مورد تایید دفتر فنی است
                Ref_SCM_OS_Details_TO_ConfirmedInquiryNum.SetVisible(true);
            }
            else
            {
                // فایل های استعلام
                Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile1.SetVisible(false);
                Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile2.SetVisible(false);
                Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile3.SetVisible(false);
                // کدام استعلام مورد تایید دفتر فنی است
                Ref_SCM_OS_Details_TO_ConfirmedInquiryNum.SetVisible(false);
            }
        }

        public async Task SCM_ResultingFromId_onitemselected(Entity.SCM_ResultingFrom Selected)
        {

            // تعمیرات
            if (_Entity.SCM_ResultingFromId.ToString() == "6b3840b6-6a26-ef11-8351-005056a02a64")
            {
                Ref_SCM_OS_Details_DesignMakeNum.SetVisible(false);
            }
            // طراحی و ساخت
            if (_Entity.SCM_ResultingFromId.ToString() == "6c3840b6-6a26-ef11-8351-005056a02a64")
            {
                Ref_SCM_OS_Details_DesignMakeNum.SetVisible(true);
            }
            // اجرایی
            if (_Entity.SCM_ResultingFromId.ToString() == "07503dc3-6a26-ef11-8351-005056a02a64")
            {
                Ref_SCM_OS_Details_DesignMakeNum.SetVisible(true);
            }
        }

        public async Task UploadFileIsEnable_oninput(ChangeEventArgs Selected, Entity.SCM_OS_Details Item)
        {
            var Item1 = Ref_SCM_OS_Details_SCM_OS_Details_UploadFile;

            if (Selected.Value.ToString() == "true")
            {
                Item1.SetVisible(true);
            }
            else
            {
                Item1.SetVisible(false);
            }
        }

        public async Task InquiryIsEnable_oninput(ChangeEventArgs Selected, Entity.SCM_OS_Details Item)
        {
            // نمایش 3 فایل بارگذاری استعلام
            var InquiryFile1 = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile1;
            var InquiryFile2 = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile2;
            var InquiryFile3 = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile3;
            // کدام استعلام مورد تایید دفتر فنی است
            var TO_ConfirmedInquiryNum = Ref_SCM_OS_Details_TO_ConfirmedInquiryNum;

            if (Selected.Value.ToString() == "true")
            {
                InquiryFile1.SetVisible(true);
                InquiryFile2.SetVisible(true);
                InquiryFile3.SetVisible(true);
                TO_ConfirmedInquiryNum.SetVisible(true);
            }
            else
            {
                InquiryFile1.SetVisible(false);
                InquiryFile2.SetVisible(false);
                InquiryFile3.SetVisible(false);
                TO_ConfirmedInquiryNum.SetVisible(false);
            }
        }

        #endregion FunctionEvents

    }
}