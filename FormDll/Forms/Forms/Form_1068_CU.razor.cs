using System;
using Microsoft.AspNetCore.Components;
using Baya.Models.Utility;
using System.Net;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Sitko.Blazor.CKEditor;
using static System.Net.WebRequestMethods;
using Baya.Models.ORM;
using Entity;
using Utility;
using Castle.DynamicLinqQueryBuilder;

namespace Forms.Forms
{
    public class Form_1068_CUBase : Form_1068_CUPeropeties
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

        public async Task Users_NotMapped_onitemselected(dynamic Selected)
        {
            _Entity.UserId = Selected.Id;
            //_Entity.UserFullName = (Selected.FullName == null ? "تکمیل نشده است" : Selected.FullName);
            _Entity.Name = (Selected.NAME == null ? "تکمیل نشده است" : Selected.NAME);
            _Entity.Family = (Selected.FAMILY == null ? "تکمیل نشده است" : Selected.FAMILY);
            _Entity.CompanyName = (Selected.CompanyID == null ? "تکمیل نشده است" : Selected.CompanyID);
            //_Entity.UnitName = (Selected.Sections.Unit.UnitID == null ? "تکمیل نشده است" : Selected.Sections.Unit.UnitID);
            _Entity.SectionName = (Selected.SectionID == null ? "تکمیل نشده است" : Selected.SectionID);
            _Entity.User_EMP_NO = (Selected.EMP_NO == null ? "تکمیل نشده است" : Selected.EMP_NO);
        }

        /// <summary>
        /// تابع فهرست کامل کاربران
        /// </summary>
        /// <returns></returns>
        private async Task<List<Entity.AspNetUsers>> GetUsers()
        {
            Table table = new Table()
            {
                Name = "AspNetUsers",
                Column = new List<Coulmn>()
                {
                    new Coulmn()
                    {
                        Name = "Id"
                    },
                    new Coulmn()
                    {
                        Name = "NAME"
                    },
                    new Coulmn()
                    {
                        Name = "FAMILY"
                    },
                    new Coulmn()
                    {
                        Name = "SectionID"
                    },
                    new Coulmn()
                    {
                        Name = "EMP_NO"
                    },
                    new Coulmn()
                    {
                        Name = "CompanyID"
                    },
                },
                Relation = new List<Table>()
                {
                    new Table()
                    {
                        Name = "Section",
                        Column = new List<Coulmn>()
                        {
                            new Coulmn()
                            {
                                Name = "Id"
                            },
                            new Coulmn()
                            {
                                Name = "UnitID"
                            },
                        },
                            Relation = new List<Table>()
                            {
                                new Table()
                                {
                                    Name = "Unit",
                                    Column = new List<Coulmn>()
                                    {
                                        new Coulmn()
                                        {
                                            Name = "Id"
                                        },
                                        new Coulmn()
                                        {
                                            Name = "Title"
                                        },
                                    },

                                }
                            },
                            ModeErtebat = ModeErtebat._1N
                    }
                },
                ModeErtebat = ModeErtebat._1N
            };


            var DataResult = await ApiServer.External.Services.Data.GetList("AspNetUsers", 1000, 0, table,null);

            Console.WriteLine("" + DataResult.Status);
            Console.WriteLine("" + DataResult.Content.ToString());

            List<Entity.AspNetUsers> result = await JSON.ToObject<List<Entity.AspNetUsers>>(DataResult.Content.ToString());

            Console.WriteLine("" + result.Count);
            return result;
        }

        /// <summary>
        /// تابع فهرست یک کاربر
        /// </summary>
        /// <returns></returns>
        private async Task<Entity.AspNetUsers> GetUser()
        {
            Table table = new Table()
            {
                Name = "AspNetUsers",
                Column = new List<Coulmn>()
                {
                    new Coulmn()
                    {
                        Name = "Id"
                    },
                    new Coulmn()
                    {
                        Name = "NAME"
                    },
                    new Coulmn()
                    {
                        Name = "FAMILY"
                    },
                    new Coulmn()
                    {
                        Name = "SectionID"
                    },
                    new Coulmn()
                    {
                        Name = "EMP_NO"
                    },
                    new Coulmn()
                    {
                        Name = "CompanyID"
                    },
                },
                Relation = new List<Table>()
                {
                    new Table()
                    {
                        Name = "Section",
                        Column = new List<Coulmn>()
                        {
                            new Coulmn()
                            {
                                Name = "Id"
                            },
                            new Coulmn()
                            {
                                Name = "UnitID"
                            },
                        },
                        Relation = new List<Table>()
                        {
                            new Table()
                            {
                                Name = "Unit",
                                Column = new List<Coulmn>()
                                {
                                    new Coulmn()
                                    {
                                        Name = "Id"
                                    },
                                    new Coulmn()
                                    {
                                        Name = "Title"
                                    },
                                },

                            }
                        },
                        ModeErtebat = ModeErtebat._1N
                    }
                },
                ModeErtebat = ModeErtebat._1N
            };


            //var a = AspNetUsers();
            //var DataResult = await ApiServer.External.Services.Data.GetList("AspNetUsers", 1000, 0, table);
            var DataResult = await ApiServer.External.Services.Data.Get(table, null, "AspNetUsers", null);

            Console.WriteLine("" + DataResult.Status);
            Console.WriteLine("" + DataResult.Content.ToString());

            Entity.AspNetUsers result = await JSON.ToObject<Entity.AspNetUsers>(DataResult.Content.ToString());

            //Console.WriteLine("" + result.Count);
            return result;
        }


        private async Task<string> Fullname(string userId)
        {
            // var QueryFilter = await JSON.ToObject<QueryBuilderFilterRule>("{\"Condition\":\"AND\",\"Rules\":[{\"Field\":\"AspNetUsers.Id\",\"Id\":\"1904\",\"Input\":\"text\",\"Operator\":\"equal\",\"Type\":\"string\",\"Value\":[]}]}");

            // //var DataResult = await ApiServer.External.Services.Data.GetList("AspNetUsers", 1000, 0, table);
            var DataResult = await ApiServer.External.Services.Data.Get(null, QueryFilter, "AspNetUsers", null);

            Console.WriteLine("" + DataResult.Status);
            Console.WriteLine("" + DataResult.Content.ToString());

            Entity.AspNetUsers result = await JSON.ToObject<Entity.AspNetUsers>(DataResult.Content.ToString());

            //Console.WriteLine("" + result.Count);

            var r = result.NAME + " " + result.FAMILY;
            return r;
        }

      

		#endregion FunctionEvents

    }
}