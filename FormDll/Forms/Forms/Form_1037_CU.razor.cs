using Baya.Models.ORM;
using Baya.Models.Utility;
using Baya.Models.Utility.Entity;
using Baya.Models.Utility.Menu;
using Baya.Models.Utility.Pagination.Pagings;
using BlazorBootstrap;
using Blazored.Toast.Services;
using Castle.DynamicLinqQueryBuilder;
using DevExpress.Blazor;
using Entity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using Sitko.Blazor.CKEditor;
using System;
using System.Net;
using System.Web;
using Utility;
using System.Net;
using System.Text.RegularExpressions;

namespace Forms.Forms
{
	public class Form_1037_CUBase : Form_1037_CUPeropeties
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
				// تعریف مدل پیام بر اساس تابع تعریف شدهa
				_MSG = new MSG(toastService);

				await InvokeAsync(StateHasChanged);
				await Task.Yield();

				// در شرایطی باید از Save_New هم باشد که در لیست اصلی در تنظیمان form.io تیک گزینه جدید برداشته نشده باشد
				//await JS.InvokeVoidAsync("ModalAddClass", "#btn_listi_Save_New", "d-none");
				await JS.InvokeVoidAsync("AddClass", "#btn_listi_Previous", "d-none");
				await JS.InvokeVoidAsync("AddClass", "#btn_listi_Next", "d-none");

				//// بررسی و تغییرات حساب بانکی کارمند
				//await CheckGridDataAndToggleButton();

				// بررسی دکمه های اطلاعات تماس کارمند
				//await EmployeeDetails_ToggleGridButton();

				// بررسی دکمه های گرید جزئیات اطلاعات کارمند
				//await ToggleDetails_Grid_EmployeeInfos_Button(true);

