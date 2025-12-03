using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using System.Text;
using Utility;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using System.Text.Json;
using Baya.Models.ORM;
using Castle.DynamicLinqQueryBuilder;
using System.Globalization;

namespace Forms.Forms
{
    public class Form_981_CUBase : Form_981_CUPeropeties
    {
        public string invCode = "";
        public MSG _MSG { get; set; }

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
            bool addupdate = await AddFactbBuy();

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
            if(_Entity.Creator == null)
            {
                var TablePost = new Table();
                    TablePost.Name = "SH_PolFilm_ShomaranUsers";
                    TablePost.Column = new List<Coulmn>
                    {
                        new Coulmn {Name="UserId"  , NameAs= "UserId"},
                        new Coulmn {Name ="UserName",NameAs="UserName" }
                    };


                    var NewQuery = new QueryBuilderFilterRule() { Condition = "AND" };
                    NewQuery.Rules = new List<QueryBuilderFilterRule>
                    {
                            new QueryBuilderFilterRule()
                            {
                                Field = "UserId",
                                Id = "UserId",
                                Input = "text",
                                Operator = "equal",
                                Type =  "string",
                                Value = new string[]{ _User.UserID.ToString() },
                            }
                    };

                    bool IsOk = false;
                    var Model = await ApiServer.External.Services.Data.Get(TablePost, NewQuery, "SH_PolFilm_ShomaranUsers", _User.UserID.ToString());

                    if (Model?.Status == HttpStatusCode.OK)
                    {
                        

                        Entity.SH_PolFilm_ShomaranUsers en = await JSON.ToObject<Entity.SH_PolFilm_ShomaranUsers>(Model.Content.ToString());

                        _Entity.Creator = en.UserName;
                        //_Entity.Year = تبدیل تاریخ شمسی و گرفتن سال
                        var pc = new PersianCalendar();
                        int persianYear = pc.GetYear(DateTime.Now);
                        _Entity.Year = persianYear;

                        StateHasChanged();
                    }

            }
        }


