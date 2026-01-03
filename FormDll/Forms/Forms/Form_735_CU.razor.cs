using ApiServer.External.Services;
using Baya.Models.ORM;
using Baya.Models.Utility;
using Blazored.Toast.Services;
using Castle.DynamicLinqQueryBuilder;
using DevExpress.Blazor;
using Entity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using Sitko.Blazor.CKEditor;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Text;
using Utility;

namespace Forms.Forms
{
	public class Form_735_CUBase : Form_735_CUPeropeties
	{
		// Toast  
		[Inject]
		public IToastService toastService { get; set; }

		#region [Form_775]
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

			if ((await BeforSubmit()).Status != HttpStatusCode.OK)
			{
				StateHasChanged();
				Result.Status = HttpStatusCode.InternalServerError;
				return Result;
			}

			_loadingService.Show();
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
			_loadingService.Hide();
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
		#endregion

		#region FunctionEvents

		#endregion FunctionEvents

	}
}

#region SCM_Uitility

public class SCM_Uitility
{
	// // عطف تحویل کالا - حواله مصرف شماران سیستم
	//     public static async Task HavaleMasrafIsVisible(bool Visible)
	//     {
	//         await Task.Delay(100);
	//         Ref_SCMPETCO_ProductRequestDetails_T_Search_NotMapped.SetVisible(Visible);

	//         Ref_SCMPETCO_ProductRequestDetails_T_CENTCODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_T_CENTCODE_GUID.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_T_PAYCENTName.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_T_CREATOR.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_T_FACTDATE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_T_FACTNO.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_T_FACTNO_GUID.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_T_TEMPNO.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_T_TempNoNum.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_T_YEAR.SetVisible(Visible);

	//         // جزئیات تحویل کالا
	//         Ref_SCMPETCO_ProductRequestDetails_SH_Tahvil_DTL.SetVisible(Visible);

	//         Ref_SCMPETCO_ProductRequestDetails_T_CENTCODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_T_CENTCODE_GUID.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_T_PAYCENTName.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_T_CREATOR.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_T_FACTDATE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_T_FACTNO.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_T_FACTNO_GUID.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_T_TEMPNO.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_T_TempNoNum.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_T_YEAR.SetDisabled(true);
	//     }

	//     public static async Task HavaleMasrafIsNull()
	//     {
	//         Ref_SCMPETCO_ProductRequestDetails_T_Search_NotMapped = null;
	//         Ref_SCMPETCO_ProductRequestDetails_T_CENTCODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_T_CENTCODE_GUID = null;
	//         Ref_SCMPETCO_ProductRequestDetails_T_PAYCENTName = null;
	//         Ref_SCMPETCO_ProductRequestDetails_T_CREATOR = null;
	//         Ref_SCMPETCO_ProductRequestDetails_T_FACTDATE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_T_FACTNO = null;
	//         Ref_SCMPETCO_ProductRequestDetails_T_FACTNO_GUID = null;
	//         Ref_SCMPETCO_ProductRequestDetails_T_TEMPNO = null;
	//         Ref_SCMPETCO_ProductRequestDetails_T_TempNoNum = null;
	//         Ref_SCMPETCO_ProductRequestDetails_T_YEAR = null;
	//         Ref_SCMPETCO_ProductRequestDetails_SH_Tahvil_DTL = null;
	//     }

	//     public static async Task HavaleMasrafSetShomaran(dynamic Selected, Entity.SCMPETCO_ProductRequestDetails Item)
	//     {
	//         //Console.WriteLine("#Log 2");
	//         await Task.Delay(100);
	//         Item.T_TempNoNum = Selected.TempNoNum;
	//         Item.T_PAYCENTName = Selected.PAYCENTName;
	//         Item.T_TEMPNO = Selected.TEMPNO;
	//         Item.T_CREATOR = Selected.CREATOR;
	//         Item.T_YEAR = Selected.YEAR;
	//         Item.T_CENTCODE_GUID = Selected.CENTCODE_GUID;
	//         Item.T_CENTCODE = Selected.CENTCODE;
	//         Item.T_FACTDATE = Selected.FACTDATE;
	//         Item.T_FACTNO = Selected.FACTNO;
	//         Item.T_FACTNO_GUID = Selected.FACTNO_GUID;
	//         //Console.WriteLine("#Log 2.2");
	//         // فراخوانی داده از dropdown TempNoNum برای گرید داده های آن
	//         Ref_SCMPETCO_ProductRequestDetails_SH_Tahvil_DTL.SetEntity(Item);
	//         //Console.WriteLine(await Utility.JSON.ToJson(Item));
	//         await Task.Delay(100);
	//         //Console.WriteLine("#Log 2.3");
	//         Ref_SCMPETCO_ProductRequestDetails_SH_Tahvil_DTL.LoadData();
	//     }

	//     // عطف خرید کالا در شماران سیستم
	//     public static async Task KharidIsVisible(bool Visible)
	//     {
	//         // Console.WriteLine("#Log 2");
	//         // Console.WriteLine("#Log 2.1" + Ref_SCMPETCO_ProductRequestDetails_KH_Search_NotMapped.Value);
	//         await Task.Delay(100);

	//         Ref_SCMPETCO_ProductRequestDetails_KH_Search_NotMapped.SetVisible(Visible);

	//         Ref_SCMPETCO_ProductRequestDetails_KH_APPROVER.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_CENTCODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_CENTCODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_CENTCODE_GUID.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_PAYCENTName.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_WUSER.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_TEMPNO.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_TempNoNum.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_ORDERNO.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_ORDERNO_GUID.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_ORDERDATE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_OKFACTDATE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_INVCODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_REQPERSON.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_YEAR.SetVisible(Visible);
	//         // جزئیات خرید کالا
	//         Ref_SCMPETCO_ProductRequestDetails_SH_Kharid_DTL.SetVisible(Visible);

	//         Ref_SCMPETCO_ProductRequestDetails_KH_APPROVER.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_CENTCODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_CENTCODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_CENTCODE_GUID.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_PAYCENTName.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_WUSER.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_TEMPNO.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_TempNoNum.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_ORDERNO.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_ORDERNO_GUID.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_ORDERDATE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_OKFACTDATE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_INVCODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_REQPERSON.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_KH_YEAR.SetDisabled(true);
	//     }

	//     public static async Task KharidIsNull()
	//     {
	//         Ref_SCMPETCO_ProductRequestDetails_KH_Search_NotMapped = null;
	//         Ref_SCMPETCO_ProductRequestDetails_KH_APPROVER = null;
	//         Ref_SCMPETCO_ProductRequestDetails_KH_CENTCODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_KH_CENTCODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_KH_CENTCODE_GUID = null;
	//         Ref_SCMPETCO_ProductRequestDetails_KH_PAYCENTName = null;
	//         Ref_SCMPETCO_ProductRequestDetails_KH_WUSER = null;
	//         Ref_SCMPETCO_ProductRequestDetails_KH_TEMPNO = null;
	//         Ref_SCMPETCO_ProductRequestDetails_KH_TempNoNum = null;
	//         Ref_SCMPETCO_ProductRequestDetails_KH_ORDERNO = null;
	//         Ref_SCMPETCO_ProductRequestDetails_KH_ORDERNO_GUID = null;
	//         Ref_SCMPETCO_ProductRequestDetails_KH_ORDERDATE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_KH_OKFACTDATE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_KH_INVCODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_KH_REQPERSON = null;
	//         Ref_SCMPETCO_ProductRequestDetails_KH_YEAR = null;
	//         Ref_SCMPETCO_ProductRequestDetails_SH_Kharid_DTL = null;
	//     }

	//     public static async Task KharidSetShomaran(dynamic Selected, Entity.SCMPETCO_ProductRequestDetails Item)
	//     {
	//         //Console.WriteLine("#Log 3");
	//         //Console.WriteLine(await Utility.JSON.ToJson(Selected));

	//         Item.KH_TEMPNO = Selected.TEMPNO;
	//         Item.KH_PAYCENTName = Selected.PAYCENTName;
	//         Item.KH_CENTCODE = Selected.CENTCODE;
	//         Item.KH_CENTCODE_GUID = Selected.CENTCODE_GUID;
	//         Item.KH_REQPERSON = Selected.REQPERSON;
	//         Item.KH_WUSER = Selected.WUSER;
	//         Item.KH_APPROVER = Selected.APPROVER;
	//         Item.KH_ORDERDATE = Selected.ORDERDATE;
	//         Item.KH_OKFACTDATE = Selected.OKFACTDATE;
	//         Item.KH_ORDERNO = Selected.ORDERNO;
	//         Item.KH_ORDERNO_GUID = Selected.ORDERNO_GUID;
	//         Item.KH_TempNoNum = Selected.TempNoNum;
	//         Item.KH_YEAR = Selected.YEAR;
	//         Item.KH_INVCODE = Selected.INVCODE;

	//         // فراخوانی داده از dropdown TempNoNum برای گرید داده های آن
	//         Ref_SCMPETCO_ProductRequestDetails_SH_Kharid_DTL.SetEntity(Item);
	//         Ref_SCMPETCO_ProductRequestDetails_SH_Kharid_DTL.LoadData();
	//     }

	//     // عطف رسید انبار در شماران سیستم 
	//     public static async Task ResidAnbarIsVisible(bool Visible)
	//     {
	//         // Console.WriteLine("#Log 3");
	//         // Console.WriteLine("#Log 3.1" + Ref_SCMPETCO_ProductRequestDetails_FB_Search_NotMapped.Value);
	//         await Task.Delay(200);

