using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using BlazorBootstrap;
using Blazored.Toast.Services;

namespace Forms.Forms
{
    public class Form_565Base : Form_565Peropeties
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
            // var List = _Entity.SCMPETCO_OS_Details.ToList();

            // foreach (var Item in List)
            // {
            //     if(Item.IsEnableApproved = null)
            //     {
            //         Item
            //     }
            // }
        }
    }

    /// <summary>
    /// اعتبار سنجی فرم
    /// </summary>
    /// <returns></returns>
    public override async Task<bool> FormValidator()
    {
        bool IsValid = true;

        var List = _Entity.SCMPETCO_OS_Details.ToList();

        foreach (var Item in List)
        {
            // IsEnableApproved - آیا مورد تایید است؟
            if (Item.IsEnableApproved == null)
			{
				IsValid = false;
                
				toastService.ShowError("لطفا گزینه آیا مورد تایید است؟ را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}

            // در حالت اصلاح
            if(!(Item.IsEnableApproved.HasValue && Item.IsEnableApproved.Value))
            {
                // آیا این درخواست اصلاح گردد؟
                if (Item.IsEnableNeedModified == null)
			    {
			    	IsValid = false;

			    	toastService.ShowError("لطفا گزینه آیا این درخواست اصلاح گردد؟ را تکمیل نمایید.",
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

        if ((await BeforSubmit()).Status != HttpStatusCode.OK)
        {
            StateHasChanged();
            Result.Status = HttpStatusCode.InternalServerError;
            return Result;
        }

        _loadingService.Show();
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
            _loadingService.Hide();
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

        public async Task ResultingFrom_Code13(bool Visible, bool Value, Entity.SCMPETCO_OS_Details Item)
        {
            // IsEnableDemolitionAndRenovation
            Ref_SCMPETCO_OS_Details_IsEnableDemolitionAndRenovation.SetVisible(Visible);
			Item.IsEnableDemolitionAndRenovation = Value;
            Ref_SCMPETCO_OS_Details_IsEnableDemolitionAndRenovation.SetDisabled(true);
        }


    public async Task<bool> SCMPETCO_OS_Details_editmodelsaving(object e   )
    {
        bool IsCancelled = false;

		var MainModel = (Entity.SCMPETCO_OS_Details)e;

        // IsEnableApproved - آیا مورد تایید است؟
        if (MainModel.IsEnableApproved == null)
		{
			IsCancelled = true;
            
			toastService.ShowError("لطفا گزینه آیا مورد تایید است؟ را تکمیل نمایید.",
			settings => {
				settings.Timeout = 4;
				settings.ShowProgressBar = true;
				settings.PauseProgressOnHover = true;
			});
		}

        // آیا این درخواست اصلاح گردد؟
        if(!(MainModel.IsEnableApproved.HasValue && MainModel.IsEnableApproved.Value))
        {
            if (MainModel.IsEnableNeedModified == null)
		    {
		    	IsCancelled = true;

		    	toastService.ShowError("لطفا گزینه آیا این درخواست اصلاح گردد؟ را تکمیل نمایید.",
		    	settings => {
		    		settings.Timeout = 4;
		    		settings.ShowProgressBar = true;
		    		settings.PauseProgressOnHover = true;
		    	});
		    }
        }

        return IsCancelled;
    }
        
    public async Task  SCMPETCO_OS_Details_afterrendermodal(Entity.SCMPETCO_OS_Details Item)
    {
        Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_UploadFiles.SetDisabled(true);
        Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_SampleGoodsFiles.SetDisabled(true);


        // 
        if (Item.UploadFileIsEnable.HasValue && Item.UploadFileIsEnable.Value)
		{
			Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_UploadFiles.SetVisible(true); 
		}
		else
		{
			Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_UploadFiles.SetVisible(false); 
		}


        // فیلد: منتج از پیمانکار با Code 13
        if( _Entity.SCMPETCO_OS_ResultingFromId.HasValue && _Entity.SCMPETCO_OS_ResultingFromId.ToString() == "2a087a09-9fbe-ef11-a4fa-005056a2b6bd")
        {
            await ResultingFrom_Code13(true, true, Item);
        }
        else
        {
            await ResultingFrom_Code13(false, false, Item);
        }

        // فایل نمونه
        if (Item.IsEnableSampleGoods.HasValue && Item.IsEnableSampleGoods.Value)
		{
			Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_SampleGoodsFiles.SetVisible(true); 
		}
		else
		{
			Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_SampleGoodsFiles.SetVisible(false); 
		}
        Console.WriteLine("Log 0 ");
        Console.WriteLine("- " + Item.IsEnableApproved + " -");
        // در صورتی که مورد تایید نباشد فیلد آیا نیاز به اصلاح گردد.
        if (!(Item.IsEnableApproved.HasValue && Item.IsEnableApproved.Value))
		{
            Console.WriteLine("Log 1 ");
            Console.WriteLine("- " + Ref_SCMPETCO_OS_Details_IsEnableNeedModified + " -");
			Ref_SCMPETCO_OS_Details_IsEnableNeedModified.SetVisible(false); 
		}
		else
		{
            Console.WriteLine("Log 2 ");
			Ref_SCMPETCO_OS_Details_IsEnableNeedModified.SetVisible(true); 
		}
        Console.WriteLine("Log 3 ");
    }

	public async Task  UploadFileIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCMPETCO_OS_Details Item)
    {
        var UploadFilesIsVisible = Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_UploadFiles;

		if (Selected.Value.ToString() == "true")
		{
			UploadFilesIsVisible.SetVisible(true); 
		}
		else
		{
			UploadFilesIsVisible.SetVisible(false); 
		}
    }

	public async Task  SCMPETCO_OS_ResultingFromId_onitemselected(Entity.SCMPETCO_OS_ResultingFrom Selected   )
    {
    }

	public async Task  IsEnableSampleGoods_oninput(ChangeEventArgs Selected ,Entity.SCMPETCO_OS_Details Item  )
    {
        // مخفی کردن فیلد فایل نمونه
		var SampleFileIsVisible = Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_SampleGoodsFiles;
        
		if (Selected.Value.ToString() == "true")
		{
			SampleFileIsVisible.SetVisible(true); 
		}
		else
		{
			SampleFileIsVisible.SetVisible(false); 
		}            
    }


        // آیا مورد تایید است؟
		public async Task  IsEnableApproved_oninput(ChangeEventArgs Selected ,Entity.SCMPETCO_OS_Details Item  )
        {
            var NeedModifiedIsVisible = Ref_SCMPETCO_OS_Details_IsEnableNeedModified;

            if (Selected.Value.ToString() == "false")
		    {
		    	NeedModifiedIsVisible.SetVisible(true); 
		    }
		    else
		    {
		    	NeedModifiedIsVisible.SetVisible(false); 
		    }
        }

		#endregion FunctionEvents

}
}
