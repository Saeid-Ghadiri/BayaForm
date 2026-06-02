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
    public class Form_1151Base : Form_1151Peropeties
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

        // if (_Entity.ReqCount != 5)
        // {
        //     IsValid = false;
        //     SumaryMessage += "تعداد درخواست مخالف 5 باشد";
        // }

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


    public async Task <bool> GridSCMPLATE_OS_MasterId_863_editmodelsaving(object e   )
        {

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
        }

		#endregion FunctionEvents

}
}
