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
    public class Form_1156Base : Form_1156Peropeties
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

        var List = _Entity.SCMPLATE_OS_Details.ToList();

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

    public async Task ResultingFrom_Code13(bool Visible, bool Value, Entity.SCMPLATE_OS_Details Item)
        {
            // IsEnableDemolitionAndRenovation
            Ref_SCMPLATE_OS_Details_IsEnableDemolitionAndRenovation.SetVisible(Visible);
			Item.IsEnableDemolitionAndRenovation = Value;
            Ref_SCMPLATE_OS_Details_IsEnableDemolitionAndRenovation.SetDisabled(true);
        }

    public async Task  IsEnableApproved_oninput(ChangeEventArgs Selected ,Entity.SCMPLATE_OS_Details Item  )
        {
            var NeedModifiedIsVisible = Ref_SCMPLATE_OS_Details_IsEnableNeedModified;
        
        if (Selected.Value.ToString() == "false")
		{
			NeedModifiedIsVisible.SetVisible(true); 
		}
		else
		{
			NeedModifiedIsVisible.SetVisible(false); 
		}
            
        }

		public async Task <bool> GridSCMPLATE_OS_MasterId_863_editmodelsaving(object e   )
        {

             bool IsCancelled = false;

		var MainModel = (Entity.SCMPLATE_OS_Details)e;

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

        return false;
        }
public async Task  GridSCMPLATE_OS_MasterId_863_afterrendermodal(Entity.SCMPLATE_OS_Details Item   )
        {

            Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_UploadFiles.SetDisabled(true);
        Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_SampleGoodsFiles.SetDisabled(true);


        // 
        if (Item.UploadFileIsEnable.HasValue && Item.UploadFileIsEnable.Value)
		{
			Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_UploadFiles.SetVisible(true); 
		}
		else
		{
			Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_UploadFiles.SetVisible(false); 
		}


        // فیلد: منتج از پیمانکار با Code 13
        if( _Entity.SCMPLATE_OS_ResultingFromId.HasValue && _Entity.SCMPLATE_OS_ResultingFromId.ToString() == "2a087a09-9fbe-ef11-a4fa-005056a2b6bd")
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
			Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_SampleGoodsFiles.SetVisible(true); 
		}
		else
		{
			Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_SampleGoodsFiles.SetVisible(false); 
		}
        Console.WriteLine("Log 0 ");
        Console.WriteLine("- " + Item.IsEnableApproved + " -");
        // در صورتی که مورد تایید نباشد فیلد آیا نیاز به اصلاح گردد.
        if (!(Item.IsEnableApproved.HasValue && Item.IsEnableApproved.Value))
		{
            Console.WriteLine("Log 1 ");
            Console.WriteLine("- " + Ref_SCMPLATE_OS_Details_IsEnableNeedModified + " -");
			Ref_SCMPLATE_OS_Details_IsEnableNeedModified.SetVisible(true); 
		}
		else
		{
            Console.WriteLine("Log 2 ");
			Ref_SCMPLATE_OS_Details_IsEnableNeedModified.SetVisible(false); 
		}
        Console.WriteLine("Log 3 ");
        }

		#endregion FunctionEvents

}
}
