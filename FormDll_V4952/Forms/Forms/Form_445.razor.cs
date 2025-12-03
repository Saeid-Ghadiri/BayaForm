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
    public class Form_445Base : Form_445Peropeties
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
            var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();

			foreach (var Item in List)
			{
                // نحوه تامین کالا
                Item.SupplyGoodsIsEnable = null;              
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
                // Global_SCMRequestTypeId - مشخص کردن وضعیت یک درخواست زنجیره تامین 
                if (Item.Global_SCMRequestTypeId == null)
				{
					IsValid = false;
					toastService.ShowError("یک نوع درخواست انتخاب کنید.",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
				}

                // SupplyGoodsIsEnable - نحوه تامین کالا 
                if (Item.SupplyGoodsIsEnable == null)
				{
					IsValid = false;
					toastService.ShowError("لطفا گزینه نحوه تامین کالا تکمیل گردد",
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
        #region DateTimeGetDeliveryCode
        // تبدیل تاریخ و تکمیل فیلد
	    for (int i = 0; i < _Entity.SCMPLATE_ProductRequestDetails.Count; i++)
	    {
	    	var item = _Entity.SCMPLATE_ProductRequestDetails.ToList()[i];
	    	if (item.GetDeliveryCode != null)
	    	{
	    		// تبدیل تاریخ شمسی به میلادی            
	    		System.Globalization.PersianCalendar PC = new System.Globalization.PersianCalendar();  
	    		var DateNow = DateTime.Now;
	    		// تاریخ شمسی پر می شود
	    		item.DateTimeDeliveryCode = PC.GetYear(DateNow) + "/" + PC.GetMonth(DateNow).ToString("0#") + "/" + PC.GetDayOfMonth(DateNow).ToString("0#");
	    	}
	    }
        #endregion  / DateTimeGetDeliveryCode
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

        public async Task<bool> SCMPLATE_ProductRequestDetails_editmodelsaving(object e   )
        {
            bool IsValid = false;
            
			var MainModel = (Entity.SCMPLATE_ProductRequestDetails)e;


		    // Note نوع درخواست
		    if (MainModel.Global_SCMRequestTypeId == null)
		    {
		    	IsValid = true;
		    	toastService.ShowError("لطفا گزینه  نوع درخواست را تکمیل نمایید.",
		    		settings =>
		    		{
		    			settings.Timeout = 4;
		    			settings.ShowProgressBar = true;
		    			settings.PauseProgressOnHover = true;
		    		});
		    }

		    // Note نحوه تامین کالا		
		    if (MainModel.SupplyGoodsIsEnable == null)
		    {
		    	IsValid = true;
		    	toastService.ShowError("لطفا گزینه نحوه تامین کالا تکمیل گردد.",
		    		settings =>
		    		{
		    			settings.Timeout = 4;
		    			settings.ShowProgressBar = true;
		    			settings.PauseProgressOnHover = true;
		    		});
		    }

			return IsValid;
        }

        public async Task SCMPLATE_ProductRequestDetails_customizeeditmodel(GridCustomizeEditModelEventArgs e   )
        {
            // var Item = (Entity.SCMPLATE_ProductRequestDetails)e.EditModel;
			// if (e.IsNew)
			// {
            //     // آیا تامین کسری انجام شود؟
            //     Item.DeficitSupplyIsEnable = true;
            //     // آیا فرآیند تحویل اجرا گردد؟
            //     Item.GoodsDeliveryIsEnable = true;
			// }
            // StateHasChanged();
        }
        public async Task SCMPLATE_ProductRequestDetails_afterrendermodal(Entity.SCMPLATE_ProductRequestDetails Item   )
        {
            // Note: مخفی کردن فیلد تعداد تامین کسری بر اساس فیلد آیا تامین کسری دارد؟ و آیا نیاز به استعلام دارد یا خیر؟
			if (Item.DeficitSupplyIsEnable.HasValue && Item.DeficitSupplyIsEnable.Value)
			{
				Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true);
			}
			else
			{
				Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);
			}

            // در حالت نال هر دو فقط مخفی باشد
            if (Item.Global_SCMRequestTypeId == null)
            {
                Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(false);
                Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(false);
                Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);
            }
            else
            {
                // شرط اینکه آیا فرآیند تحویل اجرا گردد؟
                if(Item.Global_SCMRequestTypeId.ToString() =="a9c5df1c-d99c-ef11-8354-005056a02a64")
                {
                    Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(true);
                    Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(false);
                }

                // شرط نوع درخواست بر اساس خرید کالا
                if(Item.Global_SCMRequestTypeId.ToString() =="3b9e934d-d99c-ef11-8354-005056a02a64")
                {
                    Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(false);
                    Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(true);
                }

                // شرط نوع درخواست بر اساس تحویل و خرید کالا
                if(Item.Global_SCMRequestTypeId.ToString() =="73f6c459-d99c-ef11-8354-005056a02a64")
                {
                    Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(true);
                    Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(true);
                }
            }


        }

		public async Task DeficitSupplyIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
        {
            // Note: مخفی کردن فیلد تعداد تامین کسری بر اساس فیلد آیا تامین کسری دارد؟       
			var Item1 = Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyNumber;
			if (Selected.Value.ToString() == "true")
			{
				Item1.Visible = true;
			}
			else
			{
				Item1.Visible = false;
			}
        }
		public async Task Global_SCMRequestTypeId_onitemselected(dynamic Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
        {          
            //Console.WriteLine(await Utility.JSON.ToJson(Selected));

            // شرط نوع درخواست بر اساس تحویل کالا
            if(Selected.Id.ToString() =="a9c5df1c-d99c-ef11-8354-005056a02a64")
            {
                // تحویل
                Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(true);
                Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryIsEnable.Value =true;
                // خرید
                Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(false);
                Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.Value =false;
                Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);               
            }
         
            // شرط نوع درخواست بر اساس خرید کالا
            if(Selected.Id.ToString() =="3b9e934d-d99c-ef11-8354-005056a02a64")
            {
                // تحویل
                Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(false);
                Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryIsEnable.Value =false;
                // خرید
                Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.Value =true;
                Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(true);              
                if (Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.Value.HasValue && Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.Value.Value )
                {
                    Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true); 
                }
                else{
                    Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);
                }
            }
         
            // شرط نوع درخواست بر اساس تحویل و خرید کالا
            if(Selected.Id.ToString() =="73f6c459-d99c-ef11-8354-005056a02a64")
            {
                // تحویل
                Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(true);
                Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryIsEnable.Value =true;
                // خرید
                Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.Value =true;               
                Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(true);                             
                if (Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.Value.HasValue && Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.Value.Value)
                {
                    Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true);
                }
                else
                {
                    Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false); 
                }
            }
        }

		public async Task InsertDeliveryCode_oninput(ChangeEventArgs Selected   )
        {
			var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();
			foreach (var Item in List)
			{
				Item.GetDeliveryCode = int.Parse(Selected.Value.ToString());
			}
			StateHasChanged();
        }

		#endregion FunctionEvents

}
}
