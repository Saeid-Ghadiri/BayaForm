using Baya.Models.ViewEngine;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Forms.Components
{
 public partial class DataGridBase : ComponentBase //, IAsyncDisposable
 {


     public DataGridBase()
     {

     }
     protected override async Task OnAfterRenderAsync(bool firstRender)
     {
         //if (firstRender)
         //{
         //     module = await JS.InvokeAsync<IJSObjectReference>(
         //        "import", "./Components/Dropdown.razor.js");

         //    await module.InvokeVoidAsync("Dropdown.initial");
         //}
     }

     //async ValueTask IAsyncDisposable.DisposeAsync()
     //{
     //    if (module is not null)
     //    {
     //        await module.DisposeAsync();
     //    }
     //}
 }
}
