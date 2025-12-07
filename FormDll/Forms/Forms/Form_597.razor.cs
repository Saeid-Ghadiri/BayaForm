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
    public class Form_597Base : Form_597Peropeties
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
				var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();
				foreach (var Item in List)
				{
				    // آیا کالا دارای (Technical Data Sheet - (TDS است؟
				    // Item.IsEnableTDS = null;
				    // // نحوه تامین کالا
				    // Item.SupplyGoodsIsEnable = null;
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

            var List = _Entity.SCMPLATE_ProductRequestDetails.Where(p=>p.IsDelete == false).ToList();

            var listCount = List.Count();

            // Console.WriteLine("#Log FormValidator :");

            // دکمه ثبت و ادامه           
            if (BtnWorkFlowId == "SequenceFlow_0fkw21h")
            {
                // Console.WriteLine("#Log FormValidator btn :");
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
        public async Task<bool> CheckFieldValidation(Entity.SCMPLATE_ProductRequestDetails Item)
        {
            bool IsValid = true;

		// پرکردن فیلد کد رهگیری

        // شرط پُر بودن فیلد نام کالا دیزیبل
		if (Item.SH_DESC == null)
		{
			IsValid = false;
			toastService.ShowError("لطفا گزینه نام کالا را تکمیل نمایید",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
		}

		// شرط پُر بودن فیلد تعداد یا مقدار درخواستی
		if (Item.ProductRequestingQTY == null || Item.ProductRequestingQTY == 0)
		{
			IsValid = false;
			toastService.ShowError("لطفا گزینه تعداد یا مقدار درخواستی را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
		}

		// شرط پُر بودن فیلد اولویت
		if (Item.Global_PriorityId == null || Item.Global_PriorityId == Guid.Empty)
		{
			IsValid = false;
			toastService.ShowError("لطفا گزینه اولویت را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
		}

		// شرط پُر بودن فیلد محل مصرف
		if (Item.PlaceOfUse == null)
		{
			IsValid = false;
			toastService.ShowError("لطفا گزینه محل مصرف تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
		}

        // در صورتی که خرید یا تحویل و خرید باشد گزینه تعداد تامین کسری ضروری گردد.
		if(Item.Global_SCMRequestTypeId.ToString() =="73f6c459-d99c-ef11-8354-005056a02a64" ||
		  Item.Global_SCMRequestTypeId.ToString() =="3b9e934d-d99c-ef11-8354-005056a02a64")
		{
			if (Item.DeficitSupplyNumber == null)
			{
				IsValid = false;
				toastService.ShowError("لطفا گزینه تعداد تامین کسری ضروری گردد.",
				settings =>
				{
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}
		}

        // SupplyGoodsIsEnable - نحوه تامین کالا
		if (Item.SupplyGoodsIsEnable == null)
		{
			IsValid = false;

			toastService.ShowError("لطفا گزینه نحوه تامین کالا را تکمیل نمایید",
				settings =>
				{
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
		}

            return IsValid;
        }

    public async Task ITILVisible(bool Visible)
    {
        Ref_SCMPLATE_ProductRequestDetails_ResultingFromITIL.SetVisible(Visible);
    }
	public async Task ITILDetailsVisible(bool Visible)
    {
        Ref_SCMPLATE_ProductRequestDetails_RequestIdITIL.SetVisible(Visible);
        Ref_SCMPLATE_ProductRequestDetails_RequestIdITIL.SetDisabled(true);
        Ref_SCMPLATE_ProductRequestDetails_RequesterUserITIL.SetVisible(Visible);
        Ref_SCMPLATE_ProductRequestDetails_RequesterUserITIL.SetDisabled(true);

        Ref_SCMPLATE_ProductRequestDetails_CreatedAtITIL.SetVisible(Visible);
        Ref_SCMPLATE_ProductRequestDetails_CreatedAtITIL.SetDisabled(true);

        Ref_SCMPLATE_ProductRequestDetails_ITILDetails.SetVisible(Visible);
    }
	public async Task TahvilIsVisible(bool Visible, bool Value, Entity.SCMPLATE_ProductRequestDetails Item)
	{
		// تحویل
		Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(Visible);
		Item.GoodsDeliveryIsEnable = Value;
	}
	
	public async Task KharidIsVisible(bool Visible, bool Value, Entity.SCMPLATE_ProductRequestDetails Item)
	{
		// خرید
		Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(Visible);
		Item.DeficitSupplyIsEnable = Value;
		// تعداد تامین کسری	
		Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyNumber.SetVisible(Visible);
	}
	
	public async Task TahvilKharidIsVisible(bool Visible, bool Value, Entity.SCMPLATE_ProductRequestDetails Item)
	{
		// تحویل
		Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible (Visible);
		Item.GoodsDeliveryIsEnable = Value;
		// خرید
		Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(Visible);
		Item.DeficitSupplyIsEnable = Value;
		// تعداد تامین کسری
		Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyNumber.SetVisible (Visible);
	}

    public async Task ProductSearch_NotMapped_onitemselected(dynamic Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
    {
        /// <summary>
        /// فیلدهای زیر فیلدهای اصلی برای نمایش در فرم هستند.
        ///
        ///</summary>

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

	public async Task<bool> SCMPLATE_ProductRequestDetails_editmodelsaving(object e   )
    {
         bool IsCancelled = false;

            var Item = (Entity.SCMPLATE_ProductRequestDetails)e;

            IsCancelled = !await CheckFieldValidation(Item);

            return IsCancelled;
    }

    public async Task SCMPLATE_ProductRequestDetails_afterrendermodal(Entity.SCMPLATE_ProductRequestDetails Item   )
    {
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

		// Show ITIL
        if (Item.ITILCodeIsEnable.HasValue && Item.ITILCodeIsEnable.Value)
		{
            await ITILVisible(true); 
		}
		else
		{
            await ITILVisible(false);
		}

        // Show ITIL Details
        if (!string.IsNullOrEmpty(Item.ResultingFromITIL))
		{
            await ITILDetailsVisible(true);
		}
		else
		{
            await ITILDetailsVisible(false);
		}

        // نمایش جزئیات ITIL
        if (Item.ResultingFromITIL != null)
		{
     		Ref_SCMPLATE_ProductRequestDetails_ITILDetails.SetEntity(Item);
			Ref_SCMPLATE_ProductRequestDetails_ITILDetails.LoadData();
 		}
    }

    public async Task  Global_SCMRequestTypeId_onitemselected(Entity.Global_SCMRequestType Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
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

    public async Task  ITILCodeIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
    {
        // نمایش / عدم نمایش فیلد منتج از ITIL
        if (Selected.Value.ToString() == "true")
	    {
            await ITILVisible(true);
	    }
	    else
	    {
            await ITILVisible(false); 
	    } 
    }

    public async Task  ResultingFromITIL_onitemselected(dynamic Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
    {
        // نمایش / عدم نمایش فیلد ITIL Detail
        await ITILDetailsVisible(true);

        if (Item.ResultingFromITIL != null)
	    {
	    	Item.RequestIdITIL = Selected.RequestID;
	    	Item.RequesterUserITIL = Selected.UserName;
	    	Item.CreatedAtITIL = Selected.CreateDate;

			Ref_SCMPLATE_ProductRequestDetails_ITILDetails.SetEntity(Item);
			Ref_SCMPLATE_ProductRequestDetails_ITILDetails.LoadData();
	    }
    }

	#endregion FunctionEvents

}
}
