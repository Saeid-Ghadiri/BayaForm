using Baya.Models.ORM;
using Baya.Models.Utility;
using Baya.Models.Utility.Entity;
using Baya.Models.Utility.Menu;
using Blazored.Toast.Services;
using Castle.DynamicLinqQueryBuilder;
using DateUtils;
using DevExpress.Blazor;
using Entity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using Sitko.Blazor.CKEditor;
using System;
using System.Globalization;
using System.Net;
using Utility;

namespace Forms.Forms
{
	public class Form_1002_CUBase : Form_1002_CUPeropeties
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
				// تٌست
				_MSG = new MSG(toastService);
			}
		}

		/// <summary>
		/// اعتبار سنجی فرم
		/// </summary>
		/// <returns></returns>
		public override async Task<bool> FormValidator()
		{
			bool IsValid = true;


			//foreach (var Item in List)
			//{
			//    Console.WriteLine("#Log FormValidator btn foreach :");
			//    IsValid = IsValid && await CheckFieldValidation(Item);
			//}

			return IsValid;
		}


		/// <summary>
		/// تابع قبل اجرا شدن ارسال داده
		/// </summary>
		/// <returns></returns>
		public override async Task<Result> BeforSubmit()
		{
			// تبدیل تاریخ شمسی اجرای حکم به میلادی
			PrepareExecutionDateForSubmit();

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
		}


		#region FunctionEvents

		string employeeId = "";



		public async Task<bool> CheckFieldValidation(Entity.HR_CVR_VerdictRecruiting Item)
		{
			bool IsValid = true;

			// var List = _Entity.HR_CVR_VerdictRecruiting.ToList();

			// - تاریخ اجرای حکم
			// اعتبارسنجی تاریخ اجرای حکم (شمسی)
			if (!string.IsNullOrWhiteSpace(_Entity.ExecutionDateSentence_Fa))
			{
				if (!PersianDateUtils.TryParseDateString(_Entity.ExecutionDateSentence_Fa, out _))
				{
					IsValid = false;
					await _MSG.ShowWarning("تاریخ اجرای حکم وارد شده معتبر نیست.");
				}
			}
			else
			{
				// اگر فیلد اجباری است:
				IsValid = false;
				await _MSG.ShowError("لطفاً تاریخ اجرای حکم را وارد کنید.");
			}


			return IsValid;
		}


		public async Task HR_CVR_PersonnelContractId_onitemselected(Entity.HR_CVR_PersonnelContract Selected)
		{
		}

		/// <summary>
		/// پرکردن فیلد گروه شغل بر اساس شغل کارمند
		/// شغل کارمند از View گرفته شده است
		/// </summary>
		/// <param name="Selected"></param>
		/// <returns></returns>
		public async Task HR_CVR_JobId_onitemselected(dynamic Selected)
		{
			// HR_CVR_JobId => شغل کارمند
			// HR_CVR_JobGroupId => گروه شغلی کارمند

			Entity.HR_CVR_JobGroup JobGroup = new();
			JobGroup.Id = Selected.JobGroupId;
			JobGroup.Title = Selected.JobGroupTitle;

			// Console.WriteLine("#Log 1");

			Ref_HR_CVR_JobGroupId.SetEntity(JobGroup);

			// Console.WriteLine(await Utility.JSON.ToJson(JobGroup));
			Ref_HR_CVR_JobGroupId.ItemSelected(JobGroup);

			// Console.WriteLine("#Log 2");

			await Task.Delay(100);
			Ref_HR_CVR_JobGroupId.LoadData();
		}

		/// <summary>
		/// تبدیل تاریخ شمسی اجرای حکم به میلادی و ذخیره در مدل
		/// </summary>
		private void PrepareExecutionDateForSubmit()
		{
			if (!string.IsNullOrWhiteSpace(_Entity.ExecutionDateSentence_Fa))
			{
				if (PersianDateUtils.TryParseDateString(_Entity.ExecutionDateSentence_Fa, out _))
				{
					// تبدیل به تاریخ میلادی (بدون زمان)
					_Entity.ExecutionDateSentence = PersianDateUtils.ToGregorian(_Entity.ExecutionDateSentence_Fa);
				}
				else
				{
					// در صورت نامعتبر بودن، مقدار میلادی را null کنید
					_Entity.ExecutionDateSentence = null;
				}
			}
			else
			{
				_Entity.ExecutionDateSentence = null;
			}
		}

		public async Task<bool> GridHR_CVR_VerdictRecruitingId_711_editmodelsaving(object e)
		{

			return false;
		}

		public async Task GridHR_CVR_VerdictRecruitingId_711_afterrendermodal(Entity.HR_CVR_RecruitmentRules Item)
		{
			string detailEmployeeId = employeeId;

			Console.WriteLine($"🔍 Fetching employee data for ID: {detailEmployeeId}");

			try
			{
				var emp_data = await EMP_Data.EmployeeData.EmployeeMasterDetail(detailEmployeeId, _User.UserID.ToString());

				var x = await Utility.JSON.ToJson(emp_data);
				Console.WriteLine("Log :: data_emp Data ::" + x);


				if (emp_data == null)
				{
					Console.WriteLine($"❌ Employee data NOT FOUND for ID: {detailEmployeeId}");
					return;
				}

				Console.WriteLine($"✅ Employee data loaded: {emp_data.FirstName} {emp_data.LastName}");

				// شماره بیمه در بخش 
				//Ref_HR_CVR_RecruitmentRules_InsuranceNumber.Value = _Entity.InsuranceNumber;

				Ref_HR_CVR_RecruitmentRules_EmployeeNo.Value = emp_data.EmployeeNo;
				Ref_HR_CVR_RecruitmentRules_FirstName.Value = emp_data.FirstName;
				Ref_HR_CVR_RecruitmentRules_LastName.Value = emp_data.LastName;
				Ref_HR_CVR_RecruitmentRules_FatherName.Value = emp_data.FatherName;
				Ref_HR_CVR_RecruitmentRules_NationalCode.Value = emp_data.NationalCode;
				Ref_HR_CVR_RecruitmentRules_IdCardNo.Value = emp_data.IdCardNo;
				Ref_HR_CVR_RecruitmentRules_BirthDate_Fa.Value = emp_data.BirthDate_Fa;
				Ref_HR_CVR_RecruitmentRules_BaseInfo_GenderId.Value = emp_data.BaseInfo_GenderTitle;
				Ref_HR_CVR_RecruitmentRules_BaseInfo_MaritalStatusId.Value = emp_data.BaseInfo_MaritalStatusTitle;
				//Ref_HR_CVR_RecruitmentRules_EmployeeAgeText.Value = emp_data.EmployeeAgeText;
				//Ref_HR_CVR_RecruitmentRules_CityOfIssue.Value = emp_data.CityOfIssueTitle;
				//Ref_HR_CVR_RecruitmentRules_CityOfBirth.Value = emp_data.CityOfBirthTitle;
				//Ref_HR_CVR_RecruitmentRules_EmploymentDateInGroup_Fa.Value = emp_data.EmploymentDateInGroup_Fa;
				////Ref_HR_CVR_RecruitmentRules_DailyEmploymentDateInGroup.Value = emp_data.DailyEmploymentDateInGroup; // در ویو نیامده است
				//Ref_HR_CVR_RecruitmentRules_EmploymentDate_Fa.Value = emp_data.EmploymentDate_Fa;
				////Ref_HR_CVR_RecruitmentRules_DailyEmploymentDate.Value = emp_data.DailyEmploymentDate; // در ویو نیامده است
				//Ref_HR_CVR_RecruitmentRules_EmploymentStartDate_Fa.Value = emp_data.EmploymentStartDate_Fa;

				//// اطلاعات حساب بانکی
				//// HR_Base_AcademicDegrees
				//Ref_HR_CVR_RecruitmentRules_BankAccountNumber.Value = empBnakAcc.BankAccountNumber;
				//Ref_HR_CVR_RecruitmentRules_IBAN.Value = empBnakAcc.IBAN;

				//// مدرک تحصیلی
				////Ref_HR_CVR_RecruitmentRules_HR_Base_AcademicDegreesId.Value = emp_data.HR_Base_AcademicDegreesTitle; // مدرک تحصیلی اصلا برای این ویو نیست

				StateHasChanged();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"💥 Exception in GridHR_CVR_VerdictRecruitingId_711_afterrendermodal: {ex}");
			}

		}

		//public async Task HR_EMP_EmployeesId_onitemselected(Entity.HR_EMP_Employees Selected)
		public async Task HR_EMP_EmployeesId_onitemselected(dynamic Selected)
		{

			Console.WriteLine("#Log +++++++++++++ 0001");
			Console.WriteLine("#Log::  Selected:Id :: " + Selected.Id);

			employeeId = Selected.Id.ToString();

			Console.WriteLine("#Log +++++++++++++ 0002");


			var x = await Utility.JSON.ToJson(Selected);
			Console.WriteLine("Log :: Selected Data ::" + x);
		}

		public async Task Grid_HR_EMP_EmployeesId_onitemselected(dynamic Selected, Entity.HR_CVR_RecruitmentRules Item)
		{
			var x = await Utility.JSON.ToJson(Selected);
			Console.WriteLine("Log :: Grid_Selected Data ::" + x);

			Item.HR_EMP_EmployeesId = Guid.Parse(employeeId);

			Ref_HR_CVR_RecruitmentRules_HR_EMP_EmployeesId.SetEntity(Item);

			//Console.WriteLine(await Utility.JSON.ToJson(SalaryCalculations));

			Ref_HR_CVR_RecruitmentRules_HR_EMP_EmployeesId.ItemSelected(Item);


			await Task.Delay(100);
			Ref_HR_CVR_RecruitmentRules_HR_EMP_EmployeesId.LoadData();
		}

		#endregion FunctionEvents

	}
}


