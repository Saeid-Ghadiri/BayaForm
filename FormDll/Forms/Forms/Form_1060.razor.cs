using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;

namespace Forms.Forms
{
    public class Form_1060Base : Form_1060Peropeties
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

        // if (_Entity.ReqCount != 5)
        // {
        //     IsValid = false;
        //     SumaryMessage += "تعداد درخواست مخالف 5 باشد";
        // }

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
         _Entity.CurrentCodeMelli =_CartablesParameters.CurentUserCodeMelli;
        _Entity.CodeMelli =_CartablesParameters.CurentUserCodeMelli;

        _Entity.Company =_CartablesParameters.CurentUserCompanyName;
        StateHasChanged();
    }


    #region FunctionEvents

    public async Task  submit_onclick(MouseEventArgs Selected   )
        {

                if(_Entity.CurrentCodeMelli != _Entity.CodeMelli){
                     //return; // طبق گزارشات خطا و جهت تست و خطایابی کاربران
                }
            Console.WriteLine("Log ::  CodeMelli ::" + _Entity.CodeMelli.ToString());
            Console.WriteLine("Log ::  Year ::" + _Entity.Month);
            Console.WriteLine("Log ::  Month ::" + _Entity.Year);

            // var ApiResult = await ApiServer.Internal.Finantial.Salary.Get(Convert.ToInt32(_Entity.Year), Convert.ToInt32(_Entity.Month), _Entity.CodeMelli.ToString(), ApplicationType.InfoBig, await (_authenticationStateProvider as CustomAuthProvider).GetToken());

          var ApiResult = await  ApiServer.External.Services.Salary.Get(Convert.ToInt32(_Entity.Year),Convert.ToInt32(_Entity.Month),_Entity.CodeMelli.ToString(),"https://fi.workcv.ir:449/api/");

              Console.WriteLine("Log ::  Api Call ::");
            var x = await Utility.JSON.ToJson(ApiResult);
			Console.WriteLine("Log ::  Data ::" + x);

            var Sal = await Utility.JSON.ToObject<Salary>(ApiResult.Content.ToString());
            
            _Entity.Emp_No = Sal.Emp_No;
            _Entity.Sum_v_day = Sal.Sum_v_dayNUM.ToString();
            _Entity.Sum_v_hour = Sal.Sum_v_hour;

_Entity.Emp_Name = Sal.Emp_Name;
_Entity.Emp_Family = Sal.Emp_Family;
_Entity.WorkName = Sal.WorkName;
_Entity.Centname = Sal.Centname;
_Entity.JobName = Sal.JobName;

            _Entity.Pardakhti = (Sal._Mazaya.Select(p => p.MazayapriceNum).Sum() - Sal._Kosorat.Select(p => p.KosoorpriceNum).Sum()).ToString();
            _Entity.Nakhales = Sal._Mazaya.Select(p => p.MazayapriceNum).Sum().ToString();
            _Entity.KosoratTotal = Sal._Kosorat.Select(p => p.KosoorpriceNum).Sum().ToString();


var bedehiQbl = Sal._Mazaya
    .FirstOrDefault(x => x.Mazayapname == "پرداخت بدهي ماه قبل");

    var bedehiQblPrice = bedehiQbl?.Mazayaprice;

var ravandQbl = Sal._Mazaya
    .FirstOrDefault(x => x.Mazayapname == "روند ماه قبل");

    var ravandQblPrice = ravandQbl?.Mazayaprice;
    
var kharbarMaskan = Sal._Mazaya
    .FirstOrDefault(x => x.Mazayapname == "خواربار و مسکن");

    var kharbarMaskanPrice = kharbarMaskan?.Mazayaprice;

var refahi = Sal._Mazaya
    .FirstOrDefault(x => x.Mazayapname == "مزاياي رفاهي انگيزه ايي");

    var refahiPrice = refahi?.Mazayaprice;

var jazb2 = Sal._Mazaya
    .FirstOrDefault(x => x.Mazayapname == "حق جذب 2");

    var jazb2Price = jazb2?.Mazayaprice;

var padash1 = Sal._Mazaya
    .FirstOrDefault(x => x.Mazayapname == "پاداش افزايش توليد1");

    var padash1Price = padash1?.Mazayaprice;

var maBeOTafavot = Sal._Mazaya
    .FirstOrDefault(x => x.Mazayapname == "ما به التفاوت");

    var maBeOTafavotPrice = maBeOTafavot?.Mazayaprice;

var payeSanavat = Sal._Mazaya
    .FirstOrDefault(x => x.Mazayapname == "پايه سنوات");

    var payeSanavatPrice = payeSanavat?.Mazayaprice;

var padash2 = Sal._Mazaya
    .FirstOrDefault(x => x.Mazayapname == "پاداش افزايش توليد2");

    var padash2Price = padash2?.Mazayaprice;


var hoghoghPaye = Sal._Mazaya
    .FirstOrDefault(x => x.Mazayapname == "حقوق پايه");

    var hoghoghPayePrice = hoghoghPaye?.Mazayaprice;


var ezafeKariAdi = Sal._Mazaya
    .FirstOrDefault(x => x.Mazayapname == "اضافه کاري عادي");
    var ezafeKariAdiPrice = ezafeKariAdi?.Mazayaprice;
    var ezafeKariAdiHour  = ezafeKariAdi?.Mazayaphour;


var ezafeKariTatil = Sal._Mazaya
    .FirstOrDefault(x => x.Mazayapname == "اضافه کاري تعطيلي");
    var ezafeKariTatilPrice = ezafeKariTatil?.Mazayaprice;
    var ezafeKariTatilHour  = ezafeKariTatil?.Mazayaphour;


var haghTaahol = Sal._Mazaya
    .FirstOrDefault(x => x.Mazayapname == "حق تأهل");

    var haghTaaholPrice = haghTaahol?.Mazayaprice;

// var haghTaahol = Sal._Mazaya
//     .FirstOrDefault(x => x.Mazayapname == "ایاب و ذهاب غیر مستمر");

//     var haghTaaholPrice = haghTaahol?.Mazayaprice;


var mandegariPost = Sal._Mazaya
    .FirstOrDefault(x => x.Mazayapname == "ماندگاري پست");

    var mandegariPostPrice = mandegariPost?.Mazayaprice;

var hagheOlad = Sal._Mazaya
    .FirstOrDefault(x => x.Mazayapname == "حق اولاد");

    var hagheOladPrice = hagheOlad?.Mazayaprice;



var mamooriat = Sal._Mazaya
    .FirstOrDefault(x => x.Mazayapname == "حق مأموريت");

    var mamooriatPrice = mamooriat?.Mazayaprice;

var haghePost = Sal._Mazaya
    .FirstOrDefault(x => x.Mazayapname == "حق پست");

    var haghePostPrice = haghePost?.Mazayaprice;



//*****



var maliat = Sal._Kosorat
    .FirstOrDefault(x => x.Kosoorpname == "ماليات");

var maliatPrice = maliat?.Kosoorprice;


var bimeKarmand = Sal._Kosorat
    .FirstOrDefault(x => x.Kosoorpname == "بيمه اصلي سهم کارمند");

var bimeKarmandPrice = bimeKarmand?.Kosoorprice;


var ravandJari = Sal._Kosorat
    .FirstOrDefault(x => x.Kosoorpname == "روند ماه جاري");

var ravandJariPrice = ravandJari?.Kosoorprice;


var bimeTakmili = Sal._Kosorat
    .FirstOrDefault(x => x.Kosoorpname == "بيمه تکميلي");

var bimeTakmiliPrice = bimeTakmili?.Kosoorprice;


var tabaee1 = Sal._Kosorat
    .FirstOrDefault(x => x.Kosoorpname == "تبعي 1");

var tabaee1Price = tabaee1?.Kosoorprice;

var tabaee2 = Sal._Kosorat
    .FirstOrDefault(x => x.Kosoorpname == "تبعي 2");

var tabaee2Price = tabaee2?.Kosoorprice;

var tabaee3 = Sal._Kosorat
    .FirstOrDefault(x => x.Kosoorpname == "تبعي 3");

var tabaee3Price = tabaee3?.Kosoorprice;


var bimeBazneshastegi = Sal._Kosorat
    .FirstOrDefault(x => x.Kosoorpname == "بيمه بازنشستگي سهم کارمند");

var bimeBazneshastegiPrice = bimeBazneshastegi?.Kosoorprice;


var bedehiMahQbl = Sal._Kosorat
    .FirstOrDefault(x => x.Kosoorpname == "بدهي ماه قبل");

var bedehiMahQblPrice = bedehiMahQbl?.Kosoorprice;


var alalHesab = Sal._Kosorat
    .FirstOrDefault(x => x.Kosoorpname == "علي الحساب پاداش افزايش توليد 1و2");

var alalHesabPrice = alalHesab?.Kosoorprice;



var mosaede = Sal._Kosorat
    .FirstOrDefault(x => x.Kosoorpname == "مساعده پرداختي کارکنان");

    var mosaedePrice = mosaede?.Kosoorprice;

var vaamZaroori = Sal._Kosorat
    .FirstOrDefault(x => x.Kosoorpname == "وام ضروري کارکنان");

    var vaamZarooriPrice = vaamZaroori?.Kosoorprice;



_Entity.BedehiQbl    = bedehiQblPrice?.ToString();
_Entity.RavandQbl    = ravandQblPrice?.ToString();
_Entity.RavandJari    = ravandJariPrice?.ToString();
_Entity.KharbarMaskan    = kharbarMaskanPrice?.ToString();
_Entity.Refahi    = refahiPrice?.ToString();
_Entity.Jazb2    = jazb2Price?.ToString();
_Entity.Padash1    = padash1Price?.ToString();
_Entity.MaBeOTafavot    = maBeOTafavotPrice?.ToString();
_Entity.PayeSanavat    = payeSanavatPrice?.ToString();
_Entity.Padash2    = padash2Price?.ToString();
_Entity.EzafeKariTatil  = ezafeKariTatilPrice?.ToString();
_Entity.EzafeKariTatilHour = ezafeKariTatilHour?.ToString();

_Entity.HoghoghPaye    = hoghoghPayePrice?.ToString();
_Entity.EzafeKariAdi   = ezafeKariAdiPrice?.ToString();
_Entity.EzafeKariAdiHour   = ezafeKariAdiHour?.ToString();
_Entity.HaghTaaholPrice = haghTaaholPrice?.ToString();

_Entity.MandegariPost    = mandegariPostPrice?.ToString();
_Entity.HagheOlad    = hagheOladPrice?.ToString();
_Entity.Mamooriat    = mamooriatPrice?.ToString();
_Entity.HaghePost    = haghePostPrice?.ToString();


_Entity.Maliat         = maliatPrice?.ToString();
_Entity.Bime           = bimeKarmandPrice?.ToString();
_Entity.BimeTakmili           = bimeTakmiliPrice?.ToString();
_Entity.Tabaee1           = tabaee1Price?.ToString();
_Entity.Tabaee2           = tabaee2Price?.ToString();
_Entity.Tabaee3           = tabaee3Price?.ToString();
_Entity.BimeBazneshastegi           = bimeBazneshastegiPrice?.ToString();
_Entity.BedehiMahQbl           = bedehiMahQblPrice?.ToString();
_Entity.AlalHesab           = alalHesabPrice?.ToString();
_Entity.Mosaede           = mosaedePrice?.ToString();
_Entity.VaamZaroori           = vaamZarooriPrice?.ToString();
            // int coutMazaya = 0;
            // int coutKosorat = 0;
            // if (Sal != null)
            // {
            //     coutMazaya = Sal._Mazaya.Count();
            //     coutKosorat = Sal._Kosorat.Count();
            // }

            // for (int i = 0; i <= (coutMazaya > coutKosorat ? coutMazaya : coutKosorat) - 1; i++)
            //     {
            //             if (coutMazaya - 1 >= i)
            //             {
            //                     //Sal._Mazaya[i].Mazayapname

            //                     if (!string.IsNullOrEmpty(Sal._Mazaya[i].Mazayaphour) && Sal._Mazaya[i].Mazayaphour != "0")
            //                     {
            //                        _Entity.Mazaya =  (Sal._Mazaya[i].Mazayaphour).ToString();
            //                     }

            //                     //Sal._Mazaya[i].MazayapriceNum.ToString();
            //             }
            //             else
            //             {
            //             }
            //             if (coutKosorat - 1 >= i)
            //             {
            //                     //Sal._Kosorat[i].Kosoorpname

            //                     if (Sal._Kosorat[i].kosoorremain != 0)
            //                     {
            //                         _Entity.Kosorat = (Sal._Kosorat[i].kosoorremain).ToString();
            //                     }

            //                     //Sal._Kosorat[i].KosoorpriceNum.ToString();
            //             }
            //             else
            //             {
            //             }

            //     }


        }


 public class Salary
 {
     public string Emp_No { get; set; }
     public string Emp_Name { get; set; }
     public string Emp_Family { get; set; }
     public string WorkName { get; set; }
     public string Sum_v_day { get; set; }
     public int Sum_v_dayNUM
     {
         get
         {
             String s = Sum_v_day;
             Double temp;

             Boolean isOk = Double.TryParse(s, out temp);

             int value = isOk ? (int)temp : 0;
             return value;
         }
     }
     public string Sum_v_hour { get; set; }
     public string Centname { get; set; }
     public string Melicode { get; set; }
     public string JobName { get; set; }

     /// <summary>
     /// مزایا
     /// </summary>
     public string Mazaya { get; set; }

     /// <summary>
     /// کسورات
     /// </summary>
     public string Kosorat { get; set; }
     /// <summary>
     /// سال
     /// </summary>
     public int Year { get; set; }
     /// <summary>
     /// ماه
     /// </summary>
     public int Month { get; set; }
     public string Company { get; set; }



     public List<SalaryMazayaItem> _Mazaya
     {
         get
         {
             if (!string.IsNullOrEmpty(Mazaya))
             {
                 return Utility.JSON.ToObject<List<SalaryMazayaItem>>(Mazaya).Result;
             }
             else
             {
                 return null;
             }
         }
         set
         {
             if (value != null)
             {
                 Mazaya = Utility.JSON.ToJson(value).Result;
             }

         }
     }

     public List<SalaryKosoratItem> _Kosorat
     {
         get
         {
             if (!string.IsNullOrEmpty(Kosorat))
             {
                 return Utility.JSON.ToObject<List<SalaryKosoratItem>>(Kosorat).Result;
             }
             else
             {
                 return null;
             }
         }
         set
         {
             if (value != null)
             {
                 Kosorat = Utility.JSON.ToJson(value).Result;
             }
         }
     }

     public string Kosoorpname { get; set; }
     public string Kosoorprice { get; set; }
     public string Kosoorphour { get; set; }
     public string Mazayapname { get; set; }
     public string Mazayaprice { get; set; }
     public string Mazayaphour { get; set; }
 }

 public class SalaryMazayaItem
 {
     public string Mazayapname { get; set; }
     public string Mazayaprice { get; set; }

     public long MazayapriceNum
     {
         get
         {
             String s = Mazayaprice;
             Double temp;

             Boolean isOk = Double.TryParse(s, out temp);

             long value = isOk ? (long)temp : 0;
             return value;
         }
     }

     public string Mazayaphour { get; set; }
 }

 public class SalaryKosoratItem
 {
     public string Kosoorpname { get; set; }
     public string Kosoorprice { get; set; }
     public long KosoorpriceNum
     {
         get
         {
             String s = Kosoorprice;
             Double temp;

             Boolean isOk = Double.TryParse(s, out temp);

             long value = isOk ? (long)temp : 0;
             return value;
         }
     }

     public long kosoorremain { get; set; }
     public string Kosoorphour { get; set; }
 }


	

		#endregion FunctionEvents

}
}
