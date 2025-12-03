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
    public class Form_711Base : Form_711Peropeties
    {

    // Toast
		[Inject]
		public IToastService toastService { get; set; }


		public async Task ChangeDateTime_Anbardar()
		{
			var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();
			// تبدیل تاریخ و تکمیل فیلد
			for (int i = 0; i < List.Count; i++)
			{
				var item = List[i];
				if (item.GetDeliveryCode != null)
				{
					// تبدیل تاریخ شمسی به میلادی
					System.Globalization.PersianCalendar PC = new System.Globalization.PersianCalendar();

					var DateNow = DateTime.Now;
					// تاریخ شمسی پر می شود
					item.DateTimeDeliveryCode = PC.GetYear(DateNow) + "/" + PC.GetMonth(DateNow).ToString("0#") + "/" + PC.GetDayOfMonth(DateNow).ToString("0#");
				}
			}
		}

		public async Task<bool> CheckDeliveryCode(bool IsValid)
		{
			IsValid = true;
			// بررسی کد تحویل وارد شده توسط انباردار
			var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();
			for (int i = 0; i < List.Count(); i++)
			{
				var Item = List[i];
				if (Item.DeliveryCode != Item.GetDeliveryCode)
				{
					IsValid = false;
					toastService.ShowError("کد تحویل وارد شده صحیح نیست!! لطفا کد صحیح را وارد نمایید.");
				}
			}

			return IsValid;
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
    // protected override async Task OnAfterRenderAsync(bool firstRender)
    // {
    //     if (firstRender)
    //     {
    //         var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();

	// 		foreach (var Item in List)
	// 		{
    //             // نحوه تامین کالا
    //             Item.SupplyGoodsIsEnable = null;
	// 		}

    //         StateHasChanged();
    //     }
    // }

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
                // // Global_SCMRequestTypeId - مشخص کردن وضعیت یک درخواست زنجیره تامین
                // if (Item.Global_SCMRequestTypeId == null)
				// {
				// 	IsValid = false;
				// 	toastService.ShowError("یک نوع درخواست انتخاب کنید.",
				// 	settings => {
				// 		settings.Timeout = 4;
				// 		settings.ShowProgressBar = true;
				// 		settings.PauseProgressOnHover = true;
				// 	});
				// }

                // // SupplyGoodsIsEnable - نحوه تامین کالا
                // if (Item.SupplyGoodsIsEnable == null)
				// {
				// 	IsValid = false;
				// 	toastService.ShowError("لطفا گزینه نحوه تامین کالا تکمیل گردد",
				// 	settings => {
				// 		settings.Timeout = 4;
				// 		settings.ShowProgressBar = true;
				// 		settings.PauseProgressOnHover = true;
				// 	});
				// }
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



        public async Task SCMPLATE_ProductRequestDetails_afterrendermodal(Entity.SCMPLATE_ProductRequestDetails Item   )
        {

        }
		
		public async Task InsertDeliveryCode_oninput(ChangeEventArgs Selected   )
        {
			var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();
			foreach (var Item in List)
			{
				Item.GetDeliveryCode = int.Parse(Selected.Value.ToString());
			}
			StateHasChanged();
        }






		public async Task <bool> GridSCMPLATE_ProductRequestId_303_editmodelsaving(object e   )
        {

            bool IsCancelled = false;

			var Item = (Entity.SCMPLATE_ProductRequestDetails)e;


		    // // Note نوع درخواست
		    // if (Item.Global_SCMRequestTypeId == null)
		    // {
		    // 	IsCancelled = true;
		    // 	toastService.ShowError("لطفا گزینه  نوع درخواست را تکمیل نمایید.",
		    // 		settings =>
		    // 		{
		    // 			settings.Timeout = 4;
		    // 			settings.ShowProgressBar = true;
		    // 			settings.PauseProgressOnHover = true;
		    // 		});
		    // }

		    // // Note نحوه تامین کالا
		    // if (Item.SupplyGoodsIsEnable == null)
		    // {
		    // 	IsCancelled = true;
		    // 	toastService.ShowError("لطفا گزینه نحوه تامین کالا تکمیل گردد.",
		    // 		settings =>
		    // 		{
		    // 			settings.Timeout = 4;
		    // 			settings.ShowProgressBar = true;
		    // 			settings.PauseProgressOnHover = true;
		    // 		});
		    // }

			return IsCancelled;
        }
        
        public async Task  GridSCMPLATE_ProductRequestId_303_afterrendermodal(Entity.SCMPLATE_ProductRequestDetails Item   )
        {

            
        }

		#endregion FunctionEvents

}
}
