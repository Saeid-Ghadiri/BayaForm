using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using System.Text.RegularExpressions;
using Entity;

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


		#region متد دسته بندی حساب بانکی
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

		#endregion FunctionEvents

	}
}