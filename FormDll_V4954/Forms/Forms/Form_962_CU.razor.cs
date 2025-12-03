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
    public class Form_962_CUBase : Form_962_CUPeropeties
    {

        // تابع پیام توــست
        public MSG _MSG { get; set; }
        private string fORMCODE = " ";

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
            bool addupdate = await AddAnbord();

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
        /// <summary>
        /// ثبت خرید در شماران با استفاده از استورد پروسیجر
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AddAnbord()
        {
            Console.WriteLine("#Log Start::");
            Console.WriteLine("#_Entity.CENTCODE:" + _Entity.CENTCODE);
            Console.WriteLine("#_Entity.Creator:" + _Entity.CREATOR);
            Console.WriteLine("#_Entity.REQPERSON:" + _Entity.REQPERSON);
            Console.WriteLine("#_Entity.ORDERDATE:" + _Entity.ORDERDATE);
            Console.WriteLine("#_Entity.OKFACTDATE:" + _Entity.OKFACTDATE);
            Console.WriteLine("#_Entity.INVCODE:" + _Entity.INVCODE);
            Console.WriteLine("#_Entity.TEMPNO:" + _Entity.TEMPNO);
            Console.WriteLine("#_Entity.Decs:" + _Entity.Description);
            Console.WriteLine("#_Entity.PSOURCE:" + _Entity.PSOURCENavigation.Code);
            Console.WriteLine("#_Entity.YEAR:" + _Entity.YEAR);

            //Console.WriteLine("#_Entity.CLOSED:" + _Entity.CLOSEDNavigation.Code);
            //Console.WriteLine("#_Entity.FORMCODE:" + _Entity.FORMCODE);
            //Console.WriteLine("#_Entity.FORMYEAR:" + _Entity.FORMYEAR);
            //Console.WriteLine("#_Entity.ORDERNO:" + _Entity.ORDERNO);
            //Console.WriteLine("#_Entity.ORDERYEAR:" + _Entity.ORDERYEAR);
            //Console.WriteLine("#_Entity.MAIN_MNT:" + _Entity.MAIN_MNT);
            //Console.WriteLine("#_Entity.NDATE:" + _Entity.NDATE);
            //Console.WriteLine("#_Entity.OK_BTN:" + _Entity.OK_BTNNavigation.Code);
            //Console.WriteLine("#_Entity.ORDWANTNO:" + _Entity.ORDWANTNO);
            //Console.WriteLine("#_Entity.NOTE:" + _Entity.NOTE);
            //Console.WriteLine("#_Entity.WUSER:" + _Entity.WUSER);
            if (_Entity._IdIsEmpty.Value || string.IsNullOrWhiteSpace(_Entity.CENTCODE))
            {
                //// ثبت
                var anbordDetails = _Entity.SH_PolFilm_AnbordDetail.Select(p => new AnbordDetailDto()
                {
                    OkAmount = Convert.ToDecimal(p.OKAMOUNT),
                    OrdAmount = Convert.ToDecimal(p.ORDAMOUNT),
                    //OrdAmount1 = Convert.ToDecimal(p.ORDAMOUNT1),
                    PartCode = p.PARTCODE,
                    //Radyabi = p.RADYABI,
                    //Sefaresh = p.SEFARESH,
                    RowId = Convert.ToInt32(p.RowId),
                    Desc1 = p.DESC1,
                    Year = Convert.ToInt32(p.Year)
                }).ToList();


                foreach (var item in anbordDetails)
                {
                    Console.WriteLine("#Detail {item.OkAmount}:" + item.OkAmount);
                    Console.WriteLine("#Detail {item.OrdAmount}:" + item.OrdAmount);
                    //Console.WriteLine("#Detail {item.OrdAmount1}:" + item.OrdAmount1);
                    Console.WriteLine("#Detail {item.PartCode}:" + item.PartCode);
                    Console.WriteLine("#Detail {item.Radyabi}:" + item.Radyabi);
                    Console.WriteLine("#Detail {item.RowId}:" + item.RowId);
                    Console.WriteLine("#Detail {item.Sefaresh}:" + item.Sefaresh);
                    Console.WriteLine("#Detail {item.Year}:" + item.Year);
                }

                fORMCODE = _Entity.FORMCODE != null ? _Entity.FORMCODE : " ";
                AnbordDto anbordData = new()
                {
                    //
                    CentCode = _Entity.CENTCODE,
                    Creator = _Entity.CREATOR,
                    ReqPerson = _Entity.REQPERSON,
                    OrderDate = _Entity.ORDERDATE,
                    OkFactDate = _Entity.OKFACTDATE,
                    Psource = Convert.ToInt32(_Entity.PSOURCENavigation.Code),
                    InvCode = _Entity.INVCODE,
                    TempNo = _Entity.TEMPNO,
                    Desc = _Entity.Description,
                    Year = Convert.ToInt32(_Entity.YEAR),

                    //Closed = Convert.ToInt32(_Entity.CLOSEDNavigation.Code),
                    //FormCode = fORMCODE,
                    //FormYear = Convert.ToInt32(_Entity.FORMYEAR),
                    //OrderNo = _Entity.ORDERNO,
                    //OrderYear = Convert.ToInt32(_Entity.ORDERYEAR),
                    //NDate = _Entity.NDATE,
                    //OkBtn = Convert.ToInt32(_Entity.OK_BTNNavigation.Code),
                    //OrdWantNo = _Entity.ORDWANTNO,
                    //MainMnt = Convert.ToDecimal(_Entity.MAIN_MNT),
                    //WUser = _Entity.WUSER,

                    AnbordDetails = anbordDetails,
                };


                Console.WriteLine("#Log anbordData ::" + await JSON.ToJson(anbordData));

                var data = await ApiServer.External.Services.ShomaranPart.CreateAnbord2(ShomaranApiMode.Polfilm, anbordData);


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
                    _Entity.ORDERNO = number;

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
                //UpdateAnbordInput anbordDto = new()
                //{
                //    //
                //    //FactNo = _Entity.FactNo,//
                //    OrderNo = _Entity.ORDERNO,
                //    Closed = Convert.ToInt32(_Entity.CLOSEDNavigation.Code),
                //    CentCode = _Entity.CENTCODE,
                //    NDate = _Entity.NDATE,
                //    OkFactDate = _Entity.OKFACTDATE,
                //    InvCode = _Entity.INVCODE,
                //    OkBtn = Convert.ToInt32(_Entity.OK_BTNNavigation.Code),
                //    MainMnt = Convert.ToDecimal(_Entity.MAIN_MNT),
                //    OrderDate = _Entity.ORDERDATE,
                //    PSource = Convert.ToInt32(_Entity.PSOURCENavigation.Code),
                //    ReqPerson = _Entity.REQPERSON,
                //    TempNo = _Entity.TEMPNO,
                //    FormCode = _Entity.FORMCODE,
                //    FormYear = Convert.ToInt32(_Entity.FORMYEAR),

                //    Desc = _Entity.Description,
                //    Moj = Convert.ToDecimal(_Entity.MOJ),
                //    Desc1 = _Entity.DESC1,
                //    OkAmount = Convert.ToDecimal(_Entity.OKAMOUNT),
                //    OrdAmount = Convert.ToDecimal(_Entity.ORDAMOUNT),
                //    Radyabi = _Entity.RADYABI,
                //    PartCode = _Entity.PARTCODE,
                //    Sefaresh = _Entity.SEFARESH,
                //    Year = Convert.ToInt32(_Entity.YEAR),
                //};

                //var data = await ApiServer.External.Services.ShomaranPart.UpdateAnbord2(ShomaranApiMode.Polfilm, anbordDto);

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


        public async Task<bool> GridSH_PolFilm_AnbordId_708_editmodelsaving(object e)
        {
            var Item = (Entity.SH_PolFilm_AnbordDetail)e;
            if (!Item._IdIsEmpty.Value) {
                //// آپدیت
                UpdateAnbordInput anbordDto = new()
                {
                    //
                    OrderNo = _Entity.ORDERNO,//_Entity.ORDERNO,
                    CentCode = _Entity.CENTCODE,
                    InvCode = _Entity.INVCODE,
                    Creator = _Entity.CREATOR,
                    OrderDate = _Entity.ORDERDATE,
                    ReqPerson = _Entity.REQPERSON,
                    TempNo = _Entity.TEMPNO,
                    Desc = _Entity.Description,

                    //Closed = Convert.ToInt32(_Entity.CLOSEDNavigation.Code),
                    //NDate = _Entity.NDATE,
                    //OkFactDate = _Entity.OKFACTDATE,
                    //OkBtn = Convert.ToInt32(_Entity.OK_BTNNavigation.Code),
                    //MainMnt = Convert.ToDecimal(_Entity.MAIN_MNT),
                    //PSource = Convert.ToInt32(_Entity.PSOURCENavigation.Code),
                    //FormCode = _Entity.FORMCODE,
                    //FormYear = Convert.ToInt32(_Entity.FORMYEAR),

                    //Moj = Convert.ToDecimal(Item.MOJ),

                    Desc1 = Item.DESC1,
                    OkAmount = Convert.ToDecimal(Item.OKAMOUNT),
                    //OrdAmount = Convert.ToDecimal(Item.ORDAMOUNT),
                    //Radyabi = Item.RADYABI,
                    //Sefaresh = Item.SEFARESH,
                    PartCode = Item.PARTCODE,
                    Year = Convert.ToInt32(_Entity.YEAR),
                };
                Console.WriteLine(await JSON.ToJson(anbordDto));
                var data = await ApiServer.External.Services.ShomaranPart.UpdateAnbord2(ShomaranApiMode.Polfilm, anbordDto);

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

        public static async Task<Result> CreateAnbord2(ShomaranApiMode ApiMode, AnbordDto anbordDto)
        {
            // Serialize the entire havaleData object into a JSON string
            var DataJson = await JSON.ToJson(anbordDto);

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
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/InsertAnbord", shomaranApi, ApplicationType.None);

            return apiresult;
        }

        public static async Task<Result> UpdateAnbord2(ShomaranApiMode ApiMode, UpdateAnbordInput anbordDto)
        {
            // Serialize the entire havaleData object into a JSON string
            var DataJson = await JSON.ToJson(anbordDto);

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
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/UpdateAnbord", shomaranApi, ApplicationType.None);

            return apiresult;
        }
    }
}
