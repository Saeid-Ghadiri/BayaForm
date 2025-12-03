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
    public class Form_378Base : Form_378Peropeties
    {
      
        // Toast  
		[Inject]
	    public IToastService toastService { get; set; }


        // Id					                    ProcurementExpert
        // **********************************************************
        // ae92a51e-f182-ee11-8320-005056a02a64 	جواد جعفرزاده
        // af92a51e-f182-ee11-8320-005056a02a64	    علی عباسی
        // b092a51e-f182-ee11-8320-005056a02a64	    مهدی عباسی
        // a473ea8f-2061-ef11-8351-005056a02a64	    دنیا پور رضایی
        // 55c03b4f-ebcb-ef11-a4fa-005056a2b6bd     علی زمانی

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

        foreach (var Item in List)
        {
            // فیلد اولویت
            if (Item.SCM_PriorityId == null || Item.SCM_PriorityId == Guid.Empty)
			{
				IsValid = false;
                
				toastService.ShowError("لطفا گزینه اولویت را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}

             // فیلد واحد
            if (Item.SCM_UnitsId == null || Item.SCM_UnitsId == Guid.Empty)
			{
				IsValid = false;
                
				toastService.ShowError("لطفا گزینه واحد را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}
        }

            // فیلد منتج از برون سپاری
            if (_Entity.SCM_ResultingFromId == null)
			{
				IsValid = false;
                
				toastService.ShowError("لطفا یک نوع منتج از درخواست برون سپاری یا خرید خدمات را انتخاب نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
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

    public async Task SCM_OS_Details_afterrendermodal(Entity.SCM_OS_Details Item   )
    {    
    }

	public async Task<bool> SCM_OS_Details_editmodelsaving(object e   )
    {
        bool IsCancelled = false;

		var Item = (Entity.SCM_OS_Details)e;
        
        // SCM_OS_JobTitleId
        if (Item.SCM_OS_JobTitleId == null)
		{
			IsCancelled = true;            
			toastService.ShowError("لطفا گزینه عنوان کار را تکمیل نمایید.",
			settings => {
				settings.Timeout = 4;
				settings.ShowProgressBar = true;
				settings.PauseProgressOnHover = true;
			});
		}

        // SCM_UnitsId
        if (Item.SCM_UnitsId == null || Item.SCM_UnitsId == Guid.Empty)
		{
			IsCancelled = true;            
			toastService.ShowError("لطفا گزینه واحد کالا را تکمیل نمایید.",
			settings => {
				settings.Timeout = 4;
				settings.ShowProgressBar = true;
				settings.PauseProgressOnHover = true;
			});
		}

        // SCM_OS_PlaceofUseId
        if (Item.SCM_OS_PlaceofUseId == null)
		{
			IsCancelled = true;            
			toastService.ShowError("لطفا گزینه محل مصرف را تکمیل نمایید.",
			settings => {
				settings.Timeout = 4;
				settings.ShowProgressBar = true;
				settings.PauseProgressOnHover = true;
			});
		}
         // SCM_AreaOperationId
        if (Item.SCM_AreaOperationId == null)
		{
			IsCancelled = true;            
			toastService.ShowError("لطفا گزینه محل عملیات را تکمیل نمایید.",
			settings => {
				settings.Timeout = 4;
				settings.ShowProgressBar = true;
				settings.PauseProgressOnHover = true;
			});
		}
        
		// Amount
		if (Item.Amount == null || Item.Amount == 0)
		{
			IsCancelled = true;
			toastService.ShowError("لطفا گزینه تعداد را تکمیل نمایید",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
		}

        // فیلد اولویت
        if (Item.SCM_PriorityId == null || Item.SCM_PriorityId == Guid.Empty)
		{
			IsCancelled = true;            
			toastService.ShowError("لطفا گزینه اولویت را تکمیل نمایید.",
			settings => {
				settings.Timeout = 4;
				settings.ShowProgressBar = true;
				settings.PauseProgressOnHover = true;
			});
		}

        return IsCancelled;     
    }

	#endregion FunctionEvents

}
}
