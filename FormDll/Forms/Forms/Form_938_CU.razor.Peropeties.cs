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
    public class Form_938_CUPeropeties : FormBase
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
    public string? VersionForm { get; set; } = "5550";

    /// <summary>
    ///  موجودیت
    /// </summary>
    public Entity.Shomaran_Anbord? _Entity { get; set; } = new Entity.Shomaran_Anbord();

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

public Dropdown Ref_FORMCODE;
public Input<string?> Ref_FORMCODE2;
public Dropdown Ref_ORDERNO;
public Dropdown Ref_INVCODE;
public Input<short?> Ref_FORMYEAR;
public Input<string?> Ref_REQPERSON;
public Input<decimal?> Ref_MAIN_MNT;
public Dropdown Ref_CENTCODE;
public Dropdown Ref_CLOSED2;
public Dropdown Ref_OK_BTN2;
public Dropdown Ref_PSOURCE2;
public Input<string?> Ref_ORDWANTNO;
public Input<string?> Ref_TEMPNO;
public Input<short?> Ref_ORDERYEAR;
public Input<string?> Ref_NOTE;
public Input<short?> Ref_YEAR;
public Input<string?> Ref_NDATE;
public Input<string?> Ref_ORDERDATE;
public Input<string?> Ref_OKFACTDATE;
public Input<string?> Ref_CREATOR;
public Input<string?> Ref_WUSER;
public Input<Guid> Ref_Id;
public DxGrid? Grid_Shomaran_AnbordDetail;

public Input<Guid> Ref_Shomaran_AnbordDetail_Id;
public Input<Guid?> Ref_Shomaran_AnbordDetail_RequestID;
public Input<Guid?> Ref_Shomaran_AnbordDetail_CreateUser;
public Input<Guid?> Ref_Shomaran_AnbordDetail_UpdateUser;
public Input<DateTime?> Ref_Shomaran_AnbordDetail_CreateDate;
public Input<DateTime?> Ref_Shomaran_AnbordDetail_UpdateDate;
public Input<bool?> Ref_Shomaran_AnbordDetail_IsDelete;
public Input<double?> Ref_Shomaran_AnbordDetail_OKAMOUNT;
public Input<decimal?> Ref_Shomaran_AnbordDetail_ORDAMOUNT;
public Input<decimal?> Ref_Shomaran_AnbordDetail_ORDAMOUNT1;
public Input<string?> Ref_Shomaran_AnbordDetail_RADYABI;
public Input<string?> Ref_Shomaran_AnbordDetail_SEFARESH;
public Input<string?> Ref_Shomaran_AnbordDetail_RowId;
public Input<short?> Ref_Shomaran_AnbordDetail_Year;
public Dropdown Ref_Shomaran_AnbordDetail_PARTCODE;
public Input<Guid?> Ref_RequestID;
public Input<Guid?> Ref_CreateUser;
public Input<Guid?> Ref_UpdateUser;
public Input<DateTime?> Ref_CreateDate;
public Input<DateTime?> Ref_UpdateDate;
public Input<bool?> Ref_IsDelete;
public Input<string?> Ref_ApiResult;


    #endregion

 

}
}
