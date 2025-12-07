using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using CurrieTechnologies.Razor.SweetAlert2;
using BlazorBootstrap;
using Blazored.Toast.Services;

namespace Forms.Forms
{
    public class Form_374Base : Form_374Peropeties
    {
      
    	// SweetAlert2
		[Inject]
		public SweetAlertService Swal { get; set; }
        

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

        // عطف خرید کالا شماران 
        if(_Entity.KH_TEMPNO == null)
        {
            IsValid = false;
            
            toastService.ShowError("لطفا گزینه عطف خرید کالا شماران  را تکمیل نمایید.",
            settings => {
                settings.Timeout = 4;
                settings.ShowProgressBar = true;
                settings.PauseProgressOnHover = true;
            });
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
        // 
        if(_Entity.KH_TEMPNO != null){
                SweetAlertResult result = await Swal.FireAsync(new SweetAlertOptions
                {
                  Title = "بررسی داده",
                  Text = "شما قبلا با این کد پیگیری یکبار عطف ثبت کرده اید، در صورتی که ردیف هایی از این درخواست ممکن است به صورت بدون استعلام ثبت شده باشد، لطفا مجدد این عطف در شماران را ویرایش کرده و ردیف جدید کالا را برای صدور درخواست خرید صادر نمایید.",
                  Icon = SweetAlertIcon.Warning,
                  ConfirmButtonText = "متوجه شدم!!",
                  ShowCancelButton = false,
                  CancelButtonText = ""
                });
        }
    }

    #region FunctionEvents

    public async Task KH_TempNoNum_NotMapped_onitemselected(dynamic Selected   )
        {
            _Entity.KH_APPROVER = Selected.APPROVER;
            _Entity.KH_CENTCODE = Selected.CENTCODE;
            _Entity.KH_CENTCODE_GUID = Selected.CENTCODE_GUID;
			_Entity.KH_PAYCENTName = Selected.PAYCENTName;
            _Entity.KH_WUSER = Selected.WUSER;
			_Entity.KH_TEMPNO = Selected.TEMPNO;
			_Entity.KH_TempNoNum = Selected.TempNoNum;
			_Entity.KH_ORDERNO = Selected.ORDERNO;
			_Entity.KH_ORDERNO_GUID = Selected.ORDERNO_GUID;
			_Entity.KH_ORDERDATE = Selected.ORDERDATE;
			_Entity.KH_OKFACTDATE = Selected.OKFACTDATE;
            _Entity.KH_INVCODE = Selected.INVCODE;
			_Entity.KH_REQPERSON = Selected.REQPERSON;
			_Entity.KHARID_YEAR = Selected.YEAR;

			 Ref_KH_KharidDTL.LoadData();
        }

		public async Task  ProductName_NotMapped_onitemselected(dynamic Selected ,Entity.SCM_ProductRequestDetails Item  )
        {
            /// <summary>
			/// فیلدهای زیر فیلدهای اصلی برای نمایش در فرم هستند.
			///
			///</summary>

			//Console.WriteLine("start");
			//  نام کالا
			Item.ProductNameText = Selected.DESC;
			//Console.WriteLine(Selected.DESC);
			//  نام دسته بندی فرعی
			Item.ProductSubCategoryText = Selected.SubGroupName;
			//Console.WriteLine(Selected.SubGroupName);
			// کد کالا
			Item.ProductCodeText = Selected.PARTNO;
			// واحد کالا
			Item.ProductUnitText = Selected.UNIT;
			//دسته بندی اصلی کالا
			Item.ProductMainCategoryText = Selected.GroupName;
			// شناسه دسته بندی اصلی کالا
			Item.ProductMainCategoryIdText = Selected.GRCODE;
			// شناسه دسته بندی فرعی
			Item.ProductSubCategoryIdText = Selected.SUBGRCODE;
			// سال مالی شماران
			Item.ShomaranFiscalYearText = Selected.YEAR;
			//کالا موجود است یا خیر
			Item.IsExistText = Selected.IsExist;
			// کد اصلی گروه کالا شماران 
			Item.MapGroupCodeNum = Selected.MapGroupCode;
			// موجودی کالا در شماران
			if (Selected.Amount > -1)
			{
				Item.ProductInventoryText = (double)Selected.Amount;
			}
			//Console.WriteLine("End");
        }

		public async Task <bool> SCM_ProductRequestDetails_editmodelsaving(object e   )
        {
            bool IsCancelled = false;

			var Item = (Entity.SCM_ProductRequestDetails)e;

			// شرط پُر بودن فیلد نام کالا دیزیبل
			if (Item.ProductNameText == null)
			{
				IsCancelled = true;

				toastService.ShowError("لطفا گزینه نام کالا را تکمیل نمایید",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}

            return IsCancelled;
        }
        public async Task  SCM_ProductRequestDetails_afterrendermodal(Entity.SCM_ProductRequestDetails Item   )
        {
        }

		#endregion FunctionEvents

}
}
