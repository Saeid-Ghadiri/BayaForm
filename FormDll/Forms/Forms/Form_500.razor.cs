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
using Baya.Models.Utility.Menu;

namespace Forms.Forms
{
    public class Form_500Base : Form_500Peropeties
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
            if(!await TempNoNum()){
                return new Result() { Status = HttpStatusCode.BadRequest };
            }
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

        // عطف تحویل کالا - حواله مصرف شماران سیستم
        public async Task HavaleMasrafIsVisible(bool Visible)
        {
            await Task.Delay(100);
            Ref_SCMPETCO_ProductRequestDetails_T_Search_NotMapped.SetVisible(Visible);

            Ref_SCMPETCO_ProductRequestDetails_T_CENTCODE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_T_CENTCODE_GUID.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_T_PAYCENTName.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_T_CREATOR.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_T_FACTDATE.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_T_FACTNO.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_T_FACTNO_GUID.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_T_TEMPNO.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_T_TempNoNum.SetVisible(Visible);
            Ref_SCMPETCO_ProductRequestDetails_T_YEAR.SetVisible(Visible);

            // جزئیات تحویل کالا
            Ref_SCMPETCO_ProductRequestDetails_SH_Tahvil_DTL.SetVisible(Visible);

            Ref_SCMPETCO_ProductRequestDetails_T_CENTCODE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_T_CENTCODE_GUID.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_T_PAYCENTName.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_T_CREATOR.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_T_FACTDATE.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_T_FACTNO.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_T_FACTNO_GUID.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_T_TEMPNO.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_T_TempNoNum.SetDisabled(true);
            Ref_SCMPETCO_ProductRequestDetails_T_YEAR.SetDisabled(true);
        }

