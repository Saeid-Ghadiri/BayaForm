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
	public class Form_1125Base : Form_1125Peropeties
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

			// Console.WriteLine($"تعداد ردیف‌های معتبر: {List.Count}");

			if (List.Count == 0)
			{
				IsValid = false;
				await ShowEmptyRequestDialogAsync(_Entity.RequestTrakingCode);
			}

			// Console.WriteLine("#Log FormValidator btn :");
			foreach (var Item in List)
			{
				// Console.WriteLine("#Log FormValidator btn foreach :");
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
			await GenerateUniqueDeliveryCodeForAllRows();

			await ShowDeliveryCodeConfirmationDialog();

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

		#region نمایش دیالوگ تأیید کد تحویل کالا
		/// <summary>
		/// نمایش دیالوگ تأیید کد تحویل کالا
		/// </summary>
		private async Task ShowDeliveryCodeConfirmationDialog()
		{
			var options = new ConfirmDialogOptions
			{
				YesButtonText = "ادامه فرآیند",
				YesButtonColor = ButtonColor.Success,
				NoButtonText = ""
			};

			string htmlString = $@"
				<div class='text-center'>
					<span>کد پیگیری درخواست: </span>
					<span class='fs-4 fw-bold'>{_Entity.RequestTrakingCode}</span>
					<hr class='hrdash' />
					<div>
						<span class='fw-normal'>کد تحویل کالا: </span>
						<span class='fs-3 text-green fw-bold'>{RandomDeliveryCode}</span>
						<span class='btn btn-yellow-light btn-sm mx-2 mb-2'
							  onclick='navigator.clipboard.writeText(&quot;{RandomDeliveryCode}&quot;); $(&quot;#copylable&quot;).removeClass(&quot;d-none&quot;);'>
							<i class='fal fa-copy'></i>
							<span class='px-1'>کپی</span>
						</span>
					</div>
					<p id='copylable' class='d-none text-green mt-3'>کد تحویل کالا کپی شد.</p>
					<div>
						<hr class='hrdash' />
						<span class='fal fa-exclamation-circle text-red btnicon mx-1' style='font-size: 24px;'></span>
						<span>لطفا در زمان دریافت کالا از انبار، کد فوق را به انباردار اعلام نمایید.</span>
					</div>
				</div>";

			await Confirm.ShowAsync(
				title: "",
				message1: htmlString,
				confirmDialogOptions: options
				);
		}

		/// <summary>
		/// کپی متن در کلیپ‌بورد (قابل استفاده در سراسر کلاس)
		/// </summary>
		private async Task CopyToClipboard(string text)
		{
			await JS.InvokeVoidAsync("navigator.clipboard.writeText", text);
			toastService.ShowSuccess("کد تحویل کالا کپی شد.");
		}
		#endregion

		#region مدیریت کد تحویل
		/// <summary>
		/// مدیریت کد تحویل - تولید رندوم کد تحویل
		/// </summary>
		/// <returns></returns>
		public int RandomDeliveryCode { get; set; }

		/// <summary>تولید کد رندوم ۶ رقمی و اختصاص به همه ردیف‌هایی که کد ندارند</summary>
		private async Task GenerateUniqueDeliveryCodeForAllRows()
		{
			// تولید یک کد رندوم جدید
			var rand = new Random();
			int newCode;
			do
			{
				//  کد 6 رقمی
				newCode = rand.Next(100000, 999999);
			} while (_Entity.SCMATLASBOBIN_ProductRequestDetails.Any(x => x.DeliveryCode == newCode && x.IsDelete != true));

			RandomDeliveryCode = newCode;

			// اختصاص کد به همه ردیف‌های بدون کد
			foreach (var item in _Entity.SCMATLASBOBIN_ProductRequestDetails)
			{
				if (!item.DeliveryCode.HasValue)
					item.DeliveryCode = RandomDeliveryCode;
			}
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

			// شرط پُر بودن فیلد نوع درخواست
			if (Item.Global_SCMRequestTypeId == null)
			{
				IsValid = false;
				await _MSG.ShowError("لطفا گزینه نوع درخواست را تکمیل نمایید.");
			}

			// در صورتی که خرید یا تحویل و خرید باشد گزینه تعداد تامین کسری ضروری گردد.
			if (Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64" ||
				Item.Global_SCMRequestTypeId.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64")
			{
				if (Item.DeficitSupplyNumber == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه تعداد تامین کسری را تکمیل نمایید.");
				}
			}

			// در صورتی که خرید یا تحویل و خرید باشد گزینه حدود هزینه خرید ضروری گردد.
			if (Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64" ||
				Item.Global_SCMRequestTypeId.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64")
			{
				if (Item.PurchasePrice == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه حدود قیمت کالا را تکمیل نمایید.");
				}
			}

			// شرط پُر بودن فیلد منتج از ITIL
			if (Item.ResultingFromITIL == null)
			{
				IsValid = false;
				await _MSG.ShowError("لطفا گزینه منتج از ITIL را تکمیل نمایید.");
			}

			//Iآیا کد ITIL دارد؟ 
			// if (Item.ITILCodeIsEnable == null)
			// {
			//     IsValid = false;
			//     toastService.ShowError("لطفا گزینه آیا ITIL  دارد؟ را تکمیل نمایید.",
			//     settings => {
			//         settings.Timeout = 4;
			//         settings.ShowProgressBar = true;
			//         settings.PauseProgressOnHover = true;
			//     });
			// }

			// منتج از ITIL
			// if(Item.ITILCodeIsEnable.HasValue && Item.ITILCodeIsEnable.Value)
			// {                
			//     if (Item.ITILCodeIsEnable == null)
			//     {
			//         IsValid = false;

			//         toastService.ShowError("لطفا گزینه  منتج از ITIL را تکمیل نمایید.",
			//         settings => {
			//             settings.Timeout = 4;
			//             settings.ShowProgressBar = true;
			//             settings.PauseProgressOnHover = true;
			//         });
			//     }
			// }

			return IsValid;
		}
		#endregion

		#region مدیریت ITIL
		public async Task ITILVisible(bool Visible)
		{
			Ref_SCMATLASBOBIN_ProductRequestDetails_ResultingFromITIL.SetVisible(Visible);
		}
		public async Task ITILDetailsVisible(bool Visible)
		{
			Ref_SCMATLASBOBIN_ProductRequestDetails_RequestIdITIL.SetVisible(Visible);
			Ref_SCMATLASBOBIN_ProductRequestDetails_RequestIdITIL.SetDisabled(true);
			Ref_SCMATLASBOBIN_ProductRequestDetails_RequesterUserITIL.SetVisible(Visible);
			Ref_SCMATLASBOBIN_ProductRequestDetails_RequesterUserITIL.SetDisabled(true);

			Ref_SCMATLASBOBIN_ProductRequestDetails_CreatedAtITIL.SetVisible(Visible);
			Ref_SCMATLASBOBIN_ProductRequestDetails_CreatedAtITIL.SetDisabled(true);

			Ref_SCMATLASBOBIN_ProductRequestDetails_ITILDetails.SetVisible(Visible);
		}

		#endregion

		#region مدیریت شرایط تحویل و خرید
		public async Task TahvilIsVisible(bool Visible, bool Value, Entity.SCMATLASBOBIN_ProductRequestDetails Item)
		{
			// تحویل
			Ref_SCMATLASBOBIN_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(Visible);
			Item.GoodsDeliveryIsEnable = Value;
		}

		public async Task KharidIsVisible(bool Visible, bool Value, Entity.SCMATLASBOBIN_ProductRequestDetails Item)
		{
			// خرید
			Ref_SCMATLASBOBIN_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(Visible);
			Item.DeficitSupplyIsEnable = Value;
			// تعداد تامین کسری	
			Ref_SCMATLASBOBIN_ProductRequestDetails_DeficitSupplyNumber.SetVisible(Visible);
			// مبلغ حدودی خرید
			Ref_SCMATLASBOBIN_ProductRequestDetails_PurchasePrice.SetVisible(Visible);
			//Item.PurchasePrice = Value;
		}

		public async Task TahvilKharidIsVisible(bool Visible, bool Value, Entity.SCMATLASBOBIN_ProductRequestDetails Item)
		{
			// تحویل
			Ref_SCMATLASBOBIN_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(Visible);
			Item.GoodsDeliveryIsEnable = Value;
			// خرید
			Ref_SCMATLASBOBIN_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(Visible);
			Item.DeficitSupplyIsEnable = Value;
			// تعداد تامین کسری
			Ref_SCMATLASBOBIN_ProductRequestDetails_DeficitSupplyNumber.SetVisible(Visible);
			// مبلغ حدودی خرید
			Ref_SCMATLASBOBIN_ProductRequestDetails_PurchasePrice.SetVisible(Visible);
			//Item.PurchasePrice = Value;
		}
		#endregion

		/// <summary>
		/// تکمیل فیلدهای بایا بر اساس فیلدهای شماران سیستم
		/// </summary>
		/// <param name="Selected">شیء داینامیک حاوی اطلاعات کالا از سیستم شماران</param>
		/// <param name="Item">شیء جزئیات درخواست محصول که قرار است فیلدهای آن مقداردهی شود</param>
		/// <returns>یک Task غیرهمزمان (بدون مقدار بازگشتی)</returns>
		

		// متد کمکی برای کنترل دکمه New
		private async Task ToggleNewButtonBasedOnCount()
		{
			int activeCount = _Entity.SCMATLASBOBIN_ProductRequestDetails.Count(p => p.IsDelete != true);
			string buttonId = "SCMATLASBOBIN_ProductRequestDetails_GridSCMATLASBOBIN_ProductRequestId_847ButtonNew";

			if (activeCount >= 5)
				await JS.InvokeVoidAsync("eval", $"document.getElementById('{buttonId}').classList.add('d-none')");
			else
				await JS.InvokeVoidAsync("eval", $"document.getElementById('{buttonId}').classList.remove('d-none')");
		}

		

		

		public async Task <bool> GridSCMPLATE_ProductRequestId_847_editmodelsaving(object e   )
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
				await ToggleNewButtonBasedOnCount(); // مخفی کردن دکمه New اگر تعداد به 5 رسید
			}

			// مقدار دهی اولیه از کد رندوم به فیلد سیستمی در بایا
			if (!Item.DeliveryCode.HasValue)
				Item.DeliveryCode = RandomDeliveryCode;

			// بررسی اعتبارسنجی فیلدها
			IsCancelled = !await CheckFieldValidation(Item);

			return IsCancelled;
        }
public async Task  GridSCMPLATE_ProductRequestId_847_afterrendermodal(Entity.SCMATLASBOBIN_ProductRequestDetails Item   )
        {

            int activeCount = _Entity.SCMATLASBOBIN_ProductRequestDetails.Count(p => p.IsDelete != true);
			if (activeCount >= 5)
			{
				string saveAndNewId = "SCMATLASBOBIN_ProductRequestDetails_GridSCMATLASBOBIN_ProductRequestId_847ButtonSaveAndNew";
				await JS.InvokeVoidAsync("eval", $"document.getElementById('{saveAndNewId}').classList.add('d-none')");
			}

			// فیلد نوع درخواست
			if (Item.Global_SCMRequestTypeId == null)
			{
				await TahvilKharidIsVisible(false, false, Item);
			}
			else
			{
				// شرط اینکه آیا فرآیند تحویل اجرا گردد؟
				if (Item.Global_SCMRequestTypeId.ToString() == "a9c5df1c-d99c-ef11-8354-005056a02a64")
				{
					await TahvilIsVisible(true, true, Item);
					await KharidIsVisible(false, false, Item);
				}

				// شرط نوع درخواست بر اساس خرید کالا
				if (Item.Global_SCMRequestTypeId.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64")
				{
					await TahvilIsVisible(false, false, Item);
					await KharidIsVisible(true, true, Item);
				}

				// شرط نوع درخواست بر اساس تحویل و خرید کالا
				if (Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64")
				{
					await TahvilKharidIsVisible(true, true, Item);
				}
			}

			// Show ITIL Details
			if (!string.IsNullOrEmpty(Item.ResultingFromITIL))
			{
				await ITILDetailsVisible(true);
			}
			else
			{
				await ITILDetailsVisible(false);
			}

			// نمایش جزئیات ITIL
			if (Item.ResultingFromITIL != null)
			{
				Ref_SCMATLASBOBIN_ProductRequestDetails_ITILDetails.SetEntity(Item);
				Ref_SCMATLASBOBIN_ProductRequestDetails_ITILDetails.LoadData();
			}
        }
public async Task  ProductSearch_NotMapped_onitemselected(dynamic Selected ,Entity.SCMATLASBOBIN_ProductRequestDetails Item  )
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
public async Task  Global_SCMRequestTypeId_onitemselected(Entity.Global_SCMRequestType Selected ,Entity.SCMATLASBOBIN_ProductRequestDetails Item  )
        {
			// Console.WriteLine(await Utility.JSON.ToJson(Selected));

			// شرط اینکه آیا فرآیند تحویل اجرا گردد؟
			if (Item.Global_SCMRequestTypeId.ToString() == "a9c5df1c-d99c-ef11-8354-005056a02a64")
			{
				await TahvilIsVisible(true, true, Item);
				await KharidIsVisible(false, false, Item);
			}

			// شرط نوع درخواست بر اساس خرید کالا
			if (Item.Global_SCMRequestTypeId.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64")
			{
				await TahvilIsVisible(false, false, Item);
				await KharidIsVisible(true, true, Item);
			}

			// شرط نوع درخواست بر اساس تحویل و خرید کالا
			if (Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64")
			{
				await TahvilKharidIsVisible(true, true, Item);
			}
            
        }
public async Task  ResultingFromITIL_onitemselected(dynamic Selected ,Entity.SCMATLASBOBIN_ProductRequestDetails Item  )
        {

            // نمایش / عدم نمایش فیلد ITIL Detail
			await ITILDetailsVisible(true);

			if (Item.ResultingFromITIL != null)
			{
				Item.RequestIdITIL = Selected.RequestID;
				Item.RequesterUserITIL = Selected.UserName;
				Item.CreatedAtITIL = Selected.CreateDate;

				Ref_SCMATLASBOBIN_ProductRequestDetails_ITILDetails.SetEntity(Item);
				Ref_SCMATLASBOBIN_ProductRequestDetails_ITILDetails.LoadData();
			}

        }

		#endregion FunctionEvents

	}
}