using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using Utility;
using System.Text;

namespace Forms.Forms
{
    public class Form_964_CUBase : Form_964_CUPeropeties
    {

        // تابع پیام توــست
        public MSG _MSG { get; set; }
        private string fORMCODE = " ";
        private string oRDERNO = " ";
        private int oRDERYEAR = 0;

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
                // تعریف مدل پیام بر اساس تابع تعریف شده
                _MSG = new MSG(toastService);
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
        /// تابع قبل اجرا شدن ارسال داده
        /// </summary>
        /// <returns></returns>
        public override async Task<Result> BeforSubmit()
        {
            // ثبت و ویرایش کالا در شماران سیستم
            bool addupdate = await AddFactBuy();

            if (addupdate)
            {
                return new Result() { Status = HttpStatusCode.OK };
            }

            return new Result() { Status = HttpStatusCode.BadRequest };
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
        /// ثبت خرید کالا در شماران با استفاده از استورد پروسیجر
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AddFactBuy()
        {
            Console.WriteLine("#Log Start::");
            Console.WriteLine("#_Entity.BuyKind:" + _Entity.BUYKIND);
            Console.WriteLine("#_Entity.Creator:" + _Entity.CREATOR);
            Console.WriteLine("#_Entity.FactDate:" + _Entity.FACTDATE);
            Console.WriteLine("#_Entity.Factor:" + _Entity.FACTOR);
            Console.WriteLine("#_Entity.FormCode:" + _Entity.FORMCODE);
            Console.WriteLine("#_Entity.FormYear:" + _Entity.FORMYEAR);
            Console.WriteLine("#_Entity.InvCode:" + _Entity.INVCODE);
            Console.WriteLine("#_Entity.OrderNo:" + _Entity.ORDERNO);
            Console.WriteLine("#_Entity.OrderYear:" + _Entity.ORDERYEAR);
            Console.WriteLine("#_Entity.PartKind:" + _Entity.PARTKIND);
            Console.WriteLine("#_Entity.SelerCode:" + _Entity.SELERCODE);
            Console.WriteLine("#_Entity.SelerName:" + _Entity.SELERNAME);
            Console.WriteLine("#_Entity.SpUser:" + _Entity.SpUser);
            Console.WriteLine("#_Entity.TempNo:" + _Entity.TEMPNO);
            Console.WriteLine("#_Entity.NOTE:" + _Entity.NOTE);
            Console.WriteLine("#_Entity.WUSER:" + _Entity.WUSER);
            Console.WriteLine("#_Entity.YEAR:" + _Entity.YEAR);
            if (_Entity._IdIsEmpty.Value || _Entity.ORDERNO == null)
            {
                //// ثبت
                var factbuyDetails = _Entity.SH_PolFilm_FactBuyDetail.Select(p => new FactbuyDetailDto()
                {
                    Amount = Convert.ToDecimal(p.AMOUNT),
                    //Amount2 = Convert.ToDecimal(p.AMOUNT2),
                    PartCode = p.PARTCODE,
                    Radyabi = p.RADYABI,
                    Sefaresh = p.SEFARESH,
                    RowId = Convert.ToInt32(p.ROW_ID),
                    Year = Convert.ToInt32(p.YEAR)
                }).ToList();


                foreach (var item in factbuyDetails)
                {
                    Console.WriteLine("#Detail {item.OkAmount}:" + item.Amount);
                    Console.WriteLine("#Detail {item.OrdAmount}:" + item.Amount2);
                    Console.WriteLine("#Detail {item.PartCode}:" + item.PartCode);
                    Console.WriteLine("#Detail {item.Radyabi}:" + item.Radyabi);
                    Console.WriteLine("#Detail {item.RowId}:" + item.RowId);
                    Console.WriteLine("#Detail {item.Sefaresh}:" + item.Sefaresh);
                    Console.WriteLine("#Detail {item.Year}:" + item.Year);
                }
                fORMCODE = _Entity.FORMCODE != null ? _Entity.FORMCODE : " ";
                oRDERNO = _Entity.ORDERNO != null ? _Entity.ORDERNO : " ";
                oRDERYEAR = _Entity.ORDERYEAR != null ? Convert.ToInt32(_Entity.ORDERYEAR) : 0;
                FactbuyDto factbuyData = new()
                {
                    //
                    Creator = _Entity.CREATOR,
                    FactDate = _Entity.FACTDATE,
                    Factor = _Entity.FACTOR,
                    InvCode = _Entity.INVCODE,
                    OrderNo = oRDERNO,
                    OrderYear = oRDERYEAR,
                    PartKind = Convert.ToInt32(_Entity.PARTKIND),
                    SelerCode = _Entity.SELERCODE,
                    SelerName = _Entity.SELERNAME,
                    Note = _Entity.NOTE,
                    Year = Convert.ToInt32(_Entity.YEAR),

                    //BuyKind = Convert.ToInt32(_Entity.BUYKIND),
                    //FormCode = fORMCODE,
                    //FormYear = Convert.ToInt32(_Entity.FORMYEAR),
                    //SpUser = _Entity.SpUser,
                    //TempNo = _Entity.TEMPNO,
                    //WUser = _Entity.WUSER,

                    FactbuyDetails = factbuyDetails,
                };


                Console.WriteLine("#Log factbuyData ::" + await JSON.ToJson(factbuyData));

                var data = await ApiServer.External.Services.ShomaranPart.CreateFactBuy2(ShomaranApiMode.Polfilm, factbuyData);


                Console.WriteLine("#Log End ::");
                Console.WriteLine("#Log status ::" + data.Status.ToString());
                string result = await JSON.ToJson(data);

                Console.WriteLine("#Log data ::" + result);
                if (data.Status == HttpStatusCode.OK)
                {
                    await _MSG.ShowSuccess(await JSON.ToJson(data.Content));
                    _Entity.ApiResult = data.Content.ToString();
                    return true;
                }
                else
                {
                    await _MSG.ShowError(await JSON.ToJson(data.Content));
                    return false;
                }

            }
            else
            {
                ////// آپدیت
                //UpdateFactBuyRequest factBuyData = new()
                //{
                //    //
                //    FactNo = _Entity.FACTNO,//
                //    FactDate = _Entity.FACTDATE,
                //    Factor = _Entity.FACTOR,
                //    InvCode = _Entity.INVCODE,
                //    Note = _Entity.NOTE,
                //    PartKind = Convert.ToInt32(_Entity.PARTKIND),
                //    SelerCode = _Entity.SELERCODE,
                //    SelerName = _Entity.SELERNAME,
                //    Amount = Convert.ToDecimal(_Entity.AMOUNT),
                //    NoteD = _Entity.NoteD,
                //    RowId = _Entity.ROW_ID,

                //    //CUnitPrice = _Entity.C_UNITPRICE,

                //    PartCode = _Entity.PARTCODE,

                //    Year = Convert.ToInt32(_Entity.YEAR),
                //};

                //var data = await ApiServer.External.Services.ShomaranPart.UpdateFactBuy2(ShomaranApiMode.Polfilm, factBuyData);

                //if (data.Status == HttpStatusCode.OK)
                //{
                //    await _MSG.ShowSuccess(await JSON.ToJson(data.Content));
                //    return true;
                //}
                //else
                //{
                //    await _MSG.ShowError(await JSON.ToJson(data.Content));
                //    return false;
                //}
            }
            return true;
        }
        public async Task<bool> GridSH_PolFilm_FACTBUYId_710_editmodelsaving(object e)
        {
            var Item = (Entity.SH_PolFilm_FactBuyDetail)e;
            if (!Item._IdIsEmpty.Value)
            {
                UpdateFactBuyRequest factBuyData = new()
                {
                    //
                    FactNo = _Entity.FACTNO,//
                    FactDate = _Entity.FACTDATE,
                    Factor = _Entity.FACTOR,
                    InvCode = _Entity.INVCODE,
                    Note = _Entity.NOTE,
                    PartKind = Convert.ToInt32(_Entity.PARTKIND),
                    SelerCode = _Entity.SELERCODE,
                    SelerName = _Entity.SELERNAME,
                    Amount = Convert.ToDecimal(_Entity.AMOUNT),
                    NoteD = Item.NoteD,
                    RowId = Convert.ToInt32(Item.ROW_ID),

                    //CUnitPrice = _Entity.C_UNITPRICE,

                    PartCode = _Entity.PARTCODE,

                    Year = Convert.ToInt32(_Entity.YEAR),
                };

                var data = await ApiServer.External.Services.ShomaranPart.UpdateFactBuy2(ShomaranApiMode.Polfilm, factBuyData);

                if (data.Status == HttpStatusCode.OK)
                {
                    await _MSG.ShowSuccess(await JSON.ToJson(data.Content));
                    //return true;
                }
                else
                {
                    await _MSG.ShowError(await JSON.ToJson(data.Content));
                    //return false;
                }

            }

               
            return false;
        }

        #endregion FunctionEvents

    }
}

namespace ApiServer.External.Services
{
    public partial class ShomaranPart
    {

