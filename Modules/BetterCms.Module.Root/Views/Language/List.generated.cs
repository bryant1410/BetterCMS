﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 1 "..\..\Views\Language\List.cshtml"
    using BetterCms.Module.Root;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Views\Language\List.cshtml"
    using BetterCms.Module.Root.Content.Resources;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Views\Language\List.cshtml"
    using BetterCms.Module.Root.Mvc.Grids;
    
    #line default
    #line hidden
    
    #line 4 "..\..\Views\Language\List.cshtml"
    using BetterCms.Module.Root.ViewModels.Shared;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Language/List.cshtml")]
    public partial class _Views_Language_List_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Views_Language_List_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"bcms-window-settings\"");

WriteLiteral(">\r\n");

            
            #line 7 "..\..\Views\Language\List.cshtml"
    
            
            #line default
            #line hidden
            
            #line 7 "..\..\Views\Language\List.cshtml"
      
        var gridViewModel = new EditableGridViewModel
        {
            ShowMessages = true,
            TopBlockTitle = RootGlobalization.SiteSettings_Languages_Title,
            Columns = new List<EditableGridColumn>
            {
                new EditableGridColumn(RootGlobalization.SiteSettings_Languages_Code_Title, "Code", "code")
                    {
                        CustomBinding = "autocomplete: 'onlyExisting', attr: {tabindex: 50}",
                        HeaderAttributes = "style=\"width: 200px;\"",
                        AutoFocus = true
                    },
                new EditableGridColumn(RootGlobalization.SiteSettings_Languages_Name_Title, "Name", "name")
                    {
                        CustomBinding = "attr: {tabindex: 100}, hasfocus: hasNameFocus"
                    }
            }
        };
    
            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

WriteLiteral("    ");

            
            #line 28 "..\..\Views\Language\List.cshtml"
Write(Html.Partial(RootModuleConstants.EditableGridTemplate, gridViewModel));

            
            #line default
            #line hidden
WriteLiteral("\r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591
