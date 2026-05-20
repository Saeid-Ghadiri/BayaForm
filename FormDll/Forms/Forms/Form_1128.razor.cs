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
    public class Form_1128Base : Form_1128Peropeties
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

				
			}
		}

		/// <summary>
		/// اعتبار سنجی فرم
		/// </summary>
		/// <returns></returns>
		public override async Task<bool> FormValidator()
		{
			bool IsValid = true;

			var List = _Entity.SCMATLASBOBIN_ProductRequestDetails
							.Where(p => p.IsDelete != true)
							.ToList();

			Console.WriteLine($"تعداد ردیف‌های معتبر: {List.Count}");

			if (List.Count == 0)
			{
				IsValid = false;
				await ShowEmptyRequestDialogAsync(_Entity.RequestTrakingCode);
			}

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

		#region بررسی تعداد ردیف های جزییات درخواست کالا
		public async Task<bool> ValidateMaxFiveProductRequestRecords(bool isNewRow = false)
		{
			int activeCount = _Entity.SCMATLASBOBIN_ProductRequestDetails.Count(p => p.IsDelete != true);
			if (activeCount >= 5)
			{
				await _MSG.ShowError("تعداد رکوردها نمی‌تواند بیشتر از ۵ باشد.");
				return false;
			}
			return true;
		}
		#endregion

		#region اعتبارسنجی فیلدها
		public async Task<bool> CheckFieldValidation(Entity.SCMATLASBOBIN_ProductRequestDetails Item)
		{
			bool IsValid = true;

			// شرط پُر بودن فیلد نام کالا (از نوع string)
			if (string.IsNullOrWhiteSpace(Item.SH_DESC))
			{
				IsValid = false;
				await _MSG.ShowError("لطفا گزینه نام کالا را تکمیل نمایید.");
			}

			// شرط پُر بودن فیلد تعداد یا مقدار درخواستی (عدد صحیح یا اعشاری)
			if (Item.ProductRequestingQTY == null || Item.ProductRequestingQTY == 0)
			{
				IsValid = false;
				await _MSG.ShowError("لطفا گزینه تعداد یا مقدار درخواستی را تکمیل نمایید.");
			}

			// شرط پُر بودن فیلد اولویت (شناسه از نوع nullable)
			if (Item.Global_PriorityId == null)
			{
				IsValid = false;
				await _MSG.ShowError("لطفا گزینه اولویت را تکمیل نمایید.");
			}

			// شرط پُر بودن فیلد محل مصرف (از نوع string)
			if (string.IsNullOrWhiteSpace(Item.PlaceOfUse))
			{
				IsValid = false;
				await _MSG.ShowError("لطفا گزینه محل مصرف را تکمیل نمایید.");
			}

			return IsValid;
		}
		#endregion

		/// <summary>
		/// تکمیل فیلدهای بایا بر اساس فیلدهای شماران سیستم
		/// </summary>
		/// <param name="Selected">شیء داینامیک حاوی اطلاعات کالا از سیستم شماران</param>
		/// <param name="Item">شیء جزئیات درخواست محصول که قرار است فیلدهای آن مقداردهی شود</param>
		/// <returns>یک Task غیرهمزمان (بدون مقدار بازگشتی)</returns>
		public async Task ProductSearch_NotMapped_onitemselected(dynamic Selected, Entity.SCMATLASBOBIN_ProductRequestDetails Item)
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

			// شماره کالا شماران (در صورت نیاز می‌توان فعال کرد)
			//Item.PARTCODE = Selected.PARTCODE;
		}


		public async Task<bool> GridSCMATLASBOBIN_ProductRequestId_847_editmodelsaving(object e)
		{
			bool IsCancelled = false;

			var Item = e as Entity.SCMATLASBOBIN_ProductRequestDetails;

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
				
			}

			// بررسی اعتبارسنجی فیلدها
			IsCancelled = !await CheckFieldValidation(Item);

			return IsCancelled;
		}

		

		public async Task GridSCMATLASBOBIN_ProductRequestId_847_afterrendermodal(Entity.SCMATLASBOBIN_ProductRequestDetails Item)
		{
			
		}

		


		#endregion FunctionEvents

	}
}
