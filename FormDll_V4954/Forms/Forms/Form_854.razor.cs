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
	public class Form_854Base : Form_854Peropeties
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

			int ListCount = List.Count();


			// SequenceFlow_1hoqt1e		----	دکمه ثبت و ادامه
			if (BtnWorkFlowId == "SequenceFlow_1hoqt1e")
            {
                // Console.WriteLine("#Log FormValidator btn :");
                foreach (var Item in List)
                {
                    //Console.WriteLine("#Log FormValidator btn foreach :");

                    IsValid = IsValid && await CheckFieldValidation(Item);
                }
            }

			//return false;
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

			// تعداد خرید مرحله اول- Stage1PurchasedQuantity
            if (Item.SCMPLATE_PurchaseStageId == null)
            {
                IsValid = false;
                toastService.ShowError("لطفا گزینه مراحل واگذاری کالا به انبار را تکمیل نمایید.",
                settings =>
                {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                }); 
            }
            else 
            {
				// 7e065b51-37fe-ef11-a4fc-005056a2b6bd				مرحله 1
				// dbabd157-37fe-ef11-a4fc-005056a2b6bd				مرحله 2
				// dcabd157-37fe-ef11-a4fc-005056a2b6bd				مرحله 3
				// 6b98c962-37fe-ef11-a4fc-005056a2b6bd				مرحله 4
				Console.WriteLine("#Log0::::::");
				Console.WriteLine("#Log1:" + Item.SCMPLATE_PurchaseStageId.Value);
                // خرید مرحله 1
                if (Item.SCMPLATE_PurchaseStageId.Value.ToString() == "7e065b51-37fe-ef11-a4fc-005056a2b6bd")
                {
					Console.WriteLine("#Log2: 01");
                    if (!Item.GoodsDeliveryToRequester1.HasValue)
                    {
                        IsValid = false;
                        toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به درخواست کننده 1 را تکمیل نمایید.",
                        settings =>
                        {
                            settings.Timeout = 4;
                            settings.ShowProgressBar = true;
                            settings.PauseProgressOnHover = true;
                        });
                    }
                }
                if(Item.SCMPLATE_PurchaseStageId.Value.ToString() == "dbabd157-37fe-ef11-a4fc-005056a2b6bd")
                {
					Console.WriteLine("#Log3: 01");
                    if (!Item.GoodsDeliveryToRequester1.HasValue)
                    {
                        IsValid = false;
                        toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به درخواست کننده 1 را تکمیل نمایید.",
                        settings =>
                        {
                            settings.Timeout = 4;
                            settings.ShowProgressBar = true;
                            settings.PauseProgressOnHover = true;
                        });
                    }
					Console.WriteLine("#Log3: 02");
                    if (!Item.GoodsDeliveryToRequester2.HasValue)
                    {
                        IsValid = false;
                        toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به درخواست کننده 2  را تکمیل نمایید.",
                        settings =>
                        {
                            settings.Timeout = 4;
                            settings.ShowProgressBar = true;
                            settings.PauseProgressOnHover = true;
                        });
                    }
				}
					    // خرید مرحله 3
                
                if(Item.SCMPLATE_PurchaseStageId.Value.ToString() == "dcabd157-37fe-ef11-a4fc-005056a2b6bd")
                {
					Console.WriteLine("#Log4: 01");
                    if (!Item.GoodsDeliveryToRequester1.HasValue)
                    {
                        IsValid = false;
                        toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به درخواست کننده 1 را تکمیل نمایید.",
                        settings =>
                        {
                            settings.Timeout = 4;
                            settings.ShowProgressBar = true;
                            settings.PauseProgressOnHover = true;
                        });
                    }
					Console.WriteLine("#Log4: 02");
                    if (!Item.GoodsDeliveryToRequester2.HasValue)
                    {
                        IsValid = false;
                        toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به درخواست کننده 2  را تکمیل نمایید.",
                        settings =>
                        {
                            settings.Timeout = 4;
                            settings.ShowProgressBar = true;
                            settings.PauseProgressOnHover = true;
                        });
                    }
					Console.WriteLine("#Log4: 03");
                    if (!Item.GoodsDeliveryToRequester3.HasValue)
                    {
                        IsValid = false;
                        toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا به درخواست کننده 3 را تکمیل نمایید.",
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

		public async Task LogisticDeliveryIsVisible(bool Visible)
        {
            // تعداد مراحل واگذاری کالا به انباردار
            Ref_SCMPLATE_ProductRequestDetails_SCMPLATE_PurchaseStageId.SetVisible(Visible);
			Ref_SCMPLATE_ProductRequestDetails_SCMPLATE_PurchaseStageId.SetDisabled(true);
            // تعداد یا مقدار واگذاری کالا 1
            Ref_SCMPLATE_ProductRequestDetails_Stage1PurchasedQuantity.SetVisible(Visible);
			Ref_SCMPLATE_ProductRequestDetails_Stage1PurchasedQuantity.SetDisabled(true);
            // تعداد یا مقدار واگذاری کالا 2
            Ref_SCMPLATE_ProductRequestDetails_Stage2PurchasedQuantity.SetVisible(Visible);
			Ref_SCMPLATE_ProductRequestDetails_Stage2PurchasedQuantity.SetDisabled(true);
            // تعداد یا مقدار واگذاری کالا 3
            Ref_SCMPLATE_ProductRequestDetails_Stage3PurchasedQuantity.SetVisible(Visible);
			Ref_SCMPLATE_ProductRequestDetails_Stage3PurchasedQuantity.SetDisabled(true);
        }

		public async Task TadarokatBeAnbarIsVisible(bool Visible)
		{
			Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryToRequester1.SetVisible(Visible);

            Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryToRequester2.SetVisible(Visible);

            Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryToRequester3.SetVisible(Visible);

		}

		// await LogisticDeliveryIsVisible(true);


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
					Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryToRequester1.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 2
                    Ref_SCMPLATE_ProductRequestDetails_Stage2PurchasedQuantity.SetVisible(false);
					Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryToRequester2.SetVisible(false);
                    // تعداد یا مقدار واگذاری کالا 3
                    Ref_SCMPLATE_ProductRequestDetails_Stage3PurchasedQuantity.SetVisible(false);
					Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryToRequester3.SetVisible(false);
                }

                // شرط دو مرحله ای
                //if (Item.SCMPLATE_PurchaseStageId.ToString() == "dbabd157-37fe-ef11-a4fc-005056a2b6bd")
                if (Item.SCMPLATE_PurchaseStage.Code.ToString() == "2")
                {
                    // تعداد یا مقدار واگذاری کالا 1
                    Ref_SCMPLATE_ProductRequestDetails_Stage1PurchasedQuantity.SetVisible(true);
					Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryToRequester1.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 2
                    Ref_SCMPLATE_ProductRequestDetails_Stage2PurchasedQuantity.SetVisible(true);
					Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryToRequester2.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 3
                    Ref_SCMPLATE_ProductRequestDetails_Stage3PurchasedQuantity.SetVisible(false);
					Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryToRequester3.SetVisible(false);
                }

                // شرط سه مرحله ای
                //if (Item.SCMPLATE_PurchaseStageId.ToString() == "dcabd157-37fe-ef11-a4fc-005056a2b6bd")
                if (Item.SCMPLATE_PurchaseStage.Code.ToString() == "3")
                {
                    // تعداد یا مقدار واگذاری کالا 1
                    Ref_SCMPLATE_ProductRequestDetails_Stage1PurchasedQuantity.SetVisible(true);
					Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryToRequester1.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 2
                    Ref_SCMPLATE_ProductRequestDetails_Stage2PurchasedQuantity.SetVisible(true);
					Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryToRequester2.SetVisible(true);
                    // تعداد یا مقدار واگذاری کالا 3
                    Ref_SCMPLATE_ProductRequestDetails_Stage3PurchasedQuantity.SetVisible(true);
					Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryToRequester3.SetVisible(true);
                }
                // شرط سه مرحله ای
                if (Item.SCMPLATE_PurchaseStage.Code.ToString() == "4")
                {
                }

            }

        }

		public async Task IsEnableTDS_oninput(ChangeEventArgs Selected, Entity.SCMPLATE_ProductRequestDetails Item)
		{
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

			// نمایش تعداد مقدار واگذاری کالا و مراحل واگذاری کالا به انبار
            if (Item.SCMPLATE_PurchaseStageId == null)
            {
                await LogisticDeliveryIsVisible(false);
				await TadarokatBeAnbarIsVisible(false);
			}
            else if (Item.SCMPLATE_PurchaseStageId != null)
            {
                //
                await LogisticDeliveryIsVisible(true);
				// 
				await TadarokatBeAnbarIsVisible(true);
            }
			else  
			{
                // 
                await LogisticDeliveryIsVisible(false);
				// 
				await TadarokatBeAnbarIsVisible(false);
            }

            await LogisticDeliveryAfterVisible(Item);
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
			if (Item.IsExcessPurchasedItem.HasValue && Item.IsExcessPurchasedItem.Value)
			{
				UFIsVisible.SetVisible(true);
			}
			else
			{
				UFIsVisible.SetVisible(false);
			}
		}

// 		public async Task <bool> GridSCMPLATE_ProductRequestId_303_editmodelsaving(object e   )
//         {

//             return false;
//         }
// public async Task  GridSCMPLATE_ProductRequestId_303_afterrendermodal(Entity.SCMPLATE_ProductRequestDetails Item   )
//         {

            
//         }
// public async Task  IsExcessPurchasedItem_oninput(ChangeEventArgs Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
//         {

            
//         }
// public async Task  SCMPLATE_PurchaseStageId_onitemselected(Entity.SCMPLATE_PurchaseStage Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
//         {

            
//         }

		#endregion FunctionEvents

	}
}