	//         Ref_SCMPETCO_ProductRequestDetails_FB_Search_NotMapped.SetVisible(Visible);

	//         Ref_SCMPETCO_ProductRequestDetails_FB_ACCCODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ACTCODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ACTYEAR.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ADD1.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ADD2.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ADD3.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ADD4.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_AMOUNT.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ARRNO.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ARRYEAR.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_BAARNAME.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_BABCODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_BRANCHCODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_BUYKIND.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C1901.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C1950.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CAR_TYPE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CASHCODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CASHKIND.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_COMP_NAME.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CPRICE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CREATOR.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO1.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO2.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO3.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO4.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO5.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO6.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO7.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO8.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO9.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO10.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO1.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO2.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO3.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO4.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO5.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO6.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO7.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO8.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO9.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO10.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CURFACT.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CURKIND.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CURPKID.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C_CURVAL.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C_DEDUCE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C_OTHERPAY.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C_PARPRICE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C_PREPRICE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C_WELLDONE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DEDUCE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DOC_ID.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DOC_ID2.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DOC_YEAR.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DOC_YEAR2.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DRV_CARTNO.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DRV_NAME.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ELAMNO.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FACTDATE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FACTNO.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FACTOR.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FACTPOS.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FACTSELER.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FDATE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FID.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FORMCODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FORMYEAR.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_HAZCODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_INVCODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_MBCODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_MBPROJCODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_MBSBSTCODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_NEW_CURVAL.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_NOSEFARESH.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_NOTE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ORDERNO.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ORDERYEAR.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_OTHERPAY.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PAGE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PARPRICE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PARTKIND.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PAYKNDCODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PELAK.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PKIND.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PRECODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PREDATE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PREPRICE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PRJMRDTLID.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PROFORMCOD.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PROJCODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PROJYEAR.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PRO_YEAR.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_QC.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_RECNO.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_RESNO.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ROW_ID3.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SEFCODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SELERCODE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SELERNAME.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SUB1.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SUB2.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SUB3.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SUB4.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SUMSAN.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE1.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE2.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE3.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE4.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE5.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE6.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE7.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO1.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO2.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO3.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO4.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO5.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO6.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO7.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TANNO.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_USER.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_USERNAME.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_WDPERCENT.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_WELLDONE.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_WUSER.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_YEAR.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FACTNO_GUID.SetVisible(Visible);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FactorNum.SetVisible(Visible);

	//         Ref_SCMPETCO_ProductRequestDetails_SH_FactBuy_DTL.SetVisible(Visible);

	//         Ref_SCMPETCO_ProductRequestDetails_FB_ACCCODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ACTCODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ACTYEAR.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ADD1.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ADD2.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ADD3.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ADD4.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_AMOUNT.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ARRNO.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ARRYEAR.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_BAARNAME.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_BABCODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_BRANCHCODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_BUYKIND.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C1901.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C1950.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CAR_TYPE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CASHCODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CASHKIND.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_COMP_NAME.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CPRICE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CREATOR.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO1.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO2.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO3.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO4.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO5.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO6.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO7.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO8.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO9.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO10.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO1.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO2.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO3.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO4.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO5.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO6.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO7.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO8.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO9.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO10.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CURFACT.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CURKIND.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CURPKID.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C_CURVAL.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C_DEDUCE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C_OTHERPAY.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C_PARPRICE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C_PREPRICE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C_WELLDONE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DEDUCE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DOC_ID.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DOC_ID2.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DOC_YEAR.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DOC_YEAR2.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DRV_CARTNO.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DRV_NAME.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ELAMNO.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FACTDATE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FACTNO.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FACTOR.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FACTPOS.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FACTSELER.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FDATE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FID.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FORMCODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FORMYEAR.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_HAZCODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_INVCODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_MBCODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_MBPROJCODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_MBSBSTCODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_NEW_CURVAL.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_NOSEFARESH.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_NOTE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ORDERNO.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ORDERYEAR.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_OTHERPAY.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PAGE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PARPRICE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PARTKIND.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PAYKNDCODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PELAK.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PKIND.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PRECODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PREDATE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PREPRICE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PRJMRDTLID.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PROFORMCOD.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PROJCODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PROJYEAR.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PRO_YEAR.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_QC.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_RECNO.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_RESNO.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ROW_ID3.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SEFCODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SELERCODE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SELERNAME.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SUB1.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SUB2.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SUB3.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SUB4.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SUMSAN.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE1.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE2.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE3.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE4.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE5.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE6.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE7.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO1.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO2.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO3.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO4.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO5.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO6.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO7.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TANNO.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_USER.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_USERNAME.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_WDPERCENT.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_WELLDONE.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_WUSER.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_YEAR.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FACTNO_GUID.SetDisabled(true);
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FactorNum.SetDisabled(true);
	//     }

	//     public static async Task ResidAnbarIsNull()
	//     {
	//         Ref_SCMPETCO_ProductRequestDetails_FB_Search_NotMapped = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ACCCODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ACTCODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ACTYEAR = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ADD1 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ADD2 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ADD3 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ADD4 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_AMOUNT = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ARRNO = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ARRYEAR = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_BAARNAME = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_BABCODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_BRANCHCODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_BUYKIND = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C1901 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C1950 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CAR_TYPE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CASHCODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CASHKIND = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_COMP_NAME = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CPRICE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CREATOR = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO1 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO2 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO3 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO4 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO5 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO6 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO7 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO8 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO9 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO10 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO1 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO2 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO3 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO4 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO5 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO6 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO7 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO8 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO9 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO10 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CURFACT = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CURKIND = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_CURPKID = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C_CURVAL = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C_DEDUCE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C_OTHERPAY = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C_PARPRICE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C_PREPRICE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_C_WELLDONE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DEDUCE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DOC_ID = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DOC_ID2 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DOC_YEAR = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DOC_YEAR2 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DRV_CARTNO = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_DRV_NAME = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ELAMNO = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FACTDATE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FACTNO = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FACTOR = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FACTPOS = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FACTSELER = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FDATE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FID = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FORMCODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FORMYEAR = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_HAZCODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_INVCODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_MBCODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_MBPROJCODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_MBSBSTCODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_NEW_CURVAL = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_NOSEFARESH = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_NOTE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ORDERNO = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ORDERYEAR = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_OTHERPAY = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PAGE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PARPRICE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PARTKIND = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PAYKNDCODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PELAK = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PKIND = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PRECODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PREDATE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PREPRICE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PRJMRDTLID = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PROFORMCOD = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PROJCODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PROJYEAR = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_PRO_YEAR = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_QC = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_RECNO = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_RESNO = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_ROW_ID3 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SEFCODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SELERCODE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SELERNAME = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SUB1 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SUB2 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SUB3 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SUB4 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_SUMSAN = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE1 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE2 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE3 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE4 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE5 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE6 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE7 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO1 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO2 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO3 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO4 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO5 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO6 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO7 = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_TANNO = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_USER = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_USERNAME = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_WDPERCENT = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_WELLDONE = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_WUSER = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_YEAR = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FACTNO_GUID = null;
	//         Ref_SCMPETCO_ProductRequestDetails_FB_FactorNum = null;
	//         Ref_SCMPETCO_ProductRequestDetails_SH_FactBuy_DTL = null;
	//     }

	//     public async Task ResidAnbarSetShomaran(dynamic Selected, Entity.SCMPETCO_ProductRequestDetails Item)
	//     {
	//         //Console.WriteLine("#Log 4");
	//         //Console.WriteLine(await Utility.JSON.ToJson(Selected));
	//         // Console.WriteLine(Selected.FACTNO_GUID);
	//         //try
	//         //{
	//         //}
	//         //catch (Exception ex)
	//         //{
	//         //    Console.WriteLine(await Utility.JSON.ToJson(ex));
	//         //}


