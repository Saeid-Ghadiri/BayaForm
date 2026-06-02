using Baya.Models.Utility;
using BlazorBootstrap;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Sitko.Blazor.CKEditor;
using System;
using System.Net;

namespace Forms.Forms
{
	public class Form_1164Base : Form_1164Peropeties
	{
		// SCMATLASCELL_OS_ResultingFrom — Code 3: پیمانکار
		private static readonly Guid ResultingFromContractorId = Guid.Parse("AD87E05A-B65C-F111-A514-005056A2B6BD");

		public MSG _MSG { get; set; }

		protected override async Task OnInitializedAsync()
		{
		}

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

			var List = _Entity.SCMATLASCELL_OS_Details
							.Where(p => p.IsDelete != true)
							.ToList();

			if (List.Count == 0)
			{
				IsValid = false;
				await ShowEmptyRequestDialogAsync(_Entity.RequestTrakingCode);
			}

			foreach (var Item in List)
			{
				IsValid = IsValid && await CheckFieldValidation(Item);
			}

			if (_Entity.SCMATLASCELL_OS_ResultingFromId == null)
			{
				IsValid = false;
				await _MSG.ShowError("لطفا یک نوع منتج از درخواست برون سپاری یا خرید خدمات را انتخاب نمایید.");
			}

			return IsValid;
		}

		public override async Task<Result> BeforSubmit()
		{
			return new Result() { Status = HttpStatusCode.OK };
		}

		public override async Task AfterSubmit()
		{
		}

		public override async Task BeforGetData()
		{
		}

		public override async Task AfterGetData()
		{
		}

		#region FunctionEvents

		#region نمایش دیالوگ خطا برای درخواست بدون ردیف
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
					<picture>
						<img src='https://file.workcv.ir/fa-ir/api/v1/File/Get?FileID=e35322d0-23f4-4d03-b886-08de9f64ab6f'
							 class='logo-image'
							 alt='لوگو اطلس سلولز'
							 width='96'>
					</picture>
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

