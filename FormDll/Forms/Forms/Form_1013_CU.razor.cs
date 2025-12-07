using ApiServer.External.Services;
using Baya.Models.ORM;
using Baya.Models.Utility;
using BlazorBootstrap;
using Castle.DynamicLinqQueryBuilder;
using DevExpress.Blazor;
using Entity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitko.Blazor.CKEditor;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Utility;

namespace Forms.Forms
{
	public class Form_1013_CUBase : Form_1013_CUPeropeties
	{
		public MSG _MSG { get; set; }

		// ذخیره‌سازی ListC از SP برای استفاده در Dropdown
		private List<Entity.HR_CRS_ContractTime> _cachedContractTimeList = new();

		protected override async Task OnInitializedAsync()
		{
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_MSG = new MSG(toastService);
			}
		}

		public override async Task<bool> FormValidator()
		{
			return true;
		}

		public override async Task<Result> BeforSubmit()
		{
			return new Result() { Status = HttpStatusCode.OK };
		}

		public override async Task AfterSubmit() { }

		public override async Task BeforGetData() { }

		public override async Task AfterGetData() { }

		#region FunctionEvents

		#region Utility Methods

		/// <summary>
		/// پر کردن فیلدهای نمایشی از View_HR_CRS_ContractTime
		/// </summary>
		private async Task FillContractTimeDisplayFields(Guid contractTimeId)
		{
			Console.WriteLine($"#Log FillContractTimeDisplayFields: contractTimeId = {contractTimeId}");

			var tableView = new Table
			{
				Name = "View_HR_CRS_ContractTime",
				Column = new List<Coulmn>
				{
					new Coulmn { Name = "Id", NameAs = "Id" },
					new Coulmn { Name = "HR_ORG_PositionClassificationTitle", NameAs = "HR_ORG_PositionClassificationTitle" },
					new Coulmn { Name = "HR_ContractTimeTypeTitle", NameAs = "HR_ContractTimeTypeTitle" },
					new Coulmn { Name = "FromNumber", NameAs = "FromNumber" },
					new Coulmn { Name = "ToNumber", NameAs = "ToNumber" },
					new Coulmn { Name = "HR_EmployeeContractTypeTitle", NameAs = "HR_EmployeeContractTypeTitle" },
					new Coulmn { Name = "ContractTimeCounter", NameAs = "ContractTimeCounter" }
				}
			};

			var queryFilter = new QueryBuilderFilterRule
			{
				Condition = "AND",
				Rules = new List<QueryBuilderFilterRule>
				{
					new QueryBuilderFilterRule
					{
						Field = "Id",
						Operator = "equal",
						Type = "string",
						Value = new[] { contractTimeId.ToString() }
					}
				}
			};

			var response = await ApiServer.External.Services.Data.Get(
				tableView, queryFilter, "View_HR_CRS_ContractTime", _User.UserID.ToString());

			if (response?.Status == HttpStatusCode.OK && response.Content != null)
			{
				var viewItem = await Utility.JSON.ToObject<Entity.View_HR_CRS_ContractTime>(response.Content.ToString());
				if (viewItem != null)
				{
					_Entity.HR_ORG_PositionClassification = viewItem.HR_ORG_PositionClassificationTitle ?? string.Empty;
					_Entity.HR_ContractTimeType = viewItem.HR_ContractTimeTypeTitle ?? string.Empty;
					_Entity.FromNumber = viewItem.FromNumber?.ToString() ?? string.Empty;
					_Entity.ToNumber = viewItem.ToNumber?.ToString() ?? string.Empty;
					_Entity.HR_EmployeeContractType = viewItem.HR_EmployeeContractTypeTitle ?? string.Empty;
					_Entity.ContractTimeCounter = viewItem.ContractTimeCounter?.ToString() ?? string.Empty;
					StateHasChanged();
					Console.WriteLine("#Log FillContractTimeDisplayFields: Success");
				}
				else
				{
					Console.WriteLine("#Log FillContractTimeDisplayFields: viewItem is null");
				}
			}
			else
			{
				Console.WriteLine("#Log FillContractTimeDisplayFields: API call failed or no data");
			}
		}

		#endregion

		public async Task submit_onclick(MouseEventArgs Selected)
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
				new PersonnelContractRequest
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
				var aModel = aList[0].ToObject<SP_ContractTime.AllContractModel>();
				var A = aModel.CountOfAllContract;

				// پر کردن فیلد شمارنده از روی SP
				_Entity.Counter = A;

				Console.WriteLine($"✅ A = {A}");

				// --- 2. لیست D: PositionClasificationId ---
				var dList = dataSets[1] as JArray;
				if (dList == null || dList.Count == 0)
				{
					await _MSG.ShowError("لیست D خالی است");
					Console.WriteLine("❌ لیست D خالی یا null است");
					return;
				}
				var dModel = dList[0].ToObject<SP_ContractTime.PositionClasificationModel>();
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


					// ساخت لیست قوانین
					var rules = new List<QueryBuilderFilterRule>();

					foreach (var item in ListC)
					{
						rules.Add(new QueryBuilderFilterRule
						{
							Id = "Id",
							Field = "Id",
							Type = "string",
							Input = "text",
							Operator = "equal",
							Value = new string[] { item.Id.ToString() } // ⚠️ حتماً آرایه باشد
						});
					}

