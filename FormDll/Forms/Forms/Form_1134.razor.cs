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
	public class Form_1134Base : Form_1134Peropeties
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

		private static bool HasInquirySelected(Entity.SCMATLASCELL_ProductRequestDetails item) =>
			item.HasInquiry.HasValue && item.HasInquiry.Value;

		private static bool RequiresWarehouseDelivery(Entity.SCMATLASCELL_ProductRequestDetails item) =>
			!item.HasInquiry.HasValue || !item.HasInquiry.Value;

		private static string? GetPurchaseStageCode(Entity.SCMATLASCELL_ProductRequestDetails item)
		{
			if (item.SCMATLASCELL_PurchaseStage?.Code != null)
				return item.SCMATLASCELL_PurchaseStage.Code.ToString();

			if (item.SCMATLASCELL_PurchaseStageId == null)
				return null;

			return item.SCMATLASCELL_PurchaseStageId.Value.ToString() switch
			{
				"aab4df46-6556-f111-a514-005056a2b6bd" => "1",
				"dd4b304f-6556-f111-a514-005056a2b6bd" => "2",
				"de4b304f-6556-f111-a514-005056a2b6bd" => "3",
				_ => null
			};
		}

		private async Task<bool> ValidateCommonFieldsAsync(Entity.SCMATLASCELL_ProductRequestDetails item)
		{
			bool isValid = true;

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

		private async Task<bool> ValidateInquiryFieldsAsync(Entity.SCMATLASCELL_ProductRequestDetails item)
		{
			bool isValid = true;

			if (item.HasInquiry == null)
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه آیا نیاز به استعلام دارد؟ را تکمیل نمایید.");
			}

			if (HasInquirySelected(item))
			{
				if (item.SCMATLASCELL_ProductRequestDetails_Inquiry_File1 == null
					|| item.SCMATLASCELL_ProductRequestDetails_Inquiry_File1.Count < 1)
				{
					isValid = false;
					await _MSG.ShowError("لطفا فایل استعلام 1 را بارگذاری نمایید.");
				}
			}

			return isValid;
		}

		private async Task<bool> ValidatePurchaseStageQuantitiesAsync(Entity.SCMATLASCELL_ProductRequestDetails item)
		{
			bool isValid = true;
			var stageCode = GetPurchaseStageCode(item);

			if (string.IsNullOrEmpty(stageCode))
				return isValid;

			if ((stageCode is "1" or "2" or "3" or "4") && item.Stage1PurchasedQuantity == null)
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه تعداد یا مقدار مرحله 1 را تکمیل نمایید.");
			}

			if ((stageCode is "2" or "3") && item.Stage2PurchasedQuantity == null)
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه تعداد یا مقدار مرحله 2 را تکمیل نمایید.");
			}

			if (stageCode == "3" && item.Stage3PurchasedQuantity == null)
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه تعداد یا مقدار مرحله 3 را تکمیل نمایید.");
			}

			return isValid;
		}

		private async Task<bool> ValidateWarehouseDeliveryFieldsAsync(Entity.SCMATLASCELL_ProductRequestDetails item)
		{
			bool isValid = true;

			if (!RequiresWarehouseDelivery(item))
				return isValid;

			if (item.SCMATLASCELL_PurchaseStageId == null)
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه مراحل واگذاری کالا به انبار را تکمیل نمایید.");
				return isValid;
			}

			return await ValidatePurchaseStageQuantitiesAsync(item);
		}

		/// <summary>
		/// اعتبارسنجی یک ردیف درخواست کالا (تحویل/خرید)
		/// </summary>
		/// <param name="item">ردیف جاری</param>
		/// <returns>true در صورت معتبر بودن</returns>
		public async Task<bool> CheckFieldValidation(Entity.SCMATLASCELL_ProductRequestDetails item)
		{
			bool isValid = await ValidateCommonFieldsAsync(item);
			isValid = isValid && await ValidateInquiryFieldsAsync(item);
			isValid = isValid && await ValidateWarehouseDeliveryFieldsAsync(item);
			return isValid;
		}

		#endregion

		#region نمایش فیلدها (استعلام / واگذاری انبار)

		public bool flage { get; set; } = true;

		public Task InquiryIsVisible(bool visible)
		{
			Ref_SCMATLASCELL_ProductRequestDetails_SCMATLASCELL_ProductRequestDetails_Inquiry_File1.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_SCMATLASCELL_ProductRequestDetails_Inquiry_File2.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_SCMATLASCELL_ProductRequestDetails_Inquiry_File3.SetVisible(visible);
			return Task.CompletedTask;
		}

		public Task LogisticDeliveryIsVisible(bool visible)
		{
			Ref_SCMATLASCELL_ProductRequestDetails_SCMATLASCELL_PurchaseStageId.SetVisible(visible);
			Ref_SCMATLASCELL_ProductRequestDetails_Stage1PurchasedQuantity.SetVisible(false);
			Ref_SCMATLASCELL_ProductRequestDetails_Stage2PurchasedQuantity.SetVisible(false);
			Ref_SCMATLASCELL_ProductRequestDetails_Stage3PurchasedQuantity.SetVisible(false);
			flage = visible;
			return Task.CompletedTask;
		}

		public Task LogisticDeliveryAfterVisible(Entity.SCMATLASCELL_ProductRequestDetails item)
		{
			if (item.SCMATLASCELL_PurchaseStageId == null || item.SCMATLASCELL_PurchaseStageId == Guid.Empty)
			{
				Ref_SCMATLASCELL_ProductRequestDetails_Stage1PurchasedQuantity.SetVisible(false);
				Ref_SCMATLASCELL_ProductRequestDetails_Stage2PurchasedQuantity.SetVisible(false);
				Ref_SCMATLASCELL_ProductRequestDetails_Stage3PurchasedQuantity.SetVisible(false);
				return Task.CompletedTask;
			}

			var stageCode = GetPurchaseStageCode(item);

			Ref_SCMATLASCELL_ProductRequestDetails_Stage1PurchasedQuantity.SetVisible(stageCode is "1" or "2" or "3");
			Ref_SCMATLASCELL_ProductRequestDetails_Stage2PurchasedQuantity.SetVisible(stageCode is "2" or "3");
			Ref_SCMATLASCELL_ProductRequestDetails_Stage3PurchasedQuantity.SetVisible(stageCode == "3");

			return Task.CompletedTask;
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
			if (HasInquirySelected(Item))
			{
				await InquiryIsVisible(true);
				await LogisticDeliveryIsVisible(false);
			}
			else if (Item.HasInquiry.HasValue && !Item.HasInquiry.Value)
			{
				await InquiryIsVisible(false);
				await LogisticDeliveryIsVisible(true);
			}
			else
			{
				await InquiryIsVisible(false);
				await LogisticDeliveryIsVisible(false);
			}

			await LogisticDeliveryAfterVisible(Item);
		}

		public async Task HasInquiry_oninput(ChangeEventArgs Selected, Entity.SCMATLASCELL_ProductRequestDetails Item)
		{
			bool inquirySelected = Selected.Value?.ToString() == "true";

			if (inquirySelected)
			{
				await InquiryIsVisible(true);
				await LogisticDeliveryIsVisible(false);
			}
			else
			{
				await InquiryIsVisible(false);
				await LogisticDeliveryIsVisible(true);
				await LogisticDeliveryAfterVisible(Item);
			}
		}

		public Task SCMATLASCELL_PurchaseStageId_onitemselected(Entity.SCMATLASCELL_PurchaseStage Selected, Entity.SCMATLASCELL_ProductRequestDetails Item)
		{
			/*
				Id										Code	Title
				AAB4DF46-6556-F111-A514-005056A2B6BD	1		یک مرحله
				DD4B304F-6556-F111-A514-005056A2B6BD	2		دو مرحله
				DE4B304F-6556-F111-A514-005056A2B6BD	3		سه مرحله
			*/
			Item.SCMATLASCELL_PurchaseStage = Selected;
			return LogisticDeliveryAfterVisible(Item);
		}

		public async Task IsPostponedPurchase_oninput(ChangeEventArgs Selected, Entity.SCMATLASCELL_ProductRequestDetails Item)
		{
		}

		public async Task IsMarkedForDeletion_oninput(ChangeEventArgs Selected, Entity.SCMATLASCELL_ProductRequestDetails Item)
		{
		}

		public async Task  IsExcessPurchasedItem_oninput(ChangeEventArgs Selected ,Entity.SCMATLASCELL_ProductRequestDetails Item  )
        {
        }

		#endregion FunctionEvents

	}
}
