using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using System.Collections.ObjectModel;
using Baya.Models.Utility.Entity;
using Baya.Models.Utility.Menu;
using Blazored.Toast.Services;
using Newtonsoft.Json.Linq;
using Utility;
using Baya.Models.ORM;

namespace Forms.Forms
{
    public class Form_857_CUBase : Form_857_CUPeropeties
    {

        // تابع پیام تُــست
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

            // بررسی تابع اعتبارسنجی فیلد ها
            IsValid = await CheckFieldValidation(_Entity);

            return IsValid;
        }


        /// <summary>
        /// تابع قبل اجرا شدن ارسال داده
        /// </summary>
        /// <returns></returns>
        public override async Task<Result> BeforSubmit()
        {
            var IsCancelled = !await ValidateEmployeeDetails(_Entity.HR_EMP_EmployeeInfos);
            if(IsCancelled){
                return new Result() { Status = HttpStatusCode.BadRequest };
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

        }


        #region FunctionEvents


        public async Task<bool> CheckFieldValidation(Entity.HR_EMP_Employees Item)
        {
            bool IsValid = true;
            // **************************************************
            // // فیلد شرکت
            // if (Item.BaseInfo_ORG_CompaniesId == null || Item.BaseInfo_ORG_CompaniesId == Guid.Empty)
            // {
            //     IsValid = false;
            //     await _MSG.ShowError("لطفا گزینه شرکت را تکمیل نمایید.");
            // }

            // فیلد نام
            //if (MasterItem.FirstName == null)
            if (string.IsNullOrWhiteSpace(Item.FirstName))
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه نام را تکمیل نمایید.");
            }

            // فیلد نام خانوادگی
            //if (MasterItem.FirstName == null)
            if (string.IsNullOrWhiteSpace(Item.LastName))
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه نام خانوادگی را تکمیل نمایید.");
            }

