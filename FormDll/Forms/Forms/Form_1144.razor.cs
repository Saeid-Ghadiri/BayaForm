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
    public class Form_1144Base : Form_1144Peropeties
    {

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

        if (BtnWorkFlowId == "SequenceFlow_05jd0ly")
        {
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
                    if(Item.UploadFileIsEnable.HasValue & Item.UploadFileIsEnable.Value)
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

        }

        
        


        // لغو کلی درخواست
            if (BtnWorkFlowId == "SequenceFlow_1pwn7d0")
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
    
        public async Task ResultingFrom_Code13(bool Visible, bool Value, Entity.SCMPLATE_OS_Details Item)
        {
            // IsEnableDemolitionAndRenovation
            Ref_SCMPLATE_OS_Details_IsEnableDemolitionAndRenovation.SetVisible(Visible);
			Item.IsEnableDemolitionAndRenovation = Value;            
        }
        

    public async Task <bool> GridSCMPLATE_OS_MasterId_863_editmodelsaving(object e   )
        {

             bool IsCancelled = false;

            var MainModel = (Entity.SCMPLATE_OS_Details)e;

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
                if(MainModel.UploadFileIsEnable.HasValue & MainModel.UploadFileIsEnable.Value)
                {
                    if (MainModel.SCMPLATE_OS_Details_UploadFiles == null)
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

                // SCMPLATE_OS_Details_SampleGoodsFiles - آیا نمونه دارد؟>
                if(MainModel.IsEnableSampleGoods == null)
                {
                    IsCancelled = true;

                    toastService.ShowError("لطفا گزینه آیا نمونه دارد؟ را تکمیل نمایید.",
                    settings => {
                        settings.Timeout = 4;
                        settings.ShowProgressBar = true;
                        settings.PauseProgressOnHover = true;
                    });
                }

                // فیلد: فایل نمونه؟
                if(MainModel.IsEnableSampleGoods.HasValue && MainModel.IsEnableSampleGoods.Value)
                {                
                    if (MainModel.SCMPLATE_OS_Details_SampleGoodsFiles == null)
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

            return false;
        }
public async Task  GridSCMPLATE_OS_MasterId_863_afterrendermodal(Entity.SCMPLATE_OS_Details Item   )
        {

             // فیلد: منتج از پیمانکار با Code 13
            if( _Entity.SCMPLATE_OS_ResultingFromId.HasValue && _Entity.SCMPLATE_OS_ResultingFromId.ToString() == "2a087a09-9fbe-ef11-a4fa-005056a2b6bd")
            {
                await ResultingFrom_Code13(true, true, Item);
            }
            else
            {
                await ResultingFrom_Code13(false, false, Item);
            }

            // فایل مدارک
            if (Item.UploadFileIsEnable.HasValue && Item.UploadFileIsEnable.Value)
            {
                Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_UploadFiles.SetVisible(true); 
            }
            else
            {
                Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_UploadFiles.SetVisible(false); 
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
        }
public async Task  UploadFileIsEnable_oninput(ChangeEventArgs Selected ,Entity.SCMPLATE_OS_Details Item  )
        {
                var UploadFilesIsVisible = Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_UploadFiles;
                if (Selected.Value.ToString() == "true")
                {
                    UploadFilesIsVisible.SetVisible(true); 
                }
                else
                {
                    UploadFilesIsVisible.SetVisible(false); 
                }
            
        }
public async Task  IsEnableSampleGoods_oninput(ChangeEventArgs Selected ,Entity.SCMPLATE_OS_Details Item  )
        {
            // مخفی کردن فیلد فایل نمونه
            var SampleFileIsVisible = Ref_SCMPLATE_OS_Details_SCMPLATE_OS_Details_SampleGoodsFiles;
            if (Selected.Value.ToString() == "true")
            {
                SampleFileIsVisible.SetVisible(true); 
            }
            else
            {
                SampleFileIsVisible.SetVisible(false); 
            }            
            
        }

		#endregion FunctionEvents

}
}
