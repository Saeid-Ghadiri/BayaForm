using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using System.Globalization;
using BlazorBootstrap;
using Blazored.Toast.Services;

namespace Forms.Forms
{
    public class Form_399Base : Form_399Peropeties
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
		
			var List = _Entity.SCMICT_ProductRequestDetails.ToList();
			foreach (var Item in List)
			{
                // بررسی کد تحویل وارد شده توسط انباردار
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

                // NumberofGoodsDelivery
				if (Item.NumberofGoodsDelivery == null || Item.NumberofGoodsDelivery == 0)
				{
					IsValid = false;
					toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به درخواست دهنده را تکمیل نمایید.",
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
	    for (int i = 0; i < _Entity.SCMICT_ProductRequestDetails.Count; i++)
	    {
	    	var item = _Entity.SCMICT_ProductRequestDetails.ToList()[i];
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

    public async Task<bool> SCMICT_ProductRequestDetails_editmodelsaving(object e   )
        {
            bool IsValid = false;
			var MainModel = (Entity.SCMICT_ProductRequestDetails)e;


            

			return IsValid;
        }
public async Task SCMICT_ProductRequestDetails_customizeeditmodel(GridCustomizeEditModelEventArgs e   )
        {

        }
public async Task SCMICT_ProductRequestDetails_afterrendermodal(Entity.SCMICT_ProductRequestDetails Item   )
        {

        }

		public async Task InsertDeliveryCode_oninput(ChangeEventArgs Selected   )
        {
			// تکمیل فیلد کد تحویل کالا در همه ردیف های گرید
			// for (int i = 0; i < _Entity.SCMPETCO_ProductRequestDetails.Count; i++)
			// {
			// 	var item = _Entity.SCMPETCO_ProductRequestDetails.ToList()[i];
			// 	item.GetDeliveryCode = int.Parse(Selected.Value.ToString());
			// }

			var List = _Entity.SCMICT_ProductRequestDetails.ToList();
			foreach (var Item in List)
			{
				Item.GetDeliveryCode = int.Parse(Selected.Value.ToString());
			}
			StateHasChanged();
        }

		#endregion FunctionEvents

}
}
