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
    public class Form_705Peropeties : FormBase
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
    public string? VersionForm { get; set; } = "4966";

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
public Input<double?> Ref_SCM_ProductRequestDetails_ProductInventoryText;
public Dropdown Ref_SCM_ProductRequestDetails_SCM_PriorityId;
public Input<string?> Ref_SCM_ProductRequestDetails_PlaceOfUseProduct;
public Input<string?> Ref_SCM_ProductRequestDetails_ProductRowDescription;
public Input<double?> Ref_SCM_ProductRequestDetails_ProductRequestingQTY;
public Input<string?> Ref_SCM_ProductRequestDetails_DescriptionWarehouseKeeper;
public Input<double?> Ref_SCM_ProductRequestDetails_NumberofProductDelivery;
public Input<int?> Ref_SCM_ProductRequestDetails_DeliveryCode;
public Input<string?> Ref_SCM_ProductRequestDetails_DateTimeDeliveryCode;
public Input<Guid?> Ref_SCM_ProductRequestDetails_RequestID;
public Input<Guid?> Ref_SCM_ProductRequestDetails_CreateUser;
public Input<Guid?> Ref_SCM_ProductRequestDetails_UpdateUser;
public Input<DateTime?> Ref_SCM_ProductRequestDetails_CreateDate;
public Input<DateTime?> Ref_SCM_ProductRequestDetails_UpdateDate;
public Input<bool?> Ref_SCM_ProductRequestDetails_IsDelete;
public Dropdown Ref_SCM_ProductRequestDetails_Global_ShomaranInfoId;
public Dropdown Ref_SCM_ProductRequestDetails_T_Search_NotMapped;
public Dropdown Ref_SCM_ProductRequestDetails_FB_Search_NotMapped;
public Dropdown Ref_SCM_ProductRequestDetails_KH_Search_NotMapped;
public Input<string?> Ref_SCM_ProductRequestDetails_T_PAYCENTName;
public Input<string?> Ref_SCM_ProductRequestDetails_T_CENTCODE;
public Input<Guid?> Ref_SCM_ProductRequestDetails_T_CENTCODE_GUID;
public Input<string?> Ref_SCM_ProductRequestDetails_T_CREATOR;
public Input<string?> Ref_SCM_ProductRequestDetails_T_FACTDATE;
public Input<string?> Ref_SCM_ProductRequestDetails_T_FACTNO;
public Input<Guid?> Ref_SCM_ProductRequestDetails_T_FACTNO_GUID;
public Input<string?> Ref_SCM_ProductRequestDetails_T_TEMPNO;
public Input<int?> Ref_SCM_ProductRequestDetails_T_TempNoNum;
public Input<int?> Ref_SCM_ProductRequestDetails_T_YEAR;
public Dropdown Ref_SCM_ProductRequestDetails_SH_Tahvil_DTL;
public Input<string?> Ref_SCM_ProductRequestDetails_KH_APPROVER;
public Input<string?> Ref_SCM_ProductRequestDetails_KH_CENTCODE;
public Input<Guid?> Ref_SCM_ProductRequestDetails_KH_CENTCODE_GUID;
public Input<string?> Ref_SCM_ProductRequestDetails_KH_PAYCENTName;
public Input<string?> Ref_SCM_ProductRequestDetails_KH_WUSER;
public Input<string?> Ref_SCM_ProductRequestDetails_KH_TEMPNO;
public Input<int?> Ref_SCM_ProductRequestDetails_KH_TempNoNum;
public Input<string?> Ref_SCM_ProductRequestDetails_KH_ORDERNO;
public Input<Guid?> Ref_SCM_ProductRequestDetails_KH_ORDERNO_GUID;
public Input<string?> Ref_SCM_ProductRequestDetails_KH_ORDERDATE;
public Input<string?> Ref_SCM_ProductRequestDetails_KH_OKFACTDATE;
public Input<string?> Ref_SCM_ProductRequestDetails_KH_INVCODE;
public Input<string?> Ref_SCM_ProductRequestDetails_KH_REQPERSON;
public Input<int?> Ref_SCM_ProductRequestDetails_KH_YEAR;
public Dropdown Ref_SCM_ProductRequestDetails_SH_Kharid_DTL;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_USER;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_ACCCODE;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_ACTCODE;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_ACTYEAR;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_ADD1;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_ADD2;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_ADD3;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_ADD4;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_AMOUNT;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_ARRNO;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_ARRYEAR;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_BAARNAME;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_BABCODE;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_BRANCHCODE;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_BUYKIND;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_C1901;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_C1950;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CAR_TYPE;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CASHCODE;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_CASHKIND;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_COMP_NAME;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_CPRICE;
public Input<Guid> Ref_SCM_ProductRequestDetails_Id;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CREATOR;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFKNO1;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFKNO2;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFKNO3;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFKNO4;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFKNO5;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFKNO6;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFKNO7;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFKNO8;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFKNO9;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFKNO10;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFNO1;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFNO2;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFNO3;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFNO4;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFNO5;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFNO6;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFNO7;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFNO8;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFNO9;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CTFNO10;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CURFACT;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_CURKIND;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_CURPKID;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_C_CURVAL;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_C_DEDUCE;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_C_OTHERPAY;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_C_PARPRICE;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_C_PREPRICE;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_C_WELLDONE;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_DEDUCE;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_DOC_ID;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_DOC_ID2;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_DOC_YEAR;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_DOC_YEAR2;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_DRV_CARTNO;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_YEAR;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_WUSER;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_DRV_NAME;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_ELAMNO;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_FACTDATE;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_FACTNO;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_FACTOR;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_FACTPOS;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_FACTSELER;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_FDATE;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_FID;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_FORMCODE;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_FORMYEAR;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_HAZCODE;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_INVCODE;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_MBCODE;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_MBPROJCODE;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_MBSBSTCODE;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_NEW_CURVAL;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_NOSEFARESH;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_NOTE;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_ORDERNO;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_ORDERYEAR;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_OTHERPAY;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_PAGE;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_PARPRICE;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_PARTKIND;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_PAYKNDCODE;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_PELAK;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_PKIND;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_PRECODE;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_PREDATE;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_PREPRICE;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_PRJMRDTLID;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_PROFORMCOD;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_PROJCODE;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_PROJYEAR;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_PRO_YEAR;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_QC;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_RECNO;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_RESNO;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_ROW_ID3;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_SEFCODE;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_SELERCODE;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_SELERNAME;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_SUB1;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_SUB2;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_SUB3;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_SUB4;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_SUMSAN;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_TAFCODE1;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_TAFCODE2;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_TAFCODE3;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_TAFCODE4;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_TAFCODE5;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_TAFCODE6;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_TAFCODE7;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_TAFKINDNO1;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_TAFKINDNO2;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_TAFKINDNO3;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_TAFKINDNO4;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_TAFKINDNO5;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_TAFKINDNO6;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_TAFKINDNO7;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_TANNO;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_USERNAME;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_WDPERCENT;
public Input<string?> Ref_SCM_ProductRequestDetails_FB_WELLDONE;
public Input<Guid?> Ref_SCM_ProductRequestDetails_FB_FACTNO_GUID;
public Input<int?> Ref_SCM_ProductRequestDetails_FB_FactorNum;
public Dropdown Ref_SCM_ProductRequestDetails_SH_FactBuy_DTL;
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
