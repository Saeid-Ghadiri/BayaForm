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
    public class Form_339Base : Form_339Peropeties
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

    
    public async Task KH_TempNoNum_NotMapped_onitemselected(dynamic Selected   )
    {
        Console.WriteLine("#Log 1");
        // Console.WriteLine("#Log1.1" + " Is: " + _Entity.KH_ORDERNO.ToString());
        Console.WriteLine(await Utility.JSON.ToJson(Selected));
	    _Entity.KH_TEMPNO = Selected.TEMPNO;
	    _Entity.KH_PAYCENTName = Selected.PAYCENTName;
	    _Entity.KH_CENTCODE = Selected.CENTCODE;
	    _Entity.KH_REQPERSON = Selected.REQPERSON;
	    _Entity.KH_WUSER = Selected.WUSER;
	    _Entity.KH_APPROVER = Selected.APPROVER;
	    _Entity.KH_ORDERDATE = Selected.ORDERDATE;
	    _Entity.KH_OKFACTDATE = Selected.OKFACTDATE;
	    _Entity.KH_ORDERNO = Selected.ORDERNO;
        _Entity.KH_ORDERNO_GUID = Selected.ORDERNO_GUID;
	    _Entity.KH_TempNoNum = Selected.TempNoNum;
	    _Entity.KHARID_YEAR = Selected.YEAR;
	    _Entity.KH_INVCODE = Selected.INVCODE;
        
		// فراخوانی داده از dropdown TempNoNum برای گرید داده های آن
		Ref_KH_KharidDTL.LoadData();
    }

	#endregion FunctionEvents

}
}
