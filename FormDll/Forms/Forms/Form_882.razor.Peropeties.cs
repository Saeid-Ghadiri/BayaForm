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
    public class Form_882Peropeties : FormBase
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
    public string? VersionForm { get; set; } = "6068";

    /// <summary>
    ///  موجودیت
    /// </summary>
    public Entity.SCMPETCO_SupplierMaster? _Entity { get; set; } = new Entity.SCMPETCO_SupplierMaster();

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

public Input<string?> Ref_SystemUser;
public Input<string?> Ref_SystemUnitUser;
public Input<string?> Ref_SystemSectionUser;
public Input<string?> Ref_UserCompanyName;
public Input<string?> Ref_RequestTrakingCode;
public Input<string?> Ref_ProcessVersionID;
public Input<string?> Ref_SupplierName;
public Input<string?> Ref_SupplierBusinessOwner;
public Input<string?> Ref_ServiceName;
public Input<bool?> Ref_IsDelete;
public Input<string?> Ref_ContactPerson;
public Input<string?> Ref_Email;
public Input<string?> Ref_OfficePhoneNumber;
public Input<string?> Ref_OfficeFax;
public Input<string?> Ref_OfficeAddress;
public Input<string?> Ref_FactoryPhoneNumber;
public Input<string?> Ref_FactoryFax;
public Input<string?> Ref_FactoryAddress;
public FileUploadPage.Uploader<Entity.SCMPETCO_SupplierMaster_Attachments> Ref_SCMPETCO_SupplierMaster_Attachments;
public DxGrid? Grid_SCMPETCO_OrgSkills_SupplierDetail;

public Input<string?> Ref_SCMPETCO_OrgSkills_SupplierDetail_ServicesName;
public Input<string?> Ref_SCMPETCO_OrgSkills_SupplierDetail_ProductName;
public Input<decimal?> Ref_SCMPETCO_OrgSkills_SupplierDetail_DailyCapacity;
public Input<string?> Ref_SCMPETCO_OrgSkills_SupplierDetail_Description;
public Input<Guid> Ref_SCMPETCO_OrgSkills_SupplierDetail_Id;
public Input<Guid?> Ref_SCMPETCO_OrgSkills_SupplierDetail_RequestID;
public Input<Guid?> Ref_SCMPETCO_OrgSkills_SupplierDetail_CreateUser;
public Input<Guid?> Ref_SCMPETCO_OrgSkills_SupplierDetail_UpdateUser;
public Input<DateTime?> Ref_SCMPETCO_OrgSkills_SupplierDetail_CreateDate;
public Input<DateTime?> Ref_SCMPETCO_OrgSkills_SupplierDetail_UpdateDate;
public Input<bool?> Ref_SCMPETCO_OrgSkills_SupplierDetail_IsDelete;
public DxGrid? Grid_SCMPETCO_Partnerships_SupplierDetail;

public Input<string?> Ref_SCMPETCO_Partnerships_SupplierDetail_ContractorDetails;
public Input<string?> Ref_SCMPETCO_Partnerships_SupplierDetail_ServicesTitle;
public Input<string?> Ref_SCMPETCO_Partnerships_SupplierDetail_StartDate;
public Input<DateTime?> Ref_SCMPETCO_Partnerships_SupplierDetail_StartDateEN;
public Input<Guid> Ref_SCMPETCO_Partnerships_SupplierDetail_Id;
public Input<Guid?> Ref_SCMPETCO_Partnerships_SupplierDetail_RequestID;
public Input<Guid?> Ref_SCMPETCO_Partnerships_SupplierDetail_CreateUser;
public Input<Guid?> Ref_SCMPETCO_Partnerships_SupplierDetail_UpdateUser;
public Input<DateTime?> Ref_SCMPETCO_Partnerships_SupplierDetail_CreateDate;
public Input<DateTime?> Ref_SCMPETCO_Partnerships_SupplierDetail_UpdateDate;
public Input<bool?> Ref_SCMPETCO_Partnerships_SupplierDetail_IsDelete;
public Input<string?> Ref_TestingSkillsDesc;
public Input<Guid?> Ref_RequestID;
public Input<Guid?> Ref_CreateUser;
public Input<Guid?> Ref_UpdateUser;
public Input<DateTime?> Ref_CreateDate;
public Input<DateTime?> Ref_UpdateDate;
public Input<Guid> Ref_Id;


    #endregion

 

}
}
