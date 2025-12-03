using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using System.Globalization;
using System.Threading.Tasks;
using BlazorBootstrap;
using Blazored.Toast.Services;


namespace Forms.Forms
{
	//public class Form_289Base : Form_289Peropeties
	public class Form_967Base : Form_967Peropeties
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

			var List = _Entity.SCM_ProductRequestDetails.ToList();

			foreach (var Item in List)
			{
				if (string.IsNullOrEmpty(Item.T_EeNo))
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
			}

			// لغو کلی درخواست
            if (BtnWorkFlowId == "SequenceFlow_19qfb96") //903
            {
                // Console.WriteLine("#Log:: 0");

                string htmlString = 
                    "<div>" +
                        "<picture>" +
                            "<img src='https://File.workcv.ir/fa/api/v1/File/Get?FileID=6e5b6fb8-a5b2-490c-f83f-08dbea5b8061' class='' alt='لوگو پل فیلم' width='96px'>" +
                        "</picture>" +
                        "<hr class='hrdash border-success-subtle'>" +
                    "</div>" +
                    "<div class='fw-bold text-right'>" + 
                        "<div class='fs-6'>کاربر لغو کننده درخواست: "  + "</div>" +
                        "<div class='fs-6'>"  + "</div>" +
                        "<div class='fs-6'>دلیل لغو این درخواست:</div>" +
                        "<textarea id='InConfirmCancelationText' name='InConfirmCancelationText' " +
                            "style='padding: 8px; border: 1px solid #ddd; " +
                            "border-radius: 5px; resize: none; display: block; margin: 5px 0' " +
                            "width: 100%; height: 150px;' " +
                            "placeholder='دلیل لغو درخواست را وارد کنید...' " +
                            "oninput='document.getElementById(\"ConfirmCancelationText\").value=this.value'>" +
                        "</textarea>" +
                        "<div></div>" +
                        "<div class='fs-6 text-secondary text-right'>" +
                        "<i class='fal fa-exclamation-triangle px-2' style='font-size:24px; color:red'></i>" +
                            "کاربر محترم در نظر داشته باشید اطلاعات ثبت در این ناحیه در حال حاضر صرفاً در گزارش‌ها واحد سیستم‌ها و روش‌ها قابل نمایش است، همچنین کاربر درخواست‌دهنده از بخش درخواست‌های من می‌تواند وضعیت درخواست خود را بررسی نماید." +
                        "</div>" +
                    "</div>";

                var options = new ConfirmDialogOptions
                {
                    YesButtonText = "لغو درخواست",
                    YesButtonColor = ButtonColor.Success,
                    NoButtonText = "انصراف",
                    NoButtonColor = ButtonColor.Danger
                };

                var confirmation = await Confirm.ShowAsync(
                    title: "",
                    message1: htmlString,
                    confirmDialogOptions: options);

                if (!confirmation)
                {
                     IsValid = false;
                }
                else
                {
                    // در بخش Razor فیلد input hidden همین فیلد وجود دارد و از طریق آن داده به فیلد اصلی داده می شود.
                    string Value = await JS.InvokeAsync<string>("eval", "document.getElementById('ConfirmCancelationText')?.value || ''");
                    //_Entity.CancellationReason=Value;
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

		// فیلد ارتباط با شماران
		// Id                                       عنوان
		// **************************************************
		// a5b1bc7b-8bb7-ef11-a4fa-005056a2b6bd     حواله مصرف
		// 09cf6986-8bb7-ef11-a4fa-005056a2b6bd     خرید کالا  
		// 0acf6986-8bb7-ef11-a4fa-005056a2b6bd     رسید انبار
		// f79ea98c-b6ee-ef11-a4fb-005056a2b6bd     حواله مصرف و رسید انبار

		// عطف تحویل کالا - حواله مصرف شماران سیستم
		public async Task HavaleMasrafIsVisible(bool Visible)
		{
			//Console.WriteLine("#Log 3-dropdown4" + " 1 ");
			await Task.Delay(1000);
			//Console.WriteLine(Visible.ToString());
			try
			{
				//Console.WriteLine("#Log Ref" + (Ref_SCM_ProductRequestDetails_T_Search_NotMapped2 == null).ToString());
				Ref_SCM_ProductRequestDetails_T_Transfer_Search_NotMapped.SetVisible(Visible);
			}
			catch (Exception ex)
			{
				//Console.WriteLine(ex.Message);
				//Console.WriteLine(await Utility.JSON.ToJson(ex));
			}
			Ref_SCM_ProductRequestDetails_T_Transfer_Search_NotMapped.SetVisible(Visible);
			//Ref_SCM_ProductRequestDetails_T_CENTCODE.SetVisible(Visible);
			//Ref_SCM_ProductRequestDetails_T_CENTCODE_GUID.SetVisible(Visible);
			//Ref_SCM_ProductRequestDetails_T_PAYCENTName.SetVisible(Visible);
			//Ref_SCM_ProductRequestDetails_T_CREATOR.SetVisible(Visible);
			//Ref_SCM_ProductRequestDetails_T_FACTDATE.SetVisible(Visible);
			//Ref_SCM_ProductRequestDetails_T_FACTNO.SetVisible(Visible);

			Ref_SCM_ProductRequestDetails_T_EeDate.SetVisible(Visible);
			Ref_SCM_ProductRequestDetails_T_EeNo.SetVisible(Visible);
			//Ref_SCM_ProductRequestDetails_T_FACTNO_GUID.SetVisible(Visible);
			Ref_SCM_ProductRequestDetails_T_Transfer_TEMPNO.SetVisible(Visible);
			//Ref_SCM_ProductRequestDetails_T_Transfer_TEMPNONum.SetVisible(Visible);
			Ref_SCM_ProductRequestDetails_T_YEAR.SetVisible(Visible);
			// جزئیات تحویل کالا
			Ref_SCM_ProductRequestDetails_SH_Transfer_Tahvil_DTL.SetVisible(Visible);

			//Ref_SCM_ProductRequestDetails_T_CENTCODE.SetDisabled(true);
			//Ref_SCM_ProductRequestDetails_T_CENTCODE_GUID.SetDisabled(true);
			//Ref_SCM_ProductRequestDetails_T_PAYCENTName.SetDisabled(true);
			//Ref_SCM_ProductRequestDetails_T_CREATOR.SetDisabled(true);
			//Ref_SCM_ProductRequestDetails_T_FACTDATE.SetDisabled(true);
			//Ref_SCM_ProductRequestDetails_T_FACTNO.SetDisabled(true);

			Ref_SCM_ProductRequestDetails_T_EeDate.SetDisabled(true);
			Ref_SCM_ProductRequestDetails_T_EeNo.SetDisabled(true);
			//Ref_SCM_ProductRequestDetails_T_FACTNO_GUID.SetDisabled(true);
			Ref_SCM_ProductRequestDetails_T_Transfer_TEMPNO.SetDisabled(true);
			//Ref_SCM_ProductRequestDetails_T_Transfer_TEMPNONum.SetDisabled(true);
			Ref_SCM_ProductRequestDetails_T_YEAR.SetDisabled(true);
		}

		public async Task HavaleMasrafIsNull(Entity.SCM_ProductRequestDetails Item)
		{
			Item.T_CENTCODE = null;
			Item.T_CENTCODE_GUID = null;
			Item.T_PAYCENTName = null;
			Item.T_CREATOR = null;
			//Item.T_FACTDATE = null;
			Item.T_EeDate = null;
			//Item.T_FACTNO = null;
			Item.T_EeNo = null;
			//Item.T_FACTNO_GUID = null;
			Item.T_Transfer_TEMPNO = null;
			//Item.T_Transfer_TEMPNONum = null;
			Item.T_YEAR = null;
			Item.SH_Transfer_Tahvil_DTL = null;
		}

		


		public async Task<bool> GridSCM_ProductRequestId_editmodelsaving(object e)
		{
			bool IsCancelled = false;

			var Item = (Entity.SCM_ProductRequestDetails)e;

			if (string.IsNullOrEmpty(Item.T_EeNo))
			{
				IsCancelled = true;

				toastService.ShowError("لطفا گزینه جستجوی عطف را تکمیل نمایید.",
					settings =>
					{
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}

			return IsCancelled;
		}
		
		public async Task GridSCM_ProductRequestId_afterrendermodal(Entity.SCM_ProductRequestDetails Item)
		{
			// داده های جزئیات بخش حواله مصرف
			if (!string.IsNullOrEmpty(Item.T_EeNo))
			{
				Ref_SCM_ProductRequestDetails_SH_Transfer_Tahvil_DTL.SetEntity(Item);
				Ref_SCM_ProductRequestDetails_SH_Transfer_Tahvil_DTL.LoadData();
			}
		}

		public async Task T_Search_NotMapped2_onitemselected(dynamic Selected, Entity.SCM_ProductRequestDetails Item)
		{
			await HavaleMasraf_Transfer_SetShomaran(Selected, Item);
		}


		public async Task AnbarMavad_TempNoNum_NotMapped_onitemselected(dynamic Selected)
		{
			// var List = _Entity.SCM_ProductRequestDetails.ToList();

			// foreach (var item in List)
			// {
			// 	await HavaleMasraf_Transfer_SetShomaran(Selected, item);
			// }

			// _Entity.FACTNO_GUID = Selected.FACTNO_GUID;

			// Ref_T_HavaleDTL.SetEntity(_Entity);

			// Ref_T_HavaleDTL.LoadData();

			// StateHasChanged();

			
		}

	



		

		public async Task  AnbarMavad_Transfer_TempNoNum_NotMapped_onitemselected(dynamic Selected   )
        {
			Console.WriteLine("#Log 0");
			Console.WriteLine("#0factno :" + Selected.factno);
			var List = _Entity.SCM_ProductRequestDetails.ToList();

			foreach (var item in List)
			{
				await HavaleMasraf_Transfer_SetShomaran(Selected, item); // باید تغییر کند
			}

			//_Entity.FACTNO_GUID = Selected.FACTNO_GUID; 
			_Entity.FACTNO_Transfer = Selected.factno; // افزودن یک فیلد جدید؟ یا استفاده از همین فیلد
			
			Ref_T_TransferDtl.SetEntity(_Entity);

			Ref_T_TransferDtl.LoadData();

			StateHasChanged();
            
        }

		public async Task HavaleMasraf_Transfer_SetShomaran(dynamic Selected, Entity.SCM_ProductRequestDetails Item)
		{
			//Console.WriteLine("#Log 2");
			await Task.Delay(500);
			//Item.T_Transfer_TEMPNONum = Selected.tempno;
			Console.WriteLine("#Log 1");
			//Item.T_PAYCENTName = Selected.PAYCENTName;
			Item.T_Transfer_TEMPNO = Selected.TEMPNO;
			//Item.T_CREATOR = Selected.CREATOR;
			Console.WriteLine("#Log year" + Selected.year);
			Item.T_YEAR = Selected.year;
			//Item.T_CENTCODE_GUID = Selected.CENTCODE_GUID;
			//Item.T_CENTCODE = Selected.CENTCODE;
			Console.WriteLine("#Log 3");
			//Item.T_EeDate = Selected.eedate;
			Console.WriteLine("#factno :" + Selected.factno);
			Item.T_EeNo = Selected.factno;
			//Item.T_FACTNO_GUID = Selected.FACTNO_GUID;
			Console.WriteLine("#Log 4");
			// فراخوانی داده از dropdown TempNoNum برای گرید داده های آن
			Ref_SCM_ProductRequestDetails_SH_Transfer_Tahvil_DTL?.SetEntity(Item);
			await Task.Delay(100);
			Ref_SCM_ProductRequestDetails_SH_Transfer_Tahvil_DTL?.LoadData();
		}

		public async Task  T_Transfer_Search_NotMapped_onitemselected(dynamic Selected ,Entity.SCM_ProductRequestDetails Item  )
        {
			await HavaleMasraf_Transfer_SetShomaran(Selected, Item);
            
        }

		
		#endregion FunctionEvents

	}
}
