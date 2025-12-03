using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using Blazored.Toast.Services;


namespace Forms.Forms
{
	public class Form_714Base : Form_714Peropeties
	{

		// Toast  
		[Inject]
		public IToastService toastService { get; set; }


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
				var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();
				foreach (var Item in List)
				{
					// آیا کالا دارای (Technical Data Sheet - (TDS است؟
					// Item.IsEnableTDS = null;
					// // نحوه تامین کالا
					// Item.SupplyGoodsIsEnable = null;
				}

				StateHasChanged();
			}
		}

		/// <summary>
		/// اعتبار سنجی فرم
		/// </summary>
		/// <returns></returns>
		public override async Task<bool> FormValidator()
		{
			bool IsValid = true;

			var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();

			// SequenceFlow_1vfs7yi - ثبت و ادامه

			if (BtnWorkFlowId == "SequenceFlow_1vfs7yi")
			{
				// Console.WriteLine("#Log FormValidator btn :");
				foreach (var Item in List)
				{
					//Console.WriteLine("#Log FormValidator btn foreach :");

					IsValid = IsValid && await CheckFieldValidation(Item);
				}
			}

			return IsValid;
		}

		/// <summary>
		/// ارسال داده
		/// </summary>
		/// <returns></returns>
		public override async Task<Baya.Models.Utility.Result> Submit()
		{
			SumaryMessage = "";
			Baya.Models.Utility.Result Result = new Baya.Models.Utility.Result();

			if (!await FormValidator())
			{
				StateHasChanged();
				Result.Status = HttpStatusCode.InternalServerError;
				return Result;
			}

			await BeforSubmit();

			var Resualt = await PostData();

			if (Resualt)
			{
				Result.Status = HttpStatusCode.OK;

				SumaryMessage = "داده ها با موفقیت ثبت شد";
			}
			else
			{
				Result.Status = HttpStatusCode.InternalServerError;
				SumaryMessage = "ذخیره داده با مشکل مواجه شد";
			}

			Result.Message = SumaryMessage;

			switch ((int)Result.Status)
			{
				case 200:
					toastService.ShowSuccess(Result.Message);
					break;
				case 500:
					toastService.ShowError(Result.Message);
					break;
				default:
					break;
			}

			await AfterSubmit();

			return Result;
		}

		/// <summary>
		/// ارسال داده
		/// </summary>
		/// <returns></returns>
		private async Task<bool> PostData()
		{
			string Data = await Utility.JSON.ToJson(_Entity);

			bool IsOk = false;
			var Model = await ApiServer.External.Services.Data.Put(Data, TablePost.Name, TablePost, RequestID?.ToString(), _User.UserID.ToString());
			if (Model?.Status == HttpStatusCode.OK)
			{
				IsOk = true;
			}

			return IsOk;
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

		/// <summary>
		/// بررسی نوع درخواست انتخابی توسط انباردار
		///
		/// Id										    Title
		/// *********************************************************
		/// a9c5df1c-d99c-ef11-8354-005056a02a64		تحویل کالا
		/// 3b9e934d-d99c-ef11-8354-005056a02a64		خرید کالا
		/// 73f6c459-d99c-ef11-8354-005056a02a64		تحویل و خرید کالا
		/// 
		/// بررسی ضرورت تکمیل فیلدها از این تابع انجام می شود.
		/// </summary>
		/// <param name="Item"></param>
		/// <returns></returns>
		public async Task<bool> CheckFieldValidation(Entity.SCMPLATE_ProductRequestDetails Item)
		{
			bool IsValid = true;

			var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();

			// // تعداد یا مقدار واگذاری کالا
			// if (Item.NumberofGoodsDelivery == null)
			// {
			// 	IsValid = false;

			// 	toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا را تکمیل نمایید",
			// 		settings =>
			// 		{
			// 			settings.Timeout = 4;
			// 			settings.ShowProgressBar = true;
			// 			settings.PauseProgressOnHover = true;
			// 		});
			// }

			// آیا خرید تعداد / مقدار مازاد دارد؟ - SurplusProductIsEnable
			if (Item.IsExcessPurchasedItem == null)
			{
				IsValid = false;

				toastService.ShowError("لطفا گزینه آیا خرید تعداد / مقدار مازاد دارد؟ را تکمیل نمایید.",
				settings =>
				{
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}

			// آیا خرید تعداد / مقدار مازاد دارد؟
			if (Item.IsExcessPurchasedItem.HasValue && Item.IsExcessPurchasedItem.Value)
			{
				// تعداد / مقدار خرید مازاد
				if (Item.ExcessPurchasedQuantity == null)
				{
					IsValid = false;

					toastService.ShowError("لطفا گزینه تعداد / مقدار خرید مازاد را تکمیل نمایید.",
					settings =>
					{
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
				}
			}


			// تعداد مراحل واگذاری
			//if (Item.SCMPLATE_PurchaseStageId == null)
			//if (Item.SCMPLATE_PurchaseStageId == null || Item.SCMPLATE_PurchaseStageId == Guid.Empty)
			if (!Item.SCMPLATE_PurchaseStageId.HasValue || Item.SCMPLATE_PurchaseStageId.Value == Guid.Empty)
			{
				IsValid = false;
				toastService.ShowError("لطفا گزینه مراحل واگذاری کالا به انبار را تکمیل نمایید",
				settings =>
				{
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}
			else if (Item.SCMPLATE_PurchaseStageId != null)
			{
				// مراحل واگذاری کالا
				var Stage1IsVisible = Ref_SCMPLATE_ProductRequestDetails_Stage1PurchasedQuantity;
				var Stage2IsVisible = Ref_SCMPLATE_ProductRequestDetails_Stage2PurchasedQuantity;
				var Stage3IsVisible = Ref_SCMPLATE_ProductRequestDetails_Stage3PurchasedQuantity;

				// خرید مرحله 1
				if (Item.SCMPLATE_PurchaseStageId.ToString() == "7e065b51-37fe-ef11-a4fc-005056a2b6bd")
				{
					if (Item.Stage1PurchasedQuantity == null)
					{
						IsValid = false;
						toastService.ShowError("لطفا گزینه تعداد یا مقدار مرحله 1 را تکمیل نمایید.",
						settings =>
						{
							settings.Timeout = 4;
							settings.ShowProgressBar = true;
							settings.PauseProgressOnHover = true;
						});
					}
				}
				else if (Item.SCMPLATE_PurchaseStageId.ToString() == "dbabd157-37fe-ef11-a4fc-005056a2b6bd")
				{
					if (Item.Stage2PurchasedQuantity == null)
					{
						IsValid = false;
						toastService.ShowError("لطفا گزینه تعداد یا مقدار مرحله  2 را تکمیل نمایید.",
						settings =>
						{
							settings.Timeout = 4;
							settings.ShowProgressBar = true;
							settings.PauseProgressOnHover = true;
						});
					}

					if (Item.Stage1PurchasedQuantity == null)
					{
						IsValid = false;
						toastService.ShowError("لطفا گزینه تعداد یا مقدار مرحله 1   را تکمیل نمایید.",
						settings =>
						{
							settings.Timeout = 4;
							settings.ShowProgressBar = true;
							settings.PauseProgressOnHover = true;
						});
					}


				}
				else if (Item.SCMPLATE_PurchaseStageId.ToString() == "dcabd157-37fe-ef11-a4fc-005056a2b6bd")
				{
					if (Item.Stage3PurchasedQuantity == null )
					{
						IsValid = false;
						toastService.ShowError("لطفا گزینه تعداد یا مقدار مرحله 3 را تکمیل نمایید.",
						settings =>
						{
							settings.Timeout = 4;
							settings.ShowProgressBar = true;
							settings.PauseProgressOnHover = true;
						});
					}

					if (Item.Stage2PurchasedQuantity == null )
					{
						IsValid = false;
						toastService.ShowError("لطفا گزینه تعداد یا مقدار مرحله  2  را تکمیل نمایید.",
						settings =>
						{
							settings.Timeout = 4;
							settings.ShowProgressBar = true;
							settings.PauseProgressOnHover = true;
						});
					}

					if (Item.Stage1PurchasedQuantity == null )
					{
						IsValid = false;
						toastService.ShowError("لطفا گزینه تعداد یا مقدار مرحله 1 را تکمیل نمایید.",
						settings =>
						{
							settings.Timeout = 4;
							settings.ShowProgressBar = true;
							settings.PauseProgressOnHover = true;
						});
					}
				}
			}

			return IsValid;
		}

		public async Task InquiryIsVisible(bool Visible)
		{
			// استعلام شماره 1
			Ref_SCMPLATE_ProductRequestDetails_SCMPLATE_ProductRequestDetails_InquiryFile1.SetVisible(Visible);
			// استعلام شماره 2
			Ref_SCMPLATE_ProductRequestDetails_SCMPLATE_ProductRequestDetails_InquiryFile2.SetVisible(Visible);
			// استعلام شماره 3
			Ref_SCMPLATE_ProductRequestDetails_SCMPLATE_ProductRequestDetails_InquiryFile3.SetVisible(Visible);
		}

		public async Task LogisticDeliveryIsVisible(bool Visible)
		{
			// تعداد مراحل واگذاری کالا به انباردار
			Ref_SCMPLATE_ProductRequestDetails_SCMPLATE_PurchaseStageId.SetVisible(Visible);
			// تعداد یا مقدار واگذاری کالا 1
			Ref_SCMPLATE_ProductRequestDetails_Stage1PurchasedQuantity.SetVisible(Visible);
			// تعداد یا مقدار واگذاری کالا 2
			Ref_SCMPLATE_ProductRequestDetails_Stage2PurchasedQuantity.SetVisible(Visible);
			// تعداد یا مقدار واگذاری کالا 3
			Ref_SCMPLATE_ProductRequestDetails_Stage3PurchasedQuantity.SetVisible(Visible);
		}


		public async Task LogisticDeliveryAfterVisible(Entity.SCMPLATE_ProductRequestDetails Item)
		{
			// 7e065b51-37fe-ef11-a4fc-005056a2b6bd ==> مرحله 1  && Code = 1
			// dbabd157-37fe-ef11-a4fc-005056a2b6bd ==> مرحله 2  && Code = 2
			// dcabd157-37fe-ef11-a4fc-005056a2b6bd ==> مرحله 3  && Code = 3
			// 6b98c962-37fe-ef11-a4fc-005056a2b6bd ==> مرحله 4  && Code = 4
			// 

			if (Item.SCMPLATE_PurchaseStageId == null || Item.SCMPLATE_PurchaseStageId == Guid.Empty)
			{
				//Console.WriteLine("log 2 SCM_NumTransfersGoodsWarehouseId");
				// تعداد یا مقدار واگذاری کالا 1
				Ref_SCMPLATE_ProductRequestDetails_Stage1PurchasedQuantity.SetVisible(false);
				// تعداد یا مقدار واگذاری کالا 2
				Ref_SCMPLATE_ProductRequestDetails_Stage2PurchasedQuantity.SetVisible(false);
				// تعداد یا مقدار واگذاری کالا 3
				Ref_SCMPLATE_ProductRequestDetails_Stage3PurchasedQuantity.SetVisible(false);
			}
			else
			{
				// شرط یک مرحله ای    
				//if (Item.SCMPLATE_PurchaseStageId.ToString() == "7e065b51-37fe-ef11-a4fc-005056a2b6bd")
				if (Item.SCMPLATE_PurchaseStage.Code.ToString() == "1")
				{
					// تعداد یا مقدار واگذاری کالا 1
					Ref_SCMPLATE_ProductRequestDetails_Stage1PurchasedQuantity.SetVisible(true);
					// تعداد یا مقدار واگذاری کالا 2
					Ref_SCMPLATE_ProductRequestDetails_Stage2PurchasedQuantity.SetVisible(false);
					// تعداد یا مقدار واگذاری کالا 3
					Ref_SCMPLATE_ProductRequestDetails_Stage3PurchasedQuantity.SetVisible(false);
				}

				// شرط دو مرحله ای
				//if (Item.SCMPLATE_PurchaseStageId.ToString() == "dbabd157-37fe-ef11-a4fc-005056a2b6bd")
				if (Item.SCMPLATE_PurchaseStage.Code.ToString() == "2")
				{
					// تعداد یا مقدار واگذاری کالا 1
					Ref_SCMPLATE_ProductRequestDetails_Stage1PurchasedQuantity.SetVisible(true);
					// تعداد یا مقدار واگذاری کالا 2
					Ref_SCMPLATE_ProductRequestDetails_Stage2PurchasedQuantity.SetVisible(true);
					// تعداد یا مقدار واگذاری کالا 3
					Ref_SCMPLATE_ProductRequestDetails_Stage3PurchasedQuantity.SetVisible(false);
				}

				// شرط سه مرحله ای
				//if (Item.SCMPLATE_PurchaseStageId.ToString() == "dcabd157-37fe-ef11-a4fc-005056a2b6bd")
				if (Item.SCMPLATE_PurchaseStage.Code.ToString() == "3")
				{
					// تعداد یا مقدار واگذاری کالا 1
					Ref_SCMPLATE_ProductRequestDetails_Stage1PurchasedQuantity.SetVisible(true);
					// تعداد یا مقدار واگذاری کالا 2
					Ref_SCMPLATE_ProductRequestDetails_Stage2PurchasedQuantity.SetVisible(true);
					// تعداد یا مقدار واگذاری کالا 3
					Ref_SCMPLATE_ProductRequestDetails_Stage3PurchasedQuantity.SetVisible(true);
				}
				// شرط سه مرحله ای
				if (Item.SCMPLATE_PurchaseStage.Code.ToString() == "4")
				{
				}

			}

		}
		public async Task<bool> GridSCMPLATE_ProductRequestId_303_editmodelsaving(object e)
		{
			bool IsCancelled = false;
			var Item = (Entity.SCMPLATE_ProductRequestDetails)e;

			IsCancelled = !await CheckFieldValidation(Item);

			return IsCancelled;
		}

		public async Task GridSCMPLATE_ProductRequestId_303_afterrendermodal(Entity.SCMPLATE_ProductRequestDetails Item)
		{

			await LogisticDeliveryAfterVisible(Item);

			// نمایش / عدم نمایش فیلد آیا کالای خریداری شده مازاد دارد؟
			var UFIsVisible = Ref_SCMPLATE_ProductRequestDetails_ExcessPurchasedQuantity;
			if (Item.IsExcessPurchasedItem.HasValue && Item.IsExcessPurchasedItem.Value)
			{
				UFIsVisible.SetVisible(true);
			}
			else
			{
				UFIsVisible.SetVisible(false);
			}
		}

		public async Task HasInquiry_oninput(ChangeEventArgs Selected, Entity.SCMPLATE_ProductRequestDetails Item)
		{
		}

		public async Task HasInquiryApproved_oninput(ChangeEventArgs Selected, Entity.SCMPLATE_ProductRequestDetails Item)
		{
		}
		public async Task SCMPLATE_PurchaseStageId_onitemselected(Entity.SCMPLATE_PurchaseStage Selected, Entity.SCMPLATE_ProductRequestDetails Item)
		{
			// 7e065b51-37fe-ef11-a4fc-005056a2b6bd ==> مرحله 1
			// dbabd157-37fe-ef11-a4fc-005056a2b6bd ==> مرحله 2
			// dcabd157-37fe-ef11-a4fc-005056a2b6bd ==> مرحله 3
			// 6b98c962-37fe-ef11-a4fc-005056a2b6bd ==> مرحله 4

			// شرط یک مرحله ای    
			if (Selected.Code.ToString() == "1")
			{
				// تعداد یا مقدار واگذاری کالا 1
				Ref_SCMPLATE_ProductRequestDetails_Stage1PurchasedQuantity.SetVisible(true);
				// تعداد یا مقدار واگذاری کالا 2
				Ref_SCMPLATE_ProductRequestDetails_Stage2PurchasedQuantity.SetVisible(false);
				// تعداد یا مقدار واگذاری کالا 3
				Ref_SCMPLATE_ProductRequestDetails_Stage3PurchasedQuantity.SetVisible(false);
			}
			// شرط دو مرحله ای
			if (Selected.Code.ToString() == "2")
			{
				// تعداد یا مقدار واگذاری کالا 1
				Ref_SCMPLATE_ProductRequestDetails_Stage1PurchasedQuantity.SetVisible(true);
				// تعداد یا مقدار واگذاری کالا 2
				Ref_SCMPLATE_ProductRequestDetails_Stage2PurchasedQuantity.SetVisible(true);
				// تعداد یا مقدار واگذاری کالا 3
				Ref_SCMPLATE_ProductRequestDetails_Stage3PurchasedQuantity.SetVisible(false);
			}
			// شرط سه مرحله ای
			if (Selected.Code.ToString() == "3")
			{
				// تعداد یا مقدار واگذاری کالا 1
				Ref_SCMPLATE_ProductRequestDetails_Stage1PurchasedQuantity.SetVisible(true);
				// تعداد یا مقدار واگذاری کالا 2
				Ref_SCMPLATE_ProductRequestDetails_Stage2PurchasedQuantity.SetVisible(true);
				// تعداد یا مقدار واگذاری کالا 3
				Ref_SCMPLATE_ProductRequestDetails_Stage3PurchasedQuantity.SetVisible(true);
			}
			if (Selected.Code.ToString() == "4")
			{
			}
		}

		public async Task IsExcessPurchasedItem_oninput(ChangeEventArgs Selected, Entity.SCMPLATE_ProductRequestDetails Item)
		{
			// نمایش / عدم نمایش فیلد آیا کالای خریداری شده مازاد دارد؟
			var UFIsVisible = Ref_SCMPLATE_ProductRequestDetails_ExcessPurchasedQuantity;
			if (!(Item.IsExcessPurchasedItem.HasValue && Item.IsExcessPurchasedItem.Value))
			{
				UFIsVisible.SetVisible(true);
			}
			else
			{
				UFIsVisible.SetVisible(false);
			}
		}

		#endregion FunctionEvents

	}
}