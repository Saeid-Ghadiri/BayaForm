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
    public class Form_1037_CUPeropeties : FormBase
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
    public string? VersionForm { get; set; } = "5622";

    /// <summary>
    ///  موجودیت
    /// </summary>
    public Entity.HR_EMP_Employees? _Entity { get; set; } = new Entity.HR_EMP_Employees();

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

public ElementReference Ref_submit1;
public ElementReference Ref_submit;
public ElementReference Ref_submit2;
public ElementReference Ref_btn_Help;
public Dropdown Ref_BaseInfo_ORG_CompaniesId;
public Input<string?> Ref_NationalCode;
public Dropdown Ref_HR_EMP_StatusId;
public Input<string?> Ref_FirstName;
public Input<string?> Ref_EmployeeNo;
public Input<string?> Ref_EmployeePersonelNo;
public Input<string?> Ref_LastName;
public Input<string?> Ref_LastEmployeeNo;
public Input<string?> Ref_EmployeeLastPersonelNo;
public SingleUploader Ref_ProfilePicture;
public DxGrid? Grid_HR_EMP_EmployeeInfos;

public Dropdown Ref_HR_EMP_EmployeeInfos_HR_EMP_EmployeesId;
public Input<string?> Ref_HR_EMP_EmployeeInfos_BaseInfo_ORG_Companies;
public Input<string?> Ref_HR_EMP_EmployeeInfos_FirstName;
public Input<string?> Ref_HR_EMP_EmployeeInfos_LastName;
public Input<string?> Ref_HR_EMP_EmployeeInfos_NationalCode;
public Input<string?> Ref_HR_EMP_EmployeeInfos_EmployeeNo;
public Input<string?> Ref_HR_EMP_EmployeeInfos_LastEmployeeNo;
public Input<string?> Ref_HR_EMP_EmployeeInfos_EmployeePersonelNo;
public Input<string?> Ref_HR_EMP_EmployeeInfos_EmployeeLastPersonelNo;
public Input<string?> Ref_HR_EMP_EmployeeInfos_HR_EMP_Status;
public Input<string?> Ref_HR_EMP_EmployeeInfos_FatherName;
public Input<string?> Ref_HR_EMP_EmployeeInfos_IdCardNo;
public Input<string?> Ref_HR_EMP_EmployeeInfos_IdCardSerialNoSection1;
public Input<string?> Ref_HR_EMP_EmployeeInfos_IdCardSerialNoSection2;
public Input<string?> Ref_HR_EMP_EmployeeInfos_IdCardSerialNoSection3;
public Dropdown Ref_HR_EMP_EmployeeInfos_BaseInfo_MaritalStatusId;
public Dropdown Ref_HR_EMP_EmployeeInfos_CityOfIssue;
public Dropdown Ref_HR_EMP_EmployeeInfos_CityOfBirth;
public Input<string?> Ref_HR_EMP_EmployeeInfos_BirthDate_Fa;
public Input<byte?> Ref_HR_EMP_EmployeeInfos_BirthDateDD;
public Input<byte?> Ref_HR_EMP_EmployeeInfos_BirthDateMM;
public Input<short?> Ref_HR_EMP_EmployeeInfos_BirthDateYYYY;
public Input<DateTime?> Ref_HR_EMP_EmployeeInfos_BirthDate;
public Input<int?> Ref_HR_EMP_EmployeeInfos_EmployeeAge;
public Input<string?> Ref_HR_EMP_EmployeeInfos_EmployeeAgeText;
public SingleUploader Ref_HR_EMP_EmployeeInfos_EmployeeWorkExperienceFile;
public Input<string?> Ref_HR_EMP_EmployeeInfos_DateEmployeeWorkExperience_Fa;
public Input<int?> Ref_HR_EMP_EmployeeInfos_EmployeeWorkExperience;
public Input<string?> Ref_HR_EMP_EmployeeInfos_EmployeeWorkExperienceText;
public Dropdown Ref_HR_EMP_EmployeeInfos_BaseInfo_GenderId;
public Dropdown Ref_HR_EMP_EmployeeInfos_BaseInfo_BloodGroupId;
public Dropdown Ref_HR_EMP_EmployeeInfos_BaseInfo_ReligionId;
public Dropdown Ref_HR_EMP_EmployeeInfos_BaseInfo_DenominationsId;
public Input<bool?> Ref_HR_EMP_EmployeeInfos_HasDisabledChild;
public Input<bool?> Ref_HR_EMP_EmployeeInfos_Arzagh;
public Input<bool?> Ref_HR_EMP_EmployeeInfos_HasOrganizationalAutomobile;
public Input<bool?> Ref_HR_EMP_EmployeeInfos_HasCashCharge;
public Input<bool?> Ref_HR_EMP_EmployeeInfos_HasTransportService;
public Dropdown Ref_HR_EMP_EmployeeInfos_HR_Base_TransportServiceId;
public Dropdown Ref_HR_EMP_EmployeeInfos_BaseInfo_MilitaryStatusId;
public Input<string?> Ref_HR_EMP_EmployeeInfos_Mobile;
public Input<string?> Ref_HR_EMP_EmployeeInfos_Phone;
public Dropdown Ref_HR_EMP_EmployeeInfos_BaseInfo_CitiesAreasId;
public Input<string?> Ref_HR_EMP_EmployeeInfos_Address;
public Input<string?> Ref_HR_EMP_EmployeeInfos_ContactEmployee_FullName;
public Input<string?> Ref_HR_EMP_EmployeeInfos_ContactEmployee_Tel;
public Dropdown Ref_HR_EMP_EmployeeInfos_HR_Base_ContactEmployeeRelativeId;
public Input<string?> Ref_HR_EMP_EmployeeInfos_ContactEmployee_Address;
public Input<Guid> Ref_HR_EMP_EmployeeInfos_Id;
public Input<Guid?> Ref_HR_EMP_EmployeeInfos_CreateUser;
public Input<Guid?> Ref_HR_EMP_EmployeeInfos_RequestID;
public Input<Guid?> Ref_HR_EMP_EmployeeInfos_UpdateUser;
public Input<DateTime?> Ref_HR_EMP_EmployeeInfos_CreateDate;
public Input<DateTime?> Ref_HR_EMP_EmployeeInfos_UpdateDate;
public Input<bool?> Ref_HR_EMP_EmployeeInfos_IsDelete;
public Input<string?> Ref_HR_EMP_EmployeeInfos_BaseInfo_ORG_CompaniesId;
public Input<string?> Ref_HR_EMP_EmployeeInfos_HR_EMP_StatusId;
public DxGrid? Grid_HR_EMP_EmployeeFamileis;

