using Baya.Models.ORM;
using Baya.Models.Utility;
using Castle.DynamicLinqQueryBuilder;
using DevExpress.Blazor;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Sitko.Blazor.CKEditor;
using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Forms.Forms
{
	public class Form_688_CUBase : Form_688_CUPeropeties
	{

		// تابع پیام تٌــست
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


				if (!_Entity.IsActive.HasValue)
				{
					_Entity.IsActive = true;
					Ref_IsActive.AddAttribute("checked", "checked");
				}

				AddIR();

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

			// if (_Entity.ReqCount != 5)
			// {
			//     IsValid = false;
			//     SumaryMessage += "تعداد درخواست مخالف 5 باشد";
			// }

			return IsValid;
		}

		/// <summary>
		/// ارسال داده
		/// </summary>
		/// <returns></returns>
		public override async Task<Baya.Models.Utility.Result> Submit()
		{
			SumaryMessage = "";
			Baya.Models.Utility.Result Result = new Baya.Models.Utility.Result();

			if (!await FormValidator())
			{
				StateHasChanged();
				Result.Status = HttpStatusCode.InternalServerError;
				return Result;
			}

			if ((await BeforSubmit()).Status != HttpStatusCode.OK)
			{
				StateHasChanged();
				Result.Status = HttpStatusCode.InternalServerError;
				return Result;
			}

			_loadingService.Show();
			var Resualt = await PostData();

			if (Resualt)
			{
				Result.Status = HttpStatusCode.OK;

				SumaryMessage = "داده ها با موفقیت ثبت شد";
			}
			else
			{
				Result.Status = HttpStatusCode.InternalServerError;
				SumaryMessage = "ذخیره داده با مشکل مواجه شد";
			}

			Result.Message = SumaryMessage;

			switch ((int)Result.Status)
			{
				case 200:
					toastService.ShowSuccess(Result.Message);
					break;
				case 500:
					toastService.ShowError(Result.Message);
					break;
				default:
					break;
			}
			_loadingService.Hide();
			await AfterSubmit();

			return Result;
		}

		/// <summary>
		/// ارسال داده
		/// </summary>
		/// <returns></returns>
		private async Task<bool> PostData()
		{
			string Data = await Utility.JSON.ToJson(_Entity);
			bool IsOk = false;
			var Model = await ApiServer.External.Services.Data.Put(Data, TablePost.Name, TablePost, RequestID?.ToString(), _User.UserID.ToString());
			if (Model?.Status == HttpStatusCode.OK)
			{
				IsOk = true;
			}

			return IsOk;
		}

		/// <summary>
		/// تابع قبل اجرا شدن ارسال داده
		/// </summary>
		/// <returns></returns>
		public override async Task<Result> BeforSubmit()
		{
			//SetBankToSepahIfNeeded();

			var isShebaValid = ValidateIR();
			var isCartValid = IsValidCardNumber();
			if (!isShebaValid)
			{
				await _MSG.ShowError("فرمت و یا تعداد کاراکتر های شماره شبا درست نیست");
				return new Result() { Status = HttpStatusCode.BadRequest };
			}
			if (!isCartValid)
			{
				await _MSG.ShowError("فرمت و یا تعداد کاراکتر های شماره کارت درست نیست");
				return new Result() { Status = HttpStatusCode.BadRequest };
			}

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
			//SetBankToSepahIfNeeded();

			Console.WriteLine("### AfterGetData CALLED");
			// بررسی نوع دارنده حساب بانکی
			await AccHolderType();
		}


		#region FunctionEvents


		//FK 14040520
		private void AddIR()
		{
			Console.WriteLine("AddIR");
			if (_Entity.IBAN == null)
			{
				_Entity.IBAN = "IR";
				Console.WriteLine("AddIR 2");
				// if (!_Entity.IBAN.StartsWith("IR"))
				//     {
				//         _Entity.IBAN = "IR" + _Entity.IBAN;
				//     }
			}
			Console.WriteLine("AddIR 3");
		}
		//FK 14040520
		private bool ValidateIR()
		{
			if (string.IsNullOrWhiteSpace(_Entity.IBAN))
				return false;

			if (_Entity.IBAN.Length != 26)
				return false;

			if (!_Entity.IBAN.StartsWith("IR"))
				return false;

			string numberPart = _Entity.IBAN.Substring(2);
			if (!Regex.IsMatch(numberPart, @"^\d{24}$"))
				return false;

			return true;
		}

		private bool ValidateIR2()
		{
			if (string.IsNullOrWhiteSpace(_Entity.IBAN))
				return false;

			if (_Entity.IBAN.Length != 24)
				return false;

			if (!Regex.IsMatch(_Entity.IBAN, @"^\d{24}$"))
				return false;

			return true;
		}

		private bool IsValidCardNumber()
		{

			if (string.IsNullOrWhiteSpace(_Entity.CartNo))
				return false;

			// باید دقیقاً 16 رقم باشد
			if (!System.Text.RegularExpressions.Regex.IsMatch(_Entity.CartNo, @"^\d{16}$"))
				return false;

			return true;
		}


		#region HR_Base_BankAcountCategory Selected
		public async Task HR_Base_BankAcountCategoryId_onitemselected(dynamic Selected)
		{
			Console.WriteLine("#Log :: BankAcountCategoryId_onitemselected :: ");

			// شناسه ثابت دسته‌بندی "حقوق و دستمزد"
			var BankAccCatId = "E8B28475-6FEC-F011-A50E-005056A2B6BD";

			// شناسه ثابت بانک "سپه"
			var SepahBankId = "6A2FEC9D-9E93-F011-A50E-005056A2B6BD";
			var SepahBankTitle = "سپه";

			// دریافت مقدار فعلی دسته‌بندی حساب
			var BankAcountCategoryId = _Entity.HR_Base_BankAcountCategoryId?.ToString();

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
				 _Entity.BaseInfo_Bank = new Entity.BaseInfo_Banks { Id = sepahId, Title = sepahTitle };

				// ۳. همگام‌سازی با رابط کاربری (UI)
				// این بخش بسیار مهم است. ما باید به کامپوننت بگوییم که مقدارش تغییر کرده است.
				// فرض بر این است که Ref_BaseInfo_BankId یک Reference به کامپوننت انتخاب بانک است.

				if (Ref_BaseInfo_BankId != null)
				{
					// اگر متد SetEntity در کامپوننت شما وجود دارد، از آن استفاده کنید
					// این متد معمولاً مقدار را در کامپوننت ست کرده و UI را رفرش می‌کند
					Ref_BaseInfo_BankId.SetEntity(_Entity.BaseInfo_Bank);
					Console.WriteLine("#Log :: Ref_BaseInfo_BankId :: ");

					// یا اگر متد خاصی برای ست کردن مقدار (Value) دارید:
					//Ref_BaseInfo_BankId.Value = sepahGuid;


					Ref_BaseInfo_BankId.ItemSelected(_Entity.BaseInfo_Bank);

					await Task.Delay(100);
					Ref_BaseInfo_BankId.LoadData();
				}

				// ۴. اطلاع به Blazor برای رندر مجدد صفحه
				StateHasChanged();

				Console.WriteLine("Log:: Bank set to Sepah successfully.");
			}
		}

		#endregion

		#region HR_Base_AccountHolderType Selected
		///public async Task  HR_Base_AccountHolderTypeId_onitemselected(Entity.HR_Base_AccountHolderType Selected   )
		public async Task  HR_Base_AccountHolderTypeId_onitemselected(dynamic Selected)
        {
			Console.WriteLine("### HR_Base_AccountHolderTypeId_onitemselected TRIGGERED");

			// بررسی null بودن Selected
			if (Selected == null)
			{
				Console.WriteLine("### Selected is NULL");
				return;
			}

			await AccHolderType();
		}
		#endregion

		#region HR_EMP_EmployeesId Selected
		/// <summary>
		/// وقتی کارمند انتخاب می‌شود، فقط همسر همان کارمند را در دراپ‌دان خانواده لود می‌کند.
		/// </summary>
		public async Task HR_EMP_EmployeesId_onitemselected(dynamic Selected)
		{
			Ref_HR_EMP_EmployeeFamileisId?.LoadData();
		}
		#endregion

		#region AccountHolder Method
		/// <summary>
		/// این متد مسئول نمایش یا مخفی کردن فیلدهای مربوط به صاحب حساب است.
		/// اگر showFields = false باشد، هر دو دراپ‌دان کارمند و همسر مخفی می‌شوند.
		/// اگر showFields = true باشد، هر دو نمایش داده می‌شوند (در متد AccHolderType دقیق‌تر کنترل می‌شود).
		/// </summary>
		private async Task ToggleAccountHolderTypeFields(bool showFields)
		{
			Console.WriteLine("### ToggleAccountHolderTypeFields CALLED");
			Console.WriteLine($"showFields = {showFields}");

			Ref_HR_EMP_EmployeesId?.SetVisible(showFields);
			Console.WriteLine($"Employees Visible = {showFields}");

			Ref_HR_EMP_EmployeeFamileisId?.SetVisible(showFields);
			Console.WriteLine($"EmployeeFamilies Visible = {showFields}");

			await InvokeAsync(StateHasChanged);

			Console.WriteLine("### ToggleAccountHolderTypeFields FINISHED");
		}


		// Id										Code    title
		// 117F1A3A-3BFB-F011-A50E-005056A2B6BD		1		اطلاعات حساب بانکی کارمند
		// 4C395B47-3BFB-F011-A50E-005056A2B6BD		2		اطلاعات حساب بانکی همسر


		/// <summary>
		/// این متد بر اساس مقدار انتخاب‌شده در دراپ‌دان HR_Base_AccountHolderTypeId
		/// تعیین می‌کند کدام فیلدها نمایش داده شوند:
		/// - در شروع: هر دو مخفی می‌شوند
		/// - اگر نوع = کارمند → فقط دراپ‌دان کارمند نمایش داده می‌شود
		/// - اگر نوع = همسر → دراپ‌دان کارمند و همسر هر دو نمایش داده می‌شوند
		/// </summary>
		private async Task AccHolderType()
		{
			Console.WriteLine("======================================");
			Console.WriteLine("### AccHolderType CALLED");

			// تعریف ثابت‌ها با حروف بزرگ (استاندارد GUID)
			const string EmpHusband = "117F1A3A-3BFB-F011-A50E-005056A2B6BD"; // کارمند
			const string EmpWife = "4C395B47-3BFB-F011-A50E-005056A2B6BD";     // همسر

			// نرمال‌سازی مقدار انتخاب شده به حروف بزرگ برای مقایسه صحیح
			var selectedTypeId = _Entity.HR_Base_AccountHolderTypeId?.ToString()?.ToUpper();

			Console.WriteLine($"Selected HR_Base_AccountHolderTypeId (normalized) = {selectedTypeId}");

			// پیش‌فرض: هر دو مخفی
			Console.WriteLine("Setting default state: hide all fields");
			await ToggleAccountHolderTypeFields(false);

			if (string.IsNullOrWhiteSpace(selectedTypeId))
			{
				Console.WriteLine("SelectedTypeId is NULL or Empty → Only AccountHolderType dropdown is visible");
				await InvokeAsync(StateHasChanged);
				return;
			}

			if (selectedTypeId == EmpHusband)
			{
				Console.WriteLine("AccountHolderType = EMPLOYEE (کارمند)");
				Console.WriteLine("→ Showing only Employees dropdown");

				Ref_HR_EMP_EmployeesId?.SetVisible(true);
				Ref_HR_EMP_EmployeeFamileisId?.SetVisible(false);
			}
			else if (selectedTypeId == EmpWife)
			{
				Console.WriteLine("AccountHolderType = WIFE (همسر)");
				Console.WriteLine("→ Showing Employees + EmployeeFamilies dropdowns");

				Ref_HR_EMP_EmployeesId?.SetVisible(true);
				Ref_HR_EMP_EmployeeFamileisId?.SetVisible(true);

				// // اگر کارمند قبلاً انتخاب شده، همسرانش را لود کن
				// if (!string.IsNullOrWhiteSpace(_Entity.HR_EMP_EmployeesId?.ToString()))
				// {
				// 	Console.WriteLine("→ Employee already selected, loading spouses...");
				// 	await HR_EMP_EmployeesId_onitemselected(new { Id = _Entity.HR_EMP_EmployeesId });
				// }
			}
			else
			{
				Console.WriteLine($"AccountHolderType = UNKNOWN VALUE: {selectedTypeId}");
				Console.WriteLine("→ All fields remain hidden");
			}

			await InvokeAsync(StateHasChanged);

			Console.WriteLine("### AccHolderType FINISHED");
			Console.WriteLine("======================================");
		}

		#endregion

	

		#endregion FunctionEvents

	}
}