            // فیلد وضعیت کارمند
            if (Item.HR_EMP_StatusId == null)
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه وضعیت کارمند را تکمیل نمایید.");
            }

            // فیلد کد قدیم پرسنلی کارمند
            if (Item.EmployeeLastPersonelNo == null)
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه کد قدیم پرسنلی کارمند را تکمیل نمایید.");
            }

            // فیلد کد کارمند
            if (Item.EmployeeNo == null)
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه کد کارمند را تکمیل نمایید.");
            }

            // فیلد کد پرسنلی کارمند
            if (Item.EmployeePersonelNo == null)
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه کد پرسنلی کارمند را تکمیل نمایید.");
            }

            // **************************************************

            // **************************************************
            // Details 
            foreach (var item in _Entity.HR_EMP_EmployeeInfos)
            {
                // فیلد نام پدر
                if (item.FatherName == null)
                {
                    IsValid = false;
                    await _MSG.ShowError("لطفا گزینه نام پدر را تکمیل نمایید.");
                }

                // فیلد کد ملی
                if (item.NationalCode == null)
                {
                    IsValid = false;
                    await _MSG.ShowError("لطفا گزینه کد ملی را تکمیل نمایید.");
                }
            }


            // **************************************************


            return IsValid;
        }

        /// <summary>
        ///  تابعی برای بررسی تعداد ردیف‌های جزئیات
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public async Task<bool> ValidateEmployeeDetails(ICollection<Entity.HR_EMP_EmployeeInfos> Item)
        {
            if (Item != null && Item.Count > 1)
            {
                await _MSG.ShowError("شما مجاز به ثبت یک ردیف بیشتر نیستید!!");
                return false;
            }
            return true;
        }

        // public async Task<bool> GridHR_EMP_EmployeesId_382_editmodelsaving(object e)
        // {
        //     // var IsCancelled = false;

        //     // var HR_EMP_Employees = _Entity.HR_EMP_EmployeeInfos;
        //     // Console.WriteLine("#Log 4 : c " + _Entity.HR_EMP_EmployeeInfos.Count());
        //     // IsCancelled = !await ValidateEmployeeDetails(_Entity.HR_EMP_EmployeeInfos);

        //     // return IsCancelled;
        //     return true;
        // }

        public async Task ExportToCSVEpmloees_onclick(MouseEventArgs Selected)
        {

            await ExportToXLSEpmloee(_Entity);
        }


        public async Task ExportToXLSEpmloees_onclick(MouseEventArgs Selected)
        {

            await ExportToXLSEpmloees(await ListEmploeesGenerator());
        }

        private async Task<List<Entity.HR_EMP_Employees>> ListEmploeesGenerator()
        {
            Table table = new Table()
            {
                Name = "HR_EMP_Employees",
                Column = new List<Coulmn>()
            {
                new Coulmn()
                {
                    Name = "Id"
                },
                new Coulmn()
                {
                    Name = "FirstName"
                },
                new Coulmn()
                {
                    Name = "LastName"
                },
                new Coulmn()
                {
                    Name = "EmployeeNo"
                },
                new Coulmn()
                {
                    Name = "EmployeeLastPersonelNo"
                },
                new Coulmn()
                {
                    Name = "EmployeePersonelNo"
                },
            },
                        Relation = new List<Table>()
            {
                new Table()
                {
                    Name = "HR_EMP_EmployeeInfos",
                    Column = new List<Coulmn>()
                    {
                        new Coulmn()
                        {
                            Name = "Id"
                        },
                        new Coulmn()
                        {
                            Name = "FatherName"
                        },
                        new Coulmn()
                        {
                            Name = "NationalCode"
                        },
                    },
                    //Relation = new List<Models.ORM.Table>()
                    //{
                    //    new Models.ORM.Table()
                    //    {

                    //    }
                    //},
                    //ModeErtebat = ModeErtebat._1N
        }
    },
                ModeErtebat = ModeErtebat._1N
            };
            var DataResult = await ApiServer.External.Services.Data.GetList("HR_EMP_Employees", 1000, 0, table, null);

            //var obj = JObject.Parse(DataResult.Content);
            //List<Entity.HR_EMP_Employees> fieldModel = obj["FModel"].ToObject<List<Entity.HR_EMP_Employees>>();

            Console.WriteLine("" + DataResult.Status);
            Console.WriteLine("" + DataResult.Content.ToString());

            List<Entity.HR_EMP_Employees> result = await JSON.ToObject<List<Entity.HR_EMP_Employees>>(DataResult.Content.ToString());

            Console.WriteLine("" + result.Count);
            return result;
        }
        private async Task ExportToXLSEpmloees(List<Entity.HR_EMP_Employees> Item)
        {


            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet1");

            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "نام";
            worksheet.Cell(1, 3).Value = "نام خانوادگی";
            worksheet.Cell(1, 4).Value = "کد قدیم پرسنلی کارمند";
            worksheet.Cell(1, 5).Value = "کد کارمند";

            worksheet.Cell(1, 6).Value = "نام پدر";
            worksheet.Cell(1, 7).Value = "کد ملی";

            int i = 1;
            foreach (var employee in Item)
            {
                i++;

                worksheet.Cell(i, 1).Value = employee.Id.ToString();
                worksheet.Cell(i, 2).Value = employee.FirstName;
                worksheet.Cell(i, 3).Value = employee.LastName;
                worksheet.Cell(i, 4).Value = employee.EmployeeLastPersonelNo;
                worksheet.Cell(i, 5).Value = employee.EmployeeNo;

                worksheet.Cell(i, 6).Value = employee.HR_EMP_EmployeeInfos.FirstOrDefault()?.FatherName;
                worksheet.Cell(i, 7).Value = employee.HR_EMP_EmployeeInfos.FirstOrDefault()?.NationalCode;
            }



            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var content = stream.ToArray();

            await JS.InvokeVoidAsync(
                "downloadFileFromStream",
                "PersonalALlData.xlsx",
                Convert.ToBase64String(content));
        }


        private async Task ExportToXLSEpmloee(Entity.HR_EMP_Employees Item)
        {


            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet1");

            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "نام شرکت";
            worksheet.Cell(1, 3).Value = "نام";
            worksheet.Cell(1, 4).Value = "نام خانوادگی";
            worksheet.Cell(1, 5).Value = "کد قدیم پرسنلی کارمند";
            worksheet.Cell(1, 6).Value = "کد کارمند";
            worksheet.Cell(1, 7).Value = "کد پرسنلی";
            // detail
            worksheet.Cell(1, 8).Value = "نام پدر";
            worksheet.Cell(1, 9).Value = "کد ملی";


            worksheet.Cell(2, 1).Value = Item.Id.ToString();
            worksheet.Cell(2, 2).Value = Item.BaseInfo_ORG_Companies.Title;
            worksheet.Cell(2, 3).Value = Item.FirstName;
            worksheet.Cell(2, 4).Value = Item.LastName;
            worksheet.Cell(2, 5).Value = Item.EmployeeLastPersonelNo;
            worksheet.Cell(2, 6).Value = Item.EmployeeNo;
            worksheet.Cell(2, 7).Value = Item.EmployeePersonelNo;
            //detail
            worksheet.Cell(2, 8).Value = Item.HR_EMP_EmployeeInfos.FirstOrDefault()?.FatherName;
            worksheet.Cell(2, 9).Value = Item.HR_EMP_EmployeeInfos.FirstOrDefault()?.NationalCode;




            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var content = stream.ToArray();

            await JS.InvokeVoidAsync(
                "downloadFileFromStream",
                "PersonalData.xlsx",
                Convert.ToBase64String(content));
        }

        #endregion FunctionEvents

    }
}