public Input<Guid> Ref_HR_EMP_EmployeeFamileis_Id;
public Input<Guid?> Ref_HR_EMP_EmployeeFamileis_RequestID;
public Input<Guid?> Ref_HR_EMP_EmployeeFamileis_CreateUser;
public Input<Guid?> Ref_HR_EMP_EmployeeFamileis_UpdateUser;
public Input<DateTime?> Ref_HR_EMP_EmployeeFamileis_CreateDate;
public Input<DateTime?> Ref_HR_EMP_EmployeeFamileis_UpdateDate;
public Input<bool?> Ref_HR_EMP_EmployeeFamileis_IsDelete;
public Dropdown Ref_HR_EMP_EmployeeFamileis_HR_EMP_EmployeesId;
public Dropdown Ref_HR_EMP_EmployeeFamileis_HR_FamilyRelationshipId;
public Dropdown Ref_HR_EMP_EmployeeFamileis_HR_Base_DependentId;
public Input<string?> Ref_HR_EMP_EmployeeFamileis_FirstName;
public Input<string?> Ref_HR_EMP_EmployeeFamileis_LastName;
public Input<string?> Ref_HR_EMP_EmployeeFamileis_NationalCode;
public Input<string?> Ref_HR_EMP_EmployeeFamileis_FatherName;
public Input<string?> Ref_HR_EMP_EmployeeFamileis_IdCardNo;
public Input<string?> Ref_HR_EMP_EmployeeFamileis_BirthDate_Fa;
public Dropdown Ref_HR_EMP_EmployeeFamileis_BaseInfo_CitiesId;
public Dropdown Ref_HR_EMP_EmployeeFamileis_BaseInfo_GenderId;
public Dropdown Ref_HR_EMP_EmployeeFamileis_BaseInfo_MaritalStatusId;
public Input<string?> Ref_HR_EMP_EmployeeFamileis_MarriageDate_Fa;
public Input<string?> Ref_HR_EMP_EmployeeFamileis_DivorceDate_Fa;
public Input<string?> Ref_HR_EMP_EmployeeFamileis_SpouseDeathDate_Fa;
public Input<bool?> Ref_HR_EMP_EmployeeFamileis_IsCurrentlyStudying;
public Dropdown Ref_HR_EMP_EmployeeFamileis_HR_Base_AcademicDegreesId;
public Input<string?> Ref_HR_EMP_EmployeeFamileis_EducationStartDate_Fa;
public Input<string?> Ref_HR_EMP_EmployeeFamileis_EducationEndDate_Fa;
public SingleUploader Ref_HR_EMP_EmployeeFamileis_StudyCertificateFile;
public Input<string?> Ref_HR_EMP_EmployeeFamileis_Description;
public Input<DateTime?> Ref_HR_EMP_EmployeeFamileis_DivorceDate;
public Input<DateTime?> Ref_HR_EMP_EmployeeFamileis_SpouseDeathDate;
public Input<DateTime?> Ref_HR_EMP_EmployeeFamileis_MarriageDate;
public Input<DateTime?> Ref_HR_EMP_EmployeeFamileis_EducationStartDate;
public Input<DateTime?> Ref_HR_EMP_EmployeeFamileis_EducationEndDate;
public Input<string?> Ref_HR_EMP_EmployeeFamileis_BirthDate;
public DxGrid? Grid_HR_Base_BankAccount;

