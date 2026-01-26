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
    public class Form_798_CUPeropeties : FormBase
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
    public string? VersionForm { get; set; } = "5581";

    /// <summary>
    ///  موجودیت
    /// </summary>
    public Entity.HR_RMS_HiringQuestionnaire? _Entity { get; set; } = new Entity.HR_RMS_HiringQuestionnaire();

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

public Input<string?> Ref_FirstName;
public Input<string?> Ref_NationalCode;
public Dropdown Ref_HR_HousingSituationId;
public Input<byte?> Ref_Weight;
public Input<string?> Ref_Lastname;
public Input<string?> Ref_BirthCertificateNumber;
public Dropdown Ref_BaseInfo_GenderId;
public Input<byte?> Ref_Height;
public Input<string?> Ref_FatherName;
public Dropdown Ref_BaseInfo_CitiesId;
public Input<bool?> Ref_Married;
public Dropdown Ref_BaseInfo_BloodGroupId;
public Input<string?> Ref_BirthDate;
public Dropdown Ref_HR_Base_CountriesId;
public Input<byte?> Ref_NumberChildren;
public Dropdown Ref_BaseInfo_ReligionId;
public Dropdown Ref_BaseInfo_CitiesAddressId;
public Input<short?> Ref_HouseNumber;
public Input<string?> Ref_Village;
public Input<byte?> Ref_Floor;
public Input<string?> Ref_Street;
public Input<byte?> Ref_Unit;
public Input<string?> Ref_Alley;
public Input<string?> Ref_PhoneNumber;
public Dropdown Ref_BaseInfo_MilitaryStatusId;
public Input<string?> Ref_MilitaryServicesPlace;
public Input<byte?> Ref_MilitaryServicesTime;
public Dropdown Ref_HR_MilitaryServicesExemptionTypeId;
public Input<string?> Ref_MilitaryServicesStartDate;
public Input<string?> Ref_MilitaryServicesEndDate;
public DxGrid? Grid_HR_RMS_EducationalRecords;

public Input<Guid> Ref_HR_RMS_EducationalRecords_Id;
public Input<Guid?> Ref_HR_RMS_EducationalRecords_RequestID;
public Input<Guid?> Ref_HR_RMS_EducationalRecords_CreateUser;
public Input<Guid?> Ref_HR_RMS_EducationalRecords_UpdateUser;
public Input<DateTime?> Ref_HR_RMS_EducationalRecords_CreateDate;
public Input<DateTime?> Ref_HR_RMS_EducationalRecords_UpdateDate;
public Input<bool?> Ref_HR_RMS_EducationalRecords_IsDelete;
public Dropdown Ref_HR_RMS_EducationalRecords_HR_Base_AcademicDegreesId;
public Input<string?> Ref_HR_RMS_EducationalRecords_FromDate;
public Input<string?> Ref_HR_RMS_EducationalRecords_ToDate;
public Input<string?> Ref_HR_RMS_EducationalRecords_NameEducationalInstitution;
public Dropdown Ref_HR_RMS_EducationalRecords_HR_Base_UniversityTypeId;
public Input<double?> Ref_HR_RMS_EducationalRecords_GPA;
public Input<string?> Ref_HR_RMS_EducationalRecords_CityStudy;
public Input<string?> Ref_HR_RMS_EducationalRecords_FieldStudySpecialization;
public RadioBoolean Ref_EducationalStatus;
public Input<string?> Ref_AcademicDegrees;
public Input<string?> Ref_YearEntry;
public Input<bool?> Ref_DecisionToContinueStudying;
public DxGrid? Grid_HR_RMS_JobHistory;

public Input<Guid> Ref_HR_RMS_JobHistory_Id;
public Input<Guid?> Ref_HR_RMS_JobHistory_RequestID;
public Input<Guid?> Ref_HR_RMS_JobHistory_CreateUser;
public Input<Guid?> Ref_HR_RMS_JobHistory_UpdateUser;
public Input<DateTime?> Ref_HR_RMS_JobHistory_CreateDate;
public Input<DateTime?> Ref_HR_RMS_JobHistory_UpdateDate;
public Input<bool?> Ref_HR_RMS_JobHistory_IsDelete;
public Input<string?> Ref_HR_RMS_JobHistory_StartWork;
public Input<string?> Ref_HR_RMS_JobHistory_EndWork;
public Input<string?> Ref_HR_RMS_JobHistory_CompanyName;
public Input<string?> Ref_HR_RMS_JobHistory_TypeActivity;
public Input<string?> Ref_HR_RMS_JobHistory_LastJobTitle;
public Input<int?> Ref_HR_RMS_JobHistory_LastSalary;
public Input<bool?> Ref_HR_RMS_JobHistory_HasInsurance;
public Input<string?> Ref_HR_RMS_JobHistory_ReasonBreakup;
public Input<string?> Ref_HR_RMS_JobHistory_NameDirectManager;
public Input<string?> Ref_HR_RMS_JobHistory_WorkPhone;
public Input<string?> Ref_HR_RMS_JobHistory_InsuranceTypes;
public DxGrid? Grid_HR_RMS_TrainingCourses;

