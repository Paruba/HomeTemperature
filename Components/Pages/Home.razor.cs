using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boiler.Mobile.Components.Pages;

public partial class Home
{
    [Inject] IHttpContextAccessor _httpContext {  get; set; }
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }
}
