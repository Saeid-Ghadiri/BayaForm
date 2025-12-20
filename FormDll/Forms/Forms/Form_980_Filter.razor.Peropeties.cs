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
using RadioBooleanPage;
using InputPage2;
using FormsPage;
using DevExpress.Blazor;
using Sitko.Blazor.CKEditor;
namespace Forms.Forms
{
    public class Form_980_FilterPeropeties : FormBase
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
        /// کوئری بیلدر برای فیلتر سرچ
        /// تنظیم اولیه توسط ادمین
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
        /// غیر فعال کردن کل فرم
        /// </summary>
        [Parameter]
        public bool ReadOnly { get; set; } = false;

        /// <summary>
        ///  ورژنی که فرم باهاش ساخته شده
        /// </summary>
        public string? VersionForm { get; set; } = "5280";


        /// <summary>
        /// مشخصات کاربر
        /// UserID
        /// </summary>
        public dynamic _User { get; set; }

        /// <summary>
        /// ایونت عملیات سرچ
        /// </summary>
        [Parameter]
        public EventCallback<QueryBuilderFilterRule> OnSearch { get; set; }
        /// <summary>
        /// نمایش فرم
        /// </summary>

        public bool ShowForm { get; set; } = false;
        /// <summary>
        /// نمایش فیلتر
        /// </summary>
        public bool ShowFilterBar { get; set; } = false;
        /// <summary>
        /// با موبایل باز کرده سایت رو
        /// </summary>
        public bool IsMobile { get; set; }






    }
}
