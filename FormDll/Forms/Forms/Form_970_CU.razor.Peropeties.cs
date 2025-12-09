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
    public class Form_970_CUPeropeties : FormBase
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
    public string? VersionForm { get; set; } = "5145";

    /// <summary>
    ///  موجودیت
    /// </summary>
    public Entity.HR_CVR_RecruitmentRules? _Entity { get; set; } = new Entity.HR_CVR_RecruitmentRules();

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
public Dropdown Ref_HR_ORG_SectionsId;
public Dropdown Ref_HR_ORG_PostsId;
public Dropdown Ref_HR_CVR_JobId;
public Dropdown Ref_HR_CVR_JobGroupId;
public Dropdown Ref_HR_CVR_JobSalaryRankId;
public Dropdown Ref_HR_CVR_SalaryHistoryId;
public Dropdown Ref_HR_CVR_RightGuardianshipId;
public Dropdown Ref_HR_CVR_ApprovalsMinistryLaborGroupId;
public Input<decimal?> Ref_JobSalaryRank;
public Input<decimal?> Ref_SalaryHistory;
public Input<int?> Ref_RightGuardianship;
public Input<int?> Ref_CoefficientDurabilityPost;
public Input<int?> Ref_CoefficientDifficultAndHarmfulJobs;
public Input<int?> Ref_TotalDailyBaseWage;
public Input<decimal?> Ref_JobSalaryRankNew;
public Input<decimal?> Ref_SalaryHistoryNew;
public Input<decimal?> Ref_RightGuardianshipNew;
public Input<decimal?> Ref_CoefficientDurabilityPostNew;
public Input<decimal?> Ref_CoefficientDifficultAndHarmfulJobsNew;
public Input<double?> Ref_TotalDailyBaseWageNew;
public Input<int?> Ref_DailyAdjustmentDifference;
public Input<decimal?> Ref_RecruitmentAllowance;
public Input<int?> Ref_MinistryLabourRightHousing;
public Input<int?> Ref_MinistryLaborRightFood;
public Input<int?> Ref_RightMarryMinistryLabor;
public Input<int?> Ref_ChildrensRightsMinistryLabor;
public Input<int?> Ref_WelfareMotivationalBenefits;
public Input<int?> Ref_OtherBenefits;
public Input<int?> Ref_TotalMonthlySalaryBenefits;
public Input<decimal?> Ref_DailyAdjustmentDifferenceNew;
public Input<decimal?> Ref_RecruitmentAllowanceNew;
public Input<decimal?> Ref_MinistryLabourRightHousingNew;
public Input<decimal?> Ref_MinistryLaborRightFoodNew;
public Input<decimal?> Ref_RightMarryMinistryLaborNew;
public Input<decimal?> Ref_ChildrensRightsMinistryLaborNew;
public Input<decimal?> Ref_WelfareMotivationalBenefitsNew;
public Input<decimal?> Ref_OtherBenefitsNew;
public Input<decimal?> Ref_TotalMonthlySalaryBenefitsNew;
public Input<Guid> Ref_Id;
public Input<Guid?> Ref_RequestID;
public Input<Guid?> Ref_CreateUser;
public Input<Guid?> Ref_UpdateUser;
public Input<DateTime?> Ref_CreateDate;
public Input<DateTime?> Ref_UpdateDate;
public Input<bool?> Ref_IsDelete;
public Dropdown Ref_HR_CVR_VerdictRecruitingId;


    #endregion

 

}
}
