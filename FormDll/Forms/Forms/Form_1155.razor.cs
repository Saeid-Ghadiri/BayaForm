using Baya.Models.Utility;
using BlazorBootstrap;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Sitko.Blazor.CKEditor;
using System;
using System.Linq;
using System.Net;

namespace Forms.Forms
{
	public class Form_1155Base : Form_1155Peropeties
	{
		private static readonly Guid ResultingFromContractorId = Guid.Parse("AD87E05A-B65C-F111-A514-005056A2B6BD");

		public MSG _MSG { get; set; }

		private void EnsureMsgInitialized()
		{
			_MSG ??= new MSG(toastService);
		}

		protected override async Task OnInitializedAsync()
		{
			EnsureMsgInitialized();
			await base.OnInitializedAsync();
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			EnsureMsgInitialized();
			await base.OnAfterRenderAsync(firstRender);
		}

		public override async Task<bool> FormValidator()
		{
			EnsureMsgInitialized();
			bool IsValid = true;

			if (_Entity?.SCMATLASCELL_OS_Details == null)
			{
				IsValid = false;
				await _MSG.ShowError("لیست ردیف‌های درخواست بارگذاری نشده است.");
				return false;
			}

			var List = _Entity.SCMATLASCELL_OS_Details
							.Where(p => p.IsDelete != true)
							.ToList();

			if (List.Count == 0)
			{
				IsValid = false;
				await ShowEmptyRequestDialogAsync(_Entity.RequestTrakingCode ?? string.Empty);
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

			if (_Entity.SCMATLASCELL_ICT_DepartmentsId == null)
			{
				IsValid = false;
				await _MSG.ShowError("لطفا گزینه بخش‌های فناوری اطلاعات را انتخاب نمایید.");
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
			EnsureMsgInitialized();
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

			if (Item.GuaranteeIsEnable == null)
			{
				IsValid = false;
				await _MSG.ShowError("لطفا گزینه آیا گارانتی دارد؟ را تکمیل نمایید.");
			}

			if (Item.HistoryRepairsIsEnable == null)
			{
				IsValid = false;
				await _MSG.ShowError("لطفا گزینه سابقه تعمیرات دارد؟ را تکمیل نمایید.");
			}

			return IsValid;
		}
		#endregion

		#region مدیریت ResultingFrom (پیمانکار)
		private bool IsContractorResultingFrom()
		{
			return _Entity.SCMATLASCELL_OS_ResultingFromId.HasValue
				&& _Entity.SCMATLASCELL_OS_ResultingFromId.Value == ResultingFromContractorId;
		}

		public async Task ResultingFrom_Contractor(bool visible, bool value, Entity.SCMATLASCELL_OS_Details item)
		{
			item.IsEnableDemolitionAndRenovation = value;
		}
		#endregion

		#region مدیریت ITIL
		// نمایش فیلد itil
		public async Task ITILVisible(bool Visible)
		{
			Ref_SCMATLASCELL_OS_Details_ResultingFromITIL.SetVisible(Visible);
		}

		// نمایش فیلد جزئیات itil
		public async Task ITILDetailsVisible(bool Visible)
		{
			Ref_SCMATLASCELL_OS_Details_RequestIdITIL.SetVisible(Visible);
			Ref_SCMATLASCELL_OS_Details_RequestIdITIL.SetDisabled(true);

			Ref_SCMATLASCELL_OS_Details_RequesterUserITIL.SetVisible(Visible);
			Ref_SCMATLASCELL_OS_Details_RequesterUserITIL.SetDisabled(true);

			Ref_SCMATLASCELL_OS_Details_CreatedAtITIL.SetVisible(Visible);
			Ref_SCMATLASCELL_OS_Details_CreatedAtITIL.SetDisabled(true);

			Ref_SCMATLASCELL_OS_Details_ITILDetails.SetVisible(Visible);
		}

		public async Task ITILNull(Entity.SCMATLASCELL_OS_Details Item)
		{
			Item.ResultingFromITIL = null;
			Item.RequestIdITIL = null;
			Item.RequesterUserITIL = null;
			Item.CreatedAtITIL = null;
			Item.ITILDetails = null;
		}
		#endregion

		public async Task<bool> GridSCMATLASCELL_OS_MasterId_872_editmodelsaving(object e)
		{
			var Item = e as Entity.SCMATLASCELL_OS_Details;

			if (Item == null)
			{
				EnsureMsgInitialized();
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
				await ResultingFrom_Contractor(true, true, Item);
			}
			else
			{
				await ResultingFrom_Contractor(false, false, Item);
			}

			if (Item.IsEnableSampleGoods.HasValue && Item.IsEnableSampleGoods.Value)
			{
				Ref_SCMATLASCELL_OS_Details_SCMATLASCELL_OS_Details_SampleGoodsFiles.SetVisible(true);
			}
			else
			{
				Ref_SCMATLASCELL_OS_Details_SCMATLASCELL_OS_Details_SampleGoodsFiles.SetVisible(false);
			}

			await ITILVisible(true);
			// Show ITIL Details
			if (!string.IsNullOrEmpty(Item.ResultingFromITIL))
			{
				await ITILDetailsVisible(true);
			}
			else
			{
				await ITILDetailsVisible(false);
			}
			// نمایش جزئیات ITIL
			//if (!string.IsNullOrEmpty(Item.ResultingFromITIL))
			if (Item.ResultingFromITIL != null)
			{
				await Task.Delay(300);
				Ref_SCMATLASCELL_OS_Details_ITILDetails.SetEntity(Item);
				Ref_SCMATLASCELL_OS_Details_ITILDetails.LoadData();
			}

		}

		public async Task UploadFileIsEnable_oninput(ChangeEventArgs Selected, Entity.SCMATLASCELL_OS_Details Item)
		{
			var uploadFilesVisible = Ref_SCMATLASCELL_OS_Details_SCMATLASCELL_OS_Details_UploadFiles;
			if (Selected.Value.ToString() == "true")
			{
				uploadFilesVisible.SetVisible(true);
			}
			else
			{
				uploadFilesVisible.SetVisible(false);
			}
		}

		public async Task IsEnableSampleGoods_oninput(ChangeEventArgs Selected, Entity.SCMATLASCELL_OS_Details Item)
		{
			var sampleFileVisible = Ref_SCMATLASCELL_OS_Details_SCMATLASCELL_OS_Details_SampleGoodsFiles;
			if (Selected.Value.ToString() == "true")
			{
				sampleFileVisible.SetVisible(true);
			}
			else
			{
				sampleFileVisible.SetVisible(false);
			}
		}

		public async Task ITILCodeIsEnable_oninput(ChangeEventArgs Selected, Entity.SCMATLASCELL_OS_Details Item)
		{
			if (Selected.Value.ToString() == "true")
			{
				await ITILVisible(true);
			}
			else
			{
				await ITILNull(Item);
				await ITILVisible(false);
				await ITILDetailsVisible(false);
			}
		}

		public async Task SetITILDetailsVisible(bool Visible, dynamic? Selected, Entity.SCMATLASCELL_OS_Details Item)
		{
			// await Task.Delay(100);
			// نمایش / عدم نمایش فیلد ITIL Detail

			if (Item.ResultingFromITIL != null)
			{
				await ITILDetailsVisible(Visible);
				if (Visible)
				{
					// await Task.Delay(300);
					Item.RequestIdITIL = Selected.RequestID;
					Item.RequesterUserITIL = Selected.UserName;
					Item.CreatedAtITIL = Selected.CreateDate;
					Ref_SCMATLASCELL_OS_Details_ITILDetails.SetEntity(Item);
					Ref_SCMATLASCELL_OS_Details_ITILDetails.LoadData();
				}
				else
				{
					Item.RequestIdITIL = null;
					Item.RequesterUserITIL = null;
					Item.CreatedAtITIL = null;
					Ref_SCMATLASCELL_OS_Details_ITILDetails.SetEntity(Item);
					Ref_SCMATLASCELL_OS_Details_ITILDetails.LoadData();
				}
			}
		}

		public async Task ResultingFromITIL_onitemselected(dynamic Selected, Entity.SCMATLASCELL_OS_Details Item)
		{
			await SetITILDetailsVisible(true, Selected, Item);
		}

		public async Task SCMATLASCELL_ICT_DepartmentsId_onitemselected(dynamic Selected)
		{
		}

		#endregion FunctionEvents
	}
}
