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
    public class Form_542Base : Form_542Peropeties
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

    public async Task <bool> SCM_OS_Details_editmodelsaving(object e   )
        {

             bool IsValid = false;
	        var MainModel = (Entity.SCM_OS_Details)e;

            // TO_ConfirmedInquiryNum
            if (MainModel.TO_ConfirmedInquiryNum == null)
	        {
	        	IsValid = true;            
	        	toastService.ShowError("لطفا گزینه استعلام تایید شده واحد و دفتر فنی را تکمیل نمایید.",
	        	settings => {
	        		settings.Timeout = 4;
	        		settings.ShowProgressBar = true;
	        		settings.PauseProgressOnHover = true;
	        	});
	        }

            return IsValid;
        }
public async Task  SCM_OS_Details_afterrendermodal(Entity.SCM_OS_Details Item   )
        {
             if(_Entity.SCM_ResultingFromId == null)
            {
                Ref_SCM_OS_Details_DesignMakeNum.SetVisible(false);
            }
            else
            {
                // تعمیرات
                if(_Entity.SCM_ResultingFromId.ToString() =="6b3840b6-6a26-ef11-8351-005056a02a64")
                {
                    Ref_SCM_OS_Details_DesignMakeNum.SetVisible(false);
                }
                // طراحی و ساخت
                if(_Entity.SCM_ResultingFromId.ToString() =="6c3840b6-6a26-ef11-8351-005056a02a64")
                {
                    Ref_SCM_OS_Details_DesignMakeNum.SetVisible(true);
                }
                // اجرایی
                if(_Entity.SCM_ResultingFromId.ToString() =="07503dc3-6a26-ef11-8351-005056a02a64")
                {
                    Ref_SCM_OS_Details_DesignMakeNum.SetVisible(false);
                }
            }

            // آیا مدارک دارد یا خیر؟
            if (Item.UploadFileIsEnable.HasValue && Item.UploadFileIsEnable.Value)
		    {
		    	Ref_SCM_OS_Details_SCM_OS_Details_UploadFile.SetVisible(true);
		    }
		    else
		    {
		    	Ref_SCM_OS_Details_SCM_OS_Details_UploadFile.SetVisible(false);
		    }

            // آیا نیاز به استعلام دارد؟
            if (Item.InquiryIsEnable.HasValue && Item.InquiryIsEnable.Value)
		    {
                // فایل های استعلام
		    	Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile1.SetVisible(true);
                Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile2.SetVisible(true);
                Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile3.SetVisible(true);
                // // کدام استعلام مورد تایید دفتر فنی است
                // Ref_SCM_OS_Details_TO_ConfirmedInquiryNum.SetVisible(true);
                // // تایید استعلام مدیر فنی
                // Ref_SCM_OS_Details_TM_ConfirmedInquiryNum.SetVisible(true);
		    }
		    else
		    {
                // فایل های استعلام
		    	Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile1.SetVisible(false);
                Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile2.SetVisible(false);
                Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile3.SetVisible(false);
                // // کدام استعلام مورد تایید دفتر فنی است
                // Ref_SCM_OS_Details_TO_ConfirmedInquiryNum.SetVisible(false);
                // // تایید استعلام مدیر فنی
                // Ref_SCM_OS_Details_TM_ConfirmedInquiryNum.SetVisible(false);
		    }    
        }

		public async Task  SCM_ResultingFromId_onitemselected(Entity.SCM_ResultingFrom Selected   )
        {
        // تعمیرات
        if(_Entity.SCM_ResultingFromId.ToString() =="6b3840b6-6a26-ef11-8351-005056a02a64")
        {
            Ref_SCM_OS_Details_DesignMakeNum.SetVisible(false);
        }
        // طراحی و ساخت
        if(_Entity.SCM_ResultingFromId.ToString() =="6c3840b6-6a26-ef11-8351-005056a02a64")
        {
            Ref_SCM_OS_Details_DesignMakeNum.SetVisible(true);
        }
        // اجرایی
        if(_Entity.SCM_ResultingFromId.ToString() =="07503dc3-6a26-ef11-8351-005056a02a64")
        {
            Ref_SCM_OS_Details_DesignMakeNum.SetVisible(true);
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
public async Task  InquiryIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCM_OS_Details Item  )
        {
             // نمایش 3 فایل بارگذاری استعلام
        var InquiryFile1 = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile1;
        var InquiryFile2 = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile2;
        var InquiryFile3 = Ref_SCM_OS_Details_SCM_OS_Details_InquiryFile3;
        // // کدام استعلام مورد تایید دفتر فنی است
        // var TO_ConfirmedInquiryNum = Ref_SCM_OS_Details_TO_ConfirmedInquiryNum;
        // // تایید استعلام مدیر فنی
        // var TM_ConfirmedInquiryNum = Ref_SCM_OS_Details_TM_ConfirmedInquiryNum;

	    if (Selected.Value.ToString() == "true")
	    {
	    	InquiryFile1.SetVisible(true);
            InquiryFile2.SetVisible(true);
            InquiryFile3.SetVisible(true);
            // TO_ConfirmedInquiryNum.SetVisible(true);
            // TM_ConfirmedInquiryNum.SetVisible(true);
	    }
	    else
	    {
	    	InquiryFile1.SetVisible(false);
            InquiryFile2.SetVisible(false);
            InquiryFile3.SetVisible(false);
            // TO_ConfirmedInquiryNum.SetVisible(false);
            // TM_ConfirmedInquiryNum.SetVisible(false);
	    }    
        }

		#endregion FunctionEvents

}
}