			await Confirm.ShowAsync(
				title: "",
				message1: htmlString,
				confirmDialogOptions: options
				);
		}
		#endregion

		#region اعتبارسنجی فیلدها
		public async Task<bool> CheckFieldValidation(Entity.SCMATLASCELL_OS_Details Item)
		{
			bool IsValid = true;

			if (string.IsNullOrWhiteSpace(Item.OS_JobTitle))
			{
				IsValid = false;
				await _MSG.ShowError("لطفا گزینه عنوان کار را تکمیل نمایید.");
			}

			if (Item.Global_UnitsId == null)
			{
				IsValid = false;
				await _MSG.ShowError("لطفا گزینه واحد کالا را تکمیل نمایید.");
			}

			if (Item.Amount == null || Item.Amount == 0)
			{
				IsValid = false;
				await _MSG.ShowError("لطفا گزینه تعداد را تکمیل نمایید.");
			}

			if (Item.Global_PriorityId == null)
			{
				IsValid = false;
				await _MSG.ShowError("لطفا گزینه اولویت را تکمیل نمایید.");
			}

			if (string.IsNullOrWhiteSpace(Item.AreaUse))
			{
				IsValid = false;
				await _MSG.ShowError("لطفا گزینه محل مصرف را تکمیل نمایید.");
			}

			if (Item.UploadFileIsEnable == null)
			{
				IsValid = false;
				await _MSG.ShowError("لطفا گزینه آیا مدارک پیوست دارد؟ را تکمیل نمایید.");
			}

			if (Item.UploadFileIsEnable.HasValue && Item.UploadFileIsEnable.Value)
			{
				if (Item.SCMATLASCELL_OS_Details_UploadFiles == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه فایل مدارک پیوست را تکمیل نمایید.");
				}
			}

			if (Item.IsEnableSampleGoods == null)
			{
				IsValid = false;
				await _MSG.ShowError("لطفا گزینه آیا این درخواست نمونه دارد؟ را تکمیل نمایید.");
			}

			if (Item.IsEnableSampleGoods.HasValue && Item.IsEnableSampleGoods.Value)
			{
				if (Item.SCMATLASCELL_OS_Details_SampleGoodsFiles == null)
				{
					IsValid = false;
					await _MSG.ShowError("لطفا گزینه فایل نمونه را تکمیل نمایید.");
				}
			}

			return IsValid;
		}
		#endregion

		#region مدیریت ResultingFrom
		private bool IsContractorResultingFrom()
		{
			return _Entity.SCMATLASCELL_OS_ResultingFromId.HasValue
				&& _Entity.SCMATLASCELL_OS_ResultingFromId.Value == ResultingFromContractorId;
		}

		private async Task ApplyContractorDemolitionField(bool visible, bool value, Entity.SCMATLASCELL_OS_Details Item)
		{
			Ref_SCMATLASCELL_OS_Details_IsEnableDemolitionAndRenovation.SetVisible(visible);
			Item.IsEnableDemolitionAndRenovation = value;
		}
		#endregion

		public async Task<bool> GridSCMATLASCELL_OS_MasterId_872_editmodelsaving(object e)
		{
			var Item = e as Entity.SCMATLASCELL_OS_Details;

			if (Item == null)
			{
				await _MSG.ShowError("آیتم نامعتبر است");
				return true;
			}

			return !await CheckFieldValidation(Item);
		}

		public async Task GridSCMATLASCELL_OS_MasterId_872_afterrendermodal(Entity.SCMATLASCELL_OS_Details Item)
		{
			if (Item.UploadFileIsEnable.HasValue && Item.UploadFileIsEnable.Value)
			{
				Ref_SCMATLASCELL_OS_Details_SCMATLASCELL_OS_Details_UploadFiles.SetVisible(true);
			}
			else
			{
				Ref_SCMATLASCELL_OS_Details_SCMATLASCELL_OS_Details_UploadFiles.SetVisible(false);
			}

			if (IsContractorResultingFrom())
			{
				await ApplyContractorDemolitionField(true, true, Item);
			}
			else
			{
				await ApplyContractorDemolitionField(false, false, Item);
			}

			if (Item.IsEnableSampleGoods.HasValue && Item.IsEnableSampleGoods.Value)
			{
				Ref_SCMATLASCELL_OS_Details_SCMATLASCELL_OS_Details_SampleGoodsFiles.SetVisible(true);
			}
			else
			{
				Ref_SCMATLASCELL_OS_Details_SCMATLASCELL_OS_Details_SampleGoodsFiles.SetVisible(false);
			}
		}

		public async Task UploadFileIsEnable_oninput(ChangeEventArgs Selected, Entity.SCMATLASCELL_OS_Details Item)
		{
			if (Selected.Value.ToString() == "true")
			{
				Ref_SCMATLASCELL_OS_Details_SCMATLASCELL_OS_Details_UploadFiles.SetVisible(true);
			}
			else
			{
				Ref_SCMATLASCELL_OS_Details_SCMATLASCELL_OS_Details_UploadFiles.SetVisible(false);
			}
		}

		public async Task IsEnableSampleGoods_oninput(ChangeEventArgs Selected, Entity.SCMATLASCELL_OS_Details Item)
		{
			if (Selected.Value.ToString() == "true")
			{
				Ref_SCMATLASCELL_OS_Details_SCMATLASCELL_OS_Details_SampleGoodsFiles.SetVisible(true);
			}
			else
			{
				Ref_SCMATLASCELL_OS_Details_SCMATLASCELL_OS_Details_SampleGoodsFiles.SetVisible(false);
			}
		}

		public async Task IsEnableDemolitionAndRenovation_oninput(ChangeEventArgs Selected, Entity.SCMATLASCELL_OS_Details Item)
		{
		}

		#endregion FunctionEvents
	}
}