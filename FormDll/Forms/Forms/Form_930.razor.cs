using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using System.Globalization;
using System.Threading.Tasks;
using BlazorBootstrap;
using Blazored.Toast.Services;
namespace Forms.Forms
{
    public class Form_930Base : Form_930Peropeties
    {
      
    


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

        }
    }

    /// <summary>
    /// اعتبار سنجی فرم
    /// </summary>
    /// <returns></returns>
    public override async Task<bool> FormValidator()
    {
       bool IsValid = true;

        
        var List = _Entity.SCM_ProductRequestDetails.Where(p=>p.IsDelete == false).ToList();


            var listCount = List.Count();

            // Console.WriteLine("#Log FormValidator :");

            // دکمه ثبت و ادامه           
            if (BtnWorkFlowId == "SequenceFlow_0pkcu0e")
            {
                // Console.WriteLine("#Log FormValidator btn :");
                foreach (var Item in List)
                {
                    //Console.WriteLine("#Log FormValidator btn foreach :");
                    IsValid = IsValid && await CheckFieldValidation(Item);
                }
            }

           // لغو کلی درخواست
            if (BtnWorkFlowId == "SequenceFlow_0pr6w25")
            {
                // Console.WriteLine("#Log:: 0");

                string htmlString =
                    "<div>" +
                        "<picture>" +
                            "<img src='https://File.workcv.ir/fa/api/v1/File/Get?FileID=6e5b6fb8-a5b2-490c-f83f-08dbea5b8061' class='' alt='لوگو پل‌فیلم' width='96px'>" +
                        "</picture>" +
                        "<hr class='hrdash border-success-subtle'>" +
                    "</div>" +
                    "<div class='fw-bold text-right'>" +
                        "<div class='fs-6'>کاربر لغو کننده درخواست: " + _User.NAME + " " + _User.FAMILY + "</div>" +
                        // "<div class='fs-6'>" + _Entity.CancellationAt + "</div>" +
                        "<div class='fs-6'>دلیل لغو این درخواست:</div>" +
                        "<textarea required id='InConfirmCancelationText' name='InConfirmCancelationText' " +
                            "style='padding: 8px; border: 1px solid #ddd; " +
                            "border-radius: 5px; resize: none; display: block; margin: 5px 0' " +
                            "width: 100%; height: 150px;' " +
                            "placeholder='دلیل لغو درخواست را وارد کنید...' " +
                            "oninput='document.getElementById(\"ConfirmCancelationText\").value=this.value'>" +
                        "</textarea>" +
                        "<div></div>" +
                        "<div class='fs-6 text-secondary text-right'>" +
                        "<i class='fal fa-exclamation-triangle px-2' style='font-size:24px; color:red'></i>" +
                            "کاربر محترم در نظر داشته باشید اطلاعات ثبت در این ناحیه در حال حاضر صرفاً در گزارش‌ها واحد سیستم‌ها و روش‌ها قابل نمایش است، همچنین کاربر درخواست‌دهنده از بخش درخواست‌های من می‌تواند وضعیت درخواست خود را بررسی نماید." +
                        "</div>" +
                    "</div>";

                var options = new ConfirmDialogOptions
                {
                    YesButtonText = "لغو درخواست",
                    YesButtonColor = ButtonColor.Success,
                    NoButtonText = "انصراف",
                    NoButtonColor = ButtonColor.Danger
                };

                var confirmation = await Confirm.ShowAsync(
                    title: "",
                    message1: htmlString,
                    confirmDialogOptions: options);

                if (!confirmation)
                {
                    IsValid = false;
                }
                else
                {
                    // کاربر لغو کننده
                    _Entity.CancelledBy = _User.UserID.ToString();
                    // در بخش Razor فیلد input hidden همین فیلد وجود دارد و از طریق آن داده به فیلد اصلی داده می شود.
                    string Value = await JS.InvokeAsync<string>("eval", "document.getElementById('ConfirmCancelationText')?.value || ''");
                    _Entity.CancellationReason = Value;
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


        /// <summary>
        /// بررسی نوع درخواست انتخابی توسط انباردار
        ///
        /// Id										    Title
        /// *********************************************************
        /// a9c5df1c-d99c-ef11-8354-005056a02a64		تحویل کالا
        /// 3b9e934d-d99c-ef11-8354-005056a02a64		خرید کالا
        /// 73f6c459-d99c-ef11-8354-005056a02a64		تحویل و خرید کالا
        /// 
        /// بررسی ضرورت تکمیل فیلدها از این تابع انجام می شود.
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public async Task<bool> CheckFieldValidation(Entity.SCM_ProductRequestDetails Item)
        {
            bool IsValid = true;

            // توضیحات تایید استعلام مدیر کارخانه
            if(Item.ConfirmationOfTheInquiryFactoryManager == null)
            {
				IsValid = false;
				toastService.ShowError("لطفا گزینه توضیحات تایید استعلام مدیر کارخانه را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
            }
            return IsValid;

        }
    public async Task <bool> GridSCM_ProductRequestId_182_editmodelsaving(object e   )
        {
             bool IsCancelled = false;

        var Item = (Entity.SCM_ProductRequestDetails)e;
        IsCancelled = !await CheckFieldValidation(Item);

    
        return IsCancelled;
        }
public async Task  GridSCM_ProductRequestId_182_afterrendermodal(Entity.SCM_ProductRequestDetails Item   )
        {
            // استعلام تدراکات 1- SCM_ProductRequestDetails_InquiryFirst
        // استعلام تدارکات 2- SCM_ProductRequestDetails_InquirySecondFile
        // استعلام تدارکات 3- SCM_ProductRequestDetails_InquiryThirdFile
        var Inq1IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryFirst;
        var Inq2IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquirySecondFile;
        var Inq3IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryThirdFile;
        // نمایش / عدم نمایش فیلد منتج از ITIL
    	if (Item.InquiryTrueFalse.HasValue && Item.InquiryTrueFalse.Value)
		{
    	    Inq1IsVisible.SetVisible(true);
    	    Inq2IsVisible.SetVisible(true);
    	    Inq3IsVisible.SetVisible(true);
		}
		else
		{
    	    Inq1IsVisible.SetVisible(false);
    	    Inq2IsVisible.SetVisible(false);
    	    Inq3IsVisible.SetVisible(false);
		}  
        }
public async Task  InquiryTrueFalse_oninput(ChangeEventArgs Selected ,Entity.SCM_ProductRequestDetails Item  )
        {
             // استعلام تدراکات 1- SCM_ProductRequestDetails_InquiryFirst
        // استعلام تدارکات 2- SCM_ProductRequestDetails_InquirySecondFile
        // استعلام تدارکات 3- SCM_ProductRequestDetails_InquiryThirdFile
        var Inq1IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryFirst;
        var Inq2IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquirySecondFile;
        var Inq3IsVisible = Ref_SCM_ProductRequestDetails_SCM_ProductRequestDetails_InquiryThirdFile;
        // نمایش / عدم نمایش فیلد منتج از ITIL
    	if (Selected.Value.ToString() == "true")
		{
    	    Inq1IsVisible.SetVisible(true);
    	    Inq2IsVisible.SetVisible(true);
    	    Inq3IsVisible.SetVisible(true);
		}
		else
		{
    	    Inq1IsVisible.SetVisible(false);
    	    Inq2IsVisible.SetVisible(false);
    	    Inq3IsVisible.SetVisible(false);
		}
        }
		#endregion FunctionEvents

}
}