	//         Item.FB_FACTNO_GUID = Selected.FACTNO_GUID;
	//         Item.FB_ACCCODE = Selected.ACCCODE;
	//         Item.FB_ACTCODE = Selected.ACTCODE;
	//         Item.FB_ACTYEAR = Selected.ACTYEAR;
	//         Item.FB_ADD1 = Selected.ADD1;
	//         Item.FB_ADD2 = Selected.ADD2;
	//         Item.FB_ADD3 = Selected.ADD3;
	//         Item.FB_ADD4 = Selected.ADD4;
	//         Item.FB_AMOUNT = Selected.AMOUNT;
	//         Item.FB_ARRNO = Selected.ARRNO;
	//         Item.FB_ARRYEAR = Selected.ARRYEAR;
	//         Item.FB_BAARNAME = Selected.BAARNAME;
	//         Item.FB_BABCODE = Selected.BABCODE;
	//         Item.FB_BRANCHCODE = Selected.BRANCHCODE;
	//         Item.FB_BUYKIND = Selected.BUYKIND;
	//         Item.FB_C1901 = Selected.C1901;
	//         Item.FB_C1950 = Selected.C1950;
	//         Item.FB_CAR_TYPE = Selected.CAR_TYPE;
	//         Item.FB_CASHCODE = Selected.CASHCODE;
	//         Item.FB_CASHKIND = Selected.CASHKIND;
	//         Item.FB_COMP_NAME = Selected.COMP_NAME;
	//         Item.FB_CPRICE = Selected.CPRICE;
	//         Item.FB_CREATOR = Selected.CREATOR;
	//         Item.FB_CTFKNO1 = Selected.CTFKNO1;
	//         Item.FB_CTFKNO2 = Selected.CTFKNO2;
	//         Item.FB_CTFKNO3 = Selected.CTFKNO3;
	//         Item.FB_CTFKNO4 = Selected.CTFKNO4;
	//         Item.FB_CTFKNO5 = Selected.CTFKNO5;
	//         Item.FB_CTFKNO6 = Selected.CTFKNO6;
	//         Item.FB_CTFKNO7 = Selected.CTFKNO7;
	//         Item.FB_CTFKNO8 = Selected.CTFKNO8;
	//         Item.FB_CTFKNO9 = Selected.CTFKNO9;
	//         Item.FB_CTFKNO10 = Selected.CTFKNO10;
	//         Item.FB_CTFNO1 = Selected.CTFNO1;
	//         Item.FB_CTFNO2 = Selected.CTFNO2;
	//         Item.FB_CTFNO3 = Selected.CTFNO3;
	//         Item.FB_CTFNO4 = Selected.CTFNO4;
	//         Item.FB_CTFNO5 = Selected.CTFNO5;
	//         Item.FB_CTFNO6 = Selected.CTFNO6;
	//         Item.FB_CTFNO7 = Selected.CTFNO7;
	//         Item.FB_CTFNO8 = Selected.CTFNO8;
	//         Item.FB_CTFNO9 = Selected.CTFNO9;
	//         Item.FB_CTFNO10 = Selected.CTFNO10;
	//         Item.FB_CURFACT = Selected.CURFACT;
	//         Item.FB_CURKIND = Selected.CURKIND;
	//         Item.FB_CURPKID = Selected.CURPKID;
	//         Item.FB_C_CURVAL = Selected.C_CURVAL;
	//         Item.FB_C_DEDUCE = Selected.C_DEDUCE;
	//         Item.FB_C_OTHERPAY = Selected.C_OTHERPAY;
	//         Item.FB_C_PARPRICE = Selected.C_PARPRICE;
	//         Item.FB_C_PREPRICE = Selected.C_PREPRICE;
	//         Item.FB_C_WELLDONE = Selected.C_WELLDONE;
	//         Item.FB_DEDUCE = Selected.DEDUCE;
	//         Item.FB_DOC_ID = Selected.DOC_ID;
	//         Item.FB_DOC_ID2 = Selected.DOC_ID2;
	//         Item.FB_DOC_YEAR = Selected.DOC_YEAR;
	//         Item.FB_DOC_YEAR2 = Selected.DOC_YEAR2;
	//         Item.FB_DRV_CARTNO = Selected.DRV_CARTNO;
	//         Item.FB_DRV_NAME = Selected.DRV_NAME;
	//         Item.FB_ELAMNO = Selected.ELAMNO;
	//         Item.FB_FACTDATE = Selected.FACTDATE;
	//         Item.FB_FACTNO = Selected.FACTNO;
	//         Item.FB_FACTOR = Selected.FACTOR;
	//         Item.FB_FACTPOS = Selected.FACTPOS;
	//         Item.FB_FACTSELER = Selected.FACTSELER;
	//         Item.FB_FDATE = Selected.FDATE;
	//         Item.FB_FID = Selected.FID;
	//         Item.FB_FORMCODE = Selected.FORMCODE;
	//         Item.FB_FORMYEAR = Selected.FORMYEAR;
	//         Item.FB_HAZCODE = Selected.HAZCODE;
	//         Item.FB_INVCODE = Selected.INVCODE;
	//         Item.FB_MBCODE = Selected.MBCODE;
	//         Item.FB_MBPROJCODE = Selected.MBPROJCODE;
	//         Item.FB_MBSBSTCODE = Selected.MBSBSTCODE;
	//         Item.FB_NEW_CURVAL = Selected.NEW_CURVAL;
	//         Item.FB_NOSEFARESH = Selected.NOSEFARESH;
	//         Item.FB_NOTE = Selected.NOTE;
	//         Item.FB_ORDERNO = Selected.ORDERNO;
	//         Item.FB_ORDERYEAR = Selected.ORDERYEAR;
	//         Item.FB_OTHERPAY = Selected.OTHERPAY;
	//         Item.FB_PAGE = Selected.PAGE;
	//         Item.FB_PARPRICE = Selected.PARPRICE;
	//         Item.FB_PARTKIND = Selected.PARTKIND;
	//         Item.FB_PAYKNDCODE = Selected.PAYKNDCODE;
	//         Item.FB_PELAK = Selected.PELAK;
	//         Item.FB_PKIND = Selected.PKIND;
	//         Item.FB_PRECODE = Selected.PRECODE;
	//         Item.FB_PREDATE = Selected.PREDATE;
	//         Item.FB_PREPRICE = Selected.PREPRICE;
	//         Item.FB_PRJMRDTLID = Selected.PRJMRDTLID;
	//         Item.FB_PROFORMCOD = Selected.PROFORMCOD;
	//         Item.FB_PROJCODE = Selected.PROJCODE;
	//         Item.FB_PROJYEAR = Selected.PROJYEAR;
	//         Item.FB_PRO_YEAR = Selected.PRO_YEAR;
	//         Item.FB_QC = Selected.QC;
	//         Item.FB_RECNO = Selected.RECNO;
	//         Item.FB_RESNO = Selected.RESNO;
	//         Item.FB_ROW_ID3 = Selected.ROW_ID3;
	//         Item.FB_SEFCODE = Selected.SEFCODE;
	//         Item.FB_SELERCODE = Selected.SELERCODE;
	//         Item.FB_SELERNAME = Selected.SELERNAME;
	//         Item.FB_SUB1 = Selected.SUB1;
	//         Item.FB_SUB2 = Selected.SUB2;
	//         Item.FB_SUB3 = Selected.SUB3;
	//         Item.FB_SUB4 = Selected.SUB4;
	//         Item.FB_SUMSAN = Selected.SUMSAN;
	//         Item.FB_TAFCODE1 = Selected.TAFCODE1;
	//         Item.FB_TAFCODE2 = Selected.TAFCODE2;
	//         Item.FB_TAFCODE3 = Selected.TAFCODE3;
	//         Item.FB_TAFCODE4 = Selected.TAFCODE4;
	//         Item.FB_TAFCODE5 = Selected.TAFCODE5;
	//         Item.FB_TAFCODE6 = Selected.TAFCODE6;
	//         Item.FB_TAFCODE7 = Selected.TAFCODE7;
	//         Item.FB_TAFKINDNO1 = Selected.TAFKINDNO1;
	//         Item.FB_TAFKINDNO2 = Selected.TAFKINDNO2;
	//         Item.FB_TAFKINDNO3 = Selected.TAFKINDNO3;
	//         Item.FB_TAFKINDNO4 = Selected.TAFKINDNO4;
	//         Item.FB_TAFKINDNO5 = Selected.TAFKINDNO5;
	//         Item.FB_TAFKINDNO6 = Selected.TAFKINDNO6;
	//         Item.FB_TAFKINDNO7 = Selected.TAFKINDNO7;
	//         Item.FB_TANNO = Selected.TANNO;
	//         Item.FB_USER = Selected.USER;
	//         Item.FB_USERNAME = Selected.USERNAME;
	//         Item.FB_WDPERCENT = Selected.WDPERCENT;
	//         Item.FB_WELLDONE = Selected.WELLDONE;
	//         Item.FB_WUSER = Selected.WUSER;
	//         Item.FB_YEAR = Selected.YEAR;
	//         Item.FB_FactorNum = Selected.FactorNum;

	//         //	فراخوانی داده از dropdown TempNoNum برای گرید داده های آن
	//         Ref_SCMPETCO_ProductRequestDetails_SH_FactBuy_DTL.SetEntity(Item);
	//         Ref_SCMPETCO_ProductRequestDetails_SH_FactBuy_DTL.LoadData();
	//     }


	// حواله مصرف
	// a5b1bc7b-8bb7-ef11-a4fa-005056a2b6bd

	// خرید کالا 
	// 09cf6986-8bb7-ef11-a4fa-005056a2b6bd

	// رسید انبار
	// 0acf6986-8bb7-ef11-a4fa-005056a2b6bd
}

#endregion /SCM_Uitility

#region [MSG] 

public class MSG
{
	public IToastService toastService { get; set; }

	public MSG(IToastService _toastService)
	{
		toastService = _toastService;
	}

	public async Task ShowError(string Message)
	{
		toastService.ShowError(Message,
			settings =>
			{
				settings.Timeout = 4;
				settings.ShowProgressBar = true;
				settings.PauseProgressOnHover = true;
			});
	}

	public async Task ShowInfo(string Message)
	{
		toastService.ShowInfo(Message,
			settings =>
			{
				settings.Timeout = 4;
				settings.ShowProgressBar = true;
				settings.PauseProgressOnHover = true;
			});
	}

	public async Task ShowSuccess(string Message)
	{
		toastService.ShowSuccess(Message,
			settings =>
			{
				settings.Timeout = 4;
				settings.ShowProgressBar = true;
				settings.PauseProgressOnHover = true;
			});

	}

