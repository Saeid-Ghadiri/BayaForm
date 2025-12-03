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
    public class Form_718Base : Form_718Peropeties
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

        // if (_Entity.ReqCount != 5)
        // {
        //     IsValid = false;
        //     SumaryMessage += "تعداد درخواست مخالف 5 باشد";
        // }


				var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();
				// foreach (var Item in List)
				// {
			    //     if (Item.SupplyGoodsIsEnable == null)
			    //     {
			    //     	IsValid = true;
			    //     	toastService.ShowError("لطفا گزینه نحوه تامین کالا را تکمیل نمایید",
			    //     		settings =>
			    //     		{
			    //     			settings.Timeout = 4;
			    //     			settings.ShowProgressBar = true;
			    //     			settings.PauseProgressOnHover = true;
			    //     		});
			    //     }
				// }

                 StateHasChanged();

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

		public async Task<bool> SCMPLATE_ProductRequestDetails_editmodelsaving(object e   )
        {
            bool IsValid = false;
			var MainModel = (Entity.SCMPLATE_ProductRequestDetails)e;

            // شرط پُر بودن فیلد نام کالا دیزیبل
			if (MainModel.SH_DESC == null)
			{
				IsValid = true;
				toastService.ShowError("لطفا گزینه نام کالا را تکمیل نمایید",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}

			// شرط پُر بودن فیلد تعداد یا مقدار درخواستی
			if (MainModel.ProductRequestingQTY == null || MainModel.ProductRequestingQTY == 0)
			{
				IsValid = true;
				toastService.ShowError("لطفا گزینه تعداد یا مقدار درخواستی را تکمیل نمایید.",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}

            // IsEnableTDS - آیا دارای دیتاشیت است؟
			// if (MainModel.IsEnableTDS == null)
			// {
			// 	IsValid = true;
			// 	toastService.ShowError("لطفا گزینه آیا دیتاشیت دارد؟ را تکمیل نمایید",
			// 		settings => {
			// 			settings.Timeout = 4;
			// 			settings.ShowProgressBar = true;
			// 			settings.PauseProgressOnHover = true;
			// 		});
			// }

            // // SupplyGoodsIsEnable - نحوه تامین کالا
			// if (MainModel.SupplyGoodsIsEnable == null)
			// {
			// 	IsValid = true;
			// 	toastService.ShowError("لطفا گزینه نحوه تامین کالا را تکمیل نمایید",
			// 		settings => {
			// 			settings.Timeout = 4;
			// 			settings.ShowProgressBar = true;
			// 			settings.PauseProgressOnHover = true;
			// 		});
			// }

			return IsValid;
        }
        public async Task SCMPLATE_ProductRequestDetails_customizeeditmodel(GridCustomizeEditModelEventArgs e   )
        {

        }
        public async Task SCMPLATE_ProductRequestDetails_afterrendermodal(Entity.SCMPLATE_ProductRequestDetails Item   )
        {
			// Note: مخفی کردن فیلد بارگذاری فایل تی دی اس، بر اساس فیلد آیا دیتاشیت دارد یا خیر؟ 
			// if (Item.IsEnableTDS.HasValue && Item.IsEnableTDS.Value)
			// {

			// 	Ref_SCMPLATE_ProductRequestDetails_SCMPLATE_ProductRequestDetails_TDSFiles.SetVisible(true);

			// }
			// else
			// {
			// 	Ref_SCMPLATE_ProductRequestDetails_SCMPLATE_ProductRequestDetails_TDSFiles.SetVisible(false);
			// }
        }
        
        public async Task IsEnableTDS_oninput(ChangeEventArgs Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
        {
            // Note: مخفی کردن فیلد بارگذاری فایل تی دی اس، بر اساس فیلد آیا دیتاشیت دارد یا خیر؟
			// var Item2 = Ref_SCMPLATE_ProductRequestDetails_SCMPLATE_ProductRequestDetails_TDSFiles;

			// if (Selected.Value.ToString() == "true")
			// {
			// 	Item2.SetVisible(true);
			// }
			// else
			// {
			// 	Item2.SetVisible(false);
			// }
        }

		#endregion FunctionEvents

}
}
