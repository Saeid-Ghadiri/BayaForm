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
    public class Form_596Base : Form_596Peropeties
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
			//شش رقمی
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


		var List = _Entity.SCMPLATE_ProductRequestDetails.ToList();
                            
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
                    "<img src='https://file.workcv.ir/fa/api/v1/File/Get?FileID=66a3abd8-12c1-4281-d6ed-08dcdfbc856d' class='' alt='لوگو پلات' width='96px;'>"+
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
                // نام کالا - SH_DESC
                if (Item.SH_DESC == null)
			    {
			    	IsValid = false;

			    	toastService.ShowError("لطفا گزینه نام کالا را تکمیل نمایید.",
			    	settings => {
			    		settings.Timeout = 4;
			    		settings.ShowProgressBar = true;
			    		settings.PauseProgressOnHover = true;
			    	});
			    }

                // ProductRequestingQTY - تعداد یا مقدار درخواستی
                if (Item.ProductRequestingQTY == null || Item.ProductRequestingQTY == 0)
			    {
			    	IsValid = false;

			    	toastService.ShowError("لطفا گزینه تعداد یا مقدار درخواستی را تکمیل نمایید.",
			    	settings => {
			    		settings.Timeout = 4;
			    		settings.ShowProgressBar = true;
			    		settings.PauseProgressOnHover = true;
			    	});
			    }

                // فیلد اولویت
                if (Item.Global_PriorityId == null || Item.Global_PriorityId == Guid.Empty)
			    {
			    	IsValid = false;

			    	toastService.ShowError("لطفا گزینه اولویت را تکمیل نمایید.",
			    	settings => {
			    		settings.Timeout = 4;
			    		settings.ShowProgressBar = true;
			    		settings.PauseProgressOnHover = true;
			    	});
			    }

                // فیلد واحد کالا - SH_UNIT
                if (Item.SH_UNIT == null)
			    {
			    	IsValid = false;

			    	toastService.ShowError("لطفا گزینه واحد کالا را تکمیل نمایید.",
			    	settings => {
			    		settings.Timeout = 4;
			    		settings.ShowProgressBar = true;
			    		settings.PauseProgressOnHover = true;
			    	});
			    }

			    // PlaceOfUse - محل مصرف کالا
        	    if(Item.PlaceOfUse == null)
        	    {
        	        IsValid = false;
    
			    	toastService.ShowError("لطفا گزینه محل مصرف کالا را تکمیل نمایید.",
			    	settings => {
			    		settings.Timeout = 4;
			    		settings.ShowProgressBar = true;
			    		settings.PauseProgressOnHover = true;
			    	});
        	    }

			    // Global_SCMRequestTypeId - نوع درخواست
                if(Item.Global_SCMRequestTypeId == null)
        	    {
        	        IsValid = false;
    
			    	toastService.ShowError("لطفا گزینه نوع درخواست را انتخاب نمایید.",
			    	settings => {
			    		settings.Timeout = 4;
			    		settings.ShowProgressBar = true;
			    		settings.PauseProgressOnHover = true;
			    	});
        	    }
                // //Iآیا کد ITIL دارد؟ 
                // if (Item.ITILCodeIsEnable == null)
                // {
                //     IsValid = false;
                //     toastService.ShowError("لطفا گزینه آیا ITIL  دارد؟ را تکمیل نمایید.",
                //     settings => {
                //         settings.Timeout = 4;
                //         settings.ShowProgressBar = true;
                //         settings.PauseProgressOnHover = true;
                //     });
                // }

                // // منتج از ITIL
                // if(Item.ITILCodeIsEnable.HasValue && Item.ITILCodeIsEnable.Value)
                // {                
                //     if (Item.ITILCodeIsEnable == null)
                //     {
                //         IsValid = false;

                //         toastService.ShowError("لطفا گزینه  منتج از ITIL را تکمیل نمایید.",
                //         settings => {
                //             settings.Timeout = 4;
                //             settings.ShowProgressBar = true;
                //             settings.PauseProgressOnHover = true;
                //         });
                //     }
                // }
					// منتج از ITIL
				   if (Item.ResultingFromITIL == null)
                    {
                        IsValid = false;

                        toastService.ShowError("لطفا گزینه  منتج از ITIL را تکمیل نمایید.",
                        settings => {
                            settings.Timeout = 4;
                            settings.ShowProgressBar = true;
                            settings.PauseProgressOnHover = true;
                        });
                    }

                // در صورتی که خرید یا تحویل و خرید باشد گزینه تعداد تامین کسری ضروری گردد.
			    if(Item.Global_SCMRequestTypeId.ToString() =="73f6c459-d99c-ef11-8354-005056a02a64" ||
			      Item.Global_SCMRequestTypeId.ToString() =="3b9e934d-d99c-ef11-8354-005056a02a64")
			    {
			    	if (Item.DeficitSupplyNumber == null)
			    	{
			    		IsValid = false;
			    		toastService.ShowError("لطفا گزینه تعداد تامین کسری ضروری گردد.",
		        		settings =>
		        		{
		        			settings.Timeout = 4;
		        			settings.ShowProgressBar = true;
		        			settings.PauseProgressOnHover = true;
		        		});
			    	}
			    }

				// در صورتی که خرید یا تحویل و خرید باشد گزینه حدود هزینه خرید ضروری گردد.
				if(Item.Global_SCMRequestTypeId.ToString() =="73f6c459-d99c-ef11-8354-005056a02a64" ||
				  Item.Global_SCMRequestTypeId.ToString() =="3b9e934d-d99c-ef11-8354-005056a02a64")
				{
					if (Item.PurchasePrice == null)
					{
						IsValid = false;
						toastService.ShowError("لطفا گزینه حدود قیمت کالا تکمیل گردد.",
		    			settings =>
		    			{
		    				settings.Timeout = 4;
		    				settings.ShowProgressBar = true;
		    				settings.PauseProgressOnHover = true;
		    			});
					}
				}

        	}

            // SCMPLATE_ICT_DepartmentsId - بخش‌های فناوری اطلاعات
            if(_Entity.SCMPLATE_ICT_DepartmentsId == null)
            {
                IsValid = false;
                
		    	toastService.ShowError("لطفا گزینه بخش‌های فناوری اطلاعات را انتخاب نمایید.",
		    	settings => {
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
        "<span class='btn btn-yellow-light btn-sm mx-2 mb-2' onclick=\"navigator.clipboard.writeText('" + RandomDeliveryCode + "'); $('#copylable').removeClass('d-none')\"><i class='fal fa-copy'></i><span class='px-1'>کپی</span></span></div>" +
        "<p id='copylable' class='d-none text-green mt-3'>کد تحویل کالا کپی شد.</p>" +  
        "<div><hr class='hrdash'><span class='fal fa-exclamation-circle text-red btnicon mx-1' style='font-size: 24px;'></span>"+
		"<span>لطفا در زمان دریافت کالا از انبار، کد فوق را به انباردار اعلام نمایید.</span>" +
        "</div>";
		var confirmation = await Confirm.ShowAsync(
			title: "",
			message1: htmlString,
			confirmDialogOptions: options);

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


    public async Task ITILVisible(bool Visible)
    {
        Ref_SCMPLATE_ProductRequestDetails_ResultingFromITIL.SetVisible(Visible);
    }
	public async Task ITILDetailsVisible(bool Visible)
    {
        Ref_SCMPLATE_ProductRequestDetails_RequestIdITIL.SetVisible(Visible);
        Ref_SCMPLATE_ProductRequestDetails_RequestIdITIL.SetDisabled(true);
        Ref_SCMPLATE_ProductRequestDetails_RequesterUserITIL.SetVisible(Visible);
        Ref_SCMPLATE_ProductRequestDetails_RequesterUserITIL.SetDisabled(true);

        Ref_SCMPLATE_ProductRequestDetails_CreatedAtITIL.SetVisible(Visible);
        Ref_SCMPLATE_ProductRequestDetails_CreatedAtITIL.SetDisabled(true);

        Ref_SCMPLATE_ProductRequestDetails_ITILDetails.SetVisible(Visible);
    }
	public async Task TahvilIsVisible(bool Visible, bool Value, Entity.SCMPLATE_ProductRequestDetails Item)
	{
		// تحویل
		Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible(Visible);
		Item.GoodsDeliveryIsEnable = Value;
	}
	
	public async Task KharidIsVisible(bool Visible, bool Value, Entity.SCMPLATE_ProductRequestDetails Item)
	{
		// خرید
		Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(Visible);
		Item.DeficitSupplyIsEnable = Value;
		// تعداد تامین کسری	
		Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyNumber.SetVisible(Visible);
		// مبلغ حدودی خرید
		Ref_SCMPLATE_ProductRequestDetails_PurchasePrice.SetVisible(Visible);
		//Item.PurchasePrice = Value;
	}
	
	public async Task TahvilKharidIsVisible(bool Visible, bool Value, Entity.SCMPLATE_ProductRequestDetails Item)
	{
		// تحویل
		Ref_SCMPLATE_ProductRequestDetails_GoodsDeliveryIsEnable.SetVisible (Visible);
		Item.GoodsDeliveryIsEnable = Value;
		// خرید
		Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyIsEnable.SetVisible(Visible);
		Item.DeficitSupplyIsEnable = Value;
		// تعداد تامین کسری
		Ref_SCMPLATE_ProductRequestDetails_DeficitSupplyNumber.SetVisible (Visible);
		// مبلغ حدودی خرید
		Ref_SCMPLATE_ProductRequestDetails_PurchasePrice.SetVisible(Visible);
		//Item.PurchasePrice = Value;
	}

    public async Task ProductSearch_NotMapped_onitemselected(dynamic Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
    {
        /// <summary>
        /// فیلدهای زیر فیلدهای اصلی برای نمایش در فرم هستند.
        ///
        ///</summary>

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

		public async Task<bool> SCMPLATE_ProductRequestDetails_editmodelsaving(object e   )
        {
			bool IsCancelled = false;

			// Item = Detail Model
			var Item = (Entity.SCMPLATE_ProductRequestDetails)e;

			// پرکردن فیلد کد رهگیری
			// Console.WriteLine("DeliveryCode:");
			// Console.WriteLine(DeliveryCode+"");
			if(!Item.DeliveryCode.HasValue)
            {
                Item.DeliveryCode = RandomDeliveryCode;
            }

            // شرط پُر بودن فیلد نام کالا دیزیبل
			if (Item.SH_DESC == null)
			{
				IsCancelled = true;

				toastService.ShowError("لطفا گزینه نام کالا را تکمیل نمایید",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}

			// شرط نوع درخواست بر اساس تحویل و خرید کالا
            if(Item.Global_SCMRequestTypeId.HasValue && Item.Global_SCMRequestTypeId.Value.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64")
            {
				// در صورتی که موجودی کالا 0 باشد و یا نال باشد خطا نمایش داده شود.
                if(!Item.SH_Amount.HasValue || Item.SH_Amount==0)
				{
					IsCancelled = true;

					toastService.ShowError("به دلیل نداشتن موجودی کالا، امکان تحویل آن وجود ندارد.",
						settings =>
						{
							settings.Timeout = 4;
							settings.ShowProgressBar = true;
							settings.PauseProgressOnHover = true;
						});
				}
            }
			

			// شرط پُر بودن فیلد تعداد یا مقدار درخواستی
			if (Item.ProductRequestingQTY == null || Item.ProductRequestingQTY == 0)
			{
				IsCancelled = true;

				toastService.ShowError("لطفا گزینه تعداد یا مقدار درخواستی را تکمیل نمایید.",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}

			// شرط پُر بودن فیلد اولویت
			if (Item.Global_PriorityId == null || Item.Global_PriorityId == Guid.Empty)
			{
				IsCancelled = true;

				toastService.ShowError("لطفا گزینه اولویت را تکمیل نمایید.",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}

			// شرط پُر بودن فیلد محل مصرف
			if (Item.PlaceOfUse == null)
			{
				IsCancelled = true;

				toastService.ShowError("لطفا گزینه محل مصرف تکمیل نمایید.",
					settings => {
						settings.Timeout = 4;
						settings.ShowProgressBar = true;
						settings.PauseProgressOnHover = true;
					});
			}

            // در صورتی که خرید یا تحویل و خرید باشد گزینه تعداد تامین کسری ضروری گردد.
			if(Item.Global_SCMRequestTypeId.ToString() =="73f6c459-d99c-ef11-8354-005056a02a64" ||
			  Item.Global_SCMRequestTypeId.ToString() =="3b9e934d-d99c-ef11-8354-005056a02a64")
			{
				if (Item.DeficitSupplyNumber == null)
				{
					IsCancelled = true;

					toastService.ShowError("لطفا گزینه تعداد تامین کسری ضروری گردد.",
		    		settings =>
		    		{
		    			settings.Timeout = 4;
		    			settings.ShowProgressBar = true;
		    			settings.PauseProgressOnHover = true;
		    		});
				}
			}

			// در صورتی که خرید یا تحویل و خرید باشد گزینه حدود هزینه خرید ضروری گردد.
			if(Item.Global_SCMRequestTypeId.ToString() =="73f6c459-d99c-ef11-8354-005056a02a64" ||
			  Item.Global_SCMRequestTypeId.ToString() =="3b9e934d-d99c-ef11-8354-005056a02a64")
			{
				if (Item.PurchasePrice == null)
				{
					IsCancelled = true;
					
					toastService.ShowError("لطفا گزینه حدود قیمت کالا تکمیل گردد.",
		    		settings =>
		    		{
		    			settings.Timeout = 4;
		    			settings.ShowProgressBar = true;
		    			settings.PauseProgressOnHover = true;
		    		});
				}
			}
           
			return IsCancelled;
        }
        
        public async Task SCMPLATE_ProductRequestDetails_afterrendermodal(Entity.SCMPLATE_ProductRequestDetails Item   )
        {
            // فیلد نوع درخواست
            if (Item.Global_SCMRequestTypeId == null)
            {
                await TahvilKharidIsVisible(false,false,Item);
            }
			else
			{
				// شرط اینکه آیا فرآیند تحویل اجرا گردد؟
            	if(Item.Global_SCMRequestTypeId.ToString() =="a9c5df1c-d99c-ef11-8354-005056a02a64")
            	{
            	    await TahvilIsVisible(true,true,Item);
					await KharidIsVisible(false,false,Item);
            	}
	
            	// شرط نوع درخواست بر اساس خرید کالا
            	if(Item.Global_SCMRequestTypeId.ToString() =="3b9e934d-d99c-ef11-8354-005056a02a64")
            	{
            	    await TahvilIsVisible(false,false,Item);
					await KharidIsVisible(true,true,Item);
            	}
	
            	// شرط نوع درخواست بر اساس تحویل و خرید کالا
            	if(Item.Global_SCMRequestTypeId.ToString() =="73f6c459-d99c-ef11-8354-005056a02a64")
            	{
            	    await TahvilKharidIsVisible(true,true,Item);
            	}
			}


			// // Show ITIL
        	// if (Item.ITILCodeIsEnable.HasValue && Item.ITILCodeIsEnable.Value)
			// {
        	//     await ITILVisible(true); 
			// }
			// else
			// {
        	//     await ITILVisible(false);
			// }

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
        	if (Item.ResultingFromITIL != null)
			{
     			Ref_SCMPLATE_ProductRequestDetails_ITILDetails.SetEntity(Item);
				Ref_SCMPLATE_ProductRequestDetails_ITILDetails.LoadData();
 			}
        }

		// public async Task ITILCodeIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
        // {
        //     // نمایش / عدم نمایش فیلد منتج از ITIL
        // 	if (Selected.Value.ToString() == "true")
	    // 	{
        // 	    await ITILVisible(true);
	    // 	}
	    // 	else
	    // 	{
        // 	    await ITILVisible(false); 
	    // 	} 
        // }

        public async Task  ResultingFromITIL_onitemselected(dynamic Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
        {
            // نمایش / عدم نمایش فیلد ITIL Detail
        	await ITILDetailsVisible(true);

        	if (Item.ResultingFromITIL != null)
	    	{
	    		Item.RequestIdITIL = Selected.RequestID;
	    		Item.RequesterUserITIL = Selected.UserName;
	    		Item.CreatedAtITIL = Selected.CreateDate;

				Ref_SCMPLATE_ProductRequestDetails_ITILDetails.SetEntity(Item);
				Ref_SCMPLATE_ProductRequestDetails_ITILDetails.LoadData();
	    	}
        }

		public async Task  Global_SCMRequestTypeId_onitemselected(Entity.Global_SCMRequestType Selected ,Entity.SCMPLATE_ProductRequestDetails Item  )
        {
            // Console.WriteLine(await Utility.JSON.ToJson(Selected));

           	// شرط اینکه آیا فرآیند تحویل اجرا گردد؟
            if(Item.Global_SCMRequestTypeId.ToString() == "a9c5df1c-d99c-ef11-8354-005056a02a64")
            {
                await TahvilIsVisible(true,true,Item);
				await KharidIsVisible(false,false,Item);
            }

            // شرط نوع درخواست بر اساس خرید کالا
            if(Item.Global_SCMRequestTypeId.ToString() == "3b9e934d-d99c-ef11-8354-005056a02a64")
            {
                await TahvilIsVisible(false,false,Item);
				await KharidIsVisible(true,true,Item);
            }

            // شرط نوع درخواست بر اساس تحویل و خرید کالا
            if(Item.Global_SCMRequestTypeId.ToString() == "73f6c459-d99c-ef11-8354-005056a02a64")
            {
                await TahvilKharidIsVisible(true,true,Item);
            }
        }

		#endregion FunctionEvents

}
}
