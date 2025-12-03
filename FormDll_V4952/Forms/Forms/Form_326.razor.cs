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
    public class Form_326Base : Form_326Peropeties
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

        var List = _Entity.SCM_ProductRequestDetails.ToList();

        foreach(var Item in List)
        {
            // آیا کالای تحویلی مورد تایید است؟
            if(Item.DeliveredProductApproved == null)
            {
	        	IsValid = true;
	        	toastService.ShowError("لطفا گزینه آیا کالای تحویلی مورد تایید است؟ را تکمیل نمایید.",
	        	settings => {
	        		settings.Timeout = 4;
	        		settings.ShowProgressBar = true;
	        		settings.PauseProgressOnHover = true;
	        	});
            }

            // نیاز به اصلاح دارد یا خیر؟
            if(!(Item.DeliveredProductApproved.HasValue && Item.DeliveredProductApproved.Value))
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

    public async Task <bool> SCM_ProductRequestDetails_editmodelsaving(object e   )
        {
            bool IsCancelled = false;
        var ModelDetail = (Entity.SCM_ProductRequestDetails)e;
        
        // آیا کالای تحویلی مورد تایید است؟
        if(ModelDetail.DeliveredProductApproved == null)
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
        if(!(ModelDetail.DeliveredProductApproved.HasValue && ModelDetail.DeliveredProductApproved.Value))
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
public async Task  SCM_ProductRequestDetails_afterrendermodal(Entity.SCM_ProductRequestDetails Item   )
        {
         // وضعیت نمایش فیلد نیاز به اصلاح دارد بر اساس آیا مورد تایید است یا خیر
        if (Item.DeliveredProductApproved.HasValue && Item.DeliveredProductApproved.Value)
		{
			Ref_SCM_ProductRequestDetails_IsEnableNeedModified.SetVisible(true);
		}
		else
		{
			Ref_SCM_ProductRequestDetails_IsEnableNeedModified.SetVisible(false);
		}
        }
public async Task  DeliveredProductApproved_oninput(ChangeEventArgs Selected ,Entity.SCM_ProductRequestDetails Item  )
        {
            // وضعیت نمایش فیلد نیاز به اصلاح دارد بر اساس آیا مورد تایید است یا خیر
        var Item2 = Ref_SCM_ProductRequestDetails_IsEnableNeedModified;

		if (Selected.Value.ToString() == "false")
		{
			Item2.SetVisible(true);
		}
		else
		{
			Item2.SetVisible(false);
		}
        }

		#endregion FunctionEvents

}
}
