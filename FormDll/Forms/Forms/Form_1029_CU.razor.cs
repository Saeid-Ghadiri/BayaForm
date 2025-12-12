using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using Baya.Models.ORM;
using Castle.DynamicLinqQueryBuilder;
using System.Globalization;
using Utility;
using System.Text.Json;
using System.Text;

namespace Forms.Forms
{
    public class Form_1029_CUBase : Form_1029_CUPeropeties
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
                var transDetails = _Entity.SH_Petco_ReturnHavaleDetail.Select(p => new RethavDetail()
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

                var data = await ApiServer.External.Services.ShomaranPart.CreateReturnHavale(ShomaranApiMode.Petco, rethavData);


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

                //var data = await ApiServer.External.Services.ShomaranPart.UpdateReturnHavale(ShomaranApiMode.Petco, factbBuyData);

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



        public async Task<bool> GridSH_Petco_ReturnHavaleId_781_editmodelsaving(object e)
        {
            ////// آپدیت
            var Item = (Entity.SH_Petco_ReturnHavaleDetail)e;
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

            var data = await ApiServer.External.Services.ShomaranPart.UpdateReturnHavale(ShomaranApiMode.Petco, factbBuyData);

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
        public async Task GridSH_Petco_ReturnHavaleId_781_afterrendermodal(Entity.SH_Petco_ReturnHavaleDetail Item)
        {
            if (Item.Year == null)
            {
                var pc = new PersianCalendar();
                int persianYear = pc.GetYear(DateTime.Now);
                Item.Year = persianYear;
                Ref_SH_Petco_ReturnHavaleDetail_Year.Value = persianYear;
            }


            if (_Entity.FactNo == null)
            {
                Ref_SH_Petco_ReturnHavaleDetail_RowOrder.Value = _Entity.SH_Petco_ReturnHavaleDetail.Count() + 1;
            }

        }

        public async Task ProductSearch_onitemselected(dynamic Selected, Entity.SH_Petco_ReturnHavaleDetail Item)
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
        #endregion FunctionEvents

    }
}
