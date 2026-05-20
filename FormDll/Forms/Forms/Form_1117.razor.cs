using System;
using System.Net;
using Baya.Models.Utility;
using BlazorBootstrap;
using Blazored.Toast.Services;
using Castle.DynamicLinqQueryBuilder;
using DateUtils;
using DevExpress.Blazor;
using Entity;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Forms.Forms
{
	public class Form_1117Base : Form_1117Peropeties
	{
		/// <summary>
		/// پیام‌ها
		/// </summary>
		public MSG _MSG { get; set; }

		/// <summary>
		/// کلاینت HTTP
		/// </summary>
		[Inject] public HttpClient HttpClient { get; set; }


		#region Lifecycle Methods

		protected override async Task OnInitializedAsync()
		{
			_Entity.IDMS_RDC_Details ??= new List<Entity.IDMS_RDC_Details>();
			_Entity.IDMS_TestModel ??= new List<Entity.IDMS_TestModel>();
		}



		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_MSG = new MSG(toastService);

				// کمی صبر می‌کنیم تا ref ها آماده شوند
				//await Task.Delay(200);

				await ToggleDetailsGridAddButton();
				await ToggleResultOfAnotherProcessFields(_Entity?.IsResultOfAnotherProcess == true);
			}
			else
			{
				var currentCount = _Entity.IDMS_RDC_Details?.Count(x => x.IsDelete != true) ?? 0;
				if (currentCount != _lastDetailsCount)
				{
					_lastDetailsCount = currentCount;
					await ToggleDetailsGridAddButton();
				}
			}
		}

		#endregion

		#region Form Submission Pipeline

		/// <summary>
		/// اعتبار سنجی فرم
		/// </summary>
		/// <returns></returns>
		public override async Task<bool> FormValidator()
		{
			bool IsValid = true;
			List<Entity.IDMS_RDC_Details> List = _Entity.IDMS_RDC_Details.Where(x => x.IsDelete != true).ToList();

			if (!await HasValidDetailsCount())
			{
				await ShowRequestIncompleteDialog();
				return false;
			}

			//بررسی اعتبار سنجی فیلدها برای هر ردیف جزئیات یک به یک
			foreach (var Item in List)
			{
				IsValid = IsValid && await CheckFieldsValidation(Item);
			}

			return IsValid;
		}

		/// <summary>
		/// تابع قبل اجرا شدن ارسال داده
		/// قبل از ارسال، تاریخ‌های شمسی را به میلادی تبدیل می‌کنیم
		/// </summary>
		/// <returns></returns>
		public override async Task<Result> BeforSubmit()
		{
			if (!await HasValidDetailsCount())
				return new Result { Status = HttpStatusCode.BadRequest };

			// تبدیل تاریخ‌های شمسی به میلادی
			PrepareRequestedDueDatesForSubmit();

			return new Result { Status = HttpStatusCode.OK };
		}

		/// <summary>
		/// تابع بعد اجرا شدن ارسال داده
		/// </summary>
		/// <returns></returns>
		public override async Task AfterSubmit() { }

		/// <summary>
		/// تابع قبل دریافت داده
		/// </summary>
		/// <returns></returns>
		public override async Task BeforGetData() { }

		/// <summary>
		/// تابع بعد دریافت داده
		/// بعد از دریافت داده از سرور، وضعیت فیلدها را بر اساس IsResultOfAnotherProcess تنظیم می‌کنیم
		/// </summary>
		/// <returns></returns>
		public override async Task AfterGetData()
		{
			// Start: شروع پیشرفت بارگذاری
			StartProgressToFullLoad();

			// 
			await ToggleDetailsGridAddButton();
			await ToggleResultOfAnotherProcessFields(_Entity?.IsResultOfAnotherProcess == true);

			// کمی صبر می‌کنیم تا مطمئن شویم همه داده‌ها در فرم کامل رندر شده‌اند
			await Task.Delay(300);

			// End: بعد از لود کامل داده‌ها و رندر فرم، ProgressBar را حذف می‌کنیم و فرم را فعال می‌کنیم
			// این متد به صورت خودکار Dispose را نیز انجام می‌دهد
			await ProgressToFullLoad();
		}

		#endregion

		#region FunctionEvents

		#region Progress Management

		private bool _isFormFullyLoaded = false;
		private int _loadingProgressPercent = 25;
		private bool _isProgressStarted = false;
		protected bool IsFormFullyLoaded => _isFormFullyLoaded;
		protected int LoadingProgressPercent => _loadingProgressPercent;

		/// <summary>
		/// شروع فرآیند پیشرفت
		/// </summary>
		private void StartProgressToFullLoad()
		{
			if (_isProgressStarted)
				return;

			_isProgressStarted = true;
			SetLoadingProgress(25);
			_isFormFullyLoaded = false;
		}

		/// <summary>
		/// پایان فرآیند پیشرفت
		/// </summary>
		private async Task ProgressToFullLoad()
		{
			SetLoadingProgress(60);

			if (!_isFormFullyLoaded)
			{
				_isFormFullyLoaded = true;
				await InvokeAsync(StateHasChanged);
			}

			DisposeProgressResources();
		}

		/// <summary>
		/// تنظیم درصد پیشرفت
		/// </summary>
		private void SetLoadingProgress(int percent)
		{
			_loadingProgressPercent = Math.Max(0, Math.Min(60, percent));
		}

		/// <summary>
		/// آزادسازی منابع مرتبط با Progress
		/// </summary>
		private void DisposeProgressResources()
		{
			_isProgressStarted = false;
		}

		#endregion

		#region Validation Logic

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
			if (_Entity.IDMS_RDC_Details == null)
			{
				await _MSG.ShowError("لطفاً حداقل یک ردیف در بخش «جزئیات سیستم تحقیق و توسعه» ثبت کنید.");
				return false;
			}

			// شمارش ردیف‌های فعال (غیر حذف‌شده)
			var activeCount = _Entity.IDMS_RDC_Details.Count(x => x.IsDelete != true);

			if (activeCount == 0)
			{
				await _MSG.ShowError("لطفاً حداقل یک ردیف در بخش «جزئیات سیستم تحقیق و توسعه» ثبت کنید.");
				return false;
			}

			if (activeCount > 1)
			{
				await _MSG.ShowError("شما مجاز به ثبت بیش از یک ردیف نیستید. لطفاً فقط یک ردیف در بخش «جزئیات سیستم تحقیق و توسعه» ثبت کنید.");
				return false;
			}

			return true;
		}

		/// <summary>
		/// بررسی تمامی آیتم‌های جزئیات و اعتبار سنجی فیلدها
		/// </summary>
		/// <returns></returns>
		private async Task<bool> CheckFieldsValidation(Entity.IDMS_RDC_Details Item)
		{
			bool IsValid = true;

			// if (Item.IDMS_ProductCategoriesId == null)
			// {
			// 	IsValid = false;
			// 	await _MSG.ShowError("لطفاً دسته‌بندی محصول را انتخاب کنید.");
			// }
			// if (Item.IDMS_ProductsId == null)
			// {
			// 	IsValid = false;
			// 	await _MSG.ShowError("لطفاً محصول را انتخاب کنید.");
			// }
			// if (Item.IDMS_CustomerId == null)
			// {
			// 	IsValid = false;
			// 	await _MSG.ShowError("لطفاً مشتری را انتخاب کنید.");
			// }
			// if (Item.IDMS_ResultingFromId == null)
			// {
			// 	IsValid = false;
			// 	await _MSG.ShowError("لطفاً گزینه «منتج از» را انتخاب کنید.");
			// }

			// // تاریخ پیشنهادی انجام کار (شمسی)
			// if (string.IsNullOrWhiteSpace(Item.RequestedDueDate_Fa))
			// {
			// 	IsValid = false;
			// 	await _MSG.ShowError("لطفاً تاریخ پیشنهادی انجام کار را وارد کنید.");
			// }
			// else if (!PersianDateUtils.TryParseDateString(Item.RequestedDueDate_Fa, out _))
			// {
			// 	IsValid = false;
			// 	await _MSG.ShowError("لطفاً تاریخ پیشنهادی انجام کار را با فرمت صحیح شمسی (مثال: 1403/01/01) وارد کنید.");
			// }

			return IsValid;
		}

		#endregion

		#region Grid Events: IDMS_RDC_Details

		/// <summary>
		/// تابع قبل اجرا شدن ثبت مدل جزئیات
		/// این متد قبل از ذخیره شدن ردیف فراخوانی می‌شود.
		/// </summary>
		/// <param name="e">ردیف در حال ذخیره</param>
		/// <returns>false اگر ذخیره مجاز باشد، true اگر ذخیره مجاز نباشد</returns>
		public async Task<bool> GridIDMS_RDC_MasterId_741_editmodelsaving(object e)
		{
			bool IsCancelled = false;

			var Item = (Entity.IDMS_RDC_Details)e;

			IsCancelled = !await CheckFieldsValidation(Item);

			return IsCancelled;
		}

		/// <summary>
		/// متد برای به‌روزرسانی وضعیت دکمه بعد از ذخیره شدن ردیف
		/// این متد باید بعد از SaveGrid_IDMS_RDC_Details در فایل razor فراخوانی شود
		/// </summary>
		/// <returns></returns>
		public async Task UpdateDetailsGridAddButtonAfterSave()
		{
			// بعد از ذخیره، بررسی می‌کنیم که آیا ردیفی وجود دارد
			// اگر وجود داشت، دکمه جدید را مخفی می‌کنیم
			await ToggleDetailsGridAddButton();

			// به‌روزرسانی شمارنده
			var currentCount = _Entity.IDMS_RDC_Details?.Count(x => x.IsDelete != true) ?? 0;
			_lastDetailsCount = currentCount;
		}

		/// <summary>
		/// تابع بعد اجرا شدن مدل جزئیات
		/// این متد زمانی فراخوانی می‌شود که مودال باز می‌شود.
		/// بررسی می‌کند که آیا ردیفی در IDMS_RDC_Details ذخیره شده است یا نه.
		/// اگر ردیفی ذخیره شده باشد، دکمه جدید مخفی می‌شود.
		/// </summary>
		/// <param name="item">ردیف جزئیات که در مودال نمایش داده می‌شود</param>
		/// <returns></returns>
		public async Task GridIDMS_RDC_MasterId_741_afterrendermodal(Entity.IDMS_RDC_Details item)
		{
			// بررسی می‌کنیم که آیا یک ردیف غیر حذف‌شده در لیست وجود دارد
			// این بررسی دقیق‌تر است چون ممکن است item جدید باشد اما هنوز ذخیره نشده باشد
			var hasSavedRecord = _Entity.IDMS_RDC_Details?.Any(x => x.IsDelete != true) == true;

			// اگر item مشخص شده باشد و Id داشته باشد، مطمئن می‌شویم که ذخیره شده است
			if (item != null && item.Id != Guid.Empty && item.IsDelete != true)
			{
				// بررسی می‌کنیم که آیا این ردیف واقعاً در لیست وجود دارد
				var existsInList = _Entity.IDMS_RDC_Details?.Any(x => x.Id == item.Id && x.IsDelete != true) == true;
				if (existsInList)
				{
					hasSavedRecord = true;
				}
			}

			await ToggleDetailsGridAddButton(hasSavedRecord, isInModal: true);
		}


		/// <summary>
		/// تابع قبل اجرا شدن ورودی ثبت مدل جزئیات
		/// بر اساس گزینه «آیا این فرایند منتج از فرایند دیگری است؟»، کنترل های مربوطه نمایش داده یا مخفی می شوند.
		/// </summary>
		/// <param name="selected"></param>
		/// <returns></returns>
		public async Task IsResultOfAnotherProcess_oninput(ChangeEventArgs selected)
		{
			Console.WriteLine("=== IsResultOfAnotherProcess_oninput شروع شد ===");

			bool isChecked = false;
			if (selected?.Value != null)
			{
				bool.TryParse(selected.Value.ToString(), out isChecked);
			}

			Console.WriteLine($"#Log: isChecked = {isChecked}");
			Console.WriteLine($"#Log: selected?.Value = {selected?.Value}");

			_Entity.IsResultOfAnotherProcess = isChecked;

			// اگر کاربر checkbox را از true به false تغییر داد، باید داده‌های فیلدها را null کنیم
			if (!isChecked)
			{
				Console.WriteLine("#Log: وارد بخش !isChecked شد - شروع Reset کردن dropdown ها");

				//// null کردن مقدار IDMS_RDC_AllData در Entity
				//_Entity.IDMS_RDC_AllData = null;

				// Reset کردن dropdown ها
				try
				{
					Console.WriteLine("#Log: بررسی Ref_TrackingCode...");
					Console.WriteLine($"#Log: Ref_TrackingCode == null? {Ref_TrackingCode == null}");

					//// برای TrackingCode که یک Dropdown معمولی است (DisplayFormat="DropDown")
					//if (Ref_TrackingCode != null)
					//{
					//	// مستقیماً مقدار dropdown را reset می‌کنیم بدون فراخوانی OnItemSelected
					//	Ref_TrackingCode.DropdownSelected = "انتخاب کنید";
					//}

					// برای IDMS_RDC_AllData که یک DataGrid است (DisplayFormat="DataGrid")
					// فقط مقدار Entity را null می‌کنیم و StateHasChanged را فراخوانی می‌کنیم
					// DataGrid خودش با تغییر مقدار Entity به‌روزرسانی می‌شود
					// اگر نیاز به reset کامل DataGrid بود، می‌توان از Ref_IDMS_RDC_AllData.RefDataGrid استفاده کرد
					// اما چون RefDataGrid private است، فقط مقدار Entity را null می‌کنیم

					// کدهای قبلی ResetDropdown - کامنت شده برای استفاده احتمالی در آینده
					if (Ref_TrackingCode != null)
					{
						Console.WriteLine("#Log: Ref_TrackingCode null نیست - فراخوانی ResetDropdown...");
						await Ref_TrackingCode.ResetDropdown();
						Console.WriteLine("#Log: Ref_TrackingCode.ResetDropdown() اجرا شد");
					}
					else
					{
						Console.WriteLine("#Log: ⚠️ Ref_TrackingCode null است - نمی‌توان ResetDropdown را فراخوانی کرد");
					}

					Console.WriteLine("#Log: بررسی Ref_IDMS_RDC_AllData...");
					Console.WriteLine($"#Log: Ref_IDMS_RDC_AllData == null? {Ref_IDMS_RDC_AllData == null}");

					if (Ref_IDMS_RDC_AllData != null)
					{
						Console.WriteLine("#Log: Ref_IDMS_RDC_AllData null نیست - فراخوانی ResetDropdown...");
						await Ref_IDMS_RDC_AllData.ResetDropdown();
						Console.WriteLine("#Log: Ref_IDMS_RDC_AllData.ResetDropdown() اجرا شد");
					}
					else
					{
						Console.WriteLine("#Log: ⚠️ Ref_IDMS_RDC_AllData null است - نمی‌توان ResetDropdown را فراخوانی کرد");
					}
				}
				catch (Exception ex)
				{
					// در صورت بروز خطا، فقط مقدار Entity را null نگه می‌داریم
					Console.WriteLine($"#Log: ❌ خطا در Reset کردن dropdown ها: {ex.Message}");
					Console.WriteLine($"#Log: StackTrace: {ex.StackTrace}");
				}

				// به‌روزرسانی UI
				Console.WriteLine("#Log: فراخوانی StateHasChanged...");
				await InvokeAsync(StateHasChanged);
				Console.WriteLine("#Log: StateHasChanged اجرا شد");
			}
			else
			{
				Console.WriteLine("#Log: isChecked = true - نیازی به Reset نیست");
			}

			// سپس visibility را تنظیم می‌کنیم (مخفی یا نمایش)
			Console.WriteLine($"#Log: فراخوانی ToggleResultOfAnotherProcessFields با isChecked = {isChecked}...");
			await ToggleResultOfAnotherProcessFields(isChecked);
			Console.WriteLine("#Log: ToggleResultOfAnotherProcessFields اجرا شد");

			Console.WriteLine("=== IsResultOfAnotherProcess_oninput پایان ===");
		}

		#endregion

		#region Grid Events: IDMS_TestModel

		/// <summary>
		/// تابع قبل اجرا شدن ثبت مدل جزئیات
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public async Task<bool> GridIDMS_RDC_MasterId_753_editmodelsaving(object e)
		{
			bool IsCancelled = false;

			return IsCancelled;
		}

		/// <summary>
		/// تابع بعد اجرا شدن مدل جزئیات
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public async Task GridIDMS_RDC_MasterId_753_afterrendermodal(Entity.IDMS_TestModel item)
		{
		}

		/// <summary>
		/// تابع قبل اجرا شدن ثبت مدل جزئیات
		/// بر اساس ردیف جزئیات انتخاب شده، مدل آزمون مربوطه تکمیل می‌شود.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public async Task GridIDMS_RDC_MasterId_753_customizeeditmodel(GridCustomizeEditModelEventArgs e)
		{
			// if (e.IsNew)
			// {
			// 	var newTestModel = (Entity.IDMS_TestModel)e.EditModel;
			// 	var activeDetail = _Entity.IDMS_RDC_Details?.FirstOrDefault(x => x.IsDelete != true);

			// 	if (activeDetail == null)
			// 	{
			// 		await _MSG.ShowWarning("ابتدا یک ردیف در بخش «جزئیات سیستم تحقیق و توسعه» ثبت کنید.");
			// 		await Grid_IDMS_TestModel?.CancelEditAsync();
			// 		return;
			// 	}

			// 	newTestModel.IDMS_RDC_DetailsId = activeDetail.Id;
			// 	newTestModel.IDMS_RDC_Details = activeDetail;

			// 	// if (Ref_IDMS_TestModel_IDMS_RDC_DetailsId != null)
			// 	// {
			// 	// 	Ref_IDMS_TestModel_IDMS_RDC_DetailsId.SetEntity(activeDetail);
			// 	// 	await Task.Delay(100);
			// 	// 	await Ref_IDMS_TestModel_IDMS_RDC_DetailsId.LoadData();
			// 	// }
			// 	StateHasChanged();
			// }
		}

		#endregion

		#region Dropdown Events

		/// <summary>
		/// تابع قبل اجرا شدن ورودی ثبت مدل جزئیات
		/// بر اساس کد پیگیری انتخاب شده، داده‌های جزئیات تازه‌سازی می‌شوند.
		/// </summary>
		/// <param name="Selected"></param>
		/// <returns></returns>
		// public async Task TrackingCode_onitemselected(dynamic Selected, Entity.IDMS_RDC_Master Item)
		public async Task TrackingCode_onitemselected(dynamic Selected)
		{
			// اگر Selected null باشد (مثلاً وقتی ResetDropdown فراخوانی می‌شود)، از متد خارج می‌شویم
			if (Selected == null)
			{
				Console.WriteLine("#Log: Selected is null - dropdown was reset");
				return;
			}

			Console.WriteLine("#Log:0::");

			var jsonSelected = await Utility.JSON.ToJson(Selected);
			Console.WriteLine(jsonSelected);

			string selectedTrackingCode = Selected.RequestTrakingCode;

			Console.WriteLine("#Log:1::" + selectedTrackingCode);

			if (string.IsNullOrWhiteSpace(selectedTrackingCode))
			{
				Console.WriteLine("#Log: Selected tracking code is empty");
				return; // یا LoadData("") ارسال شود
			}

			// درست: مدل اصلی QueryBuilderFilter
			QueryBuilderFilterRule filter = new QueryBuilderFilterRule()
			{
				Condition = "AND",
				Rules = new List<QueryBuilderFilterRule>()
				{
					new QueryBuilderFilterRule()
					{
						Id = "RequestTrakingCode",
						Field = "RequestTrakingCode",
						Type = "string",
						Input = "text",
						Operator = "equal",
						Value = new string[] { selectedTrackingCode }
					}
				}
			};

			await Task.Delay(100);

			Console.WriteLine("#Log:3::" + filter);

			// LoadData رشته می‌خواهد
			await Ref_IDMS_RDC_AllData.Search(filter);

			//StateHasChanged();
		}

		/// <summary>
		/// Handler برای انتخاب آیتم در dropdown IDMS_RDC_AllData
		/// </summary>
		/// <param name="_Item">آیتم انتخاب شده</param>
		public async Task ItemSelected_IDMS_RDC_MasterIDMS_RDC_AllData(object _Item)
		{
			// اگر _Item null باشد (مثلاً وقتی ResetDropdown فراخوانی می‌شود)، مقدار را null می‌کنیم
			if (_Item == null)
			{
				_Entity.IDMS_RDC_AllData = null;
				return;
			}

			try
			{
				var jsonResult = await Utility.JSON.ToJson(_Item);
				if (jsonResult == null)
				{
					_Entity.IDMS_RDC_AllData = null;
					return;
				}

				var Selected = await Utility.JSON.ToObject<dynamic>(jsonResult);

				// اگر Selected null باشد یا MasterId وجود نداشته باشد، مقدار را null می‌کنیم
				if (Selected == null)
				{
					_Entity.IDMS_RDC_AllData = null;
					return;
				}

				_Entity.IDMS_RDC_AllData = Selected.MasterId;
			}
			catch
			{
				// در صورت بروز هرگونه خطا، مقدار را null می‌کنیم
				_Entity.IDMS_RDC_AllData = null;
			}
		}

		#endregion

		#region UI Control Helpers

		/// <summary>
		/// تبدیل تاریخ شمسی RequestedDueDate_Fa به میلادی RequestedDueDate
		/// این متد برای تمام ردیف‌های IDMS_RDC_Details که غیر حذف‌شده هستند، تاریخ را تبدیل می‌کند
		/// </summary>
		private void PrepareRequestedDueDatesForSubmit()
		{
			if (_Entity?.IDMS_RDC_Details == null)
				return;

			foreach (var detail in _Entity.IDMS_RDC_Details.Where(x => x.IsDelete != true))
			{
				if (!string.IsNullOrWhiteSpace(detail.RequestedDueDate_Fa))
				{
					// بررسی معتبر بودن تاریخ شمسی و تبدیل به میلادی
					if (PersianDateUtils.TryParseDateString(detail.RequestedDueDate_Fa, out var parts))
					{
						// تبدیل تاریخ شمسی به میلادی
						detail.RequestedDueDate = PersianDateUtils.ToGregorian(detail.RequestedDueDate_Fa);
					}
					else
					{
						// تاریخ نامعتبر → میلادی null
						detail.RequestedDueDate = null;
						Console.WriteLine($"تاریخ شمسی نامعتبر: {detail.RequestedDueDate_Fa}");
					}
				}
				else
				{
					// اگر تاریخ شمسی خالی باشد، تاریخ میلادی را null می‌کنیم
					detail.RequestedDueDate = null;
				}
			}
		}


		/// <summary>
		/// تابع قبل اجرا شدن ورودی ثبت مدل جزئیات
		/// بر اساس تعداد ردیف جزئیات انتخاب شده، مشخص می شود که آیا دکمه افزودن جزئیات قابل نمایش است یا خیر.
		/// فقط و فقط یک ردیف جزئیات مجاز است، بنابراین اگر یک ردیف غیر حذف‌شده وجود داشته باشد، دکمه افزودن مخفی می‌شود.
		/// 
		/// منطق:
		/// - اگر رکوردی در IDMS_RDC_Details وجود نداشته باشد → دکمه جدید نمایش داده می‌شود
		/// - اگر یک ردیف ذخیره شده وجود داشته باشد → دکمه جدید مخفی می‌شود
		/// </summary>
		/// <param name="hasSavedRecord">آیا ردیف جزئیات ذخیره شده است؟ (اختیاری - اگر مشخص نشود، خودکار بررسی می‌شود)</param>
		/// <param name="isInModal">آیا در مودال باشیم؟</param>
		/// <returns></returns>
		private async Task ToggleDetailsGridAddButton(bool hasSavedRecord = false, bool isInModal = false)
		{
			await Task.Yield();

			// اگر hasSavedRecord مشخص نشده باشد، خودکار بررسی می‌کنیم
			// بررسی می‌کنیم که آیا یک ردیف غیر حذف‌شده در لیست وجود دارد
			if (!hasSavedRecord)
			{
				hasSavedRecord = _Entity.IDMS_RDC_Details?.Any(x => x.IsDelete != true) == true;
			}

			// اگر یک ردیف ذخیره شده وجود دارد، دکمه افزودن را مخفی می‌کنیم
			// چون فقط یک ردیف مجاز است
			if (hasSavedRecord)
			{
				// ردیف وجود دارد → دکمه جدید را مخفی می‌کنیم
				await JS.InvokeVoidAsync("AddClass", "#IDMS_RDC_Details_GridIDMS_RDC_MasterId_741ButtonNew", "d-none");
			}
			else
			{
				// ردیف وجود ندارد → دکمه جدید را نمایش می‌دهیم
				await JS.InvokeVoidAsync("RemoveClass", "#IDMS_RDC_Details_GridIDMS_RDC_MasterId_741ButtonNew", "d-none");
			}

			// فقط اگر در مودال باشیم، دکمه‌های مودال را مخفی کن
			if (isInModal)
			{
				// دکمه ذخیره و جدید - مخفی می‌شود چون فقط یک ردیف مجاز است
				await JS.InvokeVoidAsync("ModalAddClass", "#IDMS_RDC_Details_GridIDMS_RDC_MasterId_741ButtonSaveAndNew", "d-none");
				// دکمه قبلی - مخفی می‌شود چون فقط یک ردیف وجود دارد
				await JS.InvokeVoidAsync("ModalAddClass", "#IDMS_RDC_Details_GridIDMS_RDC_MasterId_741ButtonBefore", "d-none");
				// دکمه بعدی - مخفی می‌شود چون فقط یک ردیف وجود دارد
				await JS.InvokeVoidAsync("ModalAddClass", "#IDMS_RDC_Details_GridIDMS_RDC_MasterId_741ButtonNext", "d-none");
			}
		}

		/// <summary>
		/// نمایش یا مخفی‌سازی فیلدهای مرتبط با «منتج از فرایند دیگر بودن»
		/// </summary>
		/// <param name="showFields">اگر true باشد فیلدها نمایش داده می‌شوند</param>
		private async Task ToggleResultOfAnotherProcessFields(bool showFields)
		{
			Ref_TrackingCode?.SetVisible(showFields);
			Ref_IDMS_RDC_AllData?.SetVisible(showFields);
			await InvokeAsync(StateHasChanged);
		}

		#endregion

		#region Dialogs

		/// <summary>
		/// تابع قبل اجرا شدن ورودی ثبت مدل جزئیات
		/// </summary>
		/// <returns></returns>
		private async Task ShowRequestIncompleteDialog()
		{
			var options = new ConfirmDialogOptions
			{
				YesButtonText = "بازگشت به درخواست",
				YesButtonColor = ButtonColor.Danger,
				NoButtonText = "",
			};

			string html = $@"
                <div>
                    <picture><img src='https://file.workcv.ir/fa/api/v1/File/Get?FileID=6e5b6fb8-a5b2-490c-f83f-08dbea5b8061  ' width='96px' alt='لوگو پل فیلم' /></picture>
                    <hr class='hrdash border-success-subtle'>
                </div>
                <div class='fw-bold text-center'>
                    <span class='fs-5'>کد پیگیری این درخواست: </span>
                    <span class='fs-3' style='color: #1ba156'>{_Entity.RequestTrakingCode}</span>
                    <div>
                        <span><i class='fal fa-exclamation-triangle' style='font-size:24px; color:red;'></i>&nbsp;</span>
                        <span class='fs-6 text-secondary'>تا کنون هیچ ردیف درخواستی تکمیل نشده است. لطفاً برای ثبت و ادامه به مرحله بعد حداقل یک ردیف در درخواست خود ثبت نمایید.</span>
                    </div>
                </div>";

			await Confirm.ShowAsync(title: "", message1: html, confirmDialogOptions: options);
		}

		#endregion


		#endregion FunctionEvents

	}
}