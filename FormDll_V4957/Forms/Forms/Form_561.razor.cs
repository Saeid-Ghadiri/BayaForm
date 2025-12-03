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
    public class Form_561Base : Form_561Peropeties
    {

        // 09c26d85-9ebe-ef11-a4fa-005056a2b6bd	11	تعمیرات
        // a125e1f6-9ebe-ef11-a4fa-005056a2b6bd	12	طراحی و ساخت
        // 2a087a09-9fbe-ef11-a4fa-005056a2b6bd	13	پیمانکار
        // 7d833b4a-9fbe-ef11-a4fa-005056a2b6bd	14	آنالیز - کالیبراسیون


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
            // var Item = _Entity.SCMPETCO_OS_Details;
            // // 
            // Item.IsEnableSampleGoods = null;

            // // SCMPETCO_OS_Details_SampleGoodsFiles - آیا نمونه دارد؟>
            // Item.IsEnableSampleGoods = null;
        }
    }

    /// <summary>
    /// اعتبار سنجی فرم
    /// </summary>
    /// <returns></returns>
    public override async Task<bool> FormValidator()
    {
        bool IsValid = true;

        var List = _Entity.SCMPETCO_OS_Details.ToList();
        
        int ListCount = List.Count();

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
                if (Item.SCMPETCO_OS_Details_UploadFiles == null)
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

            // SCMPETCO_OS_Details_SampleGoodsFiles - آیا نمونه دارد؟>
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
                if (Item.SCMPETCO_OS_Details_SampleGoodsFiles == null)
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
        }

        // فیلد منتج از برون سپاری
        if (_Entity.SCMPETCO_OS_ResultingFromId == null)
		{
			IsValid = false;
            
			toastService.ShowError("لطفا یک نوع منتج از درخواست برون سپاری یا خرید خدمات را انتخاب نمایید.",
			settings => {
				settings.Timeout = 4;
				settings.ShowProgressBar = true;
				settings.PauseProgressOnHover = true;
			});
		}

        // SCMPETCO_ICT_DepartmentsId - بخش‌های فناوری اطلاعات
        if(_Entity.SCMPETCO_ICT_DepartmentsId == null)
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

        if ((await BeforSubmit()).Status != HttpStatusCode.OK)
        {
            StateHasChanged();
            Result.Status = HttpStatusCode.InternalServerError;
            return Result;
        }

        _loadingService.Show();
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
            _loadingService.Hide();
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

    public async Task ITILVisible(bool Visible)
    {
        Ref_SCMPETCO_OS_Details_ResultingFromITIL.SetVisible(Visible);
    }

    public async Task ITILDetailsVisible(bool Visible)
  {
        Ref_SCMPETCO_OS_Details_RequestIdITIL.SetVisible(Visible);
        Ref_SCMPETCO_OS_Details_RequestIdITIL.SetDisabled(true);

        Ref_SCMPETCO_OS_Details_RequesterUserITIL.SetVisible(Visible);
        Ref_SCMPETCO_OS_Details_RequesterUserITIL.SetDisabled(true);
        
        Ref_SCMPETCO_OS_Details_CreatedAtITIL.SetVisible(Visible);
        Ref_SCMPETCO_OS_Details_CreatedAtITIL.SetDisabled(true);
        
        Ref_SCMPETCO_OS_Details_ITILDetails.SetVisible(Visible);

    }

    public async Task ResultingFrom_Code13(bool Visible, bool Value, Entity.SCMPETCO_OS_Details Item)
    {
        // IsEnableDemolitionAndRenovation
        Ref_SCMPETCO_OS_Details_IsEnableDemolitionAndRenovation.SetVisible(Visible);
		Item.IsEnableDemolitionAndRenovation = Value;            
    }


    public async Task<bool> SCMPETCO_OS_Details_editmodelsaving(object e   )
    {
        bool IsCancelled = false;

			var MainModel = (Entity.SCMPETCO_OS_Details)e;

            // OS_JobTitle - عنوان کار
            if (MainModel.OS_JobTitle == null)
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
            if (MainModel.Amount == null || MainModel.Amount == 0)
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
            if (MainModel.Global_PriorityId == null)
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
            if (MainModel.Global_UnitsId == null)
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
            if (MainModel.UploadFileIsEnable == null)
			{
				IsCancelled = true;
                                
				toastService.ShowError("لطفا گزینه آیا مدارک پیوست دارد؟ را تکمیل نمایید.",
				settings => {
					settings.Timeout = 4;
					settings.ShowProgressBar = true;
					settings.PauseProgressOnHover = true;
				});
			}

            // UploadFileIsEnable - آیا مدارک پیوست دارد؟
            if (MainModel.UploadFileIsEnable == null)
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
            if(MainModel.UploadFileIsEnable.HasValue && MainModel.UploadFileIsEnable.Value)
            {
                if (MainModel.SCMPETCO_OS_Details_UploadFiles == null)
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

            // SCMPETCO_OS_Details_SampleGoodsFiles - آیا فایل نمونه دارد؟>
            if(MainModel.IsEnableSampleGoods == null)
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
            if(MainModel.IsEnableSampleGoods.HasValue && MainModel.IsEnableSampleGoods.Value)
            {                
                if (MainModel.SCMPETCO_OS_Details_SampleGoodsFiles == null)
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
            if (MainModel.GuaranteeIsEnable == null)
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
            if (MainModel.HistoryRepairsIsEnable == null)
            {
                IsCancelled = true;

                toastService.ShowError("لطفا گزینه سابقه تعمیرات دارد؟ را تکمیل نمایید.",
                settings => {
                    settings.Timeout = 4;
                    settings.ShowProgressBar = true;
                    settings.PauseProgressOnHover = true;
                });
            }
        return IsCancelled;
    }
        
    public async Task  SCMPETCO_OS_Details_afterrendermodal(Entity.SCMPETCO_OS_Details Item)
    {
        // فایل مدارک
        if (Item.UploadFileIsEnable.HasValue && Item.UploadFileIsEnable.Value)
		{
			Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_UploadFiles.SetVisible(true); 
		}
		else
		{
			Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_UploadFiles.SetVisible(false); 
		}

          
        // فیلد: منتج از پیمانکار با Code 13
        if( _Entity.SCMPETCO_OS_ResultingFromId.HasValue && _Entity.SCMPETCO_OS_ResultingFromId.ToString() == "2a087a09-9fbe-ef11-a4fa-005056a2b6bd")
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
			Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_SampleGoodsFiles.SetVisible(true); 
		}
		else
		{
			Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_SampleGoodsFiles.SetVisible(false); 
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

        // جزئیات درخواست  ITIL
        if (Item.ResultingFromITIL != null)
		{
     		Ref_SCMPETCO_OS_Details_ITILDetails.SetEntity(Item);
			Ref_SCMPETCO_OS_Details_ITILDetails.LoadData();
 		}
    }

	public async Task  UploadFileIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCMPETCO_OS_Details Item)
    {
        var UploadFilesIsVisible = Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_UploadFiles;
		if (Selected.Value.ToString() == "true")
		{
			UploadFilesIsVisible.SetVisible(true); 
		}
		else
		{
			UploadFilesIsVisible.SetVisible(false); 
		}
    }

		 

		public async Task  IsEnableSampleGoods_oninput(ChangeEventArgs Selected ,Entity.SCMPETCO_OS_Details Item  )
        {
            // مخفی کردن فیلد فایل نمونه
			var SampleFileIsVisible = Ref_SCMPETCO_OS_Details_SCMPETCO_OS_Details_SampleGoodsFiles;

			if (Selected.Value.ToString() == "true")
			{
				SampleFileIsVisible.SetVisible(true); 
			}
			else
			{
				SampleFileIsVisible.SetVisible(false); 
			}            
        }

		public async Task  ITILCodeIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCMPETCO_OS_Details Item  )
        {
            // نمایش / عدم نمایش فیلد منتج از ITIL
            if (Selected.Value.ToString() == "true")
	        {
                await ITILVisible(true);
	        }
	        else
	        {
                await ITILVisible(false); 
	        }          
        }
        public async Task  ResultingFromITIL_onitemselected(dynamic Selected ,Entity.SCMPETCO_OS_Details Item  )
        {
            // نمایش / عدم نمایش فیلد ITIL Detail
            await ITILDetailsVisible(true);

            if (Item.ResultingFromITIL != null)
	        {
	        	Item.RequestIdITIL = Selected.RequestID;
	        	Item.RequesterUserITIL = Selected.UserName;
	        	Item.CreatedAtITIL = Selected.CreateDate;

		    	Ref_SCMPETCO_OS_Details_ITILDetails.SetEntity(Item);
		    	Ref_SCMPETCO_OS_Details_ITILDetails.LoadData();
	        }            
        }

		#endregion FunctionEvents

}
}
