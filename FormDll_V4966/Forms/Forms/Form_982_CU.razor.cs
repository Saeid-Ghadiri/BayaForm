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
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Text.Json;
using Baya.Models.ORM;
using Castle.DynamicLinqQueryBuilder;
using System.Globalization;

namespace Forms.Forms
{
    public class Form_982_CUBase : Form_982_CUPeropeties
    {
        public string invCode = "";
        public string centCode = "";
        public string formCode = "";
        public string babCode = "";
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
            bool addupdate = await AddRetHav();

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
        public async Task<bool> AddRetHav()
        {
            Console.WriteLine("#Log Start::");
            Console.WriteLine("#_Entity.CentCode:" + _Entity.CentCode);
            Console.WriteLine("#_Entity.Creator:" + _Entity.Creator);
            Console.WriteLine("#_Entity.FactDate:" + _Entity.FactDate);
            Console.WriteLine("#_Entity.InvCode:" + _Entity.InvCode);
            Console.WriteLine("#_Entity.Note:" + _Entity.Note);
            Console.WriteLine("#_Entity.TempNo:" + _Entity.TempNo);
            //Console.WriteLine("#_Entity.WUser:" + _Entity.WUser);
            Console.WriteLine("#_Entity.Year:" + _Entity.Year);
            if (_Entity._IdIsEmpty.Value)
            {
                //// ثبت
                var transDetails = _Entity.Shomaran_ReturnHavaleDetail.Select(p => new RethavDetail()
                {
                    Amount = Convert.ToDecimal(p.Amount),
                    //Amount2 = Convert.ToDecimal(p.Amount2),
                    PartCode = p.PartCode,
                    Radyabi = p.Radyabi,
                    Sefaresh = p.Sefaresh,
                    RowOrder = Convert.ToInt32(p.RowOrder),
                    Year = Convert.ToInt32(p.Year)
                }).ToList();


                foreach (var item in transDetails)
                {
                    Console.WriteLine("#Detail {item.Amount}:" + item.Amount);
                    //Console.WriteLine("#Detail {item.Amount2}:" + item.Amount2);
                    Console.WriteLine("#Detail {item.PartCode}:" + item.PartCode);
                    Console.WriteLine("#Detail {item.Radyabi}:" + item.Radyabi);
                    Console.WriteLine("#Detail {item.Sefaresh}:" + item.Sefaresh);
                    Console.WriteLine("#Detail {item.RowOrder}:" + item.RowOrder);
                    Console.WriteLine("#Detail {item.Year}:" + item.Year);
                }

                RethavHeader rethavData = new()
                {
                    //
                    CentCode = _Entity.CentCode,//centCode,
                    Creator = _Entity.Creator,
                    FactDate = _Entity.FactDate,
                    InvCode = _Entity.InvCode,//invCode,
                    Note = _Entity.Note,
                    TempNo = _Entity.TempNo,
                    //WUser = _Entity.WUser,
                    Year = Convert.ToInt32(_Entity.Year),

                    Details = transDetails,
                };


                Console.WriteLine("#Log factbBuyData ::" + await JSON.ToJson(rethavData));

                var data = await ApiServer.External.Services.ShomaranPart.CreateReturnHavale(ShomaranApiMode.Polfilm, rethavData);


                Console.WriteLine("#Log End ::");
                Console.WriteLine("#Log status ::" + data.Status.ToString());
                string result = await JSON.ToJson(data);

                Console.WriteLine("#Log data ::" + result);
                if (data.Status == HttpStatusCode.OK)
                {
                    await _MSG.ShowSuccess(await JSON.ToJson(data.Content));
                    _Entity.ApiResult = data.Content.ToString();

                    using var doc = JsonDocument.Parse(data.Content.ToString());
                    var number = doc.RootElement
                                .GetProperty("ReceiptNumber")[0]
                                .GetString()
                                ?.Trim();   // remove spaces
                    _Entity.FactNo = number;

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
                //UpdateRethavInput factbBuyData = new()
                //{
                //    //
                //    //FactNo = _Entity.FactNo,//
                //    FactDate = _Entity.FactDate,
                //    InvCode = _Entity.InvCode,
                //    Note = _Entity.Note,
                //    TempNo = _Entity.TempNo,
                //    Amount = _Entity.Amount,
                //    PartCode = _Entity.PartCode,
                //    Radyabi = _Entity.Radyabi,
                //    Sefaresh = _Entity.Sefaresh,
                //    Year = Convert.ToInt32(_Entity.Year),
                //};

                //var data = await ApiServer.External.Services.ShomaranPart.UpdateReturnHavale(ShomaranApiMode.Polfilm, factbBuyData);

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

        public async Task<bool> GridShomaran_ReturnHavaleId_731_editmodelsaving(object e)
        {
            ////// آپدیت
            var Item = (Entity.Shomaran_ReturnHavaleDetail)e;
            UpdateRethavInput factbBuyData = new()
            {
                //
                FactNo = _Entity.FactNo,//
                FactDate = _Entity.FactDate,
                InvCode = _Entity.InvCode,//invCode,
                //
                //BabCode = _Entity.BabCode != null ? _Entity.BabCode : "",
                Creator = _Entity.Creator,//centCode,
                CentCode = _Entity.CentCode,//centCode,
                //Code = _Entity.Code,
                //FormCode = _Entity.FormCode != null ? _Entity.FormCode : "",//formCode,
                //MainMnt = Convert.ToDecimal(_Entity.MainMnt),
                //PayCode = _Entity.PayCode,
                //
                Note = _Entity.Note,
                TempNo = _Entity.TempNo,
                PartCode = Item.PartCode,
                RowOrder = Convert.ToInt32(Item.RowOrder),
                Amount = Convert.ToDecimal(Item.Amount),
                Year = Convert.ToInt32(Item.Year),
            };

            var data = await ApiServer.External.Services.ShomaranPart.UpdateReturnHavale(ShomaranApiMode.Polfilm, factbBuyData);

            if (data.Status == HttpStatusCode.OK)
            {
                await _MSG.ShowSuccess(await JSON.ToJson(data.Content));
            }
            else
            {
                await _MSG.ShowError(await JSON.ToJson(data.Content));
            }
            return false;
        }

        public async Task InvCode_onitemselected(dynamic Selected)
        {
            invCode = Selected.INVCODE;
            Console.WriteLine("#Log invCode ::" + invCode);
        }
        public async Task CentCode_onitemselected(dynamic Selected)
        {

            centCode = Selected.CENTCODE;
            Console.WriteLine("#Log centCode ::" + centCode);
        }
        public async Task FormCode_onitemselected(dynamic Selected)
        {
            formCode = Selected.FORMCODE;
            Console.WriteLine("#Log formCode ::" + formCode);

        }

        #endregion FunctionEvents

    }
}


namespace ApiServer.External.Services
{
    public partial class ShomaranPart
    {

        public static async Task<Result> CreateReturnHavale(ShomaranApiMode ApiMode, RethavHeader retHav)
        {
            // Serialize the entire havaleData object into a JSON string
            var DataJson = await JSON.ToJson(retHav);

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
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/InsertRetHavale", shomaranApi, ApplicationType.None);

            return apiresult;
        }

        public static async Task<Result> UpdateReturnHavale(ShomaranApiMode ApiMode, UpdateRethavInput retHav)
        {
            // Serialize the entire havaleData object into a JSON string
            var DataJson = await JSON.ToJson(retHav);

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
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/UpdateRetHavale", shomaranApi, ApplicationType.None);

            return apiresult;
        }

        public static async Task<Result> DeleteRetHav(ShomaranApiMode ApiMode, string factno, int year)
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
            Result apiresult = await Send.PostAsync(_content, $"ShomaranPart/DeleteRetHav/{factno}/{year}", shomaranApi, ApplicationType.None);
            return apiresult;
        }

    }
}



public sealed class RethavHeader
{
    //public string BranchCode { get; set; }
    public string CentCode { get; set; }
    public string Creator { get; set; }       // الزامی
    public string FactDate { get; set; }      // الزامی (مثال: "1404/06/01")
    //public string FactNo { get; set; }        // اگر خالی باشد، SP شماره می‌سازد
    //public string FactPos { get; set; }
    //public string FormCode { get; set; }
    //public int FormYear { get; set; }
    //public decimal MainMnt { get; set; }

