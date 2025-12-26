using Microsoft.AspNetCore.Components;
using DrowpDownPage;
using Domain.Form;
using Microsoft.JSInterop;
using Castle.DynamicLinqQueryBuilder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Baya.Models.ORM;
using Blazored.Toast.Services;
using AKSoftware.Localization.MultiLanguages;
using InputPage;
using TimePage;
using InputPage2;
using RadioBooleanPage;
using FormsPage;
using DevExpress.Blazor;
using Sitko.Blazor.CKEditor;
using Plugins.Tools;
using BlazorSpinner;
using AKSoftware.Localization.MultiLanguages.Blazor;
using System.Net;
using System.Security.Claims;
using AKSoftware.Localization.MultiLanguages.Blazor;
using Baya.Models.Utility;
using Baya.Models.ORM;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Net;
using Microsoft.JSInterop;
using Baya.Models.ViewEngine;
using SingleFileUpload;
namespace Forms.Forms
{
    public class Form_1034Peropeties : FormBase
    {

        #region Peropeties

        /// <summary>
        /// اجرای کد JS
        /// </summary>
        [Inject]
    public IJSRuntime JS { get; set; }

    /// <summary>
    /// ماژول جهت تغییر کدهای html
    /// </summary>
    //public IJSObjectReference JSO { get; set; }
    /// <summary>
    /// برای ذخیره کردن مقدار اولیه رکورد جهت بررسی تغییرات
    /// </summary>
    public string? JsonData { get; set; }


    [Inject]
    public AuthenticationStateProvider _authenticationStateProvider { get; set; }

    [Inject]
    public IToastService toastService { get; set; }

    [Inject]
    public ILanguageContainerService languageContainer { get; set; }


    /// <summary>
    /// ایونت لود کامل فرم
    /// </summary>
    [Parameter]
    public EventCallback<Baya.Models.Utility.Result> OnFormloaded { get; set; }

    /// <summary>
    /// ایونت سابمیت دیتا
    /// </summary>
    //[Parameter]
    public EventCallback<Baya.Models.Utility.Result> OnFormSubmit { get; set; }


    /// <summary>
    ///  فیلتر داده
    /// </summary>
    [Parameter]
    public QueryBuilderFilterRule? QueryFilter { get; set; }

    //[Parameter]
    //public List<Baya.Models.Baya.Requests.Request_Parameters> RequestParameters { get; set; }

    //[Parameter]
    //public List<Baya.Models.Baya.Requests.Cartable_Parameter> CartablesParameters { get; set; }

    /// <summary>
    /// لیست پارامتر به صورت ابجکت
    /// </summary>
    [Parameter]
    public dynamic _RequestParameters { get; set; }

    /// <summary>
    /// لیست پارامتر به صورت ابجکت
    /// </summary>
    [Parameter]
    public dynamic _CartablesParameters { get; set; }

    /// <summary>
    ///  شماره درخواست
    /// </summary>
    [Parameter]
    public Guid? RequestID { get; set; }

    /// <summary>
    /// غیر فعال کردن کل فرم
    /// </summary>
    [Parameter]
    public bool ReadOnly { get; set; } = false;

    /// <summary>
    /// دکمه سابمیت نشون داده بشه یا نه
    /// </summary>
    [Parameter]
    public bool ShowSubmit { get; set; } = false;

    /// <summary>
    /// دکمه توی کارتابل ایدیش هست که کدوم زده شده
    ///آیدی فلش ها هستش
    ///ایدی دکمه ثبت موقت SaveAndClose 
    /// </summary>
    [Parameter]
    public string BtnWorkFlowId { get; set; }

    /// <summary>
    /// کانفیگ CKEditor
    /// </summary>
    public CKEditorConfig config;

    /// <summary>
    /// دیالوگ نمایش کانفرم
    /// </summary>
    public ConfirmDialog Confirm = default!;

    /// <summary>
    /// لودینگ
    /// </summary>
    [Inject] public LoadingService _loadingService { get; set; }

