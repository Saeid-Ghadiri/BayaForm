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
    public class Form_845Base : Form_845Peropeties
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
            // نتیجه مذاکره چیست؟
		    if (Item.ResultNegotiation == null)
            {
                if (Item.IsEnableNeedNegotiated.HasValue && Item.IsEnableNeedNegotiated.Value)
		        {
		        	IsValid = true;

		        	toastService.ShowError("لطفا گزینه نتیجه مذاکره چیست؟ را تکمیل نمایید",
		        		settings =>
		        		{
		        			settings.Timeout = 4;
		        			settings.ShowProgressBar = true;
		        			settings.PauseProgressOnHover = true;
		        		});
		        }
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

		public async Task <bool> GridSCMPLATE_ProductRequestId_editmodelsaving(object e   )
        {

           bool IsCancelled = false;
			var Item = (Entity.SCMPLATE_ProductRequestDetails)e;

			// نتیجه مذاکره چیست؟
		    if (Item.ResultNegotiation == null)
            {
                if (Item.IsEnableNeedNegotiated.HasValue && Item.IsEnableNeedNegotiated.Value)
		        {
		        	IsCancelled = true;

		        	toastService.ShowError("لطفا گزینه نتیجه مذاکره چیست؟ را تکمیل نمایید",
		        		settings =>
		        		{
		        			settings.Timeout = 4;
		        			settings.ShowProgressBar = true;
		        			settings.PauseProgressOnHover = true;
		        		});
		        }
            }

			return IsCancelled;
        }
        
    	public async Task  GridSCMPLATE_ProductRequestId_303_afterrendermodal(Entity.SCMPLATE_ProductRequestDetails Item   )
        {

            // چک باکس تیک مذاکره گروهی از بخش اصلی فرم
            //CheckNegotiatedAgreement
            if (Ref_CheckNegotiatedAgreement.Value == "true")
            {
                Item.ResultNegotiation = true;
                Ref_SCMPLATE_ProductRequestDetails_ResultNegotiation.SetVisible(true);
            }


            //آیا نیاز به مذاکره دارد؟
            //IsEnableNeedNegotiated

            //نتیجه مذاکره چیست؟
            //ResultNegotiation

            // نمایش / عدم نمایش فیلد نتیجه مذاکره چیست؟
            var ResultNegotiation = Ref_SCMPLATE_ProductRequestDetails_ResultNegotiation;
		    //if(!(Item.IsEnableNeedNegotiated.HasValue && Item.IsEnableNeedNegotiated.Value))
		    if(Item.IsEnableNeedNegotiated.HasValue && !Item.IsEnableNeedNegotiated.Value)
		    {
		       ResultNegotiation.SetVisible(true);
		    }
		    else
		    {
		    	ResultNegotiation.SetVisible(false); 
		    }

        }

		public async Task  IsEnableNeedNegotiated_oninput(ChangeEventArgs Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
        {
                //نتیجه مذاکره چیست؟ نمایش / عدم نمایش فیلد
            var IsEnableNeedNegotiated = Ref_SCMPLATE_ProductRequestDetails_ResultNegotiation;
		    if (Selected.Value.ToString() == "false")
		    {
		        IsEnableNeedNegotiated.SetVisible(true);
		    }
		    else
		    {
		    	IsEnableNeedNegotiated.SetVisible(false); 
		    }
        }

		#endregion FunctionEvents

}
}