    public string InvCode { get; set; }       // انبار (الزامی)
    public string Note { get; set; }
    public string TempNo { get; set; }
    //public string WUser { get; set; }
    public int Year { get; set; }             // الزامی

    public List<RethavDetail> Details { get; set; } = new();
}

public sealed class RethavDetail
{
    public decimal Amount { get; set; }       // numeric(15,3)
    //public decimal Amount2 { get; set; }      // numeric(14,3)
    //public string BabCode { get; set; }
    //public string Code { get; set; }
    public string PartCode { get; set; }      // ضروری
    //public string PayCode { get; set; }
    public string Sefaresh { get; set; }
    public string Radyabi { get; set; }
    public int RowOrder { get; set; }
    //public string TafCode1 { get; set; }
    //public string TafCode2 { get; set; }
    //public string TafCode3 { get; set; }
    //public string TafCode4 { get; set; }
    //public string TafCode5 { get; set; }
    //public string TafCode6 { get; set; }
    //public string TafCode7 { get; set; }
    //public string TafKindNo1 { get; set; }
    //public string TafKindNo2 { get; set; }
    //public string TafKindNo3 { get; set; }
    //public string TafKindNo4 { get; set; }
    //public string TafKindNo5 { get; set; }
    //public string TafKindNo6 { get; set; }
    //public string TafKindNo7 { get; set; }
    public int Year { get; set; }             // معمولا همان Year هدر
}


public sealed class UpdateRethavInput
{
    public string FactNo { get; set; }      // @FACTNO (varchar(10)) - کلید رکورد
    public string Creator { get; set; }    // @Creator 
    public string CentCode { get; set; }    // @CENTCODE (varchar(10))
    public string FactDate { get; set; }    // @FACTDATE (varchar(10))
    public string InvCode { get; set; }     // @INVCODE (varchar(10))
    public string Note { get; set; }        // @NOTE (varchar(50))

    //public string FormCode { get; set; }    // @FORMCODE (varchar(10))
    //public decimal MainMnt { get; set; }    // @MAIN_MNT (numeric(10,3))
    public string TempNo { get; set; }      // @TEMPNO (varchar(10))

    // detail fields (آپدیت روی همه ردیف‌ها با همان FACTNO انجام می‌شود)
    public decimal Amount { get; set; }     // @AMOUNT (numeric(18,3))
    //public string BabCode { get; set; }     // @BABCODE (varchar(10))
    //public string Code { get; set; }        // @CODE (varchar(10))
    //public string PayCode { get; set; }     // @PAYCODE (varchar(10))
    public string PartCode { get; set; }     // @PartCode
    public int RowOrder { get; set; }     // @RowId

    public int Year { get; set; }           // @YEAR (int)
}
