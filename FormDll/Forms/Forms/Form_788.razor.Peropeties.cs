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
using RadioBooleanPage;
using DevExpress.Blazor;
using FormsPage;
using Sitko.Blazor.CKEditor;
using SingleFileUpload;
namespace Forms.Forms
{
    public class Form_788Peropeties : FormBase
    {
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
        /// کوئری بیلدر برای فیلتر سرچ
        /// تنظیم اولیه توسط ادمین
        /// </summary>
        [Parameter]
        public QueryBuilderFilterRule? QueryFilter { get; set; }

        /// <summary>
        /// کوئری بیلدر برای فیلتر سرچ
        /// تنظیم اولیه توسط ادمین
        /// </summary>

        public QueryBuilderFilterRule? SearchQueryFilter { get; set; }

        /// <summary>
        /// ایونت لود کامل فرم
        /// </summary>
        [Parameter]
        public EventCallback<Baya.Models.Utility.Result> OnFormloaded { get; set; }


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

        ///// <summary>
        /////  شماره درخواست
        ///// </summary>
        //[Parameter]
        //public Guid? RequestID { get; set; }

        /// <summary>
        /// غیر فعال کردن کل فرم
        /// </summary>
        [Parameter]
        public bool ReadOnly { get; set; } = false;

        /// <summary>
        ///  ورژنی که فرم باهاش ساخته شده
        /// </summary>
        public string? VersionForm { get; set; } = "5622";

        /// <summary>
        /// لیست داده های گرید
        /// </summary>
        public List<Entity.HR_RMS_FirstDegreeRelatives> _Entity { get; set; }
            /// <summary>
            /// نام فیلد اصلی این موجودیت
            /// </summary>
            public string PrimaryFieldName { get; set; }= (new Entity.HR_RMS_FirstDegreeRelatives())._GetIdName;
        /// <summary>
        /// مشخصات کاربر
        /// UserID
        /// </summary>
        public dynamic _User { get; set; }

        public EditForm? _Form { get; set; }

        public string? SumaryMessage { get; set; }

        /// <summary>
        /// پیج بندی گرید
        /// </summary>
        public DxPager DxPager { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; } = 10;
        public int PageCount { get; set; } = 100;



        public bool ShowForm { get; set; } = false;

        /// <summary>
        /// با موبایل باز کرده سایت رو
        /// </summary>
        public bool IsMobile { get; set; }
        /// <summary>
        /// ریست صفحه گرید از صفر
        /// </summary>

        public bool PageReset { get; set; }

        public Table TableGet { get; set; } 

        public Form_788_CU RefForm { get; set; }
    public Form_788_Filter RefListFilter  { get; set; }

public DxGrid? Grid_HR_RMS_FirstDegreeRelatives;


}
}
