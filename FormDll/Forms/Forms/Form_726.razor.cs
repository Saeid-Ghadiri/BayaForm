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
    public class Form_726Base : Form_726Peropeties
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

        // if (_Entity.ReqCount != 5)
        // {
        //     IsValid = false;
        //     SumaryMessage += "تعداد درخواست مخالف 5 باشد";
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
        foreach (var item in _Entity.SCMPETCO_ProductRequestDetails)
        {
            item.EnableLaterPurchace2 = true;
        }
    }

    #region FunctionEvents
    public async Task TahvilIsVisible(bool Visible, bool Value, Entity.SCMPETCO_ProductRequestDetails Item)
	{
		// تحویل
		Ref_SCMPETCO_ProductRequestDetails_ProductDelivery.SetVisible(Visible);
		Item.ProductDelivery = Value;
	}
	
	public async Task KharidIsVisible(bool Visible, bool Value, Entity.SCMPETCO_ProductRequestDetails Item)
	{
		// خرید
		Ref_SCMPETCO_ProductRequestDetails_FutureAction.SetVisible(Visible);
		Item.FutureAction = Value;
		// تعداد تامین کسری	
		// Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber.SetVisible(Visible);
	}
	
	public async Task TahvilKharidIsVisible(bool Visible, bool Value, Entity.SCMPETCO_ProductRequestDetails Item)
	{
		// تحویل
		Ref_SCMPETCO_ProductRequestDetails_ProductDelivery.SetVisible (Visible);
		Item.ProductDelivery = Value;
		// خرید
		Ref_SCMPETCO_ProductRequestDetails_FutureAction.SetVisible(Visible);
		Item.FutureAction = Value;
		// تعداد تامین کسری
		// Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber.SetVisible (Visible);
	}

    public async Task <bool> SCMPETCO_ProductRequestDetails_editmodelsaving(object e   )
        {

            return false;
        }
        public async Task  SCMPETCO_ProductRequestDetails_afterrendermodal(Entity.SCMPETCO_ProductRequestDetails Item   )
        {
            // // Note: مخفی کردن فیلد بارگذاری فایل تی دی اس، بر اساس فیلد آیا دیتاشیت دارد یا خیر؟ 
			// if (Item.ProductDataSheet.HasValue && Item.ProductDataSheet.Value)
			// {
			// 	Ref_SCMPETCO_ProductRequestDetails_SCMPETCO_ProductRequestDetails_ProductDataSheetFile.SetVisible(true);
			// }
			// else
			// {
			// 	Ref_SCMPETCO_ProductRequestDetails_SCMPETCO_ProductRequestDetails_ProductDataSheetFile.SetVisible(false);
			// }

            // فیلد نوع درخواست
            if (Item.Global_SCMRequestTypeId == null)
            {
                await TahvilKharidIsVisible(false,false,Item);
            }
			else
			{
				// شرط اینکه آیا فرآیند تحویل اجرا گردد؟
            	if(Item.Global_SCMRequestTypeId.ToString() =="a9c5df1c-d99c-ef11-8354-005056a02a64")
            	{
            	    await TahvilIsVisible(true,true,Item);
					await KharidIsVisible(false,false,Item);
            	}
	
            	// شرط نوع درخواست بر اساس خرید کالا
            	if(Item.Global_SCMRequestTypeId.ToString() =="3b9e934d-d99c-ef11-8354-005056a02a64")
            	{
            	    await TahvilIsVisible(false,false,Item);
					await KharidIsVisible(true,true,Item);
            	}
	
            	// شرط نوع درخواست بر اساس تحویل و خرید کالا
            	if(Item.Global_SCMRequestTypeId.ToString() =="73f6c459-d99c-ef11-8354-005056a02a64")
            	{
            	    await TahvilKharidIsVisible(true,true,Item);
            	}
			}
        }
        
        // public async Task  ProductDataSheet_oninput(ChangeEventArgs Selected ,Entity.SCMPETCO_ProductRequestDetails Item  )
        // {

        //     // Note: مخفی کردن فیلد بارگذاری فایل تی دی اس، بر اساس فیلد آیا دیتاشیت دارد یا خیر؟
		// 	var Item2 = Ref_SCMPETCO_ProductRequestDetails_SCMPETCO_ProductRequestDetails_ProductDataSheetFile;

		// 	if (Selected.Value.ToString() == "true")
		// 	{
		// 		Item2.SetVisible(true);
		// 	}
		// 	else
		// 	{
		// 		Item2.SetVisible(false);
		// 	}
        // }

		public async Task  Global_SCMRequestTypeId_onitemselected(Entity.Global_SCMRequestType Selected ,Entity.SCMPETCO_ProductRequestDetails Item  )
        {
            // Console.WriteLine(await Utility.JSON.ToJson(Selected));

           	// شرط اینکه آیا فرآیند تحویل اجرا گردد؟
            if(Item.Global_SCMRequestTypeId.ToString() == "a9c5df1c-d99c-ef11-8354-005056a02a64")
            {
                await TahvilIsVisible(true,true,Item);
				await KharidIsVisible(false,false,Item);
            }

            // شرط نوع درخواست بر اساس خرید کالا
            if(Item.Global_SCMRequestTypeId.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64")
            {
                await TahvilIsVisible(false,false,Item);
				await KharidIsVisible(true,true,Item);
            }

            // شرط نوع درخواست بر اساس تحویل و خرید کالا
            if(Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64")
            {
                await TahvilKharidIsVisible(true,true,Item);
            }
        }

		#endregion FunctionEvents

}
}
