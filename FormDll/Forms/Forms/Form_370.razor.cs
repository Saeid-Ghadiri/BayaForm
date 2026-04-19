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
	public class Form_370Base : Form_370Peropeties
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

		public async Task ProductName_NotMapped_onitemselected(dynamic Selected, Entity.SCMNFP_ProductRequestDetails Item)
		{
			/// <summary>
			/// فیلدهای زیر فیلدهای اصلی برای نمایش در فرم هستند.
			///
			///</summary>

			//  نام کالا
			Item.ProductName = Selected.DESC;

			// کد کالا
			Item.ProductCode = Selected.PARTNO;

			// واحد کالا
			Item.ProductUnit = Selected.UNIT;

			//  نام دسته بندی فرعی
			Item.ProductSubCategory = Selected.SubGroupName;

			// شناسه دسته بندی فرعی
			Item.ProductSubCategoryId = Selected.SubGroupName;

			//دسته بندی اصلی کالا
			Item.ProductMainCategory = Selected.GroupName;

			// شناسه دسته بندی اصلی کالا
			Item.ProductMainCategoryId = Selected.GRCODE;

			// سال مالی شماران
			Item.ShomaranFiscalYear = Selected.YEAR;

			//کالا موجود است یا خیر
			Item.IsExistProduct = Selected.IsExist;

			Console.WriteLine("Selected.PARTCODE : " + Selected.PARTCODE);
			if(Selected.PARTNO.ToString().StartsWith("1"))
			{
					Item.SCMNFP_SCMNFP_AnbarId = Guid.Parse("4CFA2CDA-E60B-F111-A50F-005056A2B6BD");
			}
			if(Selected.PARTNO.ToString().StartsWith("2"))
			{
					Item.SCMNFP_SCMNFP_AnbarId = Guid.Parse("4820B5CF-E60B-F111-A50F-005056A2B6BD");
			}
			if(Selected.PARTNO.ToString().StartsWith("3"))
			{
					Item.SCMNFP_SCMNFP_AnbarId = Guid.Parse("4CFA2CDA-E60B-F111-A50F-005056A2B6BD");
			}
			if(Selected.PARTNO.ToString().StartsWith("4"))
			{
				Console.WriteLine("Selected.PARTCODE : 4");
					Item.SCMNFP_SCMNFP_AnbarId = Guid.Parse("4820B5CF-E60B-F111-A50F-005056A2B6BD");
			}
			if(Selected.PARTNO.ToString().StartsWith("5"))
			{
					Item.SCMNFP_SCMNFP_AnbarId = Guid.Parse("4820B5CF-E60B-F111-A50F-005056A2B6BD");
			}
			if(Selected.PARTNO.ToString().StartsWith("6"))
			{
					Item.SCMNFP_SCMNFP_AnbarId = Guid.Parse("4820B5CF-E60B-F111-A50F-005056A2B6BD");
			}
			if(Selected.PARTNO.ToString().StartsWith("7"))
			{
					Item.SCMNFP_SCMNFP_AnbarId = Guid.Parse("6BF327B6-E50B-F111-A50F-005056A2B6BD");
			}
			if(Selected.PARTNO.ToString().StartsWith("8"))
			{
					Item.SCMNFP_SCMNFP_AnbarId = Guid.Parse("6BF327B6-E50B-F111-A50F-005056A2B6BD");
			}
			 
			// موجودی کالا در شماران
			if (Selected.Amount > -1)
			{
				Item.ProductInventory = (double)Selected.Amount;
			}
			
			// Console.WriteLine("Selected.PARTCODE : " + Selected.PARTCODE);

			// Item.PARTCODE = Selected.PARTCODE;
		}

		public async Task<bool> GridSCMNFP_ProductRequestId_228_editmodelsaving(object e)
		{
			bool IsCancelled = false;

			var Item = (Entity.SCMNFP_ProductRequestDetails)e;

			IsCancelled = !await CheckFieldValidation(Item);

			return IsCancelled;

		}

		public async Task<bool> CheckFieldValidation(Entity.SCMNFP_ProductRequestDetails Item)
		{
			bool IsValid = true;

			var List = _Entity.SCMNFP_ProductRequestDetails.ToList();

			// - نام کالا
			if (Item.SCMNFP_SCMNFP_AnbarId == null)
			{
				IsValid = false;

				toastService.ShowError("لطفا گزینه انبار را تکمیل نمایید",
					settings =>
					{
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}

			return IsValid;
		}

		#endregion FunctionEvents

	}
}