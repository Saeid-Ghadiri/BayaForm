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
    public class Form_376Base : Form_376Peropeties
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

        foreach(var Item in List)
        {

            // تعداد یا مقدار واگذاری کالا
            if(Item.NumberofProductDelivery == null)
            {
				IsValid = false;
				toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا را تکمیل نمایید.",
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

    public async Task <bool> SCM_ProductRequestDetails_editmodelsaving(object e   )
    {
        bool IsCancelled = false;
        var EntityDetail = (Entity.SCM_ProductRequestDetails)e;
        
        // تعداد یا مقدار واگذاری کالا
        if(EntityDetail.NumberofProductDelivery == null)
        {
			IsCancelled = true;
			toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا را تکمیل نمایید.",
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
            var NDL1IsVisible = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics;
            var NDL2IsVisible = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2;
            var NDL3IsVisible = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3;

            // فیلدهای انبار - 1 2 3
            var NDL1_Anabar_IsVisible = Ref_SCM_ProductRequestDetails_NumberofProductDelivery;
            var NDL2_Anabar_IsVisible = Ref_SCM_ProductRequestDetails_NumberofProductDelivery2;
            var NDL3_Anabar_IsVisible = Ref_SCM_ProductRequestDetails_NumberofProductDelivery3;

            // Console.WriteLine((Selected.Id.ToString() == "82329c07-2aa7-ef11-8354-005056a02a64").ToString());
            // Console.WriteLine((Selected.Id.ToString() == "cc59710f-2aa7-ef11-8354-005056a02a64").ToString());
            // Console.WriteLine((Selected.Id.ToString() == "13a46c17-2aa7-ef11-8354-005056a02a64").ToString());
            // نمایش تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 
            if (Item.SCM_NumTransfersGoodsWarehouseId.HasValue && Item.SCM_NumTransfersGoodsWarehouseId.ToString() == "82329c07-2aa7-ef11-8354-005056a02a64")
            {
                NDL1IsVisible.SetVisible(true);
                NDL1_Anabar_IsVisible.SetVisible(true);

                NDL2IsVisible.SetVisible(false);
                NDL2_Anabar_IsVisible.SetVisible(false);

                NDL3IsVisible.SetVisible(false);
                NDL3_Anabar_IsVisible.SetVisible(false);
            }
            else if (Item.SCM_NumTransfersGoodsWarehouseId.HasValue && Item.SCM_NumTransfersGoodsWarehouseId.ToString() == "cc59710f-2aa7-ef11-8354-005056a02a64")
            {
                NDL1IsVisible.SetVisible(true);
                NDL1_Anabar_IsVisible.SetVisible(true);

                NDL2IsVisible.SetVisible(true);
                NDL2_Anabar_IsVisible.SetVisible(true);

                NDL3IsVisible.SetVisible(false);
                NDL3_Anabar_IsVisible.SetVisible(false);
            }
            else if (Item.SCM_NumTransfersGoodsWarehouseId.ToString() == "13a46c17-2aa7-ef11-8354-005056a02a64")
            {
                NDL1IsVisible.SetVisible(true);
                NDL1_Anabar_IsVisible.SetVisible(true);

                NDL2IsVisible.SetVisible(true);
                NDL2_Anabar_IsVisible.SetVisible(true);

                NDL3IsVisible.SetVisible(true);
                NDL3_Anabar_IsVisible.SetVisible(true);
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
        }

		public async Task  SCM_NumTransfersGoodsWarehouseId_onitemselected(Entity.SCM_NumTransfersGoodsWarehouse Selected ,Entity.SCM_ProductRequestDetails Item  )
        {
            // 82329c07-2aa7-ef11-8354-005056a02a64     یک مرحله
            // cc59710f-2aa7-ef11-8354-005056a02a64     دو مرحله
            // 13a46c17-2aa7-ef11-8354-005056a02a64     سه مرحله

            // **************************************************

            // تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات- TheNumberDeliveredByLogistics
            // تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 2- TheNumberDeliveredByLogistics2
            // تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 3- TheNumberDeliveredByLogistics3

            var NDL1IsVisible = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics;
            var NDL2IsVisible = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2;
            var NDL3IsVisible = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3;

            // فیلدهای انبار - 1 2 3
            var NDL1_Anabar_IsVisible = Ref_SCM_ProductRequestDetails_NumberofProductDelivery;
            var NDL2_Anabar_IsVisible = Ref_SCM_ProductRequestDetails_NumberofProductDelivery2;
            var NDL3_Anabar_IsVisible = Ref_SCM_ProductRequestDetails_NumberofProductDelivery3;

            // Console.WriteLine((Selected.Id.ToString() == "82329c07-2aa7-ef11-8354-005056a02a64").ToString());
            // Console.WriteLine((Selected.Id.ToString() == "cc59710f-2aa7-ef11-8354-005056a02a64").ToString());
            // Console.WriteLine((Selected.Id.ToString() == "13a46c17-2aa7-ef11-8354-005056a02a64").ToString());
            // نمایش تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 
            if (Selected.Id.ToString() == "82329c07-2aa7-ef11-8354-005056a02a64")
            {
                NDL1IsVisible.SetVisible(true);
                NDL1_Anabar_IsVisible.SetVisible(true);

                NDL2IsVisible.SetVisible(false);
                NDL2_Anabar_IsVisible.SetVisible(false);

                NDL3IsVisible.SetVisible(false);
                NDL3_Anabar_IsVisible.SetVisible(false);
            }
            else if (Selected.Id.ToString() == "cc59710f-2aa7-ef11-8354-005056a02a64")
            {
                NDL1IsVisible.SetVisible(true);
                NDL1_Anabar_IsVisible.SetVisible(true);

                NDL2IsVisible.SetVisible(true);
                NDL2_Anabar_IsVisible.SetVisible(true);

                NDL3IsVisible.SetVisible(false);
                NDL3_Anabar_IsVisible.SetVisible(false);
            }
            else if (Selected.Id.ToString() == "13a46c17-2aa7-ef11-8354-005056a02a64")
            {
                NDL1IsVisible.SetVisible(true);
                NDL1_Anabar_IsVisible.SetVisible(true);

                NDL2IsVisible.SetVisible(true);
                NDL2_Anabar_IsVisible.SetVisible(true);

                NDL3IsVisible.SetVisible(true);
                NDL3_Anabar_IsVisible.SetVisible(true);
            }
        }

		public async Task  SurplusProductIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCM_ProductRequestDetails Item  )
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

		#endregion FunctionEvents

}
}
