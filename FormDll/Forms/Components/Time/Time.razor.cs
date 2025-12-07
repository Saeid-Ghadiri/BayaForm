using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;


namespace Forms.Components
{
    public partial class TimeBase : Microsoft.AspNetCore.Components.ComponentBase
    {
        #region bind
        [Parameter] public System.TimeOnly? Value { get; set; }
        [Parameter] public EventCallback<System.TimeOnly?> ValueChanged { get; set; }
        public string _Value { get; set; }
        #endregion

        #region Class
        [Parameter] public string? CssClass { get; set; }
        public EventCallback<string?> ClassChanged { get; set; }
        #endregion
        #region ParameterPublic
        [Parameter]
        public bool Visible { get; set; } = true;
        public bool _Visible { get; set; }

        [Parameter] public string? Label { get; set; }
        [Parameter] public string? DisplayFormat { get; set; } = "HH:mm";
        [Parameter] public string? type { get; set; }
        [Parameter] public string? placeholder { get; set; }
        [Parameter] public string? Id { get; set; }
        [Parameter] public string? IdInput { get; set; }
        [Parameter] public bool? Required { get; set; }
        [Parameter] public string? ToolTip { get; set; }
        [Parameter] public Dictionary<string, object>? Attributes { get; set; } = new Dictionary<string, object>();
        [Parameter] public Dictionary<string, object>? _Attributes { get; set; } = new Dictionary<string, object>();

        [Parameter]
        public bool Disabled { get; set; } = false;
        public bool _Disabled { get; set; }



        [Parameter] public string? Decimal { get; set; }

        public string? InputClass { get; set; }
        #endregion
        #region events
        [Parameter] public EventCallback<ChangeEventArgs> oninput { get; set; }

        [Parameter] public EventCallback<ClipboardEventArgs> oncut { get; set; }
        [Parameter] public EventCallback<ClipboardEventArgs> oncopy { get; set; }
        [Parameter] public EventCallback<ClipboardEventArgs> onpaste { get; set; }

        [Parameter] public EventCallback<EventArgs> onselect { get; set; }
        [Parameter] public EventCallback<EventArgs> oninvalid { get; set; }

        [Parameter] public EventCallback<FocusEventArgs> onfocus { get; set; }

        [Parameter] public EventCallback<KeyboardEventArgs> onkeydown { get; set; }
        [Parameter] public EventCallback<KeyboardEventArgs> onkeypress { get; set; }
        [Parameter] public EventCallback<KeyboardEventArgs> onkeyup { get; set; }

        [Parameter] public EventCallback<MouseEventArgs> onclick { get; set; }
        #endregion
        #region ParameterTextArea
        [Parameter] public int? Rows { get; set; }
        #endregion

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                IdInput = $"f_{Guid.NewGuid().ToString("N")}";
                if (type == "hidden") { SetVisible(false); }
                if (type == "checkbox")
                {
                    InputClass = "form-check-input";
                }
                else
                {
                    InputClass = "form-control";
                }

