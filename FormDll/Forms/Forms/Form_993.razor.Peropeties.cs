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
    public class Form_993Peropeties : FormBase
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
    public string? VersionForm { get; set; } = "5154";

    /// <summary>
    ///  موجودیت
    /// </summary>
    public Entity.SCM_ProductRequest? _Entity { get; set; } = new Entity.SCM_ProductRequest();

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
public Input<string?> Ref_UserCompanyName;
public Input<string?> Ref_SystemUnitUser;
public Input<string?> Ref_SystemSectionUser;
public DxGrid? Grid_SCM_ProductRequestDetails;

public Input<string?> Ref_SCM_ProductRequestDetails_ProductNameText;
public Input<string?> Ref_SCM_ProductRequestDetails_ProductCodeText;
public Input<string?> Ref_SCM_ProductRequestDetails_ProductUnitText;
public Dropdown Ref_SCM_ProductRequestDetails_SCM_PriorityId;
public Input<double?> Ref_SCM_ProductRequestDetails_ProductRequestingQTY;
public Input<double?> Ref_SCM_ProductRequestDetails_DeficitSupplyNumber;
public Input<string?> Ref_SCM_ProductRequestDetails_ProductRowDescription;
public Input<string?> Ref_SCM_ProductRequestDetails_Description2;
public RadioBoolean Ref_SCM_ProductRequestDetails_ProductDataSheetTrueFalse;
public FileUploadPage.Uploader<Entity.SCM_ProductRequestDetails_ProductDataSheetFile> Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductDataSheetFile;
public RadioBoolean Ref_SCM_ProductRequestDetails_InquiryTrueFalse;
public FileUploadPage.Uploader<Entity.SCM_ProductRequestDetails_InquiryFirst> Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryFirst;
public FileUploadPage.Uploader<Entity.SCM_ProductRequestDetails_InquirySecondFile> Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquirySecondFile;
public FileUploadPage.Uploader<Entity.SCM_ProductRequestDetails_InquiryThirdFile> Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryThirdFile;
public Input<string?> Ref_SCM_ProductRequestDetails_ConfirmationOfTheInquiryCEO;
public Input<string?> Ref_SCM_ProductRequestDetails_InquiryApproverDesc;
public Dropdown Ref_SCM_ProductRequestDetails_SCM_NumTransfersGoodsWarehouseId;
public Input<double?> Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics;
public Input<double?> Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2;
public Input<double?> Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3;
public RadioBoolean Ref_SCM_ProductRequestDetails_SurplusProductIsEnable;
public Input<double?> Ref_SCM_ProductRequestDetails_NumberOfSurplusProduct;
public RadioBoolean Ref_SCM_ProductRequestDetails_IsPostponedPurchase;
public Input<Guid> Ref_SCM_ProductRequestDetails_Id;
public Input<Guid?> Ref_SCM_ProductRequestDetails_RequestID;
public Input<Guid?> Ref_SCM_ProductRequestDetails_CreateUser;
public Input<Guid?> Ref_SCM_ProductRequestDetails_UpdateUser;
public Input<DateTime?> Ref_SCM_ProductRequestDetails_CreateDate;
public Input<DateTime?> Ref_SCM_ProductRequestDetails_UpdateDate;
public Input<bool?> Ref_SCM_ProductRequestDetails_IsDelete;
public RadioBoolean Ref_SCM_ProductRequestDetails_FutureActionTrueFalse;
public RadioBoolean Ref_SCM_ProductRequestDetails_ProductDelivery;
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
