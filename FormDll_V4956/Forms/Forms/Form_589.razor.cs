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
	public class Form_589Base : Form_589Peropeties
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

			}
		}

		/// <summary>
		/// اعتبار سنجی فرم
		/// </summary>
		/// <returns></returns>
		public override async Task<bool> FormValidator()
		{
			bool IsValid = true;


			// Console.WriteLine("#Log FormValidator 1");
			// Console.WriteLine(_Entity.TempNoNum);
			// Console.WriteLine("_" + (_Entity.TempNoNum == null));
			// Console.WriteLine("_" + _Entity.TempNoNum + "_");

			// //todo حذف شود
			// if	(_Entity.SystemUser.Contains("سعید غدیری"))
			// {
			// 	toastService.ShowError("این متن تست است",
			// 	settings => {
			// 		settings.Timeout = 4;
			// 		settings.ShowProgressBar = true;
			// 		settings.PauseProgressOnHover = true;
			// 	});
			// 	IsValid = false;
			// }



			// فیلد: شماره عطف تحویل کالا شماران
			if( _Entity.TempNoNum == null)
            {
            	IsValid = false;

				toastService.ShowError("لطفا گزینه شماره عطف تحویل کالا شماران از بخش ثبت عطف را تکمیل نمایید",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
            }
			
			// var List = _Entity.SCM_ProductRequestDetails.ToList();
			// foreach(var Item in List)
			// {
				
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

		public async Task TempNoNum_NotMapped_onitemselected(dynamic Selected)
		{
			_Entity.TempNoNum = Selected.TempNoNum;
			_Entity.PAYCENTName = Selected.PAYCENTName;
			_Entity.TEMPNO = Selected.TEMPNO;
			_Entity.CREATOR = Selected.CREATOR;
			_Entity.HavaleYear = Selected.YEAR;
			_Entity.CENTCODE_GUID = Selected.CENTCODE_GUID;
			_Entity.CENTCODE = Selected.CENTCODE;
			_Entity.FACTDATE = Selected.FACTDATE;
			_Entity.FACTNO = Selected.FACTNO;
			_Entity.FACTNO_GUID = Selected.FACTNO_GUID;

			 Ref_T_HavaleDTL.LoadData();
		}



		public async Task <bool> SCM_ProductRequestDetails_editmodelsaving(object e   )
        {

            return false;
        }
		
		public async Task  SCM_ProductRequestDetails_afterrendermodal(Entity.SCM_ProductRequestDetails Item   )
        {

            
        }

		#endregion FunctionEvents

	}
}
