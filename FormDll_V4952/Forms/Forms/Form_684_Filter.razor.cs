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
using Baya.Models.Utility;
using System.Net;

namespace Forms.Forms
{
    public class Form_684_FilterBase : Form_684_FilterPeropeties
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

