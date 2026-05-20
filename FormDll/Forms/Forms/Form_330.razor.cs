using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;


namespace Forms.Forms
{
	public class Form_330Base : Form_330Peropeties
	{




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
				var List = _Entity.SCMPETCO_ProductRequestDetails.ToList();
				foreach (var Item in List)
				{
					// // آیا تامین کسری انجام شود؟
					// Item.FutureAction = null;
					// // آیا فرآیند تحویل اجرا گردد؟
					// Item.ProductDelivery = null;

					// نحوه تامین کالا
					Item.ForeignMachineryProduct=null;
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


			var ListEntity = _Entity.SCMPETCO_ProductRequestDetails;
			foreach (var item in ListEntity)
			{
				// Note: شرط پُر بودن فیلد اولویت
				if (item.SCMPETCO_PriorityId == null)
				{
					IsValid = false;
					toastService.ShowError("لطفا گزینه اولویت را تکمیل نمایید.",
							settings =>
							{
								settings.Timeout = 4;
								settings.ShowProgressBar = true;
								settings.PauseProgressOnHover = true;
							});
				}

				// Note: شرط پُر بودن فیلد محل مصرف
				if (item.PlaceOfUseProduct == null)
				{
					IsValid = false;
					toastService.ShowError("لطفا گزینه محل مصرف تکمیل نمایید.",
							settings =>
							{
								settings.Timeout = 4;
								settings.ShowProgressBar = true;
								settings.PauseProgressOnHover = true;
							});

				}
				

				// Note: نحوه تامین کالا
				if (item.ForeignMachineryProduct == null)
				{
					IsValid = false;
					toastService.ShowError("لطفا گزینه نحوه تامین را تکمیل نمایید.",
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

		public async Task<bool> SCMPETCO_ProductRequestDetails_editmodelsaving(object e)
		{
			bool IsValid = false;

			var MainModel = (Entity.SCMPETCO_ProductRequestDetails)e;

			// Note: شرط پُر بودن فیلد نام کالا دیزیبل
			// if (MainModel.ProductNameText == null)
			// {
			// 	IsValid = true;
			// 	toastService.ShowError("لطفا گزینه نام کالا را تکمیل نمایید",
			// 			settings =>
			// 			{
			// 				settings.Timeout = 4;
			// 				settings.ShowProgressBar = true;
			// 				settings.PauseProgressOnHover = true;
			// 			});
			// }

			// Note: شرط پُر بودن فیلد تعداد یا مقدار درخواستی
			if (MainModel.ProductRequestingQTY == null || MainModel.ProductRequestingQTY == 0)
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

			// Note: شرط پُر بودن فیلد اولویت
			if (MainModel.SCMPETCO_PriorityId == null)
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

			// Note: شرط پُر بودن فیلد محل مصرف
			if (MainModel.PlaceOfUseProduct == null)
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

			// Note: نحوه تامین کالا
			// if (MainModel.ForeignMachineryProduct == null)
			// {
			// 	IsValid = true;
			// 	toastService.ShowError("لطفا گزینه نحوه تامین را تکمیل نمایید.",
			// 			settings =>
			// 			{
			// 				settings.Timeout = 4;
			// 				settings.ShowProgressBar = true;
			// 				settings.PauseProgressOnHover = true;
			// 			});
			// }
			return IsValid;
		}


		#endregion FunctionEvents

	}
}