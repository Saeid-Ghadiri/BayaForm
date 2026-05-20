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
	public class Form_1112Base : Form_1112Peropeties
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

			// دکمه ثبت و ادامه
			if (BtnWorkFlowId == "SequenceFlow_0zkpmcm")
			{
				foreach (var Item in List)
				{
					IsValid = IsValid && await CheckFieldValidation(Item);
				}
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

			if (string.IsNullOrWhiteSpace(item.PlaceOfUse))
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه محل مصرف را تکمیل نمایید.");
			}

			if (item.Global_SCMRequestTypeId == null)
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه نوع درخواست را تکمیل نمایید.");
			}
			else
			{
				string requestTypeId = item.Global_SCMRequestTypeId.ToString();

				// شناسه‌ مرتبط با "تحویل"
				bool isDelivery = requestTypeId == "a9c5df1c-d99c-ef11-8354-005056a02a64";
				// شناسه مرتبط با تحویل و خرید به صورت مشترک
				bool isBoth = requestTypeId == "73f6c459-d99c-ef11-8354-005056a02a64";
				// شناسه "خرید" (در صورت نیاز به اعتبارسنجی جداگانه)
				bool isPurchase = requestTypeId == "3b9e934d-d99c-ef11-8354-005056a02a64";

				// اعتبارسنجی‌های مربوط به تحویل (برای حالت‌های فقط تحویل و تحویل+خرید)
				if (isDelivery || isBoth)
				{
					if (item.NumberofGoodsDelivery == null)
					{
						isValid = false;
						await _MSG.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا را تکمیل نمایید");
					}

					//if (item.DeliveryCode != item.GetDeliveryCode)
					//{
					//	isValid = false;
					//	await _MSG.ShowError("کد تحویل وارد شده صحیح نیست! لطفاً کد صحیح را وارد نمایید.");
					//}
				}

				//  برای حالت "فقط خرید"
				if (isPurchase || isBoth)
				{
					if (item.DeficitSupplyNumber == null || item.DeficitSupplyNumber <= 0)
					{
						isValid = false;
						await _MSG.ShowError("تعداد تأمین کسری باید بزرگ‌تر از صفر باشد.");
					}
				}
			}

			return isValid;
		}

		/// <summary>
		/// بررسی تعداد ردیف های جزییات درخواست کالا
		/// </summary>
		public async Task<bool> ValidateMaxFiveProductRequestRecords()
		{
			int activeCount = _Entity.SCMATLASCELL_ProductRequestDetails
								.Count(p => p.IsDelete != true);

			if (activeCount >= 5)
			{
				await _MSG.ShowError("تعداد رکوردها نمی‌تواند بیشتر از ۵ باشد.");
				return false;
			}
			return true;
		}

		/// <summary>
		/// بررسی کد تحویل وارد شده توسط انباردار
		/// </summary>
		public async Task<bool> CheckDeliveryCode()
		{
			var activeItems = _Entity.SCMATLASCELL_ProductRequestDetails
										.Where(p => p.IsDelete != true)
										.ToList();

			foreach (var item in activeItems)
			{
				// اگر DeliveryCode و GetDeliveryCode هر دو خالی باشند خطا نمی‌دهیم
				if (item.DeliveryCode != item.GetDeliveryCode)
				{
					await _MSG.ShowError("کد تحویل وارد شده صحیح نیست! لطفاً کد صحیح را وارد نمایید.");
					return false;
				}
			}

			// کد صحیح است → تاریخ تحویل را ثبت کن
			await StampDeliveryDate();

			return true;
		}

		/// <summary>
		/// ثبت و تبدیل تاریخ تحویل کالا به درخواست‌دهنده
		/// </summary>
		public async Task StampDeliveryDate()
		{
			// فقط رکوردهای فعالی که کد تحویل برایشان ثبت شده را در نظر می‌گیریم
			var activeItems = _Entity.SCMATLASCELL_ProductRequestDetails
								.Where(p => p.IsDelete != true && p.GetDeliveryCode != null)
								.ToList();

			foreach (var item in activeItems)
			{
				// استفاده از کتابخانه PersianDateUtils برای دریافت تاریخ شمسی امروز
				item.DateTimeDeliveryCode = DateUtils.PersianDateUtils.ToPersian(DateTime.Now);
			}
		}
		#endregion

		#region مدیریت وضعیت های تحویل و خرید کالا

		/// <summary>
		/// تنظیم وضعیت نمایش فیلدهای تحویل/خرید بر اساس نوع درخواست انتخاب‌شده
		/// </summary>
		/// <param name="item">ردیف جاری درخواست</param>
		/// </summary>
		public async Task ApplyRequestTypeVisibility(Entity.SCMATLASCELL_ProductRequestDetails item)
		{
			string requestTypeId = item.Global_SCMRequestTypeId?.ToString();

			// اگر نوع درخواست انتخاب نشده باشد، همه فیلدها مخفی شوند
			if (string.IsNullOrEmpty(requestTypeId))
			{
				SetDeliveryAndPurchaseFieldsVisibility(false, item);
				return;
			}

			// بر اساس GUID نوع درخواست رفتار کن
			switch (requestTypeId)
			{
				case "a9c5df1c-d99c-ef11-8354-005056a02a64": // فقط تحویل
					SetDeliveryFieldsVisibility(true, item);
					SetPurchaseFieldsVisibility(false, item);
					break;

				case "3b9e934d-d99c-ef11-8354-005056a02a64": // فقط خرید
					SetDeliveryFieldsVisibility(false, item);
					SetPurchaseFieldsVisibility(true, item);
					break;

				case "73f6c459-d99c-ef11-8354-005056a02a64": // تحویل + خرید
					SetDeliveryAndPurchaseFieldsVisibility(true, item);
					break;

				default:
					// GUID ناشناخته – مخفی‌سازی کامل
					SetDeliveryAndPurchaseFieldsVisibility(false, item);
					break;
			}
		}

		/// <summary>
		/// نمایش یا مخفی‌سازی فیلدهای بخش "تحویل"
		/// </summary>
		public void SetDeliveryFieldsVisibility(bool isVisible, Entity.SCMATLASCELL_ProductRequestDetails item)
		{
			Ref_SCMATLASCELL_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(isVisible);
			item.GoodsDeliveryIsEnable = isVisible;

			// Ref_InsertDeliveryCode.SetVisible(isVisible);
			Ref_SCMATLASCELL_ProductRequestDetails_NumberofGoodsDelivery.SetVisible(isVisible);
			//Ref_SCMATLASCELL_ProductRequestDetails_GetDeliveryCode?.SetVisible(isVisible);
		}

		/// <summary>
		/// نمایش یا مخفی‌سازی فیلدهای بخش "خرید"
		/// </summary>
		public void SetPurchaseFieldsVisibility(bool isVisible, Entity.SCMATLASCELL_ProductRequestDetails item)
		{
			Ref_SCMATLASCELL_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(isVisible);
			item.DeficitSupplyIsEnable = isVisible;

			Ref_SCMATLASCELL_ProductRequestDetails_DeficitSupplyNumber.SetVisible(isVisible);
			// در صورت نیاز:
			// Ref_SCMATLASCELL_ProductRequestDetails_HasOrderPoint.SetVisible(isVisible);
		}

		/// <summary>
		/// نمایش یا مخفی‌سازی فیلدهای بخش "تحویل + خرید" (هر دو)
		/// </summary>
		public void SetDeliveryAndPurchaseFieldsVisibility(bool isVisible, Entity.SCMATLASCELL_ProductRequestDetails item)
		{
			SetDeliveryFieldsVisibility(isVisible, item);
			SetPurchaseFieldsVisibility(isVisible, item);
		}

		/// <summary>
		/// نمایش یا مخفی‌سازی فیلدهای مخصوص انباردار (ثبت کد و مقدار واگذاری)
		/// </summary>
		public void SetWarehouseFieldsVisibility(bool isVisible)
		{
			Ref_SCMATLASCELL_ProductRequestDetails_NumberofGoodsDelivery.SetVisible(isVisible);
			//Ref_SCMATLASCELL_ProductRequestDetails_GetDeliveryCode?.SetVisible(isVisible);
		}

		#endregion

		/// <summary>
		/// تکمیل فیلدهای بایا بر اساس فیلدهای شماران سیستم
		/// </summary>
		/// <param name="Selected">شیء داینامیک حاوی اطلاعات کالا از سیستم شماران</param>
		/// <param name="Item">شیء جزئیات درخواست محصول که قرار است فیلدهای آن مقداردهی شود</param>
		/// <returns>یک Task غیرهمزمان (بدون مقدار بازگشتی)</returns>
		public async Task ProductSearch_NotMapped_onitemselected(dynamic Selected, Entity.SCMATLASCELL_ProductRequestDetails Item)
		{
			// نام کالا
			Item.SH_DESC = Selected.DESC;
			// کد کالا شماران
			Item.SH_PARTNO = Selected.PARTNO;
			// شناسه سیستمی کد کالا شماران
			Item.SH_PARTNO_GUID = Selected.PARTNO_GUID;
			// شماره کالا شماران
			Item.SH_PARTCODE = Selected.PARTCODE;
			// شناسه شماره کالا شماران
			Item.SH_PARTCODE_GUID = Selected.PARTCODE_GUID;
			// واحد کالا شماران
			Item.SH_UNIT = Selected.UNIT;
			// کد دسته بندی فرعی کالا شماران
			Item.SH_SUBGRCODE = Selected.SUBGRCODE;
			// شناسه کد دسته بندی فرعی کالا شماران
			Item.SH_SUBGRCODE_GUID = Selected.SUBGRCODE_GUID;
			// کد دسته بندی اصلی شماران
			Item.SH_GRCODE = Selected.GRCODE;
			// شناسه کد دسته بندی اصلی کالا شماران
			Item.SH_GRCODE_GUID = Selected.GRCODE_GUID;
			// نام دسته بندی اصلی کالا شماران
			Item.SH_GroupName = Selected.GroupName;
			// نام دسته بندی فرعی کالا شماران
			Item.SH_SubGroupName = Selected.SubGroupName;
			// سال مالی کالا شماران
			Item.SH_YEAR = Selected.YEAR;
			// کالا موجود است یا خیر شماران
			Item.SH_IsExist = Selected.IsExist;
			// نام شرکت کالا شماران
			Item.SH_Factory = Selected.Factory;
			// کد گروه اصلی کالا شماران
			Item.SH_MapGroupCode = Selected.MapGroupCode;

			// موجودی فقط در صورتی مقداردهی شود که مقدار معتبری داشته باشد
			if (Selected.Amount > -1)
			{
				Item.SH_Amount = (double)Selected.Amount;
			}
		}

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

			// اعمال وضعیت نمایش فیلدها بر اساس نوع درخواست (تحویل/خرید)
			if (Item.Global_SCMRequestTypeId == null)
			{
				// اگر نوع درخواست انتخاب نشده، همه فیلدهای تحویل و خرید مخفی شوند
				SetDeliveryAndPurchaseFieldsVisibility(false, Item);
			}
			else
			{
				await ApplyRequestTypeVisibility(Item);
			}
		}

		public async Task InsertDeliveryCode_oninput(ChangeEventArgs Selected)
		{
			if (!int.TryParse(Selected.Value?.ToString(), out int code))
			{
				await _MSG.ShowError("کد تحویل باید یک عدد معتبر باشد.");
				return;
			}

			var items = _Entity.SCMATLASCELL_ProductRequestDetails.ToList();

			foreach (var item in items)
			{
				item.GetDeliveryCode = code;
			}

			StateHasChanged();
		}

		public async Task Global_SCMRequestTypeId_onitemselected(Entity.Global_SCMRequestType Selected, Entity.SCMATLASCELL_ProductRequestDetails Item)
		{
			// Console.WriteLine(await Utility.JSON.ToJson(Selected));
			try
			{
				await ApplyRequestTypeVisibility(Item);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
		#endregion FunctionEvents

	}
}