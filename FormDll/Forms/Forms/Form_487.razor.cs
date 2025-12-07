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
    public class Form_487Base : Form_487Peropeties
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

			var List = _Entity.SCM_ProductRequestDetails.ToList();
			foreach (var Item in List)
			{
                // Note: 
                if (Item.SCM_NumTransfersGoodsWarehouseId == null || Item.SCM_NumTransfersGoodsWarehouseId == Guid.Empty)
				{
					IsValid = false;
					toastService.ShowError("یک نوع از مراحل تحویل کالا به انبار را انتخاب کنید.",
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

    	public async Task<bool> SCM_ProductRequestDetails_editmodelsaving(object e   )
        {
            bool IsValid = false;
			var MainModel = (Entity.SCM_ProductRequestDetails)e;

            // حالت پیش فرض فیلد آیا مازاد دارد؟
            Ref_SCM_ProductRequestDetails_SurplusProductIsEnable.Value = false;

			// Note: تعداد یا مقدار واگذاری تدارکات به انبار
			if (MainModel.TheNumberDeliveredByLogistics == null)
			{
				IsValid = true;
				toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات را تکمیل نمایید",
					settings =>
					{
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}
            if (MainModel.SCM_NumTransfersGoodsWarehouseId == null)
			{
				IsValid = true;
				toastService.ShowError("تعداد مراحل واگذاری کالا به انبار را تکمیل نمایید",
					settings =>
					{
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}
            
			// // Note: آیا تامین کسری انجام شود؟
			// if (MainModel.FutureActionTrueFalse == null)
			// {
			// 	IsValid = true;
			// 	toastService.ShowError("لطفا گزینه آیا تامین کسری انجام شود؟ تکمیل نمایید.",
			// 		settings =>
			// 		{
			// 			settings.Timeout = 4;
			// 			settings.ShowProgressBar = true;
			// 			settings.PauseProgressOnHover = true;
			// 		});
			// }

			// if (MainModel.RepeatDeliveryProductIsEnable != null && MainModel.RepeatDeliveryProductIsEnable == true)
			// {
			// 	// Note: آیا تعداد یا مقدار واگذاری به انبار چند مرحله ایست؟
			// 	if (MainModel.RepeatDeliveryProductIsEnable == null || MainModel.DeficitSupplyNumber == 0)
			// 	{
			// 		IsValid = true;
			// 		toastService.ShowError("لطفا گزینه تعداد تامین کسری تکمیل نمایید.",
			// 			settings =>
			// 			{
			// 				settings.Timeout = 4;
			// 				settings.ShowProgressBar = true;
			// 				settings.PauseProgressOnHover = true;
			// 			});
			// 	}
			// }

			return IsValid;
        }

        public async Task SurplusProductIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCM_ProductRequestDetails Item  )
        {
			// Note: آیا خرید تعداد /مقدار مازاد دارد؟
	        var Item2 = Ref_SCM_ProductRequestDetails_NumberOfSurplusProduct;

			if (Selected.Value.ToString() == "true")
			{
				Item2.SetVisible(true);
			}
			else
			{
				Item2.SetVisible(false);
			}
        }

		public async Task RepeatDeliveryProductIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCM_ProductRequestDetails Item  )
        {
			var Item2 = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2;

			if (Selected.Value.ToString() == "true")
			{
				Item2.SetVisible(true);
			}
			else
			{
				Item2.SetVisible(false);
			}
        }

		public async Task ProductDataSheetTrueFalse_oninput(ChangeEventArgs Selected ,Entity.SCM_ProductRequestDetails Item  )
        {
			// Note: مخفی کردن فیلد بارگذاری فایل تی دی اس، بر اساس فیلد آیا دیتاشیت دارد یا خیر؟
			var Item2 = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductDataSheetFile;

			if (Selected.Value.ToString() == "true")
			{
				Item2.SetVisible(true);
			}
			else
			{
				Item2.SetVisible(false);
			}
        }
      
        public async Task SCM_ProductRequestDetails_afterrendermodal(Entity.SCM_ProductRequestDetails Item   )
        {
			// Note: مخفی کردن فیلد بارگذاری فایل تی دی اس، بر اساس فیلد آیا دیتاشیت دارد یا خیر؟ 
			if (Item.ProductDataSheetTrueFalse.HasValue && Item.ProductDataSheetTrueFalse.Value)
			{
				Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductDataSheetFile.SetVisible(true);
			}
			else
			{
				Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductDataSheetFile.SetVisible(false);
			}



            // مخفی کردن فیلد تعداد یا مقدار مازاد
			if (Item.SurplusProductIsEnable.HasValue && Item.SurplusProductIsEnable.Value)
			{
				Ref_SCM_ProductRequestDetails_NumberOfSurplusProduct.SetVisible(true);
			}
			else
			{
				Ref_SCM_ProductRequestDetails_NumberOfSurplusProduct.SetVisible(false);
			}
            

            //Console.WriteLine("log 1 SCM_NumTransfersGoodsWarehouseId");
            // در حالت نال فیلدها مخفی باشد
            if (Item.SCM_NumTransfersGoodsWarehouseId == null || Item.SCM_NumTransfersGoodsWarehouseId == Guid.Empty)
            {
                //Console.WriteLine("log 2 SCM_NumTransfersGoodsWarehouseId");
                // تعداد یا مقدار واگذاری کالا 1
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics.SetVisible(false);
                // تعداد یا مقدار واگذاری کالا 2
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2.SetVisible(false);
                // تعداد یا مقدار واگذاری کالا 3
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3.SetVisible(false); 
            }else   
            {
            
                // شرط یک مرحله ای    
                if(Item.SCM_NumTransfersGoodsWarehouse.code.ToString() == "1001")
                {
			    	// تعداد یا مقدار واگذاری کالا 1
                    Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 2
                    Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2.SetVisible(false);
                    // تعداد یا مقدار واگذاری کالا 3
                    Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3.SetVisible(false);
                }

                // شرط دو مرحله ای
                if(Item.SCM_NumTransfersGoodsWarehouse.code.ToString() == "1002")
                {
			    	// تعداد یا مقدار واگذاری کالا 1
                    Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 2
                    Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 3
                    Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3.SetVisible(false);
                }

                // شرط سه مرحله ای
                if( Item.SCM_NumTransfersGoodsWarehouse.code.ToString() == "1003")
                {
			    	// تعداد یا مقدار واگذاری کالا 1
                    Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 2
                    Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 3
                    Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3.SetVisible(true);          
                }
            }
        }

		public async Task SCM_NumTransfersGoodsWarehouseId_onitemselected(Entity.SCM_NumTransfersGoodsWarehouse Selected ,Entity.SCM_ProductRequestDetails Item  )
        {
            // شرط یک مرحله ای
            if(Selected.code.ToString() == "1001")
            {
				// تعداد یا مقدار واگذاری کالا 1
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics.SetVisible(true);
                // تعداد یا مقدار واگذاری کالا 2
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2.SetVisible(false);
                // تعداد یا مقدار واگذاری کالا 3
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3.SetVisible(false);           
            }
            
            // شرط دو مرحله ای
            if(Selected.code.ToString() == "1002")
            {
				// تعداد یا مقدار واگذاری کالا 1
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics.SetVisible(true);
                // تعداد یا مقدار واگذاری کالا 2
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2.SetVisible(true);
                // تعداد یا مقدار واگذاری کالا 3
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3.SetVisible(false);
            }
            
            // شرط سه مرحله ای
            if(Selected.code.ToString() == "1003")
            {
				// تعداد یا مقدار واگذاری کالا 1
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics.SetVisible(true);
                // تعداد یا مقدار واگذاری کالا 2
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2.SetVisible(true);
                // تعداد یا مقدار واگذاری کالا 3
                Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3.SetVisible(true);        
            }
        }

		#endregion FunctionEvents

}
}
