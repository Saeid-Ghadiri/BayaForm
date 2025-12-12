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
    public class Form_1028_CUBase : Form_1028_CUPeropeties
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
            if (_Entity.Creator == null)
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
            //Console.WriteLine("#_Entity.WUser:" + _Entity.WUser);
            Console.WriteLine("#_Entity.Year:" + _Entity.Year);
            if (_Entity._IdIsEmpty.Value)
            {
                //// ثبت
                var transDetails = _Entity.SH_Petco_FactbBuyDetail.Select(p => new FactbBuyDetail()
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

                _Entity.TempNo = _Entity.FACTOR;
                FactbBuyHeader factbBuyData = new()
                {
                    //
                    Creator = _Entity.Creator,
                    FactDate = _Entity.FactDate,
                    Factor = _Entity.FACTOR ?? "",
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

                var data = await ApiServer.External.Services.ShomaranPart.CreateFactbBuy(ShomaranApiMode.Petco, factbBuyData);


                Console.WriteLine("#Log End ::");
                Console.WriteLine("#Log status ::" + data.Status.ToString());
                string result = await JSON.ToJson(data);

                Console.WriteLine("#Log data ::" + result);
                if (data.Status == HttpStatusCode.OK)
                {
                    await _MSG.ShowSuccess(await JSON.ToJson(data.Content));
                    //_Entity.FACTNO = data.Content.ToString();
                    _Entity.ApiResult = data.Content.ToString();

                    Console.WriteLine("#Log data ::" + data.Content.ToString());

                    using var doc = JsonDocument.Parse(data.Content.ToString());
                    var number = doc.RootElement
                                    .GetProperty("Result")
                                    .GetString()
                                    ?.Trim();   // remove spaces if any
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


        public async Task<bool> GridSH_Petco_FactbBuyId_775_editmodelsaving(object e)
        {
            var Item = (Entity.SH_Petco_FactbBuyDetail)e;
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

                var data = await ApiServer.External.Services.ShomaranPart.UpdateFactbBuy(ShomaranApiMode.Petco, factbBuyData);

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
        public async Task GridSH_Petco_FactbBuyId_775_afterrendermodal(Entity.SH_Petco_FactbBuyDetail Item)
        {
            if (Item.Year == null)
            {
                var pc = new PersianCalendar();
                int persianYear = pc.GetYear(DateTime.Now);
                Item.Year = persianYear;
                Ref_SH_Petco_FactbBuyDetail_Year.Value = persianYear;
            }


            if (_Entity.FACTNO == null)
            {
                Ref_SH_Petco_FactbBuyDetail_RowId.Value = _Entity.SH_Petco_FactbBuyDetail.Count() + 1;
            }

        }

        public async Task ProductSearch_onitemselected(dynamic Selected, Entity.SH_Petco_FactbBuyDetail Item)
        {

            /// <summary>
            /// فیلدهای زیر فیلدهای اصلی برای نمایش در فرم هستند.
            ///</summary>

            //Console.WriteLine("start");
            //  نام کالا
            Item.ProductName = Selected.desc;
            //Console.WriteLine(Selected.DESC);
            //  نام دسته بندی فرعی
            //Item.shomarefani = Selected.partno;

            // کد کالا
            Item.PartCode = Selected.partcode;
            //واحد کالا
            Item.Unit = Selected.unit;

            //شماره فنی کالا
            Item.ShomareFani = Selected.tecno;
        }

        public async Task SelerCode_onitemselected(dynamic Selected)
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

