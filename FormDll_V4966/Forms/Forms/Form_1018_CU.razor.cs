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
using Baya.Models.ORM;
using Castle.DynamicLinqQueryBuilder;
using System.Globalization;

namespace Forms.Forms
{
    public class Form_1018_CUBase : Form_1018_CUPeropeties
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
            bool addupdate = await UpdateSefareshPoint();

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
        public async Task<bool> UpdateSefareshPoint()
        {
            Console.WriteLine("#Log Start::");
            Console.WriteLine("#_Entity.PartNo:" + _Entity.PartNo);
            Console.WriteLine("#_Entity.InvCode:" + _Entity.InvCode);
            Console.WriteLine("#_Entity.EhtiatAmount:" + _Entity.EhtiatAmount);
            Console.WriteLine("#_Entity.OrdPoint:" + _Entity.OrdPoint);
            Console.WriteLine("#_Entity.OrderAmount:" + _Entity.OrderAmount);
            Console.WriteLine("#_Entity.MaxAmount:" + _Entity.MaxAmount);
            Console.WriteLine("#_Entity.Year:" + _Entity.Year);
            if (_Entity._IdIsEmpty.Value)
            {

                SefareshPointDto rethavData = new()
                {
                    Creator = _Entity.Creator,
                    Sefaresh = _Entity.Sefaresh,
                    Radyabi = _Entity.Radyabi,
                    TecNO = _Entity.TecNO,
                    Unit = _Entity.Unit,
                    Unit1 = _Entity.Unit1,
                    //
                    PartNo = _Entity.PartNo,
                    InvCode = _Entity.InvCode,
                    EhtiatAmount = Convert.ToDecimal(_Entity.EhtiatAmount),
                    OrdPoint = Convert.ToDecimal(_Entity.OrdPoint),
                    OrderAmount = Convert.ToDecimal(_Entity.OrderAmount),
                    MaxAmount = Convert.ToDecimal(_Entity.MaxAmount),
                    Year = Convert.ToInt32(_Entity.Year)
                };


                Console.WriteLine("#Log factbBuyData ::" + await JSON.ToJson(rethavData));

                var data = await ApiServer.External.Services.ShomaranPart.UpdateSefaresh2(ShomaranApiMode.Polfilm, rethavData);


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
            

            return true;
        }
        #endregion FunctionEvents

    }
}

namespace ApiServer.External.Services
{
    public partial class ShomaranPart
    {

      
        public static async Task<Result> UpdateSefaresh2(ShomaranApiMode ApiMode, SefareshPointDto sefareshPoint)
        {
            // Serialize the entire havaleData object into a JSON string
            var DataJson = await JSON.ToJson(sefareshPoint);

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
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/UpdateSefareshPoint2", shomaranApi, ApplicationType.None);

            return apiresult;
        }

    }
}

public sealed class SefareshPointDto
{
    public string? PartNo { get; set; }        // @partno (varchar(10))
    public string? InvCode { get; set; }       // @invcode (varchar(10))
    public string? Creator { get; set; }       // @invcode (varchar(10))
    public string? Sefaresh { get; set; }       // @invcode (varchar(10))
    public string? Radyabi { get; set; }       // @invcode (varchar(10))
    public string? TecNO { get; set; }       // @invcode (varchar(10))
    public string? Unit { get; set; }       // @invcode (varchar(10))
    public string? Unit1 { get; set; }       // @invcode (varchar(10))
    public decimal EhtiatAmount { get; set; } // @EHTIATAMNT (numeric(15,3))
    public decimal OrdPoint { get; set; }     // @ordpoint (numeric(15,3))
    public decimal OrderAmount { get; set; }  // @ORDERAMNT (numeric(15,3))
    public decimal MaxAmount { get; set; }    // @maxamount (numeric(15,3))
    public int Year { get; set; }             // @YEAR (int)
}
