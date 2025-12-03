using Baya.Models.Utility;
using BlazorBootstrap;
using Blazored.Toast.Services;
using DevExpress.Blazor;
using Entity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Sitko.Blazor.CKEditor;
using System;
using System.Net;

namespace Forms.Forms
{
	public class Form_576Base : Form_576Peropeties
	{

		// Toast  
		[Inject]
		public IToastService toastService { get; set; }

		// تابع پیام تُــست
		public MSG _MSG { get; set; }


		// Id					                    ProcurementExpert
		// **********************************************************
		// ae92a51e-f182-ee11-8320-005056a02a64 	جواد جعفرزاده
		// af92a51e-f182-ee11-8320-005056a02a64	    علی عباسی
		// b092a51e-f182-ee11-8320-005056a02a64	    مهدی عباسی
		// a473ea8f-2061-ef11-8351-005056a02a64	    دنیا پور رضایی
		// 55c03b4f-ebcb-ef11-a4fa-005056a2b6bd     علی زمانی


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
			_masterValidated = false; // ریست فلگ قبل از هر اعتبارسنجی

			bool IsValid  = true;

			var List = _Entity.SCM_OS_Details.Where(p => p.IsDelete != true).ToList();

			if (!List.Any())
			{
				// مودال بررسی صحت گرید
				await ShowNoDetailRowWarningAsync();
				return false;
			}
			// اعتبارسنجی هر سطر + Master (اما Master فقط یک‌بار)
			foreach (var Item in List)
			{
				// دکمه ثبت و ادامه           
				if (BtnWorkFlowId == "SequenceFlow_0mdk2y4" || BtnWorkFlowId == "SequenceFlow_0jofgmf")
				{
					// Details Validation
					IsValid = IsValid && await CheckFieldValidation(Item, includeMaster: true);
				}
			}

			return IsValid;
		}

		/// <summary>
		/// ارسال داده
		/// </summary>
		/// <returns></returns>
		public override async Task<Baya.Models.Utility.Result> Submit()
		{
			SumaryMessage = "";
			Baya.Models.Utility.Result Result = new Baya.Models.Utility.Result();

			if (!await FormValidator())
			{
				StateHasChanged();
				Result.Status = HttpStatusCode.InternalServerError;
				return Result;
			}

			await BeforSubmit();

			var Resualt = await PostData();

			if (Resualt)
			{
				Result.Status = HttpStatusCode.OK;

				SumaryMessage = "داده ها با موفقیت ثبت شد";
			}
			else
			{
				Result.Status = HttpStatusCode.InternalServerError;
				SumaryMessage = "ذخیره داده با مشکل مواجه شد";
			}

			Result.Message = SumaryMessage;

			switch ((int)Result.Status)
			{
				case 200:
					toastService.ShowSuccess(Result.Message);
					break;
				case 500:
					toastService.ShowError(Result.Message);
					break;
				default:
					break;
			}

			await AfterSubmit();

			return Result;
		}

		/// <summary>
		/// ارسال داده
		/// </summary>
		/// <returns></returns>
		private async Task<bool> PostData()
		{
			string Data = await Utility.JSON.ToJson(_Entity);

			bool IsOk = false;
			var Model = await ApiServer.External.Services.Data.Put(Data, TablePost.Name, TablePost, RequestID?.ToString(), _User.UserID.ToString());
			if (Model?.Status == HttpStatusCode.OK)
			{
				IsOk = true;
			}

			return IsOk;
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

		}

		#region FunctionEvents

		// 
		private bool _masterValidated = false;

		/// <summary>
		/// اعتبارسنجی فرم
		/// </summary>
		/// <param name="Item"></param>
		/// <param name="includeMaster"></param>
		/// <returns></returns>
		public async Task<bool> CheckFieldValidation(Entity.SCM_OS_Details item, bool includeMaster = false)
		{
			bool isValid = true;

			// اعتبارسنجی Master (فقط یک‌بار)
			if (includeMaster && !_masterValidated)
			{
				_masterValidated = true;

				if (_Entity.SCM_ICT_DepartmentsId == null)
				{
					isValid = false;
					await _MSG.ShowError("لطفا بخش‌های فناوری اطلاعات را انتخاب نمایید.");
				}

				if (_Entity.SCM_ResultingFromId == null)
				{
					isValid = false;
					await _MSG.ShowError("لطفا یک نوع منتج از درخواست برون سپاری یا خرید خدمات را انتخاب نمایید.");
				}
			}

			// اعتبارسنجی Details
			if (item.SCM_PriorityId == Guid.Empty)
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه اولویت را تکمیل نمایید.");
			}

