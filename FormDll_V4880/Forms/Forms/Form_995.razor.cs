using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using Baya.Models.ORM;
using Utility;
namespace Forms.Forms
{
    public class Form_995Base : Form_995Peropeties
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

            var List = _Entity.SCM_ProductRequestDetails.ToList();

            foreach (var Item in List)
            {
                IsValid = IsValid && await CheckFieldValidation(Item);
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

        public async Task<bool> CheckFieldValidation(Entity.SCM_ProductRequestDetails Item)
        {
            bool IsValid = true;

            var List = _Entity.SCM_ProductRequestDetails.ToList();

            if (Item.FB_FACTNO_GUID == null)
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

            // تعداد یا مقدار واگذاری کالا
            if (Item.NumberofProductDelivery == null)
            {
                IsValid = false;
                toastService.ShowError("لطفا گزینه تعداد یا مقدار واگذاری کالا را تکمیل نمایید.",
                settings => {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }

            return IsValid;
        }



        // عطف رسید انبار در شماران سیستم 
        public async Task ResidAnbarIsVisible(bool Visible)
        {
            // Console.WriteLine("#Log 3");
            // Console.WriteLine("#Log 3.1" + Ref_SCM_ProductRequestDetails_FB_Search_NotMapped.Value);
            await Task.Delay(200);

            Ref_SCM_ProductRequestDetails_FB_Search_NotMapped.SetVisible(Visible);
            Ref_SCM_ProductRequestDetails_FB_FACTNO_GUID.SetVisible(Visible);
            Ref_SCM_ProductRequestDetails_FB_FactorNum.SetVisible(Visible);
            Ref_SCM_ProductRequestDetails_FB_WUSER.SetVisible(Visible);
            Ref_SCM_ProductRequestDetails_FB_YEAR.SetVisible(Visible);
            Ref_SCM_ProductRequestDetails_SH_FactBuy_DTL.SetVisible(Visible);

            // Ref_SCM_ProductRequestDetails_FB_ACCCODE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_ACTCODE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_ACTYEAR.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_ADD1.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_ADD2.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_ADD3.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_ADD4.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_AMOUNT.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_ARRNO.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_ARRYEAR.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_BAARNAME.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_BABCODE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_BRANCHCODE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_BUYKIND.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_C1901.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_C1950.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CAR_TYPE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CASHCODE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CASHKIND.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_COMP_NAME.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CPRICE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CREATOR.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO1.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO2.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO3.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO4.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO5.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO6.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO7.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO8.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO9.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO10.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO1.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO2.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO3.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO4.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO5.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO6.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO7.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO8.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO9.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO10.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CURFACT.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CURKIND.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_CURPKID.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_C_CURVAL.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_C_DEDUCE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_C_OTHERPAY.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_C_PARPRICE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_C_PREPRICE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_C_WELLDONE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_DEDUCE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_DOC_ID.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_DOC_ID2.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_DOC_YEAR.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_DOC_YEAR2.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_DRV_CARTNO.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_DRV_NAME.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_ELAMNO.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_FACTDATE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_FACTNO.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_FACTOR.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_FACTPOS.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_FACTSELER.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_FDATE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_FID.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_FORMCODE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_FORMYEAR.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_HAZCODE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_INVCODE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_MBCODE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_MBPROJCODE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_MBSBSTCODE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_NEW_CURVAL.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_NOSEFARESH.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_NOTE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_ORDERNO.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_ORDERYEAR.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_OTHERPAY.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_PAGE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_PARPRICE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_PARTKIND.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_PAYKNDCODE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_PELAK.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_PKIND.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_PRECODE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_PREDATE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_PREPRICE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_PRJMRDTLID.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_PROFORMCOD.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_PROJCODE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_PROJYEAR.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_PRO_YEAR.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_QC.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_RECNO.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_RESNO.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_ROW_ID3.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_SEFCODE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_SELERCODE.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_SELERNAME.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_SUB1.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_SUB2.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_SUB3.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_SUB4.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_SUMSAN.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE1.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE2.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE3.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE4.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE5.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE6.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE7.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO1.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO2.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO3.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO4.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO5.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO6.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO7.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_TANNO.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_USER.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_USERNAME.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_WDPERCENT.SetVisible(Visible);
            // Ref_SCM_ProductRequestDetails_FB_WELLDONE.SetVisible(Visible);

            Ref_SCM_ProductRequestDetails_FB_FACTNO_GUID.SetDisabled(true);
            Ref_SCM_ProductRequestDetails_FB_FactorNum.SetDisabled(true);
            Ref_SCM_ProductRequestDetails_FB_WUSER.SetDisabled(true);
            Ref_SCM_ProductRequestDetails_FB_YEAR.SetDisabled(true);

            // Ref_SCM_ProductRequestDetails_FB_ACCCODE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_ACTCODE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_ACTYEAR.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_ADD1.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_ADD2.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_ADD3.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_ADD4.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_AMOUNT.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_ARRNO.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_ARRYEAR.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_BAARNAME.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_BABCODE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_BRANCHCODE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_BUYKIND.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_C1901.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_C1950.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CAR_TYPE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CASHCODE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CASHKIND.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_COMP_NAME.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CPRICE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CREATOR.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO1.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO2.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO3.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO4.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO5.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO6.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO7.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO8.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO9.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO10.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO1.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO2.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO3.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO4.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO5.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO6.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO7.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO8.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO9.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CTFNO10.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CURFACT.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CURKIND.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_CURPKID.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_C_CURVAL.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_C_DEDUCE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_C_OTHERPAY.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_C_PARPRICE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_C_PREPRICE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_C_WELLDONE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_DEDUCE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_DOC_ID.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_DOC_ID2.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_DOC_YEAR.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_DOC_YEAR2.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_DRV_CARTNO.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_DRV_NAME.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_ELAMNO.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_FACTDATE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_FACTNO.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_FACTOR.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_FACTPOS.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_FACTSELER.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_FDATE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_FID.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_FORMCODE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_FORMYEAR.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_HAZCODE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_INVCODE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_MBCODE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_MBPROJCODE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_MBSBSTCODE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_NEW_CURVAL.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_NOSEFARESH.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_NOTE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_ORDERNO.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_ORDERYEAR.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_OTHERPAY.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_PAGE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_PARPRICE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_PARTKIND.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_PAYKNDCODE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_PELAK.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_PKIND.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_PRECODE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_PREDATE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_PREPRICE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_PRJMRDTLID.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_PROFORMCOD.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_PROJCODE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_PROJYEAR.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_PRO_YEAR.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_QC.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_RECNO.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_RESNO.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_ROW_ID3.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_SEFCODE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_SELERCODE.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_SELERNAME.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_SUB1.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_SUB2.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_SUB3.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_SUB4.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_SUMSAN.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE1.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE2.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE3.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE4.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE5.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE6.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE7.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO1.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO2.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO3.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO4.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO5.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO6.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO7.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_TANNO.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_USER.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_USERNAME.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_WDPERCENT.SetDisabled(true);
            // Ref_SCM_ProductRequestDetails_FB_WELLDONE.SetDisabled(true);

        }

        public async Task ResidAnbarIsNull()
        {
            Ref_SCM_ProductRequestDetails_FB_Search_NotMapped = null;
            Ref_SCM_ProductRequestDetails_FB_FACTNO_GUID = null;
            Ref_SCM_ProductRequestDetails_FB_FactorNum = null;
            Ref_SCM_ProductRequestDetails_FB_WUSER = null;
            Ref_SCM_ProductRequestDetails_FB_YEAR = null;
            Ref_SCM_ProductRequestDetails_SH_FactBuy_DTL = null;

            // Ref_SCM_ProductRequestDetails_FB_ACCCODE = null;
            // Ref_SCM_ProductRequestDetails_FB_ACTCODE = null;
            // Ref_SCM_ProductRequestDetails_FB_ACTYEAR = null;
            // Ref_SCM_ProductRequestDetails_FB_ADD1 = null;
            // Ref_SCM_ProductRequestDetails_FB_ADD2 = null;
            // Ref_SCM_ProductRequestDetails_FB_ADD3 = null;
            // Ref_SCM_ProductRequestDetails_FB_ADD4 = null;
            // Ref_SCM_ProductRequestDetails_FB_AMOUNT = null;
            // Ref_SCM_ProductRequestDetails_FB_ARRNO = null;
            // Ref_SCM_ProductRequestDetails_FB_ARRYEAR = null;
            // Ref_SCM_ProductRequestDetails_FB_BAARNAME = null;
            // Ref_SCM_ProductRequestDetails_FB_BABCODE = null;
            // Ref_SCM_ProductRequestDetails_FB_BRANCHCODE = null;
            // Ref_SCM_ProductRequestDetails_FB_BUYKIND = null;
            // Ref_SCM_ProductRequestDetails_FB_C1901 = null;
            // Ref_SCM_ProductRequestDetails_FB_C1950 = null;
            // Ref_SCM_ProductRequestDetails_FB_CAR_TYPE = null;
            // Ref_SCM_ProductRequestDetails_FB_CASHCODE = null;
            // Ref_SCM_ProductRequestDetails_FB_CASHKIND = null;
            // Ref_SCM_ProductRequestDetails_FB_COMP_NAME = null;
            // Ref_SCM_ProductRequestDetails_FB_CPRICE = null;
            // Ref_SCM_ProductRequestDetails_FB_CREATOR = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO1 = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO2 = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO3 = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO4 = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO5 = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO6 = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO7 = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO8 = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO9 = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFKNO10 = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFNO1 = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFNO2 = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFNO3 = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFNO4 = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFNO5 = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFNO6 = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFNO7 = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFNO8 = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFNO9 = null;
            // Ref_SCM_ProductRequestDetails_FB_CTFNO10 = null;
            // Ref_SCM_ProductRequestDetails_FB_CURFACT = null;
            // Ref_SCM_ProductRequestDetails_FB_CURKIND = null;
            // Ref_SCM_ProductRequestDetails_FB_CURPKID = null;
            // Ref_SCM_ProductRequestDetails_FB_C_CURVAL = null;
            // Ref_SCM_ProductRequestDetails_FB_C_DEDUCE = null;
            // Ref_SCM_ProductRequestDetails_FB_C_OTHERPAY = null;
            // Ref_SCM_ProductRequestDetails_FB_C_PARPRICE = null;
            // Ref_SCM_ProductRequestDetails_FB_C_PREPRICE = null;
            // Ref_SCM_ProductRequestDetails_FB_C_WELLDONE = null;
            // Ref_SCM_ProductRequestDetails_FB_DEDUCE = null;
            // Ref_SCM_ProductRequestDetails_FB_DOC_ID = null;
            // Ref_SCM_ProductRequestDetails_FB_DOC_ID2 = null;
            // Ref_SCM_ProductRequestDetails_FB_DOC_YEAR = null;
            // Ref_SCM_ProductRequestDetails_FB_DOC_YEAR2 = null;
            // Ref_SCM_ProductRequestDetails_FB_DRV_CARTNO = null;
            // Ref_SCM_ProductRequestDetails_FB_DRV_NAME = null;
            // Ref_SCM_ProductRequestDetails_FB_ELAMNO = null;
            // Ref_SCM_ProductRequestDetails_FB_FACTDATE = null;
            // Ref_SCM_ProductRequestDetails_FB_FACTNO = null;
            // Ref_SCM_ProductRequestDetails_FB_FACTOR = null;
            // Ref_SCM_ProductRequestDetails_FB_FACTPOS = null;
            // Ref_SCM_ProductRequestDetails_FB_FACTSELER = null;
            // Ref_SCM_ProductRequestDetails_FB_FDATE = null;
            // Ref_SCM_ProductRequestDetails_FB_FID = null;
            // Ref_SCM_ProductRequestDetails_FB_FORMCODE = null;
            // Ref_SCM_ProductRequestDetails_FB_FORMYEAR = null;
            // Ref_SCM_ProductRequestDetails_FB_HAZCODE = null;
            // Ref_SCM_ProductRequestDetails_FB_INVCODE = null;
            // Ref_SCM_ProductRequestDetails_FB_MBCODE = null;
            // Ref_SCM_ProductRequestDetails_FB_MBPROJCODE = null;
            // Ref_SCM_ProductRequestDetails_FB_MBSBSTCODE = null;
            // Ref_SCM_ProductRequestDetails_FB_NEW_CURVAL = null;
            // Ref_SCM_ProductRequestDetails_FB_NOSEFARESH = null;
            // Ref_SCM_ProductRequestDetails_FB_NOTE = null;
            // Ref_SCM_ProductRequestDetails_FB_ORDERNO = null;
            // Ref_SCM_ProductRequestDetails_FB_ORDERYEAR = null;
            // Ref_SCM_ProductRequestDetails_FB_OTHERPAY = null;
            // Ref_SCM_ProductRequestDetails_FB_PAGE = null;
            // Ref_SCM_ProductRequestDetails_FB_PARPRICE = null;
            // Ref_SCM_ProductRequestDetails_FB_PARTKIND = null;
            // Ref_SCM_ProductRequestDetails_FB_PAYKNDCODE = null;
            // Ref_SCM_ProductRequestDetails_FB_PELAK = null;
            // Ref_SCM_ProductRequestDetails_FB_PKIND = null;
            // Ref_SCM_ProductRequestDetails_FB_PRECODE = null;
            // Ref_SCM_ProductRequestDetails_FB_PREDATE = null;
            // Ref_SCM_ProductRequestDetails_FB_PREPRICE = null;
            // Ref_SCM_ProductRequestDetails_FB_PRJMRDTLID = null;
            // Ref_SCM_ProductRequestDetails_FB_PROFORMCOD = null;
            // Ref_SCM_ProductRequestDetails_FB_PROJCODE = null;
            // Ref_SCM_ProductRequestDetails_FB_PROJYEAR = null;
            // Ref_SCM_ProductRequestDetails_FB_PRO_YEAR = null;
            // Ref_SCM_ProductRequestDetails_FB_QC = null;
            // Ref_SCM_ProductRequestDetails_FB_RECNO = null;
            // Ref_SCM_ProductRequestDetails_FB_RESNO = null;
            // Ref_SCM_ProductRequestDetails_FB_ROW_ID3 = null;
            // Ref_SCM_ProductRequestDetails_FB_SEFCODE = null;
            // Ref_SCM_ProductRequestDetails_FB_SELERCODE = null;
            // Ref_SCM_ProductRequestDetails_FB_SELERNAME = null;
            // Ref_SCM_ProductRequestDetails_FB_SUB1 = null;
            // Ref_SCM_ProductRequestDetails_FB_SUB2 = null;
            // Ref_SCM_ProductRequestDetails_FB_SUB3 = null;
            // Ref_SCM_ProductRequestDetails_FB_SUB4 = null;
            // Ref_SCM_ProductRequestDetails_FB_SUMSAN = null;
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE1 = null;
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE2 = null;
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE3 = null;
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE4 = null;
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE5 = null;
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE6 = null;
            // Ref_SCM_ProductRequestDetails_FB_TAFCODE7 = null;
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO1 = null;
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO2 = null;
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO3 = null;
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO4 = null;
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO5 = null;
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO6 = null;
            // Ref_SCM_ProductRequestDetails_FB_TAFKINDNO7 = null;
            // Ref_SCM_ProductRequestDetails_FB_TANNO = null;
            // Ref_SCM_ProductRequestDetails_FB_USER = null;
            // Ref_SCM_ProductRequestDetails_FB_USERNAME = null;
            // Ref_SCM_ProductRequestDetails_FB_WDPERCENT = null;
            // Ref_SCM_ProductRequestDetails_FB_WELLDONE = null;

        }

        public async Task ResidAnbarSetShomaran(dynamic Selected, Entity.SCM_ProductRequestDetails Item)
        {
            //Console.WriteLine("#Log 4");
            //Console.WriteLine(await Utility.JSON.ToJson(Selected));
            // Console.WriteLine(Selected.FACTNO_GUID);
            //try
            //{
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(await Utility.JSON.ToJson(ex));
            //}


            Item.FB_FACTNO_GUID = Selected.FACTNO_GUID;
            Item.FB_FactorNum = Selected.FactorNum;
            Item.FB_WUSER = Selected.WUSER;
            Item.FB_YEAR = Selected.YEAR;

            //Item.FB_ACCCODE = Selected.ACCCODE;
            //Item.FB_ACTCODE = Selected.ACTCODE;
            // Item.FB_ACTYEAR = Selected.ACTYEAR;
            // Item.FB_ADD1 = Selected.ADD1;
            // Item.FB_ADD2 = Selected.ADD2;
            // Item.FB_ADD3 = Selected.ADD3;
            // Item.FB_ADD4 = Selected.ADD4;
            // Item.FB_AMOUNT = Selected.AMOUNT;
            // Item.FB_ARRNO = Selected.ARRNO;
            // Item.FB_ARRYEAR = Selected.ARRYEAR;
            // Item.FB_BAARNAME = Selected.BAARNAME;
            // Item.FB_BABCODE = Selected.BABCODE;
            // Item.FB_BRANCHCODE = Selected.BRANCHCODE;
            // Item.FB_BUYKIND = Selected.BUYKIND;
            // Item.FB_C1901 = Selected.C1901;
            // Item.FB_C1950 = Selected.C1950;
            // Item.FB_CAR_TYPE = Selected.CAR_TYPE;
            // Item.FB_CASHCODE = Selected.CASHCODE;
            // Item.FB_CASHKIND = Selected.CASHKIND;
            // Item.FB_COMP_NAME = Selected.COMP_NAME;
            // Item.FB_CPRICE = Selected.CPRICE;
            // Item.FB_CREATOR = Selected.CREATOR;
            // Item.FB_CTFKNO1 = Selected.CTFKNO1;
            // Item.FB_CTFKNO2 = Selected.CTFKNO2;
            // Item.FB_CTFKNO3 = Selected.CTFKNO3;
            // Item.FB_CTFKNO4 = Selected.CTFKNO4;
            // Item.FB_CTFKNO5 = Selected.CTFKNO5;
            // Item.FB_CTFKNO6 = Selected.CTFKNO6;
            // Item.FB_CTFKNO7 = Selected.CTFKNO7;
            // Item.FB_CTFKNO8 = Selected.CTFKNO8;
            // Item.FB_CTFKNO9 = Selected.CTFKNO9;
            // Item.FB_CTFKNO10 = Selected.CTFKNO10;
            // Item.FB_CTFNO1 = Selected.CTFNO1;
            // Item.FB_CTFNO2 = Selected.CTFNO2;
            // Item.FB_CTFNO3 = Selected.CTFNO3;
            // Item.FB_CTFNO4 = Selected.CTFNO4;
            // Item.FB_CTFNO5 = Selected.CTFNO5;
            // Item.FB_CTFNO6 = Selected.CTFNO6;
            // Item.FB_CTFNO7 = Selected.CTFNO7;
            // Item.FB_CTFNO8 = Selected.CTFNO8;
            // Item.FB_CTFNO9 = Selected.CTFNO9;
            // Item.FB_CTFNO10 = Selected.CTFNO10;
            // Item.FB_CURFACT = Selected.CURFACT;
            // Item.FB_CURKIND = Selected.CURKIND;
            // Item.FB_CURPKID = Selected.CURPKID;
            // Item.FB_C_CURVAL = Selected.C_CURVAL;
            // Item.FB_C_DEDUCE = Selected.C_DEDUCE;
            // Item.FB_C_OTHERPAY = Selected.C_OTHERPAY;
            // Item.FB_C_PARPRICE = Selected.C_PARPRICE;
            // Item.FB_C_PREPRICE = Selected.C_PREPRICE;
            // Item.FB_C_WELLDONE = Selected.C_WELLDONE;
            // Item.FB_DEDUCE = Selected.DEDUCE;
            // Item.FB_DOC_ID = Selected.DOC_ID;
            // Item.FB_DOC_ID2 = Selected.DOC_ID2;
            // Item.FB_DOC_YEAR = Selected.DOC_YEAR;
            // Item.FB_DOC_YEAR2 = Selected.DOC_YEAR2;
            // Item.FB_DRV_CARTNO = Selected.DRV_CARTNO;
            // Item.FB_DRV_NAME = Selected.DRV_NAME;
            // Item.FB_ELAMNO = Selected.ELAMNO;
            // Item.FB_FACTDATE = Selected.FACTDATE;
            // Item.FB_FACTNO = Selected.FACTNO;
            // Item.FB_FACTOR = Selected.FACTOR;
            // Item.FB_FACTPOS = Selected.FACTPOS;
            // Item.FB_FACTSELER = Selected.FACTSELER;
            // Item.FB_FDATE = Selected.FDATE;
            // Item.FB_FID = Selected.FID;
            // Item.FB_FORMCODE = Selected.FORMCODE;
            // Item.FB_FORMYEAR = Selected.FORMYEAR;
            // Item.FB_HAZCODE = Selected.HAZCODE;
            // Item.FB_INVCODE = Selected.INVCODE;
            // Item.FB_MBCODE = Selected.MBCODE;
            // Item.FB_MBPROJCODE = Selected.MBPROJCODE;
            // Item.FB_MBSBSTCODE = Selected.MBSBSTCODE;
            // Item.FB_NEW_CURVAL = Selected.NEW_CURVAL;
            // Item.FB_NOSEFARESH = Selected.NOSEFARESH;
            // Item.FB_NOTE = Selected.NOTE;
            // Item.FB_ORDERNO = Selected.ORDERNO;
            // Item.FB_ORDERYEAR = Selected.ORDERYEAR;
            // Item.FB_OTHERPAY = Selected.OTHERPAY;
            // Item.FB_PAGE = Selected.PAGE;
            // Item.FB_PARPRICE = Selected.PARPRICE;
            // Item.FB_PARTKIND = Selected.PARTKIND;
            // Item.FB_PAYKNDCODE = Selected.PAYKNDCODE;
            // Item.FB_PELAK = Selected.PELAK;
            // Item.FB_PKIND = Selected.PKIND;
            // Item.FB_PRECODE = Selected.PRECODE;
            // Item.FB_PREDATE = Selected.PREDATE;
            // Item.FB_PREPRICE = Selected.PREPRICE;
            // Item.FB_PRJMRDTLID = Selected.PRJMRDTLID;
            // Item.FB_PROFORMCOD = Selected.PROFORMCOD;
            // Item.FB_PROJCODE = Selected.PROJCODE;
            // Item.FB_PROJYEAR = Selected.PROJYEAR;
            // Item.FB_PRO_YEAR = Selected.PRO_YEAR;
            // Item.FB_QC = Selected.QC;
            // Item.FB_RECNO = Selected.RECNO;
            // Item.FB_RESNO = Selected.RESNO;
            // Item.FB_ROW_ID3 = Selected.ROW_ID3;
            // Item.FB_SEFCODE = Selected.SEFCODE;
            // Item.FB_SELERCODE = Selected.SELERCODE;
            // Item.FB_SELERNAME = Selected.SELERNAME;
            // Item.FB_SUB1 = Selected.SUB1;
            // Item.FB_SUB2 = Selected.SUB2;
            // Item.FB_SUB3 = Selected.SUB3;
            // Item.FB_SUB4 = Selected.SUB4;
            // Item.FB_SUMSAN = Selected.SUMSAN;
            // Item.FB_TAFCODE1 = Selected.TAFCODE1;
            // Item.FB_TAFCODE2 = Selected.TAFCODE2;
            // Item.FB_TAFCODE3 = Selected.TAFCODE3;
            // Item.FB_TAFCODE4 = Selected.TAFCODE4;
            // Item.FB_TAFCODE5 = Selected.TAFCODE5;
            // Item.FB_TAFCODE6 = Selected.TAFCODE6;
            // Item.FB_TAFCODE7 = Selected.TAFCODE7;
            // Item.FB_TAFKINDNO1 = Selected.TAFKINDNO1;
            // Item.FB_TAFKINDNO2 = Selected.TAFKINDNO2;
            // Item.FB_TAFKINDNO3 = Selected.TAFKINDNO3;
            // Item.FB_TAFKINDNO4 = Selected.TAFKINDNO4;
            // Item.FB_TAFKINDNO5 = Selected.TAFKINDNO5;
            // Item.FB_TAFKINDNO6 = Selected.TAFKINDNO6;
            // Item.FB_TAFKINDNO7 = Selected.TAFKINDNO7;
            // Item.FB_TANNO = Selected.TANNO;
            // Item.FB_USER = Selected.USER;
            // Item.FB_USERNAME = Selected.USERNAME;
            // Item.FB_WDPERCENT = Selected.WDPERCENT;
            // Item.FB_WELLDONE = Selected.WELLDONE;


            //	فراخوانی داده از dropdown TempNoNum برای گرید داده های آن
            Ref_SCM_ProductRequestDetails_SH_FactBuy_DTL.SetEntity(Item);
            Ref_SCM_ProductRequestDetails_SH_FactBuy_DTL.LoadData();

        }

        public async Task<bool> SCM_ProductRequestDetails_editmodelsaving(object e)
        {
            bool IsCancelled = false;
            var Item = (Entity.SCM_ProductRequestDetails)e;

            IsCancelled = !await CheckFieldValidation(Item);

            return IsCancelled;
        }
        public async Task SCM_ProductRequestDetails_afterrendermodal(Entity.SCM_ProductRequestDetails Item)
        {
            var NDL1IsVisible = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics;
            var NDL2IsVisible = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2;
            var NDL3IsVisible = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3;

            // فیلدهای انبار - 1 2 3
            var NDL1_Anabar_IsVisible = Ref_SCM_ProductRequestDetails_NumberofProductDelivery;
            var NDL2_Anabar_IsVisible = Ref_SCM_ProductRequestDetails_NumberofProductDelivery2;
            var NDL3_Anabar_IsVisible = Ref_SCM_ProductRequestDetails_NumberofProductDelivery3;

            // Console.WriteLine((Selected.Id.ToString() == "82329c07-2aa7-ef11-8354-005056a02a64").ToString());
            // Console.WriteLine((Selected.Id.ToString() == "cc59710f-2aa7-ef11-8354-005056a02a64").ToString());
            // Console.WriteLine((Selected.Id.ToString() == "13a46c17-2aa7-ef11-8354-005056a02a64").ToString());
            // نمایش تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 
            if (Item.SCM_NumTransfersGoodsWarehouseId.HasValue && Item.SCM_NumTransfersGoodsWarehouseId.ToString() == "82329c07-2aa7-ef11-8354-005056a02a64")
            {
                NDL1IsVisible.SetVisible(true);
                NDL1_Anabar_IsVisible.SetVisible(true);

                NDL2IsVisible.SetVisible(false);
                NDL2_Anabar_IsVisible.SetVisible(false);

                NDL3IsVisible.SetVisible(false);
                NDL3_Anabar_IsVisible.SetVisible(false);
            }
            else if (Item.SCM_NumTransfersGoodsWarehouseId.HasValue && Item.SCM_NumTransfersGoodsWarehouseId.ToString() == "cc59710f-2aa7-ef11-8354-005056a02a64")
            {
                NDL1IsVisible.SetVisible(true);
                NDL1_Anabar_IsVisible.SetVisible(true);

                NDL2IsVisible.SetVisible(true);
                NDL2_Anabar_IsVisible.SetVisible(true);

                NDL3IsVisible.SetVisible(false);
                NDL3_Anabar_IsVisible.SetVisible(false);
            }
            else if (Item.SCM_NumTransfersGoodsWarehouseId.ToString() == "13a46c17-2aa7-ef11-8354-005056a02a64")
            {
                NDL1IsVisible.SetVisible(true);
                NDL1_Anabar_IsVisible.SetVisible(true);

                NDL2IsVisible.SetVisible(true);
                NDL2_Anabar_IsVisible.SetVisible(true);

                NDL3IsVisible.SetVisible(true);
                NDL3_Anabar_IsVisible.SetVisible(true);
            }
            // مخفی کردن فیلد تعداد یا مقدار مازاد
            if (Item.SurplusProductIsEnable.HasValue && Item.SurplusProductIsEnable.Value)
            {
                Ref_SCM_ProductRequestDetails_NumberOfSurplusProduct.SetVisible(true);
            }
            else
            {
                Ref_SCM_ProductRequestDetails_NumberOfSurplusProduct.SetVisible(false);
            }


            // عطف رسید انبار  
            if (Item.FB_FACTNO_GUID.HasValue)
            {
                Ref_SCM_ProductRequestDetails_SH_FactBuy_DTL.SetEntity(Item);
                Ref_SCM_ProductRequestDetails_SH_FactBuy_DTL.LoadData();
            }
        }

        public async Task SCM_NumTransfersGoodsWarehouseId_onitemselected(Entity.SCM_NumTransfersGoodsWarehouse Selected, Entity.SCM_ProductRequestDetails Item)
        {
            // 82329c07-2aa7-ef11-8354-005056a02a64     یک مرحله
            // cc59710f-2aa7-ef11-8354-005056a02a64     دو مرحله
            // 13a46c17-2aa7-ef11-8354-005056a02a64     سه مرحله

            // **************************************************

            // تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات- TheNumberDeliveredByLogistics
            // تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 2- TheNumberDeliveredByLogistics2
            // تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 3- TheNumberDeliveredByLogistics3

            var NDL1IsVisible = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics;
            var NDL2IsVisible = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics2;
            var NDL3IsVisible = Ref_SCM_ProductRequestDetails_TheNumberDeliveredByLogistics3;

            // فیلدهای انبار - 1 2 3
            var NDL1_Anabar_IsVisible = Ref_SCM_ProductRequestDetails_NumberofProductDelivery;
            var NDL2_Anabar_IsVisible = Ref_SCM_ProductRequestDetails_NumberofProductDelivery2;
            var NDL3_Anabar_IsVisible = Ref_SCM_ProductRequestDetails_NumberofProductDelivery3;

            // Console.WriteLine((Selected.Id.ToString() == "82329c07-2aa7-ef11-8354-005056a02a64").ToString());
            // Console.WriteLine((Selected.Id.ToString() == "cc59710f-2aa7-ef11-8354-005056a02a64").ToString());
            // Console.WriteLine((Selected.Id.ToString() == "13a46c17-2aa7-ef11-8354-005056a02a64").ToString());
            // نمایش تعداد یا مقدار واگذاری کالا به انبار توسط تدارکات 
            if (Selected.Id.ToString() == "82329c07-2aa7-ef11-8354-005056a02a64")
            {
                NDL1IsVisible.SetVisible(true);
                NDL1_Anabar_IsVisible.SetVisible(true);

                NDL2IsVisible.SetVisible(false);
                NDL2_Anabar_IsVisible.SetVisible(false);

                NDL3IsVisible.SetVisible(false);
                NDL3_Anabar_IsVisible.SetVisible(false);
            }
            else if (Selected.Id.ToString() == "cc59710f-2aa7-ef11-8354-005056a02a64")
            {
                NDL1IsVisible.SetVisible(true);
                NDL1_Anabar_IsVisible.SetVisible(true);

                NDL2IsVisible.SetVisible(true);
                NDL2_Anabar_IsVisible.SetVisible(true);

                NDL3IsVisible.SetVisible(false);
                NDL3_Anabar_IsVisible.SetVisible(false);
            }
            else if (Selected.Id.ToString() == "13a46c17-2aa7-ef11-8354-005056a02a64")
            {
                NDL1IsVisible.SetVisible(true);
                NDL1_Anabar_IsVisible.SetVisible(true);

                NDL2IsVisible.SetVisible(true);
                NDL2_Anabar_IsVisible.SetVisible(true);

                NDL3IsVisible.SetVisible(true);
                NDL3_Anabar_IsVisible.SetVisible(true);
            }
        }

        public async Task SurplusProductIsEnable_oninput(ChangeEventArgs Selected, Entity.SCM_ProductRequestDetails Item)
        {

            // Note: آیا خرید تعداد /مقدار مازاد دارد؟
            var Item2 = Ref_SCM_ProductRequestDetails_NumberOfSurplusProduct;

            if (Selected.Value.ToString() == "true")
            {
                Item2.SetVisible(true);
            }
            else
            {
                Item2.SetVisible(false);
            }
        }

        public async Task FB_Search_NotMapped_onitemselected(dynamic Selected, Entity.SCM_ProductRequestDetails Item)
        {
            await ResidAnbarSetShomaran(Selected, Item);
        }

        public async Task submit_onclick(MouseEventArgs Selected)
        {
            //** رسید

            Table table2 = new Table()
            {
                Name = "SH_PolFilm_FACTBUY",
                Column = new List<Coulmn>()
                {
                    new Coulmn() { Name = "Id" },
                    new Coulmn() { Name = "RequestID" },
                    new Coulmn() { Name = "CreateUser" },
                    new Coulmn() { Name = "UpdateUser" },
                    new Coulmn() { Name = "CreateDate" },
                    new Coulmn() { Name = "UpdateDate" },
                    new Coulmn() { Name = "IsDelete" },
                    new Coulmn() { Name = "BUYKIND" },
                    new Coulmn() { Name = "CREATOR" },
                    new Coulmn() { Name = "FACTDATE" },
                    new Coulmn() { Name = "FACTOR" },
                    new Coulmn() { Name = "INVCODE" },
                    new Coulmn() { Name = "ORDERNO" },
                    new Coulmn() { Name = "ORDERYEAR" },
                    new Coulmn() { Name = "PARTKIND" },
                    new Coulmn() { Name = "SELERCODE" },
                    new Coulmn() { Name = "SELERNAME" },
                    new Coulmn() { Name = "SpUser" },
                    new Coulmn() { Name = "TEMPNO" },
                    new Coulmn() { Name = "NOTE" },
                    new Coulmn() { Name = "WUSER" },
                    new Coulmn() { Name = "YEAR" },
                    new Coulmn() { Name = "FACTNO" },

                },
                Relation = new List<Table>()
                {
                    new Table()
                    {
                        Name = "SH_PolFilm_AnbordDetail",
                        Column = new List<Coulmn>()
                        {
                            new Coulmn() { Name = "Id" },
                            new Coulmn() { Name = "RequestID" },
                            new Coulmn() { Name = "CreateUser" },
                            new Coulmn() { Name = "UpdateUser" },
                            new Coulmn() { Name = "CreateDate" },
                            new Coulmn() { Name = "UpdateDate" },
                            new Coulmn() { Name = "IsDelete" },
                            new Coulmn() { Name = "AMOUNT" },
                            new Coulmn() { Name = "AMOUNT2" },
                            new Coulmn() { Name = "PARTCODE" },
                            new Coulmn() { Name = "RADYABI" },
                            new Coulmn() { Name = "SEFARESH" },
                            new Coulmn() { Name = "ROW_ID" },
                            new Coulmn() { Name = "YEAR" },
                            new Coulmn() { Name = "NoteD" },
                        },
                        //ModeErtebat = ModeErtebat._N1
                    }
                },
            };
            var dataList2 = new List<Entity.SH_PolFilm_FactBuyDetail>();
            var data2 = new Entity.SH_PolFilm_FACTBUY();
            data2.FACTNO = " ";//_Entity.CENTCODE;

            // باید فیلتر شود بر اساس فقط دیتاهای تحویل
            foreach (var item in _Entity.SCM_ProductRequestDetails)
            {
                Console.WriteLine(item.PARTCODE);
                var d = new Entity.SH_PolFilm_FactBuyDetail();
                d.PARTCODE = item.PARTCODE;
                d.RADYABI = " ";
                d.SEFARESH = " ";
                d.AMOUNT = Convert.ToDecimal(item.ProductRequestingQTY);
                //d. = item.;
                dataList2.Add(d);
            }
            data2.SH_PolFilm_FactBuyDetail = dataList2;
            string sData2 = await JSON.ToJson(data2);
            var DataResult2 = await ApiServer.External.Services.Data.Put(sData2, "SH_PolFilm_FACTBUY", table2, null, _User.UserId);


        }

        #endregion FunctionEvents

    }
}
