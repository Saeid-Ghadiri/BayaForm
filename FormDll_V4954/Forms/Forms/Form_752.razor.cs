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
    public class Form_752Base : Form_752Peropeties
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
		
        foreach (var Item in List)
		{
            // KH_ORDERNO_GUID - شناسه درخواست خرید
            if (Item.KH_ORDERNO_GUID == null)
			{
				IsValid = false;

				toastService.ShowError("لطفا از طریق جستجو شناسه درخواست خرید را تکمیل نمایید",
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

     // عطف خرید کالا در شماران سیستم
        public async Task KharidIsVisible(bool Visible)
        {
            // Console.WriteLine("#Log 2");
            // Console.WriteLine("#Log 2.1" + Ref_SCMPLATE_ProductRequestDetails_KH_Search_NotMapped.Value);
            await Task.Delay(100);

            Ref_SCMPLATE_ProductRequestDetails_KH_Search_NotMapped.SetVisible(Visible);

            Ref_SCMPLATE_ProductRequestDetails_KH_APPROVER.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_KH_CENTCODE.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_KH_CENTCODE.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_KH_CENTCODE_GUID.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_KH_PAYCENTName.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_KH_WUSER.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_KH_TEMPNO.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_KH_TempNoNum.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_KH_ORDERNO.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_KH_ORDERNO_GUID.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_KH_ORDERDATE.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_KH_OKFACTDATE.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_KH_INVCODE.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_KH_REQPERSON.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_KH_YEAR.SetVisible(Visible);
            // جزئیات خرید کالا
            Ref_SCMPLATE_ProductRequestDetails_SH_Kharid_DTL.SetVisible(Visible);

            Ref_SCMPLATE_ProductRequestDetails_KH_APPROVER.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_KH_CENTCODE.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_KH_CENTCODE.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_KH_CENTCODE_GUID.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_KH_PAYCENTName.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_KH_WUSER.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_KH_TEMPNO.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_KH_TempNoNum.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_KH_ORDERNO.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_KH_ORDERNO_GUID.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_KH_ORDERDATE.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_KH_OKFACTDATE.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_KH_INVCODE.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_KH_REQPERSON.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_KH_YEAR.SetDisabled(true);
        }

        public async Task KharidIsNull()
        {
            Ref_SCMPLATE_ProductRequestDetails_KH_Search_NotMapped = null;
            Ref_SCMPLATE_ProductRequestDetails_KH_APPROVER = null;
            Ref_SCMPLATE_ProductRequestDetails_KH_CENTCODE = null;
            Ref_SCMPLATE_ProductRequestDetails_KH_CENTCODE = null;
            Ref_SCMPLATE_ProductRequestDetails_KH_CENTCODE_GUID = null;
            Ref_SCMPLATE_ProductRequestDetails_KH_PAYCENTName = null;
            Ref_SCMPLATE_ProductRequestDetails_KH_WUSER = null;
            Ref_SCMPLATE_ProductRequestDetails_KH_TEMPNO = null;
            Ref_SCMPLATE_ProductRequestDetails_KH_TempNoNum = null;
            Ref_SCMPLATE_ProductRequestDetails_KH_ORDERNO = null;
            Ref_SCMPLATE_ProductRequestDetails_KH_ORDERNO_GUID = null;
            Ref_SCMPLATE_ProductRequestDetails_KH_ORDERDATE = null;
            Ref_SCMPLATE_ProductRequestDetails_KH_OKFACTDATE = null;
            Ref_SCMPLATE_ProductRequestDetails_KH_INVCODE = null;
            Ref_SCMPLATE_ProductRequestDetails_KH_REQPERSON = null;
            Ref_SCMPLATE_ProductRequestDetails_KH_YEAR = null;
            Ref_SCMPLATE_ProductRequestDetails_SH_Kharid_DTL = null;
        }

        public async Task KharidSetShomaran(dynamic Selected, Entity.SCMPLATE_ProductRequestDetails Item)
        {
            //Console.WriteLine("#Log 3");
            //Console.WriteLine(await Utility.JSON.ToJson(Selected));

            Item.KH_TEMPNO = Selected.TEMPNO;
            Item.KH_PAYCENTName = Selected.PAYCENTName;
            Item.KH_CENTCODE = Selected.CENTCODE;
            Item.KH_CENTCODE_GUID = Selected.CENTCODE_GUID;
            Item.KH_REQPERSON = Selected.REQPERSON;
            Item.KH_WUSER = Selected.WUSER;
            Item.KH_APPROVER = Selected.APPROVER;
            Item.KH_ORDERDATE = Selected.ORDERDATE;
            Item.KH_OKFACTDATE = Selected.OKFACTDATE;
            Item.KH_ORDERNO = Selected.ORDERNO;
            Item.KH_ORDERNO_GUID = Selected.ORDERNO_GUID;
            Item.KH_TempNoNum = Selected.TempNoNum;
            Item.KH_YEAR = Selected.YEAR;
            Item.KH_INVCODE = Selected.INVCODE;

            // فراخوانی داده از dropdown TempNoNum برای گرید داده های آن
            Ref_SCMPLATE_ProductRequestDetails_SH_Kharid_DTL.SetEntity(Item);
            Ref_SCMPLATE_ProductRequestDetails_SH_Kharid_DTL.LoadData();
        }

		public async Task<bool> SCMPLATE_ProductRequestDetails_editmodelsaving(object e   )
        {
            bool IsCancelled = false;
			var Item = (Entity.SCMPLATE_ProductRequestDetails)e;

            // شرط پُر بودن فیلد نام کالا دیزیبل
			if (Item.SH_DESC == null)
			{
				IsCancelled = true;

				toastService.ShowError("لطفا گزینه نام کالا را تکمیل نمایید",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}

            // KH_ORDERNO_GUID - شناسه درخواست خرید
            if (Item.KH_ORDERNO_GUID == null)
			{
				IsCancelled = true;

				toastService.ShowError("لطفا از طریق جستجو شناسه درخواست خرید را تکمیل نمایید",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}

			return IsCancelled;
        }
        
        public async Task SCMPLATE_ProductRequestDetails_afterrendermodal(Entity.SCMPLATE_ProductRequestDetails Item   )
        {
            if (Item.KH_ORDERNO_GUID.HasValue)
            {
                Ref_SCMPLATE_ProductRequestDetails_SH_Kharid_DTL.SetEntity(Item);
                Ref_SCMPLATE_ProductRequestDetails_SH_Kharid_DTL.LoadData();
            }
        }

		public async Task  KH_Search_NotMapped_onitemselected(dynamic Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
        {
            await KharidSetShomaran(Selected, Item);
        }

		#endregion FunctionEvents

}
}
