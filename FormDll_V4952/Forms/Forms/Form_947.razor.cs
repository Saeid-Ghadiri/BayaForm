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
    public class Form_947Base : Form_947Peropeties
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
            await JS.InvokeVoidAsync("AddClass", ".form-group", "d-none");
            if (_Entity.IsReInterview.HasValue)
            {
                if (_Entity.IsReInterview.Value)
                {
                    await JS.InvokeVoidAsync("RemoveClass", "#mosahebeMojaddad", "d-none");
                    await JS.InvokeVoidAsync("RemoveClass", "#ReInterviewDateTime", "d-none");
                    await JS.InvokeVoidAsync("RemoveClass", "#IsReInterview", "d-none");
                }
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

            if (_Entity.JobInterviewer1 == null)
            {
                IsValid = false;
                toastService.ShowError("لطفا کاربر مصاحبه کننده را وارد کنید");
            }
             if (_Entity.JobInterviewer2 == null)
            {
                IsValid = false;
                toastService.ShowError("لطفا کاربر سرپرست را وارد کنید");
            }

        return IsValid;
    }


    /// <summary>
    /// تابع قبل اجرا شدن ارسال داده
    /// </summary>
    /// <returns></returns>
    public override async Task<Result> BeforSubmit()
    {
        if (!await FormValidator())
        {
            StateHasChanged();
            return new Result() { Status = HttpStatusCode.InternalServerError };
        }
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

    #endregion FunctionEvents

}
}
