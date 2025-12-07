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
    public class Form_450Base : Form_450Peropeties
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
            var List = _Entity.SCMICT_ProductRequestDetails.ToList();
			foreach (var Item in List)
			{
                // InquiryIsEnable
                Item.InquiryIsEnable = null;
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
        var List = _Entity.SCMICT_ProductRequestDetails.ToList();

        foreach (var Item in List)
        {
            // InquiryIsEnable - نیاز به استعلام دارد
            if (Item.InquiryIsEnable == null)
			{
				IsValid = false;
				toastService.ShowError("لطفا گزینه آیا نیاز به استعلام دارد را تکمیل نمایید.",
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

    public async Task  InquiryIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCMICT_ProductRequestDetails Item  )
        {
             // Note: مخفی کردن فیلد کد آی تی آی ال بر اساس فیلد آیا کد دارد؟       
			var Item1 = Ref_SCMICT_ProductRequestDetails_ResultingFromITIL;
			if (Selected.Value.ToString() == "true")
			{
				Item1.SetVisible(true);
			}
			else
			{
				Item1.SetVisible(false);
			}
        }

		public async Task <bool> SCMICT_ProductRequestDetails_editmodelsaving(object e   )
        {
            bool IsValid = false;

			var MainModel = (Entity.SCMICT_ProductRequestDetails)e;

            // ITILCodeIsEnable
			if (MainModel.ITILCodeIsEnable == null)
			{
				IsValid = true;
				toastService.ShowError("لطفا گزینه آیا کد ITIL دارد؟ را تکمیل نمایید.",
					settings =>
					{
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}

              // InquiryIsEnable
                if(MainModel.InquiryIsEnable == null)
                {
                    IsValid = true;
                    toastService.ShowError("لطفا گزینه نیاز به استعلام دارد؟ را تکمیل نمایید.",
                        settings => {
                            settings.Timeout = 4;
                            settings.ShowProgressBar = true;
                            settings.PauseProgressOnHover = true;
                        });
                }
                	return IsValid;
        }
        
        public async Task  SCMICT_ProductRequestDetails_afterrendermodal(Entity.SCMICT_ProductRequestDetails Item   )
        {
              // Note: itil
			if (Item.ITILCodeIsEnable.HasValue && Item.ITILCodeIsEnable.Value)
			{
				Ref_SCMICT_ProductRequestDetails_ResultingFromITIL.SetVisible(true);
			}
			else
			{
				Ref_SCMICT_ProductRequestDetails_ResultingFromITIL.SetVisible(false);
			}

            if (Item.Global_SCMRequestTypeId == null)
            {
                Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(false);
                Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(false);
                Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);  
            }
            else
            {
                // شرط اینکه آیا فرآیند تحویل اجرا گردد؟
                if(Item.Global_SCMRequestTypeId.ToString() =="a9c5df1c-d99c-ef11-8354-005056a02a64")
                {
                    Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(true);
                    Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(false);
                    Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false); 
                }

                // شرط نوع درخواست بر اساس خرید کالا
                if(Item.Global_SCMRequestTypeId.ToString() =="3b9e934d-d99c-ef11-8354-005056a02a64")
                {
                    Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(false);
                    Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(true);
                    Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true); 
                }

                // شرط نوع درخواست بر اساس تحویل و خرید کالا
                if(Item.Global_SCMRequestTypeId.ToString() =="73f6c459-d99c-ef11-8354-005056a02a64")
                {
                    Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(true);
                    Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(true);
                    Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true); 
                }
            }
            
        }

		public async Task  ITILCodeIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCMICT_ProductRequestDetails Item  )
        {
            // Note: مخفی کردن فیلد کد آی تی آی ال بر اساس فیلد آیا کد دارد؟       
			var Item1 = Ref_SCMICT_ProductRequestDetails_ResultingFromITIL;
			if (Selected.Value.ToString() == "true")
			{
				Item1.SetVisible(true);
			}
			else
			{
				Item1.SetVisible(false);
			}
        }

		public async Task  Global_SCMRequestTypeId_onitemselected(dynamic Selected ,Entity.SCMICT_ProductRequestDetails Item  )
        {
            //Console.WriteLine(await Utility.JSON.ToJson(Selected));

            // شرط نوع درخواست بر اساس تحویل کالا
            if(Selected.Id.ToString() =="a9c5df1c-d99c-ef11-8354-005056a02a64")
            {
                // تحویل کالا
                Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(true);
                Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.Value =true;
                // خرید کالا
                Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(false);
                Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.Value=false;
                Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);               
            }
         
            // شرط نوع درخواست بر اساس خرید کالا
            if(Selected.Id.ToString() =="3b9e934d-d99c-ef11-8354-005056a02a64")
            {
                // تحویل کالا
                Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(false);
                Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.Value =false;
                // خرید کالا
                Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(true);
                Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.Value=true;
                if (Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.Value.HasValue && Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.Value.Value)
                {
                    Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true);  
                }
                else
                {
                    Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);  
                }
            }
         
            // شرط نوع درخواست بر اساس تحویل و خرید کالا
            if(Selected.Id.ToString() =="73f6c459-d99c-ef11-8354-005056a02a64")
            {
                // تحویل کالا
                Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(true);    
                Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.Value =true;
                // خرید کالا               
                Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(true);
                Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.Value=true;
                if (Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.Value.HasValue && Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.Value.Value)
                {
                    Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true);  
                }
                else
                {
                    Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);  
                }
            }
            
        }

		#endregion FunctionEvents

}
}
