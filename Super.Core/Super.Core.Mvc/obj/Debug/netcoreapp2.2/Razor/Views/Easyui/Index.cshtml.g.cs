#pragma checksum "D:\git\Super.Core.Template\Super.Core\Super.Core.Mvc\Views\Easyui\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "33920dbe5cb0bed59777f19d808b3a7628f61125"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Easyui_Index), @"mvc.1.0.view", @"/Views/Easyui/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Easyui/Index.cshtml", typeof(AspNetCore.Views_Easyui_Index))]
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
#line 1 "D:\git\Super.Core.Template\Super.Core\Super.Core.Mvc\Views\_ViewImports.cshtml"
using Super.Core.Mvc;

#line default
#line hidden
#line 2 "D:\git\Super.Core.Template\Super.Core\Super.Core.Mvc\Views\_ViewImports.cshtml"
using Super.Core.Mvc.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"33920dbe5cb0bed59777f19d808b3a7628f61125", @"/Views/Easyui/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cca5d3f88d1d89f5146be4877fb16a5d7189f533", @"/Views/_ViewImports.cshtml")]
    public class Views_Easyui_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "D:\git\Super.Core.Template\Super.Core\Super.Core.Mvc\Views\Easyui\Index.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Views/Shared/_PsaLayout.cshtml";

#line default
#line hidden
            BeginContext(93, 6, true);
            WriteLiteral("\r\n\r\n\r\n");
            EndContext();
            DefineSection("West", async() => {
                BeginContext(113, 61, true);
                WriteLiteral("\r\n\r\n    11111\r\n    <button onclick=\"ajaxTest()\">按钮</button>\r\n");
                EndContext();
            }
            );
            BeginContext(177, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(196, 755, true);
                WriteLiteral(@"

    <script>


        function ajaxTest() {

            var data = {
                id:1,
                name:""yyg"",
                cars: [""car1"", ""car2"", ""car3""]
            };

            //$.ajax({
            //    url: ""Easyui/Post"",
            //    type: 'post',
            //    data: JSON.stringify(data),
            //    //dataType: ""json"",
            //    contentType:""application/json;charset=utf-8"",
            //    success: function (r) {

            //    },
            //    error: function (r) {
            //        alert(r);
            //    }
            //});

            var str = ""Easyui/Post"";

            $.Pims.Ajax.Post(""Easyui/Post"", data);
        }


    </script>
    ");
                EndContext();
            }
            );
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