			if (item.SCM_UnitsId == Guid.Empty)
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه واحد کالا را تکمیل نمایید.");
			}

			if (item.Amount == null || item.Amount <= 0)
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه تعداد یا مقدار را تکمیل نمایید.");
			}

			if (item.GuaranteeIsEnable == null)
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه آیا گارانتی دارد؟ را تکمیل نمایید.");
			}

			if (item.HistoryRepairsIsEnable == null)
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه سابقه تعمیرات دارد؟ را تکمیل نمایید.");
			}

			if (string.IsNullOrWhiteSpace(item.Title))
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه عنوان کار را تکمیل نمایید.");
			}

			if (string.IsNullOrWhiteSpace(item.AreaUse))
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه محل مصرف را تکمیل نمایید.");
			}

			if (item.UploadFileIsEnable == null)
			{
				isValid = false;
				await _MSG.ShowError("لطفا گزینه فایل مدارک را تکمیل نمایید.");
			}

			// نیازی به بررسی ITILCodeIsEnable نیست — این فیلد در UI وجود ندارد
			// همه چیز از طریق ResultingFromITIL کنترل می‌شود

			return isValid;
		}


		public async Task<bool> ShowNoDetailRowWarningAsync()
		{
			var options = new ConfirmDialogOptions
			{
				YesButtonText = "بازگشت به درخواست",
				YesButtonColor = ButtonColor.Danger,
				NoButtonText = "",
			};

			string htmlString =
				"<div>" +
					"<picture>" +
						"<img src='https://file.workcv.ir/fa/api/v1/File/Get?FileID=6e5b6fb8-a5b2-490c-f83f-08dbea5b8061' class='' alt='لوگو پل فیلم' width='96px;'>" +
					"</picture>" +
					"<hr class='hrdash border-warning-subtle'>" +
				"</div>" +
				"<div class='fw-bold text-center'>" +
					"<span class='fs-5'>کد پیگیری این درخواست: </span>" +
					"<span class='fs-3' style='color: #1ba156'>" + _Entity.RequestTrakingCode + "</span><div>" +
					"<span><i class='fal fa-exclamation-triangle' style='font-size:24px; color:red;'></i>&nbsp;</span>" +
					"<span class='fs-6 text-secondary text-right'>تا کنون هیچ ردیف درخواستی تکمیل نشده است. لطفا برای ثبت و ادامه به مرحله بعد حداقل یک ردیف در درخواست خود ثبت نمایید.<span></div>" +
				"</div>";

			var confirmation = await Confirm.ShowAsync(
				title: "",
				message1: htmlString,
				confirmDialogOptions: options);

			// اگر کاربر روی "بازگشت به درخواست" کلیک کند، مقدار false برگردانده می‌شود (یعنی نباید ادامه یابد)
			// ولی در اینجا ما فقط نمایش می‌دهیم و تصمیم‌گیری برعهده FormValidator است.
			// پس فقط نمایش داده شد → همیشه false برگردانیم چون ردیفی وجود ندارد!
			return false;
		}

		public async Task ITILVisible(bool Visible)
		{
			Ref_SCM_OS_Details_ResultingFromITIL.SetVisible(Visible);
		}
		public async Task ITILDetailsVisible(bool Visible, Entity.SCM_OS_Details Item)
		{
			// شماره درخواست
			Ref_SCM_OS_Details_RequestIdITIL.SetVisible(Visible);
			Ref_SCM_OS_Details_RequestIdITIL.SetDisabled(true);

			// کاربر درخواست دهنده ITIL
			Ref_SCM_OS_Details_RequesterUserITIL.SetVisible(Visible);
			Ref_SCM_OS_Details_RequesterUserITIL.SetDisabled(true);

			// زمان گزارش
			Ref_SCM_OS_Details_CreatedAtITIL.SetVisible(Visible);
			Ref_SCM_OS_Details_CreatedAtITIL.SetDisabled(true);

			// جزئیات جدول ITIL
			Ref_SCM_OS_Details_ITILDetails.SetVisible(Visible);

			Ref_SCM_OS_Details_ITILDetails.SetEntity(Item);
			await Task.Delay(100);
			Ref_SCM_OS_Details_ITILDetails.LoadData();
		}

		public async Task<bool> SCM_OS_Details_editmodelsaving(object e)
		{
			bool IsCancelled = false;

			var Item = (Entity.SCM_OS_Details)e;

			IsCancelled = !await CheckFieldValidation(Item, includeMaster: false);

			return IsCancelled;
		}

		/// <summary>
		/// به‌روزرسانی وضعیت فیلدهای وابسته بر اساس مقدارهای سطر جزئیات
		/// فراخوانی شود هر وقت مقدارهای کلیدی تغییر کنند یا مودال باز شود.
		/// </summary>
		public async Task UpdateDependentFieldsAsync(Entity.SCM_OS_Details item)
		{
			// 1. مدیریت فیلدهای مربوط به ITIL
			bool showITILFields = item.ResultingFromITIL != null;
			await ITILDetailsVisible(showITILFields, item);

			// 2. مدیریت فیلد مدارک پیوست
			bool showUpload = item.UploadFileIsEnable == true; // null یا false → false
			Ref_SCM_OS_Details_SCM_OS_Details_UploadFile?.SetVisible(showUpload);
		}

		public async Task SCM_OS_Details_afterrendermodal(Entity.SCM_OS_Details Item)
		{
			// نمایش جزئیات ITIL
			if (Item.ResultingFromITIL != null)
			{
				// نمایش / عدم نمایش فیلد ITIL Detail
				await ITILDetailsVisible(true, Item);
			}
			else
			{
				// نمایش / عدم نمایش فیلد ITIL Detail
				await ITILDetailsVisible(false, Item);
			}

			// فایل مدارک
			var UploadFilesIsVisible = Ref_SCM_OS_Details_SCM_OS_Details_UploadFile;
			if (Item.UploadFileIsEnable.HasValue && Item.UploadFileIsEnable.Value)
			{
				UploadFilesIsVisible.SetVisible(true);
			}
			else
			{
				UploadFilesIsVisible.SetVisible(false);
			}
		}

		//public async Task ITILCodeIsEnable_oninput(ChangeEventArgs Selected, Entity.SCM_OS_Details Item)
		//{
		//	// نمایش / عدم نمایش فیلد منتج از ITIL
		//	if (Selected.Value.ToString() == "true")
		//	{
		//		await ITILVisible(true);
		//	}
		//	else
		//	{
		//		await ITILVisible(false);
		//	}
		//}

		public async Task ResultingFromITIL_onitemselected(dynamic Selected, Entity.SCM_OS_Details Item)
		{
			if (Item.ResultingFromITIL != null)
			{
				Item.RequestIdITIL = Selected.RequestID;
				Item.RequesterUserITIL = Selected.UserName;
				Item.CreatedAtITIL = Selected.CreateDate;

				// نمایش / عدم نمایش فیلد ITIL Detail
				await ITILDetailsVisible(true, Item);
			}
			else
			{
				// نمایش / عدم نمایش فیلد ITIL Detail
				await ITILDetailsVisible(false, Item);
			}
		}

		public async Task UploadFileIsEnable_oninput(ChangeEventArgs Selected, Entity.SCM_OS_Details Item)
		{
			// نمایش / عدم نمایش فیلد فایل مدارک
			var UploadFilesIsVisible = Ref_SCM_OS_Details_SCM_OS_Details_UploadFile;
			if (Selected.Value.ToString() == "true")
			{
				UploadFilesIsVisible.SetVisible(true);
			}
			else
			{
				UploadFilesIsVisible.SetVisible(false);
			}
		}

		#endregion FunctionEvents

	}
}