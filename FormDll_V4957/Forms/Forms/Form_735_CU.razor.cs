using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using Blazored.Toast.Services;
using System.Globalization;
using System.ComponentModel;

namespace Forms.Forms
{
    public class Form_735_CUBase : Form_735_CUPeropeties
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

#region MSG 

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

#endregion / MSG

#region ConvertDateTime
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
