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

		// عطف خرید کالا در شماران سیستم
		public async Task KharidIsVisible(bool Visible)
		{
			// Console.WriteLine("#Log 2");
			// Console.WriteLine("#Log 2.1" + Ref_SCMATLASCELL_ProductRequestDetails_KH_Search_NotMapped.Value);
			await Task.Delay(100);

			Ref_SCMATLASCELL_ProductRequestDetails_KH_Search_NotMapped.SetVisible(Visible);

			Ref_SCMATLASCELL_ProductRequestDetails_KH_APPROVER.SetVisible(Visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_CENTCODE.SetVisible(Visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_CENTCODE.SetVisible(Visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_CENTCODE_GUID.SetVisible(Visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_PAYCENTName.SetVisible(Visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_WUSER.SetVisible(Visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_TEMPNO.SetVisible(Visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_TempNoNum.SetVisible(Visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_ORDERNO.SetVisible(Visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_ORDERNO_GUID.SetVisible(Visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_ORDERDATE.SetVisible(Visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_OKFACTDATE.SetVisible(Visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_INVCODE.SetVisible(Visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_REQPERSON.SetVisible(Visible);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_YEAR.SetVisible(Visible);
			// جزئیات خرید کالا
			Ref_SCMATLASCELL_ProductRequestDetails_SH_Kharid_DTL.SetVisible(Visible);

			Ref_SCMATLASCELL_ProductRequestDetails_KH_APPROVER.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_CENTCODE.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_CENTCODE.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_CENTCODE_GUID.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_PAYCENTName.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_WUSER.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_TEMPNO.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_TempNoNum.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_ORDERNO.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_ORDERNO_GUID.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_ORDERDATE.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_OKFACTDATE.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_INVCODE.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_REQPERSON.SetDisabled(true);
			Ref_SCMATLASCELL_ProductRequestDetails_KH_YEAR.SetDisabled(true);
		}

		public async Task KharidIsNull()
		{
			Ref_SCMATLASCELL_ProductRequestDetails_KH_Search_NotMapped = null;
			Ref_SCMATLASCELL_ProductRequestDetails_KH_APPROVER = null;
			Ref_SCMATLASCELL_ProductRequestDetails_KH_CENTCODE = null;
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

		public async Task KharidSetShomaran(dynamic Selected, Entity.SCMATLASCELL_ProductRequestDetails Item)
		{
			//Console.WriteLine("#Log 3");
			//Console.WriteLine(await Utility.JSON.ToJson(Selected));
			await Task.Delay(100);
			Item.KH_TEMPNO = Selected.TEMPNO;
			Item.KH_PAYCENTName = Selected.PAYCENTName;
			Item.KH_CENTCODE = Selected.CENTCODE;
			Item.KH_CENTCODE_GUID = Selected.CENTCODE_GUID;
			Item.KH_REQPERSON = Selected.REQPERSON;
			Item.KH_WUSER = Selected.WUSER;
			Item.KH_APPROVER = Selected.APPROVER;
			Item.KH_ORDERDATE = Selected.ORDERDATE;
			Item.KH_OKFACTDATE = Selected.OKFACTDATE;
			Item.KH_ORDERNO = Selected.ORDERNO;
			Item.KH_ORDERNO_GUID = Selected.ORDERNO_GUID;
			Item.KH_TempNoNum = Selected.TempNoNum;
			Item.KH_YEAR = Selected.YEAR;
			Item.KH_INVCODE = Selected.INVCODE;

			await Task.Delay(100);
			// فراخوانی داده از dropdown TempNoNum برای گرید داده های آن
			//Ref_SCMATLASCELL_ProductRequestDetails_SH_Kharid_DTL.SetEntity(Item);
			//Ref_SCMATLASCELL_ProductRequestDetails_SH_Kharid_DTL.LoadData();
		}


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

		public async Task GridSCMATLASCELL_ProductRequestId_817_afterrendermodal(Entity.SCMATLASCELL_ProductRequestDetails Item)
		{
			if (Item.KH_ORDERNO_GUID.HasValue)
			{
				Ref_SCMATLASCELL_ProductRequestDetails_SH_Kharid_DTL.SetEntity(Item);
				Ref_SCMATLASCELL_ProductRequestDetails_SH_Kharid_DTL.LoadData();
			}
		}

		public async Task KH_Search_NotMapped_onitemselected(dynamic Selected, Entity.SCMATLASCELL_ProductRequestDetails Item)
		{
			await KharidSetShomaran(Selected, Item);
		}

		public async Task KHMaster_Search_NotMapped_onitemselected(dynamic Selected)
		{
			foreach (var item in _Entity.SCMATLASCELL_ProductRequestDetails)
			{
				await KharidSetShomaran(Selected, item);
			}
		}

		#endregion FunctionEvents

	}
}