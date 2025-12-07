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
    public class Form_713Base : Form_713Peropeties
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
            // توضیحات تایید مدیر عامل
		    if (Item.HasInquiryApproved.HasValue && Item.HasInquiryApproved.Value)
            {
                if (Item.DescriptionInquiryApproved == null)
		        {
		        	IsValid = true;

		        	toastService.ShowError("لطفا گزینه توضیحات تایید استعلام را تکمیل نمایید",
		        		settings =>
		        		{
		        			settings.Timeout = 4;
		        			settings.ShowProgressBar = true;
		        			settings.PauseProgressOnHover = true;
		        		});
		        }
            }

            // آیا نیاز به مذاکره دارد؟
		    if (Item.IsEnableNeedNegotiated == null)
		    {
		    	IsValid = true;
		    	toastService.ShowError("لطفا گزینه آیا نیاز به مذاکره دارد؟ را تکمیل نمایید",
		    		settings =>
		    		{
		    			settings.Timeout = 4;
		    			settings.ShowProgressBar = true;
		    			settings.PauseProgressOnHover = true;
		    		});
		    }
            
		

            //استعلام مورد تایید است؟
		    if (Item.HasInquiryApproved == null)
		    {
		    	IsValid = true;

		    	toastService.ShowError("لطفا گزینه استعلام مورد تایید است؟ را تکمیل نمایید",
		    		settings =>
		    		{
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

		public async Task <bool> GridSCMPLATE_ProductRequestId_editmodelsaving(object e   )
        {

           bool IsCancelled = false;
			var Item = (Entity.SCMPLATE_ProductRequestDetails)e;

			// توضیحات تایید مدیر عامل
		    if (Item.HasInquiryApproved.HasValue && Item.HasInquiryApproved.Value)
            {
                if (Item.DescriptionInquiryApproved == null)
		        {
		        	IsCancelled = true;

		        	toastService.ShowError("لطفا گزینه توضیحات تایید استعلام را تکمیل نمایید",
		        		settings =>
		        		{
		        			settings.Timeout = 4;
		        			settings.ShowProgressBar = true;
		        			settings.PauseProgressOnHover = true;
		        		});
		        }
            }



            //آیا نیاز به مذاکره دارد؟
		    if (Item.IsEnableNeedNegotiated == null)
		    {
		    	IsCancelled = true;

		    	toastService.ShowError("لطفا گزینه آیا نیاز به مذاکره دارد؟ را تکمیل نمایید",
		    		settings =>
		    		{
		    			settings.Timeout = 4;
		    			settings.ShowProgressBar = true;
		    			settings.PauseProgressOnHover = true;
		    		});
		    }

            //استعلام مورد تایید است؟
		    if (Item.HasInquiryApproved == null)
		    {
		    	IsCancelled = true;

		    	toastService.ShowError("لطفا گزینه استعلام مورد تایید است؟ را تکمیل نمایید",
		    		settings =>
		    		{
		    			settings.Timeout = 4;
		    			settings.ShowProgressBar = true;
		    			settings.PauseProgressOnHover = true;
		    		});

            }


			return IsCancelled;
        }
        
        public async Task GridSCMPLATE_ProductRequestId_afterrendermodal(Entity.SCMPLATE_ProductRequestDetails Item   )
        {
            // چک باکس تیک مذاکره گروهی از بخش اصلی فرم
            if (Ref_CheckCEO_Plate.Value == "True")
            {
                Ref_SCMPLATE_ProductRequestDetails_IsEnableNeedNegotiated.SetVisible(true);
                Ref_SCMPLATE_ProductRequestDetails_IsEnableNeedNegotiated.Value = true;

                
            }
            Console.WriteLine(Item.HasInquiryApproved?.ToString());
                        // استعلام مورد تایید است؟
            var UF1IsVisible = Ref_SCMPLATE_ProductRequestDetails_Inquiry1Approved;
            var UF2IsVisible = Ref_SCMPLATE_ProductRequestDetails_Inquiry2Approved;
            var UF3IsVisible = Ref_SCMPLATE_ProductRequestDetails_Inquiry3Approved;
            var IDescIsVisible = Ref_SCMPLATE_ProductRequestDetails_DescriptionInquiryApproved;
            
		    if (Item.HasInquiryApproved.HasValue && Item.HasInquiryApproved.Value)
		    {
		        UF1IsVisible.SetVisible(true);
		        UF2IsVisible.SetVisible(true);
		        UF3IsVisible.SetVisible(true);
                IDescIsVisible.SetVisible(true);
               
		    }
		    else
		    {
		    	UF1IsVisible.SetVisible(false);
		    	UF2IsVisible.SetVisible(false);
		    	UF3IsVisible.SetVisible(false);
                IDescIsVisible.SetVisible(false);
                
		    }
        }

		public async Task  HasInquiryApproved_oninput(ChangeEventArgs Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
        {
            // استعلام مورد تایید است؟
            var UF1IsVisible = Ref_SCMPLATE_ProductRequestDetails_Inquiry1Approved;
            var UF2IsVisible = Ref_SCMPLATE_ProductRequestDetails_Inquiry2Approved;
            var UF3IsVisible = Ref_SCMPLATE_ProductRequestDetails_Inquiry3Approved;
            var IDescIsVisible = Ref_SCMPLATE_ProductRequestDetails_DescriptionInquiryApproved;
            
            Console.WriteLine(Selected.Value?.ToString());

		    if (Convert.ToBoolean(Selected.Value))
		    {
		        Ref_SCMPLATE_ProductRequestDetails_Inquiry1Approved.SetVisible(true);
		        Ref_SCMPLATE_ProductRequestDetails_Inquiry2Approved.SetVisible(true);
		        Ref_SCMPLATE_ProductRequestDetails_Inquiry3Approved.SetVisible(true);
                Ref_SCMPLATE_ProductRequestDetails_DescriptionInquiryApproved.SetVisible(true);
               
		    }
		    else
		    {
		    	Ref_SCMPLATE_ProductRequestDetails_Inquiry1Approved.SetVisible(false);
		        Ref_SCMPLATE_ProductRequestDetails_Inquiry2Approved.SetVisible(false);
		        Ref_SCMPLATE_ProductRequestDetails_Inquiry3Approved.SetVisible(false);
                Ref_SCMPLATE_ProductRequestDetails_DescriptionInquiryApproved.SetVisible(false);
                
		    }

            StateHasChanged();
        }

		#endregion FunctionEvents

}
}
