#pragma checksum "C:\Users\Katya\source\repos\CourseWork\CourseWork\Views\Orders\ShowCounter.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8d464a113c714b0f8c4721625311557c1a37c684"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Orders_ShowCounter), @"mvc.1.0.view", @"/Views/Orders/ShowCounter.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Katya\source\repos\CourseWork\CourseWork\Views\_ViewImports.cshtml"
using CourseWork;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Katya\source\repos\CourseWork\CourseWork\Views\_ViewImports.cshtml"
using CourseWork.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8d464a113c714b0f8c4721625311557c1a37c684", @"/Views/Orders/ShowCounter.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1cf70ce142b2c8e1070383ff6d97537d03c63b95", @"/Views/_ViewImports.cshtml")]
    public class Views_Orders_ShowCounter : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\Users\Katya\source\repos\CourseWork\CourseWork\Views\Orders\ShowCounter.cshtml"
  
    ViewData["Title"] = "ShowCounter";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n<!DOCTYPE html>\r\n\r\n<html>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8d464a113c714b0f8c4721625311557c1a37c6843515", async() => {
                WriteLiteral("\r\n    <title>Warehouse</title>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8d464a113c714b0f8c4721625311557c1a37c6844513", async() => {
                WriteLiteral(@"
    <div style=""padding-top: 60px;"">
    <div style=""        background-color: #ffc107;
        opacity: 0.8;
"">
    <table style=""background-color: #ffc107; color: #333334; font-size: 18px; opacity: 1.0;"">
        <tr>
            <th>Algorithm</th>
            <th>Average time</th>
            <th>Better cases</th>
            
        </tr>
            <tr>
                <td>Frank-Wolf</td>
                <td>");
#nullable restore
#line 27 "C:\Users\Katya\source\repos\CourseWork\CourseWork\Views\Orders\ShowCounter.cshtml"
               Write(ViewBag.Counter.WolfAvg);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                <td>");
#nullable restore
#line 28 "C:\Users\Katya\source\repos\CourseWork\CourseWork\Views\Orders\ShowCounter.cshtml"
               Write(ViewBag.Counter.FrankBetter);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n            </tr>\r\n            <tr>\r\n                <td>Genetic</td>\r\n                <td>");
#nullable restore
#line 32 "C:\Users\Katya\source\repos\CourseWork\CourseWork\Views\Orders\ShowCounter.cshtml"
               Write(ViewBag.Counter.GeneticAvg);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                <td>");
#nullable restore
#line 33 "C:\Users\Katya\source\repos\CourseWork\CourseWork\Views\Orders\ShowCounter.cshtml"
               Write(ViewBag.Counter.GeneticBetter);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n            </tr>\r\n            <tr>\r\n                <td>Simulated Annealing</td>\r\n                <td>");
#nullable restore
#line 37 "C:\Users\Katya\source\repos\CourseWork\CourseWork\Views\Orders\ShowCounter.cshtml"
               Write(ViewBag.Counter.AnnealAvg);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                <td>");
#nullable restore
#line 38 "C:\Users\Katya\source\repos\CourseWork\CourseWork\Views\Orders\ShowCounter.cshtml"
               Write(ViewBag.Counter.AnnealBetter);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n            </tr>\r\n    </table>\r\n        </div>\r\n        </div>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</html>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591