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
using System.Globalization;

namespace Forms.Forms
{
    public class Form_1020_CUBase : Form_1020_CUPeropeties
    {



        // تابع پیام توــست
        public MSG _MSG { get; set; }

        public string grcode = "";
        public string subGrCode = "";
        public int productType = 0;
        public int fiscalYear = 0;
        public string units = "";
        public string units2 = "";
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
            // ثبت و ویرایش کالا در شماران سیستم
            bool addupdate = await AddUpdatePart();

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
            if (_Entity.Year == null)
            {
                var pc = new PersianCalendar();
                int persianYear = pc.GetYear(DateTime.Now);
                _Entity.Year = Convert.ToInt32(persianYear);

                StateHasChanged();
            }
        }


        #region FunctionEvents
        /// <summary>
        /// ثبت و ویرایش کالا در شماران با استفاده از استورد پروسیجر
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AddUpdatePart()
        {
            Console.WriteLine("#Log Start::");
            Console.WriteLine("#_Entity.PartNo:" + _Entity.PartNo);
            Console.WriteLine("#_Entity.ProductName:" + _Entity.ProductName);
            Console.WriteLine("#_Entity.GRCODE:" + _Entity.GRCODE);
            Console.WriteLine("#_Entity.SUBGRCODE:" + _Entity.SUBGRCODE);
            Console.WriteLine("#productType:" + productType);
            Console.WriteLine("#_Entity.SSTID:" + _Entity.SSTID);
            Console.WriteLine("#_Entity.TECNO:" + _Entity.TECNO);
            Console.WriteLine("#_Entity.TECDESC:" + _Entity.TECDESC);
            Console.WriteLine("#_Entity.Units:" + _Entity.Units);
            Console.WriteLine("#_Entity.Units2:" + _Entity.Units2);
            //Console.WriteLine("#fiscalYear:" + fiscalYear);
            Console.WriteLine("#_Entity.Year:" + _Entity.Year);

            if (_Entity._IdIsEmpty.Value)
            {
                //// ثبت

                var data = await ApiServer.External.Services.ShomaranPart.InsertPart3(
                    ShomaranApiMode.Petco,
                    _Entity.PartNo,
                    _Entity.ProductName,
                    grcode.Trim(),
                    subGrCode.Trim(),
                     productType,
                     _Entity.SSTID,
                   _Entity.TECNO != null ? _Entity.TECNO : "",
                   _Entity.TECDESC != null ? _Entity.TECDESC : "",
                   units,
                   units2,
                    Convert.ToInt32(_Entity.Year)
                    );


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
                //بروز رسانی

                var data = await ApiServer.External.Services.ShomaranPart.UpdatePart3(
                    ShomaranApiMode.Petco,
                    _Entity.PartNo,
                    _Entity.ProductName,
                    grcode.Trim(),
                    subGrCode.Trim(),
                     productType,
                     _Entity.SSTID,
                   _Entity.TECNO != null ? _Entity.TECNO : "",
                   _Entity.TECDESC != null ? _Entity.TECDESC : "",
                     units,
                   units2,
                    Convert.ToInt32(_Entity.Year)
                    );


                Console.WriteLine("#Log End ::");
                Console.WriteLine("#Log status ::" + data.Status.ToString());
                string result = await JSON.ToJson(data);

                Console.WriteLine("#Log data ::" + result);
                if (data.Status == HttpStatusCode.OK)
                {
                    await _MSG.ShowSuccess(await JSON.ToJson(data.Content));
                    return true;
                }
                else
                {
                    await _MSG.ShowError(await JSON.ToJson(data.Content));
                    return false;
                }
            }
        }

        public async Task GRCODE_onitemselected(dynamic Selected)
        {

            grcode = Selected.TAFNO;
            Console.WriteLine("#Log grcode ::" + grcode);
        }
        public async Task SUBGRCODE_onitemselected(dynamic Selected)
        {

            subGrCode = Selected.TAFNO;
            Console.WriteLine("#Log subGrCode ::" + subGrCode);
        }
        public async Task Units_onitemselected(dynamic Selected)
        {
            units = Selected.name;
            Console.WriteLine("#Log Units ::" + Selected.name);
            Console.WriteLine("#Log Units ::" + Selected.samancode);
        }

        public async Task Units2_onitemselected(dynamic Selected)
        {

            units2 = Selected.name;
        }
        

		public async Task  ProductType_onitemselected(dynamic Selected   )
        {
             productType = Selected.code;
            Console.WriteLine("#Log ProductType ::" + Selected.Title);
            Console.WriteLine("#Log ProductType ::" + Selected.Code);
        }

		#endregion FunctionEvents

    }
}
