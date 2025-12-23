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
using SP_ContractTime;
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
			// شناسه کارمند
			await GetEmpId(_Entity.HR_EMP_EmployeesId.ToString());

			// فراخوانی - آخرین حکم و قرارداد
			await LastEmpVerdictAndContract(_Entity.HR_EMP_EmployeesId.ToString());

		}

		#region FunctionEvents

		string employeeId = "";

		SP_ContractTime.EmployeeInfo EmpInfo = new();

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

		#region btn_SP_Verdict
		/// <summary>
		/// دکمه استورد پروسیجر
		/// </summary>
		/// <param name="Selected"></param>
		/// <returns></returns>
		public async Task SP_Verdict_onclick(MouseEventArgs Selected)
		{
			#region - 1 WITH EmpId
			Console.WriteLine("### 🟡 شروع فراخوانی PersonnelContract ###");

			if (_Entity?.HR_EMP_EmployeesId == null)
			{
				await _MSG.ShowWarning("لطفاً ابتدا کارمند را انتخاب کنید.");
				Console.WriteLine("❌ HR_EMP_EmployeesId null است");
				return;
			}

			var R = await BayaApi.PersonnelVerdictInfos(
				ShomaranApiMode.Polfilm,
				new EmpId
				{
					EmployeesId = _Entity.HR_EMP_EmployeesId.Value
				}
			);

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
				var response = Newtonsoft.Json.JsonConvert.DeserializeObject<RootResponse>(jsonResponse);

				if (response?.DataSets == null || response.DataSets.Count == 0)
				{
					await _MSG.ShowError("DataSets خالی است");
					return;
				}

				var employee = response.DataSets[0][0];
				//var A =
				//	employee.RankSalaryNew;

				EmpInfo = employee;


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

						// جمع کل مزد مبنا
						item.TotalDailyBaseWageNew = EmpInfo.TotalDailyBaseWageNew;
						Console.WriteLine($":: Log Set TotalDailyBaseWageNew for item {item._Id} :: {item.TotalDailyBaseWageNew}");
						Ref_HR_CVR_RecruitmentRules_TotalDailyBaseWageNew.SetDisabled(true);

						// تفاوت تطبیق روزانه
						item.DailyAdjustmentDifferenceNew = EmpInfo.DailyAdjustmentDifferenceNew;
						Console.WriteLine($":: Log Set DailyAdjustmentDifferenceNew for item {item._Id} :: {item.DailyAdjustmentDifferenceNew}");

						// حق جذب
						item.RecruitmentAllowanceNew = EmpInfo.RecruitmentAllowanceNew;
						Console.WriteLine($":: Log Set RecruitmentAllowanceNew for item {item._Id} :: {item.RecruitmentAllowanceNew}");

						// کمک هزینه مسکن
						item.MinistryLabourRightHousingNew = EmpInfo.MinistryLabourRightHousingNew;
						Console.WriteLine($":: Log Set MinistryLabourRightHousingNew for item {item._Id} :: {item.MinistryLabourRightHousingNew}");
						Ref_HR_CVR_RecruitmentRules_MinistryLabourRightHousingNew.SetDisabled(true);

						// حق خوار و بار
						item.MinistryLaborRightFoodNew = EmpInfo.MinistryLaborRightFoodNew;
						Console.WriteLine($":: Log Set MinistryLaborRightFoodNew for item {item._Id} :: {item.MinistryLaborRightFoodNew}");
						Ref_HR_CVR_RecruitmentRules_MinistryLaborRightFoodNew.SetDisabled(true);

						// حق اولاد
						item.ChildrensRightsMinistryLaborNew = EmpInfo.ChildrensRightsMinistryLaborNew;
						Console.WriteLine($":: Log Set ChildrensRightsMinistryLaborNew for item {item._Id} :: {item.ChildrensRightsMinistryLaborNew}");
						Ref_HR_CVR_RecruitmentRules_ChildrensRightsMinistryLaborNew.SetDisabled(true);

						// مزایای رفاهی انگیزه ای ماهانه
						item.WelfareMotivationalBenefitsNew = EmpInfo.WelfareMotivationalBenefitsNew;
						Console.WriteLine($":: Log Set WelfareMotivationalBenefitsNew for item {item._Id} :: {item.WelfareMotivationalBenefitsNew}");
						Ref_HR_CVR_RecruitmentRules_WelfareMotivationalBenefitsNew.SetDisabled(true);

						// حق تاهل
						item.RightMarryMinistryLaborNew = EmpInfo.RightMarryMinistryLaborNew;
						Console.WriteLine($":: Log Set RightMarryMinistryLaborNew for item {item._Id} :: {item.RightMarryMinistryLaborNew}");
						Ref_HR_CVR_RecruitmentRules_RightMarryMinistryLaborNew.SetDisabled(true);

						// سایر مزایا
						item.OtherBenefitsNew = EmpInfo.OtherBenefitsNew;
						Console.WriteLine($":: Log Set OtherBenefitsNew for item {item._Id} :: {item.OtherBenefitsNew}");

						// جمع کل دستمزد مزایایی قانونی و جمع مزد مبنا
						item.TotalMonthlySalaryBenefitsNew = EmpInfo.TotalMonthlySalaryBenefitsNew;
						Console.WriteLine($":: Log Set TotalMonthlySalaryBenefitsNew for item {item._Id} :: {item.TotalMonthlySalaryBenefitsNew}");
						Ref_HR_CVR_RecruitmentRules_TotalMonthlySalaryBenefitsNew.SetDisabled(true);
					}
				}

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

				decimal totalSum = rule?.TotalMonthlySalaryBenefitsNew ?? 0m;
				//decimal totalSum1 = previewList.Sum(x => x.Value ?? 0m);

				var totalRow = $@"
				<tr style='background:#fff3cd; font-weight:bold;'>
					<td style='padding:8px'>جمع کل</td>
					<td style='padding:8px; text-align:right; color:#b02a37'>
						{totalSum.ToString("N0")}
					</td>
				</tr>";

				string htmlString = $@"
				<div style='direction: rtl; font-family: tahoma;'>
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
			#endregion - 1
		}

		public async Task SP_Verdict_Json_onclick(MouseEventArgs Selected)
		{
			#region - 2 WITH JSON

			var VerdictDTO = new VerdictRequest()
			{
				// شناسه کارمند
				EmployeesId = Guid.Parse(employeeId),

				// مزد شغل
				JobSalaryRank = EmpInfo.JobSalaryRankNew,

				// مزد رتبه
				RankSalary = EmpInfo.RankSalaryNew,

				// مزد سنوات
				SalaryHistory = EmpInfo.SalaryHistoryNew,

				// حق پست
				RightGuardianship = EmpInfo.RightGuardianshipNew,

				// مزایای مانداگاری در پست
				CoefficientDurabilityPost = EmpInfo.CoefficientDurabilityPostNew,

				// شرایط نامساعد محیط کار
				CoefficientDifficultAndHarmfulJobs = EmpInfo.CoefficientDifficultAndHarmfulJobsNew,

				// جمع کل مزد مبنا
				TotalDailyBaseWage = EmpInfo.TotalDailyBaseWageNew,

				// تفاوت تطبیق روزانه
				DailyAdjustmentDifference = EmpInfo.DailyAdjustmentDifferenceNew,

				// حق جذب
				RecruitmentAllowance = EmpInfo.RecruitmentAllowanceNew,

				// کمک هزینه مسکن
				MinistryLabourRightHousing = EmpInfo.MinistryLabourRightHousingNew,

				// حق خوار و بار
				MinistryLaborRightFood = EmpInfo.MinistryLaborRightFoodNew,

				// حق اولاد
				ChildrensRightsMinistryLabor = EmpInfo.ChildrensRightsMinistryLaborNew,

				// مزایای رفاهی انگیزه ای ماهانه
				WelfareMotivationalBenefits = EmpInfo.WelfareMotivationalBenefitsNew,

				// حق تاهل
				RightMarryMinistryLabor = EmpInfo.RightMarryMinistryLaborNew,

				// این فیلد به استورد باید اضافه گردد
				// سایر مزایا
				OtherBenefits = EmpInfo.OtherBenefitsNew,

				// جمع کل دستمزد مزایایی قانونی و جمع مزد مبنا
				TotalMonthlySalaryBenefits = EmpInfo.TotalMonthlySalaryBenefitsNew,

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
				new StoredProcedureRequestDto
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
				var response2 = Newtonsoft.Json.JsonConvert.DeserializeObject<RootResponse>(jsonResponse2);

				if (response2?.DataSets == null || response2.DataSets.Count == 0)
				{
					await _MSG.ShowError("DataSets خالی است");
					return;
				}

				var employee2 = response2.DataSets[0][0];
				var A2 =
					employee2.RankSalaryNew;

				EmpInfo = employee2;

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

				string htmlString = $@"
				<div style='direction: rtl; font-family: tahoma;'>
					<table style='width:100%; border-collapse: collapse;' border='1'>
						<thead style='background:#f5f5f5'>
							<tr>
								<th style='padding:8px'>عنوان</th>
								<th style='padding:8px'>مبلغ (ریال)</th>
							</tr>
						</thead>
						<tbody>
							{rows}
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

			#endregion - 2
		}
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
			await Task.Yield();
			StateHasChanged();

			// برای اولین بار وقتی ردیفی در گرید وجود ندارد کلیه عدد های در تابع را 0.0 می کند
			if (_Entity.HR_CVR_RecruitmentRules.Count == 0)
			{
				await SetZiroLastVerdictRule();
			}

			try
			{
				Console.WriteLine(":: Log Grid EmpInfo :: " + await JSON.ToJson(EmpInfo));

				Console.WriteLine("EmpInfo is null? " + (EmpInfo == null));

				// **********************************************************************************************************

				// مزد شغل
				Item.JobSalaryRankNew = EmpInfo.JobSalaryRankNew;
				Console.WriteLine($":: Log Set JobSalaryRankNew for item {Item._Id} :: {Item.JobSalaryRankNew}");

				// مزد رتبه
				Item.RankSalaryNew = EmpInfo.RankSalaryNew;
				Console.WriteLine($":: Log Set RankSalaryNew for item {Item._Id} :: {Item.JobSalaryRankNew}");

				// مزد سنوات
				Item.SalaryHistoryNew = EmpInfo.SalaryHistoryNew;
				Console.WriteLine($":: Log Set SalaryHistoryNew for item {Item._Id} :: {Item.SalaryHistoryNew}");

				// حق پست
				Item.RightGuardianshipNew = EmpInfo.RightGuardianshipNew;
				Console.WriteLine($":: Log Set RightGuardianshipNew for item {Item._Id} :: {Item.RightGuardianshipNew}");

				// مزایای مانداگاری در پست
				Item.CoefficientDurabilityPostNew = EmpInfo.CoefficientDurabilityPostNew;
				Console.WriteLine($":: Log Set CoefficientDurabilityPostNew for item {Item._Id} :: {Item.CoefficientDurabilityPostNew}");

				// شرایط نامساعد محیط کار
				Item.CoefficientDifficultAndHarmfulJobsNew = EmpInfo.CoefficientDifficultAndHarmfulJobsNew;
				Console.WriteLine($":: Log Set CoefficientDifficultAndHarmfulJobsNew for item {Item._Id} :: {Item.CoefficientDifficultAndHarmfulJobsNew}");

				// جمع کل مزد مبنا
				Item.TotalDailyBaseWageNew = EmpInfo.TotalDailyBaseWageNew;
				Console.WriteLine($":: Log Set TotalDailyBaseWageNew for item {Item._Id} :: {Item.TotalDailyBaseWageNew}");
				if (await WaitComponentLoaded(Ref_HR_CVR_RecruitmentRules_TotalDailyBaseWageNew))
				{
					Ref_HR_CVR_RecruitmentRules_TotalDailyBaseWageNew.SetDisabled(true);
				}
				// تفاوت تطبیق روزانه
				Item.DailyAdjustmentDifferenceNew = EmpInfo.DailyAdjustmentDifferenceNew;
				Console.WriteLine($":: Log Set DailyAdjustmentDifferenceNew for item {Item._Id} :: {Item.DailyAdjustmentDifferenceNew}");

				// حق جذب
				Item.RecruitmentAllowanceNew = EmpInfo.RecruitmentAllowanceNew;
				Console.WriteLine($":: Log Set RecruitmentAllowanceNew for item {Item._Id} :: {Item.RecruitmentAllowanceNew}");

				// کمک هزینه مسکن
				Item.MinistryLabourRightHousingNew = EmpInfo.MinistryLabourRightHousingNew;
				Console.WriteLine($":: Log Set MinistryLabourRightHousingNew for item {Item._Id} :: {Item.MinistryLabourRightHousingNew}");
				if (await WaitComponentLoaded(Ref_HR_CVR_RecruitmentRules_MinistryLabourRightHousingNew))
				{
					Ref_HR_CVR_RecruitmentRules_MinistryLabourRightHousingNew.SetDisabled(true);
				}

				// حق خوار و بار
				Item.MinistryLaborRightFoodNew = EmpInfo.MinistryLaborRightFoodNew;
				Console.WriteLine($":: Log Set MinistryLaborRightFoodNew for item {Item._Id} :: {Item.MinistryLaborRightFoodNew}");
				if (await WaitComponentLoaded(Ref_HR_CVR_RecruitmentRules_MinistryLaborRightFoodNew))
				{
					Ref_HR_CVR_RecruitmentRules_MinistryLaborRightFoodNew.SetDisabled(true);
				}

				// حق اولاد
				Item.ChildrensRightsMinistryLaborNew = EmpInfo.ChildrensRightsMinistryLaborNew;
				Console.WriteLine($":: Log Set ChildrensRightsMinistryLaborNew for item {Item._Id} :: {Item.ChildrensRightsMinistryLaborNew}");
				if (await WaitComponentLoaded(Ref_HR_CVR_RecruitmentRules_ChildrensRightsMinistryLaborNew))
				{
					Ref_HR_CVR_RecruitmentRules_ChildrensRightsMinistryLaborNew.SetDisabled(true);
				}

				// مزایای رفاهی انگیزه ای ماهانه
				Item.WelfareMotivationalBenefitsNew = EmpInfo.WelfareMotivationalBenefitsNew;
				Console.WriteLine($":: Log Set WelfareMotivationalBenefitsNew for item {Item._Id} :: {Item.WelfareMotivationalBenefitsNew}");
				if (await WaitComponentLoaded(Ref_HR_CVR_RecruitmentRules_WelfareMotivationalBenefitsNew))
				{
					Ref_HR_CVR_RecruitmentRules_WelfareMotivationalBenefitsNew.SetDisabled(true);
				}

				// حق تاهل
				Item.RightMarryMinistryLaborNew = EmpInfo.RightMarryMinistryLaborNew;
				Console.WriteLine($":: Log Set RightMarryMinistryLaborNew for item {Item._Id} :: {Item.RightMarryMinistryLaborNew}");
				if (await WaitComponentLoaded(Ref_HR_CVR_RecruitmentRules_RightMarryMinistryLaborNew))
				{
					Ref_HR_CVR_RecruitmentRules_RightMarryMinistryLaborNew.SetDisabled(true);
				}

				// سایر مزایا
				Item.OtherBenefitsNew = EmpInfo.OtherBenefitsNew;
				Console.WriteLine($":: Log Set OtherBenefitsNew for item {Item._Id} :: {Item.OtherBenefitsNew}");

				// جمع کل دستمزد مزایایی قانونی و جمع مزد مبنا
				Item.TotalMonthlySalaryBenefitsNew = EmpInfo.TotalMonthlySalaryBenefitsNew;
				Console.WriteLine($":: Log Set TotalMonthlySalaryBenefitsNew for item {Item._Id} :: {Item.TotalMonthlySalaryBenefitsNew}");
				if (await WaitComponentLoaded(Ref_HR_CVR_RecruitmentRules_TotalMonthlySalaryBenefitsNew))
				{
					Ref_HR_CVR_RecruitmentRules_TotalMonthlySalaryBenefitsNew.SetDisabled(true);
				}

				// تکمیل فیلدهای گرید ::
				await GetEmpDataGrid(Item);


				// **********************************

				// اگر HR_CVR_ApprovalsMinistryLaborGroupId پر باشد، مقادیر مربوطه را اعمال کن
				if (Item.HR_CVR_ApprovalsMinistryLaborGroupId.HasValue)
				{
					await EMP_Data.EmployeeData.ApplyToLastRecruitmentRules(Item, _User.UserID.ToString());
				}

				StateHasChanged();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"💥 Exception in Grid => AfterRender Error :: {ex}");
			}

		}


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

		/// <summary>
		/// پر کردن فیلد های گرید محاسبات حکم از جدول مجاز و یا SP
		/// </summary>
		/// <param name="Item"></param>
		/// <returns></returns>
		private async Task GetEmpDataGrid(Entity.HR_CVR_RecruitmentRules Item)
		{
			string detailEmployeeId = employeeId;

			Console.WriteLine($"🔍 Fetching employee data for ID: {detailEmployeeId}");

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
			Ref_HR_CVR_RecruitmentRules_EmployeeNo.SetDisabled(true);
			Ref_HR_CVR_RecruitmentRules_FirstName.Value = emp_data.FirstName;
			Ref_HR_CVR_RecruitmentRules_LastName.Value = emp_data.LastName;
			Ref_HR_CVR_RecruitmentRules_FatherName.Value = emp_data.FatherName;
			Ref_HR_CVR_RecruitmentRules_NationalCode.Value = emp_data.NationalCode;
			Ref_HR_CVR_RecruitmentRules_IdCardNo.Value = emp_data.IdCardNo;
			Ref_HR_CVR_RecruitmentRules_BirthDate_Fa.Value = emp_data.BirthDate_Fa;
			Ref_HR_CVR_RecruitmentRules_BaseInfo_GenderId.Value = emp_data.BaseInfo_GenderTitle;
			Ref_HR_CVR_RecruitmentRules_BaseInfo_MaritalStatusId.Value = emp_data.BaseInfo_MaritalStatusTitle;
			Ref_HR_CVR_RecruitmentRules_EmployeeAgeText.Value = emp_data.EmployeeAgeText;
			Ref_HR_CVR_RecruitmentRules_CityOfIssue.Value = emp_data.CityOfIssueTitle;
			Ref_HR_CVR_RecruitmentRules_CityOfBirth.Value = emp_data.CityOfBirthTitle;
			Ref_HR_CVR_RecruitmentRules_EmploymentDateInGroup_Fa.Value = emp_data.EmploymentDateInGroup_Fa;
			//Ref_HR_CVR_RecruitmentRules_DailyEmploymentDateInGroup.Value = emp_data.DailyEmploymentDateInGroup; 
			Ref_HR_CVR_RecruitmentRules_EmploymentDate_Fa.Value = emp_data.EmploymentDate_Fa;
			//Ref_HR_CVR_RecruitmentRules_DailyEmploymentDate.Value = emp_data.DailyEmploymentDate; 
			Ref_HR_CVR_RecruitmentRules_EmploymentStartDate_Fa.Value = emp_data.EmploymentStartDate_Fa;

			//// اطلاعات حساب بانکی
			//// HR_Base_AcademicDegrees
			//Ref_HR_CVR_RecruitmentRules_BankAccountNumber.Value = empBnakAcc.BankAccountNumber;
			//Ref_HR_CVR_RecruitmentRules_IBAN.Value = empBnakAcc.IBAN;

			//// مدرک تحصیلی
			////Ref_HR_CVR_RecruitmentRules_HR_Base_AcademicDegreesId.Value = emp_data.HR_Base_AcademicDegreesTitle; // مدرک تحصیلی اصلا برای این ویو نیست
			StateHasChanged();
		}


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

			decimal total = 0m;

			total += Item.JobSalaryRank ?? 0m;
			total += Item.RankSalary ?? 0m;
			total += Item.SalaryHistory ?? 0m;
			total += Item.RightGuardianship ?? 0m;
			total += Item.CoefficientDurabilityPost ?? 0m;
			total += Item.CoefficientDifficultAndHarmfulJobs ?? 0m;
			total += Item.TotalDailyBaseWage ?? 0m;
			total += Item.DailyAdjustmentDifference ?? 0m;
			total += Item.RecruitmentAllowance ?? 0m;
			total += Item.MinistryLabourRightHousing ?? 0m;
			total += Item.MinistryLaborRightFood ?? 0m;
			total += Item.ChildrensRightsMinistryLabor ?? 0m;
			total += Item.WelfareMotivationalBenefits ?? 0m;
			total += Item.RightMarryMinistryLabor ?? 0m;
			total += Item.OtherBenefits ?? 0m;

			Item.TotalDailyBaseWage = total;
			//Item.TotalDailyBaseWage = Convert.ToInt32(total);
		}

		#endregion

		#endregion FunctionEvents

	}
}


