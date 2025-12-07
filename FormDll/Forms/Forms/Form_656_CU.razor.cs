using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using Baya.Models.Utility.Menu;

namespace Forms.Forms
{
    public class Form_656_CUBase : Form_656_CUPeropeties
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
            IsValid = await CheckFieldValidation(_Entity);

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

        // تابع پیام تٌــست
        public MSG _MSG { get; set; }

        public async Task<bool> CheckFieldValidation(Entity.HR_ORG_Sections Item)
        {
            bool IsValid = true;

            // **************************************************

            // فیلد سایت
            if (Item.BaseInfo_ORG_CompaniesId == null)
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه شرکت مورد نظر را تکمیل نمایید.");
            }

            // کد قسمت
            if (Item.Code == null)
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه کد قسمت را تکمیل نمایید.");
            }

            // فیلد عنوان قسمت
            if (Item.Title == null)
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه عنوان قسمت را تکمیل نمایید.");
            }

            // // فیلد نوع قسمت
            // if (Item.HR_ORG_SectionTypesId == null)
            // {
            //     IsValid = false;
            //     await _MSG.ShowError("لطفا گزینه نوع قسمت را تکمیل نمایید.");
            // }

            // // فیلد وضعیت قسمت
            // if (Item.HR_ORG_SectionStatusId == null)
            // {
            //     IsValid = false;
            //     await _MSG.ShowError("لطفا گزینه وضعیت قسمت را تکمیل نمایید.");
            // }

            //// فیلد دسته مالیاتی
            //if (Item.HR_Base_TaxCategoryId == null)
            //{
            //    IsValid = false;
            //    await _MSG.ShowError("لطفا گزینه دسته مالیاتی را تکمیل نمایید.");
            //}

            // **************************************************

            return IsValid;
        }


        #endregion FunctionEvents

    }
}