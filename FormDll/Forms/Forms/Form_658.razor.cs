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
    public class Form_658Base : Form_658Peropeties
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
            // foreach(var Item in _Entity.SCMPETCO_ProductRequestDetails)
            // {
            // 	Item.T_FACTNO_GUID=new Guid("00000000-0000-0000-0000-353030303030");
            // }
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

            var List = _Entity.SCMPETCO_ProductRequestDetails.ToList();



            if (BtnWorkFlowId == "SequenceFlow_1dquv2y")
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
            foreach (var item in _Entity.SCMPETCO_ProductRequestDetails)
            {
                item.EnableLaterPurchace = true;
            }
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
        public async Task<bool> CheckFieldValidation(Entity.SCMPETCO_ProductRequestDetails Item)
        {
            bool IsValid = true;

            var List = _Entity.SCMPETCO_ProductRequestDetails.ToList();

            if(Item.FB_FACTNO_GUID == null)
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

            if(Item.IsPurchasedItemDelivered == null)
            {
                IsValid = false;
                toastService.ShowError("لطفا گزینه کالای خریداری شده تحویل گردد ؟ را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
            }
                // 

            return IsValid;
        }


        // عطف رسید انبار در شماران سیستم 
        public async Task ResidAnbarIsVisible(bool Visible)
        {
            // //Console.WriteLine("#Log 3");
            // //Console.WriteLine("#Log 3.1" + Ref_SCMPETCO_ProductRequestDetails_FB_Search_NotMapped.Value);
            await Task.Delay(200);

            Ref_SCMPETCO_ProductRequestDetails_FB_Search_NotMapped.SetVisible(Visible);

            Ref_SCMPETCO_ProductRequestDetails_FB_ACCCODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_ACTCODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_ACTYEAR.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_ADD1.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_ADD2.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_ADD3.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_ADD4.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_AMOUNT.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_ARRNO.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_ARRYEAR.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_BAARNAME.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_BABCODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_BRANCHCODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_BUYKIND.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_C1901.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_C1950.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CAR_TYPE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CASHCODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CASHKIND.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_COMP_NAME.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CPRICE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CREATOR.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO1.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO2.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO3.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO4.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO5.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO6.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO7.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO8.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO9.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO10.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO1.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO2.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO3.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO4.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO5.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO6.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO7.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO8.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO9.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO10.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CURFACT.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CURKIND.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_CURPKID.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_C_CURVAL.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_C_DEDUCE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_C_OTHERPAY.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_C_PARPRICE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_C_PREPRICE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_C_WELLDONE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_DEDUCE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_DOC_ID.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_DOC_ID2.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_DOC_YEAR.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_DOC_YEAR2.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_DRV_CARTNO.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_DRV_NAME.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_ELAMNO.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_FACTDATE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_FACTNO.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_FACTOR.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_FACTPOS.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_FACTSELER.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_FDATE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_FID.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_FORMCODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_FORMYEAR.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_HAZCODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_INVCODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_MBCODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_MBPROJCODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_MBSBSTCODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_NEW_CURVAL.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_NOSEFARESH.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_NOTE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_ORDERNO.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_ORDERYEAR.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_OTHERPAY.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_PAGE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_PARPRICE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_PARTKIND.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_PAYKNDCODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_PELAK.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_PKIND.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_PRECODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_PREDATE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_PREPRICE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_PRJMRDTLID.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_PROFORMCOD.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_PROJCODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_PROJYEAR.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_PRO_YEAR.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_QC.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_RECNO.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_RESNO.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_ROW_ID3.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_SEFCODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_SELERCODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_SELERNAME.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_SUB1.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_SUB2.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_SUB3.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_SUB4.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_SUMSAN.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE1.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE2.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE3.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE4.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE5.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE6.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE7.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO1.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO2.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO3.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO4.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO5.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO6.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO7.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_TANNO.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_USER.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_USERNAME.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_WDPERCENT.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_WELLDONE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_WUSER.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_YEAR.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_FACTNO_GUID.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_FB_FactorNum.SetVisible(Visible);

            Ref_SCMPETCO_ProductRequestDetails_SH_FactBuy_DTL.SetVisible(Visible);

            Ref_SCMPETCO_ProductRequestDetails_FB_ACCCODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_ACTCODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_ACTYEAR.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_ADD1.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_ADD2.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_ADD3.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_ADD4.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_AMOUNT.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_ARRNO.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_ARRYEAR.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_BAARNAME.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_BABCODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_BRANCHCODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_BUYKIND.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_C1901.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_C1950.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CAR_TYPE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CASHCODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CASHKIND.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_COMP_NAME.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CPRICE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CREATOR.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO1.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO2.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO3.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO4.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO5.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO6.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO7.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO8.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO9.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFKNO10.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO1.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO2.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO3.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO4.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO5.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO6.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO7.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO8.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO9.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CTFNO10.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CURFACT.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CURKIND.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_CURPKID.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_C_CURVAL.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_C_DEDUCE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_C_OTHERPAY.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_C_PARPRICE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_C_PREPRICE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_C_WELLDONE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_DEDUCE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_DOC_ID.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_DOC_ID2.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_DOC_YEAR.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_DOC_YEAR2.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_DRV_CARTNO.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_DRV_NAME.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_ELAMNO.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_FACTDATE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_FACTNO.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_FACTOR.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_FACTPOS.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_FACTSELER.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_FDATE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_FID.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_FORMCODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_FORMYEAR.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_HAZCODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_INVCODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_MBCODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_MBPROJCODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_MBSBSTCODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_NEW_CURVAL.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_NOSEFARESH.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_NOTE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_ORDERNO.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_ORDERYEAR.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_OTHERPAY.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_PAGE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_PARPRICE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_PARTKIND.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_PAYKNDCODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_PELAK.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_PKIND.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_PRECODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_PREDATE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_PREPRICE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_PRJMRDTLID.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_PROFORMCOD.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_PROJCODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_PROJYEAR.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_PRO_YEAR.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_QC.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_RECNO.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_RESNO.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_ROW_ID3.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_SEFCODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_SELERCODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_SELERNAME.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_SUB1.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_SUB2.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_SUB3.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_SUB4.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_SUMSAN.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE1.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE2.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE3.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE4.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE5.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE6.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFCODE7.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO1.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO2.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO3.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO4.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO5.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO6.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_TAFKINDNO7.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_TANNO.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_USER.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_USERNAME.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_WDPERCENT.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_WELLDONE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_WUSER.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_YEAR.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_FACTNO_GUID.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_FB_FactorNum.SetDisabled(true);
        }

        public async Task ResidAnbarIsNull(Entity.SCMPETCO_ProductRequestDetails Item)
        {
            Item.FB_ACCCODE = null;
            Item.FB_ACTCODE = null;
            Item.FB_ACTYEAR = null;
            Item.FB_ADD1 = null;
            Item.FB_ADD2 = null;
            Item.FB_ADD3 = null;
            Item.FB_ADD4 = null;
            Item.FB_AMOUNT = null;
            Item.FB_ARRNO = null;
            Item.FB_ARRYEAR = null;
            Item.FB_BAARNAME = null;
            Item.FB_BABCODE = null;
            Item.FB_BRANCHCODE = null;
            Item.FB_BUYKIND = null;
            Item.FB_C1901 = null;
            Item.FB_C1950 = null;
            Item.FB_CAR_TYPE = null;
            Item.FB_CASHCODE = null;
            Item.FB_CASHKIND = null;
            Item.FB_COMP_NAME = null;
            Item.FB_CPRICE = null;
            Item.FB_CREATOR = null;
            Item.FB_CTFKNO1 = null;
            Item.FB_CTFKNO2 = null;
            Item.FB_CTFKNO3 = null;
            Item.FB_CTFKNO4 = null;
            Item.FB_CTFKNO5 = null;
            Item.FB_CTFKNO6 = null;
            Item.FB_CTFKNO7 = null;
            Item.FB_CTFKNO8 = null;
            Item.FB_CTFKNO9 = null;
            Item.FB_CTFKNO10 = null;
            Item.FB_CTFNO1 = null;
            Item.FB_CTFNO2 = null;
            Item.FB_CTFNO3 = null;
            Item.FB_CTFNO4 = null;
            Item.FB_CTFNO5 = null;
            Item.FB_CTFNO6 = null;
            Item.FB_CTFNO7 = null;
            Item.FB_CTFNO8 = null;
            Item.FB_CTFNO9 = null;
            Item.FB_CTFNO10 = null;
            Item.FB_CURFACT = null;
            Item.FB_CURKIND = null;
            Item.FB_CURPKID = null;
            Item.FB_C_CURVAL = null;
            Item.FB_C_DEDUCE = null;
            Item.FB_C_OTHERPAY = null;
            Item.FB_C_PARPRICE = null;
            Item.FB_C_PREPRICE = null;
            Item.FB_C_WELLDONE = null;
            Item.FB_DEDUCE = null;
            Item.FB_DOC_ID = null;
            Item.FB_DOC_ID2 = null;
            Item.FB_DOC_YEAR = null;
            Item.FB_DOC_YEAR2 = null;
            Item.FB_DRV_CARTNO = null;
            Item.FB_DRV_NAME = null;
            Item.FB_ELAMNO = null;
            Item.FB_FACTDATE = null;
            Item.FB_FACTNO = null;
            Item.FB_FACTOR = null;
            Item.FB_FACTPOS = null;
            Item.FB_FACTSELER = null;
            Item.FB_FDATE = null;
            Item.FB_FID = null;
            Item.FB_FORMCODE = null;
            Item.FB_FORMYEAR = null;
            Item.FB_HAZCODE = null;
            Item.FB_INVCODE = null;
            Item.FB_MBCODE = null;
            Item.FB_MBPROJCODE = null;
            Item.FB_MBSBSTCODE = null;
            Item.FB_NEW_CURVAL = null;
            Item.FB_NOSEFARESH = null;
            Item.FB_NOTE = null;
            Item.FB_ORDERNO = null;
            Item.FB_ORDERYEAR = null;
            Item.FB_OTHERPAY = null;
            Item.FB_PAGE = null;
            Item.FB_PARPRICE = null;
            Item.FB_PARTKIND = null;
            Item.FB_PAYKNDCODE = null;
            Item.FB_PELAK = null;
            Item.FB_PKIND = null;
            Item.FB_PRECODE = null;
            Item.FB_PREDATE = null;
            Item.FB_PREPRICE = null;
            Item.FB_PRJMRDTLID = null;
            Item.FB_PROFORMCOD = null;
            Item.FB_PROJCODE = null;
            Item.FB_PROJYEAR = null;
            Item.FB_PRO_YEAR = null;
            Item.FB_QC = null;
            Item.FB_RECNO = null;
            Item.FB_RESNO = null;
            Item.FB_ROW_ID3 = null;
            Item.FB_SEFCODE = null;
            Item.FB_SELERCODE = null;
            Item.FB_SELERNAME = null;
            Item.FB_SUB1 = null;
            Item.FB_SUB2 = null;
            Item.FB_SUB3 = null;
            Item.FB_SUB4 = null;
            Item.FB_SUMSAN = null;
            Item.FB_TAFCODE1 = null;
            Item.FB_TAFCODE2 = null;
            Item.FB_TAFCODE3 = null;
            Item.FB_TAFCODE4 = null;
            Item.FB_TAFCODE5 = null;
            Item.FB_TAFCODE6 = null;
            Item.FB_TAFCODE7 = null;
            Item.FB_TAFKINDNO1 = null;
            Item.FB_TAFKINDNO2 = null;
            Item.FB_TAFKINDNO3 = null;
            Item.FB_TAFKINDNO4 = null;
            Item.FB_TAFKINDNO5 = null;
            Item.FB_TAFKINDNO6 = null;
            Item.FB_TAFKINDNO7 = null;
            Item.FB_TANNO = null;
            Item.FB_USER = null;
            Item.FB_USERNAME = null;
            Item.FB_WDPERCENT = null;
            Item.FB_WELLDONE = null;
            Item.FB_WUSER = null;
            Item.FB_YEAR = null;
            Item.FB_FACTNO_GUID = null;
            Item.FB_FactorNum = null;
            Item.SH_FactBuy_DTL = null;
        }

        public async Task ResidAnbarSetShomaran(dynamic Selected, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            Item.FB_FACTNO_GUID = Selected.FACTNO_GUID;
            Item.FB_ACCCODE = Selected.ACCCODE;
            Item.FB_ACTCODE = Selected.ACTCODE;
            Item.FB_ACTYEAR = Selected.ACTYEAR;
            Item.FB_ADD1 = Selected.ADD1;
            Item.FB_ADD2 = Selected.ADD2;
            Item.FB_ADD3 = Selected.ADD3;
            Item.FB_ADD4 = Selected.ADD4;
            Item.FB_AMOUNT = Selected.AMOUNT;
            Item.FB_ARRNO = Selected.ARRNO;
            Item.FB_ARRYEAR = Selected.ARRYEAR;
            Item.FB_BAARNAME = Selected.BAARNAME;
            Item.FB_BABCODE = Selected.BABCODE;
            Item.FB_BRANCHCODE = Selected.BRANCHCODE;
            Item.FB_BUYKIND = Selected.BUYKIND;
            Item.FB_C1901 = Selected.C1901;
            Item.FB_C1950 = Selected.C1950;
            Item.FB_CAR_TYPE = Selected.CAR_TYPE;
            Item.FB_CASHCODE = Selected.CASHCODE;
            Item.FB_CASHKIND = Selected.CASHKIND;
            Item.FB_COMP_NAME = Selected.COMP_NAME;
            Item.FB_CPRICE = Selected.CPRICE;
            Item.FB_CREATOR = Selected.CREATOR;
            Item.FB_CTFKNO1 = Selected.CTFKNO1;
            Item.FB_CTFKNO2 = Selected.CTFKNO2;
            Item.FB_CTFKNO3 = Selected.CTFKNO3;
            Item.FB_CTFKNO4 = Selected.CTFKNO4;
            Item.FB_CTFKNO5 = Selected.CTFKNO5;
            Item.FB_CTFKNO6 = Selected.CTFKNO6;
            Item.FB_CTFKNO7 = Selected.CTFKNO7;
            Item.FB_CTFKNO8 = Selected.CTFKNO8;
            Item.FB_CTFKNO9 = Selected.CTFKNO9;
            Item.FB_CTFKNO10 = Selected.CTFKNO10;
            Item.FB_CTFNO1 = Selected.CTFNO1;
            Item.FB_CTFNO2 = Selected.CTFNO2;
            Item.FB_CTFNO3 = Selected.CTFNO3;
            Item.FB_CTFNO4 = Selected.CTFNO4;
            Item.FB_CTFNO5 = Selected.CTFNO5;
            Item.FB_CTFNO6 = Selected.CTFNO6;
            Item.FB_CTFNO7 = Selected.CTFNO7;
            Item.FB_CTFNO8 = Selected.CTFNO8;
            Item.FB_CTFNO9 = Selected.CTFNO9;
            Item.FB_CTFNO10 = Selected.CTFNO10;
            Item.FB_CURFACT = Selected.CURFACT;
            Item.FB_CURKIND = Selected.CURKIND;
            Item.FB_CURPKID = Selected.CURPKID;
            Item.FB_C_CURVAL = Selected.C_CURVAL;
            Item.FB_C_DEDUCE = Selected.C_DEDUCE;
            Item.FB_C_OTHERPAY = Selected.C_OTHERPAY;
            Item.FB_C_PARPRICE = Selected.C_PARPRICE;
            Item.FB_C_PREPRICE = Selected.C_PREPRICE;
            Item.FB_C_WELLDONE = Selected.C_WELLDONE;
            Item.FB_DEDUCE = Selected.DEDUCE;
            Item.FB_DOC_ID = Selected.DOC_ID;
            Item.FB_DOC_ID2 = Selected.DOC_ID2;
            Item.FB_DOC_YEAR = Selected.DOC_YEAR;
            Item.FB_DOC_YEAR2 = Selected.DOC_YEAR2;
            Item.FB_DRV_CARTNO = Selected.DRV_CARTNO;
            Item.FB_DRV_NAME = Selected.DRV_NAME;
            Item.FB_ELAMNO = Selected.ELAMNO;
            Item.FB_FACTDATE = Selected.FACTDATE;
            Item.FB_FACTNO = Selected.FACTNO;
            Item.FB_FACTOR = Selected.FACTOR;
            Item.FB_FACTPOS = Selected.FACTPOS;
            Item.FB_FACTSELER = Selected.FACTSELER;
            Item.FB_FDATE = Selected.FDATE;
            Item.FB_FID = Selected.FID;
            Item.FB_FORMCODE = Selected.FORMCODE;
            Item.FB_FORMYEAR = Selected.FORMYEAR;
            Item.FB_HAZCODE = Selected.HAZCODE;
            Item.FB_INVCODE = Selected.INVCODE;
            Item.FB_MBCODE = Selected.MBCODE;
            Item.FB_MBPROJCODE = Selected.MBPROJCODE;
            Item.FB_MBSBSTCODE = Selected.MBSBSTCODE;
            Item.FB_NEW_CURVAL = Selected.NEW_CURVAL;
            Item.FB_NOSEFARESH = Selected.NOSEFARESH;
            Item.FB_NOTE = Selected.NOTE;
            Item.FB_ORDERNO = Selected.ORDERNO;
            Item.FB_ORDERYEAR = Selected.ORDERYEAR;
            Item.FB_OTHERPAY = Selected.OTHERPAY;
            Item.FB_PAGE = Selected.PAGE;
            Item.FB_PARPRICE = Selected.PARPRICE;
            Item.FB_PARTKIND = Selected.PARTKIND;
            Item.FB_PAYKNDCODE = Selected.PAYKNDCODE;
            Item.FB_PELAK = Selected.PELAK;
            Item.FB_PKIND = Selected.PKIND;
            Item.FB_PRECODE = Selected.PRECODE;
            Item.FB_PREDATE = Selected.PREDATE;
            Item.FB_PREPRICE = Selected.PREPRICE;
            Item.FB_PRJMRDTLID = Selected.PRJMRDTLID;
            Item.FB_PROFORMCOD = Selected.PROFORMCOD;
            Item.FB_PROJCODE = Selected.PROJCODE;
            Item.FB_PROJYEAR = Selected.PROJYEAR;
            Item.FB_PRO_YEAR = Selected.PRO_YEAR;
            Item.FB_QC = Selected.QC;
            Item.FB_RECNO = Selected.RECNO;
            Item.FB_RESNO = Selected.RESNO;
            Item.FB_ROW_ID3 = Selected.ROW_ID3;
            Item.FB_SEFCODE = Selected.SEFCODE;
            Item.FB_SELERCODE = Selected.SELERCODE;
            Item.FB_SELERNAME = Selected.SELERNAME;
            Item.FB_SUB1 = Selected.SUB1;
            Item.FB_SUB2 = Selected.SUB2;
            Item.FB_SUB3 = Selected.SUB3;
            Item.FB_SUB4 = Selected.SUB4;
            Item.FB_SUMSAN = Selected.SUMSAN;
            Item.FB_TAFCODE1 = Selected.TAFCODE1;
            Item.FB_TAFCODE2 = Selected.TAFCODE2;
            Item.FB_TAFCODE3 = Selected.TAFCODE3;
            Item.FB_TAFCODE4 = Selected.TAFCODE4;
            Item.FB_TAFCODE5 = Selected.TAFCODE5;
            Item.FB_TAFCODE6 = Selected.TAFCODE6;
            Item.FB_TAFCODE7 = Selected.TAFCODE7;
            Item.FB_TAFKINDNO1 = Selected.TAFKINDNO1;
            Item.FB_TAFKINDNO2 = Selected.TAFKINDNO2;
            Item.FB_TAFKINDNO3 = Selected.TAFKINDNO3;
            Item.FB_TAFKINDNO4 = Selected.TAFKINDNO4;
            Item.FB_TAFKINDNO5 = Selected.TAFKINDNO5;
            Item.FB_TAFKINDNO6 = Selected.TAFKINDNO6;
            Item.FB_TAFKINDNO7 = Selected.TAFKINDNO7;
            Item.FB_TANNO = Selected.TANNO;
            Item.FB_USER = Selected.USER;
            Item.FB_USERNAME = Selected.USERNAME;
            Item.FB_WDPERCENT = Selected.WDPERCENT;
            Item.FB_WELLDONE = Selected.WELLDONE;
            Item.FB_WUSER = Selected.WUSER;
            Item.FB_YEAR = Selected.YEAR;
            Item.FB_FactorNum = Selected.FactorNum;

            //	فراخوانی داده از dropdown TempNoNum برای گرید داده های آن
            Ref_SCMPETCO_ProductRequestDetails_SH_FactBuy_DTL?.SetEntity(Item);
            await Task.Delay(100);
            Ref_SCMPETCO_ProductRequestDetails_SH_FactBuy_DTL?.LoadData();
        }

        public async Task<bool> SCMPETCO_ProductRequestDetails_editmodelsaving(object e)
        {
            bool IsCancelled = false;
            // Item
            var Item = (Entity.SCMPETCO_ProductRequestDetails)e;

            IsCancelled = !await CheckFieldValidation(Item);

            return IsCancelled;
        }

        public async Task SCMPETCO_ProductRequestDetails_afterrendermodal(Entity.SCMPETCO_ProductRequestDetails Item)
        {
            // عطف رسید انبار  
            if (Item.FB_FACTNO_GUID.HasValue)
            {
                Ref_SCMPETCO_ProductRequestDetails_SH_FactBuy_DTL.SetEntity(Item);
                Ref_SCMPETCO_ProductRequestDetails_SH_FactBuy_DTL.LoadData();
            }
        }

        public async Task FB_Search_NotMapped_onitemselected(dynamic Selected, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            await ResidAnbarSetShomaran(Selected, Item);
        }

        #endregion FunctionEvents

    }
}