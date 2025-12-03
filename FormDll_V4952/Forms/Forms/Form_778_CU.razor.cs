using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using Blazored.Toast.Services;
using DateUtils;

namespace Forms.Forms
{
    public class Form_778_CUBase : Form_778_CUPeropeties
    {
        public MSG _MSG { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _MSG = new MSG(toastService);
            }
        }

        public override async Task<bool> FormValidator()
        {
            bool IsValid = true;

            // اعتبارسنجی تاریخ شروع (اگر وجود دارد)
            if (!string.IsNullOrWhiteSpace(_Entity.StartDate_Fa))
            {
                if (!PersianDateUtils.TryParseDateString(_Entity.StartDate_Fa, out _))
                {
                    IsValid = false;
                    await _MSG.ShowError("تاریخ شروع محاسبه وارد شده معتبر نیست.");
                }
            }
            else
            {
                IsValid = false;
                await _MSG.ShowError("لطفاً تاریخ شروع محاسبه را وارد کنید.");
            }

            return IsValid;
        }

        public override async Task<Result> BeforSubmit()
        {
            // 1. تبدیل تاریخ شمسی به میلادی و استخراج سال/ماه/روز
            PrepareStartDateForSubmit();

            // 2. محاسبه TotalScore
            CalculateTotalScore();

            return new Result() { Status = HttpStatusCode.OK };
        }

        private void PrepareStartDateForSubmit()
        {
            if (!string.IsNullOrWhiteSpace(_Entity.StartDate_Fa))
            {
                if (PersianDateUtils.TryParseDateString(_Entity.StartDate_Fa, out var parts))
                {
                    _Entity.StartDate = PersianDateUtils.ToGregorian(_Entity.StartDate_Fa);
                    _Entity.CalculationStartYear = parts.year.ToString();
                    _Entity.CalculationStartMonth = parts.month.ToString("00");
                    _Entity.CalculationStartDay = parts.day.ToString("00");
                }
                else
                {
                    _Entity.StartDate = null;
                    _Entity.CalculationStartYear = null;
                    _Entity.CalculationStartMonth = null;
                    _Entity.CalculationStartDay = null;
                }
            }
            else
            {
                _Entity.StartDate = null;
                _Entity.CalculationStartYear = null;
                _Entity.CalculationStartMonth = null;
                _Entity.CalculationStartDay = null;
            }
        }

        private void CalculateTotalScore()
        {
            _Entity.TotalScore = 
                Coalesce(_Entity.ScoreAffectingProduct) +
                Coalesce(_Entity.RiskToleranceScore) +
                Coalesce(_Entity.ScoreResponsibilityAndAccountability) +
                Coalesce(_Entity.ExpertiseScore) +
                Coalesce(_Entity.ScorePersonalInteractions) +
                Coalesce(_Entity.ScoreGreatDecisions) +
                Coalesce(_Entity.DecisionMakingScore) +
                Coalesce(_Entity.DecisionScore) +
                Coalesce(_Entity.ScoreReports);
        }

        // کمکی: تبدیل null به 0 برای فیلدهای عددی nullable
        private decimal Coalesce(decimal? value) => value ?? 0;
        private int Coalesce(int? value) => value ?? 0;
        private double Coalesce(double? value) => value ?? 0;
        // اگر فیلدها از نوع double یا decimal هستند، متد مناسب را استفاده کنید.
        // در اینجا فرض کردم از نوع decimal? یا double? هستند.
    }
}