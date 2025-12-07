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
    public class Form_495Base : Form_495Peropeties
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

			var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();
			foreach (var Item in List)
			{
				// DeliveryCode - GetDeliveryCode - بررسی کد تحویل کالا در زمان ثبت
                if (Item.DeliveryCode != Item.GetDeliveryCode)
				{
					IsValid = false;
					toastService.ShowError("کد تحویل وارد شده صحیح نیست!! لطفا کد صحیح را وارد نمایید.",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
				}

                // NumberofGoodsDelivery - تعداد یا مقدار واگذاری کالا به درخواست دهنده
                if (Item.NumberofGoodsDelivery == null)
				{
					IsValid = false;
					toastService.ShowError("تعداد مقدار واگذاری کالا را تکمیل نمایید.",
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
        #region DateTimeGetDeliveryCode
        // تبدیل تاریخ و تکمیل فیلد
	    for (int i = 0; i < _Entity.SCMPLATE_ProductRequestDetails.Count; i++)
	    {
	    	var item = _Entity.SCMPLATE_ProductRequestDetails.ToList()[i];
	    	if (item.GetDeliveryCode != null)
	    	{
	    		// تبدیل تاریخ شمسی به میلادی            
	    		System.Globalization.PersianCalendar PC = new System.Globalization.PersianCalendar();  
	    		var DateNow = DateTime.Now;
	    		// تاریخ شمسی پر می شود
	    		item.DateTimeDeliveryCode = PC.GetYear(DateNow) + "/" + PC.GetMonth(DateNow).ToString("0#") + "/" + PC.GetDayOfMonth(DateNow).ToString("0#");
	    	}
	    }
        #endregion  / DateTimeGetDeliveryCode
        
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

		public async Task InsertDeliveryCode_oninput(ChangeEventArgs Selected   )
        {
			var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();
			foreach (var Item in List)
			{
				Item.GetDeliveryCode = int.Parse(Selected.Value.ToString());
			}
			StateHasChanged();
        }

	public async Task <bool> SCMPLATE_ProductRequestDetails_editmodelsaving(object e   )
        {
            bool IsCancelled = false;
            //DM = Datial Model
            var DM = (Entity.SCMPLATE_ProductRequestDetails)e;

            // NumberofGoodsDelivery - تعداد یا مقدار واگذاری کالا به درخواست دهنده
            if (DM.NumberofGoodsDelivery == null)
			{
				IsCancelled = true;
				toastService.ShowError("تعداد مقدار واگذاری کالا را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}
    
            return IsCancelled;
        }
        
        public async Task  SCMPLATE_ProductRequestDetails_afterrendermodal(Entity.SCMPLATE_ProductRequestDetails Item   )
        {
        }

		#endregion FunctionEvents

}
}
