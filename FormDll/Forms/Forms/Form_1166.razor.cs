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
    public class Form_1166Base : Form_1166Peropeties
    {


        // Toast  
		[Inject]
	    public IToastService toastService { get; set; }

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

        var List = _Entity.SCMPLATE_OS_Details.ToList();
        
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
			"<span class='fs-6 text-secondary text-right'>تا کنون هیچ ردیف درخواستی تکمیل نشده است. لطفا برای ثبت و ادامه به مرحله بعد حداقل یک ردیف در درخواست خود ثبتنمایید.	<span></div>" +
            "</div>"; 
			var confirmation = await Confirm.ShowAsync(
				title: "",
				message1: htmlString,
				confirmDialogOptions: options);
        }

        foreach (var Item in List)
        {
            // OS_JobTitle - عنوان کار
            if (Item.OS_JobTitle == null)
			{
				IsValid = false;
                
				toastService.ShowError("لطفا گزینه عنوان کار را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}

            // Amount - تعداد 
            if (Item.Amount == null || Item.Amount == 0)
			{
				IsValid = false;
                
				toastService.ShowError("لطفا گزینه تعداد را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}

            // فیلد اولویت
            if (Item.Global_PriorityId == null)
			{
				IsValid = false;
                
				toastService.ShowError("لطفا گزینه اولویت را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}

            // فیلد واحد کالا
            if (Item.Global_UnitsId == null)
			{
				IsValid = false;
                
				toastService.ShowError("لطفا گزینه واحد کالا را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}            
            
            // UploadFileIsEnable - آیا مدارک پیوست دارد؟
            if (Item.UploadFileIsEnable == null)
			{
				IsValid = false;

				toastService.ShowError("لطفا گزینه آیا مدارک پیوست دارد؟ را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}

            // فیلد: فایل مدارک پیوست
            if(Item.UploadFileIsEnable.HasValue && Item.UploadFileIsEnable.Value)
            {
                if (Item.SCMPLATE_OS_Details_UploadFiles == null)
			    {
			    	IsValid = false;

			    	toastService.ShowError("لطفا گزینه فایل مدارک پیوست را تکمیل نمایید.",
			    	settings => {
			    		settings.Timeout = 4;
			    		settings.ShowProgressBar = true;
			    		settings.PauseProgressOnHover = true;
			    	});
			    }
            }

            // SCMPLATE_OS_Details_SampleGoodsFiles - آیا نمونه دارد؟>
            if(Item.IsEnableSampleGoods == null)
            {
                IsValid = false;

                toastService.ShowError("لطفا گزینه آیا نمونه دارد؟ را تکمیل نمایید.",
                settings => {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }

            // فیلد: فایل نمونه؟
            if(Item.IsEnableSampleGoods.HasValue && Item.IsEnableSampleGoods.Value)
            {                
                if (Item.SCMPLATE_OS_Details_SampleGoodsFiles == null)
                {
                    IsValid = false;

                    toastService.ShowError("لطفا گزینه آیا نمونه دارد؟ را تکمیل نمایید.",
                    settings => {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
            }

            // آیا گارانتی دارد؟ - GuaranteeIsEnable
            if (Item.GuaranteeIsEnable == null)
            {
                IsValid = false;
                toastService.ShowError("لطفا گزینه آیا گارانتی دارد؟ را تکمیل نمایید.",
                settings => {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }

            // سابقه تعمیرات دارد؟ - HistoryRepairsIsEnable
            if (Item.HistoryRepairsIsEnable == null)
            {
                IsValid = false;
                toastService.ShowError("لطفا گزینه سابقه تعمیرات دارد؟ را تکمیل نمایید.",
                settings => {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }

                //ITIL 
            if (Item.ITILCodeIsEnable == null)
            {
                IsValid = false;
                toastService.ShowError("لطفا گزینه آیا ITIL  دارد؟ را تکمیل نمایید.",
                settings => {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }

            // منتج از ITIL
            if(Item.ITILCodeIsEnable.HasValue && Item.ITILCodeIsEnable.Value)
            {                
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
            }
        }

        // فیلد منتج از برون سپاری
        if (_Entity.SCMPLATE_OS_ResultingFromId == null)
		{
			IsValid = false;
            
			toastService.ShowError("لطفا یک نوع منتج از درخواست برون سپاری یا خرید خدمات را انتخاب نمایید.",
			settings => {
				settings.Timeout = 4;
				settings.ShowProgressBar = true;
				settings.PauseProgressOnHover = true;
			});
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

//public async Task ITIL(bool Visible, bool Value, Entity.SCMPLATE_ProductRequestDetails Item)
    public async Task ITILVisible(bool Visible)
    {
        Ref_SCMPLATE_OS_Details_ResultingFromITIL.SetVisible(Visible);
    }

    public async Task ITILDetailsVisible(bool Visible)
    {
        Ref_SCMPLATE_OS_Details_RequestIdITIL.SetVisible(Visible);
        Ref_SCMPLATE_OS_Details_RequestIdITIL.SetDisabled(true);

        Ref_SCMPLATE_OS_Details_RequesterUserITIL.SetVisible(Visible);
        Ref_SCMPLATE_OS_Details_RequesterUserITIL.SetDisabled(true);
        
        Ref_SCMPLATE_OS_Details_CreatedAtITIL.SetVisible(Visible);
        Ref_SCMPLATE_OS_Details_CreatedAtITIL.SetDisabled(true);
        
        Ref_SCMPLATE_OS_Details_ITILDetails.SetVisible(Visible);
    }

    public async Task ResultingFrom_Code13(bool Visible, bool Value, Entity.SCMPLATE_OS_Details Item)
    {
        // IsEnableDemolitionAndRenovation
        Ref_SCMPLATE_OS_Details_IsEnableDemolitionAndRenovation.SetVisible(Visible);
		Item.IsEnableDemolitionAndRenovation = Value;            
    }


    public async Task <bool> GridSCMPLATE_OS_MasterId_863_editmodelsaving(object e   )
        {

             bool IsCancelled = false;

		var ModelDetail = (Entity.SCMPLATE_OS_Details)e;

            // OS_JobTitle - عنوان کار
            if (ModelDetail.OS_JobTitle == null)
			{
				IsCancelled = true;
                
				toastService.ShowError("لطفا گزینه عنوان کار را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}

            // Amount - تعداد 
            if (ModelDetail.Amount == null || ModelDetail.Amount == 0)
			{
				IsCancelled = true;
                
				toastService.ShowError("لطفا گزینه تعداد را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}

            // فیلد اولویت
            if (ModelDetail.Global_PriorityId == null)
			{
				IsCancelled = true;
                
				toastService.ShowError("لطفا گزینه اولویت را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}

            // فیلد واحد کالا
            if (ModelDetail.Global_UnitsId == null)
			{
				IsCancelled = true;
                
				toastService.ShowError("لطفا گزینه واحد کالا را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}

            // UploadFileIsEnable - آیا مدارک پیوست دارد؟
            if (ModelDetail.UploadFileIsEnable == null)
			{
				IsCancelled = true;

				toastService.ShowError("لطفا گزینه آیا مدارک پیوست دارد؟ را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}

            // فایل مدارک پیوست 
            if(ModelDetail.UploadFileIsEnable.HasValue && ModelDetail.UploadFileIsEnable.Value)
            {
                if (ModelDetail.SCMPLATE_OS_Details_UploadFiles == null)
			    {
			    	IsCancelled = true;

			    	toastService.ShowError("لطفا گزینه فایل مدارک پیوست را تکمیل نمایید.",
			    	settings => {
			    		settings.Timeout = 4;
			    		settings.ShowProgressBar = true;
			    		settings.PauseProgressOnHover = true;
			    	});
			    }
            }

            // SCMPLATE_OS_Details_SampleGoodsFiles - آیا فایل نمونه دارد؟>
            if(ModelDetail.IsEnableSampleGoods == null)
            {
                IsCancelled = true;

                toastService.ShowError("لطفا گزینه آیا فایل نمونه دارد؟ را تکمیل نمایید.",
                settings => {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }

            // فیلد: فایل نمونه؟
            if(ModelDetail.IsEnableSampleGoods.HasValue && ModelDetail.IsEnableSampleGoods.Value)
            {                
                if (ModelDetail.SCMPLATE_OS_Details_SampleGoodsFiles == null)
                {
                    IsCancelled = true;

                    toastService.ShowError("لطفا گزینه آیا نمونه دارد؟ را تکمیل نمایید.",
                    settings => {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
            }

            // آیا گارانتی دارد؟ - GuaranteeIsEnable
            if (ModelDetail.GuaranteeIsEnable == null)
            {
                IsCancelled = true;

                toastService.ShowError("لطفا گزینه آیا گارانتی دارد؟ را تکمیل نمایید.",
                settings => {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }

            // سابقه تعمیرات دارد؟ - HistoryRepairsIsEnable
            if (ModelDetail.HistoryRepairsIsEnable == null)
            {
                IsCancelled = true;

                toastService.ShowError("لطفا گزینه سابقه تعمیرات دارد؟ را تکمیل نمایید.",
                settings => {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }

            //ITIL 
            if (ModelDetail.ITILCodeIsEnable == null)
            {
                IsCancelled = true;

                toastService.ShowError("لطفا گزینه آیا ITIL  دارد؟ را تکمیل نمایید.",
                settings => {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }

            // منتج از ITIL
            if(ModelDetail.ITILCodeIsEnable.HasValue && ModelDetail.ITILCodeIsEnable.Value)
            {                
                if (ModelDetail.ResultingFromITIL == null)
                {
                    IsCancelled = true;

                    toastService.ShowError("لطفا گزینه  منتج از ITIL را تکمیل نمایید.",
                    settings => {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }
            }

        return IsCancelled;
        }
public async Task  GridSCMPLATE_OS_MasterId_863_afterrendermodal(Entity.SCMPLATE_OS_Details Item   )
        {

             // 
        if (Item.UploadFileIsEnable.HasValue && Item.UploadFileIsEnable.Value)
		{
			Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_UploadFiles.SetVisible(true); 
		}
		else
		{
			Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_UploadFiles.SetVisible(false); 
		}

          
        // فیلد: منتج از پیمانکار با Code 13
        if( _Entity.SCMPLATE_OS_ResultingFromId.HasValue && _Entity.SCMPLATE_OS_ResultingFromId.ToString() == "3fd46f74-a15d-f111-a514-005056a2b6bd")
        {             
            await ResultingFrom_Code13(true, true, Item);
        }
        else
        {
            await ResultingFrom_Code13(false, false, Item);
        }

        // فایل نمونه
        if (Item.IsEnableSampleGoods.HasValue && Item.IsEnableSampleGoods.Value)
		{
			Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_SampleGoodsFiles.SetVisible(true); 
		}
		else
		{
			Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_SampleGoodsFiles.SetVisible(false); 
		}

        // Show ITIL
        if (Item.ITILCodeIsEnable.HasValue && Item.ITILCodeIsEnable.Value)
		{
            await ITILVisible(true); 
		}
		else
		{
            await ITILVisible(false);
		}

        // Show ITIL Details
        if (!string.IsNullOrEmpty(Item.ResultingFromITIL))
		{
            await ITILDetailsVisible(true);
		}
		else
		{
            await ITILDetailsVisible(false);
		}

        // 
        if (Item.ResultingFromITIL != null)
		{
     		Ref_SCMPLATE_OS_Details_ITILDetails.SetEntity(Item);
			Ref_SCMPLATE_OS_Details_ITILDetails.LoadData();
 		}
        }

		#endregion FunctionEvents

}
}