public Input<Guid> Ref_HR_RMS_TrainingCourses_Id;
public Input<Guid?> Ref_HR_RMS_TrainingCourses_RequestID;
public Input<Guid?> Ref_HR_RMS_TrainingCourses_CreateUser;
public Input<Guid?> Ref_HR_RMS_TrainingCourses_UpdateUser;
public Input<DateTime?> Ref_HR_RMS_TrainingCourses_CreateDate;
public Input<DateTime?> Ref_HR_RMS_TrainingCourses_UpdateDate;
public Input<bool?> Ref_HR_RMS_TrainingCourses_IsDelete;
public Input<string?> Ref_HR_RMS_TrainingCourses_TitleCourse;
public Input<string?> Ref_HR_RMS_TrainingCourses_EducationalInstitution;
public Input<string?> Ref_HR_RMS_TrainingCourses_CourseYear;
public Input<byte?> Ref_HR_RMS_TrainingCourses_CourseDuration;
public RadioBoolean Ref_HR_RMS_TrainingCourses_HasCertificate;
public DxGrid? Grid_HR_RMS_FamiliarityForeigLanguages;

public Input<Guid> Ref_HR_RMS_FamiliarityForeigLanguages_Id;
public Input<Guid?> Ref_HR_RMS_FamiliarityForeigLanguages_RequestID;
public Input<Guid?> Ref_HR_RMS_FamiliarityForeigLanguages_CreateUser;
public Input<Guid?> Ref_HR_RMS_FamiliarityForeigLanguages_UpdateUser;
public Input<DateTime?> Ref_HR_RMS_FamiliarityForeigLanguages_CreateDate;
public Input<DateTime?> Ref_HR_RMS_FamiliarityForeigLanguages_UpdateDate;
public Input<bool?> Ref_HR_RMS_FamiliarityForeigLanguages_IsDelete;
public Input<string?> Ref_HR_RMS_FamiliarityForeigLanguages_NameLanguage;
public Input<bool?> Ref_HR_RMS_FamiliarityForeigLanguages_SpecializationLiteraryField;
public Input<bool?> Ref_HR_RMS_FamiliarityForeigLanguages_ExpertiseTechnicalField;
public Input<bool?> Ref_HR_RMS_FamiliarityForeigLanguages_SpecializationBusiness;
public Dropdown Ref_HR_RMS_FamiliarityForeigLanguages_HR_ReadingId;
public Dropdown Ref_HR_RMS_FamiliarityForeigLanguages_HR_TalkId;
public Dropdown Ref_HR_RMS_FamiliarityForeigLanguages_HR_TranslationId;
public DxGrid? Grid_HR_RMS_FirstDegreeRelatives;

public Input<Guid> Ref_HR_RMS_FirstDegreeRelatives_Id;
public Input<Guid?> Ref_HR_RMS_FirstDegreeRelatives_RequestID;
public Input<Guid?> Ref_HR_RMS_FirstDegreeRelatives_CreateUser;
public Input<Guid?> Ref_HR_RMS_FirstDegreeRelatives_UpdateUser;
public Input<DateTime?> Ref_HR_RMS_FirstDegreeRelatives_CreateDate;
public Input<DateTime?> Ref_HR_RMS_FirstDegreeRelatives_UpdateDate;
public Input<bool?> Ref_HR_RMS_FirstDegreeRelatives_IsDelete;
public Dropdown Ref_HR_RMS_FirstDegreeRelatives_HR_FamilyRelationshipId;
public Input<string?> Ref_HR_RMS_FirstDegreeRelatives_FirstName;
public Input<string?> Ref_HR_RMS_FirstDegreeRelatives_LastName;
public Input<byte?> Ref_HR_RMS_FirstDegreeRelatives_Age;
public Input<string?> Ref_HR_RMS_FirstDegreeRelatives_Job;
public Input<string?> Ref_HR_RMS_FirstDegreeRelatives_NameOrganization;
public Input<bool?> Ref_HR_RMS_FirstDegreeRelatives_UnderFamilyCare;
public Dropdown Ref_HR_RMS_FirstDegreeRelatives_HR_Base_AcademicDegreesId;
public DxGrid? Grid_HR_RMS_FriendsAcquaintances;

