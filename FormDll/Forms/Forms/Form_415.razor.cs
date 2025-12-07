using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using BlazorBootstrap;
using Blazored.Toast.Services;


namespace Forms.Forms
{
    public class Form_415Base : Form_415Peropeties
    {
        // Toast  
		[Inject]
	    public IToastService toastService { get; set; }

      	/// <summary>
		/// کد تحویل رندوم 
		/// </summary>
		/// <returns></returns>
		public int RandomDeliveryCode { get; set; }
    


    /// <summary>
    /// آماده سازی فرم
    /// </summary>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
	// کد رندوم تحویل کالا - شش رقمی
			RandomDeliveryCode = new Random().Next(100000, 999999);
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

        var List = _Entity.SCMICT_ProductRequestDetails.ToList();
        
        int ListCount = List.Count();

        if(ListCount==0)
        {
            IsValid = false;
            
            var options = new ConfirmDialogOptions
			{
				YesButtonText = "بازگشت به درخواست",
				YesButtonColor = ButtonColor.Danger,
				NoButtonText = "",
			};

			string htmlString = 
            "<div>"+
                "<picture>"+
                    "<img src='https://file.workcv.ir/fa/api/v1/File/Get?FileID=a18190d9-4b1c-45e3-db13-08dc25546b46' class='' alt='لوگو پتکو' width='96px;'>"+
                "</picture>"+
                "<hr class='hrdash border-success-subtle'>"+
            "</div>"+
            "<div class='fw-bold text-center'>" + 
            "<span class='fs-5'>کد پیگیری این درخواست: </span>" + 
			"<span class='fs-3' style='color: #1ba156'>" + _Entity.RequestTrakingCode + "</span><div>"+
            "<span><i class='fal fa-exclamation-triangle' style='font-size:24px; color:red;'></i>&nbsp;</span>" +
			"<span class='fs-6 text-secondary text-right'>تا کنون هیچ ردیف درخواستی تکمیل نشده است. لطفا برای ثبت و ادامه به مرحله بعد حداقل یک ردیف در درخواست خود ثبت نمایید.<span></div>" +
            "</div>"; 

			var confirmation = await Confirm.ShowAsync(
				title: "",
				message1: htmlString,
				confirmDialogOptions: options);
        }