        #region FunctionEvents
        public async Task<bool> AddFactbBuy()
        {
            Console.WriteLine("#Log Start::");
            Console.WriteLine("#_Entity.Creator:" + _Entity.Creator);
            Console.WriteLine("#_Entity.FactDate:" + _Entity.FactDate);
            Console.WriteLine("#_Entity.InvCode:" + _Entity.InvCode);
            Console.WriteLine("#_Entity.Note:" + _Entity.Note);
            Console.WriteLine("#_Entity.PartKind:" + _Entity.PartKind);
            Console.WriteLine("#_Entity.SelerCode:" + _Entity.SelerCode);
            Console.WriteLine("#_Entity.SelerName:" + _Entity.SelerName);
            Console.WriteLine("#_Entity.SelerAddr:" + _Entity.SelerAddr);
            Console.WriteLine("#_Entity.TempNo:" + _Entity.TempNo);
            Console.WriteLine("#_Entity.WUser:" + _Entity.WUser);
            Console.WriteLine("#_Entity.Year:" + _Entity.Year);
            if (_Entity._IdIsEmpty.Value)
            {
                //// ثبت
                var transDetails = _Entity.Shomaran_FactbBuyDetail.Select(p => new FactbBuyDetail()
                {
                    Amount = Convert.ToDecimal(p.Amount),
                    //Amount2 = Convert.ToDecimal(p.Amount2),
                    PartCode = p.PartCode,
                    Radyabi = p.Radyabi,
                    Sefaresh = p.Sefaresh,
                    RowId = Convert.ToInt32(p.RowId),
                    //UnitPrice = Convert.ToDecimal(p.UnitPrice),
                    Year = Convert.ToInt32(p.Year)
                }).ToList();


                foreach (var item in transDetails)
                {
                    Console.WriteLine("#Detail {item.Amount}:" + item.Amount);
                    //Console.WriteLine("#Detail {item.Amount2}:" + item.Amount2);
                    Console.WriteLine("#Detail {item.PartCode}:" + item.PartCode);
                    Console.WriteLine("#Detail {item.Radyabi}:" + item.Radyabi);
                    Console.WriteLine("#Detail {item.Sefaresh}:" + item.Sefaresh);
                    //Console.WriteLine("#Detail {item.UnitPrice}:" + item.UnitPrice);
                    Console.WriteLine("#Detail {item.Year}:" + item.Year);
                }

                FactbBuyHeader factbBuyData = new()
                {
                    //
                    Creator = _Entity.Creator,
                    FactDate = _Entity.FactDate,
                    Factor = _Entity.FACTOR,
                    InvCode = _Entity.InvCode,//invCode,
                    Note = _Entity.Note,
                    PartKind = _Entity.PartKind,
                    SelerCode = _Entity.SelerCode,
                    SelerName = _Entity.SelerName,
                    SelerAddr = _Entity.SelerAddr,
                    TempNo = _Entity.TempNo,
                    BuyKind = Convert.ToInt32(_Entity.BUYKIND),
                    NBargasht = _Entity.NBARGASHT,
                    //WUser = _Entity.WUser,
                    Year = Convert.ToInt32(_Entity.Year),

                    Details = transDetails,
                };


                Console.WriteLine("#Log factbBuyData ::" + await JSON.ToJson(factbBuyData));

                var data = await ApiServer.External.Services.ShomaranPart.CreateFactbBuy(ShomaranApiMode.Polfilm, factbBuyData);


                Console.WriteLine("#Log End ::");
                Console.WriteLine("#Log status ::" + data.Status.ToString());
                string result = await JSON.ToJson(data);

                Console.WriteLine("#Log data ::" + result);
                if (data.Status == HttpStatusCode.OK)
                {
                    await _MSG.ShowSuccess(await JSON.ToJson(data.Content));
                    //_Entity.FACTNO = data.Content.ToString();
                    _Entity.ApiResult = data.Content.ToString();

                    using var doc = JsonDocument.Parse(data.Content.ToString());
                    var number = doc.RootElement
                                .GetProperty("Result")[0]
                                .GetString()
                                ?.Trim();   // remove spaces
                    _Entity.FACTNO = number;

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
                //// آپدیت

            }

            return true;
        }

        public async Task InvCode_onitemselected(dynamic Selected)
        {
            invCode = Selected.INVCODE;
            Console.WriteLine("#Log invCode ::" + invCode);
        }


        public async Task<bool> GridShomaran_FactbBuyId_729_editmodelsaving(object e)
        {
            var Item = (Entity.Shomaran_FactbBuyDetail)e;
            if (!Item._IdIsEmpty.Value)
            {
                UpdateFactbBuyInput factbBuyData = new()
                {
                    //
                    FactNo = _Entity.FACTNO,//
                    Factor = _Entity.FACTOR,
                    FactDate = _Entity.FactDate,
                    InvCode = _Entity.InvCode,//invCode,
                    Note = _Entity.Note,
                    PartKind = Convert.ToInt32(_Entity.PartKind),
                    SelerCode = _Entity.SelerCode,
                    SelerName = _Entity.SelerName,
                    TempNo = _Entity.TempNo,
                    BuyKind = Convert.ToInt32(_Entity.BUYKIND),
                    NBargasht = _Entity.NBARGASHT,

                    Amount = Convert.ToDecimal(Item.Amount),
                    PartCode = Item.PartCode,
                    // Radyabi = RowId.Radyabi,
                    // Sefaresh = RowId.SEFARESH,
                    RowId = Convert.ToInt32(Item.RowId),
                    Year = Convert.ToInt32(Item.Year),
                };

                var data = await ApiServer.External.Services.ShomaranPart.UpdateFactbBuy(ShomaranApiMode.Polfilm, factbBuyData);

                if (data.Status == HttpStatusCode.OK)
                {
                    await _MSG.ShowSuccess(await JSON.ToJson(data.Content));
                }
                else
                {
                    await _MSG.ShowError(await JSON.ToJson(data.Content));
                }
            }


            return false;
        }
        public async Task  SelerCode_onitemselected(dynamic Selected   )
        {
                _Entity.SelerName = Selected.name;
                // if (!string.IsNullOrWhiteSpace(Selected.HOMEADDR)) 
                // { 
                //     _Entity.SelerAddr = Selected.HOMEADDR;
                // }else{
                //     _Entity.SelerAddr = Selected.WORKADDR;
                // }
                _Entity.SelerAddr = Selected.WORKADDR;
                
            StateHasChanged();
        }

		#endregion FunctionEvents

    }
}


