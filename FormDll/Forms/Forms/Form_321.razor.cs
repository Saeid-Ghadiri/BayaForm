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
    public class Form_321Base : Form_321Peropeties
    {
      
    int CurrentYear=Utility.PublicFunctions.GetShamsiYear();


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
             if(!_Entity.LeaveType.HasValue){
                 _Entity.LeaveType =true;
                 _Entity.HR_PermissionGroupId=new Guid("29f76faa-f24b-ef11-8351-005056a02a64");
             }
             else{
                 if(_Entity.LeaveType.Value == false){
                     Ref_StartTime.SetVisible(false);
                     Ref_EndTime.SetVisible(false);
                 }
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

         if (string.IsNullOrEmpty(Ref_StartDate.Value) || string.IsNullOrEmpty(Ref_EndDate.Value))
         {
             IsValid = false;
             toastService.ShowError("تاریخ شروع و تاریخ پایان را وارد کنید");
         }
         else
         {
          
             string[] StartDateVal = Ref_StartDate.Value.Split('/');
             string[] EndDateVal = Ref_EndDate.Value.Split('/');
             if ((CurrentYear.ToString() != StartDateVal[0]) || (CurrentYear.ToString() != EndDateVal[0]))
             {
                 IsValid = false;
                 toastService.ShowError("تاریخ شروع و پایان باید در سال جاری باشد");
             }

             DateTime sd = Utility.PublicFunctions.ShamsiToMiladi(Ref_StartDate.Value);
             DateTime Ed = Utility.PublicFunctions.ShamsiToMiladi(Ref_EndDate.Value);
             if (sd > Ed)
             {
                 IsValid = false;
                 toastService.ShowError("تاریخ پایان باید بعد از تاریخ شروع باشد");
             }

             if (Ref_LeaveType.Value == true && Ref_StartTime.Value.HasValue && Ref_EndTime.Value.HasValue)
             {
                 DateTime St = sd.Add(Ref_StartTime.Value.Value.ToTimeSpan());
                 DateTime Et = Ed.Add(Ref_EndTime.Value.Value.ToTimeSpan());
                 if (St >= Et)
                 {
                     IsValid = false;
                     toastService.ShowError("ساعت پایان باید بعد از ساعت شروع باشد");
                 }
                 else{
                     if (Et.Subtract(St).TotalMinutes > 480)
                     {
                         IsValid = false;
                         toastService.ShowError("برای مرخصی بیشتر از 8 ساعت مرخصی روزانه ثبت کنید");
                     }  
                 }
              
             }
         }

         if (Ref_LeaveType.Value == true && (Ref_StartTime.Value == null || Ref_EndTime.Value == null))
         {
             IsValid = false;
             toastService.ShowError("ساعت شروع و ساعت پایان را وارد کنید");
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

             SumaryMessage = "درخواست مرخصی با موفقیت ثبت شد";
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

		public async Task LeaveType_oninput(ChangeEventArgs Selected   )
        {
            
             if(string.Equals(Selected.Value.ToString(),"true")){
                 _Entity.HR_PermissionGroupId=new Guid("29f76faa-f24b-ef11-8351-005056a02a64");
                 Ref_StartTime.SetVisible(true);
                 Ref_EndTime.SetVisible(true);
             }
             else{
                 _Entity.HR_PermissionGroupId=new Guid("28f76faa-f24b-ef11-8351-005056a02a64");
                 Ref_StartTime.SetVisible(false);
                 Ref_EndTime.SetVisible(false);
             }
        }

	

		#endregion FunctionEvents

}
}
