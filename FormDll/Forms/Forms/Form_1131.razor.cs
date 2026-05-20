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
	public class Form_1131Base : Form_1131Peropeties
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
			
		}




		

		#endregion FunctionEvents

	}
}