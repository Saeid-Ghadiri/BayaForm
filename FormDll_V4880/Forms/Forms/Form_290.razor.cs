using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using Blazored.Toast.Services;

namespace Forms.Forms
{
    public class Form_290Base : Form_290Peropeties
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
            // حذف Defualt Value ها
			var List = _Entity.SCM_ProductRequestDetails.ToList();
			foreach(var Item in List)
			{
				// نحوه تامین کالا
				Item.InquiryTrueFalse = null;
			}            
			StateHasChanged();
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
		foreach (var Item in List)
		{
            // Note: آیا نیاز به استعلام دارد؟ 
            if (Item.InquiryTrueFalse == null)
			{
				IsValid = false;
				toastService.ShowError("لطفا گزینه آیا نیاز به استعلام دارد؟ را تکمیل نمایید",
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

    public async Task DeficitSupplyNumber_oninput(ChangeEventArgs Selected ,Entity.SCM_ProductRequestDetails Item  )
        {

        }

		public async Task<bool> SCM_ProductRequestDetails_editmodelsaving(object e   )
        {
bool IsValid = false;

			var MainModel = (Entity.SCM_ProductRequestDetails)e;

			// Note: شرط پُر بودن فیلد نام کالا دیزیبل
			if (MainModel.ProductNameText == null)
			{
				IsValid = true;
				toastService.ShowError("لطفا گزینه نام کالا را تکمیل نمایید",
					settings =>
					{
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}

            // Note: آیا نیاز به استعلام دارد؟ 
			if (MainModel.InquiryTrueFalse == null)
			{
				IsValid = true;
				toastService.ShowError("لطفا گزینه آیا نیاز به استعلام دارد؟ را تکمیل نمایید",
					settings =>
					{
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}

			// Note: شرط پُر بودن فیلد تعداد یا مقدار درخواستی
			if (MainModel.ProductRequestingQTY == null || MainModel.ProductRequestingQTY == 0)
			{
				IsValid = true;
				toastService.ShowError("لطفا گزینه تعداد یا مقدار درخواستی را تکمیل نمایید.",
					settings =>
					{
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}

			// Note: آیا تامین کسری انجام شود؟
			if (MainModel.FutureActionTrueFalse == null)
			{
				IsValid = true;
				toastService.ShowError("لطفا گزینه آیا تامین کسری انجام شود؟ تکمیل نمایید.",
					settings =>
					{
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}

			if (MainModel.FutureActionTrueFalse != null && MainModel.FutureActionTrueFalse == true)
			{
				// Note: تعداد تامین کسری
				if (MainModel.DeficitSupplyNumber == null || MainModel.DeficitSupplyNumber == 0)
				{
					IsValid = true;
					toastService.ShowError("لطفا گزینه تعداد تامین کسری تکمیل نمایید.",
						settings =>
						{
							settings.Timeout = 4;
							settings.ShowProgressBar = true;
							settings.PauseProgressOnHover = true;
						});
				}
			}

			return IsValid;
        }

		public async Task SCM_ProductRequestDetails_afterrendermodal(Entity.SCM_ProductRequestDetails Item   )
        {
			// Note: مخفی کردن فیلد بارگذاری فایل تی دی اس، بر اساس فیلد آیا دیتاشیت دارد یا خیر؟ 
			if (Item.ProductDataSheetTrueFalse.HasValue && Item.ProductDataSheetTrueFalse.Value)
			{

				Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductDataSheetFile.SetVisible(true);

			}
			else
			{
				Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductDataSheetFile.SetVisible(false);
			}
        }
        public async Task ProductDataSheetTrueFalse_oninput(ChangeEventArgs Selected ,Entity.SCM_ProductRequestDetails Item  )
        {
            // Note: مخفی کردن فیلد بارگذاری فایل تی دی اس، بر اساس فیلد آیا دیتاشیت دارد یا خیر؟
			var Item2 = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductDataSheetFile;

			if (Selected.Value.ToString() == "true")
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
