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
    public class Form_578Base : Form_578Peropeties
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

        var List = _Entity.SCM_OS_Details.ToList();
        
        int ListCount = List.Count();

        if(ListCount==0)
        {
            IsValid = false;
            
            var options = new ConfirmDialogOptions
			{
				YesButtonText = "بازگشت به درخواست",
				YesButtonColor = ButtonColor.Danger,
				NoButtonText = "",
			};

			string htmlString = 
            "<div>"+
                "<picture>"+
                    "<img src='https://file.workcv.ir/fa/api/v1/File/Get?FileID=6e5b6fb8-a5b2-490c-f83f-08dbea5b8061' class='' alt='لوگو پل فیلم' width='96px;'>"+
                "</picture>"+
                "<hr class='hrdash border-warning-subtle'>"+
            "</div>"+
            "<div class='fw-bold text-center'>" + 
            "<span class='fs-5'>کد پیگیری این درخواست: </span>" + 
			"<span class='fs-3' style='color: #1ba156'>" + _Entity.RequestTrakingCode + "</span><div>"+
            "<span><i class='fal fa-exclamation-triangle' style='font-size:24px; color:red;'></i>&nbsp;</span>" +
			"<span class='fs-6 text-secondary text-right'>تا کنون هیچ ردیف درخواستی تکمیل نشده است. لطفا برای ثبت و ادامه به مرحله بعد حداقل یک ردیف در درخواست خود ثبت نمایید.<span></div>" +
            "</div>"; 

			var confirmation = await Confirm.ShowAsync(
				title: "",
				message1: htmlString,
				confirmDialogOptions: options);
        }

            //Console.WriteLine("#Log FormValidator :");

            // دکمه ثبت و ادامه
            if (BtnWorkFlowId == "SequenceFlow_0y2g3d1")
            {
                //Console.WriteLine("#Log FormValidator btn :");
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
    public async Task<bool> CheckFieldValidation(Entity.SCM_OS_Details Item)
    {
        bool IsValid = true;

        // عنوان کار
        if (Item.Title == null)
		{
			IsValid = false;

			toastService.ShowError("لطفا گزینه عنوان کار را تکمیل نمایید.",
			settings => {
				settings.Timeout = 4;
				settings.ShowProgressBar = true;
				settings.PauseProgressOnHover = true;
			});
		}

        // SCM_UnitsId
        if (Item.SCM_UnitsId == null)
		{
			IsValid = false;

			toastService.ShowError("لطفا گزینه واحد کالا را تکمیل نمایید.",
			settings => {
				settings.Timeout = 4;
				settings.ShowProgressBar = true;
				settings.PauseProgressOnHover = true;
			});
		}

        // محل مصرف
        if (Item.AreaUse == null)
		{
			IsValid = false;            

			toastService.ShowError("لطفا گزینه محل مصرف را تکمیل نمایید.",
			settings => {
				settings.Timeout = 4;
				settings.ShowProgressBar = true;
				settings.PauseProgressOnHover = true;
			});
		}
        
		// Amount
		if (Item.Amount == null || Item.Amount == 0)
		{
			IsValid = false;

			toastService.ShowError("لطفا گزینه تعداد را تکمیل نمایید",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
		}

        // فیلد اولویت
        if (Item.SCM_PriorityId == null)
		{
			IsValid = false;
            
			toastService.ShowError("لطفا گزینه اولویت را تکمیل نمایید.",
			settings => {
				settings.Timeout = 4;
				settings.ShowProgressBar = true;
				settings.PauseProgressOnHover = true;
			});
		}

        return IsValid;
    }


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

	public async Task<bool> SCM_OS_Details_editmodelsaving(object e   )
    {
        bool IsCancelled = false;

		var Item = (Entity.SCM_OS_Details)e;
        
        IsCancelled = !await CheckFieldValidation(Item);

        return IsCancelled;     
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
