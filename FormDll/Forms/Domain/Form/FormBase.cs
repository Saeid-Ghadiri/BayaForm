using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;

namespace Domain.Form
{
	public class FormBase : ComponentBase
	{

		/// <summary>
		/// سامبیت فرم
		/// </summary>
		/// <returns></returns>
		public virtual async Task<Baya.Models.Utility.Result> Submit()
		{
			return null;
		}

		/// <summary>
		/// تابع ولیدیت کردن فرم
		/// </summary>
		/// <returns></returns>
		public virtual async Task<bool> FormValidator()
		{
			bool IsValid = true;

			return IsValid;
		}

		/// <summary>
		/// تابع قبل اجرا شدن ارسال داده
		/// </summary>
		/// <returns></returns>
		public virtual async Task<Result> BeforSubmit()
		{

			return null;
		}

		/// <summary>
		/// تابع بعد اجرا شدن ارسال داده
		/// </summary>
		/// <returns></returns>
		public virtual async Task AfterSubmit()
		{

		}

		/// <summary>
		/// تابع قبل دریافت داده
		/// </summary>
		/// <returns></returns>
		public virtual async Task BeforGetData()
		{

		}

		/// <summary>
		/// تابع بعد دریافت داده
		/// </summary>
		/// <returns></returns>
		public virtual async Task AfterGetData()
		{

		}

	}
}
