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
    public class Form_447Base : Form_447Peropeties
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

            var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();

            foreach (var Item in List)
            {

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
            await Task.Delay(100);
            //Console.WriteLine(Visible.ToString());
            try
            {
                //Console.WriteLine("#Log Ref" + (Ref_SCMPLATE_ProductRequestDetails_T_Search_NotMapped == null).ToString());
                Ref_SCMPLATE_ProductRequestDetails_T_Search_NotMapped.SetVisible(Visible);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                //Console.WriteLine(await Utility.JSON.ToJson(ex));
            }
            Ref_SCMPLATE_ProductRequestDetails_T_Search_NotMapped.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_T_CENTCODE.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_T_CENTCODE_GUID.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_T_PAYCENTName.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_T_CREATOR.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_T_FACTDATE.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_T_FACTNO.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_T_FACTNO_GUID.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_T_TEMPNO.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_T_TempNoNum.SetVisible(Visible);
            Ref_SCMPLATE_ProductRequestDetails_T_YEAR.SetVisible(Visible);
            // جزئیات تحویل کالا
            Ref_SCMPLATE_ProductRequestDetails_SH_Tahvil_DTL.SetVisible(Visible);

            // #############
            Ref_SCMPLATE_ProductRequestDetails_T_CENTCODE.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_T_CENTCODE_GUID.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_T_PAYCENTName.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_T_CREATOR.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_T_FACTDATE.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_T_FACTNO.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_T_FACTNO_GUID.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_T_TEMPNO.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_T_TempNoNum.SetDisabled(true);
            Ref_SCMPLATE_ProductRequestDetails_T_YEAR.SetDisabled(true);
        }

        public async Task HavaleMasrafIsNull(Entity.SCMPLATE_ProductRequestDetails Item)
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

        public async Task HavaleMasrafSetShomaran(dynamic Selected, Entity.SCMPLATE_ProductRequestDetails Item)
        {
            //Console.WriteLine("#Log 2");
            await Task.Delay(200);

            Item.T_TempNoNum = Selected.TempNoNum;
            Item.T_PAYCENTName = Selected.PAYCENTName;
            Item.T_TEMPNO = Selected.TEMPNO;
            Item.T_CREATOR = Selected.CREATOR;
            Item.T_YEAR = Selected.YEAR;
            Item.T_CENTCODE_GUID = Selected.CENTCODE_GUID;
            Item.T_CENTCODE = Selected.CENTCODE;
            Item.T_FACTDATE = Selected.FACTDATE;
            Item.T_FACTNO = Selected.FACTNO;
            Item.T_FACTNO_GUID = Selected.FACTNO_GUID;
             
                // تغییرات - 793
                Ref_SCMPLATE_ProductRequestDetails_SH_Tahvil_DTL?.SetEntity(Item);
                
                await Task.Delay(100); 
                Ref_SCMPLATE_ProductRequestDetails_SH_Tahvil_DTL?.LoadData();           
        }

       


		public async Task <bool> GridSCMPLATE_ProductRequestId_editmodelsaving(object e   )
        {
            bool IsCancelled = false;

            var Item = (Entity.SCMPLATE_ProductRequestDetails)e;

            // کد شماره درخواست تحویل کالا شماران
            if (Item.T_FACTNO_GUID == null)
            {
                IsCancelled = true;

                toastService.ShowError("لطفا گزینه ارتباط با شماران را تکمیل نمایید.",
                    settings =>
                    {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
            }

            return IsCancelled;
        }
        public async Task  GridSCMPLATE_ProductRequestId_afterrendermodal(Entity.SCMPLATE_ProductRequestDetails Item   )
        {
             if (Item.T_FACTNO_GUID.HasValue)
            {
                Ref_SCMPLATE_ProductRequestDetails_SH_Tahvil_DTL.SetEntity(Item);
                Ref_SCMPLATE_ProductRequestDetails_SH_Tahvil_DTL.LoadData();
            }
            
        }
       
        public async Task T_Search_NotMapped_onitemselected(dynamic Selected, Entity.SCMPLATE_ProductRequestDetails Item)
        {
            await HavaleMasrafSetShomaran(Selected, Item);
            
        }

        
       
		public async Task  TMaster_Search_NotMapped_onitemselected(dynamic Selected   )
        {
            foreach(var item in _Entity.SCMPLATE_ProductRequestDetails)
            {
                await HavaleMasrafSetShomaran(Selected, item);
            }
            
        }

		#endregion FunctionEvents
    }
}