namespace ApiServer.External.Services
{
    public partial class ShomaranPart
    {

        public static async Task<Result> CreateFactbBuy(ShomaranApiMode ApiMode, FactbBuyHeader factbBuy)
        {
            // Serialize the entire havaleData object into a JSON string
            var DataJson = await JSON.ToJson(factbBuy);

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
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/InsertFactbBuy", shomaranApi, ApplicationType.None);

            return apiresult;
        }

        public static async Task<Result> UpdateFactbBuy(ShomaranApiMode ApiMode, UpdateFactbBuyInput trans)
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
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/UpdateFactbBuy", shomaranApi, ApplicationType.None);

            return apiresult;
        }

        public static async Task<Result> DeleteFactbbuy(ShomaranApiMode ApiMode, string factno, int year)
        {
            var DataJson = await JSON.ToJson(new
            {
                factno,
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
            Result apiresult = await Send.PostAsync(_content, $"ShomaranPart/DeleteFactbbuy/{factno}/{year}", shomaranApi, ApplicationType.None);
            return apiresult;
        }

    }
}

public sealed class FactbBuyHeader
{
    public string Creator { get; set; }     // CREATOR (ضروری)
    public string FactDate { get; set; }    // FACTDATE (ضروری) مثال: 1404/01/01
    public string Factor { get; set; }    // Factor شماره درخواست خرید
    public string InvCode { get; set; }     // INVCODE
    public string Note { get; set; }        // NOTE
    public string PartKind { get; set; }    // PARTKIND (مثلاً "1")
    public string SelerCode { get; set; }   // SELERCODE
    public string SelerName { get; set; }   // SELERNAME
    public string SelerAddr { get; set; }   // SELERADDR
    public string TempNo { get; set; }      // TEMPNO
    public string NBargasht { get; set; }      // NBARGASHT
    public int BuyKind { get; set; }           // BUYKIND (ضروری)

    //public string WUser { get; set; }       // WUSER
    public int Year { get; set; }           // YEAR (ضروری)
    public List<FactbBuyDetail> Details { get; set; } = new();
}

public sealed class FactbBuyDetail
{
    public decimal Amount { get; set; }     // AMOUNT
    //public decimal Amount2 { get; set; }    // AMOUNT2
    public string PartCode { get; set; }    // PARTCODE (ضروری)
    public string Radyabi { get; set; }     // RADYABI
    public string Sefaresh { get; set; }    // SEFARESH
    public int RowId { get; set; }          // ROW_ID
    //public decimal UnitPrice { get; set; }  // UNITPRICE
    public int Year { get; set; }           // YEAR
}








public sealed class UpdateFactbBuyInput
{
    public string FactNo { get; set; }      // @FACTNO
    public string FactDate { get; set; }    // @FACTDATE (مثال: "1404/06/01")
    public string Factor { get; set; }      // @FACTOR
    public string InvCode { get; set; }     // @INVCODE
    public string Note { get; set; }        // @NOTE
    public int PartKind { get; set; }       // @PARTKIND

    public string SelerCode { get; set; }   // @SELERCODE
    public string SelerName { get; set; }   // @SELERNAME
    public string TempNo { get; set; }      // @TEMPNO

    public string NBargasht { get; set; }      // NBARGASHT
    public int BuyKind { get; set; }           // BUYKIND (ضروری)

    // detail
    public decimal Amount { get; set; }     // @AMOUNT
    public string PartCode { get; set; }    // @PARTCODE
    //public string Radyabi { get; set; }     // @RADYABI
    //public string Sefaresh { get; set; }    // @SEFARESH
    public int RowId { get; set; }           // @YEAR
    public int Year { get; set; }           // @YEAR
}

