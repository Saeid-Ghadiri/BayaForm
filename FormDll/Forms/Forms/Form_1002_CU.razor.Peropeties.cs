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
    public class Form_1002_CUPeropeties : FormBase
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
    public string? VersionForm { get; set; } = "5491";

    /// <summary>
    ///  موجودیت
    /// </summary>
    public Entity.HR_CVR_VerdictRecruiting? _Entity { get; set; } = new Entity.HR_CVR_VerdictRecruiting();

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
public ElementReference Ref_submit;
public ElementReference Ref_submit1;
public Dropdown Ref_HR_CVR_PersonnelContractId;
public Dropdown Ref_HR_CVR_VerdictRecruitingId;
public Dropdown Ref_HR_CVR_TypesRulingsId;
public Dropdown Ref_HR_Base_InsuranceTypesId;
public Input<string?> Ref_InsuranceNumber;
public Dropdown Ref_HR_StatusVerdictRecruitingId;
public RadioBoolean Ref_TypeBonusPayment;
public DxGrid? Grid_HR_CVR_EveryPostVerdict;

public Dropdown Ref_HR_CVR_EveryPostVerdict_HR_ORG_SectionsId;
public RadioBoolean Ref_HR_CVR_EveryPostVerdict_SectionsType;
public Dropdown Ref_HR_CVR_EveryPostVerdict_HR_ORG_PostsId;
public RadioBoolean Ref_HR_CVR_EveryPostVerdict_PostType;
public Input<Guid> Ref_HR_CVR_EveryPostVerdict_Id;
public Input<Guid?> Ref_HR_CVR_EveryPostVerdict_RequestID;
public Input<Guid?> Ref_HR_CVR_EveryPostVerdict_CreateUser;
public Input<Guid?> Ref_HR_CVR_EveryPostVerdict_UpdateUser;
public Input<DateTime?> Ref_HR_CVR_EveryPostVerdict_CreateDate;
public Input<DateTime?> Ref_HR_CVR_EveryPostVerdict_UpdateDate;
public Input<bool?> Ref_HR_CVR_EveryPostVerdict_IsDelete;
public Dropdown Ref_HR_CVR_EveryPostVerdict_HR_CVR_VerdictRecruitingId;
public Dropdown Ref_RegisterUserId;
public Input<string?> Ref_RegisterTime;
public Dropdown Ref_ConfirmerUserId;
public Input<string?> Ref_ConfirmerTime;
public Dropdown Ref_ApproverUserId;
public Input<string?> Ref_ApproverTime;
public Dropdown Ref_NullifierUserId;
public Input<string?> Ref_NullifierTime;
public Input<string?> Ref_ExecutionDateSentence_Fa;
public Input<DateTime?> Ref_ExecutionDateSentence;
public Dropdown Ref_HR_CVR_JobId;
public Dropdown Ref_HR_CVR_JobGroupId;
public DxGrid? Grid_HR_CVR_RecruitmentRules;

