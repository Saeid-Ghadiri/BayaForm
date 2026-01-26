using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using Baya.Extentions.Blazor;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;

namespace Forms.Forms
{
    public class Form_1043_CUBase : Form_1043_CUPeropeties
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

    public async Task  submit_onclick(MouseEventArgs Selected   )
        {

            Console.WriteLine("Log ::  CodeMelli ::" + _Entity.CodeMelli.ToString());
            Console.WriteLine("Log ::  Year ::" + _Entity.Month);
            Console.WriteLine("Log ::  Month ::" + _Entity.Year);

            var ApiResult = await ApiServer.Internal.Finantial.Salary.Get(Convert.ToInt32(_Entity.Year), Convert.ToInt32(_Entity.Month), _Entity.CodeMelli.ToString(), ApplicationType.InfoBig, await (_authenticationStateProvider as CustomAuthProvider).GetToken());


            var x = await Utility.JSON.ToJson(ApiResult);
			Console.WriteLine("Log ::  Data ::" + x);
            
        }

		#endregion FunctionEvents

}
}
