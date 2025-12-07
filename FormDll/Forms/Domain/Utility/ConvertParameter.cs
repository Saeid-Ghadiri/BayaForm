using Baya.Models.Baya.Requests;
using Baya.Models.HR.JOBs;
using System.Collections.Generic;
using System.Drawing;
using Utility;

namespace Domain.Utility
{
    public class ConvertParameter
    {
        /// <summary>
        /// تبدیل کننده لیست های پارامتر به ابجکت
        /// </summary>
        /// <returns></returns>
        public static async Task<(dynamic _RequestParameters, dynamic _CartablesParameters)> ConvertParametersToDaunamic(List<Baya.Models.Baya.Requests.Request_Parameters> RequestParameters, List<Baya.Models.Baya.Requests.Cartable_Parameter> CartablesParameters)
        {
            //لیست های دریافتی رو تبدیل به ابجکت میکنه بر میگردونه
            dynamic _RequestParameters = new Dictionary<string, object>();
            if (RequestParameters != null)
            {
                _RequestParameters = await ConvertToObject(RequestParameters.ToDictionary(x => x?.ProcessParameters?.Parameter?.Key, x => x._Value));
            }

            dynamic _CartablesParameters = new Dictionary<string, object>();
            if (CartablesParameters != null)
            {
                _CartablesParameters = await ConvertToObject(CartablesParameters.ToDictionary(x => x?.Key, x => x._Value));
            }

            return (_RequestParameters, _CartablesParameters);
        }

        /// <summary>
        /// تبدیل دیکشنری به ابجکت
        /// لیست دیکشنری Key Valu به ابجکت
        /// </summary>
        /// <param name="List"></param>
        /// <returns></returns>
        public static async Task<dynamic> ConvertToObject(Dictionary<string, object> List)
        {
            //درست کردن رشته جی سان آرایه پارامتر ها
            string Json = "{";
            foreach (var item in List)
            {
                if (item.Value is not null)
                {
                    var Type = item.Value.GetType();

                    string AppendQutation = await IfAppendQutation(Type);
                    string KeyClean = item.Key.Replace("@", "").Replace("$", "");
                    Json += $"\"{KeyClean}\":" + AppendQutation + item.Value + AppendQutation;
                    Json += ",";
                }
            }
            Json += "}";

            //تبدیل به ابجک رشته جی سان
            dynamic Object = await JSON.ToObject<dynamic>(Json);

            return Object;
        }

        /// <summary>
        /// اگه تایپ رشته باشه کوتیشکن بر میگردونه
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static async Task<string> IfAppendQutation(Type Type)
        {
            switch (Type.Name)
            {
                case "String":
                    return "\"";
                    break;
            }
            return "";
        }

        public static async Task<List<KeyValuePair<string, string>>> GetTemplate(string template)
        {
            //لیست ستون ها در بیاد
            //<span>{{item.PriorityTitle}} </span>
            //<span>{{item.Title}} {{item.Id}} </span>
            //<span>{{item.PriorityTitle}} {{item.PriorityDescription}} </span>
            //<span>{{item.Title:عنوان}} {{item.Id:آیدی}} </span>

            template = template.Replace("<span>", "");
            template = template.Replace("</span>", "");
            template = template.Replace("item.", "");
            template = template.Trim();

            List<KeyValuePair<string, string>> kvp = template.Split("}}").
             Where(x => !string.IsNullOrEmpty(x)).Select(x => {

                 return new KeyValuePair<string, string>(
                    x.Contains(':') ? x.Replace("{{", "").Split(':')[0].TrimStart() : x.Replace("{{", "").TrimStart(),
                     x.Contains(':') ?
                     (!string.IsNullOrEmpty(x.Replace("{{", "").Split(':')[1])
                     ? x.Replace("{{", "").Split(':')[1] : x.Replace("{{", "").Split(':')[0]).TrimStart()
                     : x.Replace("{{", "").TrimStart()
                );
             }).ToList();

            return kvp;
        }

    }
}