    /// <summary>
    ///  ورژنی که فرم باهاش ساخته شده
    /// </summary>
    public string? VersionForm { get; set; } = "5365";

    /// <summary>
    ///  موجودیت
    /// </summary>
    public Entity.IDMS_RDC_Master? _Entity { get; set; } = new Entity.IDMS_RDC_Master();

            /// <summary>
            /// مشخصات کاربر
            /// UserID
            /// </summary>
            public dynamic _User { get; set; }

    public EditForm? _Form { get; set; }

    public string? SumaryMessage { get; set; }

    /// <summary>
    /// ساختار تیبل برای دریافت داده
    /// </summary>
    public Table? TableGet { get; set; }
    /// <summary>
    /// ساختار تیبل برای دریافت داده
    /// </summary>
    public Table? TablePost { get; set; }
        /// <summary>
        /// فرم بصورت مستقل اجرا میش.د؟
        /// </summary>
        [Parameter]
        public bool IndependentForm { get; set; } = true;

    #endregion

    #region FormProperty

public Input<string?> Ref_UserStarterFullName;
public Input<string?> Ref_UserStarterCompanyName;
public Input<string?> Ref_UserStarterUnitName;
public Input<string?> Ref_UserStarterSectionName;
public DxGrid? Grid_IDMS_RDC_Details;

public Dropdown Ref_IDMS_RDC_Details_IDMS_ProductCategoriesId;
public Dropdown Ref_IDMS_RDC_Details_IDMS_ProductsId;
public Dropdown Ref_IDMS_RDC_Details_IDMS_CustomerId;
public Dropdown Ref_IDMS_RDC_Details_IDMS_ResultingFromId;
public Input<string?> Ref_IDMS_RDC_Details_RequestedDueDate_Fa;
public Input<string?> Ref_IDMS_RDC_Details_Description;
public Input<string?> Ref_IDMS_RDC_Details_RDC_ActualCompletionDate_Fa;
public CKEditor Ref_IDMS_RDC_Details_RDC_SUPERVISOR_Description;
public Input<Guid> Ref_IDMS_RDC_Details_Id;
public Input<Guid?> Ref_IDMS_RDC_Details_RequestID;
public Input<Guid?> Ref_IDMS_RDC_Details_CreateUser;
public Input<Guid?> Ref_IDMS_RDC_Details_UpdateUser;
public Input<DateTime?> Ref_IDMS_RDC_Details_CreateDate;
public Input<DateTime?> Ref_IDMS_RDC_Details_UpdateDate;
public Input<bool?> Ref_IDMS_RDC_Details_IsDelete;
public Input<DateTime?> Ref_IDMS_RDC_Details_RequestedDueDate;
public Input<DateTime?> Ref_IDMS_RDC_Details_RDC_ActualCompletionDate;
public DxGrid? Grid_IDMS_TestModel;

public Dropdown Ref_IDMS_TestModel_IDMS_TestModelObjId;
public Input<string?> Ref_IDMS_TestModel_TesterDescription;
public Dropdown Ref_IDMS_TestModel_Tester;
public Input<Guid> Ref_IDMS_TestModel_Id;
public Input<Guid?> Ref_IDMS_TestModel_RequestID;
public Input<Guid?> Ref_IDMS_TestModel_CreateUser;
public Input<Guid?> Ref_IDMS_TestModel_UpdateUser;
public Input<DateTime?> Ref_IDMS_TestModel_CreateDate;
public Input<DateTime?> Ref_IDMS_TestModel_UpdateDate;
public Input<bool?> Ref_IDMS_TestModel_IsDelete;
public Input<Guid> Ref_Id;
public Input<Guid?> Ref_RequestID;
public Input<Guid?> Ref_CreateUser;
public Input<Guid?> Ref_UpdateUser;
public Input<DateTime?> Ref_CreateDate;
public Input<DateTime?> Ref_UpdateDate;
public Input<bool?> Ref_IsDelete;


    #endregion

 

}
}
