using System;
using System.Net;
using Baya.Models.Utility;
using BlazorBootstrap;
using Blazored.Toast.Services;
using Castle.DynamicLinqQueryBuilder;
using DateUtils;
using DevExpress.Blazor;
using Entity;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Forms.Forms
{
	public class Form_1010Base : Form_1010Peropeties
	{
		public MSG _MSG { get; set; }

		protected override async Task OnInitializedAsync()
		{
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_MSG = new MSG(toastService);
				await ToggleDetailsGridAddButton();
				// تنظیم وضعیت نمایش اولیه
				await ToggleResultOfAnotherProcessFields(_Entity?.IsResultOfAnotherProcess == true);
			}
			else
			{
				var currentCount = _Entity.IDMS_RDC_Details?
									.Count(x => x.IsDelete != true) ?? 0;

				if (currentCount != _lastDetailsCount)
				{
					_lastDetailsCount = currentCount;
					await ToggleDetailsGridAddButton();
				}
			}
		}

		public override async Task<bool> FormValidator()
		{
			bool IsValid = true;

			var List = _Entity.IDMS_RDC_Details
							.Where(p => p.IsDelete != true)
							.ToList();

			if (List.Count == 0)
			{
				IsValid = false;
				await ShowEmptyRequestDialogAsync(_Entity.RequestTrakingCode);
			}

			await HasValidDetailsCount();

			foreach (var Item in List)
				IsValid = IsValid && await CheckFieldsValidation(Item);

			return IsValid;
		}

		public override async Task<Result> BeforSubmit()
		{
			PrepareRequestedDueDatesForSubmit();
			return new Result { Status = HttpStatusCode.OK };
		}

		public override async Task AfterSubmit() 
		{
		}

		public override async Task BeforGetData()
		{
		}

		public override async Task AfterGetData()
		{
			await ToggleDetailsGridAddButton();
			await ToggleResultOfAnotherProcessFields(_Entity?.IsResultOfAnotherProcess == true);
			await Task.Delay(300);
		}

		#region FunctionEvents

		// ───── رویداد اصلی چک‌باکس «منتج از فرایند دیگر» ─────
		public async Task IsResultOfAnotherProcess_oninput(ChangeEventArgs selected)
		{
			bool isChecked = false;
			if (selected?.Value != null)
				bool.TryParse(selected.Value.ToString(), out isChecked);

			_Entity.IsResultOfAnotherProcess = isChecked;

			if (!isChecked)
				await ClearAnotherProcessData();

			await ToggleResultOfAnotherProcessFields(isChecked);
		}

		// ───── پاک کردن داده‌های فیلدهای وابسته هنگام غیرفعال شدن ─────
		private async Task ClearAnotherProcessData()
		{
			if (Ref_TrackingCode != null)
				await Ref_TrackingCode.ResetDropdown();

			//_Entity.IDMS_RDC_AllData = null;

			if (Ref_IDMS_RDC_AllData != null)
				await Ref_IDMS_RDC_AllData.ResetDropdown();

			//await InvokeAsync(StateHasChanged);

			StateHasChanged();
		}

		// ───── Dropdown Events ─────
		public async Task TrackingCode_onitemselected(dynamic Selected)
		{
			if (Selected == null)
			{
				// مخفی کردن گرید و پاک‌سازی
				_Entity.IDMS_RDC_AllData = null;
				if (Ref_IDMS_RDC_AllData != null)
					await Ref_IDMS_RDC_AllData.ResetDropdown();

				await JS.InvokeVoidAsync("AddClass", "#IDMS_RDC_AllData", "d-none");
				return;
			}

			string selectedTrackingCode = Selected.RequestTrakingCode;
			if (string.IsNullOrWhiteSpace(selectedTrackingCode))
				return;

			var filter = new QueryBuilderFilterRule()
			{
				Condition = "AND",
				Rules = new List<QueryBuilderFilterRule>()
				{
					new QueryBuilderFilterRule()
					{
						Id = "RequestTrakingCode",
						Field = "RequestTrakingCode",
						Type = "string",
						Input = "text",
						Operator = "equal",
						Value = new string[] { selectedTrackingCode }
					}
				}
			};

			// نمایش گرید و بارگذاری داده‌ها
			await JS.InvokeVoidAsync("RemoveClass", "#IDMS_RDC_AllData", "d-none");
			await Ref_IDMS_RDC_AllData.Search(filter);
		}

		public async Task ItemSelected_IDMS_RDC_MasterIDMS_RDC_AllData(object _Item)
		{
			if (_Item == null)
			{
				_Entity.IDMS_RDC_AllData = null;
				return;
			}

			try
			{
				var jsonResult = await Utility.JSON.ToJson(_Item);
				if (jsonResult == null)
				{
					_Entity.IDMS_RDC_AllData = null;
					return;
				}

				var Selected = await Utility.JSON.ToObject<dynamic>(jsonResult);
				if (Selected == null)
				{
					_Entity.IDMS_RDC_AllData = null;
					return;
				}

				_Entity.IDMS_RDC_AllData = Selected.MasterId;
			}
			catch
			{
				_Entity.IDMS_RDC_AllData = null;
			}
		}

		// ───── Grid Events: IDMS_RDC_Details ─────
		public async Task<bool> GridIDMS_RDC_MasterId_741_editmodelsaving(object e)
		{
			var Item = (Entity.IDMS_RDC_Details)e;
			return !await CheckFieldsValidation(Item);
		}

		public async Task UpdateDetailsGridAddButtonAfterSave()
		{
			await ToggleDetailsGridAddButton();
			_lastDetailsCount = _Entity.IDMS_RDC_Details?.Count(x => x.IsDelete != true) ?? 0;
		}

		public async Task GridIDMS_RDC_MasterId_741_afterrendermodal(Entity.IDMS_RDC_Details item)
		{
			var hasSavedRecord = _Entity.IDMS_RDC_Details?.Any(x => x.IsDelete != true) == true;
			if (item != null && item.Id != Guid.Empty && item.IsDelete != true)
			{
				if (_Entity.IDMS_RDC_Details?.Any(x => x.Id == item.Id && x.IsDelete != true) == true)
					hasSavedRecord = true;
			}
			await ToggleDetailsGridAddButton(hasSavedRecord, isInModal: true);
		}

		// ───── Grid Events: IDMS_TestModel ─────
		public async Task<bool> GridIDMS_RDC_MasterId_753_editmodelsaving(object e) => false;

		public async Task GridIDMS_RDC_MasterId_753_afterrendermodal(Entity.IDMS_TestModel item) { }

		public async Task GridIDMS_RDC_MasterId_753_customizeeditmodel(GridCustomizeEditModelEventArgs e) { }

		// ───── اعتبارسنجی‌ها ─────
		private int _lastDetailsCount = 0;

		private async Task<bool> HasValidDetailsCount()
		{
			if (_Entity.IDMS_RDC_Details == null)
			{
				await _MSG.ShowError("لطفاً حداقل یک ردیف در بخش «جزئیات سیستم تحقیق و توسعه» ثبت کنید.");
				return false;
			}

			var activeCount = _Entity.IDMS_RDC_Details.Count(x => x.IsDelete != true);
			if (activeCount == 0)
			{
				await _MSG.ShowError("لطفاً حداقل یک ردیف در بخش «جزئیات سیستم تحقیق و توسعه» ثبت کنید.");
				return false;
			}
			if (activeCount > 1)
			{
				await _MSG.ShowError("شما مجاز به ثبت بیش از یک ردیف نیستید. لطفاً فقط یک ردیف در بخش «جزئیات سیستم تحقیق و توسعه» ثبت کنید.");
				return false;
			}
			return true;
		}

		private async Task<bool> CheckFieldsValidation(Entity.IDMS_RDC_Details Item) => true;

		public async Task<bool> ValidateMaxFiveProductRequestRecords(bool isNewRow = false)
		{
			int activeCount = _Entity.IDMS_RDC_Details.Count(p => p.IsDelete != true);
			if (activeCount >= 1)
			{
				await _MSG.ShowError("تعداد رکوردها نمی‌تواند بیشتر از ۱ باشد.");
				return false;
			}
			return true;
		}

		private async Task ShowEmptyRequestDialogAsync(string trackingCode)
		{
			var options = new ConfirmDialogOptions
			{
				YesButtonText = "بازگشت به درخواست",
				YesButtonColor = ButtonColor.Danger,
				NoButtonText = "",
			};

			string htmlString = $@"
                <div class='request-header'>
                    <hr class='hrdash border-success-subtle'>
                </div>
                <div class='fw-bold text-center'>
                    <div class='mb-3'>
                        <span class='fs-5'>کد پیگیری این درخواست: </span>
                        <span class='fs-3' style='color: #1ba156'>{trackingCode}</span>
                    </div>
                    <div class='d-flex justify-content-center align-items-start gap-2'>
                        <i class='fal fa-exclamation-triangle' style='font-size:24px; color:red;' aria-hidden='true'></i>
                        <span class='fs-6 text-secondary text-end'>
                            تا کنون هیچ ردیف درخواستی تکمیل نشده است. لطفا برای ثبت و ادامه به مرحله بعد حداقل یک ردیف در درخواست خود ثبت نمایید.
                        </span>
                    </div>
                </div>";

			await Confirm.ShowAsync(title: "", message1: htmlString, confirmDialogOptions: options);
		}

		public async Task IDMS_ProductTypesId_onitemselected(dynamic Selected, Entity.IDMS_RDC_Details Item) { }

		// ───── UI Control Helpers ─────
		private void PrepareRequestedDueDatesForSubmit()
		{
			if (_Entity?.IDMS_RDC_Details == null) return;

			foreach (var detail in _Entity.IDMS_RDC_Details.Where(x => x.IsDelete != true))
			{
				if (!string.IsNullOrWhiteSpace(detail.RequestedDueDate_Fa))
				{
					if (PersianDateUtils.TryParseDateString(detail.RequestedDueDate_Fa, out _))
						detail.RequestedDueDate = PersianDateUtils.ToGregorian(detail.RequestedDueDate_Fa);
					else
						detail.RequestedDueDate = null;
				}
				else
					detail.RequestedDueDate = null;
			}
		}

		private async Task ToggleDetailsGridAddButton(bool hasSavedRecord = false, bool isInModal = false)
		{
			await Task.Yield();

			if (!hasSavedRecord)
				hasSavedRecord = _Entity.IDMS_RDC_Details?.Any(x => x.IsDelete != true) == true;

			if (hasSavedRecord)
				await JS.InvokeVoidAsync("AddClass", "#IDMS_RDC_Details_GridIDMS_RDC_MasterId_741ButtonNew", "d-none");
			else
				await JS.InvokeVoidAsync("RemoveClass", "#IDMS_RDC_Details_GridIDMS_RDC_MasterId_741ButtonNew", "d-none");

			if (isInModal)
			{
				await JS.InvokeVoidAsync("ModalAddClass", "#IDMS_RDC_Details_GridIDMS_RDC_MasterId_741ButtonSaveAndNew", "d-none");
				await JS.InvokeVoidAsync("ModalAddClass", "#IDMS_RDC_Details_GridIDMS_RDC_MasterId_741ButtonBefore", "d-none");
				await JS.InvokeVoidAsync("ModalAddClass", "#IDMS_RDC_Details_GridIDMS_RDC_MasterId_741ButtonNext", "d-none");
			}
		}

		/// <summary>
		/// نمایش/مخفی کردن فیلدهای TrackingCode و IDMS_RDC_AllData.
		/// IDMS_RDC_AllData فقط زمانی نمایش داده می‌شود که TrackingCode مقدار داشته باشد.
		/// </summary>
		private async Task ToggleResultOfAnotherProcessFields(bool showFields)
		{
			if (showFields)
			{
				// نمایش TrackingCode
				await JS.InvokeVoidAsync("RemoveClass", "#TrackingCode", "d-none");

				// IDMS_RDC_AllData فقط در صورت پر بودن TrackingCode نمایش یابد
				bool hasTrackingCode = !string.IsNullOrWhiteSpace(_Entity.TrackingCode);
				if (hasTrackingCode)
					await JS.InvokeVoidAsync("RemoveClass", "#IDMS_RDC_AllData", "d-none");
				else
					await JS.InvokeVoidAsync("AddClass", "#IDMS_RDC_AllData", "d-none");
			}
			else
			{
				// مخفی کردن هر دو
				await JS.InvokeVoidAsync("AddClass", "#TrackingCode", "d-none");
				await JS.InvokeVoidAsync("AddClass", "#IDMS_RDC_AllData", "d-none");
			}

			await InvokeAsync(StateHasChanged);
		}

		#endregion FunctionEvents
	}
}