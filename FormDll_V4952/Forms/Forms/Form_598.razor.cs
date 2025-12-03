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
    public class Form_598Base : Form_598Peropeties
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

        foreach (var Item in List)
        {
            if(Item.InquiryIsEnable.HasValue && Item.InquiryIsEnable.Value)
            {
                // فیلد شماره استعلام مورد تایید (تاییدکننده واحد)
                if (Item.InqueryApproveNum == null)
			    {
			    	IsValid = false;

			    	toastService.ShowError("لطفا گزینه شماره استعلام مورد تایید (تاییدکننده واحد) را تکمیل نمایید.",
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
    public async Task ITILVisible(bool Visible)
    {
        Ref_SCM_OS_Details_ResultingFromITIL.SetVisible(Visible);
    }
	public async Task ITILDetailsVisible(bool Visible)
    {
        Ref_SCM_OS_Details_RequestIdITIL.SetVisible(Visible);
        Ref_SCM_OS_Details_RequestIdITIL.SetDisabled(true);

        Ref_SCM_OS_Details_RequesterUserITIL.SetVisible(Visible);
        Ref_SCM_OS_Details_RequesterUserITIL.SetDisabled(true);

        Ref_SCM_OS_Details_CreatedAtITIL.SetVisible(Visible);
        Ref_SCM_OS_Details_CreatedAtITIL.SetDisabled(true);

        Ref_SCM_OS_Details_ITILDetails.SetVisible(Visible);
    }

    public async Task <bool> SCM_OS_Details_editmodelsaving(object e   )
    {
        return false;
    }

    public async Task  SCM_OS_Details_afterrendermodal(Entity.SCM_OS_Details Item   )
    {
        // Show ITIL
        if (Item.ITILCodeIsEnable.HasValue && Item.ITILCodeIsEnable.Value)
		{
            await ITILVisible(true); 
		}
		else
		{
            await ITILVisible(false);
		}
        // Show ITIL Details
        if (!string.IsNullOrEmpty(Item.ResultingFromITIL))
		{
            await ITILDetailsVisible(true);
		}
		else
		{
            await ITILDetailsVisible(false);
		}
        // نمایش جزئیات ITIL
        if (Item.ResultingFromITIL != null)
		{
     		Ref_SCM_OS_Details_ITILDetails.SetEntity(Item);
			Ref_SCM_OS_Details_ITILDetails.LoadData();
 		}

        // فایل مدارک
        var UploadFilesIsVisible = Ref_SCM_OS_Details_SCM_OS_Details_UploadFile;
        if (Item.UploadFileIsEnable.HasValue && Item.UploadFileIsEnable.Value)
		{
			UploadFilesIsVisible.SetVisible(true); 
		}
		else
		{
			UploadFilesIsVisible.SetVisible(false); 
		}

        // // نمایش / عدم نمایش فیلد فایل مدارک
        // var NeedModifiedIsVisible = Ref_SCM_OS_Details_IsEnableNeedModified;
		// if(!(Item.IsEnableApproved.HasValue))
		// {
		//     NeedModifiedIsVisible.SetVisible(true);
		// }
		// else
		// {
		// 	NeedModifiedIsVisible.SetVisible(false); 
		// }

        // نمایش فایل های بارگذاری استعلام
        var InquiryFileIsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile1;
        var InquiryFile2IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile2;
        var InquiryFile3IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile3;
        var InqApprover1 = Ref_SCM_OS_Details_InqueryApproveNum;
        if(Item.InquiryIsEnable.HasValue && Item.InquiryIsEnable.Value)
        {
            InquiryFileIsVisible.SetVisible(true);
            InquiryFile2IsVisible.SetVisible(true);
            InquiryFile3IsVisible.SetVisible(true);
            InqApprover1.SetVisible(true);
        }
        else
        {
            InquiryFileIsVisible.SetVisible(false);
            InquiryFile2IsVisible.SetVisible(false);
            InquiryFile3IsVisible.SetVisible(false);
            InqApprover1.SetVisible(true);
        }
    }

    public async Task  InquiryIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCM_OS_Details Item  )
    {
        // نمایش فایل های بارگذاری استعلام
        var InquiryFileIsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile1;
        var InquiryFile2IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile2;
        var InquiryFile3IsVisible = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile3;
        var InqApprover1 = Ref_SCM_OS_Details_InqueryApproveNum;

        if (Selected.Value.ToString() == "true")
        {
            InquiryFileIsVisible.SetVisible(true);
            InquiryFile2IsVisible.SetVisible(true);
            InquiryFile3IsVisible.SetVisible(true);
            InqApprover1.SetVisible(true);
        }
        else
        {
            InquiryFileIsVisible.SetVisible(false);
            InquiryFile2IsVisible.SetVisible(false);
            InquiryFile3IsVisible.SetVisible(false);
            InqApprover1.SetVisible(true);
        }
    }

    public async Task  UploadFileIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCM_OS_Details Item  )
    {
        // نمایش / عدم نمایش فیلد فایل مدارک
        var UploadFilesIsVisible = Ref_SCM_OS_Details_SCM_OS_Details_UploadFile;
		if (Selected.Value.ToString() == "true")
		{
			UploadFilesIsVisible.SetVisible(true); 
		}
		else
		{
			UploadFilesIsVisible.SetVisible(false); 
		}
    }

    public async Task  ITILCodeIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCM_OS_Details Item  )
    {
        // نمایش / عدم نمایش فیلد منتج از ITIL
        if (Selected.Value.ToString() == "true")
	    {
            await ITILVisible(true);
	    }
	    else
	    {
            await ITILVisible(false); 
	    } 
    }

    public async Task  ResultingFromITIL_onitemselected(dynamic Selected ,Entity.SCM_OS_Details Item  )
    {
        // نمایش / عدم نمایش فیلد ITIL Detail
        await ITILDetailsVisible(true);

        if (Item.ResultingFromITIL != null)
	    {
	    	Item.RequestIdITIL = Selected.RequestID;
	    	Item.RequesterUserITIL = Selected.UserName;
	    	Item.CreatedAtITIL = Selected.CreateDate;

			Ref_SCM_OS_Details_ITILDetails.SetEntity(Item);
			Ref_SCM_OS_Details_ITILDetails.LoadData();
	    }
    }

	#endregion FunctionEvents

}
}