				StateHasChanged();
			}
		}

		/// <summary>
		/// اعتبار سنجی فرم
		/// </summary>
		/// <returns></returns>
		public override async Task<bool> FormValidator()
		{
			bool IsValid = true;

			// // بررسی اطلاعات تماس
			// if (!await HasValidCountEmployeeDetails())
			// {
			// 	await EmployeeDetailsShowRecordDialog();
			// 	return false;
			// }

			// #region CheckFieldsValidation

			// // اعتبارسنجی کارمند اصلی
			// IsValid = await CheckFieldsValidation_Employees(_Entity);

			// // جزئیات اطلاعات کارمند
			// foreach (var item in _Entity.HR_EMP_EmployeeInfos.Where(x => x.IsDelete != true))
			// {
			// 	if (!await CheckFieldsValidation_EmployeeInfos(item))
			// 		IsValid = false;
			// }

			// // خانواده کارمند
			// foreach (var item in _Entity.HR_EMP_EmployeeFamileis.Where(x => x.IsDelete != true))
			// {
			// 	if (!await CheckFieldsValidation_EmployeeFamilies(item))
			// 		IsValid = false;
			// }

			// // حساب بانکی
			// foreach (var item in _Entity.HR_Base_BankAccount.Where(x => x.IsDelete != true))
			// {
			// 	if (!await CheckFieldsValidation_EmpBankAccounts(item))
			// 		IsValid = false;
			// }

			// // مدارک
			// foreach (var item in _Entity.HR_EMP_Documents.Where(x => x.IsDelete != true))
			// {
			// 	if (!await CheckFieldsValidation_EmpDocuments(item))
			// 		IsValid = false;
			// }

			// // اطلاعات تماس
			// foreach (var item in _Entity.HR_EMP_EmployeeDetails.Where(x => x.IsDelete != true))
			// {
			// 	if (!await CheckFieldsValidation_EmpContacts(item))
			// 		IsValid = false;
			// }

			// #endregion

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
			//if (_Entity.HR_Base_BankAccount != null)
			//{
			//	_Entity.HR_Base_BankAccount = _Entity.HR_Base_BankAccount.Where(x => x.IsDelete == false).ToList();
			//}

			// بررسی دکمه های اطلاعات تماس کارمند
			//await EmployeeDetails_ToggleGridButton();

			// بررسی دکمه های گرید جزئیات اطلاعات کارمند
			//await ToggleDetails_Grid_EmployeeInfos_Button(true);
		}


		#region FunctionEvents

		#region CheckFieldValidation

		public async Task<bool> CheckFieldsValidation_Employees(Entity.HR_EMP_Employees Item)
		{
			bool IsValid = true;
			// **************************************************
			//// اطلاعات اصلی کارمند
			//// فیلد شرکت
			//if (Item.BaseInfo_ORG_CompaniesId == null || Item.BaseInfo_ORG_CompaniesId == Guid.Empty)
			//{
			//	IsValid = false;
			//	await _MSG.ShowError("لطفا گزینه شرکت را تکمیل نمایید.");
			//}

			//// فیلد نام
			//if (string.IsNullOrWhiteSpace(Item.FirstName))
			//{
			//	IsValid = false;
			//	await _MSG.ShowError("لطفا گزینه نام را تکمیل نمایید.");
			//}

			//// فیلد نام خانوادگی
			//if (string.IsNullOrWhiteSpace(Item.LastName))
			//{
			//	IsValid = false;
			//	await _MSG.ShowError("لطفا گزینه نام خانوادگی را تکمیل نمایید.");
			//}

			//// فیلد کد ملی
			//if (Item.NationalCode == null)
			//{
			//	IsValid = false;
			//	await _MSG.ShowError("لطفا گزینه کد ملی را تکمیل نمایید.");
			//}

			//// فیلد کد کارمند
			//if (Item.EmployeeNo == null)
			//{
			//	IsValid = false;
			//	await _MSG.ShowError("لطفا گزینه کد کارمند را تکمیل نمایید.");
			//}

			//// فیلد کد قدیم پرسنلی کارمند
			//if (Item.EmployeeLastPersonelNo == null)
			//{
			//	IsValid = false;
			//	await _MSG.ShowError("لطفا گزینه کد قدیم پرسنلی کارمند را تکمیل نمایید.");
			//}

			//// فیلد کد پرسنلی کارمند
			//if (Item.EmployeePersonelNo == null)
			//{
			//	IsValid = false;
			//	await _MSG.ShowError("لطفا گزینه کد پرسنلی کارمند را تکمیل نمایید.");
			//}

			//// فیلد وضعیت کارمند
			//if (Item.HR_EMP_StatusId == null)
			//{
			//	IsValid = false;
			//	await _MSG.ShowError("لطفا گزینه وضعیت کارمند را تکمیل نمایید.");
			//}

			// **************************************************

			return IsValid;
		}

		public async Task<bool> CheckFieldsValidation_EmployeeInfos(Entity.HR_EMP_EmployeeInfos item)
		{
			bool IsValid = true;

			// فیلد نام پدر
			if (string.IsNullOrWhiteSpace(item?.FatherName))
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه نام پدر را تکمیل نمایید.");
			}

			// شماره شناسنامه
			if (string.IsNullOrWhiteSpace(item?.IdCardNo))
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه شماره شناسنامه را تکمیل نمایید.");
			}

			// سریال شماره شناسنامه (3 بخش)
			if (string.IsNullOrWhiteSpace(item?.IdCardSerialNoSection1))
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه سریال شماره شناسنامه (بخش اول) را تکمیل نمایید.");
			}
			if (string.IsNullOrWhiteSpace(item?.IdCardSerialNoSection2))
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه سریال شماره شناسنامه (بخش دوم) را تکمیل نمایید.");
			}
			if (string.IsNullOrWhiteSpace(item?.IdCardSerialNoSection3))
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه سریال شماره شناسنامه (بخش سوم) را تکمیل نمایید.");
			}

			// وضعیت تاهل
			if (item?.BaseInfo_MaritalStatusId == null || item.BaseInfo_MaritalStatusId == Guid.Empty)
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه وضعیت تاهل را تکمیل نمایید.");
			}

			// شهر محل صدور
			if (item?.CityOfIssue == null || item.CityOfIssue == Guid.Empty)
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه شهر محل صدور را تکمیل نمایید.");
			}

			// شهر محل تولد
			if (item?.CityOfBirth == null || item.CityOfBirth == Guid.Empty)
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه شهر محل تولد را تکمیل نمایید.");
			}

			// تاریخ تولد
			if (string.IsNullOrWhiteSpace(item?.BirthDate_Fa))
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه تاریخ تولد را تکمیل نمایید.");
			}

			// // فایل سابقه کار بیمه
			// if (string.IsNullOrWhiteSpace(item?.EmployeeWorkExperienceFile))
			// {
			// 	IsValid = false;
			// 	await _MSG.ShowError("لطفاً گزینه فایل سابقه کار بیمه را تکمیل نمایید.");
			// }

			// // سابقه کار کارمند به روز
			// if (item?.EmployeeWorkExperience == null || item.EmployeeWorkExperience <= 0)
			// {
			// 	IsValid = false;
			// 	await _MSG.ShowError("لطفاً گزینه سابقه کار کارمند به روز را تکمیل نمایید.");
			// }

			// جنسیت
			if (item?.BaseInfo_GenderId == null || item.BaseInfo_GenderId == Guid.Empty)
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه جنسیت را تکمیل نمایید.");
			}

			// وضعیت گروه خونی
			if (item?.BaseInfo_BloodGroupId == null || item.BaseInfo_BloodGroupId == Guid.Empty)
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه وضعیت گروه خونی را تکمیل نمایید.");
			}

			// دین
			if (item?.BaseInfo_ReligionId == null || item.BaseInfo_ReligionId == Guid.Empty)
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه دین را تکمیل نمایید.");
			}

			// مذهب
			if (item?.BaseInfo_DenominationsId == null || item.BaseInfo_DenominationsId == Guid.Empty)
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه مذهب را تکمیل نمایید.");
			}

			// وضعیت نظام وظیفه
			if (item?.BaseInfo_MilitaryStatusId == null || item.BaseInfo_MilitaryStatusId == Guid.Empty)
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه وضعیت نظام وظیفه را تکمیل نمایید.");
			}

			// تلفن همراه
			if (string.IsNullOrWhiteSpace(item?.Mobile))
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه تلفن همراه را تکمیل نمایید.");
			}

			// نشانی
			if (string.IsNullOrWhiteSpace(item?.Address))
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه نشانی را تکمیل نمایید.");
			}

			return IsValid;
		}

		public async Task<bool> CheckFieldsValidation_EmployeeFamilies(Entity.HR_EMP_EmployeeFamileis item)
		{
			if (item == null) return false;

			bool IsValid = true;

			// نسبت
			if (item.HR_FamilyRelationshipId == null || item.HR_FamilyRelationshipId == Guid.Empty)
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه نسبت را تکمیل نمایید.");
			}

			// تحت تکفل
			if (item.HR_Base_DependentId == null || item.HR_Base_DependentId == Guid.Empty)
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه تحت تکفل را تکمیل نمایید.");
			}

			// نام
			if (string.IsNullOrWhiteSpace(item.FirstName))
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه نام را تکمیل نمایید.");
			}

			// نام خانوادگی
			if (string.IsNullOrWhiteSpace(item.LastName))
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه نام خانوادگی را تکمیل نمایید.");
			}

			// کد ملی
			if (string.IsNullOrWhiteSpace(item.NationalCode))
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه کد ملی را تکمیل نمایید.");
			}

			// نام پدر
			if (string.IsNullOrWhiteSpace(item.FatherName))
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه نام پدر را تکمیل نمایید.");
			}

			// شماره شناسنامه
			if (string.IsNullOrWhiteSpace(item.IdCardNo))
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه شماره شناسنامه را تکمیل نمایید.");
			}

			// تاریخ تولد
			if (string.IsNullOrWhiteSpace(item.BirthDate_Fa))
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه تاریخ تولد را تکمیل نمایید.");
			}

			// جنسیت
			if (item.BaseInfo_GenderId == null || item.BaseInfo_GenderId == Guid.Empty)
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه جنسیت را تکمیل نمایید.");
			}

			// وضعیت تاهل
			if (item.BaseInfo_MaritalStatusId == null || item.BaseInfo_MaritalStatusId == Guid.Empty)
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه وضعیت تاهل را تکمیل نمایید.");
			}

			return IsValid;
		}

		public async Task<bool> CheckFieldsValidation_EmpBankAccounts(Entity.HR_Base_BankAccount item)
		{
			if (item == null) return false;

			bool IsValid = true;

			// دسته‌بندی حساب بانکی
			if (item.HR_Base_BankAcountCategoryId == null || item.HR_Base_BankAcountCategoryId == Guid.Empty)
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه دسته‌بندی حساب های بانکی کارمند را تکمیل نمایید.");
			}

			// نوع حساب بانکی
			if (item.BaseInfo_BankAccountTypeId == null || item.BaseInfo_BankAccountTypeId == Guid.Empty)
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه نوع حساب بانکی را تکمیل نمایید.");
			}

			// بانک
			if (item.BaseInfo_BankId == null || item.BaseInfo_BankId == Guid.Empty)
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه بانک را تکمیل نمایید.");
			}

			// شعبه بانک
			if (item.BaseInfo_BankBranchesId == null || item.BaseInfo_BankBranchesId == Guid.Empty)
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه شعبه بانک را تکمیل نمایید.");
			}

			// شماره حساب بانکی
			if (string.IsNullOrWhiteSpace(item.BankAccountNumber))
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه شماره حساب بانکی را تکمیل نمایید.");
			}

			// شبا بانکی
			if (string.IsNullOrWhiteSpace(item.IBAN))
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه شبا بانکی را تکمیل نمایید.");
			}

			// شماره کارت بانکی
			if (string.IsNullOrWhiteSpace(item.CartNo))
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً گزینه شماره کارت بانکی را تکمیل نمایید.");
			}

			return IsValid;
		}

		public async Task<bool> CheckFieldsValidation_EmpDocuments(Entity.HR_EMP_Documents item)
		{
			if (item == null) return false;

			bool IsValid = true;

			// // عنوان مدرک
			// if (string.IsNullOrWhiteSpace(item.Title))
			// {
			// 	IsValid = false;
			// 	await _MSG.ShowError("لطفاً گزینه عنوان مدرک را تکمیل نمایید.");
			// }

			// // نوع مدرک
			// if (item.HR_Base_DocumentTypesId == null || item.HR_Base_DocumentTypesId == Guid.Empty)
			// {
			// 	IsValid = false;
			// 	await _MSG.ShowError("لطفاً گزینه نوع مدرک را تکمیل نمایید.");
			// }

			// فعال بودن
			// (اختیاری - بسته به نیاز)

			return IsValid;
		}

		public async Task<bool> CheckFieldsValidation_EmpContacts(Entity.HR_EMP_EmployeeDetails item)
		{
			if (item == null) return false;

			bool IsValid = true;

			//// حداقل یکی از فیلدها باید پر باشد
			//bool hasAnyValue = !string.IsNullOrWhiteSpace(item.OrgTel1) ||
			//				   !string.IsNullOrWhiteSpace(item.OrgTel2) ||
			//				   !string.IsNullOrWhiteSpace(item.OrgMobile1) ||
			//				   !string.IsNullOrWhiteSpace(item.InsideEmail) ||
			//				   !string.IsNullOrWhiteSpace(item.OutsideEmail);

			//if (!hasAnyValue)
			//{
			//	IsValid = false;
			//	await _MSG.ShowError("لطفاً حداقل یکی از فیلدهای تماس سازمانی را تکمیل نمایید.");
			//}

			return IsValid;
		}

		#endregion

		#region Wait Loaded

		#region Wait Element Exists
		/// <summary>
		/// منتظر می‌ماند تا المان در DOM وجود داشته باشد
		/// </summary>
		/// <param name="selector">Selector المان</param>
		/// <param name="maxWaitTimeMs">حداکثر زمان انتظار به میلی‌ثانیه</param>
		/// <returns>true اگر المان پیدا شد</returns>
		private async Task<bool> WaitElementExists(string selector, int maxWaitTimeMs = 3000)
		{
			int waitedTime = 0;
			int delayInterval = 50;

			while (waitedTime < maxWaitTimeMs)
			{
				var exists = await JS.InvokeAsync<bool>("eval", $"document.querySelector('{selector}') !== null");
				if (exists)
				{
					return true;
				}

				await Task.Delay(delayInterval);
				waitedTime += delayInterval;
			}

			Console.WriteLine($"⚠️ Warning: Element '{selector}' was not found after waiting {maxWaitTimeMs}ms");
			return false;
		}
		#endregion

		#region Wait Component Loaded
		/// <summary>
		/// منتظر می‌ماند تا کامپوننت لود شود و سپس SetDisabled را فراخوانی می‌کند
		/// </summary>
		/// <param name="componentRef">مرجع کامپوننت</param>
		/// <param name="maxWaitTimeMs">حداکثر زمان انتظار به میلی‌ثانیه (پیش‌فرض: 5000)</param>
		/// <returns></returns>
		// Waits until the component reference getter returns a non-null value.
		// Using a Func allows re-evaluating the field as Blazor sets the ref after render.
		private async Task<bool> WaitComponentLoaded(Func<dynamic> componentRefGetter, int maxWaitTimeMs = 5000)
		{
			try
			{
				var current = componentRefGetter?.Invoke();
				if (current != null)
				{
					return true;
				}

				int waitedTime = 0;
				int delayInterval = 50;

				while (waitedTime < maxWaitTimeMs)
				{
					await Task.Delay(delayInterval);
					waitedTime += delayInterval;
					StateHasChanged();

					current = componentRefGetter?.Invoke();
					if (current != null)
					{
						return true;
					}
				}

				Console.WriteLine($"⚠️ Warning: Component was not loaded after waiting {maxWaitTimeMs}ms");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in WaitComponentLoaded: {ex.Message}");
				return false;
			}
		}
		#endregion

		#endregion

		#region Grid EmployeeInfos
		public async Task<bool> GridHR_EMP_EmployeesId_382_editmodelsaving(object e)
		{
			// Validate EmployeeInfos when saving a row in its modal grid
			bool IsCancelled = false;

			var item = e as Entity.HR_EMP_EmployeeInfos;
			IsCancelled = !await CheckFieldsValidation_EmployeeInfos(item);

			return IsCancelled;
		}
		public async Task GridHR_EMP_EmployeesId_382_afterrendermodal(Entity.HR_EMP_EmployeeInfos Item)
		{
			//await CheckedRowGrid_EmployeeInfos(Item);

			//await ToggleDetails_Grid_EmployeeInfos_Button(true);

			// انتظار کوتاه برای رندر کامل مودال
			await Task.Delay(300);

			if (await WaitComponentLoaded(() => Ref_HR_EMP_EmployeeInfos_HR_EMP_EmployeesId))
			{
				Ref_HR_EMP_EmployeeInfos_HR_EMP_EmployeesId.SetDisabled(true);

				// انتظار برای وجود دکمه‌ها در DOM
				bool beforeButtonExists = await WaitElementExists("#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonBefore", 2000);
				bool nextButtonExists = await WaitElementExists("#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonNext", 2000);

				if (beforeButtonExists)
				{
					// دکمه قبلی - مخفی می‌شود چون فقط یک ردیف وجود دارد
					await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonBefore", "d-none");
				}

				if (nextButtonExists)
				{
					// دکمه بعدی - مخفی می‌شود چون فقط یک ردیف وجود دارد
					await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonNext", "d-none");
				}
			}
		}

		#region EmployeeInfos_btn

		private async Task ToggleDetails_Grid_EmployeeInfos_Button(bool hasSavedRecord = false, bool isInModal = false)
		{
			await Task.Yield();

			// اگر hasSavedRecord مشخص نشده باشد، خودکار بررسی می‌کنیم
			// بررسی می‌کنیم که آیا یک ردیف غیر حذف‌شده در لیست وجود دارد
			if (!hasSavedRecord)
			{
				hasSavedRecord = _Entity.HR_EMP_EmployeeInfos?.Any(x => x.IsDelete != true) == true;
			}

			// فقط اگر در مودال باشیم، دکمه‌های مودال را مخفی کن
			if (isInModal)
			{
				// دکمه ذخیره و جدید - مخفی می‌شود چون فقط یک ردیف مجاز است
				//await JS.InvokeVoidAsync("ModalAddClass", "#", "d-none");
				// دکمه قبلی - مخفی می‌شود چون فقط یک ردیف وجود دارد
				await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonBefore", "d-none");
				// دکمه بعدی - مخفی می‌شود چون فقط یک ردیف وجود دارد
				await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonNext", "d-none");
			}
		}

		private async Task CheckedRowGrid_EmployeeInfos(Entity.HR_EMP_EmployeeInfos Item)
		{
			// بررسی می‌کنیم که آیا یک ردیف غیر حذف‌شده در لیست وجود دارد
			// این بررسی دقیق‌تر است چون ممکن است item جدید باشد اما هنوز ذخیره نشده باشد
			var hasSavedRecord = _Entity.HR_EMP_EmployeeInfos?.Any(x => x.IsDelete != true) == true;

			// اگر Item مشخص شده باشد و Id داشته باشد، مطمئن می‌شویم که ذخیره شده است
			if (Item != null && Item.Id != Guid.Empty && Item.IsDelete != true)
			{
				// بررسی می‌کنیم که آیا این ردیف واقعاً در لیست وجود دارد
				var existsInList = _Entity.HR_EMP_EmployeeInfos?.Any(x => x.Id == Item.Id && x.IsDelete != true) == true;
				if (existsInList)
				{
					hasSavedRecord = true;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private async Task<bool> HasValidDetailsCount()
		{
			// بررسی null بودن لیست
			if (_Entity.HR_EMP_EmployeeInfos == null)
			{
				await _MSG.ShowError("لطفاً حداقل یک ردیف در بخش «جزئیات اطلاعات کارمند» ثبت کنید.");
				return false;
			}

			// شمارش ردیف‌های فعال (غیر حذف‌شده)
			var activeCount = _Entity.HR_EMP_EmployeeInfos.Count(x => x.IsDelete != true);

			if (activeCount == 0)
			{
				await _MSG.ShowError("لطفاً حداقل یک ردیف در بخش «جزئیات اطلاعات کارمند» ثبت کنید.");
				return false;
			}

			if (activeCount > 1)
			{
				await _MSG.ShowError("شما مجاز به ثبت بیش از یک ردیف نیستید. لطفاً فقط یک ردیف در بخش «جزئیات اطلاعات کارمند» ثبت کنید.");
				return false;
			}

			return true;
		}

		#endregion

		#endregion Grid EmployeeInfos

		#region Grid EmployeeFamilies
		public async Task<bool> GridHR_EMP_EmployeesId_381_editmodelsaving(object e)
		{
			bool IsCancelled = false;

			var item = e as Entity.HR_EMP_EmployeeFamileis;
			IsCancelled = !await CheckFieldsValidation_EmployeeFamilies(item);


			//if (!IsCancelled)
			//{
			//	Console.WriteLine("### Grid EmployeeFamilies ### :: Start Saving");
			//	await Submit();
			//	Console.WriteLine("### Grid EmployeeFamilies ### :: End Saving");
			//}

			return IsCancelled;


		}
		public async Task GridHR_EMP_EmployeesId_381_afterrendermodal(Entity.HR_EMP_EmployeeFamileis Item)
		{
		}
		#endregion

		#region Grid EMP_BankAccounts

		// ایونت Edit Model Saving: ویرایش و بررسی اطلاعات حساب بانکی در جدول کارمند در حال مودال
		public async Task<bool> GridHR_EMP_EmployeesId_357_editmodelsaving(object e)
		{
			bool IsCancelled = false;


			var item = e as Entity.HR_Base_BankAccount;
			IsCancelled = !await CheckFieldsValidation_EmpBankAccounts(item);

			if (!ValidateIR(item.IBAN))
			{
				await _MSG.ShowInfo(" شماره شبا با IR ثبت شود");
				await _MSG.ShowError(" شماره شبا ثبت شده اشتباه است");
				IsCancelled = true;
			}

			if (!IsValidCardNumber(item.CartNo))
			{
				await _MSG.ShowInfo(" شماره کارت با 16 رقم ثبت شود");
				await _MSG.ShowError(" شماره کارت ثبت شده اشتباه است");
				IsCancelled = true;
			}

			return IsCancelled;
		}

		// ایونت After Render Modal: برای رندر کردن در حالت مودال گرید اطلاعات حساب بانکی در جدول کارمند است
		public async Task GridHR_EMP_EmployeesId_357_afterrendermodal(Entity.HR_Base_BankAccount Item)
		{
			Console.WriteLine("### GridHR_EMP_EmployeesId_357_afterrendermodal TRIGGERED");

			// تأخیر کوتاه برای اطمینان از رندر کامل کامپوننت‌ها
			await Task.Delay(50);

			if (Item == null)
			{
				Console.WriteLine("### Item is NULL - hiding all dropdowns");
				await ManageAccountHolderTypeFields(null);
				return;
			}

			// دریافت مقدار فعلی نوع دارنده حساب از ردیف
			string currentTypeId = Item.HR_Base_AccountHolderTypeId?.ToString();
			Console.WriteLine($"→ Current AccountHolderType ID: {currentTypeId}");

			// مدیریت نمایش دراپ‌دان‌ها بر اساس مقدار فعلی
			await ManageAccountHolderTypeFields(currentTypeId, Item.HR_EMP_EmployeesId);

			Console.WriteLine("### GridHR_EMP_EmployeesId_357_afterrendermodal FINISHED");

		}

		#region HR_EMP_EmployeesId Selected
		/// <summary>
		/// وقتی کارمند انتخاب می‌شود، فقط همسر همان کارمند را در دراپ‌دان خانواده لود می‌کند.
		/// </summary>
		public async Task HR_EMP_EmployeesId_onitemselected(dynamic Selected, Entity.HR_Base_BankAccount Item)
		{
			Ref_HR_Base_BankAccount_HR_EMP_EmployeeFamileisId?.LoadData();
		}
		#endregion

		#region BankAccount_Validation

		//FK 14040520
		private bool ValidateIR(string iBAN)
		{
			if (string.IsNullOrWhiteSpace(iBAN))
				return false;

			if (iBAN.Length != 26)
				return false;

			if (!iBAN.StartsWith("IR"))
				return false;

			string numberPart = iBAN.Substring(2);
			if (!Regex.IsMatch(numberPart, @"^\d{24}$"))
				return false;

			return true;
		}

		private bool ValidateIR2(string iBAN)
		{
			if (string.IsNullOrWhiteSpace(iBAN))
				return false;

			if (iBAN.Length != 24)
				return false;

			if (!Regex.IsMatch(iBAN, @"^\d{24}$"))
				return false;

			return true;
		}

		private bool IsValidCardNumber(string cartNo)
		{

			if (string.IsNullOrWhiteSpace(cartNo))
				return false;

			// باید دقیقاً 16 رقم باشد
			if (!System.Text.RegularExpressions.Regex.IsMatch(cartNo, @"^\d{16}$"))
				return false;

			return true;
		}
		#endregion

		#region HR_Base_AccountHolderType Selected
		public async Task HR_Base_AccountHolderTypeId_onitemselected(dynamic Selected, Entity.HR_Base_BankAccount Item)
		{
			Console.WriteLine("### HR_Base_AccountHolderTypeId_onitemselected TRIGGERED");

			if (Item == null)
			{
				Console.WriteLine("### Item is NULL");
				await ManageAccountHolderTypeFields(null);
				return;
			}

			string selectedTypeId = Selected?.Id?.ToString();
			Console.WriteLine($"✅ Selected AccountHolderType ID: {selectedTypeId}");

			// مدیریت نمایش دراپ‌دان‌ها
			await ManageAccountHolderTypeFields(selectedTypeId, Item.HR_EMP_EmployeesId);

			Console.WriteLine("### HR_Base_AccountHolderTypeId_onitemselected FINISHED");
		}
		#endregion

		#region AccountHolderType - Shared Methods

		/// <summary>
		/// مدیریت نمایش/مخفی کردن دراپ‌دان‌های کارمند و خانواده بر اساس نوع دارنده حساب
		/// </summary>
		/// <param name="accountHolderTypeId">شناسه نوع دارنده حساب</param>
		/// <param name="employeeId">شناسه کارمند (اختیاری - فقط برای لود همسران)</param>
		private async Task ManageAccountHolderTypeFields(string accountHolderTypeId, Guid? employeeId = null)
		{
			const string EmpHusband = "117F1A3A-3BFB-F011-A50E-005056A2B6BD";
			const string EmpWife = "4C395B47-3BFB-F011-A50E-005056A2B6BD";

			bool showEmployee = false;
			bool showFamily = false;

			if (string.IsNullOrWhiteSpace(accountHolderTypeId))
			{
				Console.WriteLine("→ نوع دارنده حساب خالی است، همه دراپ‌دان‌ها مخفی می‌شوند");
			}
			else if (accountHolderTypeId.ToUpper() == EmpHusband)
			{
				Console.WriteLine("→ نمایش فقط دراپ‌دان کارمند");
				showEmployee = true;
			}
			else if (accountHolderTypeId.ToUpper() == EmpWife)
			{
				Console.WriteLine("→ نمایش دراپ‌دان کارمند + خانواده (همسر)");
				showEmployee = true;
				showFamily = true;

				if (employeeId != null)
				{
					Console.WriteLine("→ کارمند انتخاب شده است، در حال لود همسران...");
					try
					{
						await HR_EMP_EmployeesId_onitemselected(
							new { Id = employeeId.ToString() },
							null
						);
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Error loading spouses: {ex.Message}");
					}
				}
			}
			else
			{
				Console.WriteLine($"⚠️ نوع ناشناخته: {accountHolderTypeId}");
			}

			// تنظیم نمایش دراپ‌دان‌ها با مدیریت خطا
			try
			{
				Ref_HR_Base_BankAccount_HR_EMP_EmployeesId?.SetVisible(showEmployee);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error setting employee dropdown visibility: {ex.Message}");
			}

			try
			{
				Ref_HR_Base_BankAccount_HR_EMP_EmployeeFamileisId?.SetVisible(showFamily);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error setting family dropdown visibility: {ex.Message}");
			}

			await InvokeAsync(StateHasChanged);
		}

		#endregion

		#region HR_Base_BankAcountCategory Selected
		public async Task HR_Base_BankAcountCategoryId_onitemselected(dynamic Selected, Entity.HR_Base_BankAccount Item)
		{
			Console.WriteLine("#Log :: BankAcountCategoryId_onitemselected :: ");

			// شناسه ثابت دسته‌بندی "حقوق و دستمزد"
			var BankAccCatId = "E8B28475-6FEC-F011-A50E-005056A2B6BD";

			// شناسه ثابت بانک "سپه"
			var SepahBankId = "6A2FEC9D-9E93-F011-A50E-005056A2B6BD";
			var SepahBankTitle = "سپه";

			// دریافت مقدار فعلی دسته‌بندی حساب
			var BankAcountCategoryId = Selected.Id?.ToString();

			foreach (var item in _Entity.HR_Base_BankAccount)
			{
				if (BankAcountCategoryId == BankAccCatId.ToLower())
				{
					Console.WriteLine("#Log :: BankAccCatId :: " + BankAccCatId.ToLower());
					Console.WriteLine("#Log :: BankAcountCategoryId :: " + BankAcountCategoryId);

					// ۱. ست کردن مقدار در مدل (دیتابیس)
					Guid sepahId = Guid.Parse(SepahBankId);
					string sepahTitle = SepahBankTitle;

					Console.WriteLine("#Log :: sepahId :: " + sepahId);

					// ۲. ست کردن آبجکت کامل بانک در مدل (برای جلوگیری از خطاهای احتمالی در گرید یا فرم)
					// اگر پراپرتی BaseInfo_Bank در موجودیت شما وجود دارد، بهتر است آن را هم مقداردهی کنید
					item.BaseInfo_Bank = new Entity.BaseInfo_Banks { Id = sepahId, Title = sepahTitle };

					// ۳. همگام‌سازی با رابط کاربری (UI)
					// این بخش بسیار مهم است. ما باید به کامپوننت بگوییم که مقدارش تغییر کرده است.
					// فرض بر این است که Ref_BaseInfo_BankId یک Reference به کامپوننت انتخاب بانک است.

					if (Ref_HR_Base_BankAccount_BaseInfo_BankId != null)
					{
						// اگر متد SetEntity در کامپوننت شما وجود دارد، از آن استفاده کنید
						// این متد معمولاً مقدار را در کامپوننت ست کرده و UI را رفرش می‌کند
						Ref_HR_Base_BankAccount_BaseInfo_BankId.SetEntity(item.BaseInfo_Bank);
						Console.WriteLine("#Log :: Ref_BaseInfo_BankId :: ");

						// یا اگر متد خاصی برای ست کردن مقدار (Value) دارید:
						//Ref_BaseInfo_BankId.Value = sepahGuid;


						Ref_HR_Base_BankAccount_BaseInfo_BankId.ItemSelected(item.BaseInfo_Bank);

						await Task.Delay(100);
						Ref_HR_Base_BankAccount_BaseInfo_BankId.LoadData();
					}

					// ۴. اطلاع به Blazor برای رندر مجدد صفحه
					StateHasChanged();

					Console.WriteLine("Log:: Bank set to Sepah successfully.");
				}
			}
		}
		#endregion

		#endregion

		#region Grid EMP_Documents
		public async Task<bool> GridHR_EMP_EmployeesId_379_editmodelsaving(object e)
		{
			bool IsCancelled = false;

			var item = e as Entity.HR_EMP_Documents;
			IsCancelled = !await CheckFieldsValidation_EmpDocuments(item);

			return IsCancelled;
		}
		public async Task GridHR_EMP_EmployeesId_379_afterrendermodal(Entity.HR_EMP_Documents Item)
		{


		}
		#endregion

		#region Grid HR_EMP_AcademicDocuments
		public async Task<bool> GridHR_EMP_EmployeesId_378_editmodelsaving(object e)
		{
			bool IsCancelled = false;



			return IsCancelled;
		}
		public async Task GridHR_EMP_EmployeesId_378_afterrendermodal(Entity.HR_EMP_AcademicDocuments Item)
		{
		}
		#endregion

		#region Grid EMP_Contacts

		public async Task<bool> GridHR_EMP_EmployeesId_380_editmodelsaving(object e)
		{
			bool IsCancelled = false;

			var item = e as Entity.HR_EMP_EmployeeDetails;
			IsCancelled = !await CheckFieldsValidation_EmpContacts(item);

			return IsCancelled;
		}
		public async Task GridHR_EMP_EmployeesId_380_afterrendermodal(Entity.HR_EMP_EmployeeDetails item)
		{
			var hasSavedRecord = _Entity.HR_EMP_EmployeeDetails?.Any(x => x.IsDelete != true) == true;

			// اگر item مشخص شده باشد و Id داشته باشد، مطمئن می‌شویم که ذخیره شده است
			if (item != null && item.Id != Guid.Empty && item.IsDelete != true)
			{
				// بررسی می‌کنیم که آیا این ردیف واقعاً در لیست وجود دارد
				var existsInList = _Entity.HR_EMP_EmployeeDetails?.Any(x => x.Id == item.Id && x.IsDelete != true) == true;
				if (existsInList)
				{
					hasSavedRecord = true;
				}
			}

			// تکمیل خودکار فیلد HR_EMP_EmployeesId در مودال جزئیات اطلاعات تماس بر اساس انتخاب کارمند در فرم اصلی
			// در حالت رکورد جدید ممکن است ref هنوز تنظیم نشده باشد، چند بار تلاش می‌کنیم
			bool prefilleed = false;
			int attempts = 0;
			while (!prefilleed && attempts < 3)
			{
				prefilleed = await PrefillEmployeeDetailsModal(item);
				if (!prefilleed) await Task.Delay(120);
				attempts++;
			}

			await EmployeeDetails_ToggleGridButton(hasSavedRecord, isInModal: true);
		}

		#region PrefillEmployeeDetailsModal
		/// <summary>
		/// تکمیل خودکار فیلد کارمند در مودال اطلاعات تماس بر اساس انتخاب کارمند در فرم اصلی
		/// </summary>
		private async Task<bool> PrefillEmployeeDetailsModal(Entity.HR_EMP_EmployeeDetails item)
		{
			// بررسی وجود داده‌های ضروری
			if (item == null || _Entity?.HR_EMP_EmployeesId == null || _Entity.HR_EMP_EmployeesId == Guid.Empty)
			{
				return false;
			}

			try
			{
				// 1. ست کردن مقدار در مدل ردیف مودال
				item.HR_EMP_EmployeesId = _Entity.HR_EMP_EmployeesId.Value;

				await Task.Delay(120);

				// 2. ست کردن مقدار در دراپ‌دان UI (اگر رفرنس موجود است)
				if (Ref_HR_EMP_EmployeeDetails_HR_EMP_EmployeesId != null)
				{
					// بارگذاری داده‌های جدید در دراپ‌دان
					await Ref_HR_EMP_EmployeeDetails_HR_EMP_EmployeesId.LoadData();

					// انتخاب مقدار فعلی
					var empObj = new
					{
						Id = _Entity.HR_EMP_EmployeesId.Value,
						Title = $"{_Entity.FirstName} {_Entity.LastName}"
					};
					Ref_HR_EMP_EmployeeDetails_HR_EMP_EmployeesId.ItemSelected(empObj);
				}

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in PrefillEmployeeDetailsModal: {ex.Message}");
				return false;
			}
		}
		#endregion



		#region EmployeeDetails Validation
		/// <summary>
		/// بررسی تعداد آخرین ردیف جزئیات
		/// بر اساس تعداد ردیف جزئیات انتخاب شده، مشخص می شود که آیا فرم قابل ارسال است یا خیر.
		/// فقط و فقط یک ردیف جزئیات مجاز است.
		/// </summary>
		/// <returns>true اگر دقیقاً یک ردیف غیر حذف‌شده وجود داشته باشد</returns>
		private async Task<bool> HasValidCountEmployeeDetails()
		{
			// بررسی null بودن لیست
			if (_Entity.HR_EMP_EmployeeDetails == null)
			{
				await _MSG.ShowError("لطفاً حداقل یک ردیف در بخش «جزئیات اطلاعات تماس» ثبت کنید.");
				return false;
			}

			// شمارش ردیف‌های فعال (غیر حذف‌شده)
			var activeCount = _Entity.HR_EMP_EmployeeDetails.Count(x => x.IsDelete != true);

			if (activeCount == 0)
			{
				await _MSG.ShowError("لطفاً حداقل یک ردیف در بخش «جزئیات اطلاعات تماس» ثبت کنید.");
				return false;
			}

			if (activeCount > 1)
			{
				await _MSG.ShowError("شما مجاز به ثبت بیش از یک ردیف نیستید. لطفاً فقط یک ردیف در بخش «جزئیات اطلاعات تماس» ثبت کنید.");
				return false;
			}

			return true;
		}
		#endregion

		#region EmployeeDetailsShowRecordDialog
		public string? EmpBy { get; set; }
		public string? EmpReason { get; set; }
		/// <summary>
		/// تابع قبل اجرا شدن ورودی ثبت مدل جزئیات
		/// </summary>
		/// <returns></returns>
		private async Task EmployeeDetailsShowRecordDialog()
		{
			bool IsValid = true;

			var options = new ConfirmDialogOptions
			{
				YesButtonText = "بازگشت به درخواست",
				YesButtonColor = ButtonColor.Danger,
				NoButtonText = "",
			};

			string html = $@"
                <div>
                    <picture><img src='https://file.workcv.ir/fa-ir/api/v1/File/Get?FileID=39462980-c664-4271-9f62-08de75e9da22' width='96px' alt='لوگو سامانه بایا' /></picture>
                    <hr class='hrdash border-success-subtle'>
                </div>
                <div class='fw-bold text-center'>
                    <span class='fs-3' style='color: #1ba156'>
						<div class='fs-6'>کاربر درخواست: {_User.NAME + "" + _User.FAMILY} </div>
						<div class='fs-6'>کارمند درخواست: {_Entity.FirstName + "" + _Entity.LastName + "" + _Entity.EmployeeNo} </div>
					</span>
					
                    <div>
                        <span><i class='fal fa-exclamation-triangle' style='font-size:24px; color:red;'></i>&nbsp;</span>
                        <span class='fs-6 text-secondary'>تا کنون هیچ ردیف اطلاعات تماسی از کارمند تکمیل نشده است. لطفاً برای ثبت و ادامه به مرحله بعد حداقل یک ردیف در درخواست خود ثبت نمایید.</span>
                    </div>
                </div>";

			await Confirm.ShowAsync(title: "", message1: html, confirmDialogOptions: options);

		}

		#endregion

		#region EmployeeDetails_ToggleGridButton
		private async Task EmployeeDetails_ToggleGridButton(bool hasSavedRecord = false, bool isInModal = false)
		{
			await Task.Yield();

			// اگر hasSavedRecord مشخص نشده باشد، خودکار بررسی می‌کنیم
			// بررسی می‌کنیم که آیا یک ردیف غیر حذف‌شده در لیست وجود دارد
			if (!hasSavedRecord)
			{
				hasSavedRecord = _Entity.HR_EMP_EmployeeDetails?.Any(x => x.IsDelete != true) == true;
			}

			// اگر یک ردیف ذخیره شده وجود دارد، دکمه افزودن را مخفی می‌کنیم
			// چون فقط یک ردیف مجاز است
			if (hasSavedRecord)
			{
				// ردیف وجود دارد → دکمه جدید را مخفی می‌کنیم
				await JS.InvokeVoidAsync("AddClass", "#HR_EMP_EmployeeDetails_GridHR_EMP_EmployeesId_380ButtonNew", "d-none");
			}
			else
			{
				// ردیف وجود ندارد → دکمه جدید را نمایش می‌دهیم
				await JS.InvokeVoidAsync("RemoveClass", "#HR_EMP_EmployeeDetails_GridHR_EMP_EmployeesId_380ButtonNew", "d-none");
			}

			// فقط اگر در مودال باشیم، دکمه‌های مودال را مخفی کن
			if (isInModal)
			{
				// دکمه ذخیره و جدید - مخفی می‌شود چون فقط یک ردیف مجاز است
				try
				{
					if (await WaitElementExists("#HR_EMP_EmployeeDetails_GridHR_EMP_EmployeesId_380ButtonSaveAndNew", 1000))
					{
						await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeDetails_GridHR_EMP_EmployeesId_380ButtonSaveAndNew", "d-none");
					}
				}
				catch (JSException jsEx)
				{
					Console.WriteLine($"JSException hiding SaveAndNew button: {jsEx.Message}");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error hiding SaveAndNew button: {ex.Message}");
				}

				// دکمه قبلی - مخفی می‌شود چون فقط یک ردیف وجود دارد
				try
				{
					if (await WaitElementExists("#HR_EMP_EmployeeDetails_GridHR_EMP_EmployeesId_380ButtonBefore", 1000))
					{
						await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeDetails_GridHR_EMP_EmployeesId_380ButtonBefore", "d-none");
					}
				}
				catch (JSException jsEx)
				{
					Console.WriteLine($"JSException hiding Before button: {jsEx.Message}");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error hiding Before button: {ex.Message}");
				}

				// دکمه بعدی - مخفی می‌شود چون فقط یک ردیف وجود دارد
				try
				{
					if (await WaitElementExists("#HR_EMP_EmployeeDetails_GridHR_EMP_EmployeesId_380ButtonNext", 1000))
					{
						await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeDetails_GridHR_EMP_EmployeesId_380ButtonNext", "d-none");
					}
				}
				catch (JSException jsEx)
				{
					Console.WriteLine($"JSException hiding Next button: {jsEx.Message}");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error hiding Next button: {ex.Message}");
				}
			}
		}
		#endregion

		#endregion

		#region btn data
		public async Task submit_onclick(MouseEventArgs Selected)
		{
			// فایل راهنمای فرم اطلاعات بیمه تکمیلی کارمند
			await OpenHelpFileInNewTab0();
		}

		public async Task submit1_onclick(MouseEventArgs Selected)
		{
			await OpenHelpFileInNewTab0();
		}

		/// <summary>
		/// لینک فایل راهنمای فرم اطلاعات بیمه تکمیلی کارمند
		/// </summary>
		/// <returns></returns>
		private async Task OpenHelpFileInNewTab0()
		{
			// نهایی بیمه درمان - عمر و حادثه
			var url = "https://file.workcv.ir/fa-ir/api/v1/File/Get?FileID=4c689678-179d-4312-856f-08ddf993cb5b";
			await JS.InvokeVoidAsync("window.open", url, "_blank");
		}

		private async Task OpenHelpFileInNewTab1()
		{
			// فرم تعیین ذینفع بیمه عمر و حادثه
			var url = "https://file.workcv.ir/fa-ir/api/v1/File/Get?FileID=1297c4b8-023b-4014-8570-08ddf993cb5b";
			await JS.InvokeVoidAsync("window.open", url, "_blank");
		}

		#endregion

		#region Export
		public async Task ExportFile_onclick(MouseEventArgs Selected)
		{
			//
			await ExportInsuranceDataToExcel();
		}

		/// <summary>
		/// خروجی اکسل اطلاعات بیمه تکمیلی کارمندان (شامل اطلاعات کارمند، حساب بانکی و خانواده)
		/// </summary>
		public async Task ExportInsuranceDataToExcel()
		{
			if (_User.UserID.ToString() != "c5eb69f8-0152-470c-a7a3-0c6b39098b7c")
			{
				return;
			}
			var employees = await FetchInsuranceDataForExport();
			await GenerateInsuranceExcelFile(employees);
		}

		private async Task<List<Entity.HR_EMP_Employees>> FetchInsuranceDataForExport()
		{
			var exportTable = new Table
			{
				Name = "HR_EMP_Employees",
				Column = new List<Coulmn>
				{
					new Coulmn { Name = "Id" },                         // شناسه کارمند
                    new Coulmn { Name = "FirstName" },                  // نام
                    new Coulmn { Name = "LastName" },                   // نام خانوادگی
                    new Coulmn() { Name = "EmployeeNo" },               // کد کارمندی
                    new Coulmn() { Name = "EmployeeLastPersonelNo" },   // کد کارمندی قدیم
                    new Coulmn() { Name = "EmployeePersonelNo" },       // کد پرسنلی
                },
				Relation = new List<Table>
				{
                    // جزئیات کارمند (اطلاعات اصلی بیمه)
                    new Table
					{
						Name = "HR_EMP_EmployeeInfos",
						Column = new List<Coulmn>
						{
							new Coulmn { Name = "BirthDate_Fa" },     // تاریخ تولد
                            new Coulmn { Name = "IdCardNo" },         // شماره شناسنامه
                            new Coulmn { Name = "NationalCode" },     // کد ملی
                            new Coulmn { Name = "FatherName" },       // نام پدر
                            new Coulmn { Name = "Mobile" },           // شماره تماس
                            new Coulmn { Name = "BaseInfo_GenderId" } // جنسیت
                        },
						Relation = new List<Table>
						{
                            // جنسیت
                            new Table
							{
								Name = "BaseInfo_Gender",
								Column = new List<Coulmn> { new Coulmn { Name = "Title" } },
								ModeErtebat = ModeErtebat._1N
							}
						},
						ModeErtebat = ModeErtebat._N1
					},
                    // حساب بانکی
                    new Table
					{
						Name = "HR_Base_BankAccount",
						Column = new List<Coulmn>
						{
							new Coulmn { Name = "IBAN" } // شماره شبا
                        },
						Relation = new List<Table>
						{
                            // بانک
                            new Table
							{
								Name = "BaseInfo_Bank",
								Column = new List<Coulmn> { new Coulmn { Name = "Title" } }, // عنوان بانک
                                ModeErtebat = ModeErtebat._1N
							}
						},
						ModeErtebat = ModeErtebat._N1
					},
                    // اطلاعات خانواده (برای فیلدهای نسبت، وضعیت تکفل، کد ملی بیمه شده اصلی)
                    new Table
					{
						Name = "HR_EMP_EmployeeFamileis",
						Column = new List<Coulmn>
						{
							new Coulmn {Name= "FirstName"},                 // نام
                            new Coulmn {Name= "LastName"},                  // نام خانوادگی
                            new Coulmn {Name= "FatherName"},                // نام پدر
                            new Coulmn {Name= "NationalCode"},              // کد ملی بیمه شده (عضو خانواده)
                            new Coulmn {Name= "IdCardNo"},                  // شماره شناسنامه
                            new Coulmn {Name= "IsFamily"},                  // پوشش خانوار
                            new Coulmn {Name= "BirthDate"},                 // تاریح تولد
                            new Coulmn {Name= "BaseInfo_GenderId"},         // جنسیت
                            new Coulmn {Name= "BaseInfo_MaritalStatusId"},  // وضعیت تاهل
                            new Coulmn {Name= "HR_FamilyRelationshipId"},   // نسبت
                            new Coulmn {Name= "BaseInfo_CitiesId"},         // شهر محل تولد
                            new Coulmn {Name= "HR_Base_DependentId"}        // وضعیت تکفل
                        },
						Relation = new List<Table>
						{
                            // جنسیت
                            new Table
							{
								Name = "BaseInfo_Gender",
								Column = new List<Coulmn> { new Coulmn { Name = "Title" } },
								ModeErtebat = ModeErtebat._1N
							},
                            // وضعیت تاهل
                            new Table
							{
								Name = "BaseInfo_MaritalStatus",
								Column = new List<Coulmn> { new Coulmn { Name = "Title" } },
								ModeErtebat = ModeErtebat._1N
							},
                            // نسبت
                            new Table
							{
								Name = "HR_FamilyRelationship",
								Column = new List<Coulmn> { new Coulmn { Name = "Title" } },
								ModeErtebat = ModeErtebat._1N
							},
                            // شهر محل تولد
                            new Table
							{
								Name = "BaseInfo_Cities",
								Column = new List<Coulmn> { new Coulmn { Name = "Title" } },
								ModeErtebat = ModeErtebat._1N
							},
                            // وضعیت تکفل
                            new Table
							{
								Name = "HR_Base_Dependent",
								Column = new List<Coulmn> { new Coulmn { Name = "Title" } },
								ModeErtebat = ModeErtebat._1N
							}
						},
						ModeErtebat = ModeErtebat._N1
					}
				},
			};
			Baya.Models.ORM.PagedResult pager = new()
			{
				PageSize = 1000,
				PageNumber = 1,
			};
			//var dataResult = await ApiServer.External.Services.Data.GetList("HR_EMP_Employees", 1000, 0, exportTable, null);
			var dataResult = await ApiServer.External.Services.Data.GetListPost(exportTable, null, pager, "HR_EMP_Employees");
			Console.WriteLine("#1 " + dataResult.Content.ToString());
			Baya.Models.ORM.PagedResult result = await JSON.ToObject<Baya.Models.ORM.PagedResult>(dataResult.Content.ToString());

			if (dataResult?.Status == HttpStatusCode.OK)
			{
				//Console.WriteLine("#2 " + dataResult.Content.ToString());
				string sData = await JSON.ToJson(result.Items);
				var data = await JSON.ToObject<List<Entity.HR_EMP_Employees>>(sData);
				Console.WriteLine("#data " + sData);
				return data;
			}

			return new List<Entity.HR_EMP_Employees>();
		}

		private async Task GenerateInsuranceExcelFile(List<Entity.HR_EMP_Employees> employees)
		{
			using var workbook = new ClosedXML.Excel.XLWorkbook();
			var worksheet = workbook.Worksheets.Add("اطلاعات بیمه کارمندان");

			// هدرها
			var headers = new[] {
				"نام", "نام خانوادگی", "تاریخ تولد", "شماره شناسنامه", "کد ملی", "نام پدر",
				"ملیت", "شماره تماس", "نسبت", "جنسیت", "وضعیت تکفل", "بیمه شده اصلی",
				"کد ملی بیمه شده اصلی", "شماره شبا", "بانک"
			};

			for (int i = 0; i < headers.Length; i++)
				worksheet.Cell(1, i + 1).Value = headers[i];

			int rowIndex = 2;

			foreach (var emp in employees)
			{
				var info = emp.HR_EMP_EmployeeInfos?.FirstOrDefault();
				var bankAccount = emp.HR_Base_BankAccount?.FirstOrDefault();

				// اگر عضو خانواده‌ای وجود دارد، برای هر کدام یک سطر
				var familyMembers =
					emp.HR_EMP_EmployeeFamileis?.Where(f => f.IsDelete != true).ToList() ?? new List<Entity.HR_EMP_EmployeeFamileis>();

				if (familyMembers.Any())
				{
					foreach (var familyMember in familyMembers)
					{
						worksheet.Cell(rowIndex, 1).Value = emp.FirstName ?? "-";
						worksheet.Cell(rowIndex, 2).Value = emp.LastName ?? "-";
						worksheet.Cell(rowIndex, 3).Value = info?.BirthDate_Fa ?? "-";
						worksheet.Cell(rowIndex, 4).Value = info?.IdCardNo ?? "-";
						worksheet.Cell(rowIndex, 5).Value = info?.NationalCode ?? "-";
						worksheet.Cell(rowIndex, 6).Value = info?.FatherName ?? "-";
						worksheet.Cell(rowIndex, 7).Value = "ایران";
						worksheet.Cell(rowIndex, 8).Value = info?.Mobile ?? "-";
						worksheet.Cell(rowIndex, 9).Value = familyMember.HR_FamilyRelationship?.Title ?? "-";
						worksheet.Cell(rowIndex, 10).Value = familyMember.BaseInfo_Gender?.Title ?? "-";
						worksheet.Cell(rowIndex, 11).Value = familyMember.HR_Base_Dependent?.Title ?? "-";
						worksheet.Cell(rowIndex, 12).Value = familyMember.FirstName + " " + familyMember.LastName;
						worksheet.Cell(rowIndex, 13).Value = familyMember.NationalCode ?? "-";
						worksheet.Cell(rowIndex, 14).Value = bankAccount?.IBAN ?? "-";
						worksheet.Cell(rowIndex, 15).Value = bankAccount?.BaseInfo_Bank?.Title ?? "-";

						rowIndex++;
					}
				}
				else
				{
					// اگر عضو خانواده‌ای نیست، فقط خود کارمند را بیمه‌شده اصلی در نظر بگیر
					worksheet.Cell(rowIndex, 1).Value = emp.FirstName ?? "-";
					worksheet.Cell(rowIndex, 2).Value = emp.LastName ?? "-";
					worksheet.Cell(rowIndex, 3).Value = info?.BirthDate_Fa ?? "-";
					worksheet.Cell(rowIndex, 4).Value = info?.IdCardNo ?? "-";
					worksheet.Cell(rowIndex, 5).Value = info?.NationalCode ?? "-";
					worksheet.Cell(rowIndex, 6).Value = info?.FatherName ?? "-";
					worksheet.Cell(rowIndex, 7).Value = "ایران";
					worksheet.Cell(rowIndex, 8).Value = info?.Mobile ?? "-";
					worksheet.Cell(rowIndex, 9).Value = "-"; // نسبت
					worksheet.Cell(rowIndex, 10).Value = info?.BaseInfo_Gender?.Title ?? "-";
					worksheet.Cell(rowIndex, 11).Value = "-"; // وضعیت تکفل
					worksheet.Cell(rowIndex, 12).Value = "خود کارمند";
					worksheet.Cell(rowIndex, 13).Value = info?.NationalCode ?? "-";
					worksheet.Cell(rowIndex, 14).Value = bankAccount?.IBAN ?? "-";
					worksheet.Cell(rowIndex, 15).Value = bankAccount?.BaseInfo_Bank?.Title ?? "-";

					rowIndex++;
				}
			}

			// تنظیمات ظاهری
			worksheet.Columns().AdjustToContents();
			worksheet.Row(1).Style.Font.Bold = true;
			worksheet.Row(1).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;

			// دانلود
			using var stream = new MemoryStream();
			workbook.SaveAs(stream);
			stream.Position = 0;
			var content = stream.ToArray();

			await JS.InvokeVoidAsync(
				"downloadFileFromStream",
				"EmployeeInsuranceData.xlsx",
				Convert.ToBase64String(content));
		}
		#endregion /Export

		#region btn_Help
		public async Task btn_Help_onclick(MouseEventArgs Selected)
		{
			var options = new ConfirmDialogOptions
			{
				YesButtonText = "بازگشت به فرم نمایه",
				YesButtonColor = ButtonColor.Danger,
				NoButtonText = "",
			};

			string htmlString = @"
			<div class='btnHelp'>
				<!-- Header -->
				<div class='header-section'>
					<h1 style='color: white; font-size: 2.2rem; margin: 15px 0;'>🛡️ راهنمای جامع فرم نمایه اطلاعات کارمند</h1>
					<p class='lead' style='color: rgba(255, 255, 255, 0.9); font-size: 1.1rem;'>
						تمامی نکات و الزامات تکمیل فرم اطلاعات کارمند به صورت کامل و طبقه‌بندی شده
					</p>
				</div>

				<!-- اطلاعات اصلی کارمند -->
				<div class='section-card'>
					<h3 class='section-title'>
						<i class='bi bi-person-badge'></i>
						اطلاعات اصلی کارمند
					</h3>
        
					<div class='field-group'>
						<div class='step-indicator'>
							<span class='step-number'>1</span>
							<h5 style='margin: 0; color: var(--primary-color);'>اطلاعات هویتی</h5>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								نام
							</div>
							<div class='field-desc'>
								نام فارسی کارمند مطابق شناسنامه - بدون علائم و کاراکترهای خاص
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								نام خانوادگی
							</div>
							<div class='field-desc'>
								نام خانوادگی فارسی مطابق شناسنامه
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								کد ملی
							</div>
							<div class='field-desc'>
								۱۰ رقم - بدون خط تیره - مطابق کارت ملی
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								کد کارمند
							</div>
							<div class='field-desc'>
								کد اختصاصی سازمان - منحصر به فرد
							</div>
						</div>
            
						<div class='validation-rules'>
							<div class='rule-item'>
								<i class='bi bi-check-circle'></i>
								<span>نام و نام خانوادگی باید فارسی و بدون اعداد باشد</span>
							</div>
							<div class='rule-item'>
								<i class='bi bi-check-circle'></i>
								<span>کد ملی باید ۱۰ رقم و معتبر باشد</span>
							</div>
							<div class='rule-item'>
								<i class='bi bi-check-circle'></i>
								<span>کد کارمند نباید تکراری باشد</span>
							</div>
						</div>
					</div>
				</div>

				<!-- جزئیات اطلاعات کارمند -->
				<div class='section-card'>
					<h3 class='section-title'>
						<i class='bi bi-file-person'></i>
						جزئیات اطلاعات کارمند
					</h3>
        
					<div class='important-note'>
						<i class='bi bi-exclamation-triangle-fill'></i>
						<strong>توجه:</strong> فقط یک ردیف از این بخش قابل ثبت است
					</div>
        
					<div class='field-group'>
						<div class='step-indicator'>
							<span class='step-number'>2</span>
							<h5 style='margin: 0; color: var(--primary-color);'>اطلاعات شناسنامه‌ای</h5>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								نام پدر
							</div>
							<div class='field-desc'>
								نام کامل پدر مطابق شناسنامه
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								شماره شناسنامه
							</div>
							<div class='field-desc'>
								شماره شناسنامه بدون حروف و علائم اضافی
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								سریال شناسنامه
							</div>
							<div class='field-desc'>
								در سه بخش جداگانه وارد شود (مثال: ۱۲۳ - الف ۴۵۶)
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								شهر محل صدور
							</div>
							<div class='field-desc'>
								از لیست استان‌ها و شهرستان‌ها انتخاب شود
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								تاریخ تولد
							</div>
							<div class='field-desc'>
								به صورت شمسی (مثال: ۱۳۷۵/۰۵/۱۵)
							</div>
						</div>
					</div>
        
					<div class='field-group'>
						<div class='step-indicator'>
							<span class='step-number'>3</span>
							<h5 style='margin: 0; color: var(--primary-color);'>سابقه کار و بیمه</h5>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								فایل سابقه کار بیمه
							</div>
							<div class='field-desc'>
								PDF یا تصویر واضح - حداکثر ۵ مگابایت
								<b>
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								سابقه کار (روز)
							</div>
							<div class='field-desc'>
								تعداد روزهای سابقه کار محاسبه شده
							</div>
						</div>
					</div>
        
					<div class='field-group'>
						<div class='step-indicator'>
							<span class='step-number'>4</span>
							<h5 style='margin: 0; color: var(--primary-color);'>اطلاعات شخصی</h5>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								جنسیت
							</div>
							<div class='field-desc'>
								مرد یا زن
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								گروه خونی
							</div>
							<div class='field-desc'>
								A+, A-, B+, B-, AB+, AB-, O+, O-
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								دین
							</div>
							<div class='field-desc'>
								اسلام، مسیحیت، یهودیت، زرتشتی، سایر
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								مذهب
							</div>
							<div class='field-desc'>
								شیعه، سنی، کاتولیک، ارتدکس، ...
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								وضعیت نظام وظیفه
							</div>
							<div class='field-desc'>
								برای آقایان: مشمول، معاف، پایان خدمت، ...
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								تلفن همراه
							</div>
							<div class='field-desc'>
								۱۱ رقم - شروع با ۰۹ - فعال
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								نشانی
							</div>
							<div class='field-desc'>
								کامل و دقیق شامل: استان، شهر، خیابان، پلاک، واحد
							</div>
						</div>
					</div>
				</div>

				<!-- اطلاعات خانواده کارمند -->
				<div class='section-card'>
					<h3 class='section-title'>
						<i class='bi bi-people-fill'></i>
						اطلاعات خانواده کارمند
					</h3>
        
					<div class='grid-visual'>
						<div class='visual-item'>
							<h5><i class='bi bi-person-check'></i> فیلدهای الزامی</h5>
							<ul>
								<li>نسبت خانوادگی</li>
								<li>وضعیت تکفل</li>
								<li>نام و نام خانوادگی</li>
								<li>کد ملی</li>
								<li>نام پدر</li>
								<li>شماره شناسنامه</li>
								<li>تاریخ تولد</li>
								<li>جنسیت</li>
								<li>وضعیت تاهل</li>
							</ul>
						</div>
            
						<div class='visual-item'>
							<h5><i class='bi bi-file-earmark-text'></i> مدارک موردنیاز</h5>
							<ul>
								<li>کپی شناسنامه</li>
								<li>کپی کارت ملی</li>
								<li>گواهی اشتغال به تحصیل (برای محصلین)</li>
								<li>مدرک ازدواج/طلاق/فوت (در صورت نیاز)</li>
							</ul>
						</div>
            
						<div class='visual-item'>
							<h5><i class='bi bi-info-circle'></i> نکات مهم</h5>
							<ul>
								<li>هر عضو خانواده یک ردیف جداگانه</li>
								<li>کد ملی باید منحصر به فرد باشد</li>
								<li>تاریخ تولد شمسی وارد شود</li>
								<li>نسبت: پدر، مادر، همسر، فرزند، ...</li>
							</ul>
						</div>
					</div>
				</div>

				<!-- حساب بانکی -->
				<div class='section-card'>
					<h3 class='section-title'>
						<i class='bi bi-bank2'></i>
						اطلاعات حساب بانکی
					</h3>
        
					<div class='important-note'>
						<i class='bi bi-exclamation-triangle-fill'></i>
						<strong>محدودیت:</strong> فقط یک حساب بانکی قابل ثبت است
					</div>
        
					<div class='field-group'>
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								دسته‌بندی حساب
							</div>
							<div class='field-desc'>
								حقوقی، پاداش، وام، ...
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								نوع حساب
							</div>
							<div class='field-desc'>
								جاری، پس‌انداز، قرض‌الحسنه
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								بانک
							</div>
							<div class='field-desc'>
								نام بانک از لیست انتخاب شود
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								شعبه
							</div>
							<div class='field-desc'>
								شعبه بانک از لیست انتخاب شود
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								شماره حساب
							</div>
							<div class='field-desc'>
								شماره حساب بانکی بدون خط تیره
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								شماره شبا
							</div>
							<div class='field-desc'>
								۲۶ کاراکتر - شروع با IR
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								شماره کارت
							</div>
							<div class='field-desc'>
								۱۶ رقم - بدون فاصله
							</div>
						</div>
					</div>
				</div>

				<!-- اطلاعات تماس کارمند -->
				<div class='section-card'>
					<h3 class='section-title'>
						<i class='bi bi-telephone-fill'></i>
						اطلاعات تماس کارمند
					</h3>
        
					<div class='important-note'>
						<i class='bi bi-exclamation-triangle-fill'></i>
						<strong>توجه ویژه:</strong> فقط یک ردیف تماس سازمانی قابل ثبت است
					</div>
        
					<div class='field-group'>
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								نوع تماس
							</div>
							<div class='field-desc'>
								موبایل سازمانی، تلفن داخلی، ایمیل سازمانی، ...
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								شماره/آدرس
							</div>
							<div class='field-desc'>
								متناسب با نوع تماس وارد شود
							</div>
						</div>
					</div>
        
					<div class='validation-rules'>
						<h6><i class='bi bi-card-checklist'></i> فرمت‌های معتبر:</h6>
						<div class='rule-item'>
							<i class='bi bi-phone'></i>
							<span><strong>موبایل:</strong> ۰۹۱۲۳۴۵۶۷۸۹</span>
						</div>
						<div class='rule-item'>
							<i class='bi bi-telephone'></i>
							<span><strong>تلفن:</strong> ۰۲۱۸۸۸۸۸۸۸۸</span>
						</div>
						<div class='rule-item'>
							<i class='bi bi-envelope'></i>
							<span><strong>ایمیل:</strong> name@company.com</span>
						</div>
					</div>
				</div>

				<!-- مدارک کارمند -->
				<div class='section-card'>
					<h3 class='section-title'>
						<i class='bi bi-folder-fill'></i>
						مدارک کارمند
					</h3>
        
					<div class='field-group'>
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								عنوان مدرک
							</div>
							<div class='field-desc'>
								نام توصیفی مدرک (مثال: تصویر کارت ملی)
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								نوع مدرک
							</div>
							<div class='field-desc'>
								شناسنامه، کارت ملی، مدرک تحصیلی، ...
							</div>
						</div>
            
						<div class='field-item'>
							<div class='field-name'>
								<span class='requirement required'>الزامی</span>
								فایل مدرک
							</div>
							<div class='field-desc'>
								PDF, JPG, PNG - حداکثر ۳ مگابایت
							</div>
						</div>
					</div>
				</div>

				<!-- خلاصه و نکات نهایی -->
				<div class='section-card' style='background: linear-gradient(135deg, #f8f9fa, #e9ecef);'>
					<h3 class='section-title'>
						<i class='bi bi-lightbulb-fill'></i>
						خلاصه و نکات نهایی
					</h3>
        
					<div class='grid-visual'>
						<div class='visual-item' style='border-top-color: var(--success-color);'>
							<h5><i class='bi bi-check-circle-fill' style='color: var(--success-color);'></i> قبل از ثبت</h5>
							<ul>
								<li>تمام فیلدهای الزامی تکمیل شده باشد</li>
								<li>فرمت‌ها به درستی رعایت شده باشد</li>
								<li>تعداد ردیف‌ها مطابق محدودیت باشد</li>
								<li>فایل‌ها با کیفیت مناسب آپلود شده باشد</li>
							</ul>
						</div>
            
						<div class='visual-item' style='border-top-color: var(--warning-color);'>
							<h5><i class='bi bi-exclamation-triangle-fill' style='color: var(--warning-color);'></i> محدودیت‌ها</h5>
							<ul>
								<li>جزئیات اطلاعات: ۱ ردیف</li>
								<li>حساب بانکی: ۱ ردیف</li>
								<li>تماس سازمانی: ۱ ردیف</li>
								<li>خانواده: چند ردیف (بر حسب نیاز)</li>
								<li>مدارک: چند ردیف (بر حسب نیاز)</li>
							</ul>
						</div>
            
						<div class='visual-item' style='border-top-color: var(--info-color);'>
							<h5><i class='bi bi-shield-check' style='color: var(--info-color);'></i> امنیت اطلاعات</h5>
							<ul>
								<li>اطلاعات محرمانه تلقی می‌شوند</li>
								<li>فقط افراد مجاز دسترسی دارند</li>
								<li>اطلاعات رمزنگاری شده ذخیره می‌شود</li>
								<li>لاگ دسترسی‌ها ثبت می‌شود</li>
							</ul>
						</div>
					</div>
        
					<div style='text-align: center; margin-top: 30px; padding: 20px; background: white; border-radius: 10px;'>
						<span class='badge-custom'>تعداد کل بخش‌ها: ۶</span>
						<span class='badge-custom'>فیلدهای الزامی: ۳۲+</span>
						<span class='badge-custom'>حداکثر زمان تکمیل: ۲۰ دقیقه</span>
            
						<div style='margin-top: 20px;'>
							<button class='btn btn-primary btn-lg' style='padding: 12px 40px;'>
								<i class='bi bi-send-check'></i>
								شروع تکمیل فرم
							</button>
						</div>
					</div>
				</div>
			</div>";

			var confirmation = await Confirm.ShowAsync(
				title: "",
				message1: htmlString,
				confirmDialogOptions: options);
		}
		#endregion

		#region
		#endregion

		#endregion FunctionEvents

	}
}