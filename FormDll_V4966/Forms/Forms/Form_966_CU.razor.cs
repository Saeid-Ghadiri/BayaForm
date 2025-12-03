using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using Newtonsoft.Json.Linq;
using Utility;
using Baya.Models.ORM;
using Baya.Models.Utility.Entity;
using Baya.Models.Utility.Menu;
using Blazored.Toast.Services;
using Baya.Models.Utility.Pagination.Pagings;

namespace Forms.Forms
{
    public class Form_966_CUBase : Form_966_CUPeropeties
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
                await InvokeAsync(StateHasChanged);
                await Task.Yield();
                // Save & New Button: 
                //  await JS.InvokeVoidAsync("AddClass", "#btn_listi_Save_New", "d-none");
                // Before Button: 
                await JS.InvokeVoidAsync("AddClass", "#btn_listi_Previous", "d-none");
                // Next Button: 
                await JS.InvokeVoidAsync("AddClass", "#btn_listi_Next", "d-none");

                StateHasChanged();

                await CheckGridDataAndToggleButton();


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

            IsValid = IsValid && await DataBankAccount(_Entity.HR_Base_BankAccount);

            return IsValid;
        }


        /// <summary>
        /// تابع قبل اجرا شدن ارسال داده
        /// </summary>
        /// <returns></returns>
        public override async Task<Result> BeforSubmit()
        {
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
            if (_Entity.HR_Base_BankAccount != null)
            {
                _Entity.HR_Base_BankAccount = _Entity.HR_Base_BankAccount.Where(x => x.IsDelete == false).ToList();
            }
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
            // // فیلد وضعیت کارمند
            // if (Item.HR_EMP_StatusId == null)
            // {
            //     IsValid = false;
            //     await _MSG.ShowError("لطفا گزینه وضعیت کارمند را تکمیل نمایید.");
            // }
            // // فیلد کد قدیم پرسنلی کارمند
            // if (Item.EmployeeLastPersonelNo == null)
            // {
            //     IsValid = false;
            //     await _MSG.ShowError("لطفا گزینه کد قدیم پرسنلی کارمند را تکمیل نمایید.");
            // }
            // // فیلد کد کارمند
            // if (Item.EmployeeNo == null)
            // {
            //     IsValid = false;
            //     await _MSG.ShowError("لطفا گزینه کد کارمند را تکمیل نمایید.");
            // }
            // // فیلد کد پرسنلی کارمند
            // if (Item.EmployeePersonelNo == null)
            // {
            //     IsValid = false;
            //     await _MSG.ShowError("لطفا گزینه کد پرسنلی کارمند را تکمیل نمایید.");
            // }
            // **************************************************
            // **************************************************
            // Details 
            foreach (var item in _Entity.HR_EMP_EmployeeInfos)
            {
                // // فیلد نام پدر
                // if (item.FatherName == null)
                // {
                //     IsValid = false;
                //     await _MSG.ShowError("لطفا گزینه نام پدر را تکمیل نمایید.");
                // }
                // // فیلد کد ملی
                // if (item.NationalCode == null)
                // {
                //     IsValid = false;
                //     await _MSG.ShowError("لطفا گزینه کد ملی را تکمیل نمایید.");
                // }
            }
            // **************************************************
            return IsValid;
        }


        /// <summary>
        ///  تابعی برای بررسی تعداد ردیف‌های جزئیات
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public async Task<bool> DataBankAccount(ICollection<Entity.HR_Base_BankAccount> Item)
        {
            // دکمه جدید در گرید حساب بانکی - HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonNew
            //await JS.InvokeVoidAsync("ModalAddClass", "#HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonNew", "d-none");


            if (Item != null && Item.Count > 1)
            {
                await _MSG.ShowError("شما مجاز به ثبت یک ردیف بیشتر نیستید!!");
                return false;
            }
            return true;
        }


        /// <summary>
        /// نمایش و حذف دکمه جدید گرید حساب بانکی کاربر
        /// </summary>
        /// <returns></returns>
        private async Task CheckGridDataAndToggleButton()
        {
            StateHasChanged();

            Console.WriteLine("Log HR_Base_BankAccount:::" + _Entity.HR_Base_BankAccount.Count);

            //if (_Entity.HR_Base_BankAccount != null && _Entity.HR_Base_BankAccount.Any(x => !x.IsDelete))
            if (_Entity.HR_Base_BankAccount.Count >= 1)
            {
                // اگر داده وجود دارد، دکمه جدید را مخفی کن
                await JS.InvokeVoidAsync("AddClass", "#HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonNew", "d-none");
            }
            else
            {
                // اگر داده وجود ندارد، دکمه جدید را نمایش بده
                await JS.InvokeVoidAsync("RemoveClass", "#HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonNew", "d-none");
            }

            StateHasChanged();
        }

        // ایونت Edit Model Saving: ویرایش و بررسی اطلاعات حساب بانکی در جدول کارمند در حال مودال
        public async Task<bool> GridHR_EMP_EmployeesId_357_editmodelsaving(object e)
        {
            bool IsCancelled = false;

            //var CheckDataBankAccount = _Entity.HR_Base_BankAccount;
            //Console.WriteLine("#Log:: DataBankAccount ::>>" + _Entity.HR_Base_BankAccount.Count());

            IsCancelled = !await DataBankAccount(_Entity.HR_Base_BankAccount);

            return IsCancelled;
        }

        // ایونت After Render Modal: برای رندر کردن در حالت مودال گرید اطلاعات حساب بانکی در جدول کارمند است
        public async Task GridHR_EMP_EmployeesId_357_afterrendermodal(Entity.HR_Base_BankAccount Item)
        {
            // حذف دکمه های ذخیره، قبلی، بعدی در گرید اطلاعات حساب بانکی
            await JS.InvokeVoidAsync("ModalAddClass", "#HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonSaveAndNew", "d-none");
            await JS.InvokeVoidAsync("ModalAddClass", "#HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonBefore", "d-none");
            await JS.InvokeVoidAsync("ModalAddClass", "#HR_Base_BankAccount_GridHR_EMP_EmployeesId_357ButtonNext", "d-none");
        }



        public async Task<bool> GridHR_EMP_EmployeesId_383_editmodelsaving(object e)
        {
            // بررسی داده
            await CheckGridDataAndToggleButton();

            return false;
        }

        public async Task GridHR_EMP_EmployeesId_383_afterrendermodal(Entity.HR_EMP_EmployeeInfos Item)
        {
            // حذف دکمه های ذخیره، قبلی، بعدی در گرید جزئیات اطلاعات کارمند
            await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonSave", "d-none");
            await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonBefore", "d-none");
            await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonNext", "d-none");
        }

        public async Task submit_onclick(MouseEventArgs Selected)
        {
            // فایل راهنمای فرم اطلاعات بیمه تکمیلی کارمند
            await OpenHelpFileInNewTab0();

        }

        public async Task submit1_onclick(MouseEventArgs Selected)
        {
            await OpenHelpFileInNewTab0();

        }

        /// <summary>
        /// لینک فایل راهنمای فرم اطلاعات بیمه تکمیلی کارمند
        /// </summary>
        /// <returns></returns>
        private async Task OpenHelpFileInNewTab0()
        {
            // نهایی بیمه درمان - عمر و حادثه
            var url = "https://file.workcv.ir/fa-ir/api/v1/File/Get?FileID=4c689678-179d-4312-856f-08ddf993cb5b";
            await JS.InvokeVoidAsync("window.open", url, "_blank");
        }

        private async Task OpenHelpFileInNewTab1()
        {
            // فرم تعیین ذینفع بیمه عمر و حادثه
            var url = "https://file.workcv.ir/fa-ir/api/v1/File/Get?FileID=1297c4b8-023b-4014-8570-08ddf993cb5b";
            await JS.InvokeVoidAsync("window.open", url, "_blank");
        }

        #region Export
        /// <summary>
        /// خروجی اکسل اطلاعات بیمه تکمیلی کارمندان (شامل اطلاعات کارمند، حساب بانکی و خانواده)
        /// </summary>
        public async Task ExportInsuranceDataToExcel()
        {
            if( _User.UserID.ToString() != "c5eb69f8-0152-470c-a7a3-0c6b39098b7c"){
                return;
            }
            var employees = await FetchInsuranceDataForExport();
            await GenerateInsuranceExcelFile(employees);
        }

        private async Task<List<Entity.HR_EMP_Employees>> FetchInsuranceDataForExport()
        {
            var exportTable = new Table
            {
                Name = "HR_EMP_Employees",
                Column = new List<Coulmn>
                {
                    new Coulmn { Name = "Id" },                         // شناسه کارمند
                    new Coulmn { Name = "FirstName" },                  // نام
                    new Coulmn { Name = "LastName" },                   // نام خانوادگی
                    new Coulmn() { Name = "EmployeeNo" },               // کد کارمندی
                    new Coulmn() { Name = "EmployeeLastPersonelNo" },   // کد کارمندی قدیم
                    new Coulmn() { Name = "EmployeePersonelNo" },       // کد پرسنلی
                },
                Relation = new List<Table>
                {
                    // جزئیات کارمند (اطلاعات اصلی بیمه)
                    new Table
                    {
                        Name = "HR_EMP_EmployeeInfos",
                        Column = new List<Coulmn>
                        {
                            new Coulmn { Name = "BirthDate_Fa" },     // تاریخ تولد
                            new Coulmn { Name = "IdCardNo" },         // شماره شناسنامه
                            new Coulmn { Name = "NationalCode" },     // کد ملی
                            new Coulmn { Name = "FatherName" },       // نام پدر
                            new Coulmn { Name = "Mobile" },           // شماره تماس
                            new Coulmn { Name = "BaseInfo_GenderId" } // جنسیت
                        },
                        Relation = new List<Table>
                        {
                            // جنسیت
                            new Table
                            {
                                Name = "BaseInfo_Gender",
                                Column = new List<Coulmn> { new Coulmn { Name = "Title" } },
                                ModeErtebat = ModeErtebat._1N
                            }
                        },
                        ModeErtebat = ModeErtebat._N1
                    },
                    // حساب بانکی
                    new Table
                    {
                        Name = "HR_Base_BankAccount",
                        Column = new List<Coulmn>
                        {
                            new Coulmn { Name = "IBAN" } // شماره شبا
                        },
                        Relation = new List<Table>
                        {
                            // بانک
                            new Table
                            {
                                Name = "BaseInfo_Bank",
                                Column = new List<Coulmn> { new Coulmn { Name = "Title" } }, // عنوان بانک
                                ModeErtebat = ModeErtebat._1N
                            }
                        },
                        ModeErtebat = ModeErtebat._N1
                    },
                    // اطلاعات خانواده (برای فیلدهای نسبت، وضعیت تکفل، کد ملی بیمه شده اصلی)
                    new Table
                    {
                        Name = "HR_EMP_EmployeeFamileis",
                        Column = new List<Coulmn>
                        {
                            new Coulmn {Name= "FirstName"},                 // نام
                            new Coulmn {Name= "LastName"},                  // نام خانوادگی
                            new Coulmn {Name= "FatherName"},                // نام پدر
                            new Coulmn {Name= "NationalCode"},              // کد ملی بیمه شده (عضو خانواده)
                            new Coulmn {Name= "IdCardNo"},                  // شماره شناسنامه
                            new Coulmn {Name= "IsFamily"},                  // پوشش خانوار
                            new Coulmn {Name= "BirthDate"},                 // تاریح تولد
                            new Coulmn {Name= "BaseInfo_GenderId"},         // جنسیت
                            new Coulmn {Name= "BaseInfo_MaritalStatusId"},  // وضعیت تاهل
                            new Coulmn {Name= "HR_FamilyRelationshipId"},   // نسبت
                            new Coulmn {Name= "BaseInfo_CitiesId"},         // شهر محل تولد
                            new Coulmn {Name= "HR_Base_DependentId"}        // وضعیت تکفل
                        },
                        Relation = new List<Table>
                        {
                            // جنسیت
                            new Table
                            {
                                Name = "BaseInfo_Gender",
                                Column = new List<Coulmn> { new Coulmn { Name = "Title" } },
                                ModeErtebat = ModeErtebat._1N
                            },
                            // وضعیت تاهل
                            new Table
                            {
                                Name = "BaseInfo_MaritalStatus",
                                Column = new List<Coulmn> { new Coulmn { Name = "Title" } },
                                ModeErtebat = ModeErtebat._1N
                            },
                            // نسبت
                            new Table
                            {
                                Name = "HR_FamilyRelationship",
                                Column = new List<Coulmn> { new Coulmn { Name = "Title" } },
                                ModeErtebat = ModeErtebat._1N
                            },
                            // شهر محل تولد
                            new Table
                            {
                                Name = "BaseInfo_Cities",
                                Column = new List<Coulmn> { new Coulmn { Name = "Title" } },
                                ModeErtebat = ModeErtebat._1N
                            },
                            // وضعیت تکفل
                            new Table
                            {
                                Name = "HR_Base_Dependent",
                                Column = new List<Coulmn> { new Coulmn { Name = "Title" } },
                                ModeErtebat = ModeErtebat._1N
                            }
                        },
                        ModeErtebat = ModeErtebat._N1
                    }
                },
            };
            Baya.Models.ORM.PagedResult pager = new()
            {
                PageSize = 1000,
                PageNumber = 1,
            };
            //var dataResult = await ApiServer.External.Services.Data.GetList("HR_EMP_Employees", 1000, 0, exportTable, null);
            var dataResult = await ApiServer.External.Services.Data.GetListPost(exportTable, null, pager, "HR_EMP_Employees");
            Console.WriteLine("#1 " + dataResult.Content.ToString());
            Baya.Models.ORM.PagedResult result = await JSON.ToObject<Baya.Models.ORM.PagedResult>(dataResult.Content.ToString());

            if (dataResult?.Status == HttpStatusCode.OK)
            {
                //Console.WriteLine("#2 " + dataResult.Content.ToString());
                string sData = await JSON.ToJson(result.Items);
                var data = await JSON.ToObject<List<Entity.HR_EMP_Employees>>(sData);
                Console.WriteLine("#data " + sData);
                return data;
            }

            return new List<Entity.HR_EMP_Employees>();
        }

        private async Task GenerateInsuranceExcelFile(List<Entity.HR_EMP_Employees> employees)
        {
            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var worksheet = workbook.Worksheets.Add("اطلاعات بیمه کارمندان");

            // هدرها
            var headers = new[] {
                "نام", "نام خانوادگی", "تاریخ تولد", "شماره شناسنامه", "کد ملی", "نام پدر",
                "ملیت", "شماره تماس", "نسبت", "جنسیت", "وضعیت تکفل", "بیمه شده اصلی",
                "کد ملی بیمه شده اصلی", "شماره شبا", "بانک"
            };

            for (int i = 0; i < headers.Length; i++)
                worksheet.Cell(1, i + 1).Value = headers[i];

            int rowIndex = 2;

            foreach (var emp in employees)
            {
                var info = emp.HR_EMP_EmployeeInfos?.FirstOrDefault();
                var bankAccount = emp.HR_Base_BankAccount?.FirstOrDefault();

                // اگر عضو خانواده‌ای وجود دارد، برای هر کدام یک سطر
                var familyMembers = 
                    emp.HR_EMP_EmployeeFamileis?.Where(f => f.IsDelete != true).ToList() ?? new List<Entity.HR_EMP_EmployeeFamileis>();

                if (familyMembers.Any())
                {
                    foreach (var familyMember in familyMembers)
                    {
                        worksheet.Cell(rowIndex, 1).Value = emp.FirstName ?? "-";
                        worksheet.Cell(rowIndex, 2).Value = emp.LastName ?? "-";
                        worksheet.Cell(rowIndex, 3).Value = info?.BirthDate_Fa ?? "-";
                        worksheet.Cell(rowIndex, 4).Value = info?.IdCardNo ?? "-";
                        worksheet.Cell(rowIndex, 5).Value = info?.NationalCode ?? "-";
                        worksheet.Cell(rowIndex, 6).Value = info?.FatherName ?? "-";
                        worksheet.Cell(rowIndex, 7).Value = "ایران";
                        worksheet.Cell(rowIndex, 8).Value = info?.Mobile ?? "-";
                        worksheet.Cell(rowIndex, 9).Value = familyMember.HR_FamilyRelationship?.Title ?? "-";
                        worksheet.Cell(rowIndex, 10).Value = familyMember.BaseInfo_Gender?.Title ?? "-";
                        worksheet.Cell(rowIndex, 11).Value = familyMember.HR_Base_Dependent?.Title ?? "-";
                        worksheet.Cell(rowIndex, 12).Value = familyMember.FirstName + " " + familyMember.LastName;
                        worksheet.Cell(rowIndex, 13).Value = familyMember.NationalCode ?? "-";
                        worksheet.Cell(rowIndex, 14).Value = bankAccount?.IBAN ?? "-";
                        worksheet.Cell(rowIndex, 15).Value = bankAccount?.BaseInfo_Bank?.Title ?? "-";

                        rowIndex++;
                    }
                }
                else
                {
                    // اگر عضو خانواده‌ای نیست، فقط خود کارمند را بیمه‌شده اصلی در نظر بگیر
                    worksheet.Cell(rowIndex, 1).Value = emp.FirstName ?? "-";
                    worksheet.Cell(rowIndex, 2).Value = emp.LastName ?? "-";
                    worksheet.Cell(rowIndex, 3).Value = info?.BirthDate_Fa ?? "-";
                    worksheet.Cell(rowIndex, 4).Value = info?.IdCardNo ?? "-";
                    worksheet.Cell(rowIndex, 5).Value = info?.NationalCode ?? "-";
                    worksheet.Cell(rowIndex, 6).Value = info?.FatherName ?? "-";
                    worksheet.Cell(rowIndex, 7).Value = "ایران";
                    worksheet.Cell(rowIndex, 8).Value = info?.Mobile ?? "-";
                    worksheet.Cell(rowIndex, 9).Value = "-"; // نسبت
                    worksheet.Cell(rowIndex, 10).Value = info?.BaseInfo_Gender?.Title ?? "-";
                    worksheet.Cell(rowIndex, 11).Value = "-"; // وضعیت تکفل
                    worksheet.Cell(rowIndex, 12).Value = "خود کارمند";
                    worksheet.Cell(rowIndex, 13).Value = info?.NationalCode ?? "-";
                    worksheet.Cell(rowIndex, 14).Value = bankAccount?.IBAN ?? "-";
                    worksheet.Cell(rowIndex, 15).Value = bankAccount?.BaseInfo_Bank?.Title ?? "-";

                    rowIndex++;
                }
            }

            // تنظیمات ظاهری
            worksheet.Columns().AdjustToContents();
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Row(1).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;

            // دانلود
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            var content = stream.ToArray();

            await JS.InvokeVoidAsync(
                "downloadFileFromStream",
                "EmployeeInsuranceData.xlsx",
                Convert.ToBase64String(content));
        }
        #endregion /Export

        public async Task ExportFile_onclick(MouseEventArgs Selected)
        {
            //
            await ExportInsuranceDataToExcel();
        }

        #endregion FunctionEvents

    }
}