public Dropdown Ref_HR_Base_BankAccount_HR_Base_AccountHolderTypeId;
public Dropdown Ref_HR_Base_BankAccount_HR_EMP_EmployeesId;
public Dropdown Ref_HR_Base_BankAccount_HR_EMP_EmployeeFamileisId;
public Dropdown Ref_HR_Base_BankAccount_HR_Base_BankAcountCategoryId;
public Dropdown Ref_HR_Base_BankAccount_BaseInfo_BankAccountTypeId;
public Dropdown Ref_HR_Base_BankAccount_BaseInfo_BankId;
public Dropdown Ref_HR_Base_BankAccount_BaseInfo_BankBranchesId;
public Input<string?> Ref_HR_Base_BankAccount_BankAccountNumber;
public Input<string?> Ref_HR_Base_BankAccount_IBAN;
public Input<string?> Ref_HR_Base_BankAccount_CartNo;
public Input<Guid> Ref_HR_Base_BankAccount_Id;
public Input<Guid?> Ref_HR_Base_BankAccount_RequestID;
public Input<Guid?> Ref_HR_Base_BankAccount_CreateUser;
public Input<Guid?> Ref_HR_Base_BankAccount_UpdateUser;
public Input<DateTime?> Ref_HR_Base_BankAccount_CreateDate;
public Input<DateTime?> Ref_HR_Base_BankAccount_UpdateDate;
public Input<bool?> Ref_HR_Base_BankAccount_IsDelete;
public DxGrid? Grid_HR_EMP_Documents;

