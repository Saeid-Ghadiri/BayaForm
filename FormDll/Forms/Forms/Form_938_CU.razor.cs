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

namespace Forms.Forms
{
    public class Form_938_CUBase : Form_938_CUPeropeties
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
            Console.WriteLine("#_Entity.CLOSED:" + _Entity.CLOSED2Navigation.Code);
            Console.WriteLine("#_Entity.FORMCODE:" + _Entity.FORMCODE);
            Console.WriteLine("#_Entity.FORMYEAR:" + _Entity.FORMYEAR);
            Console.WriteLine("#_Entity.REQPERSON:" + _Entity.REQPERSON);
            Console.WriteLine("#_Entity.ORDERNO:" + _Entity.ORDERNO);
            Console.WriteLine("#_Entity.ORDERYEAR:" + _Entity.ORDERYEAR);
            Console.WriteLine("#_Entity.Creator:" + _Entity.CREATOR);
            Console.WriteLine("#_Entity.INVCODE:" + _Entity.INVCODE);
            Console.WriteLine("#_Entity.MAIN_MNT:" + _Entity.MAIN_MNT);
            Console.WriteLine("#_Entity.NDATE:" + _Entity.NDATE);
            Console.WriteLine("#_Entity.ORDERDATE:" + _Entity.ORDERDATE);
            Console.WriteLine("#_Entity.OKFACTDATE:" + _Entity.OKFACTDATE);
            Console.WriteLine("#_Entity.OK_BTN:" + _Entity.OK_BTN2Navigation.Code);
            Console.WriteLine("#_Entity.ORDWANTNO:" + _Entity.ORDWANTNO);
            Console.WriteLine("#_Entity.PSOURCE:" + _Entity.PSOURCE2Navigation.Code);
            Console.WriteLine("#_Entity.TEMPNO:" + _Entity.TEMPNO);
            Console.WriteLine("#_Entity.NOTE:" + _Entity.NOTE);
            Console.WriteLine("#_Entity.WUSER:" + _Entity.WUSER);
            Console.WriteLine("#_Entity.YEAR:" + _Entity.YEAR);
            if (_Entity._IdIsEmpty.Value)
            {
                //// ثبت
                var anbordDetails = _Entity.Shomaran_AnbordDetail.Select(p => new AnbordDetailDto()
                {
                    OkAmount = Convert.ToDecimal(p.OKAMOUNT),
                    OrdAmount = Convert.ToDecimal(p.ORDAMOUNT),
                    OrdAmount1 = Convert.ToDecimal(p.ORDAMOUNT1),
                    PartCode = p.PARTCODE,
                    Radyabi = p.RADYABI,
                    RowId = Convert.ToInt32(p.RowId),
                    Sefaresh = p.SEFARESH,
                    Year = Convert.ToInt32(p.Year)
                }).ToList();


                foreach (var item in anbordDetails)
                {
                    Console.WriteLine("#Detail {item.OkAmount}:" + item.OkAmount);
                    Console.WriteLine("#Detail {item.OrdAmount}:" + item.OrdAmount);
                    Console.WriteLine("#Detail {item.OrdAmount1}:" + item.OrdAmount1);
                    Console.WriteLine("#Detail {item.PartCode}:" + item.PartCode);
                    Console.WriteLine("#Detail {item.Radyabi}:" + item.Radyabi);
                    Console.WriteLine("#Detail {item.RowId}:" + item.RowId);
                    Console.WriteLine("#Detail {item.Sefaresh}:" + item.Sefaresh);
                    Console.WriteLine("#Detail {item.Year}:" + item.Year);
                }

                AnbordDto anbordData = new()
                {
                    //
                    CentCode = _Entity.CENTCODE,
                    Closed = Convert.ToInt32(_Entity.CLOSED2Navigation.Code),
                    FormCode = _Entity.FORMCODE2,
                    FormYear = Convert.ToInt32(_Entity.FORMYEAR),
                    ReqPerson = _Entity.REQPERSON,
                    OrderNo = _Entity.ORDERNO,
                    OrderYear = Convert.ToInt32(_Entity.ORDERYEAR),
                    Creator = _Entity.CREATOR,
                    NDate = _Entity.NDATE,
                    OrderDate = _Entity.ORDERDATE,
                    OkFactDate = _Entity.OKFACTDATE,
                    OkBtn = Convert.ToInt32(_Entity.OK_BTN2Navigation.Code),
                    OrdWantNo = _Entity.ORDWANTNO,
                    Psource = Convert.ToInt32(_Entity.PSOURCE2Navigation.Code),
                    InvCode = _Entity.INVCODE,
                    MainMnt = Convert.ToDecimal(_Entity.MAIN_MNT),
                    TempNo = _Entity.TEMPNO,
                    Desc = _Entity.Description,
                    WUser = _Entity.WUSER,
                    Year = Convert.ToInt32(_Entity.YEAR),

                    AnbordDetails = anbordDetails,
                };


                Console.WriteLine("#Log anbordData ::" + await JSON.ToJson(anbordData));

                var data = await ApiServer.External.Services.ShomaranPart.CreateAnbord(ShomaranApiMode.Polfilm, anbordData);


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
                //// آپدیت
                UpdateAnbordInput anbordDto = new()
                {
                    //
                    //FactNo = _Entity.FactNo,//
                    OrderNo = _Entity.ORDERNO,
                    Closed = Convert.ToInt32(_Entity.CLOSED2Navigation.Code),
                    CentCode = _Entity.CENTCODE,
                    NDate = _Entity.NDATE,
                    OkFactDate = _Entity.OKFACTDATE,
                    InvCode = _Entity.INVCODE,
                    OkBtn = Convert.ToInt32(_Entity.OK_BTN2Navigation.Code),
                    MainMnt = Convert.ToDecimal(_Entity.MAIN_MNT),
                    OrderDate = _Entity.ORDERDATE,
                    PSource = Convert.ToInt32(_Entity.PSOURCE2Navigation.Code),
                    ReqPerson = _Entity.REQPERSON,
                    TempNo = _Entity.TEMPNO,
                    FormCode = _Entity.FORMCODE,
                    FormYear = Convert.ToInt32(_Entity.FORMYEAR),

                    Desc = _Entity.Description,
                    Moj = Convert.ToDecimal(_Entity.MOJ),
                    Desc1 = _Entity.DESC1,
                    OkAmount = Convert.ToDecimal(_Entity.OKAMOUNT),
                    OrdAmount = Convert.ToDecimal(_Entity.ORDAMOUNT),
                    Radyabi = _Entity.RADYABI,
                    PartCode = _Entity.PARTCODE,
                    Sefaresh = _Entity.SEFARESH,
                    Year = Convert.ToInt32(_Entity.YEAR),
                };

                var data = await ApiServer.External.Services.ShomaranPart.UpdateAnbord(ShomaranApiMode.Polfilm, anbordDto);

                if (data.Status == HttpStatusCode.OK)
                {
                    await _MSG.ShowSuccess(await JSON.ToJson(data.Content));
                    return true;
                }
                else
                {
                    await _MSG.ShowError(await JSON.ToJson(data.Content));
                    return false;
                }
            }