	public async Task ShowWarning(string Message)
	{
		toastService.ShowWarning(Message,
			settings =>
			{
				settings.Timeout = 4;
				settings.ShowProgressBar = true;
				settings.PauseProgressOnHover = true;
			});
	}
}

#endregion [MSG]

#region [ConvertDateTime]
// تبدیل تاریخ 
public class DateTimeConverter
{
	public DateTime ConvertShamsiToMiladi(string shamsiDate)
	{
		// Expecting format: yyyy/MM/dd
		var parts = shamsiDate.Split('/');
		if (parts.Length != 3)
			throw new FormatException("Invalid Shamsi date format");

		int year = int.Parse(parts[0]);
		int month = int.Parse(parts[1]);
		int day = int.Parse(parts[2]);

		PersianCalendar pc = new PersianCalendar();
		DateTime miladiDate = pc.ToDateTime(year, month, day, 0, 0, 0, 0);
		return miladiDate;

		////sample
		////string shamsi = "1403/04/01";
		////DateTime miladi = DateConverter.ConvertShamsiToMiladi(shamsi);
		////Console.WriteLine(miladi.ToString("yyyy-MM-dd")); // Output: 2024-06-21

	}

	public static string ConvertMiladiToShamsi(DateTime miladiDate)
	{
		PersianCalendar pc = new PersianCalendar();

		int year = pc.GetYear(miladiDate);
		int month = pc.GetMonth(miladiDate);
		int day = pc.GetDayOfMonth(miladiDate);

		return $"{year:0000}/{month:00}/{day:00}";


		////DateTime miladi = new DateTime(2025, 07, 23);
		////string shamsi = DateConverter.ConvertMiladiToShamsi(miladi);
		////Console.WriteLine(shamsi); // Output: 1404/05/01
	}


	// Converts Shamsi date string with time (e.g. "1403/04/01 14:30:00") to Miladi DateTime
	public static DateTime ConvertShamsiToMiladiWithTime(string shamsiDateTime)
	{
		var dateTimeParts = shamsiDateTime.Split(' ');
		var dateParts = dateTimeParts[0].Split('/');
		var timeParts = dateTimeParts.Length > 1 ? dateTimeParts[1].Split(':') : new[] { "0", "0" };

		int year = int.Parse(dateParts[0]);
		int month = int.Parse(dateParts[1]);
		int day = int.Parse(dateParts[2]);

		int hour = int.Parse(timeParts[0]);
		int minute = int.Parse(timeParts[1]);
		//int second = int.Parse(timeParts[2]);

		PersianCalendar pc = new PersianCalendar();
		return pc.ToDateTime(year, month, day, hour, minute, 0, 0);


		////string shamsi = "1403/04/01 14:30";
		////DateTime miladi = DateTimeConverter.ConvertShamsiToMiladi(shamsi);
		////Console.WriteLine(miladi); // Output: 2024-06-21 14:30:00
	}

	// Converts Miladi DateTime to Shamsi string with time (e.g. "1403/04/01 14:30:00")
	public static string ConvertMiladiToShamsiWithTime(DateTime miladiDate)
	{
		PersianCalendar pc = new PersianCalendar();

		int year = pc.GetYear(miladiDate);
		int month = pc.GetMonth(miladiDate);
		int day = pc.GetDayOfMonth(miladiDate);

		int hour = pc.GetHour(miladiDate);
		int minute = pc.GetMinute(miladiDate);
		int second = pc.GetSecond(miladiDate);

		return $"{year:0000}/{month:00}/{day:00} {hour:00}:{minute:00}:{second:00}";


		////DateTime miladi = DateTime.Now;
		////string shamsi = DateTimeConverter.ConvertMiladiToShamsi(miladi);
		////Console.WriteLine(shamsi); // Output: e.g. "1404/05/01 10:45:23"
	}

}
#endregion ConvertDateTime

#region [EMP_Data]

