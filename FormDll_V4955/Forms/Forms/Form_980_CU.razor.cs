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
using System.Text.Json;
using Baya.Models.ORM;
using Castle.DynamicLinqQueryBuilder;
using System.Globalization;

namespace Forms.Forms
{
    public class Form_980_CUBase : Form_980_CUPeropeties
    {
        public MSG _MSG { get; set; }
        public string sInvCode = "";
        public string tInvCode = "";

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
            bool addupdate = await AddTrans();

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
        public async Task<bool> AddTrans()
        {
            Console.WriteLine("#Log Start::");
            Console.WriteLine("#_Entity.Creator:" + _Entity.Creator);
            Console.WriteLine("#_Entity.FactDate:" + _Entity.FactDate);
            Console.WriteLine("#_Entity.SInvCode:" + _Entity.SInvCode);
            Console.WriteLine("#_Entity.TInvCode:" + _Entity.TInvCode);
            Console.WriteLine("#_Entity.TDesc:" + _Entity.TDesc);
            Console.WriteLine("#_Entity.TempNo:" + _Entity.TempNo);
            Console.WriteLine("#_Entity.WUser:" + _Entity.WUser);
            Console.WriteLine("#_Entity.Year:" + _Entity.Year);
            if (_Entity._IdIsEmpty.Value)
            {
                //// ثبت
                var transDetails = _Entity.Shomaran_TransDetail.Select(p => new TransDtlInput()
                {
                    Amount = Convert.ToDecimal(p.Amount),
                    //Amount2 = Convert.ToDecimal(p.Amount2),
                    Note = p.Note,
                    PartCode = p.PartCode,
                    Radyabi = p.Radyabi,
                    Sefaresh = p.Sefaresh,
                    RowOrder = p.RowOrder,
                    Year = Convert.ToInt32(p.Year)
                }).ToList();


                foreach (var item in transDetails)
                {
                    Console.WriteLine("#Detail {item.Amount}:" + item.Amount);
                    //Console.WriteLine("#Detail {item.Amount2}:" + item.Amount2);
                    Console.WriteLine("#Detail {item.Note}:" + item.Note);
                    Console.WriteLine("#Detail {item.PartCode}:" + item.PartCode);
                    Console.WriteLine("#Detail {item.Radyabi}:" + item.Radyabi);
                    Console.WriteLine("#Detail {item.RowOrder}:" + item.RowOrder);
                    Console.WriteLine("#Detail {item.Year}:" + item.Year);
                }

                TransInput transData = new()
                {
                    //
                    Creator = _Entity.Creator,
                    FactDate = _Entity.FactDate,
                    SInvCode = _Entity.SInvCode,//sInvCode,
                    TInvCode = _Entity.TInvCode,//tInvCode,
                    Note = _Entity.Note,
                    TempNo = _Entity.TempNo,
                    //WUser = _Entity.WUser,
                    Year = Convert.ToInt32(_Entity.Year),

                    Details = transDetails,
                };


                Console.WriteLine("#Log depoOutData ::" + await JSON.ToJson(transData));

                var data = await ApiServer.External.Services.ShomaranPart.CreateTrans(ShomaranApiMode.Polfilm, transData);


                Console.WriteLine("#Log End ::");
                Console.WriteLine("#Log status ::" + data.Status.ToString());
                string result = await JSON.ToJson(data);

                Console.WriteLine("#Log data ::" + result);
                if (data.Status == HttpStatusCode.OK)
                {
                    await _MSG.ShowSuccess(await JSON.ToJson(data.Content));
                    _Entity.ApiResult = data.Content.ToString();
                    
Console.WriteLine("#Log data ::" + data.Content.ToString());

                    // using var doc = JsonDocument.Parse(data.Content.ToString());
                    // var number = doc.RootElement
                    //             .GetProperty("FactNo")[0]
                    //             .GetString()
                    //             ?.Trim();   // remove spaces
                    // _Entity.FactNo = number;

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
                //TransUpdateInput transData = new()
                //{
                //    //
                //    FactNo = _Entity.FactNo,//
                //    FactDate = _Entity.FactDate,
                //    SInvCode = _Entity.SInvCode,
                //    TDesc = _Entity.TDesc,
                //    Note = _Entity.Note,//
                //    TInvCode = _Entity.TInvCode,
                //    TempNo = _Entity.TempNo,
                //    Amount = _Entity.Amount,//
                //    PartCode = _Entity.PartCode,//
                //    NoteDetail = _Entity.NoteDetail,//
                //    Radyabi = _Entity.Radyabi,//
                //    Sefaresh = _Entity.Sefaresh,//
                //    Year = Convert.ToInt32(_Entity.Year),
                //};

                //var data = await ApiServer.External.Services.ShomaranPart.UpdateTrans(ShomaranApiMode.Polfilm, transData);

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
        public async Task<bool> GridShomaran_TransId_727_editmodelsaving(object e)
        {
            var Item = (Entity.Shomaran_TransDetail)e;

            if (!Item._IdIsEmpty.Value)
            {
                ////// آپدیت
                TransUpdateInput transData = new()
                {
                    //
                    FactNo = _Entity.FactNo,//
                    Amount = Convert.ToDecimal(Item.Amount),//
                    PartCode = Item.PartCode,//
                    NoteDetail = Item.Note,//
                    Radyabi = Item.Radyabi,//
                    Sefaresh = Item.Sefaresh,//

                    FactDate = _Entity.FactDate,
                    SInvCode = _Entity.SInvCode,//sInvCode,
                    TInvCode = _Entity.TInvCode,//tInvCode,
                    //TDesc = _Entity.TDesc,
                    TempNo = _Entity.TempNo,
                    Note = _Entity.Note,//

                    Year = Convert.ToInt32(_Entity.Year),
                };

                var data = await ApiServer.External.Services.ShomaranPart.UpdateTrans(ShomaranApiMode.Polfilm, transData);

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

        public async Task SInvCode_onitemselected(dynamic Selected)
        {

            sInvCode = Selected.INVCODE;
            Console.WriteLine("#Log sInvCode ::" + sInvCode);
        }
        public async Task TInvCode_onitemselected(dynamic Selected)
        {
            tInvCode = Selected.INVCODE;
            Console.WriteLine("#Log tInvCode ::" + tInvCode);

        }

        #endregion FunctionEvents

    }
}


namespace ApiServer.External.Services
{
    public partial class ShomaranPart
    {

        public static async Task<Result> CreateTrans(ShomaranApiMode ApiMode, TransInput trans)
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

            // Note: You may need to change the API endpoint "ShomaranHavale/InsertHavale"
            // to match the correct one for creating a "Havale".
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/InsertTrans", shomaranApi, ApplicationType.None);

            return apiresult;
        }

        public static async Task<Result> UpdateTrans(ShomaranApiMode ApiMode, TransUpdateInput trans)
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
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/UpadteTrans", shomaranApi, ApplicationType.None);

            return apiresult;
        }


        public static async Task<Result> DeleteTrans(ShomaranApiMode ApiMode, string factno, int year)
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
            Result apiresult = await Send.PostAsync(_content, $"ShomaranPart/DeleteTrans/{factno}/{year}", shomaranApi, ApplicationType.None);
            return apiresult;
        }


    }
}


public sealed class TransInput
{
    public string? Creator { get; set; } = "WEBSERVICE";
    public string? FactDate { get; set; } = "";   // keep as string (e.g., "1404/05/14" or "")
    public string? SInvCode { get; set; } = "";   // SINVCODE
    public string? TInvCode { get; set; } = "";   // TINVCODE
    public string? Note { get; set; } = "";      // TDESC
    public string? TempNo { get; set; } = "";
    //public string? WUser { get; set; } = "WEBSERVICE";
    public int? Year { get; set; }                // YEAR
    public List<TransDtlInput> Details { get; set; } = new();
}

public sealed class TransDtlInput
{
    public decimal? Amount { get; set; }          // AMOUNT
    //public decimal? Amount2 { get; set; }         // AMOUNT2
    public string? Note { get; set; } = "";       // NOTE
    public string? PartCode { get; set; } = "";
    public string? Radyabi { get; set; } = "";
    public string? Sefaresh { get; set; } = "";
    public string? RowOrder { get; set; } = "1";  // ROWORDER
    public int? Year { get; set; }                // YEAR
}


public sealed class TransUpdateInput
{
    public string FactNo { get; set; }        // @FACTNO (varchar(10)) - کلید رکورد موجود
    public string FactDate { get; set; }      // @FACTDATE (varchar(10)) مثال: 1404/06/01
    public string SInvCode { get; set; }      // @SINVCODE (varchar(20)) انبار مبدا
    public string TDesc { get; set; }         // @TDESC (varchar(20)) شرح فرم
    public string Note { get; set; }          // @NOTE (varchar(50)) توضیحات فرم
    public string TInvCode { get; set; }      // @TINVCODE (varchar(20)) انبار مقصد
    public string TempNo { get; set; }        // @TEMPNO (varchar(10)) شماره عطف
    public decimal Amount { get; set; }       // @AMOUNT (numeric(10,3)) مقدار ردیف
    public string PartCode { get; set; }      // @PARTCODE (varchar(10)) کد کالا
    public string NoteDetail { get; set; }    // @NOTEDetail (varchar(10)) شرح ردیف
    public string Radyabi { get; set; }       // @RADYABI (varchar(10))
    public string Sefaresh { get; set; }      // @SEFARESH (varchar(10))
    public int Year { get; set; }             // @YEAR (int)
}

