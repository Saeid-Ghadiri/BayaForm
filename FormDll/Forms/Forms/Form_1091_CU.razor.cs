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
    public class Form_1091_CUBase : Form_1091_CUPeropeties
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

        public async Task<bool> AddDepoIn()
        {
            Console.WriteLine("#Log Start::");
            Console.WriteLine("#_Entity.RecNo:" + _Entity.RecNo);
            Console.WriteLine("#_Entity.RecDate:" + _Entity.RecDate);
            //Console.WriteLine("#_Entity.Amount:" + _Entity.Amount);
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
                var depoInDetails = _Entity.SH_Petco_DepoInDetail.Select(p => new DepoInDetailInput()
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
                    Creator = _Entity.Creator,
                    //RecNo = _Entity.RecNo,
                    Amount = _Entity.Amount,
                    //Amount2 = _Entity.Amount2,
                    Desc = _Entity.Description,
                    PartCode = _Entity.PartCode,//partCode,
                    Product = Convert.ToInt32(_Entity.Product),
                    InvCode = _Entity.InvCode,//invCode,
                    QC = _Entity.QC,
                    Receipt = _Entity.Receipt,
                    TempNo = _Entity.TempNo,
                    Year = _Entity.Year,

                    Details = depoInDetails,
                };


                Console.WriteLine("#Log depoInData ::" + await JSON.ToJson(depoInData));

                var data = await ApiServer.External.Services.ShomaranPart.CreateDepoIn(ShomaranApiMode.Petco, depoInData);


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
                    _Entity.RecNo = number;

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

        public async Task<bool> GridSH_Petco_DepoInId_767_editmodelsaving(object e)
        {
            var Item = (Entity.SH_Petco_DepoInDetail)e;
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
                    Creator = _Entity.Creator,
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

                var data = await ApiServer.External.Services.ShomaranPart.UpdateDepoIn(ShomaranApiMode.Petco, depoInData);

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
        public async Task GridSH_Petco_DepoInId_767_afterrendermodal(Entity.SH_Petco_DepoInDetail Item)
        {

            if (Item.Year == null)
            {
                var pc = new PersianCalendar();
                int persianYear = pc.GetYear(DateTime.Now);
                Item.Year = persianYear;
                Ref_SH_Petco_DepoInDetail_Year.Value = persianYear;
            }


            if (_Entity.RecNo == null)
            {
                Ref_SH_Petco_DepoInDetail_RowId.Value = (_Entity.SH_Petco_DepoInDetail.Count() + 1).ToString();
            }
        }
        public async Task ProductSearch_onitemselected(dynamic Selected, Entity.SH_Petco_DepoInDetail Item)
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
