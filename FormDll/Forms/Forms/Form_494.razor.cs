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
	public class Form_494Base : Form_494Peropeties
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
				// // حذف Defualt Value ها
				// var List = _Entity.SCMPETCO_ProductRequestDetails.ToList();
				// foreach (var Item in List)
				// {
				// 	// // آیا تامین کسری انجام شود؟
				// 	// Item.FutureAction = null;
				// 	// // آیا فرآیند تحویل اجرا گردد؟
				// 	// Item.ProductDelivery = null;
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
				// Note:  بررسی کد تحویل وارد شده توسط انباردار
				if (Item.DeliveryCode != Item.GetDeliveryCode)
				{
					IsValid = false;
					toastService.ShowError("کد تحویل وارد شده صحیح نیست!! لطفا کد صحیح را وارد نمایید.",
						settings =>
						{
							settings.Timeout = 4;
							settings.ShowProgressBar = true;
							settings.PauseProgressOnHover = true;
						});
				}

				// Note:  تعداد یا مقدار واگذاری کالا به درخواست  دهنده 
				if(Item.NumberofProductDelivery == null)
				{
					IsValid = false;
					toastService.ShowError("لطفا تعداد یا مقدار واگذاری کالا به درخواست  دهنده را تکمیل نمایید.",
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
			// تبدیل تاریخ و تکمیل فیلد
			for (int i = 0; i < _Entity.SCMPETCO_ProductRequestDetails.Count; i++)
			{
				var item = _Entity.SCMPETCO_ProductRequestDetails.ToList()[i];
				if (item.GetDeliveryCode != null)
				{
					// تبدیل تاریخ شمسی به میلادی            
					System.Globalization.PersianCalendar PC = new System.Globalization.PersianCalendar();

					var DateNow = DateTime.Now;
					// تاریخ شمسی پر می شود
					item.DateTimeDeliveryCode = PC.GetYear(DateNow) + "/" + PC.GetMonth(DateNow).ToString("0#") + "/" + PC.GetDayOfMonth(DateNow).ToString("0#");
				}
			}
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


		public async Task<bool> SCMPETCO_ProductRequestDetails_editmodelsaving(object e)
		{
			bool IsValid = false;
			var MainModel = (Entity.SCMPETCO_ProductRequestDetails)e;
				// Note: تعداد یا مقدار واگذاری کالا
				if (MainModel.NumberofProductDelivery == null)
				{
					IsValid = true;
					toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا را تکمیل نمایید",
						settings =>
						{
							settings.Timeout = 4;
							settings.ShowProgressBar = true;
							settings.PauseProgressOnHover = true;
						});
				}

			return IsValid;
		}

		public async Task SCMPETCO_ProductRequestDetails_customizeeditmodel(GridCustomizeEditModelEventArgs e)
		{

		}

		public async Task SCMPETCO_ProductRequestDetails_afterrendermodal(Entity.SCMPETCO_ProductRequestDetails Item)
		{
	
		}

		public async Task DeliveryCode_NotMapped_oninput(ChangeEventArgs Selected)
		{
			// تکمیل فیلد کد تحویل کالا در همه ردیف های گرید
			for (int i = 0; i < _Entity.SCMPETCO_ProductRequestDetails.Count; i++)
			{
				var item = _Entity.SCMPETCO_ProductRequestDetails.ToList()[i];
				item.GetDeliveryCode = int.Parse(Selected.Value.ToString());
			}
			StateHasChanged();
		}

		#endregion FunctionEvents

	}
}
