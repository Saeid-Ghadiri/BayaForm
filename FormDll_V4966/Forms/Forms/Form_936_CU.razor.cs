using ApiServer.External.Services;
using Baya.Models.Utility;
using BlazorBootstrap;
using DateUtils;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitko.Blazor.CKEditor;
using System;
using System.Globalization;
using System.Net;
using System.Text;
using Utility;
using static DevExpress.ReportServer.Printing.RemoteDocumentSource;

namespace Forms.Forms
{
    public class Form_936_CUBase : Form_936_CUPeropeties
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

            // فراخوانی تابع اعتبارسنجی فیلدها
            if (!await CheckFieldValidation(_Entity))
            {
                IsValid = false;
            }

            return IsValid;
        }


        /// <summary>
        /// تابع قبل اجرا شدن ارسال داده
        /// </summary>
        /// <returns></returns>
        public override async Task<Result> BeforSubmit()
        {
            // کل تغییرات قبل از Submit فرم
            PrepareEntityForSubmit();

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
            // Console.WriteLine("#Log => AfterGetData::");
            // بررسی داده ها قبل داده‌ها
            PrepareEntityForAfterGetData();

            // Console.WriteLine("#Log => AfterGetData End::");
        }


        #region FunctionEvents

        public async Task<bool> CheckFieldValidation(Entity.HR_CVR_PersonnelContract Item)
        {
            bool IsValid = true;
            // **************************************************

            // اعتبارسنجی منطقی: تاریخ پایان باید بزرگتر یا مساوی تاریخ شروع باشد
            if (!string.IsNullOrWhiteSpace(_Entity.StartDate_Fa) && !string.IsNullOrWhiteSpace(_Entity.EndDate_Fa))
            {
                if (PersianDateUtils.TryParseDateString(_Entity.StartDate_Fa, out _) &&
                    PersianDateUtils.TryParseDateString(_Entity.EndDate_Fa, out _))
                {
                    var start = PersianDateUtils.ToGregorian(_Entity.StartDate_Fa);
                    var end = PersianDateUtils.ToGregorian(_Entity.EndDate_Fa);

                    if (end < start)
                    {
                        IsValid = false;
                        await _MSG.ShowError("تاریخ پایان قرارداد نمی‌تواند کوچکتر از تاریخ شروع باشد.");
                    }
                }
            }

            // StartDate_Fa - تاریخ شروع قرارداد شمسی
            if (!string.IsNullOrWhiteSpace(Item.StartDate_Fa) && !PersianDateUtils.TryParseDateString(Item.StartDate_Fa, out _))
            {
                IsValid = false;
                await _MSG.ShowError("لطفاً تاریخ شروع قرارداد را به فرمت صحیح شمسی (مثال: 1404/01/01) وارد نمایید.");
            }

            // EndDate_Fa - تاریخ پایان قرارداد شمسی
            if (!string.IsNullOrWhiteSpace(Item.EndDate_Fa) && !PersianDateUtils.TryParseDateString(Item.EndDate_Fa, out _))
            {
                IsValid = false;
                await _MSG.ShowError("لطفاً تاریخ پایان قرارداد را به فرمت صحیح شمسی (مثال: 1404/12/29) وارد نمایید.");
            }
            // **************************************************

            // **************************************************
            // Details 
            //foreach (var item in _Entity.HR_EMP_EmployeeInfos)
            //{
            //    // فیلد نام پدر
            //    if (item.FatherName == null)
            //    {
            //        IsValid = false;
            //        await _MSG.ShowError("لطفا گزینه نام پدر را تکمیل نمایید.");
            //    }

            //}

            // **************************************************


            return IsValid;
        }

        /// <summary>
        /// آماده‌سازی موجودیت برای ذخیره — شامل:
        /// - تبدیل تاریخ شمسی به میلادی
        /// - محاسبه Total_Days, Days_Elapsed, Days_Remaining با استفاده از PersianDateUtils
        /// </summary>
        private void PrepareEntityForSubmit()
        {
            // تبدیل تاریخ‌های شمسی به میلادی — فقط اگر معتبر باشند
            if (!string.IsNullOrWhiteSpace(_Entity.StartDate_Fa) && PersianDateUtils.TryParseDateString(_Entity.StartDate_Fa, out _))
            {
                _Entity.StartDate = PersianDateUtils.ToGregorian(_Entity.StartDate_Fa);
                StateHasChanged();
            }

            if (!string.IsNullOrWhiteSpace(_Entity.EndDate_Fa) && PersianDateUtils.TryParseDateString(_Entity.EndDate_Fa, out _))
            {
                _Entity.EndDate = PersianDateUtils.ToGregorian(_Entity.EndDate_Fa);
                StateHasChanged();
            }

            // محاسبه Total_Days — با استفاده از PersianDateUtils
            if (!string.IsNullOrWhiteSpace(_Entity.StartDate_Fa) && !string.IsNullOrWhiteSpace(_Entity.EndDate_Fa))
            {
                if (PersianDateUtils.TryParseDateString(_Entity.StartDate_Fa, out _) && PersianDateUtils.TryParseDateString(_Entity.EndDate_Fa, out _))
                {
                    var difference = PersianDateUtils.GetDifference(_Entity.StartDate_Fa, _Entity.EndDate_Fa, inclusive: true);
                    var days = difference.TotalDays.ToString();
                    _Entity.Total_Days = $"{days} روز";
                }
            }
        }

        private void PrepareEntityForAfterGetData()
        {
            // اگر تاریخ میلادی وجود داشت — برای نمایش در UI به شمسی تبدیل می‌شود
            if (_Entity.StartDate.HasValue)
            {
                _Entity.StartDate_Fa = PersianDateUtils.ToPersian(_Entity.StartDate.Value);
            }

            if (_Entity.EndDate.HasValue)
            {
                _Entity.EndDate_Fa = PersianDateUtils.ToPersian(_Entity.EndDate.Value);
            }

            // محاسبه فیلدهای پویا
            RecalculateDynamicFields();
        }

        /// <summary>
        /// محاسبه مجدد فیلدهای پویا — در تغییر تاریخ‌ها
        /// </summary>
        private void RecalculateDynamicFields()
        {
            // محاسبه Days_Elapsed — با استفاده از PersianDateUtils.DaysPassed
            if (!string.IsNullOrWhiteSpace(_Entity.StartDate_Fa) && PersianDateUtils.TryParseDateString(_Entity.StartDate_Fa, out _))
            {
                var days = PersianDateUtils.DaysPassed(_Entity.StartDate_Fa, inclusive: true).ToString();
                _Entity.Days_Elapsed = $"{days} روز";
            }

            // محاسبه Days_Remaining — با استفاده از PersianDateUtils.DaysRemaining
            if (!string.IsNullOrWhiteSpace(_Entity.EndDate_Fa) && PersianDateUtils.TryParseDateString(_Entity.EndDate_Fa, out _))
            {
                var days = PersianDateUtils.DaysRemaining(_Entity.EndDate_Fa, inclusive: true).ToString();
                _Entity.Days_Remaining = $"{days} روز";
            }
        }

        public async Task StartDate_Fa_oninput(ChangeEventArgs Selected)
        {
            // محاسبه مجدد فیلدهای پویا
            RecalculateDynamicFields();
        }

        public async Task EndDate_Fa_oninput(ChangeEventArgs Selected)
        {
            //محاسبه مجدد فیلدهای پویا
            RecalculateDynamicFields();
        }

        #region بررسی SP های جدول مدت قرارداد و قرارداد
        public async Task PersonnelContract_onclick(MouseEventArgs Selected)
        {
            Console.WriteLine("### شروع فراخوانی PersonnelContract ###");

            var R = await BayaApi.PersonnelContract(
                ShomaranApiMode.Polfilm,
                new PersonnelContractRequest
                {
                    EmployeesId = _Entity.HR_EMP_EmployeesId.Value
                }
            );

            if (R == null)
            {
                await _MSG.ShowError("خروجی وب سرویس null است");
                Console.WriteLine("خطا: R == null");
                return;
            }

            var jsonResponse = R.Content.ToString();
            Console.WriteLine("### jsonResponse کامل ###");
            Console.WriteLine(jsonResponse);

            if (!(jsonResponse.TrimStart().StartsWith("{")))
            {
                await _MSG.ShowError("خروجی وب سرویس JSON نیست:\n" + jsonResponse);
                Console.WriteLine("خطا: پاسخ JSON نیست");
                return;
            }

            // --- پارس مستقیم روت ---
            JObject root;
            try
            {
                root = JObject.Parse(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine("خطا در پارس JSON کلی: " + ex.Message);
                await _MSG.ShowError("خطا در پارس JSON: " + ex.Message);
                return;
            }

            // --- تغییر اصلی: مستقیماً DataSets را از روت بگیر ---
            var dataSetsToken = root["DataSets"];
            if (dataSetsToken == null)
            {
                Console.WriteLine("خطا: DataSets در روت یافت نشد");
                await _MSG.ShowError("ساختار پاسخ نامعتبر: DataSets وجود ندارد");

                return;
            }

            if (!(dataSetsToken is JArray dataSets))
            {
                Console.WriteLine("خطا: DataSets باید آرایه باشد");
                await _MSG.ShowError("DataSets باید لیستی از لیست‌ها باشد");
                return;
            }

            Console.WriteLine($"### تعداد لیست‌های داخل DataSets: {dataSets.Count} ###");
            if (dataSets.Count < 4)
            {
                Console.WriteLine("خطا: DataSets کمتر از 4 لیست دارد");
                await _MSG.ShowError("DataSets کامل نیست (نیاز به حداقل 4 لیست دارد)");
                return;
            }

            // --- 1. لیست A: CountOfAllContract ---
            Console.WriteLine("### پردازش لیست A (CountOfAllContract) ###");
            var aList = dataSets[0] as JArray;
            if (aList == null || aList.Count == 0)
            {
                Console.WriteLine("لیست A خالی یا null است");
                await _MSG.ShowError("لیست A خالی است");
                return;
            }
            var aModel = aList[0].ToObject<SP_ContractTime.AllContractModel>();
            var A = aModel.CountOfAllContract;
            Console.WriteLine($"✅ A = {A}");

            // --- 2. لیست D: PositionClasificationId ---
            Console.WriteLine("### پردازش لیست D (PositionClasificationId) ###");
            var dList = dataSets[1] as JArray;
            if (dList == null || dList.Count == 0)
            {
                Console.WriteLine("لیست D خالی یا null است");
                await _MSG.ShowError("لیست D خالی است");
                return;
            }
            var dModel = dList[0].ToObject<SP_ContractTime.PositionClasificationModel>();
            var D = dModel.PositionClasificationId ?? "null";
            Console.WriteLine($"✅ D = '{D}'");

            // --- 3. لیست C: لیست بلند ContractTime ---
            Console.WriteLine("### پردازش لیست C (لیست بلند ContractTime) ###");
            var cList = dataSets[2] as JArray;
            if (cList == null)
            {
                Console.WriteLine("لیست C null است");
                await _MSG.ShowError("لیست C null است");
                return;
            }
            var ListC = new List<Entity.HR_CRS_ContractTime>();
            try
            {
                ListC = cList.ToObject<List<Entity.HR_CRS_ContractTime>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("خطا در تبدیل لیست C: " + ex.Message);
                if (cList.Count > 0)
                    Console.WriteLine("اولین آیتم لیست C برای عیب‌یابی:");
                Console.WriteLine(cList.FirstOrDefault()?.ToString());
                await _MSG.ShowError("خطا در پردازش لیست C: " + ex.Message);
                return;
            }
            Console.WriteLine($"✅ ListC با موفقیت بارگذاری شد. تعداد = {ListC.Count}");

            // --- 4. لیست B: TheLastCounterOfCurrentContract ---
            Console.WriteLine("### پردازش لیست B (TheLastCounterOfCurrentContract) ###");
            var bList = dataSets[3] as JArray;
            if (bList == null || bList.Count == 0)
            {
                Console.WriteLine("لیست B خالی یا null است");
                await _MSG.ShowError("لیست B خالی است");
                return;
            }
            var bModel = bList[0].ToObject<SP_ContractTime.CurrentContractModel>();
            var B = bModel.TheLastCounterOfCurrentContract ?? "null";
            Console.WriteLine($"✅ B = '{B}'");

            // --- نمایش نهایی ---
            // await _MSG.ShowInfo($"✅ نتیجه:\nA = {A}\nD = {D}\nB = {B}\nتعداد ListC = {ListC.Count}");

            var options = new BlazorBootstrap.ConfirmDialogOptions
            {
                YesButtonText = "بازگشت به قرارداد",
                YesButtonColor = ButtonColor.Info,
                NoButtonText = "",
            };

            //string htmlString = "";
            string htmlString = $@"
                <div style='text-align: right; direction: rtl; line-height: 1.8;'>
                    <div><strong>داده A:</strong> {A} </div>
                    <div><strong>داده D:</strong> {(string.IsNullOrEmpty(D) ? "ـ" : D)} </div>
                    <div><strong>داده B:</strong> {(string.IsNullOrEmpty(B) ? "ـ" : B)} </div>
                    <div><strong>تعداد داده‌های فهرست C:</strong> {ListC.Count}</div>
                </div>";

            var confirmation = await Confirm.ShowAsync(
                title: "",
                message1: htmlString,
                confirmDialogOptions: options);

            Console.WriteLine("### اتمام عملیات با موفقیت ###");
        }


        public async Task PersonnelContract1_onclick(MouseEventArgs Selected)
        {
            var R = await BayaApi.GetEndTimeOfContract(ShomaranApiMode.Polfilm,
                new PersonnelEndTimeOfContractRequest
                {
                    EmployeesId = _Entity.HR_EMP_EmployeesId.Value,
                    startDate = _Entity.StartDate.Value.ToString().Replace("/", ""),
                    contractTime = _Entity.HR_CRS_ContractTimeId.ToString()
                });
            if (R != null)
            {
                await _MSG.ShowInfo(R.Content.ToString());
            }


            Console.WriteLine("#Log_GetEndTimeOfContract::: " +
                _Entity.HR_EMP_EmployeesId.Value + "\n" +
                _Entity.StartDate.Value.ToString() + "\n" +
                _Entity.HR_CRS_ContractTimeId.ToString()
                );
        }
        #endregion بررسی SP های جدول مدت قرارداد و قرارداد

        #endregion FunctionEvents
    }
}