public Dropdown Ref_HR_CVR_RecruitmentRules_HR_EMP_EmployeesId;
public Input<string?> Ref_HR_CVR_RecruitmentRules_EmployeeNo;
public Input<string?> Ref_HR_CVR_RecruitmentRules_EmployeePersonelNo;
public Input<string?> Ref_HR_CVR_RecruitmentRules_FirstName;
public Input<string?> Ref_HR_CVR_RecruitmentRules_LastName;
public Input<string?> Ref_HR_CVR_RecruitmentRules_FatherName;
public Input<string?> Ref_HR_CVR_RecruitmentRules_BaseInfo_GenderId;
public Input<string?> Ref_HR_CVR_RecruitmentRules_IdCardNo;
public Input<string?> Ref_HR_CVR_RecruitmentRules_NationalCode;
public Input<string?> Ref_HR_CVR_RecruitmentRules_CityOfIssue;
public Input<string?> Ref_HR_CVR_RecruitmentRules_CityOfBirth;
public Input<string?> Ref_HR_CVR_RecruitmentRules_BirthDate_Fa;
public Input<string?> Ref_HR_CVR_RecruitmentRules_EmployeeAgeText;
public Input<string?> Ref_HR_CVR_RecruitmentRules_BaseInfo_MaritalStatusId;
public Input<string?> Ref_HR_CVR_RecruitmentRules_EmploymentDateInGroup_Fa;
public Input<string?> Ref_HR_CVR_RecruitmentRules_EmploymentDate_Fa;
public Input<string?> Ref_HR_CVR_RecruitmentRules_EmploymentStartDate_Fa;
public Input<string?> Ref_HR_CVR_RecruitmentRules_HR_Base_AcademicDegreesId;
public Input<string?> Ref_HR_CVR_RecruitmentRules_BaseInfo_MilitaryStatusId;
public Input<string?> Ref_HR_CVR_RecruitmentRules_InsuranceNumber;
public Input<string?> Ref_HR_CVR_RecruitmentRules_BankAccountNumber;
public Input<string?> Ref_HR_CVR_RecruitmentRules_IBAN;
public Dropdown Ref_HR_CVR_RecruitmentRules_HR_ORG_SectionsId;
public Dropdown Ref_HR_CVR_RecruitmentRules_HR_ORG_PostsId;
public Dropdown Ref_HR_CVR_RecruitmentRules_HR_CVR_ApprovalsMinistryLaborGroupId;
public Dropdown Ref_HR_CVR_RecruitmentRules_HR_CVR_JobId;
public Dropdown Ref_HR_CVR_RecruitmentRules_HR_CVR_JobGroupId;
public Dropdown Ref_HR_CVR_RecruitmentRules_HR_CVR_JobSalaryRankId;
public Dropdown Ref_HR_CVR_RecruitmentRules_HR_CVR_RightGuardianshipId;
public Dropdown Ref_HR_CVR_RecruitmentRules_HR_CVR_SalaryHistoryId;
public Input<int?> Ref_HR_CVR_RecruitmentRules_EmployeeChildrenCount;
public Input<bool?> Ref_HR_CVR_RecruitmentRules_IsChildAllowanceGrantedToEmployee;
public Input<string?> Ref_HR_CVR_RecruitmentRules_FirstChildAllowanceEstablishmentDate_Fa;
public Input<bool?> Ref_HR_CVR_RecruitmentRules_IsMarriageAllowanceGrantedToEmployee;
public Input<string?> Ref_HR_CVR_RecruitmentRules_FirstMarriageAllowanceEstablishmentDate_Fa;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_JobSalaryRank;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_JobSalaryRankNew;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_RankSalary;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_RankSalaryNew;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_SalaryHistory;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_SalaryHistoryNew;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_RightGuardianship;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_RightGuardianshipNew;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_CoefficientDurabilityPost;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_CoefficientDurabilityPostNew;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_CoefficientDifficultAndHarmfulJobs;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_CoefficientDifficultAndHarmfulJobsNew;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_TotalDailyBaseWage;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_TotalDailyBaseWageNew;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_DailyAdjustmentDifference;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_DailyAdjustmentDifferenceNew;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_RecruitmentAllowance;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_RecruitmentAllowanceNew;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_MinistryLaborRightFood;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_MinistryLaborRightFoodNew;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_MinistryLabourRightHousing;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_MinistryLabourRightHousingNew;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_RightMarryMinistryLabor;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_RightMarryMinistryLaborNew;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_ChildrensRightsMinistryLabor;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_ChildrensRightsMinistryLaborNew;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_WelfareMotivationalBenefits;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_WelfareMotivationalBenefitsNew;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_OtherBenefits;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_OtherBenefitsNew;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_TotalMonthlySalaryBenefits;
public Input<decimal?> Ref_HR_CVR_RecruitmentRules_TotalMonthlySalaryBenefitsNew;
public Dropdown Ref_HR_CVR_RecruitmentRules_HR_CVR_LaborCouncilFixedValuesId;
public Dropdown Ref_HR_CVR_RecruitmentRules_HR_CVR_AnnualBaseWageIncreaseRatesId;
public Input<bool?> Ref_HR_CVR_RecruitmentRules_HasSupervisionRight;
public Input<string?> Ref_HR_CVR_RecruitmentRules_SupervisionRightStartDate_Fa;
public Input<DateTime?> Ref_HR_CVR_RecruitmentRules_SupervisionRightStartDate;
public Input<string?> Ref_HR_CVR_RecruitmentRules_SupervisionRightCancellationDate_Fa;
public Input<DateTime?> Ref_HR_CVR_RecruitmentRules_SupervisionRightCancellationDate;
public Input<bool?> Ref_HR_CVR_RecruitmentRules_HasPostRetentionRight;
public Input<string?> Ref_HR_CVR_RecruitmentRules_PostRetentionStartDate_Fa;
public Input<DateTime?> Ref_HR_CVR_RecruitmentRules_PostRetentionStartDate;
public Input<string?> Ref_HR_CVR_RecruitmentRules_PostRetentionCancellationDate_Fa;
public Input<DateTime?> Ref_HR_CVR_RecruitmentRules_PostRetentionCancellationDate;
public Input<Guid> Ref_HR_CVR_RecruitmentRules_Id;
public Input<Guid?> Ref_HR_CVR_RecruitmentRules_RequestID;
public Input<Guid?> Ref_HR_CVR_RecruitmentRules_CreateUser;
public Input<Guid?> Ref_HR_CVR_RecruitmentRules_UpdateUser;
public Input<DateTime?> Ref_HR_CVR_RecruitmentRules_CreateDate;
public Input<DateTime?> Ref_HR_CVR_RecruitmentRules_UpdateDate;
public Input<bool?> Ref_HR_CVR_RecruitmentRules_IsDelete;
public Input<DateTime?> Ref_HR_CVR_RecruitmentRules_FirstChildAllowanceEstablishmentDate;
public Input<DateTime?> Ref_HR_CVR_RecruitmentRules_FirstMarriageAllowanceEstablishmentDate;
public DxGrid? Grid_HR_CVR_DescriptionRulings;

public Input<string?> Ref_HR_CVR_DescriptionRulings_Title;
public Input<bool?> Ref_HR_CVR_DescriptionRulings_IsActive;
public Input<Guid> Ref_HR_CVR_DescriptionRulings_Id;
public Input<Guid?> Ref_HR_CVR_DescriptionRulings_RequestID;
public Input<Guid?> Ref_HR_CVR_DescriptionRulings_CreateUser;
public Input<Guid?> Ref_HR_CVR_DescriptionRulings_UpdateUser;
public Input<DateTime?> Ref_HR_CVR_DescriptionRulings_CreateDate;
public Input<DateTime?> Ref_HR_CVR_DescriptionRulings_UpdateDate;
public Input<bool?> Ref_HR_CVR_DescriptionRulings_IsDelete;
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