// **************************************************


public class SalaryPreviewItem
{
	public string Title { get; set; }
	public decimal? Value { get; set; }
}

#region EMP_Data
namespace EMP_Data
{
	public static class EmployeeData
	{
		#region EmployeeMasterDetail
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

			var TablePost = new Baya.Models.ORM.Table
			{
				Name = "HR_EMP_Employees_EmployeeInfos",
				Column = new List<Coulmn>
				{
					new Coulmn { Name = "Id", NameAs = "Id" }, // شناسه رکورد (GUID)
					//new Coulmn { Name = "HR_EMP_EmployeesId", NameAs = "HR_EMP_EmployeesId" }, // شناسه اطلاعات کارمند (مرتبط با جدول اصلی HR_EMP_Employees)
					new Coulmn { Name = "EmployeeNo", NameAs = "EmployeeNo" }, // کد کارمندی
					new Coulmn { Name = "LastEmployeeNO", NameAs = "LastEmployeeNO" }, // آخرین کد کارمندی
					new Coulmn { Name = "EmployeePersonelNo", NameAs = "EmployeePersonelNo" }, // شماره پرسنلی کارمند
					new Coulmn { Name = "EmployeeLastPersonelNo", NameAs = "EmployeeLastPersonelNo" }, // آخرین شماره پرسنلی کارمند
					new Coulmn { Name = "FirstName", NameAs = "FirstName" }, // نام
					new Coulmn { Name = "LastName", NameAs = "LastName" }, // نام خانوادگی
					new Coulmn { Name = "FatherName", NameAs = "FatherName" }, // نام پدر
					new Coulmn { Name = "NationalCode", NameAs = "NationalCode" }, // کد ملی
					new Coulmn { Name = "IdCardNo", NameAs = "IdCardNo" }, // شماره شناسنامه
					//new Coulmn { Name = "BirthDate_Fa", NameAs = "BirthDate_Fa" }, // تاریخ تولد (شمسی به صورت رشته)
					//new Coulmn { Name = "BirthDate", NameAs = "BirthDate" }, // تاریخ تولد (میلادی)
					//new Coulmn { Name = "BirthDateDD", NameAs = "BirthDateDD" }, // روز تاریخ تولد
					//new Coulmn { Name = "BirthDateMM", NameAs = "BirthDateMM" }, // ماه تاریخ تولد
					//new Coulmn { Name = "BirthDateYYYY", NameAs = "BirthDateYYYY" }, // سال تاریخ تولد
					//new Coulmn { Name = "CityOfBirth", NameAs = "CityOfBirth" }, // شناسه شهر محل تولد
					//new Coulmn { Name = "CityOfBirthTitle", NameAs = "CityOfBirthTitle" }, // نام شهر محل تولد
					//new Coulmn { Name = "CityOfIssue", NameAs = "CityOfIssue" }, // شناسه شهر محل صدور
					//new Coulmn { Name = "CityIssueTitle", NameAs = "CityIssueTitle" }, // نام شهر محل صدور
					//new Coulmn { Name = "Address", NameAs = "Address" }, // نشانی
					//new Coulmn { Name = "Mobile", NameAs = "Mobile" }, // تلفن همراه
					//new Coulmn { Name = "Phone", NameAs = "Phone" }, // شماره تلفن ثابت
					//new Coulmn { Name = "EmploymentDate_Fa", NameAs = "EmploymentDate_Fa" }, // تاریخ استخدام (شمسی)
					//new Coulmn { Name = "EmploymentStartDate_Fa", NameAs = "EmploymentStartDate_Fa" }, // تاریخ آخرین تسویه حساب (شمسی)
					//new Coulmn { Name = "EmploymentDateInGroup_Fa", NameAs = "EmploymentDateInGroup_Fa" }, // تاریخ استخدام در گروه (شمسی)
					//new Coulmn { Name = "EmploymentDate", NameAs = "EmploymentDate" }, // تاریخ استخدام (میلادی)
					//new Coulmn { Name = "EmploymentStartDate", NameAs = "EmploymentStartDate" }, // تاریخ آخرین تسویه حساب (میلادی)
					//new Coulmn { Name = "EmploymentDateInGroup", NameAs = "EmploymentDateInGroup" }, // تاریخ استخدام در گروه (میلادی)
					//new Coulmn { Name = "EmployeeAge", NameAs = "EmployeeAge" }, // سن کارمند (به روز)
					//new Coulmn { Name = "EmployeeAgeText", NameAs = "EmployeeAgeText" }, // سن کارمند (به صورت متن، مثلاً "13593سال")
					//new Coulmn { Name = "EmployeeWorkExperienceText", NameAs = "EmployeeWorkExperienceText" }, // سابقه کار کارمند (به روز)
					//new Coulmn { Name = "BaseInfo_MaritalStatusId", NameAs = "BaseInfo_MaritalStatusId" }, // شناسه وضعیت تاهل
					//new Coulmn { Name = "BaseInfo_MaritalStatusTitle", NameAs = "BaseInfo_MaritalStatusTitle" }, // عنوان وضعیت تاهل
					//new Coulmn { Name = "BaseInfo_GenderId", NameAs = "BaseInfo_GenderId" }, // شناسه جنسیت
					//new Coulmn { Name = "BaseInfo_GenderTitle", NameAs = "BaseInfo_GenderTitle" }, // عنوان جنسیت
					//new Coulmn { Name = "BaseInfo_MilitaryStatusId", NameAs = "BaseInfo_MilitaryStatusId" }, // شناسه وضعیت نظام وظیفه
					//new Coulmn { Name = "BaseInfo_MilitaryStatusTitle", NameAs = "BaseInfo_MilitaryStatusTitle" }, // عنوان وضعیت نظام وظیفه
					//new Coulmn { Name = "BaseInfo_CitiesAreasId", NameAs = "BaseInfo_CitiesAreasId" }, // شناسه منطقه شهری
					//new Coulmn { Name = "BaseInfo_CitiesAreasTitle", NameAs = "BaseInfo_CitiesAreasTitle" }, // عنوان منطقه شهری
					//new Coulmn { Name = "BaseInfo_ORG_CompaniesId", NameAs = "BaseInfo_ORG_CompaniesId" }, // شناسه شرکت
					//new Coulmn { Name = "BaseInfo_ORG_CompaniesTitle", NameAs = "BaseInfo_ORG_CompaniesTitle" }, // نام شرکت
					//new Coulmn { Name = "HR_EMP_StatusId", NameAs = "HR_EMP_StatusId" }, // شناسه وضعیت کارمند
					//new Coulmn { Name = "HR_EMP_StatusTitle", NameAs = "HR_EMP_StatusTitle" }, // عنوان وضعیت کارمند
					//new Coulmn { Name = "HR_Base_TransportServiceId", NameAs = "HR_Base_TransportServiceId" }, // شناسه خدمات حمل و نقل
					//new Coulmn { Name = "HR_Base_TransportServiceTitle", NameAs = "HR_Base_TransportServiceTitle" }, // عنوان خدمات حمل و نقل
					//new Coulmn { Name = "HasTransportService", NameAs = "HasTransportService" }, // سرویس ایاب ذهاب دارد؟
					//new Coulmn { Name = "SupplementaryInsurance", NameAs = "SupplementaryInsurance" }, // بیمه تکمیلی
					//new Coulmn { Name = "lifeInsurance", NameAs = "lifeInsurance" }, // بیمه عمر
					//new Coulmn { Name = "AccidentInsurance", NameAs = "AccidentInsurance" }, // بیمه حوادث
					//new Coulmn { Name = "Arzagh", NameAs = "Arzagh" }, // ارزاق دارد؟
					//new Coulmn { Name = "HasDisabledChild", NameAs = "HasDisabledChild" }, // فرزند معلول دارد؟
					//new Coulmn { Name = "MartyrsFamily", NameAs = "MartyrsFamily" }, // خانواده شهدا
					//new Coulmn { Name = "MartyrsChild", NameAs = "MartyrsChild" }, // فرزند شهید
					//new Coulmn { Name = "JebheMotanaveb_Days", NameAs = "JebheMotanaveb_Days" }, // مدت جبهه متناوب (روز)
					//new Coulmn { Name = "JebheMotavali_Days", NameAs = "JebheMotavali_Days" }, // مدت جبهه متوالی (روز)
					//new Coulmn { Name = "Captivity_Days", NameAs = "Captivity_Days" }, // مدت اسارت (روز)
					//new Coulmn { Name = "Relatives_Captivity_Days", NameAs = "Relatives_Captivity_Days" }, // مدت اسارت فرد مرتبط (روز)
					//new Coulmn { Name = "Relatives_Jebhe_Days", NameAs = "Relatives_Jebhe_Days" }, // مدت جبهه فرد مرتبط (روز)
					//new Coulmn { Name = "Relatives_VeteranPercentage", NameAs = "Relatives_VeteranPercentage" }, // درصد جانبازی فرد مرتبط
					//new Coulmn { Name = "VeteranPercentage", NameAs = "VeteranPercentage" }, // درصد جانبازی کارمند
					//new Coulmn { Name = "SanavatEnteghali_Day", NameAs = "SanavatEnteghali_Day" }, // سنوات انتقالی (روز)
					//new Coulmn { Name = "IsActive", NameAs = "IsActive" }, // فعال
					//new Coulmn { Name = "FullName", NameAs = "FullName" }, // نام کامل
					////new Coulmn { Name = "UserId", NameAs = "UserId" }, // شناسه کاربر سیستمی
					//// فیلدهای ContactEmployee
					//new Coulmn { Name = "ContactEmployee_FullName", NameAs = "ContactEmployee_FullName" }, // نام و نام خانوادگی فرد مرتبط
					//new Coulmn { Name = "ContactEmployee_Address", NameAs = "ContactEmployee_Address" }, // نشانی فرد مرتبط
					//new Coulmn { Name = "ContactEmployee_Tel", NameAs = "ContactEmployee_Tel" }, // شماره تلفن ضروری فرد مرتبط
					//// فیلدهای نسبت به سایر جداول
					//new Coulmn { Name = "HR_Base_ContactEmployeeRelativeId", NameAs = "HR_Base_ContactEmployeeRelativeId" }, // شناسه نسبت فرد با کارمند
					//new Coulmn { Name = "HR_Base_ContactEmployeeRelativeTitle", NameAs = "HR_Base_ContactEmployeeRelativeTitle" }, // عنوان نسبت فرد با کارمند
					//// فیلدهای سیستمی
					////new Coulmn { Name = "CreateDate", NameAs = "CreateDate" }, // تاریخ ایجاد رکورد
					////new Coulmn { Name = "UpdateDate", NameAs = "UpdateDate" }, // تاریخ آخرین ویرایش رکورد
					////new Coulmn { Name = "CreateUser", NameAs = "CreateUser" }, // شناسه کاربر ایجادکننده
					////new Coulmn { Name = "UpdateUser", NameAs = "UpdateUser" }, // شناسه کاربر ویرایش‌کننده
					////new Coulmn { Name = "IsDelete", NameAs = "IsDelete" }, // حذف منطقی
					////new Coulmn { Name = "RequestID", NameAs = "RequestID" }, // شناسه درخواست
				}
			};

