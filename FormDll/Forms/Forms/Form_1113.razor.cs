using Baya.Models.Utility;
using BlazorBootstrap;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Sitko.Blazor.CKEditor;
using System;
using System.Net;

namespace Forms.Forms
{
	public class Form_1113Base : Form_1113Peropeties
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
				// فراخوانی سرویس تُست
				_MSG = new MSG(toastService);

				await ToggleNewButtonBasedOnCount();
			}
		}

		/// <summary>
		/// اعتبار سنجی فرم
		/// </summary>
		/// <returns></returns>
		public override async Task<bool> FormValidator()
		{
			bool IsValid = true;

			var List = _Entity.SCMATLASCELL_ProductRequestDetails
							.Where(p => p.IsDelete != true)
							.ToList();
			// بررسی خالی نبودن گرید فرم
			if (List.Count == 0)
			{
				IsValid = false;
				await ShowEmptyRequestDialogAsync(_Entity.RequestTrakingCode);
			}

			// 
			foreach (var Item in List)
			{
				IsValid = IsValid && await CheckFieldValidation(Item);
			}

			return IsValid;
		}


		/// <summary>
		/// تابع قبل اجرا شدن ارسال داده
		/// </summary>
		/// <returns></returns>
		public override async Task<Result> BeforSubmit()
		{
			// await StampDeliveryDate();

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

		#region نمایش دیالوگ خطا برای درخواست بدون ردیف
		/// <summary>
		/// نمایش دیالوگ خطا برای درخواست بدون ردیف
		/// </summary>
		/// <param name="trackingCode">کد پیگیری درخواست</param>
		private async Task ShowEmptyRequestDialogAsync(string trackingCode)
		{
			var options = new ConfirmDialogOptions
			{
				YesButtonText = "بازگشت به درخواست",
				YesButtonColor = ButtonColor.Danger,
				NoButtonText = "",
			};

			string htmlString = $@"
				<div class='request-header'>
					<picture>
						<img src='https://file.workcv.ir/fa-ir/api/v1/File/Get?FileID=e35322d0-23f4-4d03-b886-08de9f64ab6f' 
							 class='logo-image' 
							 alt='لوگو اطلس سلولز' 
							 width='96'>
					</picture>
					<hr class='hrdash border-success-subtle'>
				</div>

				<div class='fw-bold text-center'>
					<div class='mb-3'>
						<span class='fs-5'>کد پیگیری این درخواست: </span>
						<span class='fs-3' style='color: #1ba156'>{trackingCode}</span>
					</div>

					<div class='d-flex justify-content-center align-items-start gap-2'>
						<i class='fal fa-exclamation-triangle' style='font-size:24px; color:red;' aria-hidden='true'></i>
						<span class='fs-6 text-secondary text-end'>
							تا کنون هیچ ردیف درخواستی تکمیل نشده است. لطفا برای ثبت و ادامه به مرحله بعد حداقل یک ردیف در درخواست خود ثبت نمایید.
						</span>
					</div>
				</div>";

			await Confirm.ShowAsync(
				title: "",
				message1: htmlString,
				confirmDialogOptions: options
				);
		}
		#endregion

		#region اعتبارسنجی فیلدها
		/// <summary>
		/// اعتبارسنجی یک ردیف درخواست کالا (تحویل/خرید)
		/// </summary>
		/// <param name="item">ردیف جاری</param>
		/// <returns>true در صورت معتبر بودن</returns>
		public async Task<bool> CheckFieldValidation(Entity.SCMATLASCELL_ProductRequestDetails item)
		{
			bool isValid = true;

			// فیلدهای مشترک و اجباری برای تمام انواع درخواست
			if (string.IsNullOrWhiteSpace(item.SH_DESC))
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه نام کالا را تکمیل نمایید.");
			}

			if (item.ProductRequestingQTY == null || item.ProductRequestingQTY == 0)
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه تعداد یا مقدار درخواستی را تکمیل نمایید.");
			}

			if (item.Global_PriorityId == null)
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه اولویت را تکمیل نمایید.");
			}

			if (string.IsNullOrWhiteSpace(item.T_TEMPNO))
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه شماره عطف کالا را تکمیل نمایید.");
			}

			return isValid;
		}

		/// <summary>
		/// بررسی تعداد ردیف های جزییات درخواست کالا
		/// </summary>
		public async Task<bool> ValidateMaxFiveProductRequestRecords()
		{
			int activeCount = _Entity.SCMATLASCELL_ProductRequestDetails.Count(p => p.IsDelete != true);
			if (activeCount >= 5)
			{
				await _MSG.ShowError("تعداد رکوردها نمی‌تواند بیشتر از ۵ باشد.");
				return false;
			}
			return true;
		}

		#endregion


		#region مدیریت فیلدهای حواله مصرف (شماران)

		/// <summary>
		/// تنظیم وضعیت نمایش فیلدهای مرتبط با حواله مصرف
		/// </summary>
		/// <param name="visible">وضعیت نمایش</param>
		public void SetHavaleMasrafFieldsVisibility(bool visible)
		{
			Ref_SCMATLASCELL_ProductRequestDetails_T_Search_NotMapped.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_T_CENTCODE.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_T_CENTCODE_GUID.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_T_PAYCENTName.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_T_CREATOR.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_T_FACTDATE.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_T_FACTNO.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_T_FACTNO_GUID.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_T_TEMPNO.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_T_TempNoNum.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_T_YEAR.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_SH_Tahvil_DTL.SetVisible(visible);
		}

		/// <summary>
		/// غیرفعال‌سازی فیلدهای حواله مصرف (حالت فقط خواندنی)
		/// </summary>
		public void DisableHavaleMasrafFields()
		{
			Ref_SCMATLASCELL_ProductRequestDetails_T_CENTCODE.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_T_CENTCODE_GUID.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_T_PAYCENTName.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_T_CREATOR.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_T_FACTDATE.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_T_FACTNO.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_T_FACTNO_GUID.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_T_TEMPNO.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_T_TempNoNum.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_T_YEAR.SetDisabled(true);
		}

		/// <summary>
		/// نمایش و غیرفعال‌سازی هم‌زمان (رفتار یکپارچه‌ی قبلی)
		/// </summary>
		public void ShowAndDisableHavaleMasrafFields()
		{
			SetHavaleMasrafFieldsVisibility(true);
			DisableHavaleMasrafFields();
		}

		/// <summary>
		/// پاک‌سازی تمامی فیلدهای حواله مصرف در یک ردیف
		/// </summary>
		/// <param name="item">ردیف جاری</param>
		public void ClearHavaleMasrafFields(Entity.SCMATLASCELL_ProductRequestDetails item)
		{
			if (item == null) return;

			item.T_CENTCODE = null;
			item.T_CENTCODE_GUID = null;
			item.T_PAYCENTName = null;
			item.T_CREATOR = null;
			item.T_FACTDATE = null;
			item.T_FACTNO = null;
			item.T_FACTNO_GUID = null;
			item.T_TEMPNO = null;
			item.T_TempNoNum = null;
			item.T_YEAR = null;
			item.SH_Tahvil_DTL = null;
		}

		/// <summary>
		/// پر کردن فیلدهای حواله مصرف بر اساس شیء انتخاب‌شده
		/// </summary>
		/// <param name="selected">شیء حاوی اطلاعات حواله (از نوع HavaleMasrafDto)</param>
		/// <param name="item">ردیف مقصد</param>
		public void FillHavaleMasrafFields(dynamic selected, Entity.SCMATLASCELL_ProductRequestDetails item)
		{
			if (selected == null || item == null) return;

			item.T_TempNoNum = selected.TempNoNum;
			item.T_PAYCENTName = selected.PAYCENTName;
			item.T_TEMPNO = selected.TEMPNO;
			item.T_CREATOR = selected.CREATOR;
			item.T_YEAR = selected.YEAR;
			item.T_CENTCODE_GUID = selected.CENTCODE_GUID;
			item.T_CENTCODE = selected.CENTCODE;
			item.T_FACTDATE = selected.FACTDATE;
			item.T_FACTNO = selected.FACTNO;
			item.T_FACTNO_GUID = selected.FACTNO_GUID;
		}

		/// <summary>
		/// بارگذاری داده‌های جزئیات تحویل در کامپوننت مربوطه
		/// </summary>
		public async Task LoadTahvilDetailsAsync(Entity.SCMATLASCELL_ProductRequestDetails item)
		{
			if (item == null) return;

			Ref_SCMATLASCELL_ProductRequestDetails_SH_Tahvil_DTL?.SetEntity(item);
			await Ref_SCMATLASCELL_ProductRequestDetails_SH_Tahvil_DTL?.LoadData();
		}

		#endregion

		public async Task<bool> GridSCMATLASCELL_ProductRequestId_817_editmodelsaving(object e)
		{
			bool IsCancelled = false;

			var Item = e as Entity.SCMATLASCELL_ProductRequestDetails;

			if (Item == null)
			{
				await _MSG.ShowError("آیتم نامعتبر است");
				return true;
			}

			// تشخیص اینکه آیا این یک ردیف جدید است (مثلاً با بررسی کلید اصلی)
			bool isNewRow = (Item.Id == Guid.Empty); // یا هر کلید اصلی دیگری مثل ProductRequestDetailId

			// بررسی محدودیت تعداد (قبل از هر چیز)
			if (isNewRow && !await ValidateMaxFiveProductRequestRecords())
				return true;

			// بررسی محدودیت ۵ ردیف فقط برای ردیف‌های جدید
			if (isNewRow)
			{
				Item.IsDelete = false;
				await ToggleNewButtonBasedOnCount(); // مخفی کردن دکمه New اگر تعداد به 5 رسید
			}

			// بررسی اعتبارسنجی فیلدها
			IsCancelled = !await CheckFieldValidation(Item);

			return IsCancelled;
		}

		// متد کمکی برای کنترل دکمه New
		private async Task ToggleNewButtonBasedOnCount()
		{
			int activeCount = _Entity.SCMATLASCELL_ProductRequestDetails
								.Count(p => p.IsDelete != true);

			string buttonId = "SCMATLASCELL_ProductRequestDetails_GridSCMATLASCELL_ProductRequestId_817ButtonNew";

			if (activeCount >= 5)
				await JS.InvokeVoidAsync("eval", $"document.getElementById('{buttonId}').classList.add('d-none')");
			else
				await JS.InvokeVoidAsync("eval", $"document.getElementById('{buttonId}').classList.remove('d-none')");
		}

		public async Task GridSCMATLASCELL_ProductRequestId_817_afterrendermodal(Entity.SCMATLASCELL_ProductRequestDetails Item)
		{
			int activeCount = _Entity.SCMATLASCELL_ProductRequestDetails
								.Count(p => p.IsDelete != true);
			// اگر 5 ردیف گرید بود بیشتر ثبت نکن و دکمه ها مخفی شود
			if (activeCount >= 5)
			{
				string saveAndNewId = "SCMATLASCELL_ProductRequestDetails_GridSCMATLASCELL_ProductRequestId_817ButtonSaveAndNew";
				await JS.InvokeVoidAsync("eval", $"document.getElementById('{saveAndNewId}').classList.add('d-none')");
			}

			if (Item.T_FACTNO_GUID.HasValue)
			{
				Ref_SCMATLASCELL_ProductRequestDetails_SH_Tahvil_DTL.SetEntity(Item);
				await Ref_SCMATLASCELL_ProductRequestDetails_SH_Tahvil_DTL.LoadData();
			}
		}

		public async Task T_Search_NotMapped_onitemselected(dynamic Selected, Entity.SCMATLASCELL_ProductRequestDetails Item)
		{

			FillHavaleMasrafFields(Selected, Item);
			await LoadTahvilDetailsAsync(Item);
		}

		public async Task T_TempNoNum_NotMapped_onitemselected(dynamic Selected)
		{
			var List = _Entity.SCMATLASCELL_ProductRequestDetails.ToList();

			_Entity.FACTNO_GUID = Selected.FACTNO_GUID;

			foreach (var item in List)
			{
				FillHavaleMasrafFields(Selected, item);
				item.T_Search_NotMapped = Selected.FACTNO_GUID;
			}

			Ref_T_HavaleDTL.LoadData();

			StateHasChanged();

		}

		#endregion FunctionEvents

	}
}