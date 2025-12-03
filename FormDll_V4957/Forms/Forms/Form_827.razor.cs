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
    //public class Form_333Base : Form_333Peropeties // فرم تحویل کالا به درخواست دهنده در فرایند تحویل
    public class Form_827Base : Form_827Peropeties
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
        // var List = _Entity.SCMPETCO_ProductRequestDetails.ToString();
        // int ListCount = List.Count();
        // // DeliveryCode - کد تحویل بخش جزئیات درخواست
        // DeliveryCodeKharid = List.DeliveryCode;

        // if(ListCount==0)
        // {

        // if (Item.DeliveryCode == 0)
        // {


        //     <button class="btn btn-primary" disabled>
        //         //کد تحویل کالا: 
        //     </button>
        // }
        // else
        // {
        //     //<p>هیچ داده‌ای برای نمایش وجود ندارد.</p>
        // }
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
             var Finded = _Entity.SCMPETCO_ProductRequestDetails.FirstOrDefault();
             if  (Finded != null)
             {
                Console.WriteLine("Finded.DeliveryCode"+Finded.DeliveryCode);
                 await JS.InvokeVoidAsync("eval", $"document.getElementById('DeliveryCode1').innerHTML = '{Finded.DeliveryCode}'");
                //await JS.InvokeVoidAsync("eval", "document.getElementById('DeliveryCode1').innerHTML='+@Finded.DeliveryCode");
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

        // if (_Entity.ReqCount != 5)
        // {
        //     IsValid = false;
        //     SumaryMessage += "تعداد درخواست مخالف 5 باشد";
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

    public async Task <bool> GridSCMPETCO_ProductRequestId_editmodelsaving(object e   )
        {

            return false;
        }
        public async Task  GridSCMPETCO_ProductRequestId_afterrendermodal(Entity.SCMPETCO_ProductRequestDetails Item   )
        {

            
        }

		public async Task  DeliveryCodeKharid_onclick(MouseEventArgs Selected   )
        {

            
        }

		#endregion FunctionEvents

}
}
