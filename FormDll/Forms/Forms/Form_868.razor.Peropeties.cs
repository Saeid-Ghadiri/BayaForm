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
    public class Form_868Peropeties : FormBase
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
    public string? VersionForm { get; set; } = "5122";

    /// <summary>
    ///  موجودیت
    /// </summary>
    public Entity.SCMPETCO_ProductRequest? _Entity { get; set; } = new Entity.SCMPETCO_ProductRequest();

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
public Input<string?> Ref_SystemUser;
public Input<string?> Ref_UserCompanyName;
public Input<string?> Ref_SystemUnitUser;
public Input<string?> Ref_SystemSectionUser;
public DxGrid? Grid_SCMPETCO_ProductRequestDetails;

public Dropdown Ref_SCMPETCO_ProductRequestDetails_ProductName_NotMapped;
public Input<string?> Ref_SCMPETCO_ProductRequestDetails_ProductNameText;
public Input<string?> Ref_SCMPETCO_ProductRequestDetails_ProductCodeText;
public Input<string?> Ref_SCMPETCO_ProductRequestDetails_ProductUnitText;
public Input<string?> Ref_SCMPETCO_ProductRequestDetails_PlaceOfUseProduct;
public Input<double?> Ref_SCMPETCO_ProductRequestDetails_ProductRequestingQTY;
public Dropdown Ref_SCMPETCO_ProductRequestDetails_SCMPETCO_PriorityId;
public Input<int?> Ref_SCMPETCO_ProductRequestDetails_DeliveryCode;
public Input<string?> Ref_SCMPETCO_ProductRequestDetails_ProductRowDescription;
public Input<string?> Ref_SCMPETCO_ProductRequestDetails_IsExist;
public Input<string?> Ref_SCMPETCO_ProductRequestDetails_ProductMainCategoryText;
public Input<Guid> Ref_SCMPETCO_ProductRequestDetails_Id;
public Input<Guid?> Ref_SCMPETCO_ProductRequestDetails_RequestID;
public Input<Guid?> Ref_SCMPETCO_ProductRequestDetails_CreateUser;
public Input<Guid?> Ref_SCMPETCO_ProductRequestDetails_UpdateUser;
public Input<DateTime?> Ref_SCMPETCO_ProductRequestDetails_UpdateDate;
public Input<bool?> Ref_SCMPETCO_ProductRequestDetails_IsDelete;
public Input<DateTime?> Ref_SCMPETCO_ProductRequestDetails_CreateDate;
public Input<string?> Ref_SCMPETCO_ProductRequestDetails_ProductMainCategoryIdText;
public Input<string?> Ref_SCMPETCO_ProductRequestDetails_ProductSubCategoryText;
public Input<string?> Ref_SCMPETCO_ProductRequestDetails_ProductSubCategoryIdText;
public Input<string?> Ref_SCMPETCO_ProductRequestDetails_ShomaranFiscalYearText;
public Input<double?> Ref_SCMPETCO_ProductRequestDetails_ProductInventoryText;
public Input<Guid?> Ref_RequestID;
public Input<Guid?> Ref_CreateUser;
public Input<Guid?> Ref_UpdateUser;
public Input<DateTime?> Ref_CreateDate;
public Input<DateTime?> Ref_UpdateDate;
public Input<bool?> Ref_IsDelete;
public Input<string?> Ref_RequestTrakingCode;
public Input<string?> Ref_ProcessVersionID;


    #endregion

 

}
}
