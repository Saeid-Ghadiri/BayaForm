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

namespace Forms.Forms
{
    public class Form_974_CUBase : Form_974_CUPeropeties
    {

        public MSG _MSG { get; set; }
        public string invCode = "";
        public string formCode = "";
        public string partCode = "";
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
            bool addupdate = await AddDepoIn();

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
        public async Task<bool> AddDepoIn()
        {
            Console.WriteLine("#Log Start::");
            Console.WriteLine("#_Entity.RecNo:" + _Entity.RecNo);
            Console.WriteLine("#_Entity.RecDate:" + _Entity.RecDate);
            Console.WriteLine("#_Entity.Amount:" + _Entity.Amount);
            //Console.WriteLine("#_Entity.Amount2:" + _Entity.Amount2);
            Console.WriteLine("#_Entity.PartCode:" + _Entity.PartCode);
            Console.WriteLine("#_Entity.InvCode:" + _Entity.InvCode);
            Console.WriteLine("#_Entity.Product:" + _Entity.Product);
            Console.WriteLine("#_Entity.QC:" + _Entity.QC);
            Console.WriteLine("#_Entity.Receipt:" + _Entity.Receipt);
            Console.WriteLine("#_Entity.TempNo:" + _Entity.TempNo);
            Console.WriteLine("#_Entity.Year:" + _Entity.Year);
            if (_Entity._IdIsEmpty.Value)
            {
                //// ثبت
                var depoInDetails = _Entity.Shomaran_DepoInDetail.Select(p => new DepoInDetailInput()
                {
                    Amount = Convert.ToDecimal(p.Amount),
                    //Amount2 = Convert.ToDecimal(p.Amount2),
                    //RecNo = p.RecNo,
                    PartCode = p.PartCode,
                    Radyabi = p.Radyabi,
                    Sefaresh = p.Sefaresh,
                    TgReceipt = p.RowId,
                    Note = p.Note,
                    Year = Convert.ToInt32(p.Year)
                }).ToList();


                foreach (var item in depoInDetails)
                {
                    Console.WriteLine("#Detail {item.Amount}:" + item.Amount);
                    Console.WriteLine("#Detail {item.Amount2}:" + item.Amount2);
                    Console.WriteLine("#Detail {item.PartCode}:" + item.PartCode);
                    Console.WriteLine("#Detail {item.Radyabi}:" + item.Radyabi);
                    Console.WriteLine("#Detail {item.RowId}:" + item.TgReceipt);
                    Console.WriteLine("#Detail {item.Sefaresh}:" + item.Sefaresh);
                    Console.WriteLine("#Detail {item.Year}:" + item.Year);
                }

                DepoInInput depoInData = new()
                {
                    //
                    RecDate = _Entity.RecDate,
                    //RecNo = _Entity.RecNo,
                    Amount = _Entity.Amount,
                    //Amount2 = _Entity.Amount2,
                    Desc = _Entity.Description,
                    PartCode = _Entity.PartCode,//partCode,
                    Product = _Entity.Product,
                    InvCode = _Entity.InvCode,//invCode,
                    QC = _Entity.QC,
                    Receipt = _Entity.Receipt,
                    TempNo = _Entity.TempNo,
                    Year = _Entity.Year,

                    Details = depoInDetails,
                };


                Console.WriteLine("#Log depoInData ::" + await JSON.ToJson(depoInData));

                var data = await ApiServer.External.Services.ShomaranPart.CreateDepoIn(ShomaranApiMode.Polfilm, depoInData);


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

            }

            return true;
        }



        public async Task InvCode_onitemselected(dynamic Selected)
        {

            invCode = Selected.INVCODE;
            Console.WriteLine("#Log invCode ::" + invCode);
        }
        public async Task FormCode_onitemselected(dynamic Selected)
        {

            formCode = Selected.FORMCODE;
            Console.WriteLine("#Log formCode ::" + formCode);
        }


        public async Task PartCode_onitemselected(dynamic Selected)
        {
            partCode = Selected.partcode;
            Console.WriteLine("#Log partCode ::" + partCode);

        }