					// ساخت فیلتر نهایی
					var filter = new QueryBuilderFilterRule
					{
						Condition = "OR",
						Rules = rules
					};

					await Task.Delay(100);

					Console.WriteLine("#Log:3::" + filter);

					// LoadData رشته می‌خواهد
					await Ref_HR_CRS_ContractTimeId.Search(filter);
				}

				// --- 4. لیست B: TheLastCounterOfCurrentContract ---
				var bList = dataSets[3] as JArray;
				if (bList == null || bList.Count == 0)
				{
					await _MSG.ShowError("لیست B خالی است");
					Console.WriteLine("❌ لیست B خالی یا null است");
					return;
				}
				var bModel = bList[0].ToObject<SP_ContractTime.CurrentContractModel>();
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

		public async Task submit1_onclick(MouseEventArgs Selected)
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
					new PersonnelEndTimeOfContractRequest
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

					var result = JsonConvert.DeserializeObject<RootContractTime>(R.Content.ToString());

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

		// public async Task HR_CRS_ContractTimeId_onitemselected(Entity.HR_CRS_ContractTime Selected)
		// {
		// 	//Console.WriteLine($"🟡 HR_CRS_ContractTimeId_onitemselected: Selected = {(Selected != null ? Selected.Id.ToString() : "null")}");

		// 	//if (Selected == null || Selected.Id == Guid.Empty)
		// 	//{
		// 	//	Console.WriteLine("⚠️ انتخاب null یا Guid.Empty");
		// 	//	return;
		// 	//}

		// 	//// فقط پر کردن فیلدهای نمایشی
		// 	//await FillContractTimeDisplayFields(Selected.Id);


		// 	//// اگر Selected null باشد (مثلاً وقتی ResetDropdown فراخوانی می‌شود)، از متد خارج می‌شویم
		// 	//if (Selected == null)
		// 	//{
		// 	//	Console.WriteLine("#Log: Selected is null - dropdown was reset");
		// 	//	return;
		// 	//}

		// 	//Console.WriteLine("#Log:0::");

		// 	//var jsonSelected = await Utility.JSON.ToJson(Selected);
		// 	//Console.WriteLine(jsonSelected);

		// 	//string selectedId = Selected.Id.ToString();

		// 	//Console.WriteLine("#Log:1::" + selectedId);

		// 	//if (string.IsNullOrWhiteSpace(selectedId))
		// 	//{
		// 	//	Console.WriteLine("#Log: Selected tracking code is empty");
		// 	//	return; // یا LoadData("") ارسال شود
		// 	//}



		// 	//List<string> x = new List<string>();

		// 	//// SP_Contract.spContract.GetContractListC ==> در فرم 936 وجود دارد.
		// 	//var getlistc =await SP_Contract.spContract.GetContractListC(_Entity.HR_EMP_EmployeesId.Value);
		// 	//foreach (var item in getlistc)
		// 	//{
		// 	//	x.Add(item.Id.ToString());
		// 	//}

		// 	//// درست: مدل اصلی QueryBuilderFilter
		// 	//QueryBuilderFilterRule filter = new QueryBuilderFilterRule()
		// 	//{
		// 	//	Condition = "OR",
		// 	//	Rules = new List<QueryBuilderFilterRule>()
		// 	//	{
		// 	//		new QueryBuilderFilterRule()
		// 	//		{
		// 	//			Id = "Id",
		// 	//			Field = "Id",
		// 	//			Type = "string",
		// 	//			Input = "text",
		// 	//			Operator = "equal",
		// 	//			Value = x.ToArray()
		// 	//		}
		// 	//	}
		// 	//};

		// 	//await Task.Delay(100);

		// 	//Console.WriteLine("#Log:3::" + filter);

		// 	//// LoadData رشته می‌خواهد
		// 	//await Ref_HR_CRS_ContractTimeId.Search(filter);
		// }

		public async Task  HR_CRS_ContractTimeId_onitemselected(Entity.HR_CRS_ContractTime Selected   )
        {

            
        }

		public async Task  HR_EMP_EmployeesId_onitemselected(Entity.HR_EMP_Employees Selected   )
        { 
			Entity.HR_CVR_PersonnelContract personnelContract_Id = new();
			personnelContract_Id.HR_EMP_EmployeesId = Selected.Id;

			Console.WriteLine("#Log 1" + personnelContract_Id);

			Ref_HR_CVR_PersonnelContractId.SetEntity(personnelContract_Id);

			Console.WriteLine(await Utility.JSON.ToJson(personnelContract_Id));
			await Ref_HR_CVR_PersonnelContractId.ItemSelected(personnelContract_Id);

			Console.WriteLine("#Log 2");

			await Task.Delay(100);
			await Ref_HR_CVR_PersonnelContractId.LoadData();
        }

		#endregion FunctionEvents
	}
}

// اطلاعات زیر در فرم 936 فرم قرارداد - قرار دارد
// namespace SP_ContractTime
// namespace ApiServer.External.Services
// PersonnelContractRequest
// PersonnelEndTimeOfContractRequest
// RootContractTime
// namespace SP_Contract