        foreach (var Item in List)
        {           
            // Global_SCMRequestTypeId - مشخص کردن وضعیت یک درخواست زنجیره تامین 
            if (Item.Global_SCMRequestTypeId == null)
			{
				IsValid = false;
				toastService.ShowError("یک نوع درخواست انتخاب کنید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}

            // ITIL
        }

        // Note: انتخاب مدل سخت افزار و یا شبکه
		if (_Entity.HardwareORNetwork == null)
		{
            IsValid = false;
			toastService.ShowError("لطفا گزینه سخت افزار یا شبکه را تکمیل نمایید.",
				settings =>
				{
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
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
   
			#region DeliveryCode Confirm
			// نمایش کد تحویل کالا در زمان ثبت درخواست
			var options = new ConfirmDialogOptions
			{
				YesButtonText = "ادامه فرآیند",
				YesButtonColor = ButtonColor.Success,
				NoButtonText = "",
			};

			string htmlString = 
            "<div class='text-center'>" + 
            "<span>کد پیگیری درخواست: </span>" + 
			"<span class='fs-4 fw-bold'>" + _Entity.RequestTrakingCode + "</span><hr class='hrdash'><div>"+
			"<div><span class='fw-normal'>کد تحویل کالا: </span>" + 
			"<span class='fs-3 text-green fw-bold'>" + RandomDeliveryCode + "</span>" +
            "<span class='btn btn-yellow-light btn-sm mx-2 mb-2' onclick=\"navigator.clipboard.writeText('" + RandomDeliveryCode + "'); $('#copylable').removeClass('d-none');\"><i class='fal fa-copy'></i><span class='px-1'>کپی</span></span></div>" +
            "<p id='copylable' class='d-none text-green mt-3'>کد تحویل کالا کپی شد.</p>" +  
            "<div><hr class='hrdash'><span class='fal fa-exclamation-circle text-red btnicon mx-1' style='font-size: 24px;'></span>"+
			"<span>لطفا در زمان دریافت کالا از انبار، کد فوق را به انباردار اعلام نمایید.</span>" +
            "</div>";

			var confirmation = await Confirm.ShowAsync(
				title: "",
				message1: htmlString,
				confirmDialogOptions: options);

			// if (confirmation)
			// {		
			// 	toastService.ShowSuccess("لطفا کد تحویل را بخاطر بسپارید!!",
			// 		settings => {
			// 			settings.Timeout = 4;
			// 			settings.ShowProgressBar = true;
			// 			settings.PauseProgressOnHover = true;
			// 		});
			// }
			// else
			// {
			// 	toastService.ShowError("*-*-*-*-*-*-*-*-*-*");
			// }
			#endregion / DeliveryCode Confirm
			
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

    public async Task petcoProductSearch_NotMapped_onitemselected(dynamic Selected ,Entity.SCMICT_ProductRequestDetails Item  )
    {
                /// <summary>
        /// فیلدهای زیر فیلدهای اصلی برای نمایش در فرم هستند.
        ///
        ///</summary>
        Console.WriteLine("start");
        //  نام کالا
        Item.SH_DESC = Selected.DESC;
        // کد کالا شماران
        Item.SH_PARTNO = Selected.PARTNO;
        // شناسه سیستمی کد کالا شماران
        Item.SH_PARTNO_GUID = Selected.PARTNO_GUID;
        // شماره کالا شماران
        Item.SH_PARTCODE = Selected.PARTCODE;
        // شناسه شماره کالا شماران
        Item.SH_PARTCODE_GUID = Selected.PARTCODE_GUID;
        // نام کالا شماران
        Item.SH_DESC = Selected.DESC;
        // واحد کالا شماران
        Item.SH_UNIT = Selected.UNIT;
        // کد دسته بندی فرعی کالا شماران
        Item.SH_SUBGRCODE = Selected.SUBGRCODE;
        // شناسه کد دسته بندی فرعی کالا شماران
        Item.SH_SUBGRCODE_GUID = Selected.SUBGRCODE_GUID;
        // کد دسته بندی اصلی شماران
        Item.SH_GRCODE = Selected.GRCODE;
        // شناسه کد دسته بندی اصلی کالا شماران
        Item.SH_GRCODE_GUID = Selected.GRCODE_GUID;
        // نام دسته بندی اصلی کالا شماران
        Item.SH_GroupName = Selected.GroupName;
        // نام دسته بندی فرعی کالا شماران
        Item.SH_SubGroupName = Selected.SubGroupName;
        // سال مالی کالا شماران
        Item.SH_YEAR = Selected.YEAR;
        // کالا موجود است یا خیر شماران
        Item.SH_IsExist = Selected.IsExist;
        // نام شرکت کالا شماران
        Item.SH_Factory = Selected.Factory;
        // کد گروه اصلی کالا شماران
        Item.SH_MapGroupCode = Selected.MapGroupCode;

        // موجودی کالا در شماران            
        if (Selected.Amount > -1)
        {
            Item.SH_Amount = (double)Selected.Amount;
        }
    }

		public async Task<bool> SCMICT_ProductRequestDetails_editmodelsaving(object e   )
        {
            bool IsValid = false;
			var MainModel = (Entity.SCMICT_ProductRequestDetails)e;

			// پرکردن فیلد کد رهگیری
			// Console.WriteLine("DeliveryCode:");
			// Console.WriteLine(DeliveryCode+"");
			if(!MainModel.DeliveryCode.HasValue)
            {
                MainModel.DeliveryCode = RandomDeliveryCode;
            }

			// شرط پُر بودن فیلد نام کالا دیزیبل
			if (MainModel.SH_DESC == null)
			{
				IsValid = true;
				toastService.ShowError("لطفا گزینه نام کالا را تکمیل نمایید",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}
			// شرط پُر بودن فیلد تعداد یا مقدار درخواستی
			if (MainModel.ProductRequestingQTY == null || MainModel.ProductRequestingQTY == 0)
			{
				IsValid = true;
				toastService.ShowError("لطفا گزینه تعداد یا مقدار درخواستی را تکمیل نمایید.",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}
			// شرط پُر بودن فیلد اولویت
			if (MainModel.SCMICT_PriorityId == null)
			{
				IsValid = true;
				toastService.ShowError("لطفا گزینه اولویت را تکمیل نمایید.",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}
			// شرط پُر بودن فیلد محل مصرف
			if (MainModel.PlaceOfUse == null)
			{
				IsValid = true;
				toastService.ShowError("لطفا گزینه محل مصرف تکمیل نمایید.",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}

            // Global_SCMRequestTypeId - مشخص کردن وضعیت یک درخواست زنجیره تامین 
            if (MainModel.Global_SCMRequestTypeId == null)
			{
				IsValid = false;
				toastService.ShowError("یک نوع درخواست انتخاب کنید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}

            // ITILCodeIsEnable
			if (MainModel.ITILCodeIsEnable == null)
			{
				IsValid = true;
				toastService.ShowError("لطفا گزینه آیا کد ITIL دارد؟ را تکمیل نمایید.",
					settings =>
					{
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}
            
            if (MainModel.ITILCodeIsEnable != null && MainModel.ITILCodeIsEnable == true)
			{
				// Note: تعداد تامین کسری
				if (MainModel.ResultingFromITIL == null)
				{
					IsValid = true;
					toastService.ShowError("لطفا کد ITIL را تکمیل نمایید.",
						settings =>
						{
							settings.Timeout = 4;
							settings.ShowProgressBar = true;
							settings.PauseProgressOnHover = true;
						});
				}
			}
			return IsValid;
        }

		public async Task SCMICT_ProductRequestDetails_customizeeditmodel(GridCustomizeEditModelEventArgs e   )
        {
            // var Item = (Entity.SCMICT_ProductRequestDetails)e.EditModel;
			// if (e.IsNew)
			// {
            //     // آیا تامین کسری انجام شود؟
            //     Item.DeficitSupplyIsEnable = true;
            //     // آیا فرآیند تحویل اجرا گردد؟
            //     Item.GoodsDeliveryIsEnable = true;
            //     // آیا ITIL دارد
            //     Item.ITILCodeIsEnable = null;
            //     // // نوع درخواست سخت افزار - شبکه
            //     // _Entity.HardwareORNetwork = null;
			// }
            // StateHasChanged();
        }
public async Task SCMICT_ProductRequestDetails_afterrendermodal(Entity.SCMICT_ProductRequestDetails Item   )
        {
			if(!Item.ITILCodeIsEnable.HasValue){
                Item.ITILCodeIsEnable = true; 
            }
			
            // Note: مخفی کردن فیلد تعداد تامین کسری بر اساس فیلد آیا تامین کسری دارد؟
			if (Item.DeficitSupplyIsEnable.HasValue && Item.DeficitSupplyIsEnable.Value)
			{
				Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true);
			}
			else
			{
				Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);
			}

            // Note: itil
			if (Item.ITILCodeIsEnable.HasValue && Item.ITILCodeIsEnable.Value)
			{
				Ref_SCMICT_ProductRequestDetails_ResultingFromITIL.SetVisible(true);
			}
			else
			{
				Ref_SCMICT_ProductRequestDetails_ResultingFromITIL.SetVisible(false);
			}

			/// <summery>
			/// Note: تعیین وضعیت درخواست ها
			/// </summery>
			// در حالت نال هر دو فقط مخفی باشد
            if (Item.Global_SCMRequestTypeId == null)
            {
                Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(false);
                Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(false);
                Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);  
            }
            else
            {
                // شرط اینکه آیا فرآیند تحویل اجرا گردد؟
                if(Item.Global_SCMRequestTypeId.ToString() =="a9c5df1c-d99c-ef11-8354-005056a02a64")
                {
                    Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(true);
                    Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(false);
                }

                // شرط نوع درخواست بر اساس خرید کالا
                if(Item.Global_SCMRequestTypeId.ToString() =="3b9e934d-d99c-ef11-8354-005056a02a64")
                {
                    Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(false);
                    Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(true);
                }

                // شرط نوع درخواست بر اساس تحویل و خرید کالا
                if(Item.Global_SCMRequestTypeId.ToString() =="73f6c459-d99c-ef11-8354-005056a02a64")
                {
                    Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(true);
                    Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(true);
                }
            }
        }

		public async Task ITILCodeIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCMICT_ProductRequestDetails Item  )
        {
            // Note: مخفی کردن فیلد کد آی تی آی ال بر اساس فیلد آیا کد دارد؟       
			var Item1 = Ref_SCMICT_ProductRequestDetails_ResultingFromITIL;
			if (Selected.Value.ToString() == "true")
			{
				Item1.SetVisible(true);
			}
			else
			{
				Item1.SetVisible(false);
			}
        }

		public async Task DeficitSupplyIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCMICT_ProductRequestDetails Item  )
        {
            // Note: مخفی کردن فیلد تعداد تامین کسری بر اساس فیلد آیا تامین کسری دارد؟       
			var Item1 = Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber;        
			if (Selected.Value.ToString() == "true")
			{
				Item1.SetVisible(true);
			}
			else
			{
				Item1.SetVisible(false);
			}
        }

		public async Task Global_SCMRequestTypeId_onitemselected(dynamic Selected ,Entity.SCMICT_ProductRequestDetails Item  )
        {
            //Console.WriteLine(await Utility.JSON.ToJson(Selected));

            // شرط نوع درخواست بر اساس تحویل کالا
            if(Selected.Id.ToString() =="a9c5df1c-d99c-ef11-8354-005056a02a64")
            {
                // تحویل کالا
                Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(true);
                Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.Value =true;
                // خرید کالا
                Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(false);
                Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.Value=false;
                Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);               
            }
         
            // شرط نوع درخواست بر اساس خرید کالا
            if(Selected.Id.ToString() =="3b9e934d-d99c-ef11-8354-005056a02a64")
            {
                // تحویل کالا
                Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(false);
                Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.Value =false;
                // خرید کالا
                Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(true);
                Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.Value=true;
                if (Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.Value.HasValue && Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.Value.Value)
                {
                    Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true);  
                }
                else
                {
                    Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);  
                }
            }
         
            // شرط نوع درخواست بر اساس تحویل و خرید کالا
            if(Selected.Id.ToString() =="73f6c459-d99c-ef11-8354-005056a02a64")
            {
                // تحویل کالا
                Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(true);    
                Ref_SCMICT_ProductRequestDetails_GoodsDeliveryIsEnable.Value =true;
                // خرید کالا               
                Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(true);
                Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.Value=true;
                if (Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.Value.HasValue && Ref_SCMICT_ProductRequestDetails_DeficitSupplyIsEnable.Value.Value)
                {
                    Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber.SetVisible(true);  
                }
                else
                {
                    Ref_SCMICT_ProductRequestDetails_DeficitSupplyNumber.SetVisible(false);  
                }
            }
        }

		#endregion FunctionEvents

}
}
