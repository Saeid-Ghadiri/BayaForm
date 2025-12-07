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
using System.Text.Json;
using Baya.Models.ORM;
using Castle.DynamicLinqQueryBuilder;
using System.Globalization;

namespace Forms.Forms
{
    public class Form_958_CUBase : Form_958_CUPeropeties
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
            Console.WriteLine("Log On Init");
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
            if (_Entity._IdIsEmpty.Value || _Entity.FACTNO == null)// if (string.IsNullOrEmpty(_Entity.CENTCODE))
            {
                //// ثبت
                var havaleDetails = _Entity.SH_PolFilm_ProductDeliveryDetail.Select(p => new HavaleDetailDto()
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


                var data = await ApiServer.External.Services.ShomaranPart.CreateHavale2(ShomaranApiMode.Polfilm, havaleData);


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



        public async Task CENTCODE_onitemselected(dynamic Selected)
        {


        }
        public async Task FORMCODE_onitemselected(dynamic Selected)
        {


        }
        public async Task ORDERNO_onitemselected(dynamic Selected)
        {


        }
        public async Task ORDERYEAR_onitemselected(dynamic Selected)
        {


        }
        public async Task INVCODE_onitemselected(dynamic Selected)
        {


        }




        public async Task<bool> GridSH_PolFilm_ProductDeliveryId_702_editmodelsaving(object e)
        {
            Console.WriteLine("1");
            var Item = (Entity.SH_PolFilm_ProductDeliveryDetail)e;
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
            var data = await ApiServer.External.Services.ShomaranPart.UpdateHavale2(ShomaranApiMode.Polfilm, havaleDto);

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




        public async Task submit2_onclick(MouseEventArgs Selected)
        {
            Console.WriteLine("Log 1");

            var TablePost = new Table();
            TablePost.Name = "SH_PolFilm_ShomaranUsers";
            TablePost.Column = new List<Coulmn>
                {
                    new Coulmn {Name="UserId"  , NameAs= "UserId"},
                    new Coulmn {Name ="UserName",NameAs="UserName" }
                };

            Console.WriteLine("Log 2");

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
            Console.WriteLine("Log 3");

            bool IsOk = false;
            var Model = await ApiServer.External.Services.Data.Get(TablePost, NewQuery, "SH_PolFilm_ShomaranUsers", _User.UserID.ToString());

            Console.WriteLine("Log 4");
            if (Model?.Status == HttpStatusCode.OK)
            {
                Console.WriteLine("Log 5");

                Console.WriteLine("Log 6: " + Model.Content.ToString());

                Entity.SH_PolFilm_ShomaranUsers en = await JSON.ToObject<Entity.SH_PolFilm_ShomaranUsers>(Model.Content.ToString());

                Console.WriteLine("Log 7: " + en.UserName);
                _Entity.CREATOR = en.UserName;

                //_Entity.Year = تبدیل تاریخ شمسی و گرفتن سال

            }

        }


        public async Task ProductSearch_onitemselected(dynamic Selected, Entity.SH_PolFilm_ProductDeliveryDetail Item)
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
            Item.PARTCODE = Selected.partcode;
            //واحد کالا
            Item.Unit = Selected.unit;

            //شماره فنی کالا
            Item.shomarefani = Selected.tecno;
        }

        public async Task GridSH_PolFilm_ProductDeliveryId_702_afterrendermodal(Entity.SH_PolFilm_ProductDeliveryDetail Item)
        {
            if (Item.Year == null)
            {
                var pc = new PersianCalendar();
                int persianYear = pc.GetYear(DateTime.Now);
                Item.Year = persianYear;
                Ref_SH_PolFilm_ProductDeliveryDetail_Year.Value = persianYear;
            }

            
             if(_Entity.FACTNO == null)
            {
                Ref_SH_PolFilm_ProductDeliveryDetail_ROW_ID.Value = _Entity.SH_PolFilm_ProductDeliveryDetail.Count() + 1;
            }

            if (Convert.ToBoolean(_Entity.H_KIND))
            {
                Ref_CENTCODE.SetVisible(true);
                Ref_CENTCODE2.SetVisible(false);

                Ref_SH_PolFilm_ProductDeliveryDetail_CENTCODE3.SetVisible(true);
            }
            else
            {
                Ref_CENTCODE.SetVisible(false);
                Ref_CENTCODE2.SetVisible(true);

                Ref_SH_PolFilm_ProductDeliveryDetail_CENTCODE3.SetVisible(false);
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



namespace ApiServer.External.Services
{
    public partial class ShomaranPart
    {

        public static async Task<Result> CreateHavale2(ShomaranApiMode ApiMode, HavaleDto havaleData)
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

        public static async Task<Result> UpdateHavale2(ShomaranApiMode ApiMode, HavaleUpdateInput havaleData)
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


        public static async Task<Result> DeleteHavale2(ShomaranApiMode ApiMode, string factNo, int year)
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
