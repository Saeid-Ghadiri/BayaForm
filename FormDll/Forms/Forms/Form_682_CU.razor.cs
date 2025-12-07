using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;

namespace Forms.Forms
{
    public class Form_682_CUBase : Form_682_CUPeropeties
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
                // تعریف مدل پیام بر اساس تابع تعریف شده
                _MSG = new MSG(toastService);
            }
        }

        /// <summary>
        /// اعتبار سنجی فرم
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> FormValidator()
        {
            bool IsValid = true;

            // بررسی تابع اعتبارسنجی فیلد ها
            //IsValid = await CheckFieldValidation(_Entity);


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

        // تابع پیام تــست
        public MSG _MSG { get; set; }

        public async Task<bool> CheckFieldValidation(Entity.HR_ORG_Positions Item)
        {
            bool IsValid = true;

            // **************************************************

            // // فیلد طبقه بندی سمت
            // if (Item.HR_ORG_PositionClassificationId == null)
            // {
            //     IsValid = false;
            //     await _MSG.ShowError("لطفا گزینه طبقه بندی سمت را تکمیل نمایید.");
            // }

            // // فیلد کد سمت
            // if (Item.Code == null)
            // {
            //     IsValid = false;
            //     await _MSG.ShowError("لطفا گزینه کد سمت را تکمیل نمایید.");
            // }

            // // فیلد عنوان سمت
            // if (Item.Title == null)
            // {
            //     IsValid = false;
            //     await _MSG.ShowError("لطفا گزینه عنوان سمت را تکمیل نمایید.");
            // }

            // // فیلد تایید کننده است؟
            // if (!(Item.IsApprover.HasValue && Item.IsApprover.Value))
            // {
            //     IsValid = false;
            //     await _MSG.ShowError("لطفا گزینه تایید کننده است؟ را تکمیل نمایید.");
            // }

            // **************************************************

            // **************************************************
            // Details 

            // foreach (var item in _Entity.HR_ORG_Posts)
            // {
            //     // فیلد کد پست
            //     if (item.Code == null)
            //     {
            //         IsValid = false;
            //         await _MSG.ShowError("لطفا گزینه کد پست را تکمیل نمایید.");
            //     }

            //     // فیلد عنوان پست سازمانی
            //     if (item.Title == null)
            //     {
            //         IsValid = false;
            //         await _MSG.ShowError("لطفا گزینه پست سازمانی را تکمیل نمایید.");
            //     }
            // }


            // **************************************************

            return IsValid;
        }


        public async Task<bool> HR_ORG_Posts_editmodelsaving(object e)
        {
            bool IsCancelled = false;

            var Item = (Entity.HR_ORG_Posts)e;

            // // فیلد کد پست
            // if (Item.Code == null)
            // {
            //     IsCancelled = true;
            //     await _MSG.ShowError("لطفا گزینه کد پست را تکمیل نمایید.");
            // }

            // // فیلد عنوان پست سازمانی
            // if (Item.Title == null)
            // {
            //     IsCancelled = true;
            //     await _MSG.ShowError("لطفا گزینه پست سازمانی را تکمیل نمایید.");
            // }

            return IsCancelled;
        }

        public async Task HR_ORG_Posts_afterrendermodal(Entity.HR_ORG_Posts Item)
        {
        }

        #endregion FunctionEvents
    }
}