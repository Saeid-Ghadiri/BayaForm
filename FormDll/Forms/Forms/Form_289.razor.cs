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
	public class Form_289Base : Form_289Peropeties
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

			var List = _Entity.SCM_ProductRequestDetails.Where(p=>p.IsDelete == false).ToList();

			// Console.WriteLine("#Log FormValidator :");

			// SequenceFlow_0s3d624 - دکمه ثبت و ادامه
            if (BtnWorkFlowId == "SequenceFlow_0s3d624" || // ProcessID = 42
				BtnWorkFlowId == "SequenceFlow_0y7ztkw" || // ProcessID = 44
                BtnWorkFlowId == "SequenceFlow_1hvt2uw" || // ProcessID = 906
                BtnWorkFlowId == "SequenceFlow_1hvt2uw" || // ProcessID = 905
                BtnWorkFlowId == "SequenceFlow_0ocu9hq" || // ProcessID = 902
                BtnWorkFlowId == "SequenceFlow_1jl2bls" || // ProcessID = 901
                BtnWorkFlowId == "SequenceFlow_18h9obk" || // ProcessID = 900
                BtnWorkFlowId == "SequenceFlow_1hvt2uw" || // ProcessID = 898
                BtnWorkFlowId == "SequenceFlow_1j26xpn" || // ProcessID = 915       
			 	BtnWorkFlowId == "SequenceFlow_0t198fp")  // ProcessID = 922
			
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
        public async Task<bool> CheckFieldValidation(Entity.SCM_ProductRequestDetails Item)
        {
            bool IsValid = true;

            var List = _Entity.SCM_ProductRequestDetails.ToList();

			if (!Item.T_FACTNO_GUID.HasValue)
			{
			    IsValid = false;

			    toastService.ShowError("لطفا گزینه جستجوی عطف را تکمیل نمایید.",
			        settings =>
			        {
			            settings.Timeout = 4;
			            settings.ShowProgressBar = true;
			            settings.PauseProgressOnHover = true;
			        });
			}

			return IsValid;
		}

		
		// عطف تحویل کالا - حواله مصرف شماران سیستم
		public async Task HavaleMasrafIsVisible(bool Visible)
		{
		    //Console.WriteLine("#Log 3-dropdown4" + " 1 ");
		    await Task.Delay(1000);
		    //Console.WriteLine(Visible.ToString());
		    try
		    {
		        //Console.WriteLine("#Log Ref" + (Ref_SCM_ProductRequestDetails_T_Search_NotMapped == null).ToString());
		        Ref_SCM_ProductRequestDetails_T_Search_NotMapped.SetVisible(Visible);
		    }
		    catch (Exception ex)
		    {
		        //Console.WriteLine(ex.Message);
		        //Console.WriteLine(await Utility.JSON.ToJson(ex));
		    }
		    Ref_SCM_ProductRequestDetails_T_Search_NotMapped.SetVisible(Visible);
		    Ref_SCM_ProductRequestDetails_T_CENTCODE.SetVisible(Visible);
		    Ref_SCM_ProductRequestDetails_T_CENTCODE_GUID.SetVisible(Visible);
		    Ref_SCM_ProductRequestDetails_T_PAYCENTName.SetVisible(Visible);
		    Ref_SCM_ProductRequestDetails_T_CREATOR.SetVisible(Visible);
		    Ref_SCM_ProductRequestDetails_T_FACTDATE.SetVisible(Visible);
		    Ref_SCM_ProductRequestDetails_T_FACTNO.SetVisible(Visible);
		    Ref_SCM_ProductRequestDetails_T_FACTNO_GUID.SetVisible(Visible);
		    Ref_SCM_ProductRequestDetails_T_TEMPNO.SetVisible(Visible);
		    Ref_SCM_ProductRequestDetails_T_TempNoNum.SetVisible(Visible);
		    Ref_SCM_ProductRequestDetails_T_YEAR.SetVisible(Visible);
		    // جزئیات تحویل کالا
		    Ref_SCM_ProductRequestDetails_SH_Tahvil_DTL.SetVisible(Visible);
		
		    Ref_SCM_ProductRequestDetails_T_CENTCODE.SetDisabled(true);
		    Ref_SCM_ProductRequestDetails_T_CENTCODE_GUID.SetDisabled(true);
		    Ref_SCM_ProductRequestDetails_T_PAYCENTName.SetDisabled(true);
		    Ref_SCM_ProductRequestDetails_T_CREATOR.SetDisabled(true);
		    Ref_SCM_ProductRequestDetails_T_FACTDATE.SetDisabled(true);
		    Ref_SCM_ProductRequestDetails_T_FACTNO.SetDisabled(true);
		    Ref_SCM_ProductRequestDetails_T_FACTNO_GUID.SetDisabled(true);
		    Ref_SCM_ProductRequestDetails_T_TEMPNO.SetDisabled(true);
		    Ref_SCM_ProductRequestDetails_T_TempNoNum.SetDisabled(true);
		    Ref_SCM_ProductRequestDetails_T_YEAR.SetDisabled(true);
		}
		
		public async Task HavaleMasrafIsNull(Entity.SCM_ProductRequestDetails Item)
		{
		    Item.T_CENTCODE = null;
		    Item.T_CENTCODE_GUID = null;
		    Item.T_PAYCENTName = null;
		    Item.T_CREATOR = null;
		    Item.T_FACTDATE = null;
		    Item.T_FACTNO = null;
		    Item.T_FACTNO_GUID = null;
		    Item.T_TEMPNO = null;
		    Item.T_TempNoNum = null;
		    Item.T_YEAR = null;
		    Item.SH_Tahvil_DTL = null;
		}
		
		public async Task HavaleMasrafSetShomaran(dynamic Selected, Entity.SCM_ProductRequestDetails Item)
		{
		    //Console.WriteLine("#Log 2");
		    await Task.Delay(500);
		    Item.T_TempNoNum = Selected.TempNoNum;
			Console.WriteLine("#Log 0");
		    Item.T_PAYCENTName = Selected.PAYCENTName;
		    Item.T_TEMPNO = Selected.TEMPNO;
		    Item.T_CREATOR = Selected.CREATOR;
		    Item.T_YEAR = Selected.YEAR;
			Console.WriteLine("#Log 1");
		    Item.T_CENTCODE_GUID = Selected.CENTCODE_GUID;
		    Item.T_CENTCODE = Selected.CENTCODE;
		    Item.T_FACTDATE = Selected.FACTDATE;
			Console.WriteLine("#Log 2");
		    Item.T_FACTNO = Selected.FACTNO;
			Console.WriteLine("#Log 3");
		    Item.T_FACTNO_GUID = Selected.FACTNO_GUID;
			Console.WriteLine("#Log 4");
		    // فراخوانی داده از dropdown TempNoNum برای گرید داده های آن
		    Ref_SCM_ProductRequestDetails_SH_Tahvil_DTL?.SetEntity(Item);
		    await Task.Delay(100);
		    Ref_SCM_ProductRequestDetails_SH_Tahvil_DTL?.LoadData();
		}

		public async Task <bool> GridSCM_ProductRequestId_editmodelsaving(object e   )
        {
			bool IsCancelled = false;
			
			var Item = (Entity.SCM_ProductRequestDetails)e;

			IsCancelled = !await CheckFieldValidation(Item);

			return IsCancelled;
        }
		
		public async Task  GridSCM_ProductRequestId_afterrendermodal(Entity.SCM_ProductRequestDetails Item   )
        {
			// داده های جزئیات بخش حواله مصرف
            if (Item.T_FACTNO_GUID.HasValue)
			{
			    Ref_SCM_ProductRequestDetails_SH_Tahvil_DTL.SetEntity(Item);
			    Ref_SCM_ProductRequestDetails_SH_Tahvil_DTL.LoadData();
			}
        }

		public async Task  T_Search_NotMapped_onitemselected(dynamic Selected ,Entity.SCM_ProductRequestDetails Item  )
        {
			await HavaleMasrafSetShomaran(Selected, Item);
        }

		public async Task  TempNoNum_NotMapped_onitemselected(dynamic Selected   )
        {
			var List = _Entity.SCM_ProductRequestDetails.ToList();

			foreach (var Item in List)
			{
    			await HavaleMasrafSetShomaran(Selected, Item);
 			} 
			
			_Entity.FACTNO_GUID = Selected.FACTNO_GUID;
			Ref_T_HavaleDTL.SetEntity(_Entity); 
			Ref_T_HavaleDTL.LoadData();
			
			StateHasChanged();
		}

		#endregion FunctionEvents

	}
}
