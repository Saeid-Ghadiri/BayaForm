using Baya.Models.ORM;
using Baya.Models.Utility;
using Baya.Models.Utility.Entity;
using Baya.Models.Utility.Menu;
using Baya.Models.Utility.Pagination.Pagings;
using BlazorBootstrap;
using Blazored.Toast.Services;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using Sitko.Blazor.CKEditor;
using System;
using System.Net;
using System.Web;
using Utility;

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

				//await JS.InvokeVoidAsync("ModalAddClass", "#btn_listi_Save_New", "d-none");
				await JS.InvokeVoidAsync("AddClass", "#btn_listi_Previous", "d-none");
				await JS.InvokeVoidAsync("AddClass", "#btn_listi_Next", "d-none");

				//// بررسی و تغییرات حساب بانکی کارمند
				//await CheckGridDataAndToggleButton();

				// بررسی دکمه های اطلاعات تماس کارمند
				await EmployeeDetails_ToggleGridButton();

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

			// بررسی اطلاعات تماس
			if (!await HasValidCountEmployeeDetails())
			{
				// بررسی اینکه آیا رکوردی در بخش اطلاعات تماس وجود دارد یا خیر
				await EmployeeDetailsShowRecordDialog();
				return false;
			}
			
			List<Entity.HR_EMP_EmployeeDetails> EmployeeDetailsList = 
				_Entity.HR_EMP_EmployeeDetails.Where(x => x.IsDelete != true).ToList();

			//بررسی اعتبار سنجی فیلدها برای هر ردیف جزئیات یک به یک
			foreach (var Item in EmployeeDetailsList)
			{
				IsValid = IsValid && await CheckFieldsValidation_EmpContacts();
			}


			//// بررسی اطلاعات حساب بانکی کارمند
			//IsValid = IsValid && await DataBankAccount(_Entity.HR_Base_BankAccount);

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
			await EmployeeDetails_ToggleGridButton();
		}


		#region FunctionEvents

		#region CheckFieldValidation
		public async Task<bool> CheckFieldsValidation_Employee(Entity.HR_EMP_Employees Item)
		{
			bool IsValid = true;
			// **************************************************
			// اطلاعات اصلی کارمند
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

			// // فیلد کد ملی
			// if (Item.NationalCode == null)
			// {
			//     IsValid = false;
			//     await _MSG.ShowError("لطفا گزینه کد ملی را تکمیل نمایید.");
			// }

			// // فیلد کد کارمند
			// if (Item.EmployeeNo == null)
			// {
			//     IsValid = false;
			//     await _MSG.ShowError("لطفا گزینه کد کارمند را تکمیل نمایید.");
			// }

			// // فیلد کد قدیم پرسنلی کارمند
			// if (Item.EmployeeLastPersonelNo == null)
			// {
			//     IsValid = false;
			//     await _MSG.ShowError("لطفا گزینه کد قدیم پرسنلی کارمند را تکمیل نمایید.");
			// }

			// // فیلد کد پرسنلی کارمند
			// if (Item.EmployeePersonelNo == null)
			// {
			//     IsValid = false;
			//     await _MSG.ShowError("لطفا گزینه کد پرسنلی کارمند را تکمیل نمایید.");
			// }

			// // فیلد وضعیت کارمند
			// if (Item.HR_EMP_StatusId == null)
			// {
			//     IsValid = false;
			//     await _MSG.ShowError("لطفا گزینه وضعیت کارمند را تکمیل نمایید.");
			// }

			// **************************************************
			// **************************************************
			// اطلاعات جزئیات کارمند
			foreach (var item in _Entity.HR_EMP_EmployeeInfos)
			{
				// فیلد نام پدر
				if (item.FatherName == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه نام پدر را تکمیل نمایید.");
				}

				// شماره شناسنامه
				if (item.IdCardNo == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه شماره شناسنامه را تکمیل نمایید.");
				}

				// سریال شماره شناسنامه
				if (item.IdCardSerialNoSection1 == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه سریال شماره شناسنامه را تکمیل نمایید.");
				}

				// سریال شماره شناسنامه
				if (item.IdCardSerialNoSection2 == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه سریال شماره شناسنامه را تکمیل نمایید.");
				}

				// سریال شماره شناسنامه
				if (item.IdCardSerialNoSection3 == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه سریال شماره شناسنامه را تکمیل نمایید.");
				}

				// وضعیت تاهل
				if (item.BaseInfo_MaritalStatusId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه وضعیت تاهل را تکمیل نمایید.");
				}

				// شهر محل صدور
				if (item.CityOfIssue == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه شهر محل صدور را تکمیل نمایید.");
				}

				// شهر محل تولد
				if (item.CityOfBirth == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه شهر محل تولد را تکمیل نمایید.");
				}

				// تاریخ تولد
				if (item.BirthDate_Fa == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه تاریخ تولد را تکمیل نمایید.");
				}

				// فایل سابقه کار بیمه
				if (item.EmployeeWorkExperienceFile == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه فایل سابقه کار بیمه را تکمیل نمایید.");
				}

				// سابقه کار کارمند به روز
				if (item.EmployeeWorkExperience == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه سابقه کار کارمند به روز را تکمیل نمایید.");
				}

				// جنسیت
				if (item.BaseInfo_GenderId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه جنسیت را تکمیل نمایید.");
				}

				// وضعیت گروه خونی
				if (item.BaseInfo_BloodGroupId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه وضعیت گروه خونی را تکمیل نمایید.");
				}

				// دین
				if (item.BaseInfo_ReligionId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه دین را تکمیل نمایید.");
				}

				// مذهب
				if (item.BaseInfo_DenominationsId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه مذهب را تکمیل نمایید.");
				}

				// وضعیت نظام وظیفه
				if (item.BaseInfo_MilitaryStatusId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه وضعیت نظام وظیفه را تکمیل نمایید.");
				}

				// تلفن همراه
				if (item.Mobile == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه تلفن همراه را تکمیل نمایید.");
				}

				// نشانی
				if (item.Address == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه نشانی را تکمیل نمایید.");
				}
			}
			// **************************************************

			// **************************************************
			// اطلاعات خانواده کارمند
			foreach (var item in _Entity.HR_EMP_EmployeeFamileis)
			{
				// نسبت
				if (item.HR_FamilyRelationshipId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه نسبت را تکمیل نمایید.");
				}

				// تحت تکفل
				if (item.HR_Base_DependentId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه تحت تکفل را تکمیل نمایید.");
				}

				// نام
				if (item.FirstName == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه نام را تکمیل نمایید.");
				}

				// نام خانوادگی
				if (item.LastName == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه نام خانوادگی را تکمیل نمایید.");
				}

				// کد ملی
				if (item.NationalCode == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه کد ملی را تکمیل نمایید.");
				}

				// نام پدر
				if (item.FatherName == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه نام پدر را تکمیل نمایید.");
				}

				// شماره شناسنامه
				if (item.IdCardNo == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه شماره شناسنامه را تکمیل نمایید.");
				}

				// تاریخ تولد
				if (item.BirthDate_Fa == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه تاریخ تولد را تکمیل نمایید.");
				}

				// جنسیت
				if (item.BaseInfo_GenderId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه جنسیت را تکمیل نمایید.");
				}

				// وضعیت تاهل
				if (item.BaseInfo_MaritalStatusId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه وضعیت تاهل را تکمیل نمایید.");
				}

			}

			// **************************************************

			// **************************************************
			// اطلاعات حساب بانکی
			foreach (var item in _Entity.HR_Base_BankAccount)
			{
				// سته بندی حساب های بانکی کارمند
				if (item.HR_Base_BankAcountCategoryId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه سته بندی حساب های بانکی کارمند را تکمیل نمایید.");
				}

				// نوع حساب بانکی
				if (item.BaseInfo_BankAccountTypeId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه نوع حساب بانکی را تکمیل نمایید.");
				}

				// بانک
				if (item.BaseInfo_BankId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه بانک را تکمیل نمایید.");
				}

				// شعبه بانک
				if (item.BaseInfo_BankBranchesId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه شعبه بانک را تکمیل نمایید.");
				}

				// شماره حساب بانکی
				if (item.BankAccountNumber == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه شماره حساب بانکی را تکمیل نمایید.");
				}

				// شبا بانکی
				if (item.IBAN == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه شبا بانکی را تکمیل نمایید.");
				}

				// شماره کارت بانکی
				if (item.CartNo == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه شماره کارت بانکی را تکمیل نمایید.");
				}
			}
			// **************************************************

			// **************************************************
			// اطلاعات تماس
			foreach (var item in _Entity.HR_EMP_EmployeeDetails)
			{
				// 

			}
			// **************************************************

			return IsValid;
		}

		public async Task<bool> CheckFieldsValidation_EmployeeFamilies()
		{
			bool IsValid = true;

			// **************************************************
			// اطلاعات خانواده کارمند
			foreach (var item in _Entity.HR_EMP_EmployeeFamileis)
			{
				// نسبت
				if (item.HR_FamilyRelationshipId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه نسبت را تکمیل نمایید.");
				}

				// تحت تکفل
				if (item.HR_Base_DependentId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه تحت تکفل را تکمیل نمایید.");
				}

				// نام
				if (item.FirstName == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه نام را تکمیل نمایید.");
				}

				// نام خانوادگی
				if (item.LastName == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه نام خانوادگی را تکمیل نمایید.");
				}

				// کد ملی
				if (item.NationalCode == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه کد ملی را تکمیل نمایید.");
				}

				// نام پدر
				if (item.FatherName == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه نام پدر را تکمیل نمایید.");
				}

				// شماره شناسنامه
				if (item.IdCardNo == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه شماره شناسنامه را تکمیل نمایید.");
				}

				// تاریخ تولد
				if (item.BirthDate_Fa == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه تاریخ تولد را تکمیل نمایید.");
				}

				// جنسیت
				if (item.BaseInfo_GenderId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه جنسیت را تکمیل نمایید.");
				}

				// وضعیت تاهل
				if (item.BaseInfo_MaritalStatusId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه وضعیت تاهل را تکمیل نمایید.");
				}

			}

			// **************************************************

			return IsValid;
		}

		public async Task<bool> CheckFieldsValidation_EmpBankAccounts()
		{
			bool IsValid = true;
			
			// **************************************************
			// اطلاعات حساب بانکی
			foreach (var item in _Entity.HR_Base_BankAccount)
			{
				// سته بندی حساب های بانکی کارمند
				if (item.HR_Base_BankAcountCategoryId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه سته بندی حساب های بانکی کارمند را تکمیل نمایید.");
				}

				// نوع حساب بانکی
				if (item.BaseInfo_BankAccountTypeId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه نوع حساب بانکی را تکمیل نمایید.");
				}

				// بانک
				if (item.BaseInfo_BankId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه بانک را تکمیل نمایید.");
				}

				// شعبه بانک
				if (item.BaseInfo_BankBranchesId == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه شعبه بانک را تکمیل نمایید.");
				}

				// شماره حساب بانکی
				if (item.BankAccountNumber == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه شماره حساب بانکی را تکمیل نمایید.");
				}

				// شبا بانکی
				if (item.IBAN == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه شبا بانکی را تکمیل نمایید.");
				}

				// شماره کارت بانکی
				if (item.CartNo == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه شماره کارت بانکی را تکمیل نمایید.");
				}
			}
			// **************************************************

			return IsValid;
		}

		public async Task<bool> CheckFieldsValidation_EmpDocuments()
		{
			bool IsValid = true;

			// **************************************************
			// اطلاعات تماس
			foreach (var item in _Entity.HR_EMP_Documents)
			{
				// 
			}
			// **************************************************

			return IsValid;
		}

		public async Task<bool> CheckFieldsValidation_EmpContacts()
		{
			bool IsValid = true;

			// **************************************************
			// اطلاعات تماس
			foreach (var item in _Entity.HR_EMP_EmployeeDetails)
			{
				// 
			}
			// **************************************************

			return IsValid;
		}

		#endregion

		#region Grid EmployeeInfos
		public async Task<bool> GridHR_EMP_EmployeesId_382_editmodelsaving(object e)
		{
			return false;
		}
		public async Task GridHR_EMP_EmployeesId_382_afterrendermodal(Entity.HR_EMP_EmployeeInfos Item)
		{

		}

		#region EmployeeInfos_btn
		//// حذف دکمه های ذخیره، قبلی، بعدی در گرید جزئیات اطلاعات کارمند
		//await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonSave", "d-none");
		//await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonBefore", "d-none");
		//await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonNext", "d-none");
		#endregion

		#endregion Grid EmployeeInfos

		#region Grid EmployeeFamilies
		public async Task<bool> GridHR_EMP_EmployeesId_381_editmodelsaving(object e)
		{
			return false;
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

			//var CheckDataBankAccount = _Entity.HR_Base_BankAccount;
			//Console.WriteLine("#Log:: DataBankAccount ::>>" + _Entity.HR_Base_BankAccount.Count());

			//// بررسی اطلاعات حساب بانکی
			//IsCancelled = !await DataBankAccount(_Entity.HR_Base_BankAccount);

			return IsCancelled;
		}

		// ایونت After Render Modal: برای رندر کردن در حالت مودال گرید اطلاعات حساب بانکی در جدول کارمند است
		public async Task GridHR_EMP_EmployeesId_357_afterrendermodal(Entity.HR_Base_BankAccount Item)
		{
			#region BankAccount BTN
			//// حذف دکمه های ذخیره، قبلی، بعدی در گرید اطلاعات حساب بانکی
			//await JS.InvokeVoidAsync("ModalAddClass", "#HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonSaveAndNew", "d-none");
			//await JS.InvokeVoidAsync("ModalAddClass", "#HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonBefore", "d-none");
			//await JS.InvokeVoidAsync("ModalAddClass", "#HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonNext", "d-none");
			#endregion
		}

		#region متدهای وابسته به حساب بانکی
		/// <summary>
		///  تابعی برای بررسی تعداد ردیف‌های جزئیات
		/// </summary>
		/// <param name="Item"></param>
		/// <returns></returns>
		//public async Task<bool> DataBankAccount(ICollection<Entity.HR_Base_BankAccount> Item)
		//{
		//    // دکمه جدید در گرید حساب بانکی - HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonNew
		//    //await JS.InvokeVoidAsync("ModalAddClass", "#HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonNew", "d-none");


		//    if (Item != null && Item.Count > 1)
		//    {
		//        await _MSG.ShowError("شما مجاز به ثبت یک ردیف بیشتر نیستید!!");
		//        return false;
		//    }
		//    return true;
		//}


		/// <summary>
		/// نمایش و حذف دکمه جدید گرید حساب بانکی کاربر
		/// </summary>
		/// <returns></returns>
		//private async Task CheckGridDataAndToggleButton()
		//{
		//    StateHasChanged();

		//    Console.WriteLine("Log HR_Base_BankAccount:::" + _Entity.HR_Base_BankAccount.Count);

		//    //if (_Entity.HR_Base_BankAccount != null && _Entity.HR_Base_BankAccount.Any(x => !x.IsDelete))
		//    if (_Entity.HR_Base_BankAccount.Count >= 1)
		//    {
		//        // اگر داده وجود دارد، دکمه جدید را مخفی کن
		//        await JS.InvokeVoidAsync("AddClass", "#HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonNew", "d-none");
		//    }
		//    else
		//    {
		//        // اگر داده وجود ندارد، دکمه جدید را نمایش بده
		//        await JS.InvokeVoidAsync("RemoveClass", "#HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonNew", "d-none");
		//    }

		//    StateHasChanged();
		//}

		#endregion

		#endregion

		#region Grid EMP_Documents
		public async Task<bool> GridHR_EMP_EmployeesId_379_editmodelsaving(object e)
		{

			return false;
		}
		public async Task GridHR_EMP_EmployeesId_379_afterrendermodal(Entity.HR_EMP_Documents Item)
		{


		}
		#endregion

		#region Grid EMP_Contacts

		public async Task<bool> GridHR_EMP_EmployeesId_380_editmodelsaving(object e)
		{
			return false;
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

			await EmployeeDetails_ToggleGridButton(hasSavedRecord, isInModal: true);
		}


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
                    <picture><img src='https://file.workcv.ir/fa/api/v1/File/Get?FileID=6e5b6fb8-a5b2-490c-f83f-08dbea5b8061  ' width='96px' alt='لوگو سامانه بایا' /></picture>
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


			// برای لغو درخواست است.

			//var confirmation = await Confirm.ShowAsync(
			//	title: "",
			//	message1: html,
			//	confirmDialogOptions: options);

			//if (!confirmation)
			//{
			//	IsValid = false;
			//}
			//else
			//{
			//	// کاربر لغو کننده
			//	EmpBy = _User.UserID.ToString();
			//	EmpBy = _User.NAME + "" + _User.FAMILY;
			//	// در بخش Razor فیلد input hidden همین فیلد وجود دارد و از طریق آن داده به فیلد اصلی داده می شود.
			//	string Value = await JS.InvokeAsync<string>("eval", "document.getElementById('ConfirmEmpText')?.value || ''");
			//	EmpReason = Value;
			//}
		}

		#endregion

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
				await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeDetails_GridHR_EMP_EmployeesId_380ButtonSaveAndNew", "d-none");
				// دکمه قبلی - مخفی می‌شود چون فقط یک ردیف وجود دارد
				await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeDetails_GridHR_EMP_EmployeesId_380ButtonBefore", "d-none");
				// دکمه بعدی - مخفی می‌شود چون فقط یک ردیف وجود دارد
				await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeDetails_GridHR_EMP_EmployeesId_380ButtonNext", "d-none");
			}
		}

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

		#region 
		#endregion

		#region
		#endregion

		#endregion FunctionEvents

	}
}