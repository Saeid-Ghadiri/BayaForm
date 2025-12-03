using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using Newtonsoft.Json.Linq;
using Utility;
using Baya.Models.ORM;
using Baya.Models.Utility.Entity;
using Baya.Models.Utility.Menu;
using Blazored.Toast.Services;

namespace Forms.Forms
{
    public class Form_969_CUBase : Form_969_CUPeropeties
    {

        // تابع پیام تُــست
        public MSG _MSG { get; set; }


        /// <summary>
        /// آماده سازی فرم
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {

        }

        /// <summary>
        /// رندر شدن فرم
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // تعریف مدل پیام بر اساس تابع تعریف شده
                _MSG = new MSG(toastService);
                 await InvokeAsync(StateHasChanged);
                await Task.Yield();
                // Save & New Button: 
             //  await JS.InvokeVoidAsync("AddClass", "#btn_listi_Save_New", "d-none");
                // Before Button: 
                await JS.InvokeVoidAsync("AddClass", "#btn_listi_Previous", "d-none");
                // Next Button: 
                await JS.InvokeVoidAsync("AddClass", "#btn_listi_Next", "d-none");

                StateHasChanged();

                await CheckGridDataAndToggleButton();

                
            }
        }

        /// <summary>
        /// اعتبار سنجی فرم
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> FormValidator()
        {
            bool IsValid = true;

            // بررسی تابع اعتبارسنجی فیلد ها
            IsValid = await CheckFieldValidation(_Entity);

            IsValid = IsValid && await DataBankAccount(_Entity.HR_Base_BankAccount);

            return IsValid;
        }


        /// <summary>
        /// تابع قبل اجرا شدن ارسال داده
        /// </summary>
        /// <returns></returns>
        public override async Task<Result> BeforSubmit()
        {
            return new Result() { Status = HttpStatusCode.OK };
        }

        /// <summary>
        /// تابع بعد اجرا شدن ارسال داده
        /// </summary>
        /// <returns></returns>
        public override async Task AfterSubmit()
        {

        }

        /// <summary>
        /// تابع قبل دریافت داده
        /// </summary>
        /// <returns></returns>
        public override async Task BeforGetData()
        {

        }

        /// <summary>
        /// تابع بعد دریافت داده
        /// </summary>
        /// <returns></returns>
        public override async Task AfterGetData()
        {
            if (_Entity.HR_Base_BankAccount != null)
            {
                _Entity.HR_Base_BankAccount = _Entity.HR_Base_BankAccount.Where(x => x.IsDelete == false).ToList();
            }
        }


        #region FunctionEvents

        public async Task<bool> CheckFieldValidation(Entity.HR_EMP_Employees Item)
        {
            bool IsValid = true;
            // **************************************************
            // // فیلد شرکت
            // if (Item.BaseInfo_ORG_CompaniesId == null || Item.BaseInfo_ORG_CompaniesId == Guid.Empty)
            // {
            //     IsValid = false;
            //     await _MSG.ShowError("لطفا گزینه شرکت را تکمیل نمایید.");
            // }
            // فیلد نام
            //if (MasterItem.FirstName == null)
            if (string.IsNullOrWhiteSpace(Item.FirstName))
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه نام را تکمیل نمایید.");
            }
            // فیلد نام خانوادگی
            //if (MasterItem.FirstName == null)
            if (string.IsNullOrWhiteSpace(Item.LastName))
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه نام خانوادگی را تکمیل نمایید.");
            }
            // // فیلد وضعیت کارمند
            // if (Item.HR_EMP_StatusId == null)
            // {
            //     IsValid = false;
            //     await _MSG.ShowError("لطفا گزینه وضعیت کارمند را تکمیل نمایید.");
            // }
            // // فیلد کد قدیم پرسنلی کارمند
            // if (Item.EmployeeLastPersonelNo == null)
            // {
            //     IsValid = false;
            //     await _MSG.ShowError("لطفا گزینه کد قدیم پرسنلی کارمند را تکمیل نمایید.");
            // }
            // // فیلد کد کارمند
            // if (Item.EmployeeNo == null)
            // {
            //     IsValid = false;
            //     await _MSG.ShowError("لطفا گزینه کد کارمند را تکمیل نمایید.");
            // }
            // // فیلد کد پرسنلی کارمند
            // if (Item.EmployeePersonelNo == null)
            // {
            //     IsValid = false;
            //     await _MSG.ShowError("لطفا گزینه کد پرسنلی کارمند را تکمیل نمایید.");
            // }
            // **************************************************
            // **************************************************
            // Details 
            foreach (var item in _Entity.HR_EMP_EmployeeInfos)
            {
                // // فیلد نام پدر
                // if (item.FatherName == null)
                // {
                //     IsValid = false;
                //     await _MSG.ShowError("لطفا گزینه نام پدر را تکمیل نمایید.");
                // }
                // // فیلد کد ملی
                // if (item.NationalCode == null)
                // {
                //     IsValid = false;
                //     await _MSG.ShowError("لطفا گزینه کد ملی را تکمیل نمایید.");
                // }
            }
            // **************************************************
            return IsValid;
        }


        /// <summary>
        ///  تابعی برای بررسی تعداد ردیف‌های جزئیات
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public async Task<bool> DataBankAccount(ICollection<Entity.HR_Base_BankAccount> Item)
        {
            // دکمه جدید در گرید حساب بانکی - HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonNew
            //await JS.InvokeVoidAsync("ModalAddClass", "#HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonNew", "d-none");


            if (Item != null && Item.Count > 1)
            {
                await _MSG.ShowError("شما مجاز به ثبت یک ردیف بیشتر نیستید!!");
                return false;
            }
            return true;
        }


        /// <summary>
        /// نمایش و حذف دکمه جدید گرید حساب بانکی کاربر
        /// </summary>
        /// <returns></returns>
        private async Task CheckGridDataAndToggleButton()
        {
            StateHasChanged();
            
            Console.WriteLine("Log HR_Base_BankAccount:::" + _Entity.HR_Base_BankAccount.Count);

            //if (_Entity.HR_Base_BankAccount != null && _Entity.HR_Base_BankAccount.Any(x => !x.IsDelete))
            if (_Entity.HR_Base_BankAccount.Count >= 1)
            {
                // اگر داده وجود دارد، دکمه جدید را مخفی کن
                await JS.InvokeVoidAsync("AddClass", "#HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonNew", "d-none");
            }
            else
            {
                // اگر داده وجود ندارد، دکمه جدید را نمایش بده
                await JS.InvokeVoidAsync("RemoveClass", "#HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonNew", "d-none");
            }

            StateHasChanged();
        }

        // ایونت Edit Model Saving: ویرایش و بررسی اطلاعات حساب بانکی در جدول کارمند در حال مودال
        public async Task<bool> GridHR_EMP_EmployeesId_357_editmodelsaving(object e)
        {
            bool IsCancelled = false;

            //var CheckDataBankAccount = _Entity.HR_Base_BankAccount;
            //Console.WriteLine("#Log:: DataBankAccount ::>>" + _Entity.HR_Base_BankAccount.Count());

            IsCancelled = !await DataBankAccount(_Entity.HR_Base_BankAccount);

            return IsCancelled;
        }

        // ایونت After Render Modal: برای رندر کردن در حالت مودال گرید اطلاعات حساب بانکی در جدول کارمند است
        public async Task GridHR_EMP_EmployeesId_357_afterrendermodal(Entity.HR_Base_BankAccount Item)
        {
            // حذف دکمه های ذخیره، قبلی، بعدی در گرید اطلاعات حساب بانکی
            await JS.InvokeVoidAsync("ModalAddClass", "#HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonSaveAndNew", "d-none");
            await JS.InvokeVoidAsync("ModalAddClass", "#HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonBefore", "d-none");
            await JS.InvokeVoidAsync("ModalAddClass", "#HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonNext", "d-none");
        }

        public async Task <bool> GridHR_EMP_EmployeesId_383_editmodelsaving(object e   )
        {
            // بررسی داده
            await CheckGridDataAndToggleButton();

            return false;
        }
        
        public async Task  GridHR_EMP_EmployeesId_383_afterrendermodal(Entity.HR_EMP_EmployeeInfos Item   )
        {
            // حذف دکمه های ذخیره، قبلی، بعدی در گرید جزئیات اطلاعات کارمند
            await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonSave", "d-none");
            await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonBefore", "d-none");
            await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonNext", "d-none");
        }

		public async Task <bool> GridHR_EMP_EmployeesId_381_editmodelsaving(object e   )
        {
            return false;
        }
        
        public async Task  GridHR_EMP_EmployeesId_381_afterrendermodal(Entity.HR_EMP_EmployeeFamileis Item   )
        {
        }

		#endregion FunctionEvents

    }
}