        public async Task<bool> GridShomaran_DepoInId_717_editmodelsaving(object e)
        {
            var Item = (Entity.Shomaran_DepoInDetail)e;
            if (!Item._IdIsEmpty.Value) 
            {
                 //// آپدیت
                DepoInUpdateInput depoInData = new()
                {
                    //
                    RecNo = _Entity.RecNo,
                    RecDate = _Entity.RecDate,
                    Amount = Convert.ToDecimal(_Entity.Amount),
                    PartCode = _Entity.PartCode, //partCode,
                    InvCode = _Entity.InvCode,  //invCode,

                    Receipt = _Entity.Receipt,
                    TempNo = _Entity.TempNo,
                    //FormCode = _Entity.FormCode,// formCode,//
                    //FormYear = Convert.ToInt32(_Entity.FormYear),//
                    //Qc = Convert.ToInt32(_Entity.QC),//
                    AmountD = Convert.ToDecimal(Item.Amount),// AmountD
                    PartCodeD = Item.PartCode,// AmountD
                    Note = Item.Note, //_Entity.Note,
                    Tg_Receipt = Convert.ToInt32(Item.RowId),
                    Radyabi = Item.Radyabi,//
                    Sefaresh = Item.Sefaresh,//
                                                //Product = _Entity.Product.ToString(),
                    Year = Convert.ToInt32(Item.Year),
                };

                var data = await ApiServer.External.Services.ShomaranPart.UpdateDepoIn(ShomaranApiMode.Polfilm, depoInData);

                if (data.Status == HttpStatusCode.OK)
                {
                    await _MSG.ShowSuccess(await JSON.ToJson(data.Content));
                    return false;
                }
                else
                {
                    await _MSG.ShowError(await JSON.ToJson(data.Content));
                    return true;
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

        public static async Task<Result> CreateDepoIn(ShomaranApiMode ApiMode, DepoInInput depoIn)
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
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/InsertDepoIn", shomaranApi, ApplicationType.None);

            return apiresult;
        }

        public static async Task<Result> UpdateDepoIn(ShomaranApiMode ApiMode, DepoInUpdateInput depoIn)
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
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/UpdateDepoIn", shomaranApi, ApplicationType.None);

            return apiresult;
        }

        public static async Task<Result> DeleteINOut(ShomaranApiMode ApiMode, string recno, int year)
        {
            var DataJson = await JSON.ToJson(new
            {
                recno,
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
            Result apiresult = await Send.PostAsync(_content, $"ShomaranPart/DeleteINOut/{recno}/{year}", shomaranApi, ApplicationType.None);
            return apiresult;
        }

    }
}

public class DepoInInput
{
    //public string? RecNo { get; set; }
    public string? RecDate { get; set; }
    public decimal? Amount { get; set; }
    //public decimal? Amount2 { get; set; }
    public string? PartCode { get; set; }
    public string? InvCode { get; set; }
    public int? Product { get; set; }
    public bool? QC { get; set; }
    public string? Receipt { get; set; }
    public string? TempNo { get; set; }
    public int? Year { get; set; }


    //public string? AccCode { get; set; }
    //public string? AnnArrNo { get; set; }
    //public int AnnArrYear { get; set; }
    //public long BatchNo { get; set; }

    public string? Desc { get; set; }

    // This property holds the nested detail records.
    public List<DepoInDetailInput> Details { get; set; } = new List<DepoInDetailInput>();
}
public class DepoInDetailInput
{
    public decimal? Amount { get; set; }
    public decimal? Amount2 { get; set; }
    public string? RecNo { get; set; }
    public string? PartCode { get; set; }
    public string? Radyabi { get; set; }
    public string? Sefaresh { get; set; }
    //public string? RowId { get; set; } // ROWORDER
    public string? TgReceipt { get; set; }// ROWORDER
    //public string? ArrNo { get; set; }
    //public long BatchNo { get; set; }
    public string? Note { get; set; }
    public int? Year { get; set; }
}

public class DepoInUpdateInput
{
    // Identifying Parameters
    public string? RecNo { get; set; }
    public string? PartCode { get; set; }
    public int Year { get; set; }

    // Header (TFW_DEPO_IN) Fields
    public decimal Amount { get; set; }
    public string? FormCode { get; set; }
    public int FormYear { get; set; }
    public string? RecDate { get; set; }
    public string? Product { get; set; }
    public string? Receipt { get; set; }
    public string? TempNo { get; set; }
    public string? InvCode { get; set; }

    // Detail (TFW_DEPO_INDTL) Fields
    public decimal AmountD { get; set; } // Corresponds to @AMOUNT_D
    public string? PartCodeD { get; set; } // Corresponds to @PartCode_D
    public string? Note { get; set; }
    public string? Radyabi { get; set; }
    public string? Sefaresh { get; set; }

    // Unused parameter in SP, but included for completeness
    public int Tg_Receipt { get; set; }
    //public int Qc { get; set; }
}
