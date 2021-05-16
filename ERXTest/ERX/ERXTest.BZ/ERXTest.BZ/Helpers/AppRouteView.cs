using ERXTest.BZ.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Net;

namespace ERXTest.BZ.Helpers
{
    public class AppRouteView : RouteView
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }


        protected override void Render(RenderTreeBuilder builder)
        {
            base.Render(builder);
        }
    }
}