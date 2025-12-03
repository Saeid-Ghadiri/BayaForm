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
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Text.Json;
using Baya.Models.ORM;
using Castle.DynamicLinqQueryBuilder;
using System.Globalization;

namespace Forms.Forms
{
    public class Form_978_CUBase : Form_978_CUPeropeties
    {
        public string formCode = " ";
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
            bool addupdate = await AddDepoOut();

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
        public async Task<bool> AddDepoOut()
        {
            Console.WriteLine("#Log Start::");
            Console.WriteLine("#_Entity.Havno:" + _Entity.Havno);
            Console.WriteLine("#_Entity.Havdate:" + _Entity.HAVDATE);
            Console.WriteLine("#_Entity.MainMnt:" + _Entity.MainMnt);
            Console.WriteLine("#_Entity.Product:" + _Entity.Product);
            Console.WriteLine("#_Entity.Receipt:" + _Entity.Receipt);
            Console.WriteLine("#_Entity.TempNo:" + _Entity.TempNo);
            Console.WriteLine("#_Entity.Year:" + _Entity.Year);
            Console.WriteLine("#_Entity.Note:" + _Entity.Note);
            if (_Entity._IdIsEmpty.Value)
            {
                //// ثبت
                var depoOutDetails = _Entity.Shomaran_DepoOutDetail.Select(p => new DepoOutDetail()
                {
                    Amount = Convert.ToDecimal(p.AMOUNT),
                    //Amount2 = Convert.ToDecimal(p.AMOUNT2),
                    //Havno = p.Havno,
                    InvCode = p.InvCode,
                    PartCode = p.PartCode,
                    Radyabi = p.Radyabi,
                    Sefaresh = p.Sefaresh,
                    RowId = p.RowId,
                    Year = Convert.ToInt32(p.Year)
                }).ToList();


                foreach (var item in depoOutDetails)
                {
                    Console.WriteLine("#Detail {item.Amount}:" + item.Amount);
                    Console.WriteLine("#Detail {item.Amount2}:" + item.Amount2);
                    //Console.WriteLine("#Detail {item.Havno}:" + item.Havno);
                    Console.WriteLine("#Detail {item.InvCode}:" + item.InvCode);
                    Console.WriteLine("#Detail {item.PartCode}:" + item.PartCode);
                    Console.WriteLine("#Detail {item.Radyabi}:" + item.Radyabi);
                    Console.WriteLine("#Detail {item.RowId}:" + item.RowId);
                    Console.WriteLine("#Detail {item.Sefaresh}:" + item.Sefaresh);
                    Console.WriteLine("#Detail {item.Year}:" + item.Year);
                }

                DepoOutHeader depoOutData = new()
                {
                    //
                    Creator = _Entity.Creator,
                   // Havno = _Entity.Havno,
                    Havdate = _Entity.HAVDATE,
                    //MainMnt = _Entity.MainMnt,
                    Product = _Entity.Product,
                    Receipt = _Entity.Receipt,
                    TempNo = _Entity.TempNo,
                    Year = Convert.ToInt32(_Entity.Year),
                    Note = _Entity.Note,

                    Details = depoOutDetails,
                };


                Console.WriteLine("#Log depoOutData ::" + await JSON.ToJson(depoOutData));

                var data = await ApiServer.External.Services.ShomaranPart.CreateDepoOut(ShomaranApiMode.Polfilm, depoOutData);


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
                    //             .GetProperty("HavaleNumber")[0]
                    //             .GetString()
                    //             ?.Trim();   // remove spaces
                    // _Entity.Havno = number;

                    using var doc = JsonDocument.Parse(data.Content.ToString());
                    var number = doc.RootElement
                                    .GetProperty("HavaleNumber")
                                    .GetString()
                                    ?.Trim();   // remove spaces if any

                    _Entity.Havno = number;

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
                //UpdateDepoOutRequest depoInData = new()
                //{
                //    //
                //    MainMnt = Convert.ToDecimal(_Entity.MainMnt),
                //    HavNo = _Entity.Havno,
                //    Receipt = _Entity.Receipt,
                //    TempNo = _Entity.TempNo,
                //    FormCode = _Entity.FormCode,//
                //    FormYear = Convert.ToInt32(_Entity.FormYear),//
                //    HavDate = _Entity.HAVDATE,
                //    InvCode = _Entity.InvCode,//
                //    Amount = _Entity.Amount,//
                //    Note = "", //_Entity.Note,
                //    PartCode = _Entity.PartCode,//
                //    Radyabi = _Entity.Radyabi,//
                //    Sefaresh = _Entity.Sefaresh,//
                //    Product = _Entity.Product.ToString(),
                //    Year = Convert.ToInt32(_Entity.Year),
                //};

                //var data = await ApiServer.External.Services.ShomaranPart.UpdateDepoOut(ShomaranApiMode.Polfilm, depoInData);

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

        public async Task<bool> GridShomaran_DepoOutId_723_editmodelsaving(object e)
        {
            var Item = (Entity.Shomaran_DepoOutDetail)e;
            if (!Item._IdIsEmpty.Value)
            {
                //// آپدیت
                UpdateDepoOutRequest depoInData = new()
                {
                    //
                    //MainMnt = Convert.ToDecimal(_Entity.MainMnt),
                    HavNo = _Entity.Havno,
                    HavDate = _Entity.HAVDATE,
                    Receipt = _Entity.Receipt,
                    TempNo = _Entity.TempNo,
                    Product = _Entity.Product.ToString(),

                    Note = _Entity.Note,
                   // FormCode = _Entity.FormCode,//formCode,//
                   // FormYear =  Convert.ToInt32(_Entity.FormYear),//
                    InvCode = Item.InvCode,//
                    Amount = Convert.ToDecimal(Item.AMOUNT),//
                    PartCode = Item.PartCode,//
                    Radyabi = Item.Radyabi,//
                    Sefaresh = Item.Sefaresh,//
                    NoteD = Item.NoteD,//
                    RowId = Convert.ToInt32(Item.RowId),
                    Year = Convert.ToInt32(Item.Year),

                };

                var data = await ApiServer.External.Services.ShomaranPart.UpdateDepoOut(ShomaranApiMode.Polfilm, depoInData);

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

        public static async Task<Result> CreateDepoOut(ShomaranApiMode ApiMode, DepoOutHeader depoIn)
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
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/InsertDepoOut", shomaranApi, ApplicationType.None);

            return apiresult;
        }

        public static async Task<Result> UpdateDepoOut(ShomaranApiMode ApiMode, UpdateDepoOutRequest depoIn)
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
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/UpdateDepoOut", shomaranApi, ApplicationType.None);

            return apiresult;
        }


        public static async Task<Result> DeleteDepoOut(ShomaranApiMode ApiMode, string havno, int year)
        {
            var DataJson = await JSON.ToJson(new
            {
                havno,
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
            Result apiresult = await Send.PostAsync(_content, $"ShomaranPart/DeleteDepoOut/{havno}/{year}", shomaranApi, ApplicationType.None);
            return apiresult;
        }


    }
}


// Represents the main header record (<TFW_DEPO_OUT>)
public class DepoOutHeader
{
    // Key properties
    //public string? Havno { get; set; }        // Dispatch Number (leave empty to generate new)
    public string? Havdate { get; set; }      // Dispatch Date (e.g., "1404/03/10")
    public string? MainMnt { get; set; }      
    public string? Product { get; set; }      
    public string? Receipt { get; set; }      
    public string? TempNo { get; set; }      
    public int Year { get; set; }

    public string? Creator { get; set; }      // User creating the record
    public string? WUser { get; set; }        // Same as creator

    // Other common properties
    public string? Note { get; set; }
    public string? BranchCode { get; set; }

    // This list will hold all the detail lines for this dispatch
    public List<DepoOutDetail> Details { get; set; } = new List<DepoOutDetail>();
}

// Represents a single detail line (<TFW_DEPO_OUTDTL>)
public class DepoOutDetail
{
    // Key properties
    public decimal? Amount { get; set; }
    public string? Havno { get; set; }
    public decimal Amount2 { get; set; }
    public string? InvCode { get; set; }      // Inventory Code is in the detail line here
    public string? PartCode { get; set; }

    public string? Radyabi { get; set; }
    public string? Sefaresh { get; set; }     // Order number
    public string? RowId { get; set; } 

    // Other common properties
    public int Year { get; set; }
    //public string? Note { get; set; }
}




public class UpdateDepoOutRequest
{
    public string? HavNo { get; set; }
    public decimal MainMnt { get; set; }
    public string? Receipt { get; set; }
    public string? TempNo { get; set; }
    //public string? FormCode { get; set; }
    //public int FormYear { get; set; }
    public string? HavDate { get; set; }   // Jalali date
    public string? InvCode { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public string? PartCode { get; set; }
    public string? Radyabi { get; set; }
    public string? Sefaresh { get; set; }
    public string? Product { get; set; }
    public string? NoteD { get; set; }
    public int RowId { get; set; }
    public int Year { get; set; }
}
