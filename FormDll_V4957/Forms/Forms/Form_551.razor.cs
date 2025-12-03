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
    public class Form_551Base : Form_551Peropeties
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

        // InqueryApproveNum
        var List = _Entity.SCM_OS_Details.ToList();
        
        int ListCount = List.Count();

        foreach (var Item in List)
        {
            // شماره استعلام مورد تایید
            if (String.IsNullOrEmpty(Item.InqueryApproveNum))
			{
				IsValid = false;

				toastService.ShowError("لطفا گزینه شماره استعلام مورد تایید را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
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

    public async Task <bool> SCM_OS_Details_editmodelsaving(object e   )
    {
        bool IsCancelled = false;

		var ModelDetail = (Entity.SCM_OS_Details)e;

        // شماره استعلام مورد تایید
        if (String.IsNullOrEmpty(ModelDetail.InqueryApproveNum))
		{
			IsCancelled = false;

			toastService.ShowError("لطفا گزینه شماره استعلام مورد تایید را تکمیل نمایید.",
			settings => {
				settings.Timeout = 4;
				settings.ShowProgressBar = true;
				settings.PauseProgressOnHover = true;
			});
		}

        return IsCancelled;
    }
    
    public async Task  SCM_OS_Details_afterrendermodal(Entity.SCM_OS_Details Item   )
    {
        // نمایش / عدم نمایش فیلد فایل استعلام
        var UF1IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile1;
        var UF2IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile2;
        var UF3IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile3;
		if (Item.InquiryIsEnable.HasValue && Item.InquiryIsEnable.Value)
		{
		    UF1IsVisible.SetVisible(true); 
		    UF2IsVisible.SetVisible(true); 
		    UF3IsVisible.SetVisible(true); 
		}
		else
		{
			UF1IsVisible.SetVisible(false); 
			UF2IsVisible.SetVisible(false); 
			UF3IsVisible.SetVisible(false); 
		}

        // نمایش / عدم نمایش فیلد فایل مدارک
        var UFIsVisible = Ref_SCM_OS_Details_SCM_OS_Details_UploadFile;
		if (Item.UploadFileIsEnable.HasValue && Item.UploadFileIsEnable.Value)
		{
		    UFIsVisible.SetVisible(true);
		}
		else
		{
			UFIsVisible.SetVisible(false);
		}
    }
    
    public async Task InquiryIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCM_OS_Details Item  )
    {
        // نمایش / عدم نمایش فیلد فایل استعلام
        var UF1IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile1;
        var UF2IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile2;
        var UF3IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile3;
		if (Selected.Value.ToString() == "true")
		{
		    UF1IsVisible.SetVisible(true); 
		    UF2IsVisible.SetVisible(true); 
		    UF3IsVisible.SetVisible(true); 
		}
		else
		{
			UF1IsVisible.SetVisible(false); 
			UF2IsVisible.SetVisible(false); 
			UF3IsVisible.SetVisible(false); 
		}
    }
    
    public async Task  UploadFileIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCM_OS_Details Item  )
    {
        // نمایش / عدم نمایش فیلد فایل مدارک
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