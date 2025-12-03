using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using Utility;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazored.Toast.Services;
using DocumentFormat.OpenXml.Bibliography;
using Entity;
using System.Text.Json;
using Baya.Models.ORM;
using Castle.DynamicLinqQueryBuilder;
using System.Globalization;

namespace Forms.Forms
{
    public class Form_979_CUBase : Form_979_CUPeropeties
    {

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
            bool addupdate = await AddAnnar();

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
            if(_Entity.CREATOR == null)
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

                        _Entity.CREATOR = en.UserName;
                        //_Entity.Year = تبدیل تاریخ شمسی و گرفتن سال
                        var pc = new PersianCalendar();
                        int persianYear = pc.GetYear(DateTime.Now);
                        _Entity.YEAR = persianYear;

                        StateHasChanged();
                    }

            }
        }


        #region FunctionEvents
        public async Task<bool> AddAnnar()
        {
            Console.WriteLine("#Log Start::");
            Console.WriteLine("#_Entity.CONCODE:" + _Entity.CONCODE);
            Console.WriteLine("#_Entity.Creator:" + _Entity.CREATOR);
            Console.WriteLine("#_Entity.AnnarrNo:" + _Entity.AnnarrNo);
            Console.WriteLine("#_Entity.OrderNo:" + _Entity.ORDERNO);
            Console.WriteLine("#_Entity.OrderYear:" + _Entity.ORDERYEAR);
            Console.WriteLine("#_Entity.Desc:" + _Entity.Description);
            Console.WriteLine("#_Entity.TempNo:" + _Entity.TEMPNO);
            Console.WriteLine("#_Entity.AnnarrDate:" + _Entity.ANNARRDATE);
            Console.WriteLine("#_Entity.YEAR:" + _Entity.YEAR);
            if (_Entity._IdIsEmpty.Value)
            {
                //// ثبت
                var annarDetails = _Entity.Shomaran_AnnarrDetail.Select(p => new AnnarrDetail()
                {
                    ArrAmount = Convert.ToDecimal(p.ARRAMOUNT),
                    //Amount1 = Convert.ToDecimal(p.AMOUNT1),
                    //RealAmount = Convert.ToDecimal(p.REALAMOUNT),
                    PartCode = p.PARTCODE,
                    //RecNo = p.RECNO,
                    //RecDate = p.RECDATE,
                    //Radyabi = p.RADYABI,
                    //Sefaresh = p.SEFARESH,
                    Desc2 = p.DESC2,
                    RowId = p.RowId,
                    Year = Convert.ToInt32(p.YEAR)
                }).ToList();


                foreach (var item in annarDetails)
                {
                    Console.WriteLine("#Detail {item.ArrAmount}:" + item.ArrAmount);
                    //Console.WriteLine("#Detail {item.Amount1}:" + item.Amount1);
                    //Console.WriteLine("#Detail {item.RealAmount}:" + item.RealAmount);
                    Console.WriteLine("#Detail {item.PartCode}:" + item.PartCode);
                    //Console.WriteLine("#Detail {item.RecNo}:" + item.RecNo);
                    //Console.WriteLine("#Detail {item.RecDate}:" + item.RecDate);
                    Console.WriteLine("#Detail {item.RowId}:" + item.RowId);
                    //Console.WriteLine("#Detail {item.Radyabi}:" + item.Radyabi);
                    //Console.WriteLine("#Detail {item.Sefaresh}:" + item.Sefaresh);
                    Console.WriteLine("#Detail {item.DESC2}:" + item.Desc2);
                    Console.WriteLine("#Detail {item.Year}:" + item.Year);
                }

                AnnarrHeader annarData = new()
                {
                    //
                    //CarNo = _Entity.CarNo,
                    //CarType = _Entity.CARTYPE,
                    //DriverName = _Entity.DRIVERNAME,
                    AnnarrDate = _Entity.ANNARRDATE,

                    Creator = _Entity.CREATOR,
                    ConCode = _Entity.CONCODE,
                    AnnarrNo = _Entity.AnnarrNo,
                    Desc = _Entity.Description,
                    OrderNo = _Entity.ORDERNO,
                    OrderYear = Convert.ToInt32(_Entity.ORDERYEAR),
                    TempNo = _Entity.TEMPNO,

                    //WUser = _Entity.WUSER,
                    Year = Convert.ToInt32(_Entity.YEAR),

                    Details = annarDetails,
                };


                Console.WriteLine("#Log depoOutData ::" + await JSON.ToJson(annarData));

                var data = await ApiServer.External.Services.ShomaranPart.CreateAnnar(ShomaranApiMode.Polfilm, annarData);


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
                    _Entity.AnnarrNo = number;

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
                //AnnarrUpdateInput depoInData = new()
                //{
                //    //
                //    AnnarrNo = _Entity.AnnarrNo,
                //    AnnarrDate = _Entity.ANNARRDATE,
                //    CarNo = _Entity.CarNo,
                //    CarType = _Entity.CARTYPE,//
                //    Desc = _Entity.Description,//
                //    DriverName = _Entity.DRIVERNAME,//
                //    TempNo = _Entity.TEMPNO,//
                //    ArrAmount = _Entity.ArrAmount,//
                //    PartCode = _Entity.PartCode,//
                //    RecNo = _Entity.RecNo,//
                //    RecYear = Convert.ToInt32(_Entity.RecYear),//
                //    Sefaresh = _Entity.Sefaresh,//
                //    RealAmount = _Entity.RealAmount,//
                //    Year = Convert.ToInt32(_Entity.YEAR),//
                //};

                //var data = await ApiServer.External.Services.ShomaranPart.UpdateAnnar(ShomaranApiMode.Polfilm, depoInData);

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

        public async Task<bool> GridShomaran_AnnarId_725_editmodelsaving(object e)
        {
            var Item = (Entity.Shomaran_AnnarrDetail)e;
            AnnarrUpdateInput depoInData = new()
            {
                //
                AnnarrNo = _Entity.AnnarrNo,
                ConCode = _Entity.CONCODE,
                AnnarrDate = _Entity.ANNARRDATE,
                //CarNo = _Entity.CarNo,
                //CarType = _Entity.CARTYPE,//
                Desc = _Entity.Description,//
                //DriverName = _Entity.DRIVERNAME,//
                TempNo = _Entity.TEMPNO,//
                ArrAmount = Convert.ToDecimal(Item.ARRAMOUNT),//
                PartCode = Item.PARTCODE,//
                //RecNo = Item.RECNO,//
                //RecYear = Convert.ToInt32(_Entity.RecYear),//
                //Sefaresh = Item.SEFARESH,//
                //Radyabi = Item.RADYABI,//
                //RealAmount = Convert.ToDecimal(Item.REALAMOUNT),//
                RowId = Convert.ToInt32(Item.RowId),
                Desc2 = Item.DESC2,
                Year = Convert.ToInt32(_Entity.YEAR),//
            };

            var data = await ApiServer.External.Services.ShomaranPart.UpdateAnnar(ShomaranApiMode.Polfilm, depoInData);

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

        #endregion FunctionEvents

    }
}



namespace ApiServer.External.Services
{
    public partial class ShomaranPart
    {

        public static async Task<Result> CreateAnnar(ShomaranApiMode ApiMode, AnnarrHeader depoIn)
        {
            // Serialize the entire havaleData object into a JSON string
            var DataJson = await JSON.ToJson(depoIn);

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
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/InsertAnnarr", shomaranApi, ApplicationType.None);

            return apiresult;
        }

        public static async Task<Result> UpdateAnnar(ShomaranApiMode ApiMode, AnnarrUpdateInput depoIn)
        {
            // Serialize the entire havaleData object into a JSON string
            var DataJson = await JSON.ToJson(depoIn);

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
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/UpdateAnnarr", shomaranApi, ApplicationType.None);

            return apiresult;
        }

        public static async Task<Result> DeleteAnnar(ShomaranApiMode ApiMode, string annarrno, int year)
        {
            var DataJson = await JSON.ToJson(new
            {
                annarrno,
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
            Result apiresult = await Send.PostAsync(_content, $"ShomaranPart/DeleteAnnar/{annarrno}/{year}", shomaranApi, ApplicationType.None);
            return apiresult;
        }

    }
}


public class AnnarrHeader
{
    public string? CarNo { get; set; }
    public string? ConCode { get; set; }
    public string? CarType { get; set; }
    public string? Creator { get; set; }
    public string? AnnarrNo { get; set; }
    public string? OrderNo { get; set; }
    public int OrderYear { get; set; }
    public string? DriverName { get; set; }
    public string? AnnarrDate { get; set; }
    public string? Desc { get; set; }
    public string? TempNo { get; set; }
    public string? WUser { get; set; }
    public int? Year { get; set; }

    public List<AnnarrDetail> Details { get; set; } = new();
}

public class AnnarrDetail
{
    public decimal ArrAmount { get; set; }
    public decimal Amount1 { get; set; }
    public string? PartCode { get; set; }
    public string? Radyabi { get; set; }
    public decimal RealAmount { get; set; }
    public string? RecNo { get; set; }
    public string? RecDate { get; set; }
    public string? Sefaresh { get; set; }
    public string? RowId { get; set; }
    public string? Desc2 { get; set; }
    public int Year { get; set; }
}




public class AnnarrUpdateInput
{
    public string? AnnarrNo { get; set; }
    public string? AnnarrDate { get; set; }
    public string? ConCode { get; set; }
    //public string CarNo { get; set; }
    //public string CarType { get; set; }
    //public string DriverName { get; set; }

    public string? Desc { get; set; }
    public string? Desc2 { get; set; }
    public string? TempNo { get; set; }
    public decimal ArrAmount { get; set; }
    public string? PartCode { get; set; }
    //public string RecNo { get; set; }
    //public int RecYear { get; set; }
    //public string Radyabi { get; set; }
    //public string Sefaresh { get; set; }
    public decimal RealAmount { get; set; }
    public int RowId { get; set; }
    public int Year { get; set; }
}

