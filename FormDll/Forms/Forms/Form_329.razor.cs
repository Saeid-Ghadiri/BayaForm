using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using System.Globalization;
using Blazored.Toast.Services;

namespace Forms.Forms
{
	public class Form_329Base : Form_329Peropeties
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
				// حذف Defualt Value ها
				// var List = _Entity.SCMPETCO_ProductRequestDetails.ToList();
				// foreach (var Item in List)
				// {
				// 	// // آیا تامین کسری انجام شود؟
				// 	// Item.FutureAction = null;
				// 	// // آیا فرآیند تحویل اجرا گردد؟
				// 	// Item.ProductDelivery = null;

				// 	// نحوه تامین کالا
				// 	//Item.ForeignMachineryProduct=null;
				// }
				// StateHasChanged();
			}
		}

		/// <summary>
		/// اعتبار سنجی فرم
		/// </summary>
		/// <returns></returns>
		public override async Task<bool> FormValidator()
		{
			bool IsValid = true;

			var List = _Entity.SCMPETCO_ProductRequestDetails.ToList();
			foreach (var Item in List)
			{
				// - فیلد نوع درخواست
				if (Item.Global_SCMRequestTypeId == null)
				{
					IsValid = false;
					toastService.ShowError("لطفا گزینه نوع درخواست را تکمیل نمایید",
						settings =>
						{
							settings.Timeout = 4;
							settings.ShowProgressBar = true;
							settings.PauseProgressOnHover = true;
						});
				}

				// // نحوه تامین کالا
				// if (Item.ForeignMachineryProduct == null)
				// {
				// 	IsValid = false;
				// 	toastService.ShowError("لطفا گزینه نحوه تامین کالا را تکمیل نمایید",
				// 		settings =>
				// 		{
				// 			settings.Timeout = 4;
				// 			settings.ShowProgressBar = true;
				// 			settings.PauseProgressOnHover = true;
				// 		});
				// }

				// Note: بررسی 2 گزینه تحویل و یا خرید	
				if(!((Item.FutureAction.HasValue && Item.FutureAction.Value) || (Item.ProductDelivery.HasValue && Item.ProductDelivery.Value)))
            	{
					IsValid = false;
            	    toastService.ShowError("لطفا یکی از دو گزینه تحویل گردد یا تامین کسری گردد را بله قرار دهید.",
		    			settings =>
		    			{
		    				settings.Timeout = 4;
		    				settings.ShowProgressBar = true;
		    				settings.PauseProgressOnHover = true;
		    			});
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

		public async Task ProductName_NotMapped_onitemselected(dynamic Selected ,Entity.SCMPETCO_ProductRequestDetails Item  )
        {
			/// <summary>
			/// فیلدهای زیر فیلدهای اصلی برای نمایش در فرم هستند.
			///</summary>
			
			//  نام کالا
			Item.ProductNameText = Selected.DESC;
			//Console.WriteLine(Selected.DESC);
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
        }

		 public async Task<bool> SCMPETCO_ProductRequestDetails_editmodelsaving(object e)
		 {
		// 	bool IsValid = false;
		// 	var MainModel = (Entity.SCMPETCO_ProductRequestDetails)e;


		//     // Note نحوه تامین کالا		
		//     if (MainModel.Global_SCMRequestTypeId == null)
		//     {
		//     	IsValid = true;
		//     	toastService.ShowError("لطفا گزینه  نوع درخواست را تکمیل نمایید.",
		//     		settings =>
		//     		{
		//     			settings.Timeout = 4;
		//     			settings.ShowProgressBar = true;
		//     			settings.PauseProgressOnHover = true;
		//     		});
		//     }

			

		    // Note نحوه تامین کالا		
		//     if (MainModel.ForeignMachineryProduct == null)
		//     {
		//     	IsValid = true;
		//     	toastService.ShowError("لطفا گزینه  نحوه تامین کالا تکمیل نمایید.",
		//     		settings =>
		//     		{
		//     			settings.Timeout = 4;
		//     			settings.ShowProgressBar = true;
		//     			settings.PauseProgressOnHover = true;
		//     		});
		//     }

		 	return false;
		 }

		public async Task SCMPETCO_ProductRequestDetails_customizeeditmodel(GridCustomizeEditModelEventArgs e)
		{
			// var Item = (Entity.SCMPETCO_ProductRequestDetails)e.EditModel;
			// if (e.IsNew)
			// {
			// 	// 
			// 	//Item.
			// 	// آیا تامین کسری انجام شود؟
			// 	Item.FutureAction = true;
			// 	// آیا فرآیند تحویل اجرا گردد؟
			// 	Item.ProductDelivery = true;
			// 	// نحوه تامین کالا
			// 	//Item.ForeignMachineryProduct = null;
			// }
		}

		public async Task SCMPETCO_ProductRequestDetails_afterrendermodal(Entity.SCMPETCO_ProductRequestDetails Item)
		{
			// Note: مخفی کردن فیلد تعداد تامین کسری بر اساس فیلد آیا تامین کسری دارد؟
			if (Item.FutureAction.HasValue && Item.FutureAction.Value)
			{
				Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true); 
			}
			else
			{
				Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false); 
			}

            // در حالت نال هر دو فقط مخفی باشد
            if (Item.Global_SCMRequestTypeId == null)
            {
                Ref_SCMPETCO_ProductRequestDetails_ProductDelivery.SetVisible (false);
                Ref_SCMPETCO_ProductRequestDetails_FutureAction.SetVisible (false);
				Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber.SetVisible (false); 
            }
			else{
				// شرط اینکه آیا فرآیند تحویل اجرا گردد؟
            	if(Item.Global_SCMRequestTypeId.ToString() =="a9c5df1c-d99c-ef11-8354-005056a02a64")
            	{
            	    Ref_SCMPETCO_ProductRequestDetails_ProductDelivery.SetVisible(true); 
            	    Ref_SCMPETCO_ProductRequestDetails_FutureAction.SetVisible(false); 
            	}
	
            	// شرط نوع درخواست بر اساس خرید کالا
            	if(Item.Global_SCMRequestTypeId.ToString() =="3b9e934d-d99c-ef11-8354-005056a02a64")
            	{
            	    Ref_SCMPETCO_ProductRequestDetails_ProductDelivery.SetVisible(false); 
            	    Ref_SCMPETCO_ProductRequestDetails_FutureAction.SetVisible(true); 
            	}
	
            	// شرط نوع درخواست بر اساس تحویل و خرید کالا
            	if(Item.Global_SCMRequestTypeId.ToString() =="73f6c459-d99c-ef11-8354-005056a02a64")
            	{
            	    Ref_SCMPETCO_ProductRequestDetails_ProductDelivery.SetVisible(true); 
            	    Ref_SCMPETCO_ProductRequestDetails_FutureAction.SetVisible(true); 
            	}
			}
		}

		public async Task FutureAction_oninput(ChangeEventArgs Selected, Entity.SCMPETCO_ProductRequestDetails Item)
		{
			// Note: مخفی کردن فیلد تعداد تامین کسری بر اساس فیلد آیا تامین کسری دارد؟       
			var OkItem = Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber;
			if (Selected.Value.ToString() == "true")
			{
				OkItem.SetVisible(true); 
			}
			else
			{
				OkItem.SetVisible(false); 
			}
		}

		public async Task Global_SCMRequestTypeId_onitemselected(Entity.Global_SCMRequestType Selected ,Entity.SCMPETCO_ProductRequestDetails Item  )
        {
            //Console.WriteLine(await Utility.JSON.ToJson(Selected));

            // شرط نوع درخواست بر اساس تحویل کالا
            if(Selected.Id.ToString() =="a9c5df1c-d99c-ef11-8354-005056a02a64")
            {
                //Ref_SCMPETCO_ProductRequestDetails_ProductDelivery.Visible = true;
				Ref_SCMPETCO_ProductRequestDetails_ProductDelivery.Value =true;
                Ref_SCMPETCO_ProductRequestDetails_FutureAction.Value = false;
                //Ref_SCMPETCO_ProductRequestDetails_FutureAction.Visible = false;
                Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);               
            }
         
            // شرط نوع درخواست بر اساس خرید کالا
            if(Selected.Id.ToString() =="3b9e934d-d99c-ef11-8354-005056a02a64")
            {
                //Ref_SCMPETCO_ProductRequestDetails_ProductDelivery.Visible = false;
				Ref_SCMPETCO_ProductRequestDetails_ProductDelivery.Value = false;				
                //Ref_SCMPETCO_ProductRequestDetails_FutureAction.SetVisible(true);
                Ref_SCMPETCO_ProductRequestDetails_FutureAction.Value =true;
              
                if (Ref_SCMPETCO_ProductRequestDetails_FutureAction.Value.HasValue && Ref_SCMPETCO_ProductRequestDetails_FutureAction.Value.Value )
                {
                    Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true);  
                }
                else{
                    Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);  
                }
            }
         
            // شرط نوع درخواست بر اساس تحویل و خرید کالا
            if(Selected.Id.ToString() =="73f6c459-d99c-ef11-8354-005056a02a64")
            {
                //Ref_SCMPETCO_ProductRequestDetails_ProductDelivery.Visible = true;
				Ref_SCMPETCO_ProductRequestDetails_ProductDelivery.Value = true;
                //Ref_SCMPETCO_ProductRequestDetails_FutureAction.SetVisible(true);
                Ref_SCMPETCO_ProductRequestDetails_FutureAction.Value =true;
                      
                if (Ref_SCMPETCO_ProductRequestDetails_FutureAction.Value.HasValue && Ref_SCMPETCO_ProductRequestDetails_FutureAction.Value.Value)
                {
                    Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true);  
                }
                else{
                    Ref_SCMPETCO_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false); 
                }
            }
        }



		#endregion FunctionEvents

	}
}