			var NewQuery = new QueryBuilderFilterRule { Condition = "AND" };
			NewQuery.Rules = new List<QueryBuilderFilterRule>
			{
				new QueryBuilderFilterRule
				{
					Id = "Id", // شناسه اصلی کارمند
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
		#endregion

		#region LastVerdictEmp
		/// <summary>
		/// جدول و نمایش آخرین حکم کارمند
		/// </summary>
		/// <param name="employeeId"></param>
		/// <param name="_UserId"></param>
		/// <returns></returns>
		public static async Task<Entity.View_HR_CVR_VerdictRecruiting> LastVerdictEmp(string employeeId, string _UserId)
		{
			if (string.IsNullOrEmpty(employeeId))
			{
				Console.WriteLine("❌ Employee ID is null or empty in LastVerdictEmp");
				return null;
			}

			// دریافت آخرین حکم فعال کارمند
			var TablePost = new Baya.Models.ORM.Table
			{
				Name = "View_HR_CVR_VerdictRecruiting",
				Column = new List<Coulmn>
				{
					new Coulmn { Name = "Id", NameAs = "Id" },
					new Coulmn { Name = "ExecutionDateSentence", NameAs = "ExecutionDateSentence" }, // برای مرتب‌سازی
					new Coulmn { Name = "ExecutionDateSentence_Fa", NameAs = "ExecutionDateSentence_Fa" },
					new Coulmn { Name = "HR_EMP_EmployeesId", NameAs = "HR_EMP_EmployeesId" },
					new Coulmn { Name = "HR_StatusVerdictRecruitingId", NameAs = "HR_StatusVerdictRecruitingId" }
				}
			};

			var NewQuery = new QueryBuilderFilterRule { Condition = "AND" };
			NewQuery.Rules = new List<QueryBuilderFilterRule>
			{
				new QueryBuilderFilterRule
				{
					//Id = "HR_EMP_EmployeesId", 
					Field = "HR_EMP_EmployeesId",// شناسه اصلی کارمند
					Input = "text",
					Operator = "equal",
					Type = "string",
					Value = new string[] { employeeId }
				},
				new QueryBuilderFilterRule
				{
					Field = "HR_StatusVerdictRecruitingId",
					Operator = "equal",
					Type = "string",
					Value = new[] { "0DA9356F-211D-F011-A502-005056A2B6BD" } // وضعیت "فعال"
				}
			};

			//var Model = await ApiServer.External.Services.Data.GetList(TablePost, NewQuery, "View_HR_CVR_VerdictRecruiting", _UserId);
			var Model = await ApiServer.External.Services.Data.GetList(
				Entity: "View_HR_CVR_VerdictRecruiting",
				limit: null,
				skip: null,
				include: TablePost,
				Filter: NewQuery
			);

			if (Model?.Status != HttpStatusCode.OK || string.IsNullOrEmpty(Model.Content?.ToString()))
			{
				Console.WriteLine($"❌ No active verdict found for employee ID: {employeeId}");
				return null;
			}


			if (string.IsNullOrEmpty(Model.Content?.ToString()))
			{
				Console.WriteLine($"❌ API returned empty content for ID: {employeeId}");
				return null;
			}

			try
			{
				// ⚠️ تجزیه به LIST چون چندین حکم ممکن است وجود داشته باشد
				var verdicts = await JSON.ToObject<List<Entity.View_HR_CVR_VerdictRecruiting>>(Model.Content.ToString());

				// مرتب‌سازی بر اساس تاریخ اجرای حکم (نزولی) و انتخاب اولین مورد = آخرین حکم
				var latest = verdicts
					.Where(v => v.ExecutionDateSentence.HasValue)
					.OrderByDescending(v => v.ExecutionDateSentence.Value)
					.FirstOrDefault();

				if (latest == null)
				{
					Console.WriteLine($"⚠️ No verdict with valid date found for employee ID: {employeeId}");
				}

				return latest;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"💥 Deserialize error in LastVerdictEmp for employee ID {employeeId}: {ex.Message}");
				return null;
			}
		}
		#endregion

		#region ApplyToLastRecruitmentRules
		/// <summary>
		/// پر کردن فیلدهای مربوطه در HR_CVR_RecruitmentRules با مقادیر مصوبات جاری
		/// بر اساس HR_CVR_ApprovalsMinistryLaborGroupId
		/// </summary>
		public static async Task ApplyToLastRecruitmentRules(HR_CVR_RecruitmentRules item, string userId)
		{
			if (item == null || item.HR_CVR_ApprovalsMinistryLaborGroupId == null)
			{
				Console.WriteLine("⚠️ HR_CVR_ApprovalsMinistryLaborGroupId خالی است یا آیتم نامعتبر است.");
				return;
			}

			var approvalId = item.HR_CVR_ApprovalsMinistryLaborGroupId.Value;

			// ساخت Table برای دریافت فقط فیلدهای مورد نیاز
			var TablePost = new Baya.Models.ORM.Table
			{
				Name = "HR_CVR_ApprovalsMinistryLaborGroup",
				Column = new List<Coulmn>
				{
					new Coulmn { Name = "Id", NameAs = "Id" },
					new Coulmn { Name = "MinistryLabourRightHousing", NameAs = "MinistryLabourRightHousing" },
					new Coulmn { Name = "MinistryLaborRightFood", NameAs = "MinistryLaborRightFood" },
					new Coulmn { Name = "BenKargariMinistryLabor", NameAs = "BenKargariMinistryLabor" },
					new Coulmn { Name = "RightMarryMinistryLabor", NameAs = "RightMarryMinistryLabor" },
					new Coulmn { Name = "ChildrensRightsMinistryLabor", NameAs = "ChildrensRightsMinistryLabor" }
				}
			};

			// فیلتر: فقط رکوردی که Id برابر با HR_CVR_ApprovalsMinistryLaborGroupId باشد
			var NewQuery = new QueryBuilderFilterRule
			{
				Condition = "AND",
				Rules = new List<QueryBuilderFilterRule>
				{
					new QueryBuilderFilterRule
					{
						Field = "Id",
						Operator = "equal",
						Type = "string",
						Value = new[] { approvalId.ToString() }
					}
				}
			};

			// فراخوانی API
			var Model = await ApiServer.External.Services.Data.GetList(
				Entity: "HR_CVR_ApprovalsMinistryLaborGroup",
				limit: null,
				skip: null,
				include: TablePost,
				Filter: NewQuery
			);

			if (Model?.Status == HttpStatusCode.OK && !string.IsNullOrEmpty(Model.Content?.ToString()))
			{
				try
				{
					var approvals = await JSON.ToObject<List<HR_CVR_ApprovalsMinistryLaborGroup>>(Model.Content.ToString());
					var approval = approvals?.FirstOrDefault();

					if (approval != null)
					{
						// 1. کمک هزینه مسکن
						item.MinistryLabourRightHousing = approval.MinistryLabourRightHousing;

						// 2. حق خوار و بار
						item.MinistryLaborRightFood = approval.MinistryLaborRightFood;

						// 3. مزایای رفاهی و انگیزه‌ای = بن کارگری
						item.WelfareMotivationalBenefits = approval.BenKargariMinistryLabor;

						// 4. حق تاهل
						item.RightMarryMinistryLabor = approval.RightMarryMinistryLabor;

						// [اختیاری] حق اولاد
						// item.ChildrensRightsMinistryLabor = approval.ChildrensRightsMinistryLabor;

						Console.WriteLine($"✅ مقادیر مصوبات با شناسه {approvalId} به آیتم اعمال شد.");
					}
					else
					{
						Console.WriteLine($"❌ هیچ مصوبه‌ای با شناسه {approvalId} یافت نشد.");
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"💥 خطا در اعمال مصوبات: {ex.Message}");
				}
			}
			else
			{
				Console.WriteLine($"❌ خطا در دریافت مصوبه با شناسه {approvalId}: {Model?.Status}");
			}
		}
		#endregion
	}
}
#endregion EMP_Data