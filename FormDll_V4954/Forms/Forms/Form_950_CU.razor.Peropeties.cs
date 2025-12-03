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
    public class Form_950_CUPeropeties : FormBase
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
    public string? VersionForm { get; set; } = "4954";

    /// <summary>
    ///  موجودیت
    /// </summary>
    public Entity.Shomaran_OK_BTN? _Entity { get; set; } = new Entity.Shomaran_OK_BTN();

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

public Input<Guid> Ref_Id;
public DxGrid? Grid_Shomaran_Anbord;

public Input<Guid> Ref_Shomaran_Anbord_Id;
public Input<Guid?> Ref_Shomaran_Anbord_RequestID;
public Input<Guid?> Ref_Shomaran_Anbord_CreateUser;
public Input<Guid?> Ref_Shomaran_Anbord_UpdateUser;
public Input<DateTime?> Ref_Shomaran_Anbord_CreateDate;
public Input<DateTime?> Ref_Shomaran_Anbord_UpdateDate;
public Input<bool?> Ref_Shomaran_Anbord_IsDelete;
public Dropdown Ref_Shomaran_Anbord_CENTCODE;
public Input<string?> Ref_Shomaran_Anbord_CREATOR;
public Input<byte?> Ref_Shomaran_Anbord_CLOSED;
public Dropdown Ref_Shomaran_Anbord_FORMCODE;
public Input<short?> Ref_Shomaran_Anbord_FORMYEAR;
public Input<string?> Ref_Shomaran_Anbord_REQPERSON;
public Dropdown Ref_Shomaran_Anbord_ORDERNO;
public Dropdown Ref_Shomaran_Anbord_INVCODE;
public Input<decimal?> Ref_Shomaran_Anbord_MAIN_MNT;
public Input<string?> Ref_Shomaran_Anbord_NDATE;
public Input<string?> Ref_Shomaran_Anbord_ORDERDATE;
public Input<string?> Ref_Shomaran_Anbord_OKFACTDATE;
public Input<string?> Ref_Shomaran_Anbord_OK_BTN;
public Input<string?> Ref_Shomaran_Anbord_ORDWANTNO;
public Input<byte?> Ref_Shomaran_Anbord_PSOURCE;
public Input<string?> Ref_Shomaran_Anbord_TEMPNO;
public Input<string?> Ref_Shomaran_Anbord_WUSER;
public Input<short?> Ref_Shomaran_Anbord_ORDERYEAR;
public Input<short?> Ref_Shomaran_Anbord_YEAR;
public Input<string?> Ref_Shomaran_Anbord_NOTE;
public Dropdown Ref_Shomaran_Anbord_CLOSED2;
public Input<Guid?> Ref_Shomaran_Anbord_OK_BTN2;
public Dropdown Ref_Shomaran_Anbord_PSOURCE2;
public Input<Guid?> Ref_RequestID;
public Input<Guid?> Ref_CreateUser;
public Input<Guid?> Ref_UpdateUser;
public Input<DateTime?> Ref_CreateDate;
public Input<DateTime?> Ref_UpdateDate;
public Input<bool?> Ref_IsDelete;
public Input<string?> Ref_Title;
public Input<byte?> Ref_Code;


    #endregion

 

}
}
