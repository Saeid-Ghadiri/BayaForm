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
    public class Form_1095_CUBase : Form_1095_CUPeropeties
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
            _Entity.ContractType = "ریالی";
            if (_Entity.CREATOR == null)
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
                var annarDetails = _Entity.SH_Petco_AnnarrDetail.Select(p => new AnnarrDetail()
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

                var data = await ApiServer.External.Services.ShomaranPart.CreateAnnar(ShomaranApiMode.Petco, annarData);


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

                //var data = await ApiServer.External.Services.ShomaranPart.UpdateAnnar(ShomaranApiMode.Petco, depoInData);

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


        public async Task<bool> GridSH_Petco_AnnarrId_771_editmodelsaving(object e)
        {
            var Item = (Entity.SH_Petco_AnnarrDetail)e;
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

            var data = await ApiServer.External.Services.ShomaranPart.UpdateAnnar(ShomaranApiMode.Petco, depoInData);

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
        public async Task GridSH_Petco_AnnarrId_771_afterrendermodal(Entity.SH_Petco_AnnarrDetail Item)
        {

            if (Item.YEAR == null)
            {
                var pc = new PersianCalendar();
                int persianYear = pc.GetYear(DateTime.Now);
                Item.YEAR = persianYear;
                Ref_SH_Petco_AnnarrDetail_YEAR.Value = persianYear;
            }


            if (_Entity.AnnarrNo == null)
            {
                Ref_SH_Petco_AnnarrDetail_RowId.Value = (_Entity.SH_Petco_AnnarrDetail.Count() + 1).ToString();
            }

        }

        public async Task ProductSearch_onitemselected(dynamic Selected, Entity.SH_Petco_AnnarrDetail Item)
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
            Item.PARTCODE = Selected.partcode;
            //واحد کالا
            Item.Unit = Selected.unit;

            //شماره فنی کالا
            Item.ShomareFani = Selected.tecno;

        }

        public async Task ORDERNO_onitemselected(dynamic Selected)
        {
            _Entity.Person = Selected.REQPERSON;
            _Entity.ConfirmDate = Selected.OKFACTDATE;

        }
      



		#endregion FunctionEvents

    }
}
