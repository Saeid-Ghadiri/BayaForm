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
    public class Form_1149Base : Form_1149Peropeties
    {

        // تابع پیام تُــست
		public MSG _MSG { get; set; }

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
            // فراخوانی سرویس تُست
			_MSG = new MSG(toastService);
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
    // select Id, Code, Title
    // from SCMATLASCELL_OS_ResultingFrom
    // Id	Code	Title
    // 42424E4D-B65C-F111-A514-005056A2B6BD	1	تعمیرات
    // F8AC7154-B65C-F111-A514-005056A2B6BD	2	طراحی و ساخت
    // AD87E05A-B65C-F111-A514-005056A2B6BD	3	پیمانکار
    // 330DF767-B65C-F111-A514-005056A2B6BD	4	آنالیز - کالیبراسیون

    public async Task <bool> GridSCMATLASCELL_OS_MasterId_872_editmodelsaving(object e   )
        {

            return false;
        }
public async Task  GridSCMATLASCELL_OS_MasterId_872_afterrendermodal(Entity.SCMATLASCELL_OS_Details Item   )
        {

            
        }

		public async Task  UploadFileIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCMATLASCELL_OS_Details Item  )
        {

            
        }
public async Task  IsEnableSampleGoods_oninput(ChangeEventArgs Selected ,Entity.SCMATLASCELL_OS_Details Item  )
        {

            
        }
public async Task  IsEnableDemolitionAndRenovation_oninput(ChangeEventArgs Selected ,Entity.SCMATLASCELL_OS_Details Item  )
        {

            
        }

		#endregion FunctionEvents

}
}
