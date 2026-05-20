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
using System.Text.Json;
using System.Text;

namespace Forms.Forms
{
    public class Form_1075_CUBase : Form_1075_CUPeropeties
    {

        // تابع پیام توــست
        public MSG _MSG { get; set; }

        private string fORMCODE = " ";
        private string oRDERNO = " ";
        private int oRDERYEAR = 0;

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
            if (!await AddHavale())
            {
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
            if (_Entity.H_KIND == null)
            {
                _Entity.H_KIND = true;
                Ref_CENTCODE2.SetVisible(false);
            }

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
                    _Entity.Year = persianYear;

                    StateHasChanged();
                }

            }

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
            //Console.WriteLine("#_Entity.FORMCODE:" + _Entity.FORMCODE);
            //Console.WriteLine("#_Entity.FORMYEAR:" + _Entity.FORMYEAR);
            //Console.WriteLine("#_Entity.ORDERNO:" + _Entity.ORDERNO);
            //Console.WriteLine("#_Entity.ORDERYEAR:" + _Entity.ORDERYEAR);
            Console.WriteLine("#_Entity.CREATOR:" + _Entity.CREATOR);
            Console.WriteLine("#_Entity.FACTDATE:" + _Entity.FACTDATE);
            Console.WriteLine("#_Entity.FACTNO:" + _Entity.FACTNO);
            Console.WriteLine("#_Entity.H_KIND:" + _Entity.H_KIND);
            Console.WriteLine("#_Entity.INVCODE:" + _Entity.INVCODE);
            //Console.WriteLine("#_Entity.MAIN_MNT:" + _Entity.MAIN_MNT);
            Console.WriteLine("#_Entity.TEMPNO:" + _Entity.TEMPNO);
            Console.WriteLine("#_Entity.NOTE:" + _Entity.NOTE);
            //Console.WriteLine("#_Entity.WUSER:" + _Entity.WUSER);
            Console.WriteLine("#_Entity.Year:" + _Entity.Year);
            if (_Entity._IdIsEmpty.Value || _Entity.FACTNO == null)// if (string.IsNullOrEmpty(_Entity.CENTCODE))
            {
                //// ثبت
                var havaleDetails = _Entity.SH_Petco_ProductDeliveryDetail.Select(p => new HavaleDetailDto()
                {
                    Amount = Convert.ToDecimal(p.AMOUNT),
                    //Amount2 = Convert.ToDecimal(p.AMOUNT2),
                    //Code = p.CODE,
                    PartCode = p.PARTCODE,
                    Radyabi = p.RADYABI,
                    RowId = Convert.ToInt32(p.ROW_ID),
                    Sefaresh = p.SEFARESH,
                    Year = Convert.ToInt32(p.Year)
                }).ToList();


                foreach (var item in havaleDetails)
                {
                    Console.WriteLine("#Detail {item.Amount}:" + item.Amount);
                    //Console.WriteLine("#Detail {item.Amount2}:" + item.Amount2);
                    //Console.WriteLine("#Detail {item.Code}:" + item.Code);
                    Console.WriteLine("#Detail {item.PartCode}:" + item.PartCode);
                    Console.WriteLine("#Detail {item.Radyabi}:" + item.Radyabi);
                    Console.WriteLine("#Detail {item.RowId}:" + item.RowId);
                    Console.WriteLine("#Detail {item.Sefaresh}:" + item.Sefaresh);
                    Console.WriteLine("#Detail {item.Year}:" + item.Year);
                }

                //fORMCODE = _Entity.FORMCODE != null ? _Entity.FORMCODE : " ";
                //oRDERNO = _Entity.ORDERNO != null ? _Entity.ORDERNO : " ";
                //oRDERYEAR =  _Entity.ORDERYEAR != null ? Convert.ToInt32(_Entity.ORDERYEAR) : 0;
                HavaleDto havaleData = new()
                {
                    //
                    CentCode = _Entity.CENTCODE,
                    //CentCode2 = _Entity.CENTCODE2,
                    Creator = _Entity.CREATOR,
                    FactDate = _Entity.FACTDATE,
                    //FormCode = fORMCODE,
                    //FormYear = Convert.ToInt32(_Entity.FORMYEAR),
                    HKind = Convert.ToInt32(_Entity.H_KIND) == 1 ? 1 : 2,//Convert.ToInt32(_Entity.H_KIND),
                    InvCode = _Entity.INVCODE,
                    //MainMnt = Convert.ToDecimal(_Entity.MAIN_MNT),
                    Note = _Entity.NOTE,
                    //OrderNo = oRDERNO,
                    //OrderYear = oRDERYEAR,
                    TempNo = _Entity.TEMPNO,
                    //WUser = _Entity.WUSER,
                    Year = Convert.ToInt32(_Entity.Year),

                    HavaleDetails = havaleDetails,
                };


                var data = await ApiServer.External.Services.ShomaranPart.CreateHavale2(ShomaranApiMode.Petco, havaleData);


                Console.WriteLine("#Log End ::");
                Console.WriteLine("#Log status ::" + data.Status.ToString());
                string result = await JSON.ToJson(data);

                Console.WriteLine("#Log data ::" + result);
                if (data.Status == HttpStatusCode.OK)
                {
                    await _MSG.ShowSuccess(await JSON.ToJson(data.Content));
                    //_Entity.FACTNO = data.Content.ToString();
                    _Entity.ApiResult = data.Content.ToString();

                    using var doc = JsonDocument.Parse(data.Content.ToString());
                    var number = doc.RootElement
                                .GetProperty("ReceiptNumber")[0]
                                .GetString()
                                ?.Trim();   // remove spaces
                    _Entity.FACTNO = number;

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



        public async Task<bool> GridSH_Petco_ProductDeliveryId_761_editmodelsaving(object e)
        {
            Console.WriteLine("1");
            var Item = (Entity.SH_Petco_ProductDeliveryDetail)e;
            //// آپدیت
            HavaleUpdateInput havaleDto = new()
            {
                //
                FactNo = _Entity.FACTNO,//_Entity.ORDERNO,
                CentCode = _Entity.CENTCODE,
                FactDate = _Entity.FACTDATE,
                InvCode = _Entity.INVCODE,
                HKind = Convert.ToInt32(_Entity.H_KIND) == 1 ? 1 : 2,//Convert.ToInt32(_Entity.H_KIND),
                //MainMnt = Convert.ToDecimal(_Entity.MAIN_MNT),
                Note = _Entity.NOTE,
                //OrderNo = _Entity.ORDERNO,
                //OrderYear = Convert.ToInt32(_Entity.ORDERYEAR),
                TempNo = _Entity.TEMPNO,
                //FormCode = _Entity.FORMCODE,
                //FormYear = Convert.ToInt32(_Entity.FORMYEAR),

                Amount = Convert.ToDecimal(Item.AMOUNT),
                //BabCode = Item.BabCode,
                //Code = Item.CODE,
                PartCode = Item.PARTCODE,
                RowId = Convert.ToInt32(Item.ROW_ID),
                Year = Convert.ToInt32(_Entity.Year),
            };
            //Console.WriteLine(await JSON.ToJson(havaleDto));
            var data = await ApiServer.External.Services.ShomaranPart.UpdateHavale2(ShomaranApiMode.Petco, havaleDto);

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
            return false;
        }
        public async Task GridSH_Petco_ProductDeliveryId_761_afterrendermodal(Entity.SH_Petco_ProductDeliveryDetail Item)
        {

            if (Item.Year == null)
            {
                var pc = new PersianCalendar();
                int persianYear = pc.GetYear(DateTime.Now);
                Item.Year = persianYear;
                Ref_SH_Petco_ProductDeliveryDetail_Year.Value = persianYear;
            }


            if (_Entity.FACTNO == null)
            {
                Ref_SH_Petco_ProductDeliveryDetail_ROW_ID.Value = _Entity.SH_Petco_ProductDeliveryDetail.Count() + 1;
            }

            if (Convert.ToBoolean(_Entity.H_KIND))
            {
                Ref_CENTCODE.SetVisible(true);
                Ref_CENTCODE2.SetVisible(false);

                Ref_SH_Petco_ProductDeliveryDetail_CENTCODE3.SetVisible(true);
            }
            else
            {
                Ref_CENTCODE.SetVisible(false);
                Ref_CENTCODE2.SetVisible(true);

                Ref_SH_Petco_ProductDeliveryDetail_CENTCODE3.SetVisible(false);
            }
        }

        public async Task H_KIND_oninput(ChangeEventArgs Selected)
        {
            if (Convert.ToBoolean(Selected.Value))
            {
                Ref_CENTCODE.SetVisible(true);
                Ref_CENTCODE2.SetVisible(false);

            }
            else
            {
                Ref_CENTCODE.SetVisible(false);
                Ref_CENTCODE2.SetVisible(true);

            }

        }

        

		#endregion FunctionEvents

    }
}
