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
using DateUtils;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;

namespace Forms.Forms
{
    public class Form_944_CUBase : Form_944_CUPeropeties
    {
        // تابع پیام تُــست
        public MSG _MSG { get; set; }

        /// <summary>
        /// آماده سازی فرم
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
        }

        /// <summary>
        /// رندر شدن فرم
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _MSG = new MSG(toastService);

                // دکمه جدید در گرید جزئیات اطلاعات کارمند

                StateHasChanged();

                // حذف دکمه های گرید جزئیات اطلاعات کارمند
                await EmployeeInfos_Grid_Buttons();
            }
        }

        /// <summary>
        /// اعتبار سنجی فرم
        /// </summary>
        public override async Task<bool> FormValidator()
        {
            bool IsValid = true;
            IsValid = await CheckFieldValidation(_Entity);
            return IsValid;
        }

        /// <summary>
        /// تابع قبل اجرا شدن ارسال داده
        /// </summary>
        public override async Task<Result> BeforSubmit()
        {
            // سینک کردن داده های بخش اصلی در بخش جزئیات اطلاعات
            SyncDataMasterToDetails();

            // محاسبه سن کارمند
            PrepareEmployeeAge();

            // تبدیل تاریخ‌های تولد در تمام ردیف‌های گرید
            PrepareBirthDatesForSubmit();

            // سابقه کار
            PrepareWorkExperienceText();

            // بررسی تاریخ های استخدام و استخدام در گروه
            PrepareEmploymentDatesForSubmit();


            var IsCancelled = !await ValidateEmployeeDetails(_Entity.HR_EMP_EmployeeInfos);
            if (IsCancelled)
            {
                return new Result() { Status = HttpStatusCode.BadRequest };
            }

            return new Result() { Status = HttpStatusCode.OK };
        }

        /// <summary>
        /// تابع بعد اجرا شدن ارسال داده
        /// </summary>
        public override async Task AfterSubmit()
        {
        }

        /// <summary>
        /// تابع قبل دریافت داده
        /// </summary>
        public override async Task BeforGetData()
        {
        }

        /// <summary>
        /// تابع بعد دریافت داده
        /// </summary>
        public override async Task AfterGetData()
        {
            // فقط یک ردیف مجاز است
            if (_Entity.HR_EMP_EmployeeInfos?.Count > 1)
            {
                _Entity.HR_EMP_EmployeeInfos = new List<Entity.HR_EMP_EmployeeInfos> { _Entity.HR_EMP_EmployeeInfos.First() };
            }

            // حذف دکمه های گرید جزئیات اطلاعات کارمند
            await EmployeeInfos_Grid_Buttons();

            // تبدیل تاریخ‌های تولد در تمام ردیف‌های گرید
            PrepareBirthDatesForSubmit();

            // سابقه کار
            PrepareWorkExperienceText();

            // بررسی تاریخ های استخدام و استخدام در گروه
            PrepareEmploymentDatesForSubmit();

            // سینک کردن داده های بخش اصلی در بخش جزئیات اطلاعات
            SyncDataMasterToDetails();

            // محاسبه سن کارمند
            PrepareEmployeeAge();
        }

        #region FunctionEvents
        /// <summary>
        /// بررسی گرید جزئیات اطلاعات کارمند در حالت ویرایش مودال
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public async Task<bool> GridHR_EMP_EmployeesId_382_editmodelsaving(object e)
        {
            var IsCancelled = false;

            // var HR_EMP_Employees = _Entity.HR_EMP_EmployeeInfos;
            // Console.WriteLine("#Log 4 : c " + _Entity.HR_EMP_EmployeeInfos.Count());
            // IsCancelled = !await ValidateEmployeeDetails(_Entity.HR_EMP_EmployeeInfos);


            StateHasChanged();

            // حذف دکمه های گرید جزئیات اطلاعات کارمند
            await EmployeeInfos_Grid_Buttons(!IsCancelled);

            StateHasChanged();

            #region سابقه کار کارمند
            // تبدیل و بروزرسانی EmployeeWorkExperienceText
            if (e is Entity.HR_EMP_EmployeeInfos editedItem)
            {
                if (editedItem.EmployeeWorkExperience.HasValue && editedItem.EmployeeWorkExperience > 0)
                {
                    editedItem.EmployeeWorkExperienceText =
                        PersianDateUtils.ConvertDaysToPersianReadable(editedItem.EmployeeWorkExperience.Value);
                }
                else
                {
                    editedItem.EmployeeWorkExperienceText = string.Empty;
                }
            }
            #endregion سابقه کار کارمند

            return IsCancelled;
        }

        /// <summary>
        /// تغییرات در زمان لود مودال اعمال شده نمایش می دهد
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public async Task GridHR_EMP_EmployeesId_382_afterrendermodal(Entity.HR_EMP_EmployeeInfos Item)
        {
            // سینک کردن داده های بخش اصلی در بخش جزئیات اطلاعات
            SyncDataMasterToDetails();

            // محاسبه سن کارمند
            PrepareEmployeeAge();

            // گزینه فعال / غیر فعال
            // Item.IsActive = true; 
            if (!Item.IsActive.HasValue)
            {
                //Console.WriteLine("Log-IsActive :::");
                Item.IsActive = true;
                Ref_HR_EMP_EmployeeInfos_IsActive.AddAttribute("checked", "checked");
            }

            // حذف دکمه های گرید جزئیات اطلاعات کارمند
            await EmployeeInfos_Grid_Buttons(isInModal: true);
        }

        #region اعتبارسنجی موجودیت های فرم
        /// <summary>
        /// اعتبارسنجی فرم
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public async Task<bool> CheckFieldValidation(Entity.HR_EMP_Employees Item)
        {
            bool IsValid = true;

            if (string.IsNullOrWhiteSpace(Item.FirstName))
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه نام را تکمیل نمایید.");
            }

            if (string.IsNullOrWhiteSpace(Item.LastName))
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه نام خانوادگی را تکمیل نمایید.");
            }

            if (Item.HR_EMP_StatusId == null)
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه وضعیت کارمند را تکمیل نمایید.");
            }

            if (Item.EmployeeLastPersonelNo == null)
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه کد قدیم پرسنلی کارمند را تکمیل نمایید.");
            }

            if (Item.EmployeeNo == null)
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه کد کارمند را تکمیل نمایید.");
            }

            if (Item.EmployeePersonelNo == null)
            {
                IsValid = false;
                await _MSG.ShowError("لطفا گزینه کد پرسنلی کارمند را تکمیل نمایید.");
            }

            foreach (var detail in _Entity.HR_EMP_EmployeeInfos)
            {
                if (string.IsNullOrWhiteSpace(detail.FatherName))
                {
                    IsValid = false;
                    await _MSG.ShowError("لطفا گزینه نام پدر را تکمیل نمایید.");
                }

                if (string.IsNullOrWhiteSpace(detail.NationalCode))
                {
                    IsValid = false;
                    await _MSG.ShowError("لطفا گزینه کد ملی را تکمیل نمایید.");
                }

                // اعتبارسنجی تاریخ تولد شمسی
                if (!string.IsNullOrWhiteSpace(detail.BirthDate_Fa) && !PersianDateUtils.TryParseDateString(detail.BirthDate_Fa, out _))
                {
                    IsValid = false;
                    await _MSG.ShowError("تاریخ تولد وارد شده معتبر نیست.");
                }
            }

            return IsValid;
        }
        #endregion /اعتبارسنجی موجودیت های فرم

        #region بررسی تاریخ ها
        /// <summary>
        /// آماده‌سازی تاریخ‌های تولد برای ارسال — تبدیل شمسی → میلادی + استخراج سال/ماه/روز
        /// </summary>
        private void PrepareBirthDatesForSubmit()
        {
            foreach (var item in _Entity.HR_EMP_EmployeeInfos)
            {
                if (!string.IsNullOrWhiteSpace(item.BirthDate_Fa))
                {
                    if (PersianDateUtils.TryParseDateString(item.BirthDate_Fa, out var parts))
                    {
                        // تبدیل به میلادی
                        item.BirthDate = PersianDateUtils.ToGregorian(item.BirthDate_Fa);

                        // استخراج سال/ماه/روز شمسی
                        item.BirthDateYYYY = (short)parts.year;
                        item.BirthDateMM = (byte)parts.month;
                        item.BirthDateDD = (byte)parts.day;
                    }
                    else
                    {
                        // اگر تاریخ نامعتبر بود — می‌توانید خطا نشان دهید یا null بگذارید
                        item.BirthDate = null;
                        item.BirthDateYYYY = null;
                        item.BirthDateMM = null;
                        item.BirthDateDD = null;
                    }
                }
                else
                {
                    // اگر تاریخ خالی بود — فیلدها پاک شوند
                    item.BirthDate = null;
                    item.BirthDateYYYY = null;
                    item.BirthDateMM = null;
                    item.BirthDateDD = null;
                }
            }
        }
        #endregion CheckedDateTime

        #region بررسی تعداد ردیف‌های
        /// <summary>
        /// این متد برای بررسی تعداد ردیف‌های جزئیات کارمند
        /// </summary>
        public async Task<bool> ValidateEmployeeDetails(ICollection<Entity.HR_EMP_EmployeeInfos> Item)
        {
            if (Item != null && Item.Count > 1)
            {
                await _MSG.ShowError("شما مجاز به ثبت یک ردیف بیشتر نیستید!!");
                return false;
            }
            return true;
        }

        #endregion بررسی تعداد ردیف‌های

        #region حذف دکمه های گرید جزئیات اطلاعات کارمند
        /// <summary>
        /// دکمه جدید در گرید جزئیات اطلاعات کارمند
        /// </summary>
        /// <returns></returns>
        private async Task EmployeeInfos_Grid_Buttons(bool hasSavedRecord = false, bool isInModal = false)
        {
            //if (!hasSavedRecord)
            //   hasSavedRecord = _Entity.HR_EMP_EmployeeInfos.Any(x => x.IsDelete == false);

            await Task.Yield();
            if (!hasSavedRecord)
            {
                hasSavedRecord = _Entity.HR_EMP_EmployeeInfos.Any();
            }
            if (hasSavedRecord)
            {
                await JS.InvokeVoidAsync("AddClass", "#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonNew", "d-none");
            }
            else
            {
                await JS.InvokeVoidAsync("RemoveClass", "#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonNew", "d-none");
            }
            //Console.WriteLine("log-0 ::" + hasSavedRecord);

            // فقط اگر در مودال باشیم، دکمه‌های مودال را مخفی کن
            if (isInModal)
            {
                // دکمه ذخیره و جدید
                await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonSaveAndNew", "d-none");
                // دکمه قبلی
                await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonBefore", "d-none");
                // دکمه بعدی
                await JS.InvokeVoidAsync("ModalAddClass", "#HR_EMP_EmployeeInfos_GridHR_EMP_EmployeesId_382ButtonNext", "d-none");
            }
        }
        #endregion حذف دکمه های گرید جزئیات اطلاعات کارمند

        #region پر کردن فیلد سابقه کار

        /// <summary>
        /// پر کردن فیلد EmployeeWorkExperienceText بر اساس EmployeeWorkExperience
        /// EmployeeWorkExperience => سابقه کار کارمند به روز
        /// </summary>
        private void PrepareWorkExperienceText()
        {
            foreach (var info in _Entity.HR_EMP_EmployeeInfos)
            {
                if (info.EmployeeWorkExperience.HasValue && info.EmployeeWorkExperience > 0)
                {
                    info.EmployeeWorkExperienceText = PersianDateUtils.ConvertDaysToPersianReadable(
                        info.EmployeeWorkExperience.Value
                    );
                }
                else
                {
                    info.EmployeeWorkExperienceText = string.Empty;
                }
            }
        }
        #endregion پر کردن فیلد سابقه کار

        #region تاریخ استخدام
        /// <summary>
        /// آماده‌سازی تاریخ‌های استخدام برای ارسال: تبدیل شمسی → میلادی + محاسبه روزهای گذشته
        /// </summary>
        private void PrepareEmploymentDatesForSubmit()
        {
            foreach (var info in _Entity.HR_EMP_EmployeeInfos)
            {
                // تاریخ استخدام
                if (!string.IsNullOrWhiteSpace(info.EmploymentDate_Fa))
                {
                    if (PersianDateUtils.TryParseDateString(info.EmploymentDate_Fa, out _))
                    {
                        info.EmploymentDate = PersianDateUtils.ToGregorian(info.EmploymentDate_Fa);
                        info.DailyEmploymentDate = PersianDateUtils.DaysPassed(info.EmploymentDate_Fa, inclusive: true);
                    }
                    else
                    {
                        info.EmploymentDate = null;
                        info.DailyEmploymentDate = null;
                    }
                }
                else
                {
                    info.EmploymentDate = null;
                    info.DailyEmploymentDate = null;
                }

                // تاریخ استخدام در گروه
                if (!string.IsNullOrWhiteSpace(info.EmploymentDateInGroup_Fa))
                {
                    if (PersianDateUtils.TryParseDateString(info.EmploymentDateInGroup_Fa, out _))
                    {
                        info.EmploymentDateInGroup = PersianDateUtils.ToGregorian(info.EmploymentDateInGroup_Fa);
                        info.DailyEmploymentDateInGroup = PersianDateUtils.DaysPassed(info.EmploymentDateInGroup_Fa, inclusive: true);
                    }
                    else
                    {
                        info.EmploymentDateInGroup = null;
                        info.DailyEmploymentDateInGroup = null;
                    }
                }
                else
                {
                    info.EmploymentDateInGroup = null;
                    info.DailyEmploymentDateInGroup = null;
                }
            }
        }
        #endregion تاریخ استخدام

        #region سینک کردن داده های بخش اصلی در بخش جزئیات اطلاعات
        private void SyncDataMasterToDetails()
        {
            if (_Entity.HR_EMP_EmployeeInfos?.Any() == true)
            {
                var detail = _Entity.HR_EMP_EmployeeInfos.First();
                detail.FirstName = _Entity.FirstName;
                detail.LastName = _Entity.LastName;
            }
        }
        #endregion سینک کردن داده های بخش اصلی در بخش جزئیات اطلاعات

        #region محاسبه سن کارمند
        /// <summary>
        /// محاسبه و پر کردن فیلدهای EmployeeAge و EmployeeAgeText بر اساس BirthDate_Fa یا BirthDate
        /// </summary>
        private void PrepareEmployeeAge()
        {
            foreach (var info in _Entity.HR_EMP_EmployeeInfos)
            {
                if (string.IsNullOrWhiteSpace(info.BirthDate_Fa))
                {
                    // اگر BirthDate_Fa خالی بود، اما BirthDate میلادی وجود داشت → تبدیل به شمسی
                    if (info.BirthDate.HasValue)
                    {
                        info.BirthDate_Fa = PersianDateUtils.ToPersian(info.BirthDate.Value);
                    }
                    else
                    {
                        // هیچ تاریخی وجود ندارد → پاک کن
                        info.EmployeeAge = null;
                        info.EmployeeAgeText = string.Empty;
                        continue;
                    }
                }

                // اعتبارسنجی تاریخ شمسی
                if (!PersianDateUtils.TryParseDateString(info.BirthDate_Fa, out _))
                {
                    info.EmployeeAge = null;
                    info.EmployeeAgeText = "تاریخ تولد نامعتبر است";
                    continue;
                }

                try
                {
                    // محاسبه اختلاف تاریخ تولد تا امروز
                    var diff = PersianDateUtils.CalculateAge(info.BirthDate_Fa);

                    // EmployeeAge → تعداد کل روزها (برای ذخیره در دیتابیس)
                    info.EmployeeAge = diff.TotalDays;

                    // ساخت رشته بدون پرانتز اضافی
                    var parts = new List<string>();
                    if (diff.Years > 0) parts.Add($"{diff.Years} سال");
                    if (diff.Months > 0) parts.Add($"{diff.Months} ماه");
                    if (diff.Days > 0 || parts.Count == 0) parts.Add($"{diff.Days} روز");

                    info.EmployeeAgeText = string.Join(" و ", parts);

                    // EmployeeAgeText → نمایش خوانا و کامل
                    //info.EmployeeAgeText = diff.ToReadableString(showZeros: false);
                }
                catch (Exception ex)
                {
                    // Log optional: Console.WriteLine($"خطا در محاسبه سن برای {info.BirthDate_Fa}: {ex.Message}");
                    info.EmployeeAge = null;
                    info.EmployeeAgeText = "خطا در محاسبه سن";
                }
            }
        }
        #endregion محاسبه سن کارمند

        #region
        #endregion

        #region
        #endregion

        #region Export CSV
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
                    new Coulmn() { Name = "Id" },
                    new Coulmn() { Name = "FirstName" },
                    new Coulmn() { Name = "LastName" },
                    new Coulmn() { Name = "EmployeeNo" },
                    new Coulmn() { Name = "EmployeeLastPersonelNo" },
                    new Coulmn() { Name = "EmployeePersonelNo" },
                },
                Relation = new List<Table>()
                {
                    new Table()
                    {
                        Name = "HR_EMP_EmployeeInfos",
                        Column = new List<Coulmn>()
                        {
                            new Coulmn() { Name = "Id" },
                            new Coulmn() { Name = "FatherName" },
                            new Coulmn() { Name = "NationalCode" },
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
            //var DataResult = await ApiServer.External.Services.Data.GetList("HR_EMP_Employees", 1000, 0, table, null);
            var DataResult = await ApiServer.External.Services.Data.GetListPost(table, null, pager, "HR_EMP_Employees");
            Console.WriteLine("#1");
            Baya.Models.ORM.PagedResult result = await JSON.ToObject<Baya.Models.ORM.PagedResult>(DataResult.Content.ToString());
            //Console.WriteLine("#2 " + DataResult.Content.ToString());
            string sData = await JSON.ToJson(result.Items);
            var data = await JSON.ToObject<List<Entity.HR_EMP_Employees>>(sData);
            return data;
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
            worksheet.Cell(1, 8).Value = "نام پدر";
            worksheet.Cell(1, 9).Value = "کد ملی";

            worksheet.Cell(2, 1).Value = Item.Id.ToString();
            worksheet.Cell(2, 2).Value = Item.BaseInfo_ORG_Companies?.Title;
            worksheet.Cell(2, 3).Value = Item.FirstName;
            worksheet.Cell(2, 4).Value = Item.LastName;
            worksheet.Cell(2, 5).Value = Item.EmployeeLastPersonelNo;
            worksheet.Cell(2, 6).Value = Item.EmployeeNo;
            worksheet.Cell(2, 7).Value = Item.EmployeePersonelNo;
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
        #endregion /Export

        #endregion FunctionEvents
    }
}