// **************************************************


#region EMP_Data
namespace EMP_Data
{
	public static class EmployeeData
	{
		/// <summary>
		/// HR_EMP_Employees_EmployeeInfos
		/// این یک ویو دیتابیسی از جداول کارمند و جزئیات اطلاعات کارمند است
		/// </summary>
		/// <param name="id"></param>
		/// <param name="_UserId"></param>
		/// <returns></returns>
		public static async Task<Entity.HR_EMP_Employees_EmployeeInfos> EmployeeMasterDetail(string id, string _UserId)
		{
			if (string.IsNullOrEmpty(id))
			{
				Console.WriteLine("❌ Employee ID is null or empty in EmployeeMasterDetail1");
				return null;
			}

			var TablePost = new Table
			{
				Name = "HR_EMP_Employees_EmployeeInfos",
				Column = new List<Coulmn>
				{
					new Coulmn { Name = "Id", NameAs = "Id" }, // شناسه
					new Coulmn { Name = "EmployeeNo", NameAs = "EmployeeNo" }, // کد کارمندی
					new Coulmn { Name = "FirstName", NameAs = "FirstName" }, // نام
					new Coulmn { Name = "LastName", NameAs = "LastName" }, // نام خانوادگی
					new Coulmn { Name = "FatherName", NameAs = "FatherName" }, // نام پدر
					new Coulmn { Name = "NationalCode", NameAs = "NationalCode" }, // کد ملی
					new Coulmn { Name = "IdCardNo", NameAs = "IdCardNo" }, // شماره شناسنامه
					new Coulmn { Name = "BirthDate_Fa", NameAs = "BirthDate_Fa" }, // تاریخ تولد
					new Coulmn { Name = "BaseInfo_GenderId", NameAs = "BaseInfo_GenderId" }, // شناسه جنسیت 
					new Coulmn { Name = "BaseInfo_GenderTitle", NameAs = "BaseInfo_GenderTitle" }, // شناسه جنسیت 
					new Coulmn { Name = "BaseInfo_MaritalStatusId", NameAs = "BaseInfo_MaritalStatusId" }, // وضعیت تاهل
					new Coulmn { Name = "BaseInfo_MaritalStatusTitle", NameAs = "BaseInfo_MaritalStatusTitle" }, // وضعیت تاهل
					//new Coulmn { Name = "EmploymentDateInGroup_Fa", NameAs = "EmploymentDateInGroup_Fa" }, // تاریخ استخدام در گروه
					//new Coulmn { Name = "EmploymentDate_Fa", NameAs = "EmploymentDate_Fa" }, // تاریخ استخدام
					//new Coulmn { Name = "EmploymentStartDate_Fa", NameAs = "EmploymentStartDate_Fa" }, // تاریخ آخرین تسویه حساب
					////new Coulmn { Name = "DailyEmploymentDate", NameAs = "DailyEmploymentDate" }, // تاریخ استخدام (به روز)
					////new Coulmn { Name = "DailyEmploymentDateInGroup", NameAs = "DailyEmploymentDateInGroup" }, // تاریخ استخدام در گروه (به روز)
					//new Coulmn { Name = "EmployeeAgeText", NameAs = "EmployeeAgeText" }, // سن کارمند به صورت متن
					//new Coulmn { Name = "CityOfIssue", NameAs = "CityOfIssue" }, // شناسه شهر محل صدور
					//new Coulmn { Name = "CityOfIssueTitle", NameAs = "CityOfIssueTitle" }, // شهر محل صدور
					//new Coulmn { Name = "CityOfBirth", NameAs = "CityOfBirth" }, // شناسه شهر محل تولد
					//new Coulmn { Name = "CityOfBirthTitle", NameAs = "CityOfBirthTitle" }, // شهر محل تولد
				}
			};

			var NewQuery = new QueryBuilderFilterRule { Condition = "AND" };
			NewQuery.Rules = new List<QueryBuilderFilterRule>
			{
				new QueryBuilderFilterRule
				{
					Id = "Id",
					Field = "Id",
					Input = "text",
					Operator = "equal",
					Type = "string",
					Value = new string[] { id }
				}
			};

			var Model = await ApiServer.External.Services.Data.Get(TablePost, NewQuery, "HR_EMP_Employees_EmployeeInfos", _UserId);

			if (Model?.Status != HttpStatusCode.OK)
			{
				Console.WriteLine($"❌ API error {Model?.Status} for employee ID: {id}");
				return null;
			}

			if (string.IsNullOrEmpty(Model.Content?.ToString()))
			{
				Console.WriteLine($"❌ API returned empty content for ID: {id}");
				return null;
			}

			try
			{
				var vw_emp_data = await JSON.ToObject<Entity.HR_EMP_Employees_EmployeeInfos>(Model.Content.ToString());
				return vw_emp_data;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"💥 JSON Deserialize error for ID {id}: {ex.Message}");
				return null;
			}
		}
	}
}
#endregion EMP_Data