using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using BlazorBootstrap;
using Blazored.Toast.Services;

namespace Forms.Forms
{
	public class Form_850Base : Form_850Peropeties
	{

		// Toast  
		[Inject]
		public IToastService toastService { get; set; }

		/// <summary>
		/// کد تحویل رندوم 
		/// </summary>
		/// <returns></returns>
		public int RandomDeliveryCode { get; set; }

		public async Task GenerateAndCheckDeliveryCode()
		{
			//شش رقمی
			RandomDeliveryCode = new Random().Next(100000, 999999);
		}


		/// <summary>
		/// آماده سازی فرم
		/// </summary>
		/// <returns></returns>
		protected override async Task OnInitializedAsync()
		{
			// تولید کد رندوم و بررسی آن
			await GenerateAndCheckDeliveryCode();
		}

		public async Task AcceptDeliveryCode()
		{
			// پرکردن فیلد کد رهگیری
			// Console.WriteLine("DeliveryCode:");
			// Console.WriteLine(DeliveryCode+"");
			var List = _Entity.SCM_ProductRequestDetails.ToList();
			foreach(var Item in List)
			{
				if(!Item.DeliveryCode.HasValue)
            	{
                	Item.DeliveryCode = RandomDeliveryCode;
            	}
			}
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
			
			var List = _Entity.SCM_ProductRequestDetails.ToList();
        
        	int ListCount = List.Count();

        	if(ListCount==0)
        	{
        	    IsValid = false;
	
        	    var options = new ConfirmDialogOptions
				{
					YesButtonText = "بازگشت به درخواست",
					YesButtonColor = ButtonColor.Danger,
					NoButtonText = "",
				};

				string htmlString = 
        	    "<div>"+
        	        "<picture>"+
        	            "<img src='https://file.workcv.ir/fa/api/v1/File/Get?FileID=6e5b6fb8-a5b2-490c-f83f-08dbea5b8061' class='' alt='لوگو پل فیلم' width='96px;'>"+
        	        "</picture>"+
        	        "<hr class='hrdash border-success-subtle'>"+
        	    "</div>"+
        	    "<div class='fw-bold text-center'>" + 
        	    "<span class='fs-5'>کد پیگیری این درخواست: </span>" + 
				"<span class='fs-3' style='color: #1ba156'>" + _Entity.RequestTrakingCode + "</span><div>"+
        	    "<span><i class='fal fa-exclamation-triangle' style='font-size:24px; color:red;'></i>&nbsp;</span>" +
				"<span class='fs-6 text-secondary text-right'>تا کنون هیچ ردیف درخواستی تکمیل نشده است. لطفا برای ثبت و ادامه به مرحله بعد حداقل یک ردیف در درخواست خود ثبت نمایید.	<span></div>" +
        	    "</div>"; 

				var confirmation = await Confirm.ShowAsync(
					title: "",
					message1: htmlString,
					confirmDialogOptions: options);
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
			// 
			await AcceptDeliveryCode();


			#region DeliveryCode Confirm
			// نمایش کد تحویل کالا در زمان ثبت درخواست
			var options = new ConfirmDialogOptions
			{
				YesButtonText = "ادامه فرآیند",
				YesButtonColor = ButtonColor.Success,
				NoButtonText = "",
			};

			string htmlString = 
            "<div class='text-center'>" + 
            "<span>کد پیگیری درخواست: </span>" + 
			"<span class='fs-4 fw-bold'>" + _Entity.RequestTrakingCode + "</span><hr class='hrdash'><div>"+
			"<div><span class='fw-normal'>کد تحویل کالا: </span>" + 
			"<span class='fs-3 text-green fw-bold'>" + RandomDeliveryCode + "</span>" +
            "<span class='btn btn-yellow-light btn-sm mx-2 mb-2' onclick=\"navigator.clipboard.writeText('" + RandomDeliveryCode + "'); $('#copylable').removeClass('d-none');\"><i class='fal fa-copy'></i><span class='px-1'>کپی</span></span></div>" +
            "<p id='copylable' class='d-none text-green mt-3'>کد تحویل کالا کپی شد.</p>" +  
            "<div><hr class='hrdash'><span class='fal fa-exclamation-circle text-red btnicon mx-1' style='font-size: 24px;'></span>"+
			"<span>لطفا در زمان دریافت کالا از انبار، کد فوق را به انباردار اعلام نمایید.</span>" +
            "</div>";

			// MarkupString htmlContent = new MarkupString(htmlString);

			var confirmation = await Confirm.ShowAsync(
				title: "",
				message1: htmlString,
				confirmDialogOptions: options);

			// if (confirmation)
			// {		
			// 	toastService.ShowSuccess("لطفا کد تحویل را بخاطر بسپارید!!",
			// 		settings => {
			// 			settings.Timeout = 4;
			// 			settings.ShowProgressBar = true;
			// 			settings.PauseProgressOnHover = true;
			// 		});
			// }
			// else
			// {
			// 	toastService.ShowError("*-*-*-*-*-*-*-*-*-*");
			// }
			#endregion / DeliveryCode Confirm

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

		public async Task <bool> GridSCM_ProductRequestId_182_editmodelsaving(object e   )
        {

           bool IsCancelled = false;

			var Item = (Entity.SCM_ProductRequestDetails)e;

			if(!Item.DeliveryCode.HasValue)
            {
                Item.DeliveryCode = RandomDeliveryCode;
            }
			//await GenerateDeliveryCode();

			// شرط پُر بودن فیلد نام کالا دیزیبل
			if (Item.ProductNameText == null)
			{
				IsCancelled = true;

				toastService.ShowError("لطفا گزینه نام کالا را تکمیل نمایید",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}
			// شرط پُر بودن فیلد تعداد یا مقدار درخواستی
			if (Item.ProductRequestingQTY == null || Item.ProductRequestingQTY == 0)
			{
				IsCancelled = true;

				toastService.ShowError("لطفا گزینه تعداد یا مقدار درخواستی را تکمیل نمایید.",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}
			// شرط پُر بودن فیلد اولویت
			if (Item.SCM_PriorityId == null)
			{
				IsCancelled = true;

				toastService.ShowError("لطفا گزینه اولویت را تکمیل نمایید.",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}

			// شرط پُر بودن فیلد محل مصرف انبار مواد
			if (Item.SCM_MaterialWarehouse_PlaceofUseId == null)
			{
				IsCancelled = true;

				toastService.ShowError("لطفا گزینه محل مصرف انبار مواد را تکمیل نمایید.",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}
			// شرط پُر بودن فیلد محل مصرف
			if (Item.PlaceOfUseProduct == null)
			{
				IsCancelled = true;

				toastService.ShowError("لطفا گزینه محل مصرف تکمیل نمایید.",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}

			return IsCancelled;
        }
		
		public async Task  GridSCM_ProductRequestId_182_afterrendermodal(Entity.SCM_ProductRequestDetails Item   )
        {
        }

		public async Task  ProductRequestingQTY_oninput(ChangeEventArgs Selected ,Entity.SCM_ProductRequestDetails Item  )
        {
        }

		public async Task  ProductName_NotMapped_onitemselected(dynamic Selected ,Entity.SCM_ProductRequestDetails Item  )
        {
			/// <summary>
			/// فیلدهای زیر فیلدهای اصلی برای نمایش در فرم هستند.
			///
			///</summary>

			//Console.WriteLine("start");
			//  نام کالا
			Item.ProductNameText = Selected.DESC;
			//Console.WriteLine(Selected.DESC);
			//  نام دسته بندی فرعی
			Item.ProductSubCategoryText = Selected.SubGroupName;
			//Console.WriteLine(Selected.SubGroupName);
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
			//کالا موجود است یا خیر
			Item.IsExistText = Selected.IsExist;
			// کد اصلی گروه کالا شماران 
			Item.MapGroupCodeNum = Selected.MapGroupCode;
			// موجودی کالا در شماران
			if (Selected.Amount > -1)
			{
				Item.ProductInventoryText = (double)Selected.Amount;
			}
			//Console.WriteLine("End");
            
        }

		#endregion FunctionEvents

	}
}