namespace EMP_Data
{
	public static class EmployeeData
	{
		#region [EmployeeMasterDetail]
		/// <summary>
		/// HR_EMP_Employees_EmployeeInfos
		/// این یک ویو دیتابیسی از جداول کارمند و جزئیات اطلاعات کارمند است
		/// </summary>
		/// <param name="id"></param>
		/// <param name="_UserId"></param>
		/// <returns></returns>
		public static async Task<Entity.HR_EMP_Employees_EmployeeInfos> EmployeeMasterDetail(string id, string _UserId)
		{
			if (string.IsNullOrEmpty(id))
			{
				Console.WriteLine("❌ Employee ID is null or empty in EmployeeMasterDetail1");
				return null;
			}

			var TablePost = new Baya.Models.ORM.Table
			{
				Name = "HR_EMP_Employees_EmployeeInfos",
				Column = new List<Coulmn>
				{
					new Coulmn { Name = "Id", NameAs = "Id" }, // شناسه رکورد (GUID)
					//new Coulmn { Name = "HR_EMP_EmployeesId", NameAs = "HR_EMP_EmployeesId" }, // شناسه اطلاعات کارمند (مرتبط با جدول اصلی HR_EMP_Employees)
					new Coulmn { Name = "EmployeeNo", NameAs = "EmployeeNo" }, // کد کارمندی
					new Coulmn { Name = "LastEmployeeNO", NameAs = "LastEmployeeNO" }, // آخرین کد کارمندی
					new Coulmn { Name = "EmployeePersonelNo", NameAs = "EmployeePersonelNo" }, // شماره پرسنلی کارمند
					new Coulmn { Name = "EmployeeLastPersonelNo", NameAs = "EmployeeLastPersonelNo" }, // آخرین شماره پرسنلی کارمند
					new Coulmn { Name = "FirstName", NameAs = "FirstName" }, // نام
					new Coulmn { Name = "LastName", NameAs = "LastName" }, // نام خانوادگی
					new Coulmn { Name = "FatherName", NameAs = "FatherName" }, // نام پدر
					new Coulmn { Name = "NationalCode", NameAs = "NationalCode" }, // کد ملی
					new Coulmn { Name = "IdCardNo", NameAs = "IdCardNo" }, // شماره شناسنامه
					//new Coulmn { Name = "BirthDate_Fa", NameAs = "BirthDate_Fa" }, // تاریخ تولد (شمسی به صورت رشته)
					//new Coulmn { Name = "BirthDate", NameAs = "BirthDate" }, // تاریخ تولد (میلادی)
					//new Coulmn { Name = "BirthDateDD", NameAs = "BirthDateDD" }, // روز تاریخ تولد
					//new Coulmn { Name = "BirthDateMM", NameAs = "BirthDateMM" }, // ماه تاریخ تولد
					//new Coulmn { Name = "BirthDateYYYY", NameAs = "BirthDateYYYY" }, // سال تاریخ تولد
					//new Coulmn { Name = "CityOfBirth", NameAs = "CityOfBirth" }, // شناسه شهر محل تولد
					//new Coulmn { Name = "CityOfBirthTitle", NameAs = "CityOfBirthTitle" }, // نام شهر محل تولد
					//new Coulmn { Name = "CityOfIssue", NameAs = "CityOfIssue" }, // شناسه شهر محل صدور
					//new Coulmn { Name = "CityIssueTitle", NameAs = "CityIssueTitle" }, // نام شهر محل صدور
					//new Coulmn { Name = "Address", NameAs = "Address" }, // نشانی
					//new Coulmn { Name = "Mobile", NameAs = "Mobile" }, // تلفن همراه
					//new Coulmn { Name = "Phone", NameAs = "Phone" }, // شماره تلفن ثابت
					//new Coulmn { Name = "EmploymentDate_Fa", NameAs = "EmploymentDate_Fa" }, // تاریخ استخدام (شمسی)
					//new Coulmn { Name = "EmploymentStartDate_Fa", NameAs = "EmploymentStartDate_Fa" }, // تاریخ آخرین تسویه حساب (شمسی)
					//new Coulmn { Name = "EmploymentDateInGroup_Fa", NameAs = "EmploymentDateInGroup_Fa" }, // تاریخ استخدام در گروه (شمسی)
					//new Coulmn { Name = "EmploymentDate", NameAs = "EmploymentDate" }, // تاریخ استخدام (میلادی)
					//new Coulmn { Name = "EmploymentStartDate", NameAs = "EmploymentStartDate" }, // تاریخ آخرین تسویه حساب (میلادی)
					//new Coulmn { Name = "EmploymentDateInGroup", NameAs = "EmploymentDateInGroup" }, // تاریخ استخدام در گروه (میلادی)
					//new Coulmn { Name = "EmployeeAge", NameAs = "EmployeeAge" }, // سن کارمند (به روز)
					//new Coulmn { Name = "EmployeeAgeText", NameAs = "EmployeeAgeText" }, // سن کارمند (به صورت متن، مثلاً "13593سال")
					//new Coulmn { Name = "EmployeeWorkExperienceText", NameAs = "EmployeeWorkExperienceText" }, // سابقه کار کارمند (به روز)
					//new Coulmn { Name = "BaseInfo_MaritalStatusId", NameAs = "BaseInfo_MaritalStatusId" }, // شناسه وضعیت تاهل
					//new Coulmn { Name = "BaseInfo_MaritalStatusTitle", NameAs = "BaseInfo_MaritalStatusTitle" }, // عنوان وضعیت تاهل
					//new Coulmn { Name = "BaseInfo_GenderId", NameAs = "BaseInfo_GenderId" }, // شناسه جنسیت
					//new Coulmn { Name = "BaseInfo_GenderTitle", NameAs = "BaseInfo_GenderTitle" }, // عنوان جنسیت
					//new Coulmn { Name = "BaseInfo_MilitaryStatusId", NameAs = "BaseInfo_MilitaryStatusId" }, // شناسه وضعیت نظام وظیفه
					//new Coulmn { Name = "BaseInfo_MilitaryStatusTitle", NameAs = "BaseInfo_MilitaryStatusTitle" }, // عنوان وضعیت نظام وظیفه
					//new Coulmn { Name = "BaseInfo_CitiesAreasId", NameAs = "BaseInfo_CitiesAreasId" }, // شناسه منطقه شهری
					//new Coulmn { Name = "BaseInfo_CitiesAreasTitle", NameAs = "BaseInfo_CitiesAreasTitle" }, // عنوان منطقه شهری
					//new Coulmn { Name = "BaseInfo_ORG_CompaniesId", NameAs = "BaseInfo_ORG_CompaniesId" }, // شناسه شرکت
					//new Coulmn { Name = "BaseInfo_ORG_CompaniesTitle", NameAs = "BaseInfo_ORG_CompaniesTitle" }, // نام شرکت
					//new Coulmn { Name = "HR_EMP_StatusId", NameAs = "HR_EMP_StatusId" }, // شناسه وضعیت کارمند
					//new Coulmn { Name = "HR_EMP_StatusTitle", NameAs = "HR_EMP_StatusTitle" }, // عنوان وضعیت کارمند
					//new Coulmn { Name = "HR_Base_TransportServiceId", NameAs = "HR_Base_TransportServiceId" }, // شناسه خدمات حمل و نقل
					//new Coulmn { Name = "HR_Base_TransportServiceTitle", NameAs = "HR_Base_TransportServiceTitle" }, // عنوان خدمات حمل و نقل
					//new Coulmn { Name = "HasTransportService", NameAs = "HasTransportService" }, // سرویس ایاب ذهاب دارد؟
					//new Coulmn { Name = "SupplementaryInsurance", NameAs = "SupplementaryInsurance" }, // بیمه تکمیلی
					//new Coulmn { Name = "lifeInsurance", NameAs = "lifeInsurance" }, // بیمه عمر
					//new Coulmn { Name = "AccidentInsurance", NameAs = "AccidentInsurance" }, // بیمه حوادث
					//new Coulmn { Name = "Arzagh", NameAs = "Arzagh" }, // ارزاق دارد؟
					//new Coulmn { Name = "HasDisabledChild", NameAs = "HasDisabledChild" }, // فرزند معلول دارد؟
					//new Coulmn { Name = "MartyrsFamily", NameAs = "MartyrsFamily" }, // خانواده شهدا
					//new Coulmn { Name = "MartyrsChild", NameAs = "MartyrsChild" }, // فرزند شهید
					//new Coulmn { Name = "JebheMotanaveb_Days", NameAs = "JebheMotanaveb_Days" }, // مدت جبهه متناوب (روز)
					//new Coulmn { Name = "JebheMotavali_Days", NameAs = "JebheMotavali_Days" }, // مدت جبهه متوالی (روز)
					//new Coulmn { Name = "Captivity_Days", NameAs = "Captivity_Days" }, // مدت اسارت (روز)
					//new Coulmn { Name = "Relatives_Captivity_Days", NameAs = "Relatives_Captivity_Days" }, // مدت اسارت فرد مرتبط (روز)
					//new Coulmn { Name = "Relatives_Jebhe_Days", NameAs = "Relatives_Jebhe_Days" }, // مدت جبهه فرد مرتبط (روز)
					//new Coulmn { Name = "Relatives_VeteranPercentage", NameAs = "Relatives_VeteranPercentage" }, // درصد جانبازی فرد مرتبط
					//new Coulmn { Name = "VeteranPercentage", NameAs = "VeteranPercentage" }, // درصد جانبازی کارمند
					//new Coulmn { Name = "SanavatEnteghali_Day", NameAs = "SanavatEnteghali_Day" }, // سنوات انتقالی (روز)
					//new Coulmn { Name = "IsActive", NameAs = "IsActive" }, // فعال
					//new Coulmn { Name = "FullName", NameAs = "FullName" }, // نام کامل
					////new Coulmn { Name = "UserId", NameAs = "UserId" }, // شناسه کاربر سیستمی
					//// فیلدهای ContactEmployee
					//new Coulmn { Name = "ContactEmployee_FullName", NameAs = "ContactEmployee_FullName" }, // نام و نام خانوادگی فرد مرتبط
					//new Coulmn { Name = "ContactEmployee_Address", NameAs = "ContactEmployee_Address" }, // نشانی فرد مرتبط
					//new Coulmn { Name = "ContactEmployee_Tel", NameAs = "ContactEmployee_Tel" }, // شماره تلفن ضروری فرد مرتبط
					//// فیلدهای نسبت به سایر جداول
					//new Coulmn { Name = "HR_Base_ContactEmployeeRelativeId", NameAs = "HR_Base_ContactEmployeeRelativeId" }, // شناسه نسبت فرد با کارمند
					//new Coulmn { Name = "HR_Base_ContactEmployeeRelativeTitle", NameAs = "HR_Base_ContactEmployeeRelativeTitle" }, // عنوان نسبت فرد با کارمند
					//// فیلدهای سیستمی
					////new Coulmn { Name = "CreateDate", NameAs = "CreateDate" }, // تاریخ ایجاد رکورد
					////new Coulmn { Name = "UpdateDate", NameAs = "UpdateDate" }, // تاریخ آخرین ویرایش رکورد
					////new Coulmn { Name = "CreateUser", NameAs = "CreateUser" }, // شناسه کاربر ایجادکننده
					////new Coulmn { Name = "UpdateUser", NameAs = "UpdateUser" }, // شناسه کاربر ویرایش‌کننده
					////new Coulmn { Name = "IsDelete", NameAs = "IsDelete" }, // حذف منطقی
					////new Coulmn { Name = "RequestID", NameAs = "RequestID" }, // شناسه درخواست
				}
			};

			var NewQuery = new QueryBuilderFilterRule { Condition = "AND" };
			NewQuery.Rules = new List<QueryBuilderFilterRule>
			{
				new QueryBuilderFilterRule
				{
					Id = "Id", // شناسه اصلی کارمند
					Field = "Id",
					Input = "text",
					Operator = "equal",
					Type = "string",
					Value = new string[] { id }
				}
			};

			var Model = await ApiServer.External.Services.Data.Get(TablePost, NewQuery, "HR_EMP_Employees_EmployeeInfos", _UserId);

			if (Model?.Status != HttpStatusCode.OK)
			{
				Console.WriteLine($"❌ API error {Model?.Status} for employee ID: {id}");
				return null;
			}

			if (string.IsNullOrEmpty(Model.Content?.ToString()))
			{
				Console.WriteLine($"❌ API returned empty content for ID: {id}");
				return null;
			}

			try
			{
				var vw_emp_data = await JSON.ToObject<Entity.HR_EMP_Employees_EmployeeInfos>(Model.Content.ToString());
				return vw_emp_data;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"💥 JSON Deserialize error for ID {id}: {ex.Message}");
				return null;
			}
		}
		#endregion

