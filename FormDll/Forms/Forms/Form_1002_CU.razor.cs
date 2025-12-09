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

		public async Task HR_EMP_EmployeesId_onitemselected(Entity.HR_EMP_Employees Selected)
		{
			employeeId = _Entity.HR_EMP_EmployeesId.Value.ToString();
		}

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
			//var data_emp = await EMP_Data.EmployeeData.EmployeeMasterDetail1(Item.HR_EMP_EmployeesId.ToString(), _User.UserID.ToString());

			//Item.FirstName = data_emp.FirstName;
			//Item.LastName = data_emp.LastName;
			//Item.FatherName = data_emp.FatherName;
			//Item.NationalCode = data_emp.NationalCode;


			// بررسی null بودن Item یا HR_EMP_EmployeesId
			if (Item?.HR_EMP_EmployeesId == null)
			{
				Console.WriteLine("⚠️ Item or HR_EMP_EmployeesId is null. Skipping employee data load.");
				return;
			}

			string detailEmployeeId = employeeId;

			Console.WriteLine($"🔍 Fetching employee data for ID: {employeeId}");

			try
			{
				var data_emp = await EMP_Data.EmployeeData.EmployeeMasterDetail1(employeeId, _User.UserID.ToString());

				if (data_emp == null)
				{
					Console.WriteLine($"❌ Employee data NOT FOUND for ID: {employeeId}");
					return;
				}

				Console.WriteLine($"✅ Employee data loaded: {data_emp.FirstName} {data_emp.LastName}");

				Item.FirstName = data_emp.FirstName;
				Item.LastName = data_emp.LastName;
				Item.FatherName = data_emp.FatherName;
				Item.NationalCode = data_emp.NationalCode;

				StateHasChanged();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"💥 Exception in GridHR_CVR_VerdictRecruitingId_711_afterrendermodal: {ex}");
			}

		}

		#endregion FunctionEvents

	}
}




// **********************************************


#region EMP_Data
namespace EMP_Data
{
	public static class EmployeeData
	{
		//public static async Task<Entity.HR_EMP_Employees_EmployeeInfos> EmployeeMasterDetail(string id, string _UserId)
		//{
		//	var TablePost = new Table();

		//	TablePost.Name = "HR_EMP_Employees_EmployeeInfos";

		//	TablePost.Column = new List<Coulmn>
		//	{
		//		new Coulmn {Name="Id" , NameAs= "Id"},
		//		new Coulmn {Name ="FirstName",NameAs="FirstName" },
		//		new Coulmn {Name ="LastName",NameAs="LastName" },
		//		new Coulmn {Name ="FatherName",NameAs="FatherName" },
		//		new Coulmn {Name ="NationalCode",NameAs="NationalCode" },
		//              // new Coulmn {Name ="UserName",NameAs="UserName" },
		//              // new Coulmn {Name ="UserName",NameAs="UserName" },
		//              // new Coulmn {Name ="UserName",NameAs="UserName" },
		//              // new Coulmn {Name ="UserName",NameAs="UserName" },
		//              // new Coulmn {Name ="UserName",NameAs="UserName" }
		//          };

		//	// ساخت لیست قوانین
		//	var NewQuery = new QueryBuilderFilterRule()
		//	{ Condition = "AND" };
		//	NewQuery.Rules = new List<QueryBuilderFilterRule>
		//	{
		//		new QueryBuilderFilterRule()
		//		{
		//			Field = "Id",
		//			Id = "Id",
		//			Input = "text",
		//			Operator = "equal",
		//			Type =  "string",
		//			Value = new string[]{ id.ToString() },
		//		}
		//	};

		//	bool IsOk = false;

		//	var Model = await ApiServer.External.Services.Data.Get(TablePost, NewQuery, "HR_EMP_Employees_EmployeeInfos", _UserId);

		//	if (Model?.Status == HttpStatusCode.OK)
		//	{
		//		Entity.HR_EMP_Employees_EmployeeInfos vw_emp_data = await JSON.ToObject<Entity.HR_EMP_Employees_EmployeeInfos>(Model.Content.ToString());

		//		return vw_emp_data;
		//	}
		//	return null;
		//}

		public static async Task<Entity.View_HR_EMP_EmployeeInfos> EmployeeMasterDetail1(string id, string _UserId)
		{
			if (string.IsNullOrEmpty(id))
			{
				Console.WriteLine("❌ Employee ID is null or empty in EmployeeMasterDetail1");
				return null;
			}

			var TablePost = new Table
			{
				Name = "View_HR_EMP_EmployeeInfos",
				Column = new List<Coulmn>
				{
					new Coulmn { Name = "Id", NameAs = "Id" },
					new Coulmn { Name = "FirstName", NameAs = "FirstName" },
					new Coulmn { Name = "LastName", NameAs = "LastName" },
					new Coulmn { Name = "FatherName", NameAs = "FatherName" },
					new Coulmn { Name = "NationalCode", NameAs = "NationalCode" },
					// اگر نیاز داشتید HR_EMP_EmployeesId را هم بگیرید:
					// new Coulmn { Name = "HR_EMP_EmployeesId", NameAs = "HR_EMP_EmployeesId" }
				}
			};

			var NewQuery = new QueryBuilderFilterRule { Condition = "AND" };
			NewQuery.Rules = new List<QueryBuilderFilterRule>
			{
				new QueryBuilderFilterRule
				{
					Field = "HR_EMP_EmployeesId", 
					Id = "HR_EMP_EmployeesId",
					Input = "text",
					Operator = "equal",
					Type = "string",
					Value = new string[] { id } // این id باید مقدار HR_EMP_EmployeesId باشد
				}
			};

			var Model = await ApiServer.External.Services.Data.Get(TablePost, NewQuery, "View_HR_EMP_EmployeeInfos", _UserId);

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
				var vw_emp_data = await JSON.ToObject<Entity.View_HR_EMP_EmployeeInfos>(Model.Content.ToString());
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