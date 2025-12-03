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
	public class Form_287Base : Form_287Peropeties
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

			}
		}

		/// <summary>
		/// اعتبار سنجی فرم
		/// </summary>
		/// <returns></returns>
		public override async Task<bool> FormValidator()
		{
			bool IsValid = true;

			// if (_Entity.ReqCount != 5)
			// {
			//     IsValid = false;
			//     SumaryMessage += "تعداد درخواست مخالف 5 باشد";
			// }

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

		public async Task<bool> SCM_ProductRequestDetails_editmodelsaving(object e)
		{
			bool IsValid = false;

			var MainModel = (Entity.SCM_ProductRequestDetails)e;

			// Note: شرط پُر بودن فیلد تعداد یا مقدار درخواستی
			if (MainModel.ProductRequestingQTY == null || MainModel.ProductRequestingQTY == 0)
			{
				IsValid = true;
				toastService.ShowError("لطفا گزینه تعداد یا مقدار درخواستی را تکمیل نمایید.");
			}

			// Note: شرط پُر بودن فیلد اولویت
			if (MainModel.SCM_PriorityId == null)
			{
				IsValid = true;
				toastService.ShowError("لطفا گزینه اولویت را تکمیل نمایید.");
			}

			// Note: آیا فرآیند تحویل اجرا گردد؟
			if (MainModel.ProductDelivery == null)
			{
				IsValid = true;
				toastService.ShowError("لطفا گزینه آیا فرآیند تحویل اجرا گردد؟ را تکمیل نمایید.");
			}

			// Note: آیا تامین کسری انجام شود؟
			if (MainModel.FutureActionTrueFalse == null)
			{
				IsValid = true;
				toastService.ShowError("لطفا گزینه آیا تامین کسری انجام شود؟ تکمیل نمایید.");
			}

			// Note: تعداد تامین کسری
			if (MainModel.DeficitSupplyNumber == null || MainModel.DeficitSupplyNumber == 0)
			{
				IsValid = true;
				toastService.ShowError("لطفا گزینه تعداد تامین کسری تکمیل نمایید.");
			}
			return IsValid;
		}
		public async Task SCM_ProductRequestDetails_customizeeditmodel(GridCustomizeEditModelEventArgs e)
		{

		}
		public async Task SCM_ProductRequestDetails_afterrendermodal(Entity.SCM_ProductRequestDetails Item)
		{
			// Note: مخفی کردن فیلد تعداد تامین کسری بر اساس فیلد آیا تامین کسری دارد؟
			if (Item.FutureActionTrueFalse.HasValue && Item.FutureActionTrueFalse.Value)
			{
				Ref_SCM_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true);
			}
			else
			{
				Ref_SCM_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);
			}

			// Note: مخفی کردن فیلد بارگذاری فایل تی دی اس، بر اساس فیلد آیا دیتاشیت دارد یا خیر؟ 
			if (Item.ProductDataSheetTrueFalse.HasValue && Item.ProductDataSheetTrueFalse.Value)
			{
				Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductDataSheetFile.SetVisible(true);
			}
			else
			{
				Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductDataSheetFile.SetVisible(false);
			}
		}

		public async Task ProductDataSheetTrueFalse_oninput(ChangeEventArgs Selected, Entity.SCM_ProductRequestDetails Item)
		{
			// Note: مخفی کردن فیلد بارگذاری فایل تی دی اس، بر اساس فیلد آیا دیتاشیت دارد یا خیر؟

			var Item1 = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_ProductDataSheetFile;

			if (Selected.Value.ToString() == "true")
			// if(Item.FutureActionTrueFalse.Value == null || !Item.ProductDataSheetTrueFalse.Value)
			{
				Item1.SetVisible(true);
			}
			else
			{
				Item1.SetVisible(false);
			}
		}
		public async Task FutureActionTrueFalse_oninput(ChangeEventArgs Selected, Entity.SCM_ProductRequestDetails Item)
		{
			// Note: مخفی کردن فیلد تعداد تامین کسری بر اساس فیلد آیا تامین کسری دارد؟       
			var Item2 = Ref_SCM_ProductRequestDetails_DeficitSupplyNumber;

			if (Selected.Value.ToString() == "true")
			{
				Item2.SetVisible(true);
			}
			else
			{
				Item2.SetVisible(false);
			}
		}

		#endregion FunctionEvents

	}
}
