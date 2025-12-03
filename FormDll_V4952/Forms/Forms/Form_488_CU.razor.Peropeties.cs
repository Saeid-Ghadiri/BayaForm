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
    public class Form_488_CUPeropeties : FormBase
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
    public string? VersionForm { get; set; } = "4952";

    /// <summary>
    ///  موجودیت
    /// </summary>
    public Entity.SCM_NumTransfersGoodsWarehouse? _Entity { get; set; } = new Entity.SCM_NumTransfersGoodsWarehouse();

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

public Input<string?> Ref_Title;
public Input<int?> Ref_code;
public Input<Guid> Ref_Id;
public Input<Guid?> Ref_RequestID;
public Input<Guid?> Ref_CreateUser;
public Input<Guid?> Ref_UpdateUser;
public Input<DateTime?> Ref_CreateDate;
public Input<DateTime?> Ref_UpdateDate;
public Input<bool?> Ref_IsDelete;
public DxGrid? Grid_SCM_ProductRequestDetails;

public Input<Guid> Ref_SCM_ProductRequestDetails_Id;
public Input<Guid?> Ref_SCM_ProductRequestDetails_RequestID;
public Input<Guid?> Ref_SCM_ProductRequestDetails_CreateUser;
public Input<Guid?> Ref_SCM_ProductRequestDetails_UpdateUser;
public Input<DateTime?> Ref_SCM_ProductRequestDetails_CreateDate;
public Input<DateTime?> Ref_SCM_ProductRequestDetails_UpdateDate;
public Input<bool?> Ref_SCM_ProductRequestDetails_IsDelete;
public Dropdown Ref_SCM_ProductRequestDetails_SCM_ProductRequestId;
public Dropdown Ref_SCM_ProductRequestDetails_ProductName_NotMapped;
public Input<string?> Ref_SCM_ProductRequestDetails_ProductMainCategoryText;
public Input<string?> Ref_SCM_ProductRequestDetails_ProductMainCategoryIdText;
public Input<string?> Ref_SCM_ProductRequestDetails_ProductSubCategoryText;
public Input<string?> Ref_SCM_ProductRequestDetails_ProductSubCategoryIdText;
public Input<string?> Ref_SCM_ProductRequestDetails_ProductCodeText;
public Input<string?> Ref_SCM_ProductRequestDetails_ProductUnitText;
public Input<string?> Ref_SCM_ProductRequestDetails_ShomaranFiscalYearText;
public Input<double?> Ref_SCM_ProductRequestDetails_ProductInventoryText;
public Input<double?> Ref_SCM_ProductRequestDetails_ProductRequestingQTY;
public Dropdown Ref_SCM_ProductRequestDetails_SCM_PriorityId;
public Input<string?> Ref_SCM_ProductRequestDetails_PlaceOfUseProduct;
public Input<string?> Ref_SCM_ProductRequestDetails_ProductRowDescription;
public Input<string?> Ref_SCM_ProductRequestDetails_ProductNameText;
public RadioBoolean Ref_SCM_ProductRequestDetails_ForeignMachineryProductTrueFasle;
public RadioBoolean Ref_SCM_ProductRequestDetails_FutureActionTrueFalse;
public RadioBoolean Ref_SCM_ProductRequestDetails_InquiryTrueFalse;
public Dropdown Ref_SCM_ProductRequestDetails_SCM_ProcurementId;
public Input<string?> Ref_SCM_ProductRequestDetails_Description2;
public RadioBoolean Ref_SCM_ProductRequestDetails_ProductDataSheetTrueFalse;
public FileUploadPage.Uploader<Entity.SCM_ProductRequestDetails_ProductDataSheetFile> Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductDataSheetFile;
public Input<string?> Ref_SCM_ProductRequestDetails_Description3;
public Input<double?> Ref_SCM_ProductRequestDetails_DeficitSupplyNumber;
public RadioBoolean Ref_SCM_ProductRequestDetails_TypeofProductDelivery;
public RadioBoolean Ref_SCM_ProductRequestDetails_ProductType;
public RadioBoolean Ref_SCM_ProductRequestDetails_ProductPartofTheProperty;
public Input<string?> Ref_SCM_ProductRequestDetails_ProductDeliveryDateTime;
public Input<double?> Ref_SCM_ProductRequestDetails_NumberofProductDelivery;
public Input<string?> Ref_SCM_ProductRequestDetails_IsExistText;
public FileUploadPage.Uploader<Entity.SCM_ProductRequestDetails_InquiryFirst> Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryFirst;
public FileUploadPage.Uploader<Entity.SCM_ProductRequestDetails_InquirySecondFile> Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquirySecondFile;
public FileUploadPage.Uploader<Entity.SCM_ProductRequestDetails_InquiryThirdFile> Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryThirdFile;
public Input<bool?> Ref_SCM_ProductRequestDetails_RequiredProuductOld;
public Input<string?> Ref_SCM_ProductRequestDetails_DescriptionWarehouseKeeper;
public RadioBoolean Ref_SCM_ProductRequestDetails_RequesterProductTypeofDelivery;
public RadioBoolean Ref_SCM_ProductRequestDetails_DeliveredProductApproved;
public Input<string?> Ref_SCM_ProductRequestDetails_productTemporaryDate;
public Input<string?> Ref_SCM_ProductRequestDetails_ProductResultDelieryDate;
public Input<string?> Ref_SCM_ProductRequestDetails_PermanentProductDeliveryDate;
public FileUploadPage.Uploader<Entity.SCM_ProductRequestDetails_InquiryofTechnicalOffice1stFile> Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryofTechnicalOffice1stFile;
public FileUploadPage.Uploader<Entity.SCM_ProductRequestDetails_InquiryofTechnicalOffice3rdFile> Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryofTechnicalOffice3rdFile;
public FileUploadPage.Uploader<Entity.SCM_ProductRequestDetails_InquiryofTechnicalOffice2ndFile> Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryofTechnicalOffice2ndFile;
public RadioBoolean Ref_SCM_ProductRequestDetails_ConfirmInquiryHeadOfMecanical;
public Dropdown Ref_SCM_ProductRequestDetails_SCM_TechnicalOfficeUnitId;
public Input<bool?> Ref_SCM_ProductRequestDetails_ConfirmInquiryHeadofTechnicalOffice;
public FileUploadPage.Uploader<Entity.SCM_ProductRequestDetails_ProductSampleFile> Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductSampleFile;
public Input<string?> Ref_SCM_ProductRequestDetails_SupervisorProcurementInquiryDateTime;
public Input<string?> Ref_SCM_ProductRequestDetails_TechnicalEngineeringOfficeInquiryDateTime;
public Input<string?> Ref_SCM_ProductRequestDetails_ProcurementWorkAllocationDateTime;
public Input<string?> Ref_SCM_ProductRequestDetails_DeliveryProcurementOfSuppliesDateTime;
public Input<string?> Ref_SCM_ProductRequestDetails_TheDueDateTimeOfTheWork;
public Input<string?> Ref_SCM_ProductRequestDetails_DueDateTimeOfWorkDelivery;
public RadioBoolean Ref_SCM_ProductRequestDetails_HardwareORNetworkSection;
public Input<string?> Ref_SCM_ProductRequestDetails_ReceiptInquiryDateTime;
public Input<string?> Ref_SCM_ProductRequestDetails_ProcurementInquiryTaskAssignmentDateTime;
public Input<string?> Ref_SCM_ProductRequestDetails_TechnicalOfficeInquiryTaskAssignmentDateTime;
public RadioBoolean Ref_SCM_ProductRequestDetails_ProductDelivery;
public RadioBoolean Ref_SCM_ProductRequestDetails_Formula1;
public Dropdown Ref_SCM_ProductRequestDetails_BIGCompanies;
public Input<string?> Ref_SCM_ProductRequestDetails_ProductDeliveryDateTimeAnotherWarehouse;
public Input<bool?> Ref_SCM_ProductRequestDetails_ConfirmationSupervisorHardware;
public Input<bool?> Ref_SCM_ProductRequestDetails_ConfirmationSupervisorNetwork;
public Input<double?> Ref_SCM_ProductRequestDetails_NumberOfLoanedGoods;
public Input<string?> Ref_SCM_ProductRequestDetails_ConfirmationOfTheInquiryExecutiveHeads;
public Input<string?> Ref_SCM_ProductRequestDetails_ConfirmationOfTheInquiryTechnicalOffice;
public Input<string?> Ref_SCM_ProductRequestDetails_ConfirmationOfTheInquiryTechnicalManager;
public Input<string?> Ref_SCM_ProductRequestDetails_ConfirmationOfTheInquiryLogistics;
public Input<string?> Ref_SCM_ProductRequestDetails_ConfirmationOfTheInquiryFactoryManager;
public Input<string?> Ref_SCM_ProductRequestDetails_ConfirmationOfTheInquiryCEO;
public Input<double?> Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics;
public Input<bool?> Ref_SCM_ProductRequestDetails_BuyForceIsEnable;
public Input<bool?> Ref_SCM_ProductRequestDetails_BuyDefaultIsEnable;
public RadioBoolean Ref_SCM_ProductRequestDetails_FormulaTahvilMostaghim;
public Input<double?> Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2;
public Input<double?> Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3;
public Input<double?> Ref_SCM_ProductRequestDetails_NumberofProductDelivery2;
public Input<double?> Ref_SCM_ProductRequestDetails_NumberofProductDelivery3;
public RadioBoolean Ref_SCM_ProductRequestDetails_FormulaSarparastFani;
public Input<int?> Ref_SCM_ProductRequestDetails_MapGroupCodeNum;
public Input<bool?> Ref_SCM_ProductRequestDetails_RejectRequestRow;
public Input<bool?> Ref_SCM_ProductRequestDetails_WK_ProductDelivery;
public RadioBoolean Ref_SCM_ProductRequestDetails_ProductRemainingIsEnable;
public Input<double?> Ref_SCM_ProductRequestDetails_NumberOfProductRemaining;
public RadioBoolean Ref_SCM_ProductRequestDetails_SurplusProductIsEnable;
public Input<double?> Ref_SCM_ProductRequestDetails_NumberOfSurplusProduct;
public RadioBoolean Ref_SCM_ProductRequestDetails_ReturnedProductIsEnable;
public Input<double?> Ref_SCM_ProductRequestDetails_NumberOfReturnedProduct;
public Input<double?> Ref_SCM_ProductRequestDetails_NumberOfDeliveredProducts;
public RadioBoolean Ref_SCM_ProductRequestDetails_RepeatDeliveryProductIsEnable;
public Input<bool?> Ref_SCM_ProductRequestDetails_ProcurementInquiryIsDone;
public Input<bool?> Ref_SCM_ProductRequestDetails_TOInquiryIsDone;
public Input<bool?> Ref_SCM_ProductRequestDetails_FinalInquiryIsDone;
public Input<bool?> Ref_SCM_ProductRequestDetails_ProductPurchasedDeliveredWarehouse;
public Input<int?> Ref_SCM_ProductRequestDetails_InvoiceAmount1;
public Input<int?> Ref_SCM_ProductRequestDetails_InvoiceAmount2;
public Input<int?> Ref_SCM_ProductRequestDetails_InvoiceAmount3;
public Input<int?> Ref_SCM_ProductRequestDetails_InvoiceAmount4;
public Input<int?> Ref_SCM_ProductRequestDetails_InvoiceAmount5;
public Input<int?> Ref_SCM_ProductRequestDetails_InvoiceAmount6;
public Input<int?> Ref_SCM_ProductRequestDetails_DeliveryCode;
public Input<int?> Ref_SCM_ProductRequestDetails_GetDeliveryCode;
public Input<string?> Ref_SCM_ProductRequestDetails_DateTimeDeliveryCode;
public Dropdown Ref_SCM_ProductRequestDetails_Global_SCMRequestTypeId;
public Input<Guid?> Ref_SCM_ProductRequestDetails_SCM_NumTransfersGoodsWarehouseId;


    #endregion

 

}
}