		#region [LastVerdictEmp]
		/// <summary>
		/// جدول و نمایش آخرین حکم کارمند
		/// </summary>
		/// <param name="employeeId"></param>
		/// <param name="_UserId"></param>
		/// <returns></returns>
		public static async Task<Entity.View_HR_CVR_VerdictRecruiting> LastVerdictEmp(string employeeId, string _UserId)
		{
			if (string.IsNullOrEmpty(employeeId))
			{
				Console.WriteLine("❌ Employee ID is null or empty in LastVerdictEmp");
				return null;
			}

			// دریافت آخرین حکم فعال کارمند
			var TablePost = new Baya.Models.ORM.Table
			{
				Name = "View_HR_CVR_VerdictRecruiting",
				Column = new List<Coulmn>
				{
					new Coulmn { Name = "Id", NameAs = "Id" },
					new Coulmn { Name = "ExecutionDateSentence", NameAs = "ExecutionDateSentence" }, // برای مرتب‌سازی
					new Coulmn { Name = "ExecutionDateSentence_Fa", NameAs = "ExecutionDateSentence_Fa" },
					new Coulmn { Name = "HR_EMP_EmployeesId", NameAs = "HR_EMP_EmployeesId" },
					new Coulmn { Name = "HR_StatusVerdictRecruitingId", NameAs = "HR_StatusVerdictRecruitingId" }
				}
			};

			var NewQuery = new QueryBuilderFilterRule { Condition = "AND" };
			NewQuery.Rules = new List<QueryBuilderFilterRule>
			{
				new QueryBuilderFilterRule
				{
					//Id = "HR_EMP_EmployeesId", 
					Field = "HR_EMP_EmployeesId",// شناسه اصلی کارمند
					Input = "text",
					Operator = "equal",
					Type = "string",
					Value = new string[] { employeeId }
				},
				new QueryBuilderFilterRule
				{
					Field = "HR_StatusVerdictRecruitingId",
					Operator = "equal",
					Type = "string",
					Value = new[] { "0DA9356F-211D-F011-A502-005056A2B6BD" } // وضعیت "فعال"
				}
			};

			//var Model = await ApiServer.External.Services.Data.GetList(TablePost, NewQuery, "View_HR_CVR_VerdictRecruiting", _UserId);
			var Model = await ApiServer.External.Services.Data.GetList(
				Entity: "View_HR_CVR_VerdictRecruiting",
				limit: null,
				skip: null,
				include: TablePost,
				Filter: NewQuery
			);

			if (Model?.Status != HttpStatusCode.OK || string.IsNullOrEmpty(Model.Content?.ToString()))
			{
				Console.WriteLine($"❌ No active verdict found for employee ID: {employeeId}");
				return null;
			}


			if (string.IsNullOrEmpty(Model.Content?.ToString()))
			{
				Console.WriteLine($"❌ API returned empty content for ID: {employeeId}");
				return null;
			}

			try
			{
				// ⚠️ تجزیه به LIST چون چندین حکم ممکن است وجود داشته باشد
				var verdicts = await JSON.ToObject<List<Entity.View_HR_CVR_VerdictRecruiting>>(Model.Content.ToString());

				// مرتب‌سازی بر اساس تاریخ اجرای حکم (نزولی) و انتخاب اولین مورد = آخرین حکم
				var latest = verdicts
					.Where(v => v.ExecutionDateSentence.HasValue)
					.OrderByDescending(v => v.ExecutionDateSentence.Value)
					.FirstOrDefault();

				if (latest == null)
				{
					Console.WriteLine($"⚠️ No verdict with valid date found for employee ID: {employeeId}");
				}

				return latest;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"💥 Deserialize error in LastVerdictEmp for employee ID {employeeId}: {ex.Message}");
				return null;
			}
		}
		#endregion

		#region [ApplyToLastRecruitmentRules]
		/// <summary>
		/// پر کردن فیلدهای مربوطه در HR_CVR_RecruitmentRules با مقادیر مصوبات جاری
		/// بر اساس HR_CVR_ApprovalsMinistryLaborGroupId
		/// </summary>
		public static async Task ApplyToLastRecruitmentRules(HR_CVR_RecruitmentRules item, string userId)
		{
			if (item == null || item.HR_CVR_ApprovalsMinistryLaborGroupId == null)
			{
				Console.WriteLine("⚠️ HR_CVR_ApprovalsMinistryLaborGroupId خالی است یا آیتم نامعتبر است.");
				return;
			}

			var approvalId = item.HR_CVR_ApprovalsMinistryLaborGroupId.Value;

			// ساخت Table برای دریافت فقط فیلدهای مورد نیاز
			var TablePost = new Baya.Models.ORM.Table
			{
				Name = "HR_CVR_ApprovalsMinistryLaborGroup",
				Column = new List<Coulmn>
				{
					new Coulmn { Name = "Id", NameAs = "Id" },
					new Coulmn { Name = "MinistryLabourRightHousing", NameAs = "MinistryLabourRightHousing" },
					new Coulmn { Name = "MinistryLaborRightFood", NameAs = "MinistryLaborRightFood" },
					new Coulmn { Name = "BenKargariMinistryLabor", NameAs = "BenKargariMinistryLabor" },
					new Coulmn { Name = "RightMarryMinistryLabor", NameAs = "RightMarryMinistryLabor" },
					new Coulmn { Name = "ChildrensRightsMinistryLabor", NameAs = "ChildrensRightsMinistryLabor" }
				}
			};

			// فیلتر: فقط رکوردی که Id برابر با HR_CVR_ApprovalsMinistryLaborGroupId باشد
			var NewQuery = new QueryBuilderFilterRule
			{
				Condition = "AND",
				Rules = new List<QueryBuilderFilterRule>
				{
					new QueryBuilderFilterRule
					{
						Field = "Id",
						Operator = "equal",
						Type = "string",
						Value = new[] { approvalId.ToString() }
					}
				}
			};

			Baya.Models.ORM.PagedResult Pager = new()
			{
				PageSize = 1000,
				PageNumber = 1,
			};

			// فراخوانی API
			var Model = await ApiServer.External.Services.Data.GetListPost(
				Table: TablePost,
				Filter: NewQuery,
				Pagination: Pager,
				Entity: "HR_CVR_ApprovalsMinistryLaborGroup"
			);

			if (Model?.Status == HttpStatusCode.OK && !string.IsNullOrEmpty(Model.Content?.ToString()))
			{
				try
				{
					var approvals = await JSON.ToObject<List<HR_CVR_ApprovalsMinistryLaborGroup>>(Model.Content.ToString());
					var approval = approvals?.FirstOrDefault();

					if (approval != null)
					{
						// 1. کمک هزینه مسکن
						item.MinistryLabourRightHousing = approval.MinistryLabourRightHousing;

						// 2. حق خوار و بار
						item.MinistryLaborRightFood = approval.MinistryLaborRightFood;

						// 3. مزایای رفاهی و انگیزه‌ای = بن کارگری
						item.WelfareMotivationalBenefits = approval.BenKargariMinistryLabor;

						// 4. حق تاهل
						item.RightMarryMinistryLabor = approval.RightMarryMinistryLabor;

						// [اختیاری] حق اولاد
						// item.ChildrensRightsMinistryLabor = approval.ChildrensRightsMinistryLabor;

						Console.WriteLine($"✅ مقادیر مصوبات با شناسه {approvalId} به آیتم اعمال شد.");
					}
					else
					{
						Console.WriteLine($"❌ هیچ مصوبه‌ای با شناسه {approvalId} یافت نشد.");
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"💥 خطا در اعمال مصوبات: {ex.Message}");
				}
			}
			else
			{
				Console.WriteLine($"❌ خطا در دریافت مصوبه با شناسه {approvalId}: {Model?.Status}");
			}
		}
		#endregion

		#region [EveryPostVerdict]
		public static async Task<List<HR_CVR_VerdictRecruiting>> EveryPostVerdict(string employeeId, string userId)
		{
			var TablePost = new Baya.Models.ORM.Table
			{
				Name = "HR_CVR_VerdictRecruiting",
				NameAs = "HR_CVR_VerdictRecruiting",

				Column = new List<Coulmn>
				{
					new Coulmn { Name = "HR_Base_InsuranceTypesId", NameAs = "HR_Base_InsuranceTypesId" },
					new Coulmn { Name = "HR_CVR_PersonnelContractId", NameAs = "HR_CVR_PersonnelContractId" },
					new Coulmn { Name = "HR_CVR_TypesRulingsId", NameAs = "HR_CVR_TypesRulingsId" },
					new Coulmn { Name = "InsuranceNumber", NameAs = "InsuranceNumber" },
					new Coulmn { Name = "TypeBonusPayment", NameAs = "TypeBonusPayment" },
					new Coulmn { Name = "GroupTitle", NameAs = "GroupTitle" },
					new Coulmn { Name = "Rank", NameAs = "Rank" },
					new Coulmn { Name = "ExecutionDateSentence", NameAs = "ExecutionDateSentence" },
					new Coulmn { Name = "RegisterTime", NameAs = "RegisterTime" },
					new Coulmn { Name = "ConfirmerTime", NameAs = "ConfirmerTime" },
					new Coulmn { Name = "ApproverTime", NameAs = "ApproverTime" },
					new Coulmn { Name = "NullifierTime", NameAs = "NullifierTime" },
					new Coulmn { Name = "HR_StatusVerdictRecruitingId", NameAs = "HR_StatusVerdictRecruitingId" },
					new Coulmn { Name = "HR_CVR_VerdictRecruitingId", NameAs = "HR_CVR_VerdictRecruitingId" },
					new Coulmn { Name = "HR_CVR_JobId", NameAs = "HR_CVR_JobId" },
					new Coulmn { Name = "HR_CVR_DescriptionRulingsId", NameAs = "HR_CVR_DescriptionRulingsId" },
					new Coulmn { Name = "ExecutionDateSentence_Fa", NameAs = "ExecutionDateSentence_Fa" },
					new Coulmn { Name = "HR_CVR_JobGroupId", NameAs = "HR_CVR_JobGroupId" },
					new Coulmn { Name = "HR_EMP_EmployeesId", NameAs = "HR_EMP_EmployeesId" }
				},
				Relation = new List<Baya.Models.ORM.Table>
				{
					new Baya.Models.ORM.Table
					{
						Name = "HR_CVR_EveryPostVerdict",
						NameAs = "HR_CVR_EveryPostVerdict",
						ModeErtebat = ModeErtebat._1N,

						Column = new List<Coulmn>
						{
						new Coulmn { Name = "Id", NameAs = "Id" },
						new Coulmn { Name = "HR_CVR_VerdictRecruitingId", NameAs = "HR_CVR_VerdictRecruitingId" },
						new Coulmn { Name = "PostType", NameAs = "PostType" },
						new Coulmn { Name = "SectionsType", NameAs = "SectionsType" },
						new Coulmn { Name = "HR_ORG_SectionsId", NameAs = "HR_ORG_SectionsId" },
						new Coulmn { Name = "HR_ORG_PostsId", NameAs = "HR_ORG_PostsId" }
						},
					}
				}
			};

			var NewQuery = new QueryBuilderFilterRule
			{
				Condition = "AND",
				Rules = new List<QueryBuilderFilterRule>
				{
				    // فیلتر بر اساس کارمند
				    new QueryBuilderFilterRule
					{
						Field = "HR_EMP_EmployeesId",
						Operator = "equal",
						Type = "string",
						Value = new string[] { employeeId }
					},
				    // (اختیاری ولی توصیه‌شده) فقط احکام فعال
				    new QueryBuilderFilterRule
					{
						Field = "HR_StatusVerdictRecruitingId",
						Operator = "equal",
						Type = "string",
						Value = new[] { "0DA9356F-211D-F011-A502-005056A2B6BD" } // وضعیت "فعال"
					}
				}
			};

			Baya.Models.ORM.PagedResult Pager = new()
			{
				PageSize = 1000,
				PageNumber = 1,
			};

			// فراخوانی API
			var Model = await ApiServer.External.Services.Data.GetListPost(
				Table: TablePost,
				Filter: NewQuery,
				Pagination: Pager,
				Entity: "HR_CVR_VerdictRecruiting"
			);

			if (Model?.Status == HttpStatusCode.OK && !string.IsNullOrEmpty(Model.Content?.ToString()))
			{
				try
				{
					var approvals = await JSON.ToObject<List<HR_CVR_VerdictRecruiting>>(Model.Content.ToString());
					return approvals;
				}
				catch (Exception ex)
				{
					Console.WriteLine($"💥 خطا در فراخوانی داده ها: {ex.Message}");
				}
			}
			else
			{
				Console.WriteLine($"❌ خطا در دریافت مصوبه با شناسه {Model?.Status}");
			}
			return null;
		}
		#endregion
	}
}

