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
    public class Form_955_CUPeropeties : FormBase
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
    public Entity.HR_JobInterview? _Entity { get; set; } = new Entity.HR_JobInterview();

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
public Input<Guid?> Ref_RequestID;
public Input<Guid?> Ref_CreateUser;
public Input<Guid?> Ref_UpdateUser;
public Input<DateTime?> Ref_CreateDate;
public Input<DateTime?> Ref_UpdateDate;
public Input<bool?> Ref_IsDelete;
public Input<string?> Ref_Code;
public Input<string?> Ref_FirstName;
public Input<string?> Ref_LastName;
public Input<string?> Ref_CellphoneNo;
public Dropdown Ref_JobInterviewer1;
public Dropdown Ref_HR_InterviewResumeSiteId;
public Dropdown Ref_HR_InterviewTypeId;
public Dropdown Ref_HR_JobInterviewContractTypeId;
public Dropdown Ref_HR_JobIntervieweTitleId;
public Input<string?> Ref_InterviewDateTime;
public Input<string?> Ref_InterviewerDescription;
public Input<string?> Ref_MeetingLink;
public Input<bool?> Ref_IsReInterview;
public RadioBoolean Ref_IsApproved;
public Input<string?> Ref_ReInterviewDateTime;
public FileUploadPage.Uploader<Entity.HR_JobInterview_JobInterviewResumeFile> Ref_HR_JobInterview_JobInterviewResumeFile;
public Input<string?> Ref_CancellatioReason;
public Input<string?> Ref_PortfolioURL;
public Input<int?> Ref_SuggestedSalary;
public Input<bool?> Ref_ShuttleService;
public Input<string?> Ref_GoogleMeetUserName;
public Input<string?> Ref_GoogleMeetPassword;
public Input<string?> Ref_SupervisorComment;
public RadioBoolean Ref_SupervisorApprove;
public Input<string?> Ref_ManagerComment;
public RadioBoolean Ref_ManagerApprove;
public Input<bool?> Ref_JobSeekerNotAvailable;
public Input<string?> Ref_RequestDescription;
public Input<byte?> Ref_Age;
public RadioBoolean Ref_MaritalStatus;
public RadioBoolean Ref_Gender;
public Input<string?> Ref_Location;
public Input<string?> Ref_Referral;
public Input<string?> Ref_InterviewLocation;
public Dropdown Ref_BaseInfo_MilitaryStatusId;
public Input<string?> Ref_Telephone;
public RadioBoolean Ref_IsEmployed;
public Input<string?> Ref_LastCompany;
public Input<string?> Ref_Skills;
public Time Ref_InterviewDuration;
public Dropdown Ref_HR_EmploymentOffersId;
public Dropdown Ref_HR_InterviewItemsId;
public Dropdown Ref_InterviewLevelItemsId;
public Input<string?> Ref_AvailabilityToStartWork;


    #endregion

 

}
}
