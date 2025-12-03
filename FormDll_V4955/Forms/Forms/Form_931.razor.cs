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
    public class Form_931Base : Form_931Peropeties
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

        }
    }

    /// <summary>
    /// اعتبار سنجی فرم
    /// </summary>
    /// <returns></returns>
    public override async Task<bool> FormValidator()
    {
        bool IsValid = true;

       
        var List = _Entity.SCM_ProductRequestDetails.Where(p=>p.IsDelete == false).ToList();


            var listCount = List.Count();

            // Console.WriteLine("#Log FormValidator :");

            // دکمه ثبت و ادامه           
            if (BtnWorkFlowId == "SequenceFlow_04qthte")
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
        public async Task<bool> CheckFieldValidation(Entity.SCM_ProductRequestDetails Item)
        {
            bool IsValid = true;

             // توضیحات تایید استعلام مدیر عامل
            if(Item.ConfirmationOfTheInquiryCEO == null)
            {
				IsValid = false;
				toastService.ShowError("لطفا گزینه توضیحات تایید استعلام مدیر عامل را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
            }

            return IsValid;

        }


    public async Task <bool> GridSCM_ProductRequestId_182_editmodelsaving(object e   )
        {
         bool IsCancelled = false;

         var Item = (Entity.SCM_ProductRequestDetails)e;
        IsCancelled = !await CheckFieldValidation(Item);

         return IsCancelled;
        }
public async Task  GridSCM_ProductRequestId_182_afterrendermodal(Entity.SCM_ProductRequestDetails Item   )
        {
         // استعلام تدراکات 1- SCM_ProductRequestDetails_InquiryFirst
        // استعلام تدارکات 2- SCM_ProductRequestDetails_InquirySecondFile
        // استعلام تدارکات 3- SCM_ProductRequestDetails_InquiryThirdFile
        var Inq1IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryFirst;
        var Inq2IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquirySecondFile;
        var Inq3IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryThirdFile;
        // نمایش / عدم نمایش فیلد منتج از ITIL
    	if (Item.InquiryTrueFalse.HasValue && Item.InquiryTrueFalse.Value)
		{
    	    Inq1IsVisible.SetVisible(true);
    	    Inq2IsVisible.SetVisible(true);
    	    Inq3IsVisible.SetVisible(true);
		}
		else
		{
    	    Inq1IsVisible.SetVisible(false);
    	    Inq2IsVisible.SetVisible(false);
    	    Inq3IsVisible.SetVisible(false);
		}  
        }

		public async Task  InquiryTrueFalse_oninput(ChangeEventArgs Selected ,Entity.SCM_ProductRequestDetails Item  )
        {
        // استعلام تدراکات 1- SCM_ProductRequestDetails_InquiryFirst
        // استعلام تدارکات 2- SCM_ProductRequestDetails_InquirySecondFile
        // استعلام تدارکات 3- SCM_ProductRequestDetails_InquiryThirdFile
        var Inq1IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryFirst;
        var Inq2IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquirySecondFile;
        var Inq3IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryThirdFile;
        // نمایش / عدم نمایش فیلد منتج از ITIL
    	if (Selected.Value.ToString() == "true")
		{
    	    Inq1IsVisible.SetVisible(true);
    	    Inq2IsVisible.SetVisible(true);
    	    Inq3IsVisible.SetVisible(true);
		}
		else
		{
    	    Inq1IsVisible.SetVisible(false);
    	    Inq2IsVisible.SetVisible(false);
    	    Inq3IsVisible.SetVisible(false);
		}
        }
		#endregion FunctionEvents

}
}
