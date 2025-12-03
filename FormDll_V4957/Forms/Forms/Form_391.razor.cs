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
    public class Form_391Base : Form_391Peropeties
    {

        // Id					                    منتج از برون سپاری
        // **********************************************************
        // 6b3840b6-6a26-ef11-8351-005056a02a64     تعمیرات
        // 6c3840b6-6a26-ef11-8351-005056a02a64     طراحی و ساخت
        // 07503dc3-6a26-ef11-8351-005056a02a64     خدمات احداثی و اجرایی

    


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

    public async Task  SCM_OS_Details_afterrendermodal(Entity.SCM_OS_Details Item   )
        {
            // استعلام های دفتر فنی
        var Inq1IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile1;
        var Inq2IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile2;
        var Inq3IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile3;
        // استعلام های تدارکات
        var Inq4IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile4;
        var Inq5IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile5;
        var Inq6IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile6;

        //  تایید استعلام ها
        var Inq7IsVisible = Ref_SCM_OS_Details_CEO_ConfirmedInquiryNum;
        var Inq8IsVisible = Ref_SCM_OS_Details_InqueryApproveNum;
        var Inq9IsVisible = Ref_SCM_OS_Details_TM_ConfirmedInquiryNum;
 

        // 
        if (Item.InquiryIsEnable.HasValue && Item.InquiryIsEnable.Value)
        {
            Inq1IsVisible.SetVisible(true); 
            Inq2IsVisible.SetVisible(true);
            Inq3IsVisible.SetVisible(true);
            Inq4IsVisible.SetVisible(true);
            Inq5IsVisible.SetVisible(true);
            Inq6IsVisible.SetVisible(true);

            Inq7IsVisible.SetVisible(true);
            Inq8IsVisible.SetVisible(true);
            Inq9IsVisible.SetVisible(true);

        }
        else
        {
            Inq1IsVisible.SetVisible(false); 
            Inq2IsVisible.SetVisible(false);
            Inq3IsVisible.SetVisible(false);
            Inq4IsVisible.SetVisible(false);
            Inq5IsVisible.SetVisible(false);
            Inq6IsVisible.SetVisible(false);

            Inq7IsVisible.SetVisible(false);
            Inq8IsVisible.SetVisible(false);
            Inq9IsVisible.SetVisible(false);

        }

        // فایل مدارک
        var UFIsVisible = Ref_SCM_OS_Details_SCM_OS_Details_UploadFile;
        if (Item.UploadFileIsEnable.HasValue && Item.UploadFileIsEnable.Value)
        {
            UFIsVisible.SetVisible(true); 
        }
        else
        {
            UFIsVisible.SetVisible(false);
        }

        // // در صورتی که منتج از برون سپاری = طراحی و ساخت باشد شماره طراحی و ساخت نمایش داده شود.
        // if(_Entity.SCM_ResultingFromId.ToString() == "6c3840b6-6a26-ef11-8351-005056a02a64")
        // {
        //     Ref_SCM_OS_Details_DesignMakeNum.SetVisible(true);
        // }
        // else
        // {
        //     Ref_SCM_OS_Details_DesignMakeNum.SetVisible(false);
        // }
        }

		public async Task  InquiryIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCM_OS_Details Item  )
        {
            // استعلام های دفتر فنی
        var Inq1IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile1;
        var Inq2IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile2;
        var Inq3IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile3;
        // استعلام های تدارکات
        var Inq4IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile4;
        var Inq5IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile5;
        var Inq6IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile6;

           //  تایید استعلام ها
        var Inq7IsVisible = Ref_SCM_OS_Details_CEO_ConfirmedInquiryNum;
        var Inq8IsVisible = Ref_SCM_OS_Details_InqueryApproveNum;
        var Inq9IsVisible = Ref_SCM_OS_Details_TM_ConfirmedInquiryNum;

        // 
        if (Selected.Value.ToString() == "true")
        {
            Inq1IsVisible.SetVisible(true); 
            Inq2IsVisible.SetVisible(true);
            Inq3IsVisible.SetVisible(true);
            Inq4IsVisible.SetVisible(true);
            Inq5IsVisible.SetVisible(true);
            Inq6IsVisible.SetVisible(true);

            Inq7IsVisible.SetVisible(true);
            Inq8IsVisible.SetVisible(true);
            Inq9IsVisible.SetVisible(true);

        }
        else
        {
            Inq1IsVisible.SetVisible(false); 
            Inq2IsVisible.SetVisible(false);
            Inq3IsVisible.SetVisible(false);
            Inq4IsVisible.SetVisible(false);
            Inq5IsVisible.SetVisible(false);
            Inq6IsVisible.SetVisible(false);

            Inq7IsVisible.SetVisible(false);
            Inq8IsVisible.SetVisible(false);
            Inq9IsVisible.SetVisible(false);
        } 
        }

		public async Task  UploadFileIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCM_OS_Details Item  )
        {
         var UFIsVisible = Ref_SCM_OS_Details_SCM_OS_Details_UploadFile;

        if (Selected.Value.ToString() == "true")
        {
            UFIsVisible.SetVisible(true); 
        }
        else
        {
            UFIsVisible.SetVisible(false);
        } 
        }

		#endregion FunctionEvents

}
}
