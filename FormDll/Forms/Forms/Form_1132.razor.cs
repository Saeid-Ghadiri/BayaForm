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
	public class Form_1132Base : Form_1132Peropeties
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

			var List = _Entity.SCMATLASCELL_ProductRequestDetails
							.Where(p => p.IsDelete != true)
							.ToList();

			// دکمه ثبت و ادامه
			if (BtnWorkFlowId == "SequenceFlow_089d953")
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

		#region عطف خرید کالا (شماران)

		/// <summary>
		/// تنظیم نمایش فیلدهای عطف خرید کالا
		/// </summary>
		public void SetPurchaseReferenceFieldsVisibility(bool visible)
		{
			Ref_SCMATLASCELL_ProductRequestDetails_KH_Search_NotMapped?.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_APPROVER?.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_CENTCODE?.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_CENTCODE_GUID?.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_PAYCENTName?.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_WUSER?.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_TEMPNO?.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_TempNoNum?.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_ORDERNO?.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_ORDERNO_GUID?.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_ORDERDATE?.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_OKFACTDATE?.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_INVCODE?.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_REQPERSON?.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_YEAR?.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_SH_Kharid_DTL?.SetVisible(visible);
		}

		/// <summary>
		/// غیرفعال‌سازی فیلدهای عطف خرید (فقط خواندنی؛ جستجو فعال می‌ماند)
		/// </summary>
		public void DisablePurchaseReferenceFields()
		{
			Ref_SCMATLASCELL_ProductRequestDetails_KH_APPROVER?.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_CENTCODE?.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_CENTCODE_GUID?.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_PAYCENTName?.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_WUSER?.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_TEMPNO?.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_TempNoNum?.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_ORDERNO?.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_ORDERNO_GUID?.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_ORDERDATE?.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_OKFACTDATE?.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_INVCODE?.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_REQPERSON?.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_YEAR?.SetDisabled(true);
		}

		/// <summary>
		/// تنظیم نمایش و غیرفعال‌سازی فیلدهای عطف خرید
		/// </summary>
		public void ApplyPurchaseReferenceFieldsState(bool visible)
		{
			SetPurchaseReferenceFieldsVisibility(visible);
			DisablePurchaseReferenceFields();
		}

		/// <summary>
		/// پاک‌سازی رفرنس‌های کامپوننت فیلدهای عطف خرید
		/// </summary>
		public void ClearPurchaseReferenceFieldReferences()
		{
			Ref_SCMATLASCELL_ProductRequestDetails_KH_Search_NotMapped = null;
			Ref_SCMATLASCELL_ProductRequestDetails_KH_APPROVER = null;
			Ref_SCMATLASCELL_ProductRequestDetails_KH_CENTCODE = null;
			Ref_SCMATLASCELL_ProductRequestDetails_KH_CENTCODE_GUID = null;
			Ref_SCMATLASCELL_ProductRequestDetails_KH_PAYCENTName = null;
			Ref_SCMATLASCELL_ProductRequestDetails_KH_WUSER = null;
			Ref_SCMATLASCELL_ProductRequestDetails_KH_TEMPNO = null;
			Ref_SCMATLASCELL_ProductRequestDetails_KH_TempNoNum = null;
			Ref_SCMATLASCELL_ProductRequestDetails_KH_ORDERNO = null;
			Ref_SCMATLASCELL_ProductRequestDetails_KH_ORDERNO_GUID = null;
			Ref_SCMATLASCELL_ProductRequestDetails_KH_ORDERDATE = null;
			Ref_SCMATLASCELL_ProductRequestDetails_KH_OKFACTDATE = null;
			Ref_SCMATLASCELL_ProductRequestDetails_KH_INVCODE = null;
			Ref_SCMATLASCELL_ProductRequestDetails_KH_REQPERSON = null;
			Ref_SCMATLASCELL_ProductRequestDetails_KH_YEAR = null;
			Ref_SCMATLASCELL_ProductRequestDetails_SH_Kharid_DTL = null;
		}

		/// <summary>
		/// پر کردن فیلدهای سطح هدر (درخواست اصلی) از عطف خرید شماران
		/// </summary>
		public void FillMasterPurchaseReferenceFromShomaran(dynamic selected)
		{
			if (selected == null) return;

			_Entity.KH_TEMPNO = selected.TEMPNO;
			_Entity.KH_TempNoNum = selected.TempNoNum;
			_Entity.KH_ORDERNO_GUID = selected.ORDERNO_GUID;
		}

		/// <summary>
		/// پر کردن فیلدهای عطف خرید ردیف جزئیات از داده انتخاب‌شده در شماران
		/// </summary>
		public void FillPurchaseReferenceFieldsFromShomaran(dynamic selected, Entity.SCMATLASCELL_ProductRequestDetails item)
		{
			if (selected == null || item == null) return;

			item.KH_Search_NotMapped = selected.ORDERNO_GUID;
			item.KH_TEMPNO = selected.TEMPNO;
			item.KH_PAYCENTName = selected.PAYCENTName;
			item.KH_CENTCODE = selected.CENTCODE;
			item.KH_CENTCODE_GUID = selected.CENTCODE_GUID;
			item.KH_REQPERSON = selected.REQPERSON;
			item.KH_WUSER = selected.WUSER;
			item.KH_APPROVER = selected.APPROVER;
			item.KH_ORDERDATE = selected.ORDERDATE;
			item.KH_OKFACTDATE = selected.OKFACTDATE;
			item.KH_ORDERNO = selected.ORDERNO;
			item.KH_ORDERNO_GUID = selected.ORDERNO_GUID;
			item.KH_TempNoNum = selected.TempNoNum;
			item.KH_YEAR = selected.YEAR;
			item.KH_INVCODE = selected.INVCODE;
		}

		/// <summary>
		/// بارگذاری جزئیات عطف خرید در گرید
		/// </summary>
		public async Task LoadPurchaseReferenceDetailsAsync(Entity.SCMATLASCELL_ProductRequestDetails item)
		{
			if (item?.KH_ORDERNO_GUID == null) return;

			Ref_SCMATLASCELL_ProductRequestDetails_SH_Kharid_DTL?.SetEntity(item);
			await Ref_SCMATLASCELL_ProductRequestDetails_SH_Kharid_DTL?.LoadData();
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

			if (item.KH_ORDERNO_GUID == null)
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه محل مصرف را تکمیل نمایید.");
			}


			return isValid;
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

			// بررسی اعتبارسنجی فیلدها
			IsCancelled = !await CheckFieldValidation(Item);

			return IsCancelled;
		}

		public Task GridSCMATLASCELL_ProductRequestId_817_afterrendermodal(Entity.SCMATLASCELL_ProductRequestDetails Item)
		{
			return LoadPurchaseReferenceDetailsAsync(Item);
		}

		public async Task KH_Search_NotMapped_onitemselected(dynamic Selected, Entity.SCMATLASCELL_ProductRequestDetails Item)
		{
			FillPurchaseReferenceFieldsFromShomaran(Selected, Item);
			await LoadPurchaseReferenceDetailsAsync(Item);
		}

		public async Task  KH_TempNoNum_NotMapped_onitemselected(dynamic Selected   )
        {
			var List = _Entity.SCMATLASCELL_ProductRequestDetails
							.Where(p => p.IsDelete != true);
							
			if (Selected == null) return;

			foreach (var item in List)
			{
				FillPurchaseReferenceFieldsFromShomaran(Selected, item);
			}

			// پر کردن فیلدهای سطح هدر (درخواست اصلی) از عطف خرید شماران
			FillMasterPurchaseReferenceFromShomaran(Selected);
			_Entity.KH_TEMPNO = Selected.TEMPNO;
			Ref_KH_KharidDTL?.SetEntity(_Entity);
			Ref_KH_KharidDTL?.LoadData();
			
			
			StateHasChanged();
        }

		#endregion FunctionEvents

	}
}