        public async Task HavaleMasrafIsNull(Entity.SCMPETCO_ProductRequestDetails Item)
        {
            // Ref_SCMPETCO_ProductRequestDetails_T_Search_NotMapped = null;
            // Ref_SCMPETCO_ProductRequestDetails_T_CENTCODE = null;
            // Ref_SCMPETCO_ProductRequestDetails_T_CENTCODE_GUID = null;
            // Ref_SCMPETCO_ProductRequestDetails_T_PAYCENTName = null;
            // Ref_SCMPETCO_ProductRequestDetails_T_CREATOR = null;
            // Ref_SCMPETCO_ProductRequestDetails_T_FACTDATE = null;
            // Ref_SCMPETCO_ProductRequestDetails_T_FACTNO = null;
            // Ref_SCMPETCO_ProductRequestDetails_T_FACTNO_GUID = null;
            // Ref_SCMPETCO_ProductRequestDetails_T_TEMPNO = null;
            // Ref_SCMPETCO_ProductRequestDetails_T_TempNoNum = null;
            // Ref_SCMPETCO_ProductRequestDetails_T_YEAR = null;
            // Ref_SCMPETCO_ProductRequestDetails_SH_Tahvil_DTL = null;

            // Item.T_Search_NotMapped = null; // فیلد نات مپ ه
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

        public async Task HavaleMasrafSetShomaran(dynamic Selected, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            Console.WriteLine("#Log 2");
            await Task.Delay(100);
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
            Console.WriteLine("#Log 2.2");
            // فراخوانی داده از dropdown TempNoNum برای گرید داده های آن
            //Ref_SCMPETCO_ProductRequestDetails_SH_Tahvil_DTL.SetEntity(Item);
            Console.WriteLine(await Utility.JSON.ToJson(Item));
            await Task.Delay(100);
            Console.WriteLine("#Log 2.3");
            //Ref_SCMPETCO_ProductRequestDetails_SH_Tahvil_DTL.LoadData();
            Console.WriteLine("#Log 2.4");
        }

        //*******************************************************************************************************************************************************

        

        //**********************************************************************************************************************

       

      

        public async Task Global_ShomaranInfoId_onitemselected(Entity.Global_ShomaranInfo Selected, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            // انتخاب نوع ثبت عطف در شماران      
            if (Selected.Id.ToString() == null)
            {
                await HavaleMasrafIsVisible(false);
            }
            else
            {
                //Console.WriteLine("#Log 3-dropdown" + " is " + Selected.Id.ToString());

                // حواله مصرف
                if (Selected.Id.ToString() == "a5b1bc7b-8bb7-ef11-a4fa-005056a2b6bd")
                {
                    await HavaleMasrafIsVisible(true);
                }

                // // خرید کالا 
                // if (Selected.Id.ToString() == "09cf6986-8bb7-ef11-a4fa-005056a2b6bd")
                // {
                //     await HavaleMasrafIsVisible(false);
                //     await KharidIsVisible(true);
                //     await ResidAnbarIsVisible(false);
                // }
                
                // رسید انبار
                if (Selected.Id.ToString() == "0acf6986-8bb7-ef11-a4fa-005056a2b6bd")
                {
                    //Console.WriteLine("#Log 3.1-dropdown" + Ref_SCMPETCO_ProductRequestDetails_FB_Search_NotMapped.Value);

                    await HavaleMasrafIsVisible(false);
                }
            }
        }

        public async Task T_Search_NotMapped_onitemselected1(dynamic Selected, Entity.SCMPETCO_ProductRequestDetails Item)
        {
            await HavaleMasrafSetShomaran(Selected, Item);
        }

        public async Task KH_Search_NotMapped_onitemselected(dynamic Selected, Entity.SCMPETCO_ProductRequestDetails Item)
        {
        }

        public async Task FB_Search_NotMapped_onitemselected(dynamic Selected, Entity.SCMPETCO_ProductRequestDetails Item)
        {
        }

        public async Task TempNoNum_NotMapped_onitemselected(dynamic Selected)
        {
            foreach (var item in _Entity.SCMPETCO_ProductRequestDetails)
            {
                await HavaleMasrafSetShomaran(Selected, item);
            }

            StateHasChanged();
        }


        public async Task <bool> TempNoNum()
        {
            foreach (var item in _Entity.SCMPETCO_ProductRequestDetails)
            {
               if(!item.T_FACTNO_GUID.HasValue){

                    toastService.ShowError("لطفا گزینه جستجوی عطف را تکمیل نمایید.",
                        settings =>
                        {
                            settings.Timeout = 4;
                            settings.ShowProgressBar = true;
                            settings.PauseProgressOnHover = true;
                        });


                        return false;
                }
            }
            return true;
        }

	

		public async Task <bool> GridSCMPETCO_ProductRequestId_200_editmodelsaving(object e   )
        {
            Console.WriteLine("200_editmodelsaving");
            bool IsCancelled = false;
            // Item
            var Item = (Entity.SCMPETCO_ProductRequestDetails)e;

            // انتخاب نوع ثبت عطف در شماران
            

            // if (Item.Global_ShomaranInfoId.HasValue)
            // {

            //     if ((Item.Global_ShomaranInfoId.Value.ToString() == "a5b1bc7b-8bb7-ef11-a4fa-005056a2b6bd" && !Item.T_FACTNO_GUID.HasValue) 
            //     // ||(Item.Global_ShomaranInfoId.Value.ToString() == "09cf6986-8bb7-ef11-a4fa-005056a2b6bd" && !Item.KH_CENTCODE_GUID.HasValue) ||
            //     //(Item.Global_ShomaranInfoId.Value.ToString() == "0acf6986-8bb7-ef11-a4fa-005056a2b6bd" && !Item.FB_FACTNO_GUID.HasValue)
            //     )
            //     {
            //         IsCancelled = true;

            //         toastService.ShowError("لطفا گزینه جستجوی عطف را تکمیل نمایید.",
            //             settings =>
            //             {
            //                 settings.Timeout = 4;
            //                 settings.ShowProgressBar = true;
            //                 settings.PauseProgressOnHover = true;
            //             });
            //     }
            // }

            if(!Item.T_FACTNO_GUID.HasValue){
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
public async Task  GridSCMPETCO_ProductRequestId_200_afterrendermodal(Entity.SCMPETCO_ProductRequestDetails Item   )
        {
             Console.WriteLine("200_afterrendermodal");
            // // انتخاب نوع ثبت عطف در شماران
            // if (Item.Global_ShomaranInfoId == null)
            // {
            // 	toastService.ShowError("لطفا گزینه ارتباط با شماران را تکمیل نمایید.",
            // 		settings =>
            // 		{
            // 			settings.Timeout = 4;
            // 			settings.ShowProgressBar = true;
            // 			settings.PauseProgressOnHover = true;
            // 		});
            // }


            //Console.WriteLine("log 0 ");

            await HavaleMasrafIsVisible(true);

            /*
            // انتخاب نوع ثبت عطف در شماران      
            if (!Item.Global_ShomaranInfoId.HasValue)
            {
                //Console.WriteLine("log 1 ");

                await HavaleMasrafIsVisible(false);
            }
            else
            {
                //Console.WriteLine("log 1.2 ");
                //Console.WriteLine("log 2 " + " Is " + Item.Global_ShomaranInfoId.ToString());

                // حواله مصرف
                if (Item.Global_ShomaranInfoId.ToString() == "a5b1bc7b-8bb7-ef11-a4fa-005056a2b6bd")
                {
                    await HavaleMasrafIsVisible(true);
                }

                // خرید کالا 
                if (Item.Global_ShomaranInfoId.ToString() == "09cf6986-8bb7-ef11-a4fa-005056a2b6bd")
                {
                    await HavaleMasrafIsVisible(false);
                    await HavaleMasrafIsNull(Item);


                }

                // رسید انبار
                if (Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64")
                {
                    await HavaleMasrafIsVisible(false);
                    await HavaleMasrafIsNull(Item);


                }
            }

            */

            if (Item.T_FACTNO_GUID.HasValue)
            {
                Ref_SCMPETCO_ProductRequestDetails_SH_Tahvil_DTL.SetEntity(Item);
                Ref_SCMPETCO_ProductRequestDetails_SH_Tahvil_DTL.LoadData();
            }

            

            if (Item.FB_FACTNO_GUID.HasValue)
            {
                Ref_SCMPETCO_ProductRequestDetails_SH_Tahvil_DTL.SetEntity(Item);
                Ref_SCMPETCO_ProductRequestDetails_SH_Tahvil_DTL.LoadData();
            }
            
        }


		#endregion FunctionEvents

    }
}