                StateHasChanged();
            }

        }

        protected override async Task OnInitializedAsync()
        {
            if (Value.HasValue)
            {
                _Value = Value.Value.ToString("HH:mm");
            }
            //set js file
            // JSO = await JS.InvokeAsync<IJSObjectReference>("import", "./js/FormFunctions.js");
            //set attributes
            if (Attributes.Keys.Count > 0)
            {
                foreach (var item in Attributes)
                {
                    _Attributes.Add(item.Key, item.Value);
                }
            }

            //Set Disabled First
            _Disabled = Disabled;
            RunDisabled();
            _Visible = Visible;
        }


        protected override void OnParametersSet()
        {
            RunDisabled();
            Visible = _Visible;
            
        }

        /// <summary>
        ///  Set Disabled To Component
        ///   اگر داخل کامپوننت دیسیبل  ثبت شده باشد دیگر نمی توان دیسیبل ست کرد که از این استفاده میشود
        /// </summary>
        protected void RunDisabled()
        {
            if (_Disabled)
            {
                AddAttribute("disabled", "true");
                RemoveAttribute("Required");
            }
            else
            {
                RemoveAttribute("disabled");
                if (Required.HasValue && Required.Value)
                {
                    AddAttribute("Required", "Required");
                }
            }
            Disabled = _Disabled;
        }

        #region class
        public bool AddAttribute(string? Key, string? Value)
        {
            try
            {
                var Attr = _Attributes.FirstOrDefault(p => p.Key == Key);
                if (Attr.Key is null)
                {
                    _Attributes.Add(Key, (!string.IsNullOrEmpty(Value)) ? (object)Value : null);
                }
                else
                {
                    RemoveAttribute(Attr.Key);
                    _Attributes.Add(Key, (!string.IsNullOrEmpty(Value)) ? (object)Value : null);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error In AddAttribute " + ex.Message);
                return false;
            }
            return true;
        }

        public bool RemoveAttribute(string? Key)
        {
            try
            {
                var Attr = _Attributes.Where(p => p.Key == Key);
                if (Attr.Count() > 0)
                {
                    foreach (var item in Attr)
                    {
                        if (item.Key is not null)
                        {
                            _Attributes.Remove(_Attributes.FirstOrDefault(p => p.Key == Key).Key);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error In RemoveAttribute " + ex.Message);
                return false;
            }
            return true;
        }

        public bool ToggleAttribute(string? Key, string? Value)
        {
            var Attr = _Attributes.FirstOrDefault(p => p.Key == Key);
            try
            {
                if (Attr.Key is null)
                {
                    _Attributes.Add(Key, (!string.IsNullOrEmpty(Value)) ? (object)Value : null);
                }
                else
                {
                    _Attributes.Remove(_Attributes.FirstOrDefault(p => p.Key == Key).Key);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error In ToggleAttribute " + ex.Message);
                return false;
            }
            return true;
        }

        public bool AddClassInput(string _NewClass)
        {
            try
            {
                if (!string.IsNullOrEmpty(_NewClass))
                {
                    InputClass += " " + _NewClass;
                    ClassChanged.InvokeAsync(InputClass);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error In AddClassInput " + ex.Message);
                return false;
            }
            return true;


        }

        public bool RemoveClassInput(string _Class)
        {
            try
            {
                if (!string.IsNullOrEmpty(InputClass))
                {
                    InputClass = InputClass.Replace(" " + _Class, string.Empty);
                    ClassChanged.InvokeAsync(InputClass);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error In RemoveClassInput " + ex.Message);
                return false;
            }

            return true;
        }

        public bool ToggleClassInput(string _Class)
        {
            try
            {
                if ((!string.IsNullOrEmpty(InputClass)) && InputClass.Contains(_Class))
                {
                    RemoveClassInput(_Class);
                }
                else
                {
                    AddClassInput(_Class);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error In ToggleClassInput " + ex.Message);
                return false;
            }
            return true;
        }

        public bool AddClass(string _NewClass)
        {
            try
            {
                CssClass += " " + _NewClass;
                ClassChanged.InvokeAsync(CssClass);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error In AddClass " + ex.Message);
                return false;
            }
            return true;
        }

        public bool RemoveClass(string _Class)
        {
            try
            {
                if (!string.IsNullOrEmpty(CssClass))
                {
                    CssClass = CssClass.Replace(" " + _Class, string.Empty);
                    ClassChanged.InvokeAsync(CssClass);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error In RemoveClass " + ex.Message);
                return false;
            }
            return true;
        }
        public bool ToggleClass(string _Class)
        {
            try
            {
                if ((!string.IsNullOrEmpty(CssClass)) && CssClass.Contains(_Class))
                {
                    RemoveClass(_Class);
                }
                else
                {
                    AddClass(_Class);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error In ToggleClass " + ex.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// تغییر فعال یا غیر فعال بودن کامپوننت
        /// </summary>
        /// <param name="val"></param>
        public void SetDisabled(bool val)
        {
            _Disabled = val;
        }
        /// <summary>
        /// نمایش یا عدم نمایش کامپوننت
        /// </summary>
        /// <param name="val"></param>
        public void SetVisible(bool val)
        {
             
            _Visible = val;
            //در صورتی که فیلد نمایش داده نشود کامپوننت آن غیر فعال میشود تا اگر اجباری بود در نظر گرفته نشود
            _Disabled = !val;

        }

        #endregion

        /// <summary>
        /// Set Time To Value
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public async Task ChangeValue(ChangeEventArgs e)
        {
            if (e.Value != null)
            {
                _Value = e.Value.ToString();

                var Item = TimeOnly.Parse(e.Value.ToString(), System.Globalization.CultureInfo.CurrentCulture);
                Value = TimeOnly.Parse(e.Value.ToString(), System.Globalization.CultureInfo.CurrentCulture);
                await ValueChanged.InvokeAsync(Item);
            }
        }
    }

}