namespace EmployeeModel
{
	public class EmpId
	{
		public Guid EmployeesId { get; set; }
	}
}

namespace StoredProcedureModel
{
	public class StoredProcedureRequestDto
	{
		public string StoredProcedureName { get; set; } = string.Empty;
		public string JsonInput { get; set; } = string.Empty;
	}

	public class Content
	{
		public List<List<object>> DataSets { get; set; }
	}

}

namespace ApiServer.External.Services
{
	public partial class BayaApi
	{

		public static async Task<Result> PersonnelContract(ShomaranApiMode ApiMode, EmployeeModel.EmpId input)
		{
			var DataJson = await JSON.ToJson(input);

			string shomaranApi = "";
			switch (ApiMode)
			{
				case ShomaranApiMode.Polfilm:
					shomaranApi = "https://shomaran.workcv.ir:2010/{0}/api/v1/";
					break;
				case ShomaranApiMode.Petco:
					shomaranApi = "https://shomaranpetcoorm.workcv.ir/{0}/api/v1/";
					break;
				case ShomaranApiMode.Pelat:
					shomaranApi = "https://shomaranatlascellorm.workcv.ir/{0}/api/v1";
					break;
				default:
					break;
			}

			var _content = new StringContent(DataJson, Encoding.UTF8, "application/json");

			Result apiresult = await Send.PostAsync(_content, "BayaApi/PersonnelContract", shomaranApi, ApplicationType.None);

			return apiresult;
		}

		public static async Task<Result> GetEndTimeOfContract(ShomaranApiMode ApiMode, ContractTimeModel.PersonnelEndTimeOfContractRequest input)
		{
			var DataJson = await JSON.ToJson(input);

			string shomaranApi = "";
			switch (ApiMode)
			{
				case ShomaranApiMode.Polfilm:
					shomaranApi = "https://shomaran.workcv.ir:2010/{0}/api/v1/";
					break;
				case ShomaranApiMode.Petco:
					shomaranApi = "https://shomaranpetcoorm.workcv.ir/{0}/api/v1/";
					break;
				case ShomaranApiMode.Pelat:
					shomaranApi = "https://shomaranatlascellorm.workcv.ir/{0}/api/v1";
					break;
				default:
					break;
			}

			var _content = new StringContent(DataJson, Encoding.UTF8, "application/json");

			Result apiresult = await Send.PostAsync(_content, "BayaApi/GetEndTimeOfContract", shomaranApi, ApplicationType.None);

			return apiresult;
		}

		public static async Task<Result> GetAllContract(ShomaranApiMode ApiMode, EmployeeModel.EmpId input)
		{
			var DataJson = await JSON.ToJson(input);

			string shomaranApi = "";
			switch (ApiMode)
			{
				case ShomaranApiMode.Polfilm:
					shomaranApi = "https://shomaran.workcv.ir:2010/{0}/api/v1/";
					break;
				case ShomaranApiMode.Petco:
					shomaranApi = "https://shomaranpetcoorm.workcv.ir/{0}/api/v1/";
					break;
				case ShomaranApiMode.Pelat:
					shomaranApi = "https://shomaranatlascellorm.workcv.ir/{0}/api/v1";
					break;
				default:
					break;
			}

			var _content = new StringContent(DataJson, Encoding.UTF8, "application/json");

			Result apiresult = await Send.PostAsync(_content, "BayaApi/GetAllContract", shomaranApi, ApplicationType.None);

			return apiresult;
		}

		// ***********************************************

		public static async Task<Result> PersonnelVerdictInfos(ShomaranApiMode ApiMode, EmployeeModel.EmpId input)
		{
			var DataJson = await JSON.ToJson(input);

			string shomaranApi = "";
			switch (ApiMode)
			{
				case ShomaranApiMode.Polfilm:
					shomaranApi = "https://shomaran.workcv.ir:2010/{0}/api/v1/";
					break;
				case ShomaranApiMode.Petco:
					shomaranApi = "https://shomaranpetcoorm.workcv.ir/{0}/api/v1/";
					break;
				case ShomaranApiMode.Pelat:
					shomaranApi = "https://shomaranatlascellorm.workcv.ir/{0}/api/v1";
					break;
				default:
					break;
			}

			var _content = new StringContent(DataJson, Encoding.UTF8, "application/json");

			Result apiresult = await Send.PostAsync(_content, "BayaApi/PersonnelVerdictInfos", shomaranApi, ApplicationType.None);

			return apiresult;
		}
		public static async Task<Result> ExecuteSp(ShomaranApiMode ApiMode, StoredProcedureModel.StoredProcedureRequestDto input)
		{
			var DataJson = await JSON.ToJson(input);

			string shomaranApi = "";
			switch (ApiMode)
			{
				case ShomaranApiMode.Polfilm:
					shomaranApi = "https://shomaran.workcv.ir:2010/{0}/api/v1/";
					break;
				case ShomaranApiMode.Petco:
					shomaranApi = "https://shomaranpetcoorm.workcv.ir/{0}/api/v1/";
					break;
				case ShomaranApiMode.Pelat:
					shomaranApi = "https://shomaranatlascellorm.workcv.ir/{0}/api/v1";
					break;
				default:
					break;
			}

			var _content = new StringContent(DataJson, Encoding.UTF8, "application/json");

			Result apiresult = await Send.PostAsync(_content, "BayaApi/ExecuteSp", shomaranApi, ApplicationType.None);

			return apiresult;
		}
	}
}

namespace SP_Contract
{
	public partial class spContract
	{
		public static async Task<List<Entity.HR_CRS_ContractTime>> GetContractListC(Guid HR_EMP_EmployeesId)
		{
			var R = await BayaApi.PersonnelContract(
				ShomaranApiMode.Polfilm,
				new EmployeeModel.EmpId
				{
					EmployeesId = HR_EMP_EmployeesId
				}
			);

			if (R == null)
			{
				Console.WriteLine("❌ خطا: R == null");
				return null;
			}

			var jsonResponse = R.Content.ToString();
			try
			{
				var root = JObject.Parse(jsonResponse);
				var dataSetsToken = root["DataSets"];
				if (dataSetsToken == null || !(dataSetsToken is JArray dataSets) || dataSets.Count < 4)
				{
					Console.WriteLine("❌ خطا: DataSets کامل نیست");
					return null;
				}

				// --- 1. لیست A: CountOfAllContract ---
				var aList = dataSets[0] as JArray;
				if (aList == null || aList.Count == 0)
				{
					Console.WriteLine("❌ لیست A خالی یا null است");
					return null;
				}

				// --- 3. لیست C: لیست بلند ContractTime ---
				var cList = dataSets[2] as JArray;
				if (cList == null)
				{
					Console.WriteLine("❌ لیست C null است");
					return null;
				}
				var ListC = new List<Entity.HR_CRS_ContractTime>();
				try
				{
					ListC = cList.ToObject<List<Entity.HR_CRS_ContractTime>>();
					return ListC;
				}
				catch (Exception ex)
				{
					return null;
				}


			}
			catch (Exception ex)
			{
				return null;

			}
		}
	}
}

namespace ContractTimeModel
{
	public class AllContractModel
	{
		public int CountOfAllContract { get; set; }
	}

	public class CurrentContractModel
	{
		public string TheLastCounterOfCurrentContract { get; set; }
	}

	public class PositionClasificationModel
	{
		public string PositionClasificationId { get; set; }
	}

	public class PersonnelEndTimeOfContractRequest
	{
		public Guid EmployeesId { get; set; }
		public string startDate { get; set; }
		public string contractTime { get; set; }
	}

	public class RootContractTime
	{
		public List<List<ContractTimeData>> DataSets { get; set; }
	}

	public class ContractTimeData
	{
		public string Shamsi { get; set; }
		public DateTime Miladi { get; set; }
	}
	public class ContractRootResponse
	{
		public int Status { get; set; }
		public StoredProcedureModel.Content Content { get; set; }
	}
}

namespace VerdictDataModel
{
	public class RootResponse
	{
		public List<List<EmployeeInfo>> DataSets { get; set; }
	}