        public static async Task<Result> CreateFactBuy2(ShomaranApiMode ApiMode, FactbuyDto factbuyDto)
        {
            // Serialize the entire havaleData object into a JSON string
            var DataJson = await JSON.ToJson(factbuyDto);

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
                    // Handle cases where the ApiMode is not recognized
                    break;
            }

            var _content = new StringContent(DataJson, Encoding.UTF8, "application/json");

            // Note: You may need to change the API endpoint "ShomaranHavale/InsertHavale"
            // to match the correct one for creating a "Havale".
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/InsertFactbuy", shomaranApi, ApplicationType.None);

            return apiresult;
        }
        public static async Task<Result> UpdateFactBuy2(ShomaranApiMode ApiMode, UpdateFactBuyRequest trans)
        {
            // Serialize the entire havaleData object into a JSON string
            var DataJson = await JSON.ToJson(trans);

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
                    // Handle cases where the ApiMode is not recognized
                    break;
            }

            var _content = new StringContent(DataJson, Encoding.UTF8, "application/json");

            // Note: You may need to change the API endpoint "ShomaranHavale/UpdateHavale"
            // to match the correct one for creating a "Havale".
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/UpdateFactBuy", shomaranApi, ApplicationType.None);

            return apiresult;
        }


        public static async Task<Result> DeleteFactbuy2(ShomaranApiMode ApiMode, string factNo, int year)
        {
            var DataJson = await JSON.ToJson(new
            {
                factNo,
                year
            });
            string shomaranApi = "";
            switch (ApiMode)
            {
                case ShomaranApiMode.Polfilm:
                    shomaranApi = "https://shomaran.workcv.ir:2010/{0}/api/v1/"; //https://api.workcv.ir/{0}/api/v1/
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
            Result apiresult = await Send.PostAsync(_content, $"ShomaranPart/DeleteFactbuy/{factNo}/{year}", shomaranApi, ApplicationType.None);
            return apiresult;
        }

    }
}
