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
    public class Form_552Base : Form_552Peropeties
    {
      
        // Toast  
		[Inject]
	    public IToastService toastService { get; set; }


        // DateOfDeliveryToWarehouse - تاریخ تحویل کالا به انبار توسط تدارکات
        public async Task DT_Tadarokat_Anbardar()
		{
			var List = _Entity.SCM_OS_Details.ToList();
			
			for (int i = 0; i < List.Count; i++)
			{
				var item = List[i];

				// تبدیل تاریخ شمسی به میلادی            
				System.Globalization.PersianCalendar PC = new System.Globalization.PersianCalendar();

				var DateNow = DateTime.Now;
				// تاریخ شمسی پر می شود
				item.DateOfDeliveryToWarehouse = PC.GetYear(DateNow) + "/" + PC.GetMonth(DateNow).ToString("0#") + "/" + PC.GetDayOfMonth(DateNow).ToString("0#");

			}
		}


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

        foreach (var Item in List)
        {
              // فایل فاکتور
            if (Item.SCM_OS_Details_InvoiceFile == null || Item.SCM_OS_Details_InvoiceFile.Count == 0)
			{
				IsValid = false;

				toastService.ShowError("لطفا گزینه فایل فاکتور را بارگذاری نمایید.",
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
        // تاریخ تحویل کار به انبار توسط تدارکات
        await DT_Tadarokat_Anbardar();

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
	    var Item = (Entity.SCM_OS_Details)e;

        // فایل فاکتور
        if (Item.SCM_OS_Details_InvoiceFile == null || Item.SCM_OS_Details_InvoiceFile.Count == 0)
		{
			IsCancelled = true;
			toastService.ShowError("لطفا گزینه فایل فاکتور را بارگذاری  نمایید.",
			settings => {
				settings.Timeout = 4;
				settings.ShowProgressBar = true;
				settings.PauseProgressOnHover = true;
			});
		}
        
        return IsCancelled;
        }

        public async Task SCM_OS_Details_afterrendermodal(Entity.SCM_OS_Details Item)
        {

            //// نمایش / عدم نمایش فیلد فایل مدارک
            //var UFIsVisible = Ref_SCM_OS_Details_SCM_OS_Details_UploadFile;
            //if (Item.UploadFileIsEnable.HasValue && Item.UploadFileIsEnable.Value)
            //{
            //    UFIsVisible.SetVisible(true);
            //}
            //else
            //{
            //    UFIsVisible.SetVisible(false);
            //}
        }

        // public async Task  UploadFileIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCM_OS_Details Item  )
        // {
        //     // نمایش / عدم نمایش فیلد فایل مدارک
        //     var UFIsVisible = Ref_SCM_OS_Details_SCM_OS_Details_UploadFile;
        //     if (Selected.Value.ToString() == "true")
        //     {
        //         UFIsVisible.SetVisible(true);
        //     }
        //     else
        //     {
        //         UFIsVisible.SetVisible(false); 
        //     }
        // }

        #endregion FunctionEvents

    }
}