public Input<Guid> Ref_HR_RMS_FriendsAcquaintances_Id;
public Input<Guid?> Ref_HR_RMS_FriendsAcquaintances_RequestID;
public Input<Guid?> Ref_HR_RMS_FriendsAcquaintances_CreateUser;
public Input<Guid?> Ref_HR_RMS_FriendsAcquaintances_UpdateUser;
public Input<DateTime?> Ref_HR_RMS_FriendsAcquaintances_CreateDate;
public Input<DateTime?> Ref_HR_RMS_FriendsAcquaintances_UpdateDate;
public Input<bool?> Ref_HR_RMS_FriendsAcquaintances_IsDelete;
public Input<string?> Ref_HR_RMS_FriendsAcquaintances_FirstName;
public Input<string?> Ref_HR_RMS_FriendsAcquaintances_LastName;
public Input<string?> Ref_HR_RMS_FriendsAcquaintances_Job;
public Input<string?> Ref_HR_RMS_FriendsAcquaintances_Address;
public Input<string?> Ref_HR_RMS_FriendsAcquaintances_Phone;
public Input<string?> Ref_FamilyRelationship;
public Input<string?> Ref_FullNameFamily;
public Input<bool?> Ref_FamiliarPersonInsideTheCompany;
public Input<string?> Ref_FamiliarPersonFullName;
public Dropdown Ref_HR_HowToKnowCompanyId;
public Input<short?> Ref_PhoneNumberFamily;
public RadioBoolean Ref_MedicalSurgery;
public Input<string?> Ref_ExplanationMedicalSurgery;
public RadioBoolean Ref_HistorySpecificMedicalDiseases;
public Input<string?> Ref_ExplanationMedicalDiseases;
public RadioBoolean Ref_OrganFailure;
public Input<string?> Ref_ExplanationOrganFailure;
public RadioBoolean Ref_UseGlasses;
public Input<string?> Ref_ExplanationUseGlasses;
public RadioBoolean Ref_Smoking;
public Input<string?> Ref_ExplanationSmoking;
public Input<string?> Ref_IndividualStrengths;
public Input<string?> Ref_FeaturesThatCanBeImproved;
public Input<string?> Ref_LeisureTime;
public Dropdown Ref_HR_SocialActivityId;
public Input<string?> Ref_ExplanationSocialActivity;
public Input<bool?> Ref_DriverLicense;
public Input<string?> Ref_ExplanationDriverLicense;
public Input<bool?> Ref_CriminalConviction;
public Input<string?> Ref_ExplanationCriminalConviction;
public Input<bool?> Ref_FinancialCommitment;
public Input<string?> Ref_Type1FinancialCommitment;
public Input<int?> Ref_Amount1FinancialCommitment;
public Input<int?> Ref_Installments1FinancialCommitment;
public Input<string?> Ref_Type2FinancialCommitment;
public Input<int?> Ref_Amount2FinancialCommitment;
public Input<int?> Ref_Installments2FinancialCommitment;
public Input<string?> Ref_Type3FinancialCommitment;
public Input<int?> Ref_Amount3FinancialCommitment;
public Input<int?> Ref_Installments3FinancialCommitment;
public Input<string?> Ref_ReasonLeavingCurrentJob;
public Input<string?> Ref_ReasonChoosingCompany;
public Input<bool?> Ref_DesireShiftWork;
public Input<string?> Ref_InterestAbility;
public Input<bool?> Ref_BusyWithWorking;
public Input<string?> Ref_FromDate;
public Input<int?> Ref_RequestedMonthlySalary;
public Input<string?> Ref_SuggestedJob;
public Input<Guid> Ref_Id;
public Dropdown Ref_RegisterUserId;
public Input<Guid?> Ref_RequestID;
public Input<Guid?> Ref_CreateUser;
public Input<Guid?> Ref_UpdateUser;
public Input<DateTime?> Ref_CreateDate;
public Input<DateTime?> Ref_UpdateDate;
public Input<bool?> Ref_IsDelete;


    #endregion

 

}
}
