using ApiServer.External.Services;
using Baya.Models.HR.Employee;
using Baya.Models.ORM;
using Baya.Models.Utility;
using Baya.Models.Utility.Entity;
using Baya.Models.Utility.Menu;
using BlazorBootstrap;
using Blazored.Toast.Services;
using Castle.DynamicLinqQueryBuilder;
using DateUtils;
using DevExpress.Blazor;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Entity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using Sitko.Blazor.CKEditor;
using ContractTimeModel;
using System;
using System.Globalization;
using System.Net;
using Utility;
using VerdictDataModel;

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

			//// محاسبه مجدد جمع‌ها برای تمام ردیف‌های گرید
			//if (_Entity.HR_CVR_RecruitmentRules != null)
			//{
			//	foreach (var item in _Entity.HR_CVR_RecruitmentRules)
			//	{
			//		CalculateTotalSalaryBenefits(item);
			//	}
			//}

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
			// شناسه کارمند
			await GetEmpId(_Entity.HR_EMP_EmployeesId.ToString());

			// فراخوانی - آخرین حکم و قرارداد
			await LastEmpVerdictAndContract(_Entity.HR_EMP_EmployeesId.ToString());

			//// محاسبه مجدد جمع‌ها بعد از بارگذاری داده
			//if (_Entity.HR_CVR_RecruitmentRules != null)
			//{
			//	foreach (var item in _Entity.HR_CVR_RecruitmentRules)
			//	{
			//		CalculateTotalSalaryBenefits(item);
			//	}
			//}

			StateHasChanged();
		}

		#region FunctionEvents

		string employeeId = "";

		#region Validation
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
		#endregion

		#region MasterData
		/// <summary>
		/// 
		/// </summary>
		/// <param name="Selected"></param>
		/// <returns></returns>
		public async Task HR_EMP_EmployeesId_onitemselected(dynamic Selected)
		{
			var x = await Utility.JSON.ToJson(Selected);
			Console.WriteLine("Log :: Selected Data ::" + x);

			// ذخیره شناسه کارمند (برای استفاده در گرید‌ها)
			await GetEmpId(Selected.Id.ToString());

			// فراخوانی - آخرین حکم و قرارداد
			await LastEmpVerdictAndContract(Selected.Id.ToString());

			StateHasChanged(); // تضمین رندر مجدد UI

			// صدا زدن SP اول
			//await SP_Verdict();


			var EveryPostVerdict1 = await EveryPostVerdict(Selected.Id.ToString());

		}

		/// <summary>
		/// ست کردن شناسه کارمند در employeeId
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		private async Task GetEmpId(string Id)
		{
			Console.WriteLine("#Log +++++++++++++ 0001");
			Console.WriteLine("#Log::  Selected:Id :: " + Id);

			employeeId = Id;

			Console.WriteLine("#Log +++++++++++++ 0002");
			Console.WriteLine("#Log employeeId ::" + employeeId);
		}

		#region LastEmpVerdictAndContract
		private async Task LastEmpVerdictAndContract(string Id)
		{
			#region LastEmpVerdict
			// ✅ اضافه شده: دریافت آخرین حکم فعال کارمند
			//var lastVerdict = await EMP_Data.EmployeeData.LastVerdictEmp(Selected.Id.ToString(), _User.UserID.ToString());

			//if (lastVerdict != null)
			//{
			//	// ست کردن شناسه آخرین حکم در فیلد مربوطه
			//	_Entity.HR_CVR_VerdictRecruitingId = lastVerdict.Id;

			//	// اگر Dropdown مربوطه رفرش نمی‌شود، ممکن است نیاز باشد Ref را دستی به‌روز کنید:
			//	// Ref_HR_CVR_VerdictRecruitingId?.Reload(); // (اختیاری - بسته به نحوه پیاده‌سازی Dropdown)
			//}
			//else
			//{
			//	// اگر حکم فعالی وجود نداشت، فیلد را خالی کن
			//	_Entity.HR_CVR_VerdictRecruitingId = null;
			//}

			// ToDo: باید برای این فیلتر OrderBy نسبت به آخرین حکم اضافه گردد
			QueryBuilderFilterRule filter_LastEmpVerdict = new QueryBuilderFilterRule()
			{
				Condition = "AND",
				Rules = new List<QueryBuilderFilterRule>()
				{
					new QueryBuilderFilterRule()
					{
						//Id = "Id",
						Field = "HR_EMP_EmployeesId", // شناسه کارمند
						Type = "string",
						Input = "text",
						Operator = "equal",
						Value = new string[] { employeeId }
					}
				}
			};

			await Task.Delay(100);

			Console.WriteLine("#Log :: LastEmpVerdict ::" + filter_LastEmpVerdict);

			// LoadData رشته می‌خواهد
			await Ref_HR_CVR_VerdictRecruitingId.Search(filter_LastEmpVerdict);
			#endregion

			#region LastEmpContract
			// ToDo: باید برای این فیلتر نسبت به ویو قرارداد و تمدید قرارداد آخرین رکورد را نمایش دهد
			// درست: مدل اصلی QueryBuilderFilter
			QueryBuilderFilterRule filter_LastEmpContract = new QueryBuilderFilterRule()
			{
				Condition = "AND",
				Rules = new List<QueryBuilderFilterRule>()
				{
					new QueryBuilderFilterRule()
					{
						//Id = "Id",
						Field = "HR_EMP_EmployeesId", // شناسه کارمند
						Type = "string",
						Input = "text",
						Operator = "equal",
						Value = new string[] { employeeId }
					}
				}
			};

			await Task.Delay(100);

			Console.WriteLine("#Log :: LastEmpContract ::" + filter_LastEmpContract);

			// LoadData رشته می‌خواهد
			await Ref_HR_CVR_PersonnelContractId.Search(filter_LastEmpContract);
			#endregion
		}
		#endregion

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
		/// نمایش آخرین قرارداد یا تمدیدقرارداد کارمند که در بخش اصلی فرم قرار دارد
		/// </summary>
		/// <param name="Selected"></param>
		/// <returns></returns>
		public async Task HR_CVR_PersonnelContractId_onitemselected(Entity.HR_CVR_PersonnelContract Selected)
		{
		}

		/// <summary>
		/// نمایش آخرین حکم کارمند که در بخش اصلی فرم قرار دارد
		/// </summary>
		/// <param name="Selected"></param>
		/// <returns></returns>
		public async Task HR_CVR_VerdictRecruitingId_onitemselected(dynamic Selected)
		{
		}


		#endregion MasterData

		#region SP_Verdict EmpId & Json

		VerdictDataModel.EmployeeInfo EmpInfo = new();

		public class SalaryPreviewItem
		{
			public string Title { get; set; }
			public decimal? Value { get; set; }
		}

		#region SP_Verdict EmpId
		/// <summary>
		/// دکمه استورد پروسیجر
		/// </summary>
		/// <param name="Selected"></param>
		/// <returns></returns>
		public async Task SP_Verdict_onclick(MouseEventArgs Selected)
		{
			await SP_Verdict();
		}

		public async Task SP_Verdict()
		{
			Console.WriteLine("### 🟡 شروع فراخوانی حکم کارگزینی ###");

			if (_Entity?.HR_EMP_EmployeesId == null)
			{
				await _MSG.ShowWarning("لطفاً ابتدا کارمند را انتخاب کنید.");
				Console.WriteLine("❌ HR_EMP_EmployeesId null است");
				return;
			}

			// API حکم کارگزینی که در فرم 936 ایجاد شده است.
			var R = await BayaApi.PersonnelVerdictInfos(
				ShomaranApiMode.Polfilm,
				new EmployeeModel.EmpId
				{
					EmployeesId = _Entity.HR_EMP_EmployeesId.Value
				}
			);

			// بررسی نال بودن خروجی API
			if (R == null)
			{
				await _MSG.ShowError("خروجی وب سرویس null است");
				Console.WriteLine("❌ خطا: R == null");
				return;
			}

			var jsonResponse = R.Content.ToString();
			Console.WriteLine("### 🟡 jsonResponse کامل ###");
			Console.WriteLine("#Log :: jsonResponse (R.Content.ToString()) :: " + jsonResponse);

			if (!jsonResponse.TrimStart().StartsWith("{"))
			{
				await _MSG.ShowError("خروجی وب سرویس JSON نیست:\n" + jsonResponse);
				Console.WriteLine("❌ خطا: پاسخ JSON نیست");
				return;
			}

			try
			{
				var response = Newtonsoft.Json.JsonConvert.DeserializeObject<VerdictDataModel.RootResponse>(jsonResponse);

				if (response?.DataSets == null || response.DataSets.Count == 0)
				{
					await _MSG.ShowError("DataSets خالی است");
					return;
				}

				var employee = response.DataSets[0][0];

				EmpInfo = employee;


				#region پر کردن فیلد های محاسبات حکم  در حالت جدید

				// برای پر کردن فیلدهای گرید محاسبات حقوق حکم از این بخش استفاده می کنیم
				if (_Entity?.HR_CVR_RecruitmentRules != null && EmpInfo?.RankSalaryNew != null)
				{
					foreach (var item in _Entity.HR_CVR_RecruitmentRules)
					{
						// مزد شغل
						item.JobSalaryRankNew = EmpInfo.JobSalaryRankNew;
						Console.WriteLine($":: Log Set JobSalaryRankNew for item {item._Id} :: {item.JobSalaryRankNew}");

						// مزد رتبه
						item.RankSalaryNew = EmpInfo.RankSalaryNew;
						Console.WriteLine($":: Log Set RankSalaryNew for item {item._Id} :: {item.JobSalaryRankNew}");

						// مزد سنوات
						item.SalaryHistoryNew = EmpInfo.SalaryHistoryNew;
						Console.WriteLine($":: Log Set SalaryHistoryNew for item {item._Id} :: {item.SalaryHistoryNew}");

						// حق پست
						item.RightGuardianshipNew = EmpInfo.RightGuardianshipNew;
						Console.WriteLine($":: Log Set RightGuardianshipNew for item {item._Id} :: {item.RightGuardianshipNew}");

						// مزایای مانداگاری در پست
						item.CoefficientDurabilityPostNew = EmpInfo.CoefficientDurabilityPostNew;
						Console.WriteLine($":: Log Set CoefficientDurabilityPostNew for item {item._Id} :: {item.CoefficientDurabilityPostNew}");

						// شرایط نامساعد محیط کار
						item.CoefficientDifficultAndHarmfulJobsNew = EmpInfo.CoefficientDifficultAndHarmfulJobsNew;
						Console.WriteLine($":: Log Set CoefficientDifficultAndHarmfulJobsNew for item {item._Id} :: {item.CoefficientDifficultAndHarmfulJobsNew}");

						// جمع کل مزد مبنا جدید
						item.TotalDailyBaseWageNew = EmpInfo.TotalDailyBaseWageNew;
						Console.WriteLine($":: Log Set TotalDailyBaseWageNew for item {item._Id} :: {item.TotalDailyBaseWageNew}");
						//Ref_HR_CVR_RecruitmentRules_TotalDailyBaseWageNew.SetDisabled(true);

						// جمع کل مزد مبنا
						item.TotalDailyBaseWage = EmpInfo.TotalDailyBaseWage;
						Console.WriteLine($":: Log Set TotalDailyBaseWage for item {item._Id} :: {item.TotalDailyBaseWage}");

						// تفاوت تطبیق روزانه
						item.DailyAdjustmentDifferenceNew = EmpInfo.DailyAdjustmentDifferenceNew;
						Console.WriteLine($":: Log Set DailyAdjustmentDifferenceNew for item {item._Id} :: {item.DailyAdjustmentDifferenceNew}");

						// حق جذب
						item.RecruitmentAllowanceNew = EmpInfo.RecruitmentAllowanceNew;
						Console.WriteLine($":: Log Set RecruitmentAllowanceNew for item {item._Id} :: {item.RecruitmentAllowanceNew}");

						// کمک هزینه مسکن
						item.MinistryLabourRightHousingNew = EmpInfo.MinistryLabourRightHousingNew;
						Console.WriteLine($":: Log Set MinistryLabourRightHousingNew for item {item._Id} :: {item.MinistryLabourRightHousingNew}");
						//Ref_HR_CVR_RecruitmentRules_MinistryLabourRightHousingNew.SetDisabled(true);

						// حق خوار و بار
						item.MinistryLaborRightFoodNew = EmpInfo.MinistryLaborRightFoodNew;
						Console.WriteLine($":: Log Set MinistryLaborRightFoodNew for item {item._Id} :: {item.MinistryLaborRightFoodNew}");
						//Ref_HR_CVR_RecruitmentRules_MinistryLaborRightFoodNew.SetDisabled(true);

						// حق اولاد
						item.ChildrensRightsMinistryLaborNew = EmpInfo.ChildrensRightsMinistryLaborNew;
						Console.WriteLine($":: Log Set ChildrensRightsMinistryLaborNew for item {item._Id} :: {item.ChildrensRightsMinistryLaborNew}");
						//Ref_HR_CVR_RecruitmentRules_ChildrensRightsMinistryLaborNew.SetDisabled(true);

						// مزایای رفاهی انگیزه ای ماهانه
						item.WelfareMotivationalBenefitsNew = EmpInfo.WelfareMotivationalBenefitsNew;
						Console.WriteLine($":: Log Set WelfareMotivationalBenefitsNew for item {item._Id} :: {item.WelfareMotivationalBenefitsNew}");
						//Ref_HR_CVR_RecruitmentRules_WelfareMotivationalBenefitsNew.SetDisabled(true);

						// حق تاهل
						item.RightMarryMinistryLaborNew = EmpInfo.RightMarryMinistryLaborNew;
						Console.WriteLine($":: Log Set RightMarryMinistryLaborNew for item {item._Id} :: {item.RightMarryMinistryLaborNew}");
						//Ref_HR_CVR_RecruitmentRules_RightMarryMinistryLaborNew.SetDisabled(true);

						// سایر مزایا
						item.OtherBenefitsNew = EmpInfo.OtherBenefitsNew;
						Console.WriteLine($":: Log Set OtherBenefitsNew for item {item._Id} :: {item.OtherBenefitsNew}");

						// جمع کل دستمزد مزایایی قانونی و جمع مزد مبنا
						item.TotalMonthlySalaryBenefitsNew = EmpInfo.TotalMonthlySalaryBenefitsNew;
						Console.WriteLine($":: Log Set TotalMonthlySalaryBenefitsNew for item {item._Id} :: {item.TotalMonthlySalaryBenefitsNew}");
						//Ref_HR_CVR_RecruitmentRules_TotalMonthlySalaryBenefitsNew.SetDisabled(true);


						item.TotalMonthlySalaryBenefits = EmpInfo.TotalMonthlySalaryBenefits;
						Console.WriteLine($":: Log Set TotalMonthlySalaryBenefits for item {item._Id} :: {item.TotalMonthlySalaryBenefits}");

						// **********
						// اطلاعات بخش قسمت های سازمانی
						item.HR_ORG_SectionsId = Guid.Parse(EmpInfo.SectionId.ToString());
						Console.WriteLine($":: HR_ORG_SectionsId for item {item._Id} :: {item.HR_ORG_SectionsId}");
						// **********

						// **********
						// اطلاعات بخش پست های سازمانی
						item.HR_ORG_PostsId = Guid.Parse(EmpInfo.PostsId.ToString());
						Console.WriteLine($":: HR_ORG_PostsId for item {item._Id} :: {item.HR_ORG_PostsId}");
						// **********

						// **********
						// عنوان شغلی
						item.HR_CVR_JobId = Guid.Parse(EmpInfo.HR_CVR_JobId.ToString());
						Console.WriteLine($":: HR_CVR_JobId for item {item._Id} :: {item.HR_CVR_JobId}");
						// **********

						// **********
						// گروه شغلی
						item.HR_CVR_JobGroupId = Guid.Parse(EmpInfo.HR_CVR_JobGroupId.ToString());
						Console.WriteLine($":: HR_CVR_JobGroupId for item {item._Id} :: {item.HR_CVR_JobGroupId}");
						// **********

						// **********
						// ضریب ريالی - عدد ثابت تعیین شده وزارت کار
						//item.HR_CVR_LaborCouncilFixedValuesId = Guid.Parse(EmpInfo.HR_CVR_LaborCouncilFixedValuesId.ToString());
						//Console.WriteLine($":: HR_CVR_LaborCouncilFixedValuesId for item {item._Id} :: {item.HR_CVR_LaborCouncilFixedValuesId}");
						// **********

						// **********
						//item.HR_CVR_AnnualBaseWageIncreaseRatesId = Guid.Parse(EmpInfo.HR_CVR_AnnualBaseWageIncreaseRatesId.ToString());
						//Console.WriteLine($":: HR_CVR_AnnualBaseWageIncreaseRatesId for item {item._Id} :: {item.HR_CVR_AnnualBaseWageIncreaseRatesId}");
						// **********

					}
				}

				#endregion

				Console.WriteLine(":: Log EmpInfo :: " + await JSON.ToJson(EmpInfo));

				// --- نمایش دیالوگ ---
				var options = new BlazorBootstrap.ConfirmDialogOptions
				{
					YesButtonText = "بازگشت به حکم کارگزینی",
					YesButtonColor = ButtonColor.Info,
					NoButtonText = "",
				};

				var Export = await JSON.ToJson(_Entity.HR_CVR_RecruitmentRules);

				var rule = _Entity.HR_CVR_RecruitmentRules.FirstOrDefault();

				var previewList = new List<SalaryPreviewItem>
				{
					new() { Title = "مزد شغل", Value = rule?.JobSalaryRankNew },
					new() { Title = "مزد رتبه", Value = rule?.RankSalaryNew },
					new() { Title = "مزد سنوات", Value = rule?.SalaryHistoryNew },
					new() { Title = "حق پست", Value = rule?.RightGuardianshipNew },
					new() { Title = "ماندگاری در پست", Value = rule?.CoefficientDurabilityPostNew },
					new() { Title = "شرایط نامساعد محیط کار", Value = rule?.CoefficientDifficultAndHarmfulJobsNew },
					new() { Title = "جمع کل مزد مبنا", Value = rule?.TotalDailyBaseWageNew },
					new() { Title = "حق جذب", Value = rule?.RecruitmentAllowanceNew },
					new() { Title = "حق مسکن", Value = rule?.MinistryLabourRightHousingNew },
					new() { Title = "حق خوار و بار", Value = rule?.MinistryLaborRightFoodNew },
					new() { Title = "حق اولاد", Value = rule?.ChildrensRightsMinistryLaborNew },
					new() { Title = "حق تاهل", Value = rule?.RightMarryMinistryLaborNew },
					new() { Title = "مزایای رفاهی انگیزشی", Value = rule?.WelfareMotivationalBenefitsNew },
					//new() { Title = "جمع کل مزد مبنا و مزایای قانونی", Value = rule?.TotalMonthlySalaryBenefitsNew },
				};

				var rows = string.Join("", previewList.Select(x => $@"
				<tr>
					<td style='padding:6px 10px;'>{x.Title}</td>
					<td style='padding:6px 10px; text-align:right; font-weight:bold'>
						{x.Value?.ToString("N0")}
					</td>
				</tr>
				"));

				decimal totalSum = rule?.TotalMonthlySalaryBenefitsNew ?? 0.0m;
				//decimal totalSum1 = previewList.Sum(x => x.Value ?? 0m);

				var totalRow = $@"
				<tr style='background:#fff3cd; font-weight:bold;'>
					<td style='padding:8px'>جمع کل</td>
					<td style='padding:8px; text-align:right; color:#b02a37'>
						{totalSum.ToString("N0")}
					</td>
				</tr>";

				string htmlString = $@"
				<div style='direction: rtl;'>
					<table style='width:100%; border-collapse: collapse;' border='1'>
						<thead style='background:#f5f5f5'>
							<tr>
								<th style='padding:8px'>عنوان</th>
								<th style='padding:8px'>مبلغ (ریال)</th>
							</tr>
						</thead>
						<tbody>
							{rows}
							{totalRow}
						</tbody>
					</table>
				</div>";

				await Confirm.ShowAsync("پیش‌نمایش محاسبات حکم", htmlString, options);

				Console.WriteLine("✅ ### اتمام عملیات با موفقیت ###");
			}
			catch (Exception ex)
			{
				Console.WriteLine("❌ خطا در پردازش کلی: " + ex.Message);
				await _MSG.ShowError("خطا در پردازش SP: " + ex.Message);
			}

			// فرواخوانی داده های مرتبط  با شناسه کارمند
			await GetEmpDataGridOnSP();

			StateHasChanged();

		}
		#endregion

		#region SP_Verdict Json
		public async Task SP_Verdict_Json_onclick(MouseEventArgs Selected)
		{
			await SP_Verdict_Json();
		}

		public async Task SP_Verdict_Json()
		{

			var VerdictDTO = new VerdictDataModel.VerdictRequest()
			{
				// شناسه کارمند
				EmployeesId = Guid.Parse(employeeId),

				// مزد شغل
				JobSalaryRank = EmpInfo.JobSalaryRank,

				// مزد رتبه
				RankSalary = EmpInfo.RankSalary,

				// مزد سنوات
				SalaryHistory = EmpInfo.SalaryHistory,

				// حق پست
				RightGuardianship = EmpInfo.RightGuardianship,

				// مزایای مانداگاری در پست
				CoefficientDurabilityPost = EmpInfo.CoefficientDurabilityPost,

				// شرایط نامساعد محیط کار
				CoefficientDifficultAndHarmfulJobs = EmpInfo.CoefficientDifficultAndHarmfulJobs,

				// جمع کل مزد مبنا
				TotalDailyBaseWage = EmpInfo.TotalDailyBaseWage,

				// تفاوت تطبیق روزانه
				DailyAdjustmentDifference = EmpInfo.DailyAdjustmentDifference,

				// حق جذب
				RecruitmentAllowance = EmpInfo.RecruitmentAllowanceNew,

				// کمک هزینه مسکن
				MinistryLabourRightHousing = EmpInfo.MinistryLabourRightHousing,

				// حق خوار و بار
				MinistryLaborRightFood = EmpInfo.MinistryLaborRightFood,

				// حق اولاد
				ChildrensRightsMinistryLabor = EmpInfo.ChildrensRightsMinistryLabor,

				// مزایای رفاهی انگیزه ای ماهانه
				WelfareMotivationalBenefits = EmpInfo.WelfareMotivationalBenefits,

				// حق تاهل
				RightMarryMinistryLabor = EmpInfo.RightMarryMinistryLabor,

				// این فیلد به استورد باید اضافه گردد
				// سایر مزایا
				OtherBenefits = EmpInfo.OtherBenefits,

				// جمع کل دستمزد مزایایی قانونی و جمع مزد مبنا
				TotalMonthlySalaryBenefits = EmpInfo.TotalMonthlySalaryBenefits,

				// ************************************************

				// نوع حکم کارمند
				HR_CVR_TypesRulingsId = EmpInfo.HR_CVR_TypesRulingsId,

				// نوع بیمه کارمند
				HR_Base_InsuranceTypesId = EmpInfo.HR_Base_InsuranceTypesId,

				// نوع پرداخت آکورد
				TypeBonusPayment = EmpInfo.TypeBonusPayment,

				// وضعیت حکم
				HR_StatusVerdictRecruitingId = EmpInfo.HR_StatusVerdictRecruitingId,

				// عنوان قسمت های سازمانی در جدول پست های حکم
				HR_ORG_SectionsId = EmpInfo.HR_ORG_SectionsId,

				// نوع قسمت در جدول پست های حکم
				SectionsType = EmpInfo.SectionsType,

				// عنوان پست سازمانی در جدول پست های حکم
				HR_ORG_PostsId = EmpInfo.HR_ORG_PostsId,

				// نوع پست در جدول پست های حکم
				PostType = EmpInfo.PostType,

				// تاریخ اجرای حکم - شمسی
				ExecutionDateSentence_Fa = EmpInfo.ExecutionDateSentence_Fa,

				// تاریخ اجرای حکم - میلادی
				ExecutionDateSentence = EmpInfo.ExecutionDateSentence,

				// *********************************************************
				// فیلدهای جدید

				// مزد شغل گروه جدید
				JobSalaryRankNew = EmpInfo.JobSalaryRankNew,

				// مزد رتبه جدید
				RankSalaryNew = EmpInfo.RankSalaryNew,

				// مزد سنوات جدید
				SalaryHistoryNew = EmpInfo.SalaryHistoryNew,

				// حق سرپرستی (پست) جدید
				RightGuardianshipNew = EmpInfo.RightGuardianshipNew,

				// مزایای ماندگاری پست جدید
				CoefficientDurabilityPostNew = EmpInfo.CoefficientDurabilityPostNew,

				// شرایط نامساعد محیط کار جدید
				CoefficientDifficultAndHarmfulJobsNew = EmpInfo.CoefficientDifficultAndHarmfulJobsNew,

				// جمع مزد مبنای روزانه جدید
				TotalDailyBaseWageNew = EmpInfo.TotalDailyBaseWageNew,

				// تفاوت تطبیق روزانه جدید
				DailyAdjustmentDifferenceNew = EmpInfo.DailyAdjustmentDifferenceNew,

				// حق جذب جدید
				RecruitmentAllowanceNew = EmpInfo.RecruitmentAllowanceNew,

				// کمک هزینه مسکن جدید
				MinistryLabourRightHousingNew = EmpInfo.MinistryLabourRightHousingNew,

				// حق خوار و بار جدید
				MinistryLaborRightFoodNew = EmpInfo.MinistryLaborRightFoodNew,

				// حق اولاد جدید
				ChildrensRightsMinistryLaborNew = EmpInfo.ChildrensRightsMinistryLaborNew,

				// مزایای رفاهی و انگیزه‌ای جدید
				WelfareMotivationalBenefitsNew = EmpInfo.WelfareMotivationalBenefitsNew,

				// حق تاهل جدید
				RightMarryMinistryLaborNew = EmpInfo.RightMarryMinistryLaborNew,

				// سایر مزایا جدید
				OtherBenefitsNew = EmpInfo.OtherBenefitsNew,

				// جمع کل 
				TotalMonthlySalaryBenefitsNew = EmpInfo.TotalMonthlySalaryBenefitsNew
			};

			var json = System.Text.Json.JsonSerializer.Serialize(VerdictDTO);

			Console.WriteLine("#Log :: json :: " + json);


			var R2 = await BayaApi.ExecuteSp(
				ShomaranApiMode.Polfilm,
				new StoredProcedureModel.StoredProcedureRequestDto
				{
					// [USP_VerdictInfos2](@JsonInput nvarchar(max))
					StoredProcedureName = "USP_VerdictInfos2",
					JsonInput = json
				}
			);

			if (R2 == null)
			{
				await _MSG.ShowError("خروجی وب سرویس null است");
				Console.WriteLine("❌ خطا: R == null");
				return;
			}

			var jsonResponse2 = R2.Content.ToString();
			Console.WriteLine("### 🟡 jsonResponse کامل ###");
			Console.WriteLine(jsonResponse2);

			if (!jsonResponse2.TrimStart().StartsWith("{"))
			{
				await _MSG.ShowError("خروجی وب سرویس JSON نیست:\n" + jsonResponse2);
				Console.WriteLine("❌ خطا: پاسخ JSON نیست");
				return;
			}

			try
			{
				var response2 = Newtonsoft.Json.JsonConvert.DeserializeObject<VerdictDataModel.RootResponse>(jsonResponse2);

				if (response2?.DataSets == null || response2.DataSets.Count == 0)
				{
					await _MSG.ShowError("DataSets خالی است");
					return;
				}

				var employee2 = response2.DataSets[0][0];

				EmpInfo = employee2;

				Console.WriteLine(":: Log EmpInfo Json :: " + await JSON.ToJson(EmpInfo));

				// --- نمایش دیالوگ ---
				var options = new BlazorBootstrap.ConfirmDialogOptions
				{
					YesButtonText = "بازگشت به حکم کارگزینی",
					YesButtonColor = ButtonColor.Info,
					NoButtonText = "",
				};

				var Export = await JSON.ToJson(_Entity.HR_CVR_RecruitmentRules);

				var rule = _Entity.HR_CVR_RecruitmentRules.FirstOrDefault();

				var previewList = new List<SalaryPreviewItem>
				{
					new() { Title = "مزد شغل", Value = rule?.JobSalaryRankNew },
					new() { Title = "مزد رتبه", Value = rule?.RankSalaryNew },
					new() { Title = "مزد سنوات", Value = rule?.SalaryHistoryNew },
					new() { Title = "حق پست", Value = rule?.RightGuardianshipNew },
					new() { Title = "ماندگاری در پست", Value = rule?.CoefficientDurabilityPostNew },
					new() { Title = "شرایط نامساعد محیط کار", Value = rule?.CoefficientDifficultAndHarmfulJobsNew },
					new() { Title = "جمع کل مزد مبنا", Value = rule?.TotalDailyBaseWageNew },
					new() { Title = "حق جذب", Value = rule?.RecruitmentAllowanceNew },
					new() { Title = "حق مسکن", Value = rule?.MinistryLabourRightHousingNew },
					new() { Title = "حق خوار و بار", Value = rule?.MinistryLaborRightFoodNew },
					new() { Title = "حق اولاد", Value = rule?.ChildrensRightsMinistryLaborNew },
					new() { Title = "حق تاهل", Value = rule?.RightMarryMinistryLaborNew },
					new() { Title = "مزایای رفاهی انگیزشی", Value = rule?.WelfareMotivationalBenefitsNew },
					//new() { Title = "جمع کل مزد مبنا و مزایای قانونی", Value = rule?.TotalMonthlySalaryBenefitsNew },
				};


				var rows = string.Join("", previewList.Select(x => $@"
				<tr>
					<td style='padding:6px 10px;'>{x.Title}</td>
					<td style='padding:6px 10px; text-align:right; font-weight:bold'>
						{x.Value?.ToString("N0")}
					</td>
				</tr>
				"));

				decimal totalSum = rule?.TotalMonthlySalaryBenefitsNew ?? 0.0m;
				//decimal totalSum1 = previewList.Sum(x => x.Value ?? 0m);

				var totalRow = $@"
				<tr style='background:#fff3cd; font-weight:bold;'>
					<td style='padding:8px'>جمع کل</td>
					<td style='padding:8px; text-align:right; color:#b02a37'>
						{totalSum.ToString("N0")}
					</td>
				</tr>";

				string htmlString = $@"
				<div style='direction: rtl;'>
					<table style='width:100%; border-collapse: collapse;' border='1'>
						<thead style='background:#f5f5f5'>
							<tr>
								<th style='padding:8px'>عنوان</th>
								<th style='padding:8px'>مبلغ (ریال)</th>
							</tr>
						</thead>
						<tbody>
							{rows}
							{totalRow}
						</tbody>
					</table>
				</div>";

				await Confirm.ShowAsync("پیش‌نمایش محاسبات حکم", htmlString, options);

				Console.WriteLine("✅ ### اتمام عملیات با موفقیت ###");

			}
			catch (Exception ex)
			{
				Console.WriteLine("❌ خطا در پردازش کلی: " + ex.Message);
				await _MSG.ShowError("خطا در پردازش SP: " + ex.Message);
			}


		}
		#endregion

		#region GetEmpDataGridOnSP
		/// <summary>
		/// پر کردن فیلد های گرید محاسبات حکم از جدول مجاز و یا SP
		/// </summary>
		/// <param name="Item"></param>
		/// <returns></returns>
		private async Task GetEmpDataGridOnSP()
		{
			//await Task.Yield();

			string detailEmployeeId = employeeId;

			Console.WriteLine($"🔍 Fetching employee data for ID: {detailEmployeeId}");

			// *************************************

			var x = await Utility.JSON.ToJson(EmpInfo);
			Console.WriteLine("Log :: data_emp Data ::" + x);


			if (EmpInfo == null)
			{
				Console.WriteLine($"❌ Employee data NOT FOUND for ID: {detailEmployeeId}");
				return;
			}

			Console.WriteLine($"✅ Employee data loaded: {EmpInfo.FirstName} {EmpInfo.LastName}");

			var ListData = _Entity.HR_CVR_RecruitmentRules.ToList();

			foreach (var EmpDataItem in ListData)
			{
				// کد کارمندی جدید
				EmpDataItem.EmployeeNo = EmpInfo.EmployeeNo;
				// کد پرسنلی جدید
				EmpDataItem.EmployeePersonelNo = EmpInfo.EmployeePersonelNo;
				// نام کارمند
				EmpDataItem.FirstName = EmpInfo.FirstName;
				// نام خانوادگی کارمند
				EmpDataItem.LastName = EmpInfo.LastName;
				// نام پدر کارمند
				EmpDataItem.FatherName = EmpInfo.FatherName;
				// کد ملی
				EmpDataItem.NationalCode = EmpInfo.NationalCode;
				// شماره شناسنامه
				EmpDataItem.IdCardNo = EmpInfo.IdCardNo;
				// جنسیت
				EmpDataItem.BaseInfo_GenderId = EmpInfo.BaseInfo_GenderTitle;
				// وضعیت تاهل
				EmpDataItem.BaseInfo_MaritalStatusId = EmpInfo.BaseInfo_MaritalStatusTitle;
				// شهر محل صدور شناسنامه
				EmpDataItem.CityOfIssue = EmpInfo.CityOfIssueTitle;
				// شهر تولد
				EmpDataItem.CityOfBirth = EmpInfo.CityOfBirthTitle;
				// تاریخ تولد شمسی
				EmpDataItem.BirthDate_Fa = EmpInfo.BirthDate_Fa;
				// نمایش سن کارمند به صورت متن
				EmpDataItem.EmployeeAgeText = EmpInfo.EmployeeAgeText;
				// تاریخ استخدام در گروه
				EmpDataItem.EmploymentDateInGroup_Fa = EmpInfo.EmploymentDateInGroup_Fa;
				// تاریخ استخدام در شرکت
				EmpDataItem.EmploymentDate_Fa = EmpInfo.EmploymentDate_Fa;
				// تاریخ آخرین تسویه حساب
				EmpDataItem.EmploymentStartDate_Fa = EmpInfo.EmploymentStartDate_Fa;
				// شماره بیمه
				EmpDataItem.InsuranceNumber = EmpInfo.InsuranceNumber;
				// نظام وظیفه
				EmpDataItem.BaseInfo_MilitaryStatusId = EmpInfo.BaseInfo_MilitaryStatusTitle;
				// حساب بانکی
				EmpDataItem.BankAccountNumber = EmpInfo.BankAccountNumber;
				// شماره شبا
				EmpDataItem.IBAN = EmpInfo.IBAN;

				// شناسه قسمت سازمانی
				//EmpDataItem.HR_ORG_SectionsId = string.IsNullOrEmpty(EmpInfo.HR_ORG_SectionsId) ? Guid.Empty : Guid.Parse(EmpInfo.HR_ORG_SectionsId);
				// شناسه پست سازمانی
				//EmpDataItem.HR_ORG_PostsId = string.IsNullOrEmpty(EmpInfo.HR_ORG_PostsId) ? Guid.Empty : Guid.Parse(EmpInfo.HR_ORG_PostsId);
				// عنوان شغلی
				//EmpDataItem.HR_CVR_JobId = string.IsNullOrEmpty(EmpInfo.HR_CVR_JobId) ? Guid.Empty : Guid.Parse(EmpInfo.HR_CVR_JobId);
				// گروه شغلی
				//EmpDataItem.HR_CVR_JobGroupId = string.IsNullOrEmpty(EmpInfo.HR_CVR_JobGroupId) ? Guid.Empty : Guid.Parse(EmpInfo.HR_CVR_JobGroupId);

				// **********
				// تعداد فرزند کارمند
				EmpDataItem.EmployeeChildrenCount = EmpInfo.CountChilderen;
				// تاریخ برقراری حق اولاد
				EmpDataItem.FirstChildAllowanceEstablishmentDate_Fa = EmpInfo.StartChildRightsGroupDate_Fa;

				// اگر HR_CVR_ApprovalsMinistryLaborGroupId پر باشد، مقادیر مربوطه را اعمال کن
				if (EmpDataItem.HR_CVR_ApprovalsMinistryLaborGroupId.HasValue)
				{
					await EMP_Data.EmployeeData.ApplyToLastRecruitmentRules(EmpDataItem, _User.UserID.ToString());
					Console.WriteLine("#log:: ApplyToLastRecruitmentRules ::");
				}
			}

			// بررسی وضعیت اینکه آیا حق تاهل تعلق می گیرد؟
			SetMarriageAllowanceStatus();

			// بررسی وضعیت اینکه آیا حق اولاد تعلق می گیرد یا خیر؟
			SetChildAllowanceStatus();

			StateHasChanged();
		}
		#endregion /GetEmpDataGridOnSP

		#region ChildAllowanceLogic

		/// <summary>
		/// بر اساس تعداد فرزند کارمند، وضعیت "آیا حق اولاد به کارمند تعلق می‌گیرد؟" را تعیین می‌کند.
		/// </summary>
		private void SetChildAllowanceStatus()
		{
			if (_Entity?.HR_CVR_RecruitmentRules == null)
			{
				Console.WriteLine("⚠️ HR_CVR_RecruitmentRules is null.");
				return;
			}

			Console.WriteLine($"✅ تعداد ردیف‌های گرید: {_Entity.HR_CVR_RecruitmentRules.Count()}");

			foreach (var item in _Entity.HR_CVR_RecruitmentRules)
			{
				Console.WriteLine($"Log :: EmployeeChildrenCount: {item.EmployeeChildrenCount}, قبل از تغییر IsChildAllowanceGrantedToEmployee: {item.IsChildAllowanceGrantedToEmployee}");

				// اگر تعداد فرزند بیشتر از 0 باشد، حق اولاد تعلق می‌گیرد
				if (item.EmployeeChildrenCount > 0)
				{
					item.IsChildAllowanceGrantedToEmployee = true;
					Console.WriteLine($"✅ IsChildAllowanceGrantedToEmployee برای ردیف {item._Id} به true تغییر یافت.");
				}
				else
				{
					item.IsChildAllowanceGrantedToEmployee = false;
					Console.WriteLine($"✅ IsChildAllowanceGrantedToEmployee برای ردیف {item._Id} به false تغییر یافت.");
				}

				Console.WriteLine($"Log :: پس از تغییر IsChildAllowanceGrantedToEmployee: {item.IsChildAllowanceGrantedToEmployee}");
			}

			// نیازی به بروزرسانی Ref نیست، چون فیلد به‌صورت مستقیم از مدل مقدار می‌گیرد
		}

		#endregion

		#region MarriageAllowanceLogic

		/// <summary>
		/// بر اساس مقدار فیلد "حق تاهل جدید"، مقدار فیلد "آیا به کارمند حق تاهل تعلق می‌گیرد؟" را تنظیم می‌کند.
		/// اگر RightMarryMinistryLaborNew > 0 باشد، IsMarriageAllowanceGrantedToEmployee = true.
		/// در غیر این‌صورت (0 یا null)، IsMarriageAllowanceGrantedToEmployee = false.
		/// </summary>
		private void SetMarriageAllowanceStatus()
		{
			Console.WriteLine("🔧 شروع محاسبه وضعیت حق تاهل بر اساس فیلد عددی ...");

			if (_Entity?.HR_CVR_RecruitmentRules == null)
			{
				Console.WriteLine("⚠️ لیست HR_CVR_RecruitmentRules خالی است. خروج از تابع.");
				return;
			}

			Console.WriteLine($"✅ تعداد ردیف‌های موجود در گرید: {_Entity.HR_CVR_RecruitmentRules.Count()}");

			foreach (var item in _Entity.HR_CVR_RecruitmentRules)
			{
				// مقدار قبلی فیلد بولین را نگه می‌داریم
				bool? oldValue = item.IsMarriageAllowanceGrantedToEmployee;

				// خواندن مقدار فیلد عددی
				decimal? rightMarryValue = item.RightMarryMinistryLaborNew;

				Console.WriteLine($"Log :: در حال بررسی ردیف با شناسه {item._Id}");
				Console.WriteLine($"Log :: مقدار فعلی RightMarryMinistryLaborNew: {rightMarryValue}");
				Console.WriteLine($"Log :: مقدار فعلی IsMarriageAllowanceGrantedToEmployee: {oldValue}");

				// تعیین وضعیت جدید بر اساس شرط
				bool newValue;
				if (rightMarryValue.HasValue && rightMarryValue > 0)
				{
					newValue = true;
					Console.WriteLine($"✅ شرط برآورده شد: RightMarryMinistryLaborNew > 0. مقدار جدید: true");
				}
				else
				{
					newValue = false;
					Console.WriteLine($"✅ شرط برآورده نشد: RightMarryMinistryLaborNew <= 0 یا null. مقدار جدید: false");
				}

				// ست کردن مقدار جدید در مدل
				item.IsMarriageAllowanceGrantedToEmployee = newValue;

				Console.WriteLine($"✅ مقدار IsMarriageAllowanceGrantedToEmployee به {newValue} تغییر یافت.");
			}

			Console.WriteLine("🔧 پایان محاسبه وضعیت حق تاهل برای تمام ردیف‌ها.");
		}

		#endregion

		#region ApplyToLastRecruitmentRules
		/// <summary>
		/// پر کردن فیلدهای مربوطه در HR_CVR_RecruitmentRules با مقادیر مصوبات جاری
		/// </summary>
		public static void ApplyToLastRecruitmentRules(HR_CVR_RecruitmentRules item)
		{

			HR_CVR_ApprovalsMinistryLaborGroup _cachedApproval = new();

			//if (item == null || _cachedApproval == null)
			//	return;

			// 1. کمک هزینه مسکن
			item.MinistryLabourRightHousing = _cachedApproval.MinistryLabourRightHousing;

			// 2. حق خوار و بار
			item.MinistryLaborRightFood = _cachedApproval.MinistryLaborRightFood;

			// 3. مزایای رفاهی و انگیزه‌ای = بن کارگری
			item.WelfareMotivationalBenefits = _cachedApproval.BenKargariMinistryLabor;

			// 4. حق تاهل
			item.RightMarryMinistryLabor = _cachedApproval.RightMarryMinistryLabor;

			// [اختیاری] حق اولاد
			// item.ChildrensRightsMinistryLabor = _cachedApproval.ChildrensRightsMinistryLabor;
		}
		#endregion

		#region SetZiroLastVerdictRule
		private async Task SetZiroLastVerdictRule()
		{
			foreach (var Item2 in _Entity.HR_CVR_RecruitmentRules)
			{
				Item2.JobSalaryRank = 0.0m;
				Item2.RankSalary = 0.0m;
				Item2.SalaryHistory = 0.0m;
				Item2.RightGuardianship = 0.0m;
				Item2.CoefficientDurabilityPost = 0.0m;
				Item2.CoefficientDifficultAndHarmfulJobs = 0.0m;
				Item2.TotalDailyBaseWage = 0.0m;
				Item2.DailyAdjustmentDifference = 0.0m;
				Item2.RecruitmentAllowance = 0.0m;
				Item2.MinistryLabourRightHousing = 0.0m;
				Item2.MinistryLaborRightFood = 0.0m;
				Item2.ChildrensRightsMinistryLabor = 0.0m;
				Item2.WelfareMotivationalBenefits = 0.0m;
				Item2.RightMarryMinistryLabor = 0.0m;
				Item2.OtherBenefits = 0.0m;
				Item2.TotalMonthlySalaryBenefits = 0.0m;
			}
		}
		#endregion

		#endregion

		#region Grid_HR_CVR_RecruitmentRules

		public async Task<bool> GridHR_CVR_VerdictRecruitingId_711_editmodelsaving(object e)
		{

			return false;
		}

		#region WaitComponentLoaded
		/// <summary>
		/// منتظر می‌ماند تا کامپوننت لود شود و سپس SetDisabled را فراخوانی می‌کند
		/// </summary>
		/// <param name="componentRef">مرجع کامپوننت</param>
		/// <param name="maxWaitTimeMs">حداکثر زمان انتظار به میلی‌ثانیه (پیش‌فرض: 5000)</param>
		/// <returns></returns>
		private async Task<bool> WaitComponentLoaded(dynamic componentRef, int maxWaitTimeMs = 5000)
		{
			if (componentRef != null)
			{
				return true;
			}

			// منتظر می‌مانیم تا کامپوننت لود شود
			int waitedTime = 0;
			int delayInterval = 50; // هر 50 میلی‌ثانیه چک می‌کنیم

			while (componentRef == null && waitedTime < maxWaitTimeMs)
			{
				await Task.Delay(delayInterval);
				waitedTime += delayInterval;
				StateHasChanged(); // برای به‌روزرسانی UI
			}

			if (componentRef != null)
			{
				return true;
			}
			else
			{
				Console.WriteLine($"⚠️ Warning: Component was not loaded after waiting {maxWaitTimeMs}ms");
				return false;
			}
		}
		#endregion

		public async Task GridHR_CVR_VerdictRecruitingId_711_afterrendermodal(Entity.HR_CVR_RecruitmentRules Item)
		{
			//await Task.Yield();
			//StateHasChanged();

			// برای اولین بار وقتی ردیفی در گرید وجود ندارد کلیه عدد های در تابع را 0.0 می کند
			if (_Entity.HR_CVR_RecruitmentRules.Count == 0)
			{
				await SetZiroLastVerdictRule();
			}

			#region فرخوانی داده ها از SP_Verdict

			// try
			// {
			// 	Console.WriteLine(":: Log Grid EmpInfo :: " + await JSON.ToJson(EmpInfo));

			// 	Console.WriteLine("EmpInfo is null? " + (EmpInfo == null));

			// 	// **********************************************************************************************************

			// 	// مزد شغل
			// 	Item.JobSalaryRankNew = EmpInfo.JobSalaryRankNew;
			// 	Console.WriteLine($":: Log Set JobSalaryRankNew for item {Item._Id} :: {Item.JobSalaryRankNew}");

			// 	// مزد رتبه
			// 	Item.RankSalaryNew = EmpInfo.RankSalaryNew;
			// 	Console.WriteLine($":: Log Set RankSalaryNew for item {Item._Id} :: {Item.JobSalaryRankNew}");

			// 	// مزد سنوات
			// 	Item.SalaryHistoryNew = EmpInfo.SalaryHistoryNew;
			// 	Console.WriteLine($":: Log Set SalaryHistoryNew for item {Item._Id} :: {Item.SalaryHistoryNew}");

			// 	// حق پست
			// 	Item.RightGuardianshipNew = EmpInfo.RightGuardianshipNew;
			// 	Console.WriteLine($":: Log Set RightGuardianshipNew for item {Item._Id} :: {Item.RightGuardianshipNew}");

			// 	// مزایای مانداگاری در پست
			// 	Item.CoefficientDurabilityPostNew = EmpInfo.CoefficientDurabilityPostNew;
			// 	Console.WriteLine($":: Log Set CoefficientDurabilityPostNew for item {Item._Id} :: {Item.CoefficientDurabilityPostNew}");

			// 	// شرایط نامساعد محیط کار
			// 	Item.CoefficientDifficultAndHarmfulJobsNew = EmpInfo.CoefficientDifficultAndHarmfulJobsNew;
			// 	Console.WriteLine($":: Log Set CoefficientDifficultAndHarmfulJobsNew for item {Item._Id} :: {Item.CoefficientDifficultAndHarmfulJobsNew}");

			// 	// جمع کل مزد مبنا جدید
			// 	Item.TotalDailyBaseWageNew = EmpInfo.TotalDailyBaseWageNew;
			// 	Console.WriteLine($":: Log Set TotalDailyBaseWageNew for item {Item._Id} :: {Item.TotalDailyBaseWageNew}");
			// 	//if (await WaitComponentLoaded(Ref_HR_CVR_RecruitmentRules_TotalDailyBaseWageNew))
			// 	//{
			// 	//	Ref_HR_CVR_RecruitmentRules_TotalDailyBaseWageNew.SetDisabled(true);
			// 	//}

			// 	// جمع کل مزد مبنا
			// 	Item.TotalDailyBaseWage = EmpInfo.TotalDailyBaseWage;
			// 	Console.WriteLine($":: Log Set TotalDailyBaseWage for item {Item._Id} :: {Item.TotalDailyBaseWage}");

			// 	// تفاوت تطبیق روزانه
			// 	Item.DailyAdjustmentDifferenceNew = EmpInfo.DailyAdjustmentDifferenceNew;
			// 	Console.WriteLine($":: Log Set DailyAdjustmentDifferenceNew for item {Item._Id} :: {Item.DailyAdjustmentDifferenceNew}");

			// 	// حق جذب
			// 	Item.RecruitmentAllowanceNew = EmpInfo.RecruitmentAllowanceNew;
			// 	Console.WriteLine($":: Log Set RecruitmentAllowanceNew for item {Item._Id} :: {Item.RecruitmentAllowanceNew}");

			// 	// کمک هزینه مسکن
			// 	Item.MinistryLabourRightHousingNew = EmpInfo.MinistryLabourRightHousingNew;
			// 	Console.WriteLine($":: Log Set MinistryLabourRightHousingNew for item {Item._Id} :: {Item.MinistryLabourRightHousingNew}");
			// 	//if (await WaitComponentLoaded(Ref_HR_CVR_RecruitmentRules_MinistryLabourRightHousingNew))
			// 	//{
			// 	//	Ref_HR_CVR_RecruitmentRules_MinistryLabourRightHousingNew.SetDisabled(true);
			// 	//}

			// 	// حق خوار و بار
			// 	Item.MinistryLaborRightFoodNew = EmpInfo.MinistryLaborRightFoodNew;
			// 	Console.WriteLine($":: Log Set MinistryLaborRightFoodNew for item {Item._Id} :: {Item.MinistryLaborRightFoodNew}");
			// 	//if (await WaitComponentLoaded(Ref_HR_CVR_RecruitmentRules_MinistryLaborRightFoodNew))
			// 	//{
			// 	//	Ref_HR_CVR_RecruitmentRules_MinistryLaborRightFoodNew.SetDisabled(true);
			// 	//}

			// 	// حق اولاد
			// 	Item.ChildrensRightsMinistryLaborNew = EmpInfo.ChildrensRightsMinistryLaborNew;
			// 	Console.WriteLine($":: Log Set ChildrensRightsMinistryLaborNew for item {Item._Id} :: {Item.ChildrensRightsMinistryLaborNew}");
			// 	//if (await WaitComponentLoaded(Ref_HR_CVR_RecruitmentRules_ChildrensRightsMinistryLaborNew))
			// 	//{
			// 	//	Ref_HR_CVR_RecruitmentRules_ChildrensRightsMinistryLaborNew.SetDisabled(true);
			// 	//}

			// 	// مزایای رفاهی انگیزه ای ماهانه
			// 	Item.WelfareMotivationalBenefitsNew = EmpInfo.WelfareMotivationalBenefitsNew;
			// 	Console.WriteLine($":: Log Set WelfareMotivationalBenefitsNew for item {Item._Id} :: {Item.WelfareMotivationalBenefitsNew}");
			// 	//if (await WaitComponentLoaded(Ref_HR_CVR_RecruitmentRules_WelfareMotivationalBenefitsNew))
			// 	//{
			// 	//	Ref_HR_CVR_RecruitmentRules_WelfareMotivationalBenefitsNew.SetDisabled(true);
			// 	//}

			// 	// حق تاهل
			// 	Item.RightMarryMinistryLaborNew = EmpInfo.RightMarryMinistryLaborNew;
			// 	Console.WriteLine($":: Log Set RightMarryMinistryLaborNew for item {Item._Id} :: {Item.RightMarryMinistryLaborNew}");
			// 	//if (await WaitComponentLoaded(Ref_HR_CVR_RecruitmentRules_RightMarryMinistryLaborNew))
			// 	//{
			// 	//	Ref_HR_CVR_RecruitmentRules_RightMarryMinistryLaborNew.SetDisabled(true);
			// 	//}

			// 	// سایر مزایا
			// 	Item.OtherBenefitsNew = EmpInfo.OtherBenefitsNew;
			// 	Console.WriteLine($":: Log Set OtherBenefitsNew for item {Item._Id} :: {Item.OtherBenefitsNew}");

			// 	// جمع کل دستمزد مزایایی قانونی و جمع مزد مبنا جدید
			// 	Item.TotalMonthlySalaryBenefitsNew = EmpInfo.TotalMonthlySalaryBenefitsNew;
			// 	Console.WriteLine($":: Log Set TotalMonthlySalaryBenefitsNew for item {Item._Id} :: {Item.TotalMonthlySalaryBenefitsNew}");
			// 	//if (await WaitComponentLoaded(Ref_HR_CVR_RecruitmentRules_TotalMonthlySalaryBenefitsNew))
			// 	//{
			// 	//	Ref_HR_CVR_RecruitmentRules_TotalMonthlySalaryBenefitsNew.SetDisabled(true);
			// 	//}

			// 	// جمع کل دستمزد مزایایی قانونی و جمع مزد مبنا
			// 	Item.TotalMonthlySalaryBenefits = EmpInfo.TotalMonthlySalaryBenefits;
			// 	Console.WriteLine($":: Log Set TotalMonthlySalaryBenefits for item {Item._Id} :: {Item.TotalMonthlySalaryBenefits}");


			// 	// **********
			// 	//// جمع کل مزد مبنا قبلی
			// 	//if (await WaitComponentLoaded(Ref_HR_CVR_RecruitmentRules_TotalDailyBaseWage))
			// 	//{
			// 	//	Ref_HR_CVR_RecruitmentRules_TotalDailyBaseWage.SetDisabled(true);
			// 	//}
			// 	//// جمع کل دستمزد مزایایی قانونی و جمع مزد مبنا قبلی
			// 	//if (await WaitComponentLoaded(Ref_HR_CVR_RecruitmentRules_TotalMonthlySalaryBenefits))
			// 	//{
			// 	//	Ref_HR_CVR_RecruitmentRules_TotalMonthlySalaryBenefits.SetDisabled(true);
			// 	//}


			// 	// **********
			// 	Entity.HR_ORG_Posts Posts = new();
			// 	//Posts.Id = EmpInfo.PostsId;
			// 	Posts.Id = Guid.Parse(EmpInfo.PostsId.ToString());
			// 	Posts.Title = EmpInfo.PostTitle;

			// 	// Console.WriteLine("#Log 1");

			// 	Ref_HR_CVR_RecruitmentRules_HR_ORG_PostsId.SetEntity(Posts);

			// 	// Console.WriteLine(await Utility.JSON.ToJson(JobGroup));
			// 	Ref_HR_CVR_RecruitmentRules_HR_ORG_PostsId.ItemSelected(Posts);

			// 	// Console.WriteLine("#Log 2");

			// 	await Task.Delay(100);
			// 	Ref_HR_CVR_RecruitmentRules_HR_ORG_PostsId.LoadData();

			// 	// **********



			// 	// تکمیل فیلدهای گرید ::
			// 	await GetEmpDataGrid(Item);

			// 	// ✅ تنظیم خودکار حق اولاد بر اساس تعداد فرزند — بعد از SP
			// 	SetChildAllowanceStatus();
			// 	// **********************************

			// 	// اگر HR_CVR_ApprovalsMinistryLaborGroupId پر باشد، مقادیر مربوطه را اعمال کن
			// 	if (Item.HR_CVR_ApprovalsMinistryLaborGroupId.HasValue)
			// 	{
			// 		await EMP_Data.EmployeeData.ApplyToLastRecruitmentRules(Item, _User.UserID.ToString());
			// 	}

			// 	StateHasChanged();
			// }
			// catch (Exception ex)
			// {
			// 	Console.WriteLine($"💥 Exception in Grid => AfterRender Error :: {ex}");
			// }
			#endregion
		}

		#region VerdictGrid_HR_EMP_EmployeesId
		/// <summary>
		/// پر کردن شناسه کارمند از بخش اصلی فرم در فیلد شناسه کارمند در گرید محاسبات حقوق و دستمزد
		/// </summary>
		/// <param name="Selected"></param>
		/// <param name="Item"></param>
		/// <returns></returns>
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

		#endregion /VerdictGrid_HR_EMP_EmployeesId

		#endregion

		#region Grid_EveryPostVerdict

		public async Task<bool> GridHR_CVR_VerdictRecruitingId_445_editmodelsaving(object e)
		{

			return false;
		}

		public async Task GridHR_CVR_VerdictRecruitingId_445_afterrendermodal(Entity.HR_CVR_EveryPostVerdict Item)
		{
			// بررسی ردیف درخواست در گرید پست های حکم کارمند 
			await CheckedRowGrid_EveryPostVerdict(Item);
			// بررس دکمه های گرید پست های حکم
			await ToggleDetails_Grid_EveryPostVerdict_AddButton();

			// 

		}

		/// <summary>
		/// آخرین تعداد ردیف جزئیات
		/// </summary>
		private int _lastDetailsCount = 0;

		/// <summary>
		/// بررسی تعداد آخرین ردیف جزئیات
		/// بر اساس تعداد ردیف جزئیات انتخاب شده، مشخص می شود که آیا فرم قابل ارسال است یا خیر.
		/// فقط و فقط یک ردیف جزئیات مجاز است.
		/// </summary>
		/// <returns>true اگر دقیقاً یک ردیف غیر حذف‌شده وجود داشته باشد</returns>
		private async Task<bool> HasValidDetailsCount()
		{
			// بررسی null بودن لیست
			if (_Entity.HR_CVR_EveryPostVerdict == null)
			{
				await _MSG.ShowError("لطفاً حداقل یک ردیف در بخش «پست های حکم کارمند» ثبت کنید.");
				return false;
			}

			// شمارش ردیف‌های فعال (غیر حذف‌شده)
			var activeCount = _Entity.HR_CVR_EveryPostVerdict.Count(x => x.IsDelete != true);

			if (activeCount == 0)
			{
				await _MSG.ShowError("لطفاً حداقل یک ردیف در بخش «پست های حکم کارمند» ثبت کنید.");
				return false;
			}

			if (activeCount > 1)
			{
				await _MSG.ShowError("شما مجاز به ثبت بیش از یک ردیف نیستید. لطفاً فقط یک ردیف در بخش «پست های حکم کارمند» ثبت کنید.");
				return false;
			}

			return true;
		}

		/// <summary>
		/// متد برای به‌روزرسانی وضعیت دکمه بعد از ذخیره شدن ردیف در گرید پست های حکم
		/// این متد باید بعد از SaveGrid_EveryPostVerdict_Details در فایل razor فراخوانی شود
		/// </summary>
		/// <returns></returns>
		public async Task UpdateDetailsGrid_EveryPostVerdict_AddButtonAfterSave()
		{
			// بعد از ذخیره، بررسی می‌کنیم که آیا ردیفی وجود دارد
			// اگر وجود داشت، دکمه جدید را مخفی می‌کنیم
			await ToggleDetails_Grid_EveryPostVerdict_AddButton();

			// به‌روزرسانی شمارنده
			var currentCount = _Entity.HR_CVR_EveryPostVerdict?.Count(x => x.IsDelete != true) ?? 0;
			_lastDetailsCount = currentCount;
		}

		/// <summary>
		/// بررسی ردیف درخواست در گرید پست های حکم کارمند
		/// </summary>
		/// <param name="Item"></param>
		/// <returns></returns>
		private async Task CheckedRowGrid_EveryPostVerdict(Entity.HR_CVR_EveryPostVerdict Item)
		{
			// بررسی می‌کنیم که آیا یک ردیف غیر حذف‌شده در لیست وجود دارد
			// این بررسی دقیق‌تر است چون ممکن است item جدید باشد اما هنوز ذخیره نشده باشد
			var hasSavedRecord = _Entity.HR_CVR_EveryPostVerdict?.Any(x => x.IsDelete != true) == true;

			// اگر Item مشخص شده باشد و Id داشته باشد، مطمئن می‌شویم که ذخیره شده است
			if (Item != null && Item.Id != Guid.Empty && Item.IsDelete != true)
			{
				// بررسی می‌کنیم که آیا این ردیف واقعاً در لیست وجود دارد
				var existsInList = _Entity.HR_CVR_EveryPostVerdict?.Any(x => x.Id == Item.Id && x.IsDelete != true) == true;
				if (existsInList)
				{
					hasSavedRecord = true;
				}
			}
		}

		/// <summary>
		/// بررسی وضعیت دکمه های ثبت جدید - ذخیر و جدید، قبلی و بعدی در گرید پست های حکم
		/// </summary>
		/// <param name="hasSavedRecord"></param>
		/// <param name="isInModal"></param>
		/// <returns></returns>
		private async Task ToggleDetails_Grid_EveryPostVerdict_AddButton(bool hasSavedRecord = false, bool isInModal = false)
		{
			await Task.Yield();

			// اگر hasSavedRecord مشخص نشده باشد، خودکار بررسی می‌کنیم
			// بررسی می‌کنیم که آیا یک ردیف غیر حذف‌شده در لیست وجود دارد
			if (!hasSavedRecord)
			{
				hasSavedRecord = _Entity.HR_CVR_EveryPostVerdict?.Any(x => x.IsDelete != true) == true;
			}

			// اگر یک ردیف ذخیره شده وجود دارد، دکمه افزودن را مخفی می‌کنیم
			// چون فقط یک ردیف مجاز است
			if (hasSavedRecord)
			{
				// ردیف وجود دارد → دکمه جدید را مخفی می‌کنیم
				await JS.InvokeVoidAsync("AddClass", "#HR_CVR_EveryPostVerdict_GridHR_CVR_VerdictRecruitingId_445ButtonNew", "d-none");
			}
			else
			{
				// ردیف وجود ندارد → دکمه جدید را نمایش می‌دهیم
				await JS.InvokeVoidAsync("RemoveClass", "#HR_CVR_EveryPostVerdict_GridHR_CVR_VerdictRecruitingId_445ButtonNew", "d-none");
			}

			// فقط اگر در مودال باشیم، دکمه‌های مودال را مخفی کن
			if (isInModal)
			{
				// دکمه ذخیره و جدید - مخفی می‌شود چون فقط یک ردیف مجاز است
				await JS.InvokeVoidAsync("ModalAddClass", "#HR_CVR_EveryPostVerdict_GridHR_CVR_VerdictRecruitingId_445ButtonSaveAndNew", "d-none");
				// دکمه قبلی - مخفی می‌شود چون فقط یک ردیف وجود دارد
				await JS.InvokeVoidAsync("ModalAddClass", "#HR_CVR_EveryPostVerdict_GridHR_CVR_VerdictRecruitingId_445ButtonBefore", "d-none");
				// دکمه بعدی - مخفی می‌شود چون فقط یک ردیف وجود دارد
				await JS.InvokeVoidAsync("ModalAddClass", "#HR_CVR_EveryPostVerdict_GridHR_CVR_VerdictRecruitingId_445ButtonNext", "d-none");
			}
		}

		#endregion Grid_EveryPostVerdict

		#region CalculateTotalSalaryBenefits
		private void CalculateTotalSalaryBenefits(HR_CVR_RecruitmentRules Item)
		{
			if (Item == null)
				return;

			Console.WriteLine($"Before calculation - Total: {Item.TotalDailyBaseWage}");

			//Item.TotalDailyBaseWage = 0.0m;

			decimal total = 0.0m;

			total += Item.JobSalaryRank ?? 0.0m;
			total += Item.RankSalary ?? 0.0m;
			total += Item.SalaryHistory ?? 0.0m;
			total += Item.RightGuardianship ?? 0.0m;
			total += Item.CoefficientDurabilityPost ?? 0.0m;
			total += Item.CoefficientDifficultAndHarmfulJobs ?? 0.0m;
			total += Item.DailyAdjustmentDifference ?? 0.0m;
			total += Item.RecruitmentAllowance ?? 0.0m;
			total += Item.MinistryLabourRightHousing ?? 0.0m;
			total += Item.MinistryLaborRightFood ?? 0.0m;
			total += Item.ChildrensRightsMinistryLabor ?? 0.0m;
			total += Item.WelfareMotivationalBenefits ?? 0.0m;
			total += Item.RightMarryMinistryLabor ?? 0.0m;
			total += Item.OtherBenefits ?? 0.0m;

			Item.TotalDailyBaseWage = total;

			Console.WriteLine($"After calculation - Total: {Item.TotalDailyBaseWage}");
		}

		private decimal SafeParseToDecimal(object value)
		{
			if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
				return 0;

			if (decimal.TryParse(value.ToString(), out decimal result))
				return result;

			return 0;
		}

		private void UpdateTotalDailyBaseWageUI(HR_CVR_RecruitmentRules item)
		{
			if (Ref_HR_CVR_RecruitmentRules_TotalDailyBaseWage != null)
			{
				Ref_HR_CVR_RecruitmentRules_TotalDailyBaseWage.Value = item.TotalDailyBaseWage;
			}
		}

		#endregion

		#region CalculateTotal

		public async Task JobSalaryRankNew_oninput(ChangeEventArgs Selected, Entity.HR_CVR_RecruitmentRules Item)
		{

		}
		public async Task JobSalaryRank_oninput(ChangeEventArgs Selected, Entity.HR_CVR_RecruitmentRules Item)
		{
			//	Item.JobSalaryRank = SafeParseToDecimal(Selected.Value);
			//	CalculateTotalSalaryBenefits(Item);
			//	Console.WriteLine($"JobSalaryRank :: TOTAL NOW = {Item.TotalDailyBaseWage}");
			//	StateHasChanged();
		}

		public async Task RankSalary_oninput(ChangeEventArgs Selected, Entity.HR_CVR_RecruitmentRules Item)
		{
			//Item.RankSalary = SafeParseToDecimal(Selected.Value);
			//CalculateTotalSalaryBenefits(Item);
			//Console.WriteLine($"RankSalary :: TOTAL NOW = {Item.TotalDailyBaseWage}");
			//StateHasChanged();
		}
		public async Task SalaryHistory_oninput(ChangeEventArgs Selected, Entity.HR_CVR_RecruitmentRules Item)
		{
			//Item.SalaryHistory = SafeParseToDecimal(Selected.Value);
			//CalculateTotalSalaryBenefits(Item);
			//Console.WriteLine($"SalaryHistory :: TOTAL NOW = {Item.TotalDailyBaseWage}");
			//StateHasChanged();
		}
		public async Task RightGuardianship_oninput(ChangeEventArgs Selected, Entity.HR_CVR_RecruitmentRules Item)
		{
			//Item.RightGuardianship = SafeParseToDecimal(Selected.Value);
			//CalculateTotalSalaryBenefits(Item);
			//Console.WriteLine($"RightGuardianship :: TOTAL NOW = {Item.TotalDailyBaseWage}");
			//StateHasChanged();
		}
		public async Task CoefficientDurabilityPost_oninput(ChangeEventArgs Selected, Entity.HR_CVR_RecruitmentRules Item)
		{
			//Item.CoefficientDurabilityPost = SafeParseToDecimal(Selected.Value);
			//CalculateTotalSalaryBenefits(Item);
			//Console.WriteLine($"CoefficientDurabilityPost :: TOTAL NOW = {Item.TotalDailyBaseWage}");
			//StateHasChanged();
		}
		public async Task CoefficientDifficultAndHarmfulJobs_oninput(ChangeEventArgs Selected, Entity.HR_CVR_RecruitmentRules Item)
		{
			//Item.CoefficientDifficultAndHarmfulJobs = SafeParseToDecimal(Selected.Value);
			//CalculateTotalSalaryBenefits(Item);
			//Console.WriteLine($"CoefficientDifficultAndHarmfulJobs :: TOTAL NOW = {Item.TotalDailyBaseWage}");
			//StateHasChanged();
		}

		#endregion

		#region EveryPostVerdict_0
		public async Task<List<HR_CVR_VerdictRecruiting>> EveryPostVerdict(string employeeId)
		{
			var TablePost = new Baya.Models.ORM.Table
			{
				Name = "HR_CVR_VerdictRecruiting",
				NameAs = "HR_CVR_VerdictRecruiting",

				Column = new List<Coulmn>
				{
					new Coulmn { Name = "HR_Base_InsuranceTypesId", NameAs = "HR_Base_InsuranceTypesId" },
					new Coulmn { Name = "HR_CVR_PersonnelContractId", NameAs = "HR_CVR_PersonnelContractId" },
					new Coulmn { Name = "HR_CVR_TypesRulingsId", NameAs = "HR_CVR_TypesRulingsId" },
					new Coulmn { Name = "InsuranceNumber", NameAs = "InsuranceNumber" },
					new Coulmn { Name = "TypeBonusPayment", NameAs = "TypeBonusPayment" },
					new Coulmn { Name = "GroupTitle", NameAs = "GroupTitle" },
					new Coulmn { Name = "Rank", NameAs = "Rank" },
					new Coulmn { Name = "ExecutionDateSentence", NameAs = "ExecutionDateSentence" },
					new Coulmn { Name = "RegisterTime", NameAs = "RegisterTime" },
					new Coulmn { Name = "ConfirmerTime", NameAs = "ConfirmerTime" },
					new Coulmn { Name = "ApproverTime", NameAs = "ApproverTime" },
					new Coulmn { Name = "NullifierTime", NameAs = "NullifierTime" },
					new Coulmn { Name = "HR_StatusVerdictRecruitingId", NameAs = "HR_StatusVerdictRecruitingId" },
					new Coulmn { Name = "HR_CVR_VerdictRecruitingId", NameAs = "HR_CVR_VerdictRecruitingId" },
					new Coulmn { Name = "HR_CVR_JobId", NameAs = "HR_CVR_JobId" },
					new Coulmn { Name = "HR_CVR_DescriptionRulingsId", NameAs = "HR_CVR_DescriptionRulingsId" },
					new Coulmn { Name = "ExecutionDateSentence_Fa", NameAs = "ExecutionDateSentence_Fa" },
					new Coulmn { Name = "HR_CVR_JobGroupId", NameAs = "HR_CVR_JobGroupId" },
					new Coulmn { Name = "HR_EMP_EmployeesId", NameAs = "HR_EMP_EmployeesId" }
				},
				Relation = new List<Baya.Models.ORM.Table>
				{
					new Baya.Models.ORM.Table
					{
						Name = "HR_CVR_EveryPostVerdict",
						NameAs = "HR_CVR_EveryPostVerdict",
						ModeErtebat = ModeErtebat._1N,

						Column = new List<Coulmn>
						{
						new Coulmn { Name = "Id", NameAs = "Id" },
						new Coulmn { Name = "HR_CVR_VerdictRecruitingId", NameAs = "HR_CVR_VerdictRecruitingId" },
						new Coulmn { Name = "PostType", NameAs = "PostType" },
						new Coulmn { Name = "SectionsType", NameAs = "SectionsType" },
						new Coulmn { Name = "HR_ORG_SectionsId", NameAs = "HR_ORG_SectionsId" },
						new Coulmn { Name = "HR_ORG_PostsId", NameAs = "HR_ORG_PostsId" }
						},
					}
				}
			};

			var NewQuery = new QueryBuilderFilterRule
			{
				Condition = "AND",
				Rules = new List<QueryBuilderFilterRule>
				{
				    // فیلتر بر اساس کارمند
				    new QueryBuilderFilterRule
					{
						Field = "HR_EMP_EmployeesId",
						Operator = "equal",
						Type = "string",
						Value = new string[] { employeeId }
					},
				    // (اختیاری ولی توصیه‌شده) فقط احکام فعال
				    new QueryBuilderFilterRule
					{
						Field = "HR_StatusVerdictRecruitingId",
						Operator = "equal",
						Type = "string",
						Value = new[] { "0DA9356F-211D-F011-A502-005056A2B6BD" } // وضعیت "فعال"
					}
				}
			};

			// فراخوانی API
			var Model = await ApiServer.External.Services.Data.GetList(
				Entity: "HR_CVR_VerdictRecruiting",
				limit: null,
				skip: null,
				include: TablePost,
				Filter: NewQuery
			);

			if (Model?.Status == HttpStatusCode.OK && !string.IsNullOrEmpty(Model.Content?.ToString()))
			{
				try
				{
					var approvals = await JSON.ToObject<List<HR_CVR_VerdictRecruiting>>(Model.Content.ToString());
					return approvals;
				}
				catch (Exception ex)
				{
					Console.WriteLine($"💥 خطا در فراخوانی داده ها: {ex.Message}");
				}
			}
			else
			{
				Console.WriteLine($"❌ خطا در دریافت مصوبه با شناسه {Model?.Status}");
			}

			var TableGet = new Baya.Models.ORM.Table
			{
				Name = "HR_CVR_EveryPostVerdict",
				NameAs = "HR_CVR_EveryPostVerdict",

				Column = new List<Coulmn>
				{
					new Coulmn { Name = "Id", NameAs = "Id" },
					new Coulmn { Name = "RequestID", NameAs = "RequestID" },
					new Coulmn { Name = "CreateUser", NameAs = "CreateUser" },
					new Coulmn { Name = "UpdateUser", NameAs = "UpdateUser" },
					new Coulmn { Name = "CreateDate", NameAs = "CreateDate" },
					new Coulmn { Name = "UpdateDate", NameAs = "UpdateDate" },
					new Coulmn { Name = "IsDelete", NameAs = "IsDelete" },
					new Coulmn { Name = "HR_CVR_VerdictRecruitingId", NameAs = "HR_CVR_VerdictRecruitingId" },
					new Coulmn { Name = "PostType", NameAs = "PostType" },
					new Coulmn { Name = "SectionsType", NameAs = "SectionsType" },
					new Coulmn { Name = "HR_ORG_SectionsId", NameAs = "HR_ORG_SectionsId" },
					new Coulmn { Name = "HR_ORG_PostsId", NameAs = "HR_ORG_PostsId" }
				},

				Relation = new List<Baya.Models.ORM.Table>
				{
				    // ================= HR_ORG_Posts =================
				    new Baya.Models.ORM.Table
					{
						Name = "HR_ORG_Posts",
						NameAs = "HR_ORG_Posts",
						ModeErtebat = ModeErtebat._1N,

						Column = new List<Coulmn>
						{
							new Coulmn { Name = "State", NameAs = "State" },
							new Coulmn { Name = "Title", NameAs = "Title" },
							new Coulmn { Name = "TitleEn", NameAs = "TitleEn" },
							new Coulmn { Name = "DeleteDateTime", NameAs = "DeleteDateTime" },
							new Coulmn { Name = "Code", NameAs = "Code" },
							new Coulmn { Name = "Description", NameAs = "Description" },
							new Coulmn { Name = "AteyehSaz_InsuranceId", NameAs = "AteyehSaz_InsuranceId" },
							new Coulmn { Name = "HR_ORG_PositionsId", NameAs = "HR_ORG_PositionsId" },
							new Coulmn { Name = "HR_Base_InsuranceJobsId", NameAs = "HR_Base_InsuranceJobsId" },
							new Coulmn { Name = "PostCode", NameAs = "PostCode" }
						},

						Relation = new List<Baya.Models.ORM.Table>()
					},

				    // ================= HR_ORG_Sections =================
				    new Baya.Models.ORM.Table
					{
						Name = "HR_ORG_Sections",
						NameAs = "HR_ORG_Sections",
						ModeErtebat = ModeErtebat._1N,

						Column = new List<Coulmn>
						{
							new Coulmn { Name = "HR_EMP_EmployeesId", NameAs = "HR_EMP_EmployeesId" },
							new Coulmn { Name = "Status", NameAs = "Status" },
							new Coulmn { Name = "Title", NameAs = "Title" },
							new Coulmn { Name = "TitleEn", NameAs = "TitleEn" },
							new Coulmn { Name = "Description", NameAs = "Description" },
							new Coulmn { Name = "Code", NameAs = "Code" },
							new Coulmn { Name = "DeleteDateTime", NameAs = "DeleteDateTime" },
							new Coulmn { Name = "HR_ORG_SitesId", NameAs = "HR_ORG_SitesId" },
							new Coulmn { Name = "HR_ORG_SectionTypesId", NameAs = "HR_ORG_SectionTypesId" },
							new Coulmn { Name = "HR_Base_TaxCategoryId", NameAs = "HR_Base_TaxCategoryId" },
							new Coulmn { Name = "HR_ORG_SectionStatusId", NameAs = "HR_ORG_SectionStatusId" },
							new Coulmn { Name = "ParentId", NameAs = "ParentId" },
							new Coulmn { Name = "BaseInfo_ORG_SitesId", NameAs = "BaseInfo_ORG_SitesId" },
							new Coulmn { Name = "BaseInfo_TaxCategoryId", NameAs = "BaseInfo_TaxCategoryId" },
							new Coulmn { Name = "BaseInfo_ORG_CompaniesId", NameAs = "BaseInfo_ORG_CompaniesId" }
						},

						Relation = new List<Baya.Models.ORM.Table>()
					},

				    // ================= HR_CVR_VerdictRecruiting =================
				    new Baya.Models.ORM.Table
					{
						Name = "HR_CVR_VerdictRecruiting",
						NameAs = "HR_CVR_VerdictRecruiting",
						ModeErtebat = ModeErtebat._N1,

						Column = new List<Coulmn>
						{
							new Coulmn { Name = "HR_Base_InsuranceTypesId", NameAs = "HR_Base_InsuranceTypesId" },
							new Coulmn { Name = "HR_CVR_PersonnelContractId", NameAs = "HR_CVR_PersonnelContractId" },
							new Coulmn { Name = "HR_CVR_TypesRulingsId", NameAs = "HR_CVR_TypesRulingsId" },
							new Coulmn { Name = "InsuranceNumber", NameAs = "InsuranceNumber" },
							new Coulmn { Name = "TypeBonusPayment", NameAs = "TypeBonusPayment" },
							new Coulmn { Name = "GroupTitle", NameAs = "GroupTitle" },
							new Coulmn { Name = "Rank", NameAs = "Rank" },
							new Coulmn { Name = "ExecutionDateSentence", NameAs = "ExecutionDateSentence" },
							new Coulmn { Name = "RegisterTime", NameAs = "RegisterTime" },
							new Coulmn { Name = "ConfirmerTime", NameAs = "ConfirmerTime" },
							new Coulmn { Name = "ApproverTime", NameAs = "ApproverTime" },
							new Coulmn { Name = "NullifierTime", NameAs = "NullifierTime" },
							new Coulmn { Name = "HR_StatusVerdictRecruitingId", NameAs = "HR_StatusVerdictRecruitingId" },
							new Coulmn { Name = "HR_CVR_VerdictRecruitingId", NameAs = "HR_CVR_VerdictRecruitingId" },
							new Coulmn { Name = "RegisterUserId", NameAs = "RegisterUserId" },
							new Coulmn { Name = "ConfirmerUserId", NameAs = "ConfirmerUserId" },
							new Coulmn { Name = "ApproverUserId", NameAs = "ApproverUserId" },
							new Coulmn { Name = "NullifierUserId", NameAs = "NullifierUserId" },
							new Coulmn { Name = "HR_CVR_JobId", NameAs = "HR_CVR_JobId" },
							new Coulmn { Name = "HR_CVR_DescriptionRulingsId", NameAs = "HR_CVR_DescriptionRulingsId" },
							new Coulmn { Name = "Employee_NotMaped", NameAs = "Employee_NotMaped" },
							new Coulmn { Name = "Code", NameAs = "Code" },
							new Coulmn { Name = "ExecutionDateSentence_Fa", NameAs = "ExecutionDateSentence_Fa" },
							new Coulmn { Name = "HR_CVR_JobGroupId", NameAs = "HR_CVR_JobGroupId" },
							new Coulmn { Name = "HR_EMP_EmployeesId", NameAs = "HR_EMP_EmployeesId" }
						},

						Relation = new List<Baya.Models.ORM.Table>()
					}
				}
			};

			return null;
		}
		#endregion

		#endregion FunctionEvents

	}
}