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


namespace Forms.Forms
{
    public class Form_871_CUBase : Form_871_CUPeropeties
    {

        // تابع پیام توــست
        public MSG _MSG { get; set; }

        /// <summary>
        /// آماده سازی فرم
        /// </summary>
        /// <returns></returns>

        public string grcode = "";
        public string subGrCode = "";

        protected override async Task OnInitializedAsync()
        {
            // 4e1db717-2d5e-f011-a506-005056a2b6bd     سال 1404
            //_Entity.Shomaran_FiscalYearId = Guid.Parse("4e1db717-2d5e-f011-a506-005056a2b6bd");
            //Ref_Shomaran_FiscalYearId.Value = Guid.Parse("4e1db717-2d5e-f011-a506-005056a2b6bd");

            //4d1db717-2d5e-f011-a506-005056a2b6bd  سال 1403
            //_Entity.Shomaran_FiscalYearId = Guid.Parse("4d1db717-2d5e-f011-a506-005056a2b6bd");


            Console.WriteLine(await Utility.JSON.ToJson(_Entity));



            StateHasChanged();
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

        }


        #region FunctionEvents

        // public int year;
        // public byte ProductType;

        public async Task Shomaran_FiscalYearId1_onitemselected(Entity.Shomaran_FiscalYear Selected)
        {
            //year = Convert.ToInt32(Selected.Title);
            _Entity.Shomaran_FiscalYear = Selected;
        }

        public async Task Shomaran_ProductTypeId_onitemselected(Entity.Shomaran_ProductType Selected)
        {

            //ProductType = Convert.ToInt32(Selected.Code);
            //ProductType =  Selected.Code.Value;
            _Entity.Shomaran_ProductType = Selected;

        }

        public async Task GRCODE_onitemselected(dynamic Selected)
        {
            grcode = Selected.TAFNO;
        }
        public async Task SUBGRCODE_onitemselected(dynamic Selected)
        {
            subGrCode = Selected.TAFNO;
        }


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
            Console.WriteLine("#_Entity.Shomaran_ProductType.Code.Value:" + _Entity.Shomaran_ProductType.Code.Value);
            Console.WriteLine("#_Entity.SSTID:" + _Entity.SSTID);
            Console.WriteLine("#_Entity.TECNO:" + _Entity.TECNO);
            Console.WriteLine("#_Entity.TECDESC:" + _Entity.TECDESC);
            Console.WriteLine("#_Entity.Units:" + _Entity.Units);
            Console.WriteLine("#_Entity.Units2:" + _Entity.Units2);
            Console.WriteLine("#_Entity.Shomaran_FiscalYear.Title.Value:" + _Entity.Shomaran_FiscalYear.Title.Value);

            if (_Entity._IdIsEmpty.Value)
            {
                //// ثبت

                var data = await ApiServer.External.Services.ShomaranPart.InsertPart2(
                    ShomaranApiMode.Polfilm,
                    _Entity.PartNo,
                    _Entity.ProductName,
                    grcode.Trim(),
                    subGrCode.Trim(),
                     _Entity.Shomaran_ProductType.Code.Value,
                     _Entity.SSTID,
                   _Entity.TECNO != null ? _Entity.TECNO : "",
                   _Entity.TECDESC != null ? _Entity.TECDESC : "",
                    _Entity.Units != null ? _Entity.Units : "",
                    _Entity.Units2 != null ? _Entity.Units2 : "",
                    _Entity.Shomaran_FiscalYear.Title.Value
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

                var data = await ApiServer.External.Services.ShomaranPart.UpdatePart2(
                    ShomaranApiMode.Polfilm,
                    _Entity.PartNo,
                    _Entity.ProductName,
                    grcode.Trim(),
                    subGrCode.Trim(),
                     _Entity.Shomaran_ProductType.Code.Value,
                     _Entity.SSTID,
                   _Entity.TECNO != null ? _Entity.TECNO : "",
                   _Entity.TECDESC != null ? _Entity.TECDESC : "",
                    _Entity.Units != null ? _Entity.Units : "",
                    _Entity.Units2 != null ? _Entity.Units2 : "",
                    _Entity.Shomaran_FiscalYear.Title.Value
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

        #endregion FunctionEvents

    }
}


namespace ApiServer.External.Services
{
    public partial class ShomaranPart
    {


        public static async Task<Result> InsertPart2(ShomaranApiMode ApiMode, string partNo, string desc, string grCode, string subGrCode,
            int partKind, string sstId, string tecNo, string tecDesc, string unit, string unit1, int year)
        {
            Console.WriteLine("InsertPart2");
            var DataJson = await JSON.ToJson(new
            {
                partNo,
                desc,
                grCode,
                subGrCode,
                partKind,
                sstId,
                tecNo,
                tecDesc,
                unit,
                unit1,
                year
            });
            Console.WriteLine("InsertPart2 1");
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
            Console.WriteLine("InsertPart2 3");
            var _content = new StringContent(DataJson, Encoding.UTF8, "application/json");
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/InsertPart", shomaranApi, ApplicationType.None);
            Console.WriteLine("InsertPart2 4");
            return apiresult;
        }



        public static async Task<Result> UpdatePart2(ShomaranApiMode ApiMode, string partNo, string desc, string grCode, string subGrCode,
            int partKind, string sstId, string tecNo, string tecDesc, string unit, string unit1, int year)
        {
            Console.WriteLine("UpdatePart2");
            var DataJson = await JSON.ToJson(new
            {
                partNo,
                desc,
                grCode,
                subGrCode,
                partKind,
                sstId,
                tecNo,
                tecDesc,
                unit,
                unit1,
                year
            });
            Console.WriteLine("UpdatePart2 1");
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
            Console.WriteLine("UpdatePart2 3");
            var _content = new StringContent(DataJson, Encoding.UTF8, "application/json");
            Result apiresult = await Send.PostAsync(_content, "ShomaranPart/UpdatePart", shomaranApi, ApplicationType.None);
            Console.WriteLine("UpdatePart2 4");
            return apiresult;
        }

        public static async Task<Result> DeletePart(ShomaranApiMode ApiMode, string partNo, int year)
        {
            Console.WriteLine("DeletePart2");
            var DataJson = await JSON.ToJson(new
            {
                partNo,
                year
            });
            Console.WriteLine("DeletePart2 1");
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
            Console.WriteLine("DeletePart2 3");
            var _content = new StringContent(DataJson, Encoding.UTF8, "application/json");
            Result apiresult = await Send.PostAsync(_content, $"ShomaranPart/DeletePart/{partNo}/{year}", shomaranApi, ApplicationType.None);
            Console.WriteLine("DeletePart2 4");
            return apiresult;
        }

    }
}