            return true;
        }
        #endregion FunctionEvents

    }
}


namespace ApiServer.External.Services
{
    public partial class ShomaranPart
    {

        public static async Task<Result> CreateAnbord(ShomaranApiMode ApiMode, AnbordDto anbordDto)
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

        public static async Task<Result> UpdateAnbord(ShomaranApiMode ApiMode, UpdateAnbordInput anbordDto)
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

        public static async Task<Result> DeleteAnbord(ShomaranApiMode ApiMode, string orderNo, int year)
        {
            var DataJson = await JSON.ToJson(new
            {
                orderNo,
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
            Result apiresult = await Send.PostAsync(_content, $"ShomaranPart/DeleteAnbord/{orderNo}/{year}", shomaranApi, ApplicationType.None);
            return apiresult;
        }
    }
}


public class AnbordDto
{
    public string? CentCode { get; set; } = "2103";
    public int Closed { get; set; } = 1;
    public string? FormCode { get; set; } = "";
    public int? FormYear { get; set; } = 0;
    public string? ReqPerson { get; set; }
    public string? OrderNo { get; set; } = "";
    public int? OrderYear { get; set; } = 0;
    public string? Creator { get; set; } = "WEBSERVICE";
    public string? NDate { get; set; }
    public string? OrderDate { get; set; }
    public string? OkFactDate { get; set; }
    public int OkBtn { get; set; } = 2;
    public string? OrdWantNo { get; set; }
    public int? Psource { get; set; }
    public string? InvCode { get; set; } = "07";
    public decimal MainMnt { get; set; } = 1;
    public string? TempNo { get; set; }
    public string? Desc { get; set; }
    public string? WUser { get; set; } = "WEBSERVICE";
    public int Year { get; set; }
    public List<AnbordDetailDto> AnbordDetails { get; set; } = new List<AnbordDetailDto>();
}

public class AnbordDetailDto
{
    public decimal OkAmount { get; set; }
    public decimal OrdAmount { get; set; }
    public decimal OrdAmount1 { get; set; } = 0;
    public string? PartCode { get; set; }
    public string? Radyabi { get; set; } = "";
    public string? Sefaresh { get; set; } = "";
    public string? Desc1 { get; set; } = "";
    public int RowId { get; set; }
    public int Year { get; set; }
}

public class UpdateAnbordInput
{
    public string OrderNo { get; set; }
    public string? Creator { get; set; }
    public int Year { get; set; }
    public int RowId { get; set; }
    public int Closed { get; set; }
    public string? CentCode { get; set; }
    public string? NDate { get; set; }
    public string? OkFactDate { get; set; }
    public string? InvCode { get; set; }
    public int OkBtn { get; set; }
    public decimal MainMnt { get; set; }
    public string? OrderDate { get; set; }
    public int PSource { get; set; }
    public string? ReqPerson { get; set; }
    public string? Desc { get; set; }
    public string? TempNo { get; set; }
    public string? FormCode { get; set; }
    public int FormYear { get; set; }
    public decimal Moj { get; set; }
    public string? Desc1 { get; set; }
    public decimal OkAmount { get; set; }
    public decimal OrdAmount { get; set; }
    public string? Radyabi { get; set; }
    public string? PartCode { get; set; }
    public string? Sefaresh { get; set; }
}