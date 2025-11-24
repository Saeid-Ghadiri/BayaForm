using Baya.Models.Utility;
using BlazorBootstrap;
using Blazored.Toast.Services;
using DevExpress.Blazor;
using Entity;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Forms.Forms
{
    public class Form_1010Base : Form_1010Peropeties
    {
        public MSG _MSG { get; set; }
        [Inject] public HttpClient HttpClient { get; set; }

        #region Lifecycle Methods

        protected override async Task OnInitializedAsync()
        {
            _Entity.IDMS_RDC_Details ??= new List<Entity.IDMS_RDC_Details>();
            _Entity.IDMS_TestModel ??= new List<Entity.IDMS_TestModel>();
            _Entity.IsResultOfAnotherProcess ??= false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _MSG = new MSG(toastService);

                // در ابتدا هر دو مخفی باشند
                Ref_IDMS_RDC_Master_TrackingCode?.SetVisible(false);
                Ref_IDMS_RDC_AllData?.SetVisible(false);

                _Entity.IDMS_RDC_Details = new List<Entity.IDMS_RDC_Details>();

                await ToggleDetailsGridAddButton();
                StateHasChanged();
            }
            else
            {
                var currentCount = _Entity.IDMS_RDC_Details?.Count(x => x.IsDelete != true) ?? 0;
                if (currentCount != _lastDetailsCount)
                {
                    _lastDetailsCount = currentCount;
                    await ToggleDetailsGridAddButton();
                }
            }
        }

        #endregion

        #region Form Submission Pipeline

        public override async Task<bool> FormValidator()
        {
            if (!HasValidDetailsCount(out _))
            {
                await ShowRequestIncompleteDialog();
                return false;
            }
            return await ValidateAllDetailItems();
        }

        public override async Task<Result> BeforSubmit()
        {
            if (!HasValidDetailsCount())
                return new Result { Status = HttpStatusCode.BadRequest };
            return new Result { Status = HttpStatusCode.OK };
        }

        public override async Task AfterSubmit() { }
        public override async Task BeforGetData() { }
        public override async Task AfterGetData()
        {
            await ToggleDetailsGridAddButton();
        }

        #endregion

        #region FunctionEvents

        private int _lastDetailsCount = 0;

        #region Validation Logic

        private bool HasValidDetailsCount()
        {
            return HasValidDetailsCount(out _);
        }

        private bool HasValidDetailsCount(out string errorMessage)
        {
            var activeCount = _Entity.IDMS_RDC_Details?.Count(x => x.IsDelete != true) ?? 0;
            errorMessage = null;

            if (activeCount == 0)
            {
                errorMessage = "لطفاً حداقل یک ردیف در بخش «جزئیات سیستم تحقیق و توسعه» ثبت کنید.";
                return false;
            }
            if (activeCount > 1)
            {
                errorMessage = "شما مجاز به ثبت بیش از یک ردیف نیستید.";
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateAllDetailItems()
        {
            var errors = new List<string>();
            foreach (var item in _Entity.IDMS_RDC_Details.Where(x => x.IsDelete != true))
            {
                if (item.IDMS_ProductCategoriesId == null)
                    errors.Add("لطفاً دسته‌بندی محصول را انتخاب کنید.");
                if (item.IDMS_ProductsId == null)
                    errors.Add("لطفاً محصول را انتخاب کنید.");
                if (item.IDMS_CustomerId == null)
                    errors.Add("لطفاً مشتری را انتخاب کنید.");
                if (item.IDMS_ResultingFromId == null)
                    errors.Add("لطفاً گزینه «منتج از» را انتخاب کنید.");
            }

            if (errors.Count > 0)
            {
                var message = string.Join("<br/>• ", errors);
                await _MSG.ShowError($"<ul style='text-align:right;'><li>• {message}</li></ul>");
                return false;
            }
            return true;
        }

        #endregion

        #region Grid Events: IDMS_RDC_Details

        public async Task<bool> GridIDMS_RDC_MasterId_741_editmodelsaving(object e) => false;

        public async Task GridIDMS_RDC_MasterId_741_afterrendermodal(Entity.IDMS_RDC_Details item)
        {
            await ToggleDetailsGridAddButton(true);
        }

        public async Task IsResultOfAnotherProcess_oninput(ChangeEventArgs selected)
        {
            var isChecked = selected.Value is bool b ? b :
                   selected.Value?.ToString()?.Equals("true", StringComparison.OrdinalIgnoreCase) == true;

            _Entity.IsResultOfAnotherProcess = isChecked;

            // نمایش هر دو فیلد همزمان
            Ref_IDMS_RDC_Master_TrackingCode?.SetVisible(isChecked);
            Ref_IDMS_RDC_AllData?.SetVisible(isChecked); // ✅ این خط اضافه شد

            if (!isChecked)
            {
                _Entity.IDMS_RDC_Master_TrackingCode = default;
                _Entity.IDMS_RDC_AllData = null;
                _Entity.IDMS_RDC_Details = new List<Entity.IDMS_RDC_Details>();
                StateHasChanged();
            }
        }

        #endregion

        #region Grid Events: IDMS_TestModel

        public async Task<bool> GridIDMS_RDC_MasterId_753_editmodelsaving(object e) => false;

        public async Task GridIDMS_RDC_MasterId_753_afterrendermodal(Entity.IDMS_TestModel item)
        {
        }

        public async Task GridIDMS_RDC_MasterId_753_customizeeditmodel(GridCustomizeEditModelEventArgs e)
        {
            if (e.IsNew)
            {
                var newTestModel = (Entity.IDMS_TestModel)e.EditModel;
                var activeDetail = _Entity.IDMS_RDC_Details?.FirstOrDefault(x => x.IsDelete != true);

                if (activeDetail == null)
                {
                    await _MSG.ShowWarning("ابتدا یک ردیف در بخش «جزئیات سیستم تحقیق و توسعه» ثبت کنید.");
                    await Grid_IDMS_TestModel?.CancelEditAsync();
                    return;
                }

                newTestModel.IDMS_RDC_DetailsId = activeDetail.Id;
                newTestModel.IDMS_RDC_Details = activeDetail;

                if (Ref_IDMS_TestModel_IDMS_RDC_DetailsId != null)
                {
                    Ref_IDMS_TestModel_IDMS_RDC_DetailsId.SetEntity(activeDetail);
                    await Task.Delay(100);
                    await Ref_IDMS_TestModel_IDMS_RDC_DetailsId.LoadData();
                }
                StateHasChanged();
            }
        }

        #endregion

        #region Dropdown Events
        private static Guid? TryParseGuid(string? value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            return Guid.TryParse(value, out Guid result) ? result : null;
        }

        public async Task IDMS_RDC_Master_TrackingCode_onitemselected(Entity.IDMS_RDC_Master Selected)
        {            
        }

        private async Task SetDataInGridWithDropDown(Entity.IDMS_RDC_Master Item)
        {
            Console.WriteLine("#Log 0000:");

            // 1) مقدار GUID را از Item به Dropdown بده
            Ref_IDMS_RDC_AllData.Value = Item.IDMS_RDC_AllData;

            Console.WriteLine(await Utility.JSON.ToJson(Item));

            await Task.Delay(80);

            // 2) مقدار را به LoadData بده (الزامی است)
            await Ref_IDMS_RDC_AllData.LoadData(Item.IDMS_RDC_AllData);

            Console.WriteLine("#Log 1111:");
        }

        public async Task TrackingCodeCheck(Entity.IDMS_RDC_Master Item)
        {
            if (Item.IsResultOfAnotherProcess.HasValue)
            {
                if (Item.IsResultOfAnotherProcess.Value == true)
                {
                    await TrackingCodeSetVisible(true);
                    await SetDataInGridWithDropDown(Item);
                }
            }
        }
        public async Task TrackingCodeSetVisible(bool Visible)
        {
            await Task.Delay(100);
            Ref_IDMS_RDC_Master_TrackingCode.SetVisible(Visible);
            Ref_IDMS_RDC_Master_TrackingCode.SetDisabled(true);

            Ref_IDMS_RDC_AllData.SetVisible(Visible);
            Ref_IDMS_RDC_AllData.SetDisabled(true);
        }

        #endregion

        #region UI Control Helpers

        private async Task ToggleDetailsGridAddButton(bool isInModal = false)
        {
            // بدون دستکاری مستقیم UI — امن‌تر و بدون خطا
        }

        #endregion

        #region Dialogs

        private async Task ShowRequestIncompleteDialog()
        {
            var options = new ConfirmDialogOptions
            {
                YesButtonText = "بازگشت به درخواست",
                YesButtonColor = ButtonColor.Danger,
                NoButtonText = "",
            };

            string html = $@"
                <div>
                    <picture><img src='https://file.workcv.ir/fa/api/v1/File/Get?FileID=6e5b6fb8-a5b2-490c-f83f-08dbea5b8061  ' width='96px' alt='لوگو پل فیلم' /></picture>
                    <hr class='hrdash border-success-subtle'>
                </div>
                <div class='fw-bold text-center'>
                    <span class='fs-5'>کد پیگیری این درخواست: </span>
                    <span class='fs-3' style='color: #1ba156'>{_Entity.RequestTrakingCode}</span>
                    <div>
                        <span><i class='fal fa-exclamation-triangle' style='font-size:24px; color:red;'></i>&nbsp;</span>
                        <span class='fs-6 text-secondary'>تا کنون هیچ ردیف درخواستی تکمیل نشده است. لطفاً برای ثبت و ادامه به مرحله بعد حداقل یک ردیف در درخواست خود ثبت نمایید.</span>
                    </div>
                </div>";

            await Confirm.ShowAsync(title: "", message1: html, confirmDialogOptions: options);
        }

        #endregion

        #region Utility and Helpers
        // برای گسترش آینده
        #endregion

        #endregion FunctionEvents
    }
}