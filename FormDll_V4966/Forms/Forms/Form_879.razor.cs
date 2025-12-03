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
    public class Form_879Base : Form_879Peropeties
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

        // if (_Entity.ReqCount != 5)
        // {
        //     IsValid = false;
        //     SumaryMessage += "تعداد درخواست مخالف 5 باشد";
        // }

        var List = _Entity.SCMPETCO_ProductRequestDetails.ToList();


        //Console.WriteLine("#Log FormValidator :");

        // دکمه ثبت و ادامه
        if (BtnWorkFlowId == "SequenceFlow_0etc8dj")
        {
            //Console.WriteLine("#Log FormValidator btn :");
            foreach (var Item in List)
            {
                //Console.WriteLine("#Log FormValidator btn foreach :");

                IsValid = IsValid && await CheckFieldValidation(Item);
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
    /// <summary>
/// بررسی نوع درخواست انتخابی توسط انباردار
///
/// Id										    Title
/// *********************************************************
/// a9c5df1c-d99c-ef11-8354-005056a02a64		تحویل کالا
/// 3b9e934d-d99c-ef11-8354-005056a02a64		خرید کالا
/// 73f6c459-d99c-ef11-8354-005056a02a64		تحویل و خرید کالا
/// 
/// بررسی ضرورت تکمیل فیلدها از این تابع انجام می شود.
/// </summary>
/// <param name="Item"></param>
/// <returns></returns>
public async Task<bool> CheckFieldValidation(Entity.SCMPETCO_ProductRequestDetails Item)
{
    bool IsValid = true;

    var List = _Entity.SCMPETCO_ProductRequestDetails.ToList();

    // تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات
    if (Item.TheNumberDeliveredByLogistics1 == null)
    {
        IsValid = false;
        toastService.ShowError("لطفا تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات را تکمیل نمایید.",
        settings =>
        {
            settings.Timeout = 4;
            settings.ShowProgressBar = true;
            settings.PauseProgressOnHover = true;
        });
    }

    // آیا این خرید بعداً انجام می گردد؟
    if (Item.IsPostponedPurchase == null)
    {
        IsValid = false;
        toastService.ShowError("لطفا گزینه آیا این خرید بعداً انجام می گردد؟ را تکمیل نمایید.",
        settings =>
        {
            settings.Timeout = 4;
            settings.ShowProgressBar = true;
            settings.PauseProgressOnHover = true;
        });
    }

    return IsValid;

}
        public async Task TadarokatAnbarIsVisible(bool Visible, bool Value, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            // تحویل
            Ref_SCMPETCO_ProductRequestDetails_RemainingPurchaseQty.SetVisible(Visible);
            // Item.ProductDelivery = Value;
            
        }


    public async Task <bool> SCMPETCO_ProductRequestDetails_editmodelsaving(object e   )
    {
        bool IsCancelled = false;

        var Item = (Entity.SCMPETCO_ProductRequestDetails)e;

        IsCancelled = !await CheckFieldValidation(Item);

        return IsCancelled;
    }

    public async Task SCMPETCO_ProductRequestDetails_afterrendermodal(Entity.SCMPETCO_ProductRequestDetails Item   )
    {
        // آیا کالا دارای (Technical Data Sheet - (TDS است؟
        var UFIsVisible = Ref_SCMPETCO_ProductRequestDetails_SCMPETCO_ProductRequestDetails_ProductDataSheetFile;
		if (Item.ProductDataSheet.HasValue && Item.ProductDataSheet.Value)
		{
		    UFIsVisible.SetVisible(true);
		}
		else
		{
			UFIsVisible.SetVisible(false); 
		}        

        // نمایش / عدم نمایش فیلد فایل استعلام
        var UF1IsVisible = Ref_SCMPETCO_ProductRequestDetails_SCMPETCO_ProductRequestDetails_InquiryFirst;
        var UF2IsVisible = Ref_SCMPETCO_ProductRequestDetails_SCMPETCO_ProductRequestDetails_InquirySecondFile;
        var UF3IsVisible = Ref_SCMPETCO_ProductRequestDetails_SCMPETCO_ProductRequestDetails_InquiryThirdFile;

		if (Item.Inquiry.HasValue && Item.Inquiry.Value)
		{
		    UF1IsVisible.SetVisible(true); 
		    UF2IsVisible.SetVisible(true); 
		    UF3IsVisible.SetVisible(true);
		}
		else
		{
			UF1IsVisible.SetVisible(false); 
			UF2IsVisible.SetVisible(false); 
			UF3IsVisible.SetVisible(false); 
		}


        // 4c0956ae-115f-f011-a506-005056a2b6bd  خرید شده و تحویل می گردد
        // 9cfdd7c2-115f-f011-a506-005056a2b6bd   خرید بعداً انجام می گردد
        // e483b3d3-115f-f011-a506-005056a2b6bd   بخشی از خرید بعداً انجام می گردد

        if (Item.SCMPETCO_LogisticsDeliveryTypeId.ToString() == "e483b3d3-115f-f011-a506-005056a2b6bd")
        {
            await TadarokatAnbarIsVisible(true, true, Item);
        }
        else
        {
            await TadarokatAnbarIsVisible(false, false, Item);
        }
    }

    public async Task ProductDataSheet_oninput(ChangeEventArgs Selected ,Entity.SCMPETCO_ProductRequestDetails Item  )
    {
        // آیا کالا دارای (Technical Data Sheet - (TDS است؟
        var UFDTSIsVisible = Ref_SCMPETCO_ProductRequestDetails_SCMPETCO_ProductRequestDetails_ProductDataSheetFile;
		if (Selected.Value.ToString() == "true")
		{
		    UFDTSIsVisible.SetVisible(true);
		}
		else
		{
			UFDTSIsVisible.SetVisible(false); 
		}
    }
    
    public async Task Inquiry_oninput(ChangeEventArgs Selected ,Entity.SCMPETCO_ProductRequestDetails Item  )
    {
        // نمایش / عدم نمایش فیلد فایل استعلام
        var UF1IsVisible = Ref_SCMPETCO_ProductRequestDetails_SCMPETCO_ProductRequestDetails_InquiryFirst;
        var UF2IsVisible = Ref_SCMPETCO_ProductRequestDetails_SCMPETCO_ProductRequestDetails_InquirySecondFile;
        var UF3IsVisible = Ref_SCMPETCO_ProductRequestDetails_SCMPETCO_ProductRequestDetails_InquiryThirdFile;

		if (Selected.Value.ToString() == "true")
		{
		    UF1IsVisible.SetVisible(true); 
		    UF2IsVisible.SetVisible(true); 
		    UF3IsVisible.SetVisible(true);
		}
		else
		{
			UF1IsVisible.SetVisible(false); 
			UF2IsVisible.SetVisible(false); 
			UF3IsVisible.SetVisible(false); 
		}
    }

		public async Task  SCMPETCO_LogisticsDeliveryTypeId_onitemselected(Entity.SCMPETCO_LogisticsDeliveryType Selected ,Entity.SCMPETCO_ProductRequestDetails Item  )
        {
            if (Item.SCMPETCO_LogisticsDeliveryTypeId.ToString() == "e483b3d3-115f-f011-a506-005056a2b6bd")
            {
                await TadarokatAnbarIsVisible(true, true, Item);
            }
            else
            {
                await TadarokatAnbarIsVisible(false, false, Item);
            }
        }

		
		#endregion FunctionEvents

}
}



