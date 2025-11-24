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
    public class Form_530Base : Form_530Peropeties
    {

        // Toast  
		[Inject]
	    public IToastService toastService { get; set; }

        // فیلد تاریخ - تبدیل تاریخ - DTFA_DeliveryProcurement
		public async Task DTFA_DeliveryProcurement()
		{
            var List = _Entity.SCMPETCO_OS_Details.ToList();

			foreach(var item in List)
			{
				// if (item.GetDeliveryCode != null)
				// {
				// 	// تبدیل تاریخ شمسی به میلادی            
				// 	System.Globalization.PersianCalendar PC = new System.Globalization.PersianCalendar();
				// 	var DateNow = DateTime.Now;

				// 	// تاریخ شمسی پر می شود
				// 	item.DTFA_DeliveryProcurement = PC.GetYear(DateNow) + "/" + PC.GetMonth(DateNow).ToString("0#") + "/" + PC.GetDayOfMonth(DateNow).ToString("0#");
				// }

                // تبدیل تاریخ شمسی به میلادی            
				System.Globalization.PersianCalendar PC = new System.Globalization.PersianCalendar();
				var DateNow = DateTime.Now;
				// تاریخ شمسی پر می شود
				item.DTFA_DeliveryProcurement = PC.GetYear(DateNow) + "/" + PC.GetMonth(DateNow).ToString("0#") + "/" + PC.GetDayOfMonth(DateNow).ToString("0#");
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

        var List = _Entity.SCMPETCO_OS_Details.ToList();
        
        int ListCount = List.Count();

        foreach (var Item in List)
        {
            // DTFA_DeliveryProcurement - تاریخ تحویل کار به انبار توسط تدارکات
            // if (Item.DTFA_DeliveryProcurement == null)
			// {
			// 	IsValid = false;
                
			// 	toastService.ShowError("لطفا گزینه تاریخ تحویل کار به انبار توسط تدارکات را تکمیل نمایید.",
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

        if ((await BeforSubmit()).Status != HttpStatusCode.OK)
        {
            StateHasChanged();
            Result.Status = HttpStatusCode.InternalServerError;
            return Result;
        }

        _loadingService.Show();
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
            _loadingService.Hide();
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
        // تاریخ تحویل کالا به انبار
        await DTFA_DeliveryProcurement();


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

        public async Task ResultingFrom_Code13(bool Visible, bool Value, Entity.SCMPETCO_OS_Details Item)
        {
            // IsEnableDemolitionAndRenovation
            Ref_SCMPETCO_OS_Details_IsEnableDemolitionAndRenovation.SetVisible(Visible);
			Item.IsEnableDemolitionAndRenovation = Value;
            Ref_SCMPETCO_OS_Details_IsEnableDemolitionAndRenovation.SetDisabled(true);
        }


    public async Task<bool> SCMPETCO_OS_Details_editmodelsaving(object e   )
    {
        bool IsCancelled = false;

		var MainModel = (Entity.SCMPETCO_OS_Details)e;

        // // تاریخ تحویل کار به انبار توسط تدارکات
        // if (MainModel.DTFA_DeliveryProcurement == null)
		// {
		// 	IsCancelled = true;
            
		// 	toastService.ShowError("لطفا گزینه تاریخ تحویل کار به انبار توسط تدارکات را تکمیل نمایید.",
		// 	settings => {
		// 		settings.Timeout = 4;
		// 		settings.ShowProgressBar = true;
		// 		settings.PauseProgressOnHover = true;
		// 	});
		// }

        return IsCancelled;
    }
        
    public async Task  SCMPETCO_OS_Details_afterrendermodal(Entity.SCMPETCO_OS_Details Item)
    {
        Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_UploadFiles.SetDisabled(true);
        Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_SampleGoodsFiles.SetDisabled(true);

        
        // 
        if (Item.UploadFileIsEnable.HasValue && Item.UploadFileIsEnable.Value)
		{
			Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_UploadFiles.SetVisible(true); 
		}
		else
		{
			Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_UploadFiles.SetVisible(false); 
		}

        // فیلد: منتج از پیمانکار با Code 13
        if( _Entity.SCMPETCO_OS_ResultingFromId.HasValue && _Entity.SCMPETCO_OS_ResultingFromId.ToString() == "2a087a09-9fbe-ef11-a4fa-005056a2b6bd")
        {
            await ResultingFrom_Code13(true, true, Item);
        }
        else
        {
            await ResultingFrom_Code13(false, false, Item);
        }

        if (Item.IsEnableSampleGoods.HasValue && Item.IsEnableSampleGoods.Value)
		{
			Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_SampleGoodsFiles.SetVisible(true); 
		}
		else
		{
			Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_SampleGoodsFiles.SetVisible(false); 
		}
    }

	public async Task  UploadFileIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCMPETCO_OS_Details Item)
    {
        var UploadFilesIsVisible = Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_UploadFiles;
        
		if (Selected.Value.ToString() == "true")
		{
			UploadFilesIsVisible.SetVisible(true); 
		}
		else
		{
			UploadFilesIsVisible.SetVisible(false); 
		}
    }

		public async Task  SCMPETCO_OS_ResultingFromId_onitemselected(Entity.SCMPETCO_OS_ResultingFrom Selected   )
        {
        }

		public async Task  IsEnableSampleGoods_oninput(ChangeEventArgs Selected ,Entity.SCMPETCO_OS_Details Item  )
        {
            // مخفی کردن فیلد فایل نمونه
			var SampleFileIsVisible = Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_SampleGoodsFiles;

			if (Selected.Value.ToString() == "true")
			{
				SampleFileIsVisible.SetVisible(true); 
			}
			else
			{
				SampleFileIsVisible.SetVisible(false); 
			}            
        }

		#endregion FunctionEvents

}
}
