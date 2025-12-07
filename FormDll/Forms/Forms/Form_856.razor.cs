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
    public class Form_856Base : Form_856Peropeties
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

                    //آیا اصلاح گردد؟     
                    Item.HasModification = null;
                    //کالا، مورد تایید است؟
                    Item.GoodsDeliveryApproved = null;
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
        
        int ListCount = List.Count();

        foreach(var Item in List)
        {
            // آیا کالای تحویلی مورد تایید است؟
            if(Item.GoodsDeliveryApproved == null)
            {
	        	IsValid = true;
	        	toastService.ShowError("لطفا گزینه کالا، مورد تایید است؟ را تکمیل نمایید.",
	        	settings => {
	        		settings.Timeout = 4;
	        		settings.ShowProgressBar = true;
	        		settings.PauseProgressOnHover = true;
	        	});
            }
            
            // Console.WriteLine("#log2");
            // Console.WriteLine("#log2-0: " + Item.IsEnableApproved);
            // نیاز به اصلاح دارد یا خیر؟
            if(!(Item.GoodsDeliveryApproved.HasValue && Item.GoodsDeliveryApproved.Value))
            {
                if(Item.HasModification == null)
                {
	            	IsValid = true;
	            	toastService.ShowError("لطفا گزینه آیا اصلاح گردد؟ را تکمیل نمایید.",
	            	settings => {
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
        
		public async Task <bool> GridSCMPLATE_ProductRequestId_303_editmodelsaving(object e   )
        {
            bool IsCancelled = false;

            var Item = (Entity.SCMPLATE_ProductRequestDetails)e;

            // آیا کالای تحویلی مورد تایید است؟
            if(Item.GoodsDeliveryApproved == null)
            {
	        	IsCancelled = true;

	        	toastService.ShowError("لطفا گزینه آیا کالای تحویلی مورد تایید است؟ را تکمیل نمایید.",
	        	settings => {
	        		settings.Timeout = 4;
	        		settings.ShowProgressBar = true;
	        		settings.PauseProgressOnHover = true;
	        	});
            }

            // نیاز به اصلاح دارد یا خیر؟
            if(!(Item.GoodsDeliveryApproved.HasValue && Item.GoodsDeliveryApproved.Value))
            {
                if(Item.HasModification == null)
                {
	            	IsCancelled = true;

	            	toastService.ShowError("لطفا گزینه آیا نیاز به اصلاح دارد؟ را تکمیل نمایید.",
	            	settings => {
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
            //کالا، مورد تایید است؟
            // GoodsDeliveryApproved

            // HasModification
            // آیا اصلاح گردد؟

            // نمایش / عدم نمایش فیلد اصلاح گردد
            var hasModification = Ref_SCMPLATE_ProductRequestDetails_HasModification;
		    //if(!(Item.GoodsDeliveryApproved.HasValue && Item.GoodsDeliveryApproved.Value))
		    if(Item.GoodsDeliveryApproved.HasValue && !Item.GoodsDeliveryApproved.Value)
		    {
		        hasModification.SetVisible(true);
		    }
		    else
		    {
		    	hasModification.SetVisible(false); 
		    }
        }

        public async Task IsEnableTDS_oninput(ChangeEventArgs Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
        {
        }


		public async Task  GoodsDeliveryApproved_oninput(ChangeEventArgs Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
        {
            // نمایش / عدم نمایش فیلد آیا اصلاح گردد؟
            var GoodsDeliveryApproved = Ref_SCMPLATE_ProductRequestDetails_HasModification;
		    if (Selected.Value.ToString() == "false")
		    {
		        GoodsDeliveryApproved.SetVisible(true);
		    }
		    else
		    {
		    	GoodsDeliveryApproved.SetVisible(false); 
		    }
        }

// 		public async Task <bool> GridSCMPLATE_ProductRequestId_303_editmodelsaving(object e   )
//         {

//             return false;
//         }
// public async Task  GridSCMPLATE_ProductRequestId_303_afterrendermodal(Entity.SCMPLATE_ProductRequestDetails Item   )
//         {

            
//         }
// public async Task  GoodsDeliveryApproved_oninput(ChangeEventArgs Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
//         {

            
//         }

		#endregion FunctionEvents

}
}
