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
    public class Form_1148Base : Form_1148Peropeties
    {

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
            // InquiryIsEnable - نیاز به استعلام دارد
            if (Item.InquiryIsEnable == null)
			{
				IsValid = false;
                
				toastService.ShowError("لطفا گزینه آیا نیاز به استعلام دارد؟ را تکمیل نمایید.",
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
     public async Task InquiryIsVisible(bool Visible)
    {
        // استعلام شماره 1
        Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_InquiryFile1.SetVisible(Visible);
        // استعلام شماره 2
        Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_InquiryFile2.SetVisible(Visible);
        // استعلام شماره 3
        Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_InquiryFile3.SetVisible(Visible);
    }


    public async Task <bool> GridSCMPLATE_OS_MasterId_863_editmodelsaving(object e   )
        {

           bool IsCancelled = false;

		var MainModel = (Entity.SCMPLATE_OS_Details)e;

        // InquiryIsEnable - نیاز به استعلام دارد
        if (MainModel.InquiryIsEnable == null)
		{
			IsCancelled = true;
            
			toastService.ShowError("لطفا گزینه آیا نیاز به استعلام دارد؟ را تکمیل نمایید.",
			settings => {
				settings.Timeout = 4;
				settings.ShowProgressBar = true;
				settings.PauseProgressOnHover = true;
			});
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

        if (Item.IsEnableSampleGoods.HasValue && Item.IsEnableSampleGoods.Value)
		{
			Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_SampleGoodsFiles.SetVisible(true); 
		}
		else
		{
			Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_SampleGoodsFiles.SetVisible(false); 
		}

        // نیاز به استعلام دارد؟
        if(Item.InquiryIsEnable.HasValue && Item.InquiryIsEnable.Value)
        {
            // نمایش فایل های استعلامی
            await InquiryIsVisible(true);
        }
        else
        {
            // نمایش فایل های استعلامی
            await InquiryIsVisible(false);
        }
            
        }

		public async Task  InquiryIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCMPLATE_OS_Details Item  )
        {
            // استعلام شماره 1
        var InquiryFileIsVisible = Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_InquiryFile1;
        // استعلام شماره 2
        var InquiryFile2IsVisible = Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_InquiryFile2;
        // استعلام شماره 3
        var InquiryFile3IsVisible = Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_InquiryFile3;

        if (Selected.Value.ToString() == "true")
        {
            InquiryFileIsVisible.SetVisible(true);
            InquiryFile2IsVisible.SetVisible(true);
            InquiryFile3IsVisible.SetVisible(true);
        }
        else
        {
            InquiryFileIsVisible.SetVisible(false);
            InquiryFile2IsVisible.SetVisible(false);
            InquiryFile3IsVisible.SetVisible(false);
        }
            
        }

		#endregion FunctionEvents

}
}
