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
    public class Form_946Base : Form_946Peropeties
    {
         // تابع پیام توــست
        public MSG _MSG { get; set; }


        /// <summary>
        /// آماده سازی فرم
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
    {
        await IsReInterview_init();
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

        if (_Entity.SupervisorApprove == null)
            {
                IsValid = false;
                toastService.ShowError("لطفا مشخص کنید مورد تایید است یا خیر");
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

    public async Task  IsReInterview_init()
        {
           
            
            if (_Entity.IsReInterview.HasValue)
            {
                if (_Entity.IsReInterview.Value)
                {
                    await JS.InvokeVoidAsync("AddClass", "#SequenceFlow_03kw4rf", "d-none");
                    await JS.InvokeVoidAsync("RemoveClass", "#SequenceFlow_0td9uia", "d-none");
                }else{
                    await JS.InvokeVoidAsync("RemoveClass", "#SequenceFlow_03kw4rf", "d-none");
                    await JS.InvokeVoidAsync("AddClass", "#SequenceFlow_0td9uia", "d-none");
                }
            }else
            {
                await JS.InvokeVoidAsync("RemoveClass", "#SequenceFlow_03kw4rf", "d-none");
                await JS.InvokeVoidAsync("AddClass", "#SequenceFlow_0td9uia", "d-none");
            }

             StateHasChanged();
        }

		public async Task  IsReInterview_oninput(ChangeEventArgs Selected   )
        {
                if (Convert.ToBoolean(Selected.Value))
                {
                    await JS.InvokeVoidAsync("AddClass", "#SequenceFlow_03kw4rf", "d-none");
                    await JS.InvokeVoidAsync("RemoveClass", "#SequenceFlow_0td9uia", "d-none");
                }else{
                    await JS.InvokeVoidAsync("RemoveClass", "#SequenceFlow_03kw4rf", "d-none");
                    await JS.InvokeVoidAsync("AddClass", "#SequenceFlow_0td9uia", "d-none");
                }
        }

		#endregion FunctionEvents

}
}
