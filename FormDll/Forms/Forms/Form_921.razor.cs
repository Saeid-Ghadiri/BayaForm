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
    public class Form_921Base : Form_921Peropeties
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
    /// تابع قبل اجرا شدن ارسال داده
    /// </summary>
    /// <returns></returns>
    public override async Task<Result> BeforSubmit()
    {
        return new Result() { Status = HttpStatusCode.OK };
    }

    public async Task FillTableGet()
    {
        if (TableGet == null)
        {
            TableGet = new Baya.Models.ORM.Table();
            TableGet.Name = RefForm.TableGet.Name;


            TableGet.NameAs = RefForm.TableGet.NameAs;
            TableGet.ModeErtebat = RefForm.TableGet.ModeErtebat;
            TableGet.Column = RefForm.TableGet.Column;
            TableGet.SqlWhere = RefForm.TableGet.SqlWhere;
            TableGet.Filter = RefForm.TableGet.Filter;
            TableGet.Relation = RefForm.TableGet.Relation?.Where(p=>p.ModeErtebat== Baya.Models.ORM.ModeErtebat._1N).ToList();
        }
    }
        #region FunctionEvents

        #endregion FunctionEvents


    }
}

