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
    public class Form_417Base : Form_417Peropeties
    {
      
    
        // Toast  
		[Inject]
	    public IToastService toastService { get; set; }


        public async Task ChangeDateTime_Anbardar()
		{
			var List = _Entity.SCMICT_ProductRequestDetails.ToList();
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
			var List = _Entity.SCMICT_ProductRequestDetails.ToList();
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
        await CheckDeliveryCode(true);
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
				// // حذف Defualt Value ها
				// var List = _Entity.SCMICT_ProductRequestDetails.ToList();
				// foreach (var Item in List)
				// {
				// 	// نحوه تامین کالا
				// 	Item.SupplyGoodsIsEnable = null;
				// }
				// StateHasChanged();
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
				// }

                // // NumberofGoodsDelivery
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
        await ChangeDateTime_Anbardar();
		
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

		public async Task TahvilIsVisible(bool Visible, bool Value, Entity.SCMICT_ProductRequestDetails Item)
		{
			// تحویل
			Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(Visible);
			Item.GoodsDeliveryIsEnable = Value;
		}
		
		public async Task KharidIsVisible(bool Visible, bool Value, Entity.SCMICT_ProductRequestDetails Item)
		{
			// خرید
			Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(Visible);
			Item.DeficitSupplyIsEnable = Value;
			// تعداد تامین کسری	
			Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber.SetVisible(Visible);
		}
		
		public async Task TahvilKharidIsVisible(bool Visible, bool Value, Entity.SCMICT_ProductRequestDetails Item)
		{
			// تحویل
			Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible (Visible);
			Item.GoodsDeliveryIsEnable = Value;
			// خرید
			Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(Visible);
			Item.DeficitSupplyIsEnable = Value;
			// تعداد تامین کسری
			Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber.SetVisible (Visible);
		}

		public async Task AnbardarIsVisible(bool Visible)
		{
			// تعداد یا مقدار واگذاری کالا
			Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible (Visible);
			// دریافت کد تحویل کالا
			Ref_SCMICT_ProductRequestDetails_GetDeliveryCode.SetVisible (Visible);
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
			// 		{
			// 			settings.Timeout = 4;
			// 			settings.ShowProgressBar = true;
			// 			settings.PauseProgressOnHover = true;
			// 		});
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

		public async Task  SCMICT_ProductRequestDetails_afterrendermodal(Entity.SCMICT_ProductRequestDetails Item   )
        {

            // فیلد نوع درخواست
            if (Item.Global_SCMRequestTypeId == null)
            {
                await TahvilKharidIsVisible(false,false,Item);
				await AnbardarIsVisible(false);
            }
			else
			{
				// شرط اینکه آیا فرآیند تحویل اجرا گردد؟
            	if(Item.Global_SCMRequestTypeId.ToString() =="a9c5df1c-d99c-ef11-8354-005056a02a64")
            	{
            	    await TahvilIsVisible(true,true,Item);
					await AnbardarIsVisible(true);
					await KharidIsVisible(false,false,Item);
            	}
	
            	// شرط نوع درخواست بر اساس خرید کالا
            	if(Item.Global_SCMRequestTypeId.ToString() =="3b9e934d-d99c-ef11-8354-005056a02a64")
            	{
            	    await TahvilIsVisible(false,false,Item);
					await AnbardarIsVisible(false);
					await KharidIsVisible(true,true,Item);
            	}
	
            	// شرط نوع درخواست بر اساس تحویل و خرید کالا
            	if(Item.Global_SCMRequestTypeId.ToString() =="73f6c459-d99c-ef11-8354-005056a02a64")
            	{
            	    await TahvilKharidIsVisible(true,true,Item);
					await AnbardarIsVisible(true);
            	}
			}
        }

		public async Task SupplyGoodsIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCMICT_ProductRequestDetails Item  )
        {

        }

		public async Task  Global_SCMRequestTypeId_onitemselected(dynamic Selected ,Entity.SCMICT_ProductRequestDetails Item  )
        {
			
           	// شرط اینکه آیا فرآیند تحویل اجرا گردد؟
            if(Item.Global_SCMRequestTypeId.ToString() =="a9c5df1c-d99c-ef11-8354-005056a02a64")
            {
                await TahvilIsVisible(true,true,Item);
				await AnbardarIsVisible(true);
				await KharidIsVisible(false,false,Item);
            }

            // شرط نوع درخواست بر اساس خرید کالا
            if(Item.Global_SCMRequestTypeId.ToString() =="3b9e934d-d99c-ef11-8354-005056a02a64")
            {
                await TahvilIsVisible(false,false,Item);
				await AnbardarIsVisible(false);
				await KharidIsVisible(true,true,Item);
            }

            // شرط نوع درخواست بر اساس تحویل و خرید کالا
            if(Item.Global_SCMRequestTypeId.ToString() =="73f6c459-d99c-ef11-8354-005056a02a64")
            {
                await TahvilKharidIsVisible(true,true,Item);
				await AnbardarIsVisible(true);
            }           
        }

		public async Task  InsertDeliveryCode_oninput(ChangeEventArgs Selected   )
        {
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
