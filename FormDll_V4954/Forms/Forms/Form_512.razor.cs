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
    public class Form_512Base : Form_512Peropeties
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
                // // بررسی کد تحویل وارد شده توسط انباردار
				// if (Item.DeliveryCode != Item.GetDeliveryCode)
				// {
				// 	IsValid = false;
				// 	toastService.ShowError("کد تحویل وارد شده صحیح نیست!! لطفا کد صحیح را وارد نمایید.",
				// 	settings => {
				// 		settings.Timeout = 4;
				// 		settings.ShowProgressBar = true;
				// 		settings.PauseProgressOnHover = true;
				// 	});
				}

                // NumberofGoodsDelivery
				// if (Item.NumberofGoodsDelivery == null || Item.NumberofGoodsDelivery == 0)
				// {
				// 	IsValid = false;
				// 	toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به درخواست دهنده را تکمیل نمایید.",
				// 	settings => {
				// 		settings.Timeout = 4;
				// 		settings.ShowProgressBar = true;
				// 		settings.PauseProgressOnHover = true;
				// 	});
				// }

                //  // SupplyGoodsIsEnable
				// if (Item.SupplyGoodsIsEnable == null )
				// {
				// 	IsValid = false;
				// 	toastService.ShowError("لطفا گزینه نحوه تامین کالا را تکمیل نمایید.",
				// 	settings => {
				// 		settings.Timeout = 4;
				// 		settings.ShowProgressBar = true;
				// 		settings.PauseProgressOnHover = true;
				// 	});
				// }
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

    public async Task InsertDeliveryCode_oninput(ChangeEventArgs Selected   )
        {
			// var List = _Entity.SCMICT_ProductRequestDetails.ToList();
			// foreach (var Item in List)
			// {
			// 	Item.GetDeliveryCode = int.Parse(Selected.Value.ToString());
			// }
			// StateHasChanged();
        }

		public async Task<bool> SCMICT_ProductRequestDetails_editmodelsaving(object e   )
        {
            bool IsValid = false;
			var MainModel = (Entity.SCMICT_ProductRequestDetails)e;

			// // NumberofGoodsDelivery
			// if (MainModel.NumberofGoodsDelivery == null)
			// {
			// 	IsValid = true;
			// 	toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به به درخواست دهنده را تکمیل نمایید.",
			// 		settings =>
			//  		{
			//  			settings.Timeout = 4;
			//  			settings.ShowProgressBar = true;
			//  			settings.PauseProgressOnHover = true;
			//  		});
			// }
           
           	// // SupplyGoodsIsEnable
            // if (MainModel.SupplyGoodsIsEnable == null)
			// {
			// 	IsValid = true;
			// 		toastService.ShowError("لطفا گزینه نحوه تامین کالا را تکمیل نمایید.",
			// 		settings =>
			// 		{
			// 			settings.Timeout = 4;
			// 			settings.ShowProgressBar = true;
			// 			settings.PauseProgressOnHover = true;
			// 		});
			// }

			return false;
        }
        
   

		public async Task SupplyGoodsIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCMICT_ProductRequestDetails Item  )
        {

        }

		public async Task  Global_SCMRequestTypeId_onitemselected(dynamic Selected ,Entity.SCMICT_ProductRequestDetails Item  )
        {

            
        }

		public async Task  petcoProductSearch_NotMapped_onitemselected(dynamic Selected ,Entity.SCMICT_ProductRequestDetails Item  )
        {
                      /// <summary>
        /// فیلدهای زیر فیلدهای اصلی برای نمایش در فرم هستند.
        ///
        ///</summary>
        Console.WriteLine("start");
        //  نام کالا
        Item.SH_DESC = Selected.DESC;
        // کد کالا شماران
        Item.SH_PARTNO = Selected.PARTNO;
        // شناسه سیستمی کد کالا شماران
        Item.SH_PARTNO_GUID = Selected.PARTNO_GUID;
        // شماره کالا شماران
        Item.SH_PARTCODE = Selected.PARTCODE;
        // شناسه شماره کالا شماران
        Item.SH_PARTCODE_GUID = Selected.PARTCODE_GUID;
        // نام کالا شماران
        Item.SH_DESC = Selected.DESC;
        // واحد کالا شماران
        Item.SH_UNIT = Selected.UNIT;
        // کد دسته بندی فرعی کالا شماران
        Item.SH_SUBGRCODE = Selected.SUBGRCODE;
        // شناسه کد دسته بندی فرعی کالا شماران
        Item.SH_SUBGRCODE_GUID = Selected.SUBGRCODE_GUID;
        // کد دسته بندی اصلی شماران
        Item.SH_GRCODE = Selected.GRCODE;
        // شناسه کد دسته بندی اصلی کالا شماران
        Item.SH_GRCODE_GUID = Selected.GRCODE_GUID;
        // نام دسته بندی اصلی کالا شماران
        Item.SH_GroupName = Selected.GroupName;
        // نام دسته بندی فرعی کالا شماران
        Item.SH_SubGroupName = Selected.SubGroupName;
        // سال مالی کالا شماران
        Item.SH_YEAR = Selected.YEAR;
        // کالا موجود است یا خیر شماران
        Item.SH_IsExist = Selected.IsExist;
        // نام شرکت کالا شماران
        Item.SH_Factory = Selected.Factory;
        // کد گروه اصلی کالا شماران
        Item.SH_MapGroupCode = Selected.MapGroupCode;

        // موجودی کالا در شماران            
        if (Selected.Amount > -1)
        {
            Item.SH_Amount = (double)Selected.Amount;
        }
        }

		#endregion FunctionEvents

}
}