	public class EmployeeInfo
	{
		public Guid Id { get; set; }
		public string? EmployeeNo { get; set; }
		public string? EmployeePersonelNo { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? FatherName { get; set; }
		public string? IdCardNo { get; set; }
		public string? NationalCode { get; set; }
		public string? CityOfIssueTitle { get; set; }
		public string? CityOfBirthTitle { get; set; }
		public string? BirthDate_Fa { get; set; }
		public string? BaseInfo_MaritalStatusTitle { get; set; }

		public Guid? BaseInfo_MaritalStatusId { get; set; }
		public Guid? BaseInfo_GenderId { get; set; }

		public string? BaseInfo_GenderTitle { get; set; }
		public string? EmployeeAgeText { get; set; }
		public int? EmployeeAge { get; set; }
		public string? EducationTitle { get; set; }
		public string? BaseInfo_MilitaryStatusTitle { get; set; }
		public string? EmploymentDate_Fa { get; set; }
		public string? EmploymentDateInGroup_Fa { get; set; }
		public string? EmploymentStartDate_Fa { get; set; }
		public string? BankAccountNumber { get; set; }
		public string? IBAN { get; set; }
		public string? InsuranceNumber { get; set; }
		public Guid? SectionId { get; set; }
		public Guid? SubSectionId { get; set; }
		public Guid? PostsId { get; set; }
		public Guid? PositionsId { get; set; }
		public string? HR_CVR_JobId { get; set; }
		public string? HR_CVR_JobTitle { get; set; }
		public string? Code { get; set; }

		// شناسه گروه شغلی
		public string? HR_CVR_JobGroupId { get; set; }

		// عنوان گروه شغلی
		public string? JobGroupTitle { get; set; }

		// رتبه
		public decimal? Rank { get; set; }

		public decimal? RecruitmentAllowance { get; set; }
		public decimal? SalaryHistory { get; set; }
		public decimal? CoefficientDifficultAndHarmfulJobs { get; set; }
		public decimal? TotalDailyBaseWage { get; set; }
		public decimal? DailyAdjustmentDifference { get; set; }
		public decimal? MinistryLabourRightHousing { get; set; }
		public decimal? MinistryLaborRightFood { get; set; }
		public decimal? RightMarryMinistryLabor { get; set; }
		public decimal? ChildrensRightsMinistryLabor { get; set; }
		public decimal? WelfareMotivationalBenefits { get; set; }
		public decimal? OtherBenefits { get; set; }
		public decimal? TotalMonthlySalaryBenefits { get; set; }
		public decimal? RankSalary { get; set; }
		public decimal? JobSalaryRank { get; set; }
		public decimal? RankSalaryNew { get; set; }
		public decimal? SalaryHistoryNew { get; set; }
		public decimal? RightGuardianship { get; set; }
		public decimal? RightGuardianshipNew { get; set; }
		public decimal? CoefficientDurabilityPost { get; set; }
		public decimal? CoefficientDurabilityPostNew { get; set; }
		public decimal? CoefficientDifficultAndHarmfulJobsNew { get; set; }
		public decimal? TotalDailyBaseWageNew { get; set; }
		public decimal? RightMarryMinistryLaborNew { get; set; }
		public decimal? RecruitmentAllowanceNew { get; set; }
		public decimal? MinistryLabourRightHousingNew { get; set; }
		public decimal? MinistryLaborRightFoodNew { get; set; }
		public decimal? ChildrensRightsMinistryLaborNew { get; set; }
		public decimal? WelfareMotivationalBenefitsNew { get; set; }
		public decimal? TotalMonthlySalaryBenefitsNew { get; set; }
		public decimal? JobSalaryRankNew { get; set; }
		public decimal? DailyAdjustmentDifferenceNew { get; set; }
		public decimal? OtherBenefitsNew { get; set; }

		// نوع حکم کارمند
		public string? HR_CVR_TypesRulingsId { get; set; }

		// نوع بیمه
		public string? HR_Base_InsuranceTypesId { get; set; }

		// نوع پرداخت آکورد
		public string? TypeBonusPayment { get; set; }

		// وضعیت حکم
		public string? HR_StatusVerdictRecruitingId { get; set; }

		// عنوان قسمت های سازمانی در جدول پست های حکم
		public string? HR_ORG_SectionsId { get; set; }
		//public string? SectionId { get; set; }

		// عنوان بخش
		public string? SectionTitle { get; set; }

		// نوع قسمت در جدول پست های حکم
		public bool? SectionsType { get; set; }

		// عنوان پست سازمانی در جدول پست های حکم
		public string? HR_ORG_PostsId { get; set; }

		// عنوان پست
		public string? PostTitle { get; set; }

		// نوع پست
		public bool? PostType { get; set; }

		//  تاریخ اجرای حکم
		public string? ExecutionDateSentence_Fa { get; set; } // شمسی
		public DateOnly? ExecutionDateSentence { get; set; } // میلادی

		// تعداد فرزند
		public int? CountChilderen { get; set; }
		// تاریخ برقراری حق اولاد
		public string? StartChildRightsGroupDate_Fa { get; set; } // شمسی
	}

	public class VerdictRequest
	{
		// بخش اصلی 
		// شناسه کارمند
		public Guid EmployeesId { get; set; }

		// نوع حکم
		public string? HR_CVR_TypesRulingsId { get; set; }

		// نوع پرداخت آکورد
		public string? TypeBonusPayment { get; set; }

		// وضعیت حکم
		public string? HR_StatusVerdictRecruitingId { get; set; }

		// **************************************************
		// بخش محاسبات حقوق حکم

		// مزد شغل گروه
		public decimal? JobSalaryRank { get; set; }

		// مزد رتبه
		public decimal? RankSalary { get; set; }

		// مزد سنوات
		public decimal? SalaryHistory { get; set; }

		// حق سرپرستی (پست)
		public decimal? RightGuardianship { get; set; }

		// مزایای ماندگاری پست
		public decimal? CoefficientDurabilityPost { get; set; }

		// شرایط نامساعد محیط کار
		public decimal? CoefficientDifficultAndHarmfulJobs { get; set; }

		// جمع مزد مبنای روزانه
		public decimal? TotalDailyBaseWage { get; set; }

		// **************************************************

		// تفاوت تطبیق روزانه
		public decimal? DailyAdjustmentDifference { get; set; }

		// حق جذب
		public decimal? RecruitmentAllowance { get; set; }

		// کمک هزینه مسکن
		public decimal? MinistryLabourRightHousing { get; set; }

		// حق خوار و بار
		public decimal? MinistryLaborRightFood { get; set; }

		// حق اولاد
		public decimal? ChildrensRightsMinistryLabor { get; set; }

		// مزایای رفاهی و انگیزه‌ای
		public decimal? WelfareMotivationalBenefits { get; set; }

		// حق تاهل
		public decimal? RightMarryMinistryLabor { get; set; }

		// سایر مزایا
		public decimal? OtherBenefits { get; set; }

		// جمع کل دستمزد و مزایا
		public decimal? TotalMonthlySalaryBenefits { get; set; }


		// **************************************************
		// بخش پست های هر حکم
		// قسمت‌های سازمانی
		public string? HR_ORG_SectionsId { get; set; }

		// نوع قسمت
		public bool? SectionsType { get; set; }

		// پست‌های سازمانی
		public string? HR_ORG_PostsId { get; set; }

		// نوع پست
		public bool? PostType { get; set; }

		// تاریخ اجرای حکم
		public string? ExecutionDateSentence_Fa { get; set; } //  شمسی
		public DateOnly? ExecutionDateSentence { get; set; } // میلادی

		// نوع بیمه کارمند
		public string? HR_Base_InsuranceTypesId { get; set; } //  شمسی

		// ********************************************************

		public decimal? JobSalaryRankNew { get; set; }
		public decimal? RankSalaryNew { get; set; }
		public decimal? SalaryHistoryNew { get; set; }
		public decimal? RightGuardianshipNew { get; set; }
		public decimal? CoefficientDurabilityPostNew { get; set; }
		public decimal? CoefficientDifficultAndHarmfulJobsNew { get; set; }
		public decimal? TotalDailyBaseWageNew { get; set; }
		public decimal? DailyAdjustmentDifferenceNew { get; set; }
		public decimal? RecruitmentAllowanceNew { get; set; }
		public decimal? MinistryLabourRightHousingNew { get; set; }
		public decimal? MinistryLaborRightFoodNew { get; set; }
		public decimal? ChildrensRightsMinistryLaborNew { get; set; }
		public decimal? WelfareMotivationalBenefitsNew { get; set; }
		public decimal? RightMarryMinistryLaborNew { get; set; }
		public decimal? OtherBenefitsNew { get; set; }
		public decimal? TotalMonthlySalaryBenefitsNew { get; set; }

		// **********
		// درصد ثابت افزایش مزد مبنا سالانه وزارت کار
		public int? IncreasePercentGroup { get; set; }
		// درصد ثابت افزایش مزد مبنا وزارت کار
		public string? HR_CVR_AnnualBaseWageIncreaseRatesId { get; set; }

		// ضریب ريالی تعیین شده سالانه شورای عالی وزارت کار
		public int? FixedNumberValueGroup { get; set; }
		// شناسه - ضریب ريالی تعیین شده سالانه شورای عالی وزارت کار
		public string? HR_CVR_LaborCouncilFixedValuesId { get; set; }

		// **********
	}
}

#endregion