namespace SP_ContractTime
{
    public class AllContractModel
    {
        public int CountOfAllContract { get; set; }
    }

    public class CurrentContractModel
    {
        public string TheLastCounterOfCurrentContract { get; set; }
    }

    public class PositionClasificationModel
    {
        public string PositionClasificationId { get; set; }
    }
    //public class PersonnelContractRoot
    //{
    //    public List<List<object>> DataSets { get; set; }
    //}
}


namespace ApiServer.External.Services
{
    public partial class BayaApi
    {

        public static async Task<Result> PersonnelContract(ShomaranApiMode ApiMode, PersonnelContractRequest input)
        {
            var DataJson = await JSON.ToJson(input);

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
                    break;
            }

            var _content = new StringContent(DataJson, Encoding.UTF8, "application/json");

            Result apiresult = await Send.PostAsync(_content, "BayaApi/PersonnelContract", shomaranApi, ApplicationType.None);

            return apiresult;
        }

        public static async Task<Result> GetEndTimeOfContract(ShomaranApiMode ApiMode, PersonnelEndTimeOfContractRequest input)
        {
            var DataJson = await JSON.ToJson(input);

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
                    break;
            }

            var _content = new StringContent(DataJson, Encoding.UTF8, "application/json");

            Result apiresult = await Send.PostAsync(_content, "BayaApi/GetEndTimeOfContract", shomaranApi, ApplicationType.None);

            return apiresult;
        }


        public static async Task<Result> GetAllContract(ShomaranApiMode ApiMode, PersonnelContractRequest input)
        {
            var DataJson = await JSON.ToJson(input);

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
                    break;
            }

            var _content = new StringContent(DataJson, Encoding.UTF8, "application/json");

            Result apiresult = await Send.PostAsync(_content, "BayaApi/GetAllContract", shomaranApi, ApplicationType.None);

            return apiresult;
        }
    }
}


public class PersonnelContractRequest
{
    public Guid EmployeesId { get; set; }
}

public class PersonnelEndTimeOfContractRequest
{
    public Guid EmployeesId { get; set; }
    public string startDate { get; set; }
    public string contractTime { get; set; }
}