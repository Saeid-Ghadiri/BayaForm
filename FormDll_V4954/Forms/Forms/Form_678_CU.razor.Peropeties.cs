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
    public class Form_678_CUPeropeties : FormBase
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
    public Entity.HR_EMP_EmployeeInfos? _Entity { get; set; } = new Entity.HR_EMP_EmployeeInfos();

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

public Dropdown Ref_HR_EMP_EmployeesId;
public Input<string?> Ref_FirstName;
public Input<string?> Ref_LastName;
public Input<string?> Ref_NationalCode;
public Input<string?> Ref_IdCardNo;
public Input<string?> Ref_IdCardSerialNo;
public Input<string?> Ref_FatherName;
public Input<short?> Ref_BirthDateYYYY;
public Input<byte?> Ref_BirthDateMM;
public Input<byte?> Ref_BirthDateDD;
public Dropdown Ref_BaseInfo_GenderId;
public Input<bool?> Ref_MartyrsFamily;
public Input<bool?> Ref_MartyrsChild;
public Input<bool?> Ref_GordanAshoora;
public Input<bool?> Ref_MazadTahsilJebhe6Mah;
public Input<bool?> Ref_SupplementaryInsurance;
public Input<bool?> Ref_lifeInsurance;
public Input<bool?> Ref_AccidentInsurance;
public Dropdown Ref_HR_Base_TransportServiceId;
public Input<bool?> Ref_Arzagh;
public Input<bool?> Ref_HasTransportService;
public Input<bool?> Ref_HasOrganizationalAutomobile;
public Input<bool?> Ref_MoafiatMaliatMode88;
public Input<bool?> Ref_MoafMaliat;
public Input<bool?> Ref_HasDisabledChild;
public Input<bool?> Ref_HasCashCharge;
public Input<string?> Ref_ContactEmployee_FullName;
public Dropdown Ref_HR_Base_ContactEmployeeRelativeId;
public Input<string?> Ref_ContactEmployee_Tel;
public Input<string?> Ref_ContactEmployee_Address;
public Input<bool?> Ref_IsActive;
public Dropdown Ref_HR_Base_RelativeSelfSacerificId;
public Input<string?> Ref_IsargarName;
public Input<string?> Ref_IsargarFamily;
public Input<short?> Ref_Relatives_Jebhe_Days;
public Input<short?> Ref_Relatives_Captivity_Days;
public Input<short?> Ref_JebheMotavali_Days;
public Input<short?> Ref_JebheMotanaveb_Days;
public Input<decimal?> Ref_Relatives_VeteranPercentage;
public Input<short?> Ref_Captivity_Days;
public Input<decimal?> Ref_VeteranPercentage;
public Input<int?> Ref_SanavatEnteghali_Day;
public Input<DateTime?> Ref_EmploymentDate;
public Input<DateTime?> Ref_EmploymentDateInGroup;
public Input<DateTime?> Ref_EmploymentStartDate;
public Input<string?> Ref_Phone;
public Input<string?> Ref_Mobile;
public Input<string?> Ref_Address;
public Input<Guid> Ref_Id;
public Input<Guid?> Ref_RequestID;
public Input<Guid?> Ref_CreateUser;
public Input<Guid?> Ref_UpdateUser;
public Input<DateTime?> Ref_CreateDate;
public Input<DateTime?> Ref_UpdateDate;
public Input<bool?> Ref_IsDelete;
public Input<string?> Ref_EmploymentDate_Fa;
public Input<string?> Ref_EmploymentDateInGroup_Fa;
public Input<string?> Ref_EmploymentStartDate_Fa;
public Input<double?> Ref_DailyEmploymentDate;
public Input<double?> Ref_DailyEmploymentDateInGroup;


    #endregion

 

}
}