public Dropdown Ref_HR_EMP_Documents_HR_EMP_EmployeesId;
public Input<string?> Ref_HR_EMP_Documents_Title;
public Dropdown Ref_HR_EMP_Documents_HR_Base_DocumentTypesId;
public FileUploadPage.Uploader<Entity.HR_EMP_Documents_AttachmentFile> Ref_HR_EMP_Documents_HR_EMP_Documents_AttachmentFile;
public Input<string?> Ref_HR_EMP_Documents_Description;
public Input<bool?> Ref_HR_EMP_Documents_IsActive;
public Input<Guid> Ref_HR_EMP_Documents_Id;
public Input<Guid?> Ref_HR_EMP_Documents_RequestID;
public Input<Guid?> Ref_HR_EMP_Documents_CreateUser;
public Input<Guid?> Ref_HR_EMP_Documents_UpdateUser;
public Input<DateTime?> Ref_HR_EMP_Documents_CreateDate;
public Input<DateTime?> Ref_HR_EMP_Documents_UpdateDate;
public Input<bool?> Ref_HR_EMP_Documents_IsDelete;
public DxGrid? Grid_HR_EMP_AcademicDocuments;

public Dropdown Ref_HR_EMP_AcademicDocuments_HR_EMP_EmployeesId;
public Dropdown Ref_HR_EMP_AcademicDocuments_HR_Base_AcademicDegreesId;
public Dropdown Ref_HR_EMP_AcademicDocuments_HR_Base_UniversityId;
public Input<string?> Ref_HR_EMP_AcademicDocuments_DocumentSerialNo;
public Input<string?> Ref_HR_EMP_AcademicDocuments_StartDate;
public Input<string?> Ref_HR_EMP_AcademicDocuments_EndDate;
public Input<decimal?> Ref_HR_EMP_AcademicDocuments_Score;
public Input<DateTime?> Ref_HR_EMP_AcademicDocuments_ActivedDateTime;
public Input<bool?> Ref_HR_EMP_AcademicDocuments_HasInquiring;
public Input<string?> Ref_HR_EMP_AcademicDocuments_InquiringDate;
public Dropdown Ref_HR_EMP_AcademicDocuments_HR_Base_SpecializationId;
public Input<bool?> Ref_HR_EMP_AcademicDocuments_IsActive;
public Input<Guid> Ref_HR_EMP_AcademicDocuments_Id;
public Input<Guid?> Ref_HR_EMP_AcademicDocuments_RequestID;
public Input<Guid?> Ref_HR_EMP_AcademicDocuments_CreateUser;
public Input<Guid?> Ref_HR_EMP_AcademicDocuments_UpdateUser;
public Input<DateTime?> Ref_HR_EMP_AcademicDocuments_CreateDate;
public Input<DateTime?> Ref_HR_EMP_AcademicDocuments_UpdateDate;
public Input<bool?> Ref_HR_EMP_AcademicDocuments_IsDelete;
public DxGrid? Grid_HR_EMP_EmployeeDetails;

public Dropdown Ref_HR_EMP_EmployeeDetails_HR_EMP_EmployeesId;
public Input<string?> Ref_HR_EMP_EmployeeDetails_OrgTel1;
public Input<string?> Ref_HR_EMP_EmployeeDetails_OrgTel2;
public Input<string?> Ref_HR_EMP_EmployeeDetails_OrgTel3;
public Input<string?> Ref_HR_EMP_EmployeeDetails_OrgTel4;
public Input<string?> Ref_HR_EMP_EmployeeDetails_OrgTel5;
public Input<string?> Ref_HR_EMP_EmployeeDetails_OrgMobile1;
public Input<string?> Ref_HR_EMP_EmployeeDetails_OrgMobile2;
public Input<string?> Ref_HR_EMP_EmployeeDetails_InsideEmail;
public Input<string?> Ref_HR_EMP_EmployeeDetails_OutsideEmail;
public Input<Guid> Ref_HR_EMP_EmployeeDetails_Id;
public Input<Guid?> Ref_HR_EMP_EmployeeDetails_RequestID;
public Input<Guid?> Ref_HR_EMP_EmployeeDetails_CreateUser;
public Input<Guid?> Ref_HR_EMP_EmployeeDetails_UpdateUser;
public Input<DateTime?> Ref_HR_EMP_EmployeeDetails_CreateDate;
public Input<DateTime?> Ref_HR_EMP_EmployeeDetails_UpdateDate;
public Input<bool?> Ref_HR_EMP_EmployeeDetails_IsDelete;
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
