using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using CurrieTechnologies.Razor.SweetAlert2;
using BlazorBootstrap;
using Blazored.Toast.Services;

namespace Forms.Forms
{
	public class Form_573Base : Form_573Peropeties
	{
		// SweetAlert2
		[Inject]
		public SweetAlertService Swal { get; set; }

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

			}
		}

		/// <summary>
		/// اعتبار سنجی فرم
		/// </summary>
		/// <returns></returns>
		public override async Task<bool> FormValidator()
		{
			bool IsValid = true;

            var List = _Entity.SCMPETCO_ProductRequestDetails.Where(p=>p.IsDelete == false).ToList();

            var listCount = List.Count();

            // Console.WriteLine("#Log FormValidator :");

            // دکمه ثبت و ادامه           
            if (BtnWorkFlowId == "SequenceFlow_1sfibuv")
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
        public async Task<bool> CheckFieldValidation(Entity.SCMPETCO_ProductRequestDetails Item)
        {
            bool IsValid = true;

			// شرط پُر بودن فیلد نام کالا 
			if (Item.ProductNameText == null)
			{
				IsValid = true;

				toastService.ShowError("لطفا گزینه نام کالا را تکمیل نمایید",
		    		settings =>
		    		{
		    			settings.Timeout = 4;
		    			settings.ShowProgressBar = true;
		    			settings.PauseProgressOnHover = true;
		    		});
			}
			// شرط پُر بودن فیلد تعداد یا مقدار درخواستی
			if (Item.ProductRequestingQTY == null || Item.ProductRequestingQTY == 0)
			{
				IsValid = true;

				toastService.ShowError("لطفا گزینه تعداد یا مقدار درخواستی را تکمیل نمایید.",
		    		settings =>
		    		{
		    			settings.Timeout = 4;
		    			settings.ShowProgressBar = true;
		    			settings.PauseProgressOnHover = true;
		    		});
			}
			// شرط پُر بودن فیلد اولویت
			if (Item.SCMPETCO_PriorityId == null)
			{
				IsValid = true;

				toastService.ShowError("لطفا گزینه اولویت را تکمیل نمایید.",
		    		settings =>
		    		{
		    			settings.Timeout = 4;
		    			settings.ShowProgressBar = true;
		    			settings.PauseProgressOnHover = true;
		    		});
			}
			// شرط پُر بودن فیلد محل مصرف
			if (Item.PlaceOfUseProduct == null)
			{
				IsValid = true;

				toastService.ShowError("لطفا گزینه محل مصرف تکمیل نمایید.",
		    		settings =>
		    		{
		    			settings.Timeout = 4;
		    			settings.ShowProgressBar = true;
		    			settings.PauseProgressOnHover = true;
		    		});
			}

			// Global_SCMRequestTypeId - نوع درخواست
            	if(Item.Global_SCMRequestTypeId == null)
        		{
        		    IsValid = false;

					toastService.ShowError("لطفا گزینه نوع درخواست را انتخاب نمایید.",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
        		}

			return IsValid;

        }

		public async Task ITILVisible(bool Visible)
    	{
    	    Ref_SCMPETCO_ProductRequestDetails_ResultingFromITIL.SetVisible(Visible);
    	}

		public async Task ITILDetailsVisible(bool Visible)
    	{
    	    Ref_SCMPETCO_ProductRequestDetails_RequestIdITIL.SetVisible(Visible);
    	    Ref_SCMPETCO_ProductRequestDetails_RequestIdITIL.SetDisabled(true);

    	    Ref_SCMPETCO_ProductRequestDetails_RequesterUserITIL.SetVisible(Visible);
    	    Ref_SCMPETCO_ProductRequestDetails_RequesterUserITIL.SetDisabled(true);
	
    	    Ref_SCMPETCO_ProductRequestDetails_CreatedAtITIL.SetVisible(Visible);
    	    Ref_SCMPETCO_ProductRequestDetails_CreatedAtITIL.SetDisabled(true);
	
    	    Ref_SCMPETCO_ProductRequestDetails_ITILDetails.SetVisible(Visible);
    	}

		public async Task TahvilIsVisible(bool Visible, bool Value, Entity.SCMPETCO_ProductRequestDetails Item)
		{
			// تحویل
			Ref_SCMPETCO_ProductRequestDetails_ProductDelivery.SetVisible(Visible);
			Item.ProductDelivery = Value;
		}
		
		public async Task KharidIsVisible(bool Visible, bool Value, Entity.SCMPETCO_ProductRequestDetails Item)
		{
			// خرید
			Ref_SCMPETCO_ProductRequestDetails_FutureAction.SetVisible(Visible);
			Item.FutureAction = Value;
			// تعداد تامین کسری	
			Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber.SetVisible(Visible);
		}
		
		public async Task TahvilKharidIsVisible(bool Visible, bool Value, Entity.SCMPETCO_ProductRequestDetails Item)
		{
			// تحویل
			Ref_SCMPETCO_ProductRequestDetails_ProductDelivery.SetVisible (Visible);
			Item.ProductDelivery = Value;
			// خرید
			Ref_SCMPETCO_ProductRequestDetails_FutureAction.SetVisible(Visible);
			Item.FutureAction = Value;
			// تعداد تامین کسری
			Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber.SetVisible (Visible);
		}


		public async Task<bool> SCMPETCO_ProductRequestDetails_editmodelsaving(object e)
		{
 			bool IsValid = false;

            var Item = (Entity.SCMPETCO_ProductRequestDetails)e;

            IsValid = !await CheckFieldValidation(Item);

            return IsValid;
		}


		public async Task  SCMPETCO_ProductRequestDetails_afterrendermodal(Entity.SCMPETCO_ProductRequestDetails Item   )
        {
			// فیلد نوع درخواست
            if (Item.Global_SCMRequestTypeId == null)
            {
                await TahvilKharidIsVisible(false,false,Item);
            }
			else
			{
				// شرط اینکه آیا فرآیند تحویل اجرا گردد؟
            	if(Item.Global_SCMRequestTypeId.ToString() =="a9c5df1c-d99c-ef11-8354-005056a02a64")
            	{
            	    await TahvilIsVisible(true,true,Item);
					await KharidIsVisible(false,false,Item);
            	}
	
            	// شرط نوع درخواست بر اساس خرید کالا
            	if(Item.Global_SCMRequestTypeId.ToString() =="3b9e934d-d99c-ef11-8354-005056a02a64")
            	{
            	    await TahvilIsVisible(false,false,Item);
					await KharidIsVisible(true,true,Item);
            	}
	
            	// شرط نوع درخواست بر اساس تحویل و خرید کالا
            	if(Item.Global_SCMRequestTypeId.ToString() =="73f6c459-d99c-ef11-8354-005056a02a64")
            	{
            	    await TahvilKharidIsVisible(true,true,Item);
            	}
			}


			// Show ITIL
        	if (Item.ITILCodeIsEnable.HasValue && Item.ITILCodeIsEnable.Value)
			{
        	    await ITILVisible(true); 
			}
			else
			{
        	    await ITILVisible(false);
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
     			Ref_SCMPETCO_ProductRequestDetails_ITILDetails.SetEntity(Item);
				Ref_SCMPETCO_ProductRequestDetails_ITILDetails.LoadData();
 			}

			// فایل مدارک
        	var UploadFilesIsVisible = Ref_SCMPETCO_ProductRequestDetails_SCMPETCO_ProductRequestDetails_ProductDataSheetFile;
        	if (Item.ProductDataSheet.HasValue && Item.ProductDataSheet.Value)
			{
				UploadFilesIsVisible.SetVisible(true); 
			}
			else
			{
				UploadFilesIsVisible.SetVisible(false); 
			}
        }
		
		public async Task  ITILCodeIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCMPETCO_ProductRequestDetails Item  )
        {
			// نمایش / عدم نمایش فیلد منتج از ITIL
        	if (Selected.Value.ToString() == "true")
	    	{
        	    await ITILVisible(true);
	    	}
	    	else
	    	{
        	    await ITILVisible(false); 
	    	} 
        }
		
		public async Task  ResultingFromITIL_onitemselected(dynamic Selected ,Entity.SCMPETCO_ProductRequestDetails Item  )
        {
			// نمایش / عدم نمایش فیلد ITIL Detail
        	await ITILDetailsVisible(true);

        	if (Item.ResultingFromITIL != null)
	    	{
	    		Item.RequestIdITIL = Selected.RequestID;
	    		Item.RequesterUserITIL = Selected.UserName;
	    		Item.CreatedAtITIL = Selected.CreateDate;

				Ref_SCMPETCO_ProductRequestDetails_ITILDetails.SetEntity(Item);
				Ref_SCMPETCO_ProductRequestDetails_ITILDetails.LoadData();
	    	}
        }
		
		public async Task  Global_SCMRequestTypeId_onitemselected(Entity.Global_SCMRequestType Selected ,Entity.SCMPETCO_ProductRequestDetails Item  )
        {
			// Console.WriteLine(await Utility.JSON.ToJson(Selected));

           	// شرط اینکه آیا فرآیند تحویل اجرا گردد؟
            if(Item.Global_SCMRequestTypeId.ToString() == "a9c5df1c-d99c-ef11-8354-005056a02a64")
            {
                await TahvilIsVisible(true,true,Item);
				await KharidIsVisible(false,false,Item);
            }

            // شرط نوع درخواست بر اساس خرید کالا
            if(Item.Global_SCMRequestTypeId.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64")
            {
                await TahvilIsVisible(false,false,Item);
				await KharidIsVisible(true,true,Item);
            }

            // شرط نوع درخواست بر اساس تحویل و خرید کالا
            if(Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64")
            {
                await TahvilKharidIsVisible(true,true,Item);
            }
        }

		public async Task  ProductDataSheet_oninput(ChangeEventArgs Selected ,Entity.SCMPETCO_ProductRequestDetails Item  )
        {
			// نمایش / عدم نمایش فیلد فایل مدارک
        	var UploadFilesIsVisible = Ref_SCMPETCO_ProductRequestDetails_SCMPETCO_ProductRequestDetails_ProductDataSheetFile;
			if (Selected.Value.ToString() == "true")
			{
				UploadFilesIsVisible.SetVisible(true); 
			}
			else
			{
				UploadFilesIsVisible.SetVisible(false); 
			}            
        }

		public async Task  ProductName_NotMapped_onitemselected(dynamic Selected ,Entity.SCMPETCO_ProductRequestDetails Item  )
        {
			/// <summary>
			/// فیلدهای زیر فیلدهای اصلی برای نمایش در فرم هستند.
			///
			///</summary>
			
			// Console.WriteLine("start");
			//  نام کالا
			Item.ProductNameText = Selected.DESC;
			//  نام دسته بندی فرعی
			Item.ProductSubCategoryText = Selected.SubGroupName;
			// کد کالا
			Item.ProductCodeText = Selected.PARTNO;
			// واحد کالا
			Item.ProductUnitText = Selected.UNIT;
			//دسته بندی اصلی کالا
			Item.ProductMainCategoryText = Selected.GroupName;
			// شناسه دسته بندی اصلی کالا
			Item.ProductMainCategoryIdText = Selected.GRCODE;
			// شناسه دسته بندی فرعی
			Item.ProductSubCategoryIdText = Selected.SUBGRCODE;
			// سال مالی شماران
			Item.ShomaranFiscalYearText = Selected.YEAR;
			// موجودی کالا در شماران        
			if (Selected.Amount > -1)
			{
				Item.ProductInventoryText = (double)Selected.Amount;
			}
			// Console.WriteLine("End");
        }

		#endregion FunctionEvents

	}
}
