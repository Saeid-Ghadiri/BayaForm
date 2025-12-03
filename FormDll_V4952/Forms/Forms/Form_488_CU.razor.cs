using System;
using Castle.DynamicLinqQueryBuilder;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Utility;
using Blazored.Toast.Services;
using AKSoftware.Localization.MultiLanguages;
using Forms.Forms;
using Forms;
using Sitko.Blazor.CKEditor;
namespace Forms.Forms
{
    public class Form_488_CUBase : Form_488_CUPeropeties
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


    #region FunctionEvents

    #endregion FunctionEvents


}
}

