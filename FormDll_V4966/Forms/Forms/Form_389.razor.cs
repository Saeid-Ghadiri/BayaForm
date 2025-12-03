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
    public class Form_389Base : Form_389Peropeties
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

        var List = _Entity.SCM_OS_Details.ToList();
        
        int ListCount = List.Count();

        foreach(var Item in List)
        {
            // آیا کالای تحویلی مورد تایید است؟
            if(Item.IsEnableApproved == null)
            {
	        	IsValid = true;
	        	toastService.ShowError("لطفا گزینه آیا کالای تحویلی مورد تایید است؟ را تکمیل نمایید.",
	        	settings => {
	        		settings.Timeout = 4;
	        		settings.ShowProgressBar = true;
	        		settings.PauseProgressOnHover = true;
	        	});
            }
            
            // Console.WriteLine("#log2");
            // Console.WriteLine("#log2-0: " + Item.IsEnableApproved);
            // نیاز به اصلاح دارد یا خیر؟
            if(!(Item.IsEnableApproved.HasValue && Item.IsEnableApproved.Value))
            {
                if(Item.IsEnableNeedModified == null)
                {
	            	IsValid = true;
	            	toastService.ShowError("لطفا گزینه آیا نیاز به اصلاح دارد؟ را تکمیل نمایید.",
	            	settings => {
	            		settings.Timeout = 4;
	            		settings.ShowProgressBar = true;
	            		settings.PauseProgressOnHover = true;
	            	});
                }
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
        
        // آیا کالای تحویلی مورد تایید است؟
        if(ModelDetail.IsEnableApproved == null)
        {
	    	IsCancelled = true;

	    	toastService.ShowError("لطفا گزینه آیا کالای تحویلی مورد تایید است؟ را تکمیل نمایید.",
	    	settings => {
	    		settings.Timeout = 4;
	    		settings.ShowProgressBar = true;
	    		settings.PauseProgressOnHover = true;
	    	});
        }

        // نیاز به اصلاح دارد یا خیر؟
        if(!(ModelDetail.IsEnableApproved.HasValue && ModelDetail.IsEnableApproved.Value))
        {
            if(ModelDetail.IsEnableNeedModified == null)
            {
	        	IsCancelled = true;

	        	toastService.ShowError("لطفا گزینه آیا نیاز به اصلاح دارد؟ را تکمیل نمایید.",
	        	settings => {
	        		settings.Timeout = 4;
	        		settings.ShowProgressBar = true;
	        		settings.PauseProgressOnHover = true;
	        	});
            }
        }

        return IsCancelled;

        }

public async Task  SCM_OS_Details_afterrendermodal(Entity.SCM_OS_Details Item   )
        {
         // نمایش / عدم نمایش فیلد فایل مدارک
        var NeedModifiedIsVisible = Ref_SCM_OS_Details_IsEnableNeedModified;
		if(!(Item.IsEnableApproved.HasValue))
		{
		    NeedModifiedIsVisible.SetVisible(true);
		}
		else
		{
			NeedModifiedIsVisible.SetVisible(false); 
		}
        }

		public async Task  IsEnableApproved_oninput(ChangeEventArgs Selected ,Entity.SCM_OS_Details Item  )
        {
            // نمایش / عدم نمایش فیلد فایل مدارک
        var NeedModifiedIsVisible = Ref_SCM_OS_Details_IsEnableNeedModified;
		if (Selected.Value.ToString() == "false")
		{
		    NeedModifiedIsVisible.SetVisible(true);
		}
		else
		{
			NeedModifiedIsVisible.SetVisible(false); 
		}
        }

		#endregion FunctionEvents

}
}
