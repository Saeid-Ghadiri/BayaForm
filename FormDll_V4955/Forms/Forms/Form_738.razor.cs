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
    public class Form_738Base : Form_738Peropeties
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

		var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();

        //Console.WriteLine("#Log FormValidator :");

        // دکمه ثبت و ادامه
        if (BtnWorkFlowId == "SequenceFlow_10corue")
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
    public async Task<bool> CheckFieldValidation(Entity.SCMPLATE_ProductRequestDetails Item)
    {
        bool IsValid = true;
    
        var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();
    
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

        return IsValid;
    }


		public async Task  ProductSearch_NotMapped_onitemselected(dynamic Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
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

		public async Task <bool> GridSCMPLATE_ProductRequestId_editmodelsaving(object e   )
        {
            bool IsCancelled = false;
			var Item = (Entity.SCMPLATE_ProductRequestDetails)e;

            IsCancelled = !await CheckFieldValidation(Item);

			return IsCancelled;
        }
        
        public async Task  GridSCMPLATE_ProductRequestId_afterrendermodal(Entity.SCMPLATE_ProductRequestDetails Item   )
        {
        }

		#endregion FunctionEvents

}
}
