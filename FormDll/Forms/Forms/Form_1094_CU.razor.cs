using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using System.Globalization;
using Utility;
using Baya.Models.ORM;
using Castle.DynamicLinqQueryBuilder;
using System.Text;
using System.Text.Json;

namespace Forms.Forms
{
    public class Form_1094_CUBase : Form_1094_CUPeropeties
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

        public async Task<bool> AddDepoOut()
        {
            Console.WriteLine("#Log Start::");
            Console.WriteLine("#_Entity.Havno:" + _Entity.Havno);
            Console.WriteLine("#_Entity.Havdate:" + _Entity.HAVDATE);
            //Console.WriteLine("#_Entity.MainMnt:" + _Entity.MainMnt);
            Console.WriteLine("#_Entity.Product:" + _Entity.Product);
            Console.WriteLine("#_Entity.Receipt:" + _Entity.Receipt);
            Console.WriteLine("#_Entity.TempNo:" + _Entity.TempNo);
            Console.WriteLine("#_Entity.Year:" + _Entity.Year);
            Console.WriteLine("#_Entity.Note:" + _Entity.Note);
            if (_Entity._IdIsEmpty.Value)
            {
                //// ثبت
                var depoOutDetails = _Entity.SH_Petco_DepoOutDetail.Select(p => new DepoOutDetail()
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

                var data = await ApiServer.External.Services.ShomaranPart.CreateDepoOut(ShomaranApiMode.Petco, depoOutData);


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

                //var data = await ApiServer.External.Services.ShomaranPart.UpdateDepoOut(ShomaranApiMode.Petco, depoInData);

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


        public async Task<bool> GridSH_Petco_DepoOutId_769_editmodelsaving(object e)
        {
            var Item = (Entity.SH_Petco_DepoOutDetail)e;
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

                var data = await ApiServer.External.Services.ShomaranPart.UpdateDepoOut(ShomaranApiMode.Petco, depoInData);

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
        public async Task GridSH_Petco_DepoOutId_769_afterrendermodal(Entity.SH_Petco_DepoOutDetail Item)
        {

            if (Item.Year == null)
            {
                var pc = new PersianCalendar();
                int persianYear = pc.GetYear(DateTime.Now);
                Item.Year = persianYear;
                Ref_SH_Petco_DepoOutDetail_Year.Value = persianYear;
            }


            if (_Entity.Havno == null)
            {
                Ref_SH_Petco_DepoOutDetail_RowId.Value = (_Entity.SH_Petco_DepoOutDetail.Count() + 1).ToString();
            }
        }

        public async Task  ProductSearch_onitemselected(dynamic Selected ,Entity.SH_Petco_DepoOutDetail Item  )
        {
            /// <summary>
            /// فیلدهای زیر فیلدهای اصلی برای نمایش در فرم هستند.
            ///</summary>

            //Console.WriteLine("start");
            //  نام کالا
            Item.ProductName = Selected.desc;
            //Console.WriteLine(Selected.DESC);
            //  نام دسته بندی فرعی
            Item.shomarefani = Selected.partno;

            // کد کالا
            Item.PartCode = Selected.partcode;
            //واحد کالا
            Item.Unit = Selected.unit;
            
        }

	


		#endregion FunctionEvents

    }
}
