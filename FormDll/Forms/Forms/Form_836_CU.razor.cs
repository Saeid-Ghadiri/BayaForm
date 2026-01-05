using Baya.Models.Utility;
using DevExpress.Blazor;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Sitko.Blazor.CKEditor;
using System;
using System.Net;

namespace Forms.Forms
{
	public class Form_836_CUBase : Form_836_CUPeropeties
	{



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
				//    Save & New Button: id615233cc-f2bb-4bc5-857d-7446472d5336
				await JS.InvokeVoidAsync("AddClass", "#btn_listi_Previous", "d-none");
				// Before Button: id0bc18b67-d8a3-4c39-820d-99bc5a6e0845
				await JS.InvokeVoidAsync("AddClass", "#btn_listi_Next", "d-none");
				//  Next Button: id1ddd0158-e04c-4582-adc1-2e20f12be8ab
				//  await JS.InvokeVoidAsync("AddClass", "#id1ddd0158-e04c-4582-adc1-2e20f12be8ab", "d-none");
			}
		}

		/// <summary>
		/// اعتبار سنجی فرم
		/// </summary>
		/// <returns></returns>
		public override async Task<bool> FormValidator()
		{
			bool IsValid = true;

			// if (_Entity.ReqCount != 5)
			// {
			//     IsValid = false;
			//     SumaryMessage += "تعداد درخواست مخالف 5 باشد";
			// }

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
			Console.WriteLine($"[DEBUG] AfterGetData: _Entity?.BaseInfo_MaritalStatusId = {_Entity?.BaseInfo_MaritalStatusId}");
			// منطق وضعیت تاهل
			await UpdateMaritalDateVisibility(_Entity?.BaseInfo_MaritalStatusId.ToString());

			// منطق بیمه تکمیلی (اگر پیاده‌سازی کرده‌اید)
			bool hasInsurance = _Entity?.SupplementaryInsurance == true;
			await SetSupplementaryInsuranceDateVisibility(hasInsurance);

			// وضعیت تحصیل
			bool isCurrentlyStudying = _Entity?.IsCurrentlyStudying == true;
			await SetStudyFieldsVisibility(isCurrentlyStudying);
		}


		#region FunctionEvents

		public async Task HR_EMP_EmployeesId_onitemselected(Entity.HR_EMP_Employees Selected)
		{
		}

		private static readonly Guid MarriedId = Guid.Parse("c1946562-1374-f011-a509-005056a2b6bd");
		private static readonly Guid DivorcedId = Guid.Parse("0d4e1f70-a8c6-f011-a50e-005056a2b6bd");
		private static readonly Guid WidowedId = Guid.Parse("0e4e1f70-a8c6-f011-a50e-005056a2b6bd");
		// اگر نیاز بود:
		// private static readonly Guid SingleId = Guid.Parse("c0946562-1374-f011-a509-005056a2b6bd");

		//public async Task BaseInfo_MaritalStatusId_onitemselected(Entity.BaseInfo_MaritalStatus Selected)
		public async Task BaseInfo_MaritalStatusId_onitemselected(dynamic Selected)
		{
			// BaseInfo_MaritalStatusId
			// این فیلد دراپ دان وضعیت تاهل هست که جی یو ای دی های هر وضیعت رو هم ارسال کردم بر اساس هر وضعیت میخوام نمایش داده شود

			// SELECT Id, Code, Title FROM BaseInfo_MaritalStatus :

			// Id									  Code    Title
			// c0946562-1374-f011-a509-005056a2b6bd    1		مجرد
			// c1946562-1374-f011-a509-005056a2b6bd    2		متاهل
			// 0d4e1f70-a8c6-f011-a50e-005056a2b6bd    3		مطلقه
			// 0e4e1f70-a8c6-f011-a50e-005056a2b6bd    4		همسر فوت شده

			// **************************************************

			// MarriageDate_Fa - تاریخ ازدواج
			// DivorceDate_Fa - تاریخ طلاق
			// SpouseDeathDate_Fa - تاریخ فوت همسر

			// -- اگر متأهل است، باید تاریخ ازدواج داشته باشد
			// -- اگر مطلقه است، باید تاریخ ازدواج و طلاق داشته باشد
			// -- اگر همسر فوت شده، باید تاریخ ازدواج و فوت همسر داشته باشد
			// -- در صورتی که Dropdown null است هیچ تاریخی از تاریخ ها بالا نمایش داده نشود


			//Console.WriteLine($"[DEBUG] BaseInfo_MaritalStatusId_onitemselected called.");
			//Console.WriteLine($"[DEBUG] Selected is null: {Selected == null}");

			var id = Selected.Id;

			//Console.WriteLine($"[DEBUG] Extracted marital status ID: {id}");
			//await UpdateMaritalDateVisibility(id);

			if (id == null)
			{
				Console.WriteLine($"[DEBUG] maritalStatusId Is Null: ");
				//return;
			}

			//Console.WriteLine($"[DEBUG] UpdateMaritalDateVisibility called with ID: {id}");

			if (id == "c1946562-1374-f011-a509-005056a2b6bd")
			{
				//Console.WriteLine("[DEBUG] Showing marriage date only.");
				await MaritalDateVisible(true, false, false);
			}
			else if (id == "0d4e1f70-a8c6-f011-a50e-005056a2b6bd")
			{
				//Console.WriteLine("[DEBUG] Showing marriage + divorce dates.");
				await MaritalDateVisible(true, true, false);
			}
			else if (id == "0e4e1f70-a8c6-f011-a50e-005056a2b6bd")
			{
				//Console.WriteLine("[DEBUG] Showing marriage + spouse death dates.");
				await MaritalDateVisible(true, false, true);
			}
			else
			{
				//Console.WriteLine($"[DEBUG] Hiding all date fields (ID: {id}).");
				await MaritalDateVisible(false, false, false);
			}
			StateHasChanged();
		}

		private async Task MaritalDateVisible(bool showMarriage, bool showDivorce, bool showSpouseDeath)
		{
			// برای اطمینان از رندر شدن کامپوننت‌ها
			//await Task.Delay(50);

			Ref_MarriageDate_Fa?.SetVisible(showMarriage);
			Ref_DivorceDate_Fa?.SetVisible(showDivorce);
			Ref_SpouseDeathDate_Fa?.SetVisible(showSpouseDeath);

			// پاک کردن مقادیر وقتی فیلد مخفی است
			if (!showMarriage) _Entity.MarriageDate_Fa = null;
			if (!showDivorce) _Entity.DivorceDate_Fa = null;
			if (!showSpouseDeath) _Entity.SpouseDeathDate_Fa = null;

			StateHasChanged();
		}

		private async Task UpdateMaritalDateVisibility(string? maritalStatusId)
		{
			if (maritalStatusId == null)
			{
				//Console.WriteLine($"[DEBUG] maritalStatusId Is Null: ");
				//return;
			}

			//Console.WriteLine($"[DEBUG] UpdateMaritalDateVisibility called with ID: {maritalStatusId}");

			if (maritalStatusId == "c1946562-1374-f011-a509-005056a2b6bd")
			{
				//Console.WriteLine("[DEBUG] Showing marriage date only.");
				await MaritalDateVisible(true, false, false);
			}
			else if (maritalStatusId == "0d4e1f70-a8c6-f011-a50e-005056a2b6bd")
			{
				//Console.WriteLine("[DEBUG] Showing marriage + divorce dates.");
				await MaritalDateVisible(true, true, false);
			}
			else if (maritalStatusId == "0e4e1f70-a8c6-f011-a50e-005056a2b6bd")
			{
				//Console.WriteLine("[DEBUG] Showing marriage + spouse death dates.");
				await MaritalDateVisible(true, false, true);
			}
			else
			{
				//Console.WriteLine($"[DEBUG] Hiding all date fields (ID: {maritalStatusId}).");
				await MaritalDateVisible(false, false, false);
			}
			StateHasChanged();
		}

		public async Task SupplementaryInsurance_oninput(ChangeEventArgs Selected)
		{
			//  بیمه تکمیلی دارد؟
			// SupplementaryInsurance - Boolean

			// تبدیل مقدار انتخابی به bool
			bool hasSupplementaryInsurance = Selected?.Value?.ToString().Equals("true", StringComparison.OrdinalIgnoreCase) == true;

			// نمایش یا مخفی‌کردن فیلدهای تاریخ
			await SetSupplementaryInsuranceDateVisibility(hasSupplementaryInsurance);
		}

		private async Task SetSupplementaryInsuranceDateVisibility(bool isVisible)
		{
			// نمایش/مخفی‌کردن فیلدها
			Ref_SupplementaryInsuranceStartDate_Fa?.SetVisible(isVisible);
			Ref_SupplementaryInsuranceEndDate_Fa?.SetVisible(isVisible);

			// اگر فیلدها مخفی شدند، مقادیرشان را پاک کن (اختیاری اما توصیه‌شده)
			if (!isVisible)
			{
				_Entity.SupplementaryInsuranceStartDate_Fa = null;
				_Entity.SupplementaryInsuranceEndDate_Fa = null;
			}
		}

		public async Task IsCurrentlyStudying_oninput(ChangeEventArgs Selected)
		{
			//  در حال حاضر محصّل است؟
			// IsCurrentlyStudying - Boolean

			// تبدیل مقدار به bool (پشتیبانی از "true" / "false" به هر شکلی)
			string value = Selected?.Value?.ToString();
			bool isCurrentlyStudying = string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);

			// بروزرسانی نمایش فیلدهای مربوط به تحصیل
			await SetStudyFieldsVisibility(isCurrentlyStudying);
		}

		private async Task SetStudyFieldsVisibility(bool isVisible)
		{
			// نمایش یا مخفی‌کردن سه فیلد
			Ref_EducationStartDate_Fa?.SetVisible(isVisible);
			Ref_EducationEndDate_Fa?.SetVisible(isVisible);
			Ref_StudyCertificateFile?.SetVisible(isVisible);

			// اختیاری: پاک کردن مقادیر اگر فیلدها مخفی شدند
			if (!isVisible)
			{
				_Entity.EducationStartDate_Fa = null;
				_Entity.EducationEndDate_Fa = null;
				_Entity.StudyCertificateFile = null; // فایل را هم null می‌کند
			}
		}

		#endregion FunctionEvents

	}
}