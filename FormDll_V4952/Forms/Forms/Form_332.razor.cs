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
	public class Form_332Base : Form_332Peropeties
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
				// var List = _Entity.SCMICT_ProductRequestDetails.ToList();
				// foreach (var Item in List)
				// {

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

                // T_FACTNO
                if (_Entity.T_FACTNO == null)
				{
					IsValid = false;
					toastService.ShowError("در بخش ثبت اطلاعات عطف: گزینه شماره درخواست تحویل کالا شماران را تکمیل نمایید.",
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
			_Entity.T_TempNoNum = Selected.TempNoNum;
			_Entity.T_PAYCENTName = Selected.PAYCENTName;
			_Entity.T_TEMPNO = Selected.TEMPNO;
			_Entity.T_CREATOR = Selected.CREATOR;
			_Entity.T_Year = Selected.YEAR;
			_Entity.T_CENTCODE_GUID = Selected.CENTCODE_GUID;
			_Entity.T_CENTCODE = Selected.CENTCODE;
			_Entity.T_FACTDATE = Selected.FACTDATE;
			_Entity.T_FACTNO = Selected.FACTNO;
			_Entity.T_FACTNO_GUID = Selected.FACTNO_GUID;

			// فراخوانی داده از dropdown TempNoNum برای گرید داده های آن
			Ref_T_HavaleDTL.LoadData();			
		}

		public async Task T_FACTNO_GUID_oninput(ChangeEventArgs Selected   )
        {       
			if (_Entity.T_FACTNO_GUID == null)
			{
				//IsValid = true;
				toastService.ShowError("لطفا گزینه شناسه شماره درخواست تحویل کالا شماران را تکمیل نمایید",
					settings =>
					{
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}
        }



		// public async Task HavaleMasrafIsEnable(bool Visible)
        // {
        //     Ref_SCMPETCO_ProductRequestDetails_FACTNO.SetVisible(Visible);
        // }

		// await HavaleMasraf(false);

		// حواله مصرف
		// a5b1bc7b-8bb7-ef11-a4fa-005056a2b6bd

		// خرید کالا 
		// 09cf6986-8bb7-ef11-a4fa-005056a2b6bd
	
		// رسید انبار
		// 0acf6986-8bb7-ef11-a4fa-005056a2b6bd

		#endregion FunctionEvents

	}
}
