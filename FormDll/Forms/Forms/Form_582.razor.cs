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
	public class Form_582Base : Form_582Peropeties
	{

		// Toast  
		[Inject]
		public IToastService toastService { get; set; }



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



		/// <summary>
		/// رندر شدن فرم
		/// </summary>
		/// <param name="firstRender"></param>
		/// <returns></returns>
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				//Console.WriteLine("#log OnAfterRenderAsync   _RequestParameters:");
				//Console.WriteLine(await Utility.JSON.ToJson(_RequestParameters));
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

			if (ListCount == 0)
			{
				IsValid = false;

				var options = new ConfirmDialogOptions
				{
					YesButtonText = "بازگشت به درخواست",
					YesButtonColor = ButtonColor.Danger,
					NoButtonText = "",
				};

				string htmlString =
				"<div>" +
					"<picture>" +
						"<img src='https://file.workcv.ir/fa/api/v1/File/Get?FileID=6e5b6fb8-a5b2-490c-f83f-08dbea5b8061' class='' alt='لوگو پل فیلم' width='96px;'>" +
					"</picture>" +
					"<hr class='hrdash border-success-subtle'>" +
				"</div>" +
				"<div class='fw-bold text-center'>" +
				"<span class='fs-5'>کد پیگیری این درخواست: </span>" +
				"<span class='fs-3' style='color: #1ba156'>" + _Entity.RequestTrakingCode + "</span><div>" +
				"<span><i class='fal fa-exclamation-triangle' style='font-size:24px; color:red;'></i>&nbsp;</span>" +
				"<span class='fs-6 text-secondary text-right'>تا کنون هیچ ردیف درخواستی تکمیل نشده است. لطفا برای ثبت و ادامه به مرحله بعد حداقل یک ردیف در درخواست خود ثبت نمایید.	<span></div>" +
				"</div>";

				var confirmation = await Confirm.ShowAsync(
					title: "",
					message1: htmlString,
					confirmDialogOptions: options);
			}

			foreach (var Item in List)
			{
				// Global_SCMRequestTypeId - نوع درخواست
				if (Item.Global_SCMRequestTypeId == null)
				{
					IsValid = false;

					toastService.ShowError("لطفا گزینه نوع درخواست را انتخاب نمایید.",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
				}

				// در صورتی که خرید یا تحویل و خرید باشد گزینه تعداد تامین کسری ضروری گردد.
				if (Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64" ||
				  Item.Global_SCMRequestTypeId.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64")
				{
					if (Item.DeficitSupplyNumber == null)
					{
						IsValid = false;
						toastService.ShowError("لطفا گزینه تعداد تامین کسری تکمیل گردد.",
						settings =>
						{
							settings.Timeout = 4;
							settings.ShowProgressBar = true;
							settings.PauseProgressOnHover = true;
						});
					}
				}

				// در صورتی که خرید یا تحویل و خرید باشد گزینه حدود هزینه خرید ضروری گردد.
				if (Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64" ||
				  Item.Global_SCMRequestTypeId.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64")
				{
					if (Item.PurchasePrice == null)
					{
						IsValid = false;
						toastService.ShowError("لطفا گزینه حدود قیمت کالا تکمیل گردد.",
						settings =>
						{
							settings.Timeout = 4;
							settings.ShowProgressBar = true;
							settings.PauseProgressOnHover = true;
						});
					}
				}

				if (Item.ResultingFrom_ITIL == null)
				{
					IsValid = false;
					toastService.ShowError("لطفا گزینه منتج از ITIL تکمیل گردد",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
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


			}

			// SCM_ICT_DepartmentsId - بخش‌های فناوری اطلاعات
			if (_Entity.SCM_ICT_DepartmentsId == null)
			{
				IsValid = false;

				toastService.ShowError("لطفا گزینه بخش‌های فناوری اطلاعات را انتخاب نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
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
			"<span class='fs-4 fw-bold'>" + _Entity.RequestTrakingCode + "</span><hr class='hrdash'><div>" +
			"<div><span class='fw-normal'>کد تحویل کالا: </span>" +
			"<span class='fs-3 text-green fw-bold'>" + RandomDeliveryCode + "</span>" +
			"<span class='btn btn-yellow-light btn-sm mx-2 mb-2' onclick=\"navigator.clipboard.writeText('" + RandomDeliveryCode + "'); $('#copylable').removeClass('d-none');\"><i class='fal fa-copy'></i><span class='px-1'>کپی</span></span></div>" +
			"<p id='copylable' class='d-none text-green mt-3'>کد تحویل کالا کپی شد.</p>" +
			"<div><hr class='hrdash'><span class='fal fa-exclamation-circle text-red btnicon mx-1' style='font-size: 24px;'></span>" +
			"<span>لطفا در زمان دریافت کالا از انبار، کد فوق را به انباردار اعلام نمایید.</span>" +
			"</div>";

			var confirmation = await Confirm.ShowAsync(
				title: "",
				message1: htmlString,
				confirmDialogOptions: options);

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
			//Console.WriteLine("#log _RequestParameters:");
			//Console.WriteLine(await Utility.JSON.ToJson(_RequestParameters));
		}

		#region FunctionEvents

		/// <summary>
		/// کد تحویل رندوم 
		/// </summary>
		/// <returns></returns>
		public int RandomDeliveryCode { get; set; }

		// نمایش فیلد itil
		public async Task ITILVisible(bool Visible)
		{
			Ref_SCM_ProductRequestDetails_ResultingFrom_ITIL.SetVisible(Visible);
		}

		// نمایش فیلد جزئیات itil
		public async Task ITILDetailsVisible(bool Visible)
		{
			Ref_SCM_ProductRequestDetails_RequestIdITIL.SetVisible(Visible);
			Ref_SCM_ProductRequestDetails_RequestIdITIL.SetDisabled(true);

			Ref_SCM_ProductRequestDetails_RequesterUserITIL.SetVisible(Visible);
			Ref_SCM_ProductRequestDetails_RequesterUserITIL.SetDisabled(true);

			Ref_SCM_ProductRequestDetails_CreatedAtITIL.SetVisible(Visible);
			Ref_SCM_ProductRequestDetails_CreatedAtITIL.SetDisabled(true);

			Ref_SCM_ProductRequestDetails_ITILDetails.SetVisible(Visible);
		}

		public async Task ITILNull(Entity.SCM_ProductRequestDetails Item)
		{
			Item.ResultingFrom_ITIL = null;
			Item.RequestIdITIL = null;
			Item.RequesterUserITIL = null;
			Item.CreatedAtITIL = null;
			Item.ITILDetails = null;
		}

		// تحویل
		public async Task TahvilIsVisible(bool Visible, bool Value, Entity.SCM_ProductRequestDetails Item)
		{
			Ref_SCM_ProductRequestDetails_ProductDelivery.SetVisible(Visible);
			Item.ProductDelivery = Value;
		}

		// خرید
		public async Task KharidIsVisible(bool Visible, bool Value, Entity.SCM_ProductRequestDetails Item)
		{
			Ref_SCM_ProductRequestDetails_FutureActionTrueFalse.SetVisible(Visible);
			Item.FutureActionTrueFalse = Value;
			// تعداد تامین کسری	
			Ref_SCM_ProductRequestDetails_DeficitSupplyNumber.SetVisible(Visible);
			Ref_SCM_ProductRequestDetails_PurchasePrice.SetVisible(Visible);
		}

		// تحویل و خرید
		public async Task TahvilKharidIsVisible(bool Visible, bool Value, Entity.SCM_ProductRequestDetails Item)
		{
			// تحویل
			Ref_SCM_ProductRequestDetails_ProductDelivery.SetVisible(Visible);
			Item.ProductDelivery = Value;
			// خرید
			Ref_SCM_ProductRequestDetails_FutureActionTrueFalse.SetVisible(Visible);
			Item.FutureActionTrueFalse = Value;
			// تعداد تامین کسری
			Ref_SCM_ProductRequestDetails_DeficitSupplyNumber.SetVisible(Visible);
			Ref_SCM_ProductRequestDetails_PurchasePrice.SetVisible(Visible);
		}

		public async Task ProductName_NotMapped_onitemselected(dynamic Selected, Entity.SCM_ProductRequestDetails Item)
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

			Console.WriteLine("Selected.PARTCODE : " + Selected.PARTCODE);
			Item.PARTCODE = Selected.PARTCODE;
		}

		public async Task<bool> GridSCM_ProductRequestId_182_editmodelsaving(object e)
		{
			bool IsCancelled = false;

			// Item = Detail Model
			var Item = (Entity.SCM_ProductRequestDetails)e;

			var ListCount = _Entity.SCM_ProductRequestDetails.Count();
            if (ListCount > 5)
            {
                IsCancelled = true;

                toastService.ShowError("تعداد ردیف‌ها نمی‌تواند بیشتر از 5 باشد",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });

                return IsCancelled;
            }

			// بررسی کد تحویل
			if (!Item.DeliveryCode.HasValue)
			{
				Item.DeliveryCode = RandomDeliveryCode;
			}

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

			// شرط نوع درخواست بر اساس تحویل و خرید کالا
			if (Item.Global_SCMRequestTypeId.HasValue && Item.Global_SCMRequestTypeId.Value.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64")
			{
				// در صورتی که موجودی کالا 0 باشد و یا نال باشد خطا نمایش داده شود.
				if (!Item.ProductInventoryText.HasValue || Item.ProductInventoryText == 0)
				{
					IsCancelled = true;

					toastService.ShowError("به دلیل نداشتن موجودی کالا، امکان تحویل آن وجود ندارد.",
						settings =>
						{
							settings.Timeout = 4;
							settings.ShowProgressBar = true;
							settings.PauseProgressOnHover = true;
						});
				}
			}

			if (Item.ResultingFrom_ITIL == null)
			{
				IsCancelled = true;
				toastService.ShowError("لطفا گزینه منتج از ITIL تکمیل گردد",
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

			// در صورتی که خرید یا تحویل و خرید باشد گزینه حدود هزینه خرید ضروری گردد.
			if (Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64" ||
			  Item.Global_SCMRequestTypeId.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64")
			{
				if (Item.PurchasePrice == null)
				{
					IsCancelled = true;
					toastService.ShowError("لطفا گزینه حدود قیمت کالا تکمیل گردد.",
					settings =>
					{
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
				}
			}

			return IsCancelled;
		}


		public async Task GridSCM_ProductRequestId_182_afterrendermodal(Entity.SCM_ProductRequestDetails Item)
		{
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

			await ITILVisible(true);
			// Show ITIL Details
			if (!string.IsNullOrEmpty(Item.ResultingFrom_ITIL))
			{
				await ITILDetailsVisible(true);
			}
			else
			{
				await ITILDetailsVisible(false);
			}
			// نمایش جزئیات ITIL
			//if (!string.IsNullOrEmpty(Item.ResultingFrom_ITIL))
			if (Item.ResultingFrom_ITIL != null)
			{
				await Task.Delay(300);
				Ref_SCM_ProductRequestDetails_ITILDetails.SetEntity(Item);
				Ref_SCM_ProductRequestDetails_ITILDetails.LoadData();
			}
		}

		public async Task ResultingFrom_ITIL_onitemselected(dynamic Selected, Entity.SCM_ProductRequestDetails Item)
		{
			await SetITILDetailsVisible(true, Selected, Item);
		}
		public async Task SetITILDetailsVisible(bool Visible, dynamic? Selected, Entity.SCM_ProductRequestDetails Item)
		{
			// await Task.Delay(100);
			// نمایش / عدم نمایش فیلد ITIL Detail

			if (Item.ResultingFrom_ITIL != null)
			{
				await ITILDetailsVisible(Visible);
				if (Visible)
				{
					// await Task.Delay(300);
					Item.RequestIdITIL = Selected.RequestID;
					Item.RequesterUserITIL = Selected.UserName;
					Item.CreatedAtITIL = Selected.CreateDate;
					Ref_SCM_ProductRequestDetails_ITILDetails.SetEntity(Item);
					Ref_SCM_ProductRequestDetails_ITILDetails.LoadData();
				}
				else
				{
					Item.RequestIdITIL = null;
					Item.RequesterUserITIL = null;
					Item.CreatedAtITIL = null;
					Ref_SCM_ProductRequestDetails_ITILDetails.SetEntity(Item);
					Ref_SCM_ProductRequestDetails_ITILDetails.LoadData();
				}
			}
		}
		public async Task Global_SCMRequestTypeId_onitemselected(Entity.Global_SCMRequestType Selected, Entity.SCM_ProductRequestDetails Item)
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

		#endregion FunctionEvents

	}
}