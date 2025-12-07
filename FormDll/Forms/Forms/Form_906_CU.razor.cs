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
using Baya.Models.Utility.Entity;

namespace Forms.Forms
{
    public class Form_906_CUBase : Form_906_CUPeropeties
    {

        // تابع پیام توــست
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
            if(!await AddHavale()){
                return new Result() { Status = HttpStatusCode.InternalServerError };
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

        /// <summary>
        /// ثبت و ویرایش کالا در شماران با استفاده از استورد پروسیجر
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AddHavale()
        {
            Console.WriteLine("#Log Start::");
            Console.WriteLine("#_Entity.CENTCODE:" + _Entity.CENTCODE);
            Console.WriteLine("#_Entity.CENTCODE2:" + _Entity.CENTCODE2);
            Console.WriteLine("#_Entity.FORMCODE:" + _Entity.FORMCODE);
            Console.WriteLine("#_Entity.FORMYEAR:" + _Entity.FORMYEAR);
            Console.WriteLine("#_Entity.ORDERNO:" + _Entity.ORDERNO);
            Console.WriteLine("#_Entity.ORDERYEAR:" + _Entity.ORDERYEAR);
            Console.WriteLine("#_Entity.CREATOR:" + _Entity.CREATOR);
            Console.WriteLine("#_Entity.FACTDATE:" + _Entity.FACTDATE);
            Console.WriteLine("#_Entity.FACTNO:" + _Entity.FACTNO);
            Console.WriteLine("#_Entity.H_KIND:" + _Entity.H_KIND);
            Console.WriteLine("#_Entity.INVCODE:" + _Entity.INVCODE);
            Console.WriteLine("#_Entity.MAIN_MNT:" + _Entity.MAIN_MNT);
            Console.WriteLine("#_Entity.TEMPNO:" + _Entity.TEMPNO);
            Console.WriteLine("#_Entity.NOTE:" + _Entity.NOTE);
            Console.WriteLine("#_Entity.WUSER:" + _Entity.WUSER);
            Console.WriteLine("#_Entity.Year:" + _Entity.Year);
            if (_Entity._IdIsEmpty.Value)
            {
                //// ثبت
                var havaleDetails = _Entity.Shomaran_ProductDeliveryDetail.Select(p=> new HavaleDetailDto()
                {
                    Amount = Convert.ToDecimal(p.AMOUNT),
                    Amount2 = Convert.ToDecimal(p.AMOUNT2),
                    Code = p.CODE,
                    PartCode = p.PARTCODE,
                    Radyabi = p.RADYABI,
                    RowId = Convert.ToInt32(p.ROW_ID),
                    Sefaresh = p.SEFARESH,
                    Year = Convert.ToInt32(p.Year)
                }).ToList();
                
                
                foreach (var item in havaleDetails)
                {
                    Console.WriteLine("#Detail {item.Amount}:" + item.Amount);
                    Console.WriteLine("#Detail {item.Amount2}:" + item.Amount2);
                    Console.WriteLine("#Detail {item.Code}:" + item.Code);
                    Console.WriteLine("#Detail {item.PartCode}:" + item.PartCode);
                    Console.WriteLine("#Detail {item.Radyabi}:" + item.Radyabi);
                    Console.WriteLine("#Detail {item.RowId}:" + item.RowId);
                    Console.WriteLine("#Detail {item.Sefaresh}:" + item.Sefaresh);
                    Console.WriteLine("#Detail {item.Year}:" + item.Year);
                }
                
                HavaleDto havaleData = new()
                {
                    //
                    CentCode = _Entity.CENTCODE,
                    CentCode2 = _Entity.CENTCODE2,
                    Creator = _Entity.CREATOR,
                    FactDate = _Entity.FACTDATE,
                    FormCode = _Entity.FORMCODE,
                    FormYear = Convert.ToInt32(_Entity.FORMYEAR),
                    HKind = Convert.ToInt32(_Entity.H_KIND),
                    InvCode = _Entity.INVCODE,
                    MainMnt = Convert.ToDecimal(_Entity.MAIN_MNT),
                    Note = _Entity.NOTE,
                    OrderNo = _Entity.ORDERNO,
                    OrderYear = Convert.ToInt32(_Entity.ORDERYEAR),
                    TempNo = _Entity.TEMPNO,
                    WUser = _Entity.WUSER,
                    Year = Convert.ToInt32(_Entity.Year),

                    HavaleDetails = havaleDetails,
                };


                var data = await ApiServer.External.Services.ShomaranPart.CreateHavale(ShomaranApiMode.Polfilm, havaleData);


                Console.WriteLine("#Log End ::");
                Console.WriteLine("#Log status ::" + data.Status.ToString());
                string result = await JSON.ToJson(data);

                Console.WriteLine("#Log data ::" + result);
                if (data.Status == HttpStatusCode.OK)
                {
                    await _MSG.ShowSuccess(await JSON.ToJson(data.Content));
                    //_Entity.FACTNO = data.Content.ToString();
                    _Entity.ApiResult = data.Content.ToString();
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

        public static async Task<Result> CreateHavale(ShomaranApiMode ApiMode, HavaleDto havaleData)
        {
            // Serialize the entire havaleData object into a JSON string
            var DataJson = await JSON.ToJson(havaleData);

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
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/InsertHavale", shomaranApi, ApplicationType.None);

            return apiresult;
        }

        public static async Task<Result> UpdateHavale(ShomaranApiMode ApiMode, HavaleUpdateInput havaleData)
{
    // Serialize the entire havaleData object into a JSON string
    var DataJson = await JSON.ToJson(havaleData);

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
    Result apiresult = await Send.PostAsync(_content, "ShomaranPart/UpdateHavale", shomaranApi, ApplicationType.None);

    return apiresult;
}

         public static async Task<Result> DeleteHavale(ShomaranApiMode ApiMode, string factNo, int year)
        {
            Console.WriteLine("DeleteHavale2");
            var DataJson = await JSON.ToJson(new
            {
                factNo,
                year
            });
            Console.WriteLine("DeleteHavale2 1");
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
            Console.WriteLine("DeleteHavale2 3");
            var _content = new StringContent(DataJson, Encoding.UTF8, "application/json");
            Result apiresult = await Send.PostAsync(_content, $"ShomaranPart/DeleteHavale/{factNo}/{year}", shomaranApi, ApplicationType.None);
            Console.WriteLine("DeleteHavale2 4");
            return apiresult;
        }
    }
}

public class HavaleDetailDto
{
    public decimal Amount { get; set; }
    public decimal Amount2 { get; set; }
    public string Code { get; set; }
    public string PartCode { get; set; }
    public string Radyabi { get; set; }
    public string Sefaresh { get; set; }
    public int RowId { get; set; }
    public int Year { get; set; }
}

// Represents the main JSON payload
public class HavaleDto
{
    public string CentCode { get; set; }
    public string CentCode2 { get; set; }
    public string FormCode { get; set; }
    public int FormYear { get; set; }
    public string OrderNo { get; set; }
    public int OrderYear { get; set; }
    public string Creator { get; set; }
    public string FactDate { get; set; }
    public int HKind { get; set; }
    public string InvCode { get; set; }
    public decimal MainMnt { get; set; }
    public string TempNo { get; set; }
    public string Note { get; set; }
    public string WUser { get; set; }
    public int Year { get; set; }
    public List<HavaleDetailDto> HavaleDetails { get; set; }
}

public class HavaleUpdateInput
{
    // Tfw_Havale Parameters
    public string? FactNo { get; set; }
    public string? CentCode { get; set; }
    public string? FactDate { get; set; }
    public string? InvCode { get; set; }
    public int HKind { get; set; }
    public decimal MainMnt { get; set; }
    public string? Note { get; set; }
    public string? OrderNo { get; set; }
    public int OrderYear { get; set; }
    public string? TempNo { get; set; }
    public string? FormCode { get; set; }
    public int FormYear { get; set; }

    // Tfw_HavaleDtl Parameters
    public decimal Amount { get; set; }
    public string? BabCode { get; set; }
    public string? Code { get; set; }
    public string? PartCode { get; set; }
    public string? PayCode { get; set; }
    public int RowId { get; set; }
    // Common Parameter
    public int Year { get; set; }
}

