using ApiServer.External.Services;
using Baya.Models.Utility;
using BlazorBootstrap;
using Castle.DynamicLinqQueryBuilder;
using DateUtils;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitko.Blazor.CKEditor;
using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using Utility;
using static DevExpress.ReportServer.Printing.RemoteDocumentSource;

namespace Forms.Forms
{
	public class Form_936_CUBase : Form_936_CUPeropeties
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
			}
		}

		/// <summary>
		/// اعتبار سنجی فرم
		/// </summary>
		/// <returns></returns>
		public override async Task<bool> FormValidator()
		{
			bool IsValid = true;

			// فراخوانی تابع اعتبارسنجی فیلدها
			if (!await CheckFieldValidation(_Entity))
			{
				IsValid = false;
			}

			return IsValid;
		}

		/// <summary>
		/// تابع قبل اجرا شدن ارسال داده
		/// </summary>
		/// <returns></returns>
		public override async Task<Result> BeforSubmit()
		{
			// کل تغییرات قبل از Submit فرم
			PrepareEntityForSubmit();

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
			// Console.WriteLine("#Log => AfterGetData::");
			// بررسی داده ها قبل داده‌ها
			PrepareEntityForAfterGetData();

			// Console.WriteLine("#Log => AfterGetData End::");
		}

		#region FunctionEvents

		public async Task<bool> CheckFieldValidation(Entity.HR_CVR_PersonnelContract Item)
		{
			bool IsValid = true;
			// **************************************************

			// اعتبارسنجی منطقی: تاریخ پایان باید بزرگتر یا مساوی تاریخ شروع باشد
			if (!string.IsNullOrWhiteSpace(_Entity.StartDate_Fa) && !string.IsNullOrWhiteSpace(_Entity.EndDate_Fa))
			{
				if (PersianDateUtils.TryParseDateString(_Entity.StartDate_Fa, out _) &&
					PersianDateUtils.TryParseDateString(_Entity.EndDate_Fa, out _))
				{
					var start = PersianDateUtils.ToGregorian(_Entity.StartDate_Fa);
					var end = PersianDateUtils.ToGregorian(_Entity.EndDate_Fa);

					if (end < start)
					{
						IsValid = false;
						await _MSG.ShowError("تاریخ پایان قرارداد نمی‌تواند کوچکتر از تاریخ شروع باشد.");
					}
				}
			}

			// StartDate_Fa - تاریخ شروع قرارداد شمسی
			if (!string.IsNullOrWhiteSpace(Item.StartDate_Fa) && !PersianDateUtils.TryParseDateString(Item.StartDate_Fa, out _))
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً تاریخ شروع قرارداد را به فرمت صحیح شمسی (مثال: 1404/01/01) وارد نمایید.");
			}

			// EndDate_Fa - تاریخ پایان قرارداد شمسی
			if (!string.IsNullOrWhiteSpace(Item.EndDate_Fa) && !PersianDateUtils.TryParseDateString(Item.EndDate_Fa, out _))
			{
				IsValid = false;
				await _MSG.ShowError("لطفاً تاریخ پایان قرارداد را به فرمت صحیح شمسی (مثال: 1404/12/29) وارد نمایید.");
			}
			// **************************************************

			// **************************************************
			// Details 
			//foreach (var item in _Entity.HR_EMP_EmployeeInfos)
			//{
			//    // فیلد نام پدر
			//    if (item.FatherName == null)
			//    {
			//        IsValid = false;
			//        await _MSG.ShowError("لطفا گزینه نام پدر را تکمیل نمایید.");
			//    }

			//}

			// **************************************************


			return IsValid;
		}

		/// <summary>
		/// آماده‌سازی موجودیت برای ذخیره — شامل:
		/// - تبدیل تاریخ شمسی به میلادی
		/// - محاسبه Total_Days, Days_Elapsed, Days_Remaining با استفاده از PersianDateUtils
		/// </summary>
		private void PrepareEntityForSubmit()
		{
			// تبدیل تاریخ‌های شمسی به میلادی — فقط اگر معتبر باشند
			if (!string.IsNullOrWhiteSpace(_Entity.StartDate_Fa) && PersianDateUtils.TryParseDateString(_Entity.StartDate_Fa, out _))
			{
				_Entity.StartDate = PersianDateUtils.ToGregorian(_Entity.StartDate_Fa);
				StateHasChanged();
			}

			if (!string.IsNullOrWhiteSpace(_Entity.EndDate_Fa) && PersianDateUtils.TryParseDateString(_Entity.EndDate_Fa, out _))
			{
				_Entity.EndDate = PersianDateUtils.ToGregorian(_Entity.EndDate_Fa);
				StateHasChanged();
			}

			// محاسبه Total_Days — با استفاده از PersianDateUtils
			if (!string.IsNullOrWhiteSpace(_Entity.StartDate_Fa) && !string.IsNullOrWhiteSpace(_Entity.EndDate_Fa))
			{
				if (PersianDateUtils.TryParseDateString(_Entity.StartDate_Fa, out _) && PersianDateUtils.TryParseDateString(_Entity.EndDate_Fa, out _))
				{
					var difference = PersianDateUtils.GetDifference(_Entity.StartDate_Fa, _Entity.EndDate_Fa, inclusive: true);
					var days = difference.TotalDays.ToString();
					_Entity.Total_Days = $"{days} روز";
				}
			}
		}

		private void PrepareEntityForAfterGetData()
		{
			// اگر تاریخ میلادی وجود داشت — برای نمایش در UI به شمسی تبدیل می‌شود
			if (_Entity.StartDate.HasValue)
			{
				_Entity.StartDate_Fa = PersianDateUtils.ToPersian(_Entity.StartDate.Value);
			}

			if (_Entity.EndDate.HasValue)
			{
				_Entity.EndDate_Fa = PersianDateUtils.ToPersian(_Entity.EndDate.Value);
			}

			// محاسبه فیلدهای پویا
			RecalculateDynamicFields();
		}

		/// <summary>
		/// محاسبه مجدد فیلدهای پویا — در تغییر تاریخ‌ها
		/// </summary>
		private void RecalculateDynamicFields()
		{
			// محاسبه Days_Elapsed — با استفاده از PersianDateUtils.DaysPassed
			if (!string.IsNullOrWhiteSpace(_Entity.StartDate_Fa) && PersianDateUtils.TryParseDateString(_Entity.StartDate_Fa, out _))
			{
				var days = PersianDateUtils.DaysPassed(_Entity.StartDate_Fa, inclusive: true).ToString();
				_Entity.Days_Elapsed = $"{days} روز";
			}

			// محاسبه Days_Remaining — با استفاده از PersianDateUtils.DaysRemaining
			if (!string.IsNullOrWhiteSpace(_Entity.EndDate_Fa) && PersianDateUtils.TryParseDateString(_Entity.EndDate_Fa, out _))
			{
				var days = PersianDateUtils.DaysRemaining(_Entity.EndDate_Fa, inclusive: true).ToString();
				_Entity.Days_Remaining = $"{days} روز";
			}
		}

		public async Task StartDate_Fa_oninput(ChangeEventArgs Selected)
		{
			// محاسبه مجدد فیلدهای پویا
			RecalculateDynamicFields();
		}

		public async Task EndDate_Fa_oninput(ChangeEventArgs Selected)
		{
			//محاسبه مجدد فیلدهای پویا
			RecalculateDynamicFields();
		}

		#region بررسی استوردهای جدول مدت قرارداد و قرارداد
		public async Task PersonnelContract_onclick(MouseEventArgs Selected)
		{
			Console.WriteLine("### 🟡 شروع فراخوانی PersonnelContract ###");

			if (_Entity?.HR_EMP_EmployeesId == null)
			{
				await _MSG.ShowWarning("لطفاً ابتدا کارمند را انتخاب کنید.");
				Console.WriteLine("❌ HR_EMP_EmployeesId null است");
				return;
			}

			var R = await BayaApi.PersonnelContract(
				ShomaranApiMode.Polfilm,
				new EmployeeModel.EmpId
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
			Console.WriteLine(jsonResponse);

			if (!jsonResponse.TrimStart().StartsWith("{"))
			{
				await _MSG.ShowError("خروجی وب سرویس JSON نیست:\n" + jsonResponse);
				Console.WriteLine("❌ خطا: پاسخ JSON نیست");
				return;
			}

			try
			{
				var root = JObject.Parse(jsonResponse);
				var dataSetsToken = root["DataSets"];
				if (dataSetsToken == null || !(dataSetsToken is JArray dataSets) || dataSets.Count < 4)
				{
					await _MSG.ShowError("ساختار پاسخ نامعتبر: DataSets کامل نیست");
					Console.WriteLine("❌ خطا: DataSets کامل نیست");
					return;
				}

				// --- 1. لیست A: CountOfAllContract ---
				var aList = dataSets[0] as JArray;
				if (aList == null || aList.Count == 0)
				{
					await _MSG.ShowError("لیست A خالی است");
					Console.WriteLine("❌ لیست A خالی یا null است");
					return;
				}
				var aModel = aList[0].ToObject<ContractTimeModel.AllContractModel>();
				var A = aModel.CountOfAllContract;
				Console.WriteLine($"✅ A = {A}");

				// --- 2. لیست D: PositionClasificationId ---
				var dList = dataSets[1] as JArray;
				if (dList == null || dList.Count == 0)
				{
					await _MSG.ShowError("لیست D خالی است");
					Console.WriteLine("❌ لیست D خالی یا null است");
					return;
				}
				var dModel = dList[0].ToObject<ContractTimeModel.PositionClasificationModel>();
				var D = dModel.PositionClasificationId ?? "null";
				Console.WriteLine($"✅ D = '{D}'");

				// --- 3. لیست C: لیست بلند ContractTime ---
				var cList = dataSets[2] as JArray;
				if (cList == null)
				{
					await _MSG.ShowError("لیست C null است");
					Console.WriteLine("❌ لیست C null است");
					return;
				}
				var ListC = new List<Entity.HR_CRS_ContractTime>();
				try
				{
					ListC = cList.ToObject<List<Entity.HR_CRS_ContractTime>>();
				}
				catch (Exception ex)
				{
					Console.WriteLine("❌ خطا در تبدیل لیست C: " + ex.Message);
					if (cList.Count > 0)
						Console.WriteLine("اولین آیتم لیست C برای عیب‌یابی:");

					Console.WriteLine(cList.FirstOrDefault()?.ToString());
					await _MSG.ShowError("خطا در پردازش لیست C: " + ex.Message);
					return;
				}
				Console.WriteLine($"✅ ListC با موفقیت بارگذاری شد. تعداد = {ListC.Count}");

				// ✅ اینجا ListC به Dropdown داده می‌شود
				if (Ref_HR_CRS_ContractTimeId != null)
				{
					Console.WriteLine("✅ Setting Dropdown with ListC via SetEntity");

					//Ref_HR_CRS_ContractTimeId.SetEntity(ListC);
					//await Task.Delay(100);
					//await Ref_HR_CRS_ContractTimeId.LoadData();

					
				}

				// --- 4. لیست B: TheLastCounterOfCurrentContract ---
				var bList = dataSets[3] as JArray;
				if (bList == null || bList.Count == 0)
				{
					await _MSG.ShowError("لیست B خالی است");
					Console.WriteLine("❌ لیست B خالی یا null است");
					return;
				}
				var bModel = bList[0].ToObject<ContractTimeModel.CurrentContractModel>();
				var B = bModel.TheLastCounterOfCurrentContract ?? "null";
				Console.WriteLine($"✅ B = '{B}'");

				// --- نمایش دیالوگ ---
				var options = new BlazorBootstrap.ConfirmDialogOptions
				{
					YesButtonText = "بازگشت به قرارداد",
					YesButtonColor = ButtonColor.Info,
					NoButtonText = "",
				};

				string htmlString = $@"
					<div style='direction: ltr; text-align: left; line-height: 1.8;'>
						<div><strong>داده A:</strong> {A} </div>
						<div><strong>داده D:</strong> {(string.IsNullOrEmpty(D) ? "ـ" : D)} </div>
						<div><strong>داده B:</strong> {(string.IsNullOrEmpty(B) ? "ـ" : B)} </div>
						<div><strong>تعداد داده‌های فهرست C:</strong> {ListC.Count}</div>
					</div>";

				await Confirm.ShowAsync("", htmlString, options);
				Console.WriteLine("✅ ### اتمام عملیات با موفقیت ###");
			}
			catch (Exception ex)
			{
				Console.WriteLine("❌ خطا در پردازش کلی: " + ex.Message);
				await _MSG.ShowError("خطا در پردازش SP: " + ex.Message);
			}
		}


		public async Task PersonnelContract1_onclick(MouseEventArgs Selected)
		{
			// --- بررسی فیلدها به صورت جداگانه ---
			if (_Entity?.HR_EMP_EmployeesId == null || _Entity.HR_EMP_EmployeesId == Guid.Empty)
			{
				await _MSG.ShowWarning("لطفاً «اطلاعات کارمند» را انتخاب کنید.");
				Console.WriteLine("❌ submit1_onclick: HR_EMP_EmployeesId خالی است");
				return;
			}

			if (string.IsNullOrEmpty(_Entity.StartDate_Fa))
			{
				await _MSG.ShowWarning("لطفاً «تاریخ شروع قرارداد» را وارد کنید.");
				Console.WriteLine("❌ submit1_onclick: StartDate_Fa خالی است");
				return;
			}

			if (_Entity.HR_CRS_ContractTimeId == Guid.Empty)
			{
				await _MSG.ShowWarning("لطفاً «مدت قرارداد» را از لیست انتخاب کنید.");
				Console.WriteLine("❌ submit1_onclick: HR_CRS_ContractTimeId خالی است");
				return;
			}

			// --- ادامه منطق ---
			try
			{
				var R = await BayaApi.GetEndTimeOfContract(ShomaranApiMode.Polfilm,
					new ContractTimeModel.PersonnelEndTimeOfContractRequest
					{
						EmployeesId = _Entity.HR_EMP_EmployeesId.Value,
						startDate = _Entity.StartDate_Fa.Replace("/", ""),
						contractTime = _Entity.HR_CRS_ContractTimeId.ToString()
					});

				if (R != null)
				{
					// --- نمایش دیالوگ ---
					var options = new BlazorBootstrap.ConfirmDialogOptions
					{
						YesButtonText = "بازگشت به قرارداد",
						YesButtonColor = ButtonColor.Info,
						NoButtonText = "",
					};

					string responseText = R.Content?.ToString() ?? "پاسخ خالی است";
					string htmlString = $@"
					<div style='direction: ltr; text-align: left; line-height: 1.8; font-family: sans-serif;'>
						<div><strong>پاسخ سرور:</strong></div>
						<div style='background-color: #f8f9fa; padding: 10px; border-radius: 4px; margin-top: 8px; white-space: pre-wrap;'>
							{responseText}
						</div>
					</div>";

					await Confirm.ShowAsync("", htmlString, options);


					await _MSG.ShowInfo(R.Content.ToString());

					// ******************

					var result = JsonConvert.DeserializeObject<ContractTimeModel.RootContractTime>(R.Content.ToString());

					// دسترسی به داده:
					var firstItem = result.DataSets[0][0];
					Console.WriteLine(firstItem.Shamsi);
					Console.WriteLine(firstItem.Miladi);

					_Entity.EndDate_Fa = firstItem.Shamsi;
					_Entity.EndDate = firstItem.Miladi;

					Console.WriteLine("✅ submit1_onclick: پاسخ دریافت شد");
				}
				else
				{
					await _MSG.ShowError("پاسخی از سرور دریافت نشد.");
					Console.WriteLine("❌ submit1_onclick: پاسخ null است");
				}
			}
			catch (Exception ex)
			{
				await _MSG.ShowError("خطا در ارتباط با سرور: " + ex.Message);
				Console.WriteLine("❌ submit1_onclick: Exception - " + ex.Message);
			}

			Console.WriteLine("#Log_GetEndTimeOfContract::: " +
				_Entity.HR_EMP_EmployeesId.Value + "\n" +
				_Entity.StartDate_Fa + "\n" +
				_Entity.HR_CRS_ContractTimeId.ToString());
		}


		#endregion

		#endregion FunctionEvents
	}
}