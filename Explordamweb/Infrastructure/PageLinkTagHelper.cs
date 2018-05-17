using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Explordamweb.Models.ViewModels;
using System.Collections.Generic;

namespace Explordamweb.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }
        [ViewContext]
        [HtmlAttributeNotBound]

        public ViewContext ViewContext { get; set; }
        public PagingInfo PageModel { get; set; }
        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; }
            = new Dictionary<string, object>();

        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }


        public override void Process(TagHelperContext context,
        TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder result = new TagBuilder("div");
            if (PageModel.TotalPages == 1)
            {

            }
            else
            {
                if (PageModel.TotalPages < 5)
                {
                    for (int i = 1; i <= PageModel.TotalPages; i++)
                    {
                        TagBuilder tag = new TagBuilder("a");
                        PageUrlValues["GamesPage"] = i;
                        tag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                        if (PageClassesEnabled)
                        {
                            tag.AddCssClass(PageClass);
                            tag.AddCssClass(i == PageModel.CurrentPage
                            ? PageClassSelected : PageClassNormal);
                        }
                        tag.InnerHtml.Append(i.ToString());
                        result.InnerHtml.AppendHtml(tag);
                    }
                }
                else
                {
                    TagBuilder Firsttag = new TagBuilder("a");
                    PageUrlValues["GamesPage"] = 1;
                    Firsttag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                    if (PageClassesEnabled)
                    {
                        Firsttag.AddCssClass(PageClass);
                        Firsttag.AddCssClass(1 == PageModel.CurrentPage
                        ? PageClassSelected : PageClassNormal);
                    }
                    Firsttag.InnerHtml.Append("First Page");
                    result.InnerHtml.AppendHtml(Firsttag);
                    if (PageModel.CurrentPage <= 5)
                    {
                        for (int i = 1; i <= 6; i++)
                        {
                            TagBuilder tag = new TagBuilder("a");
                            PageUrlValues["GamesPage"] = i;
                            tag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                            if (PageClassesEnabled)
                            {
                                tag.AddCssClass(PageClass);
                                tag.AddCssClass(i == PageModel.CurrentPage
                                ? PageClassSelected : PageClassNormal);
                            }
                            tag.InnerHtml.Append(i.ToString());
                            result.InnerHtml.AppendHtml(tag);
                        }
                    }
                    else if (PageModel.CurrentPage >= PageModel.TotalPages - 5)
                    {
                        for (int i = PageModel.CurrentPage - 5; i <= PageModel.TotalPages; i++)
                        {
                            TagBuilder tag = new TagBuilder("a");
                            PageUrlValues["GamesPage"] = i;
                            tag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                            if (PageClassesEnabled)
                            {
                                tag.AddCssClass(PageClass);
                                tag.AddCssClass(i == PageModel.CurrentPage
                                ? PageClassSelected : PageClassNormal);
                            }
                            tag.InnerHtml.Append(i.ToString());
                            result.InnerHtml.AppendHtml(tag);
                        }
                    }
                    else
                    {
                        for (int i = PageModel.CurrentPage - 5; i <= PageModel.CurrentPage + 5; i++)
                        {
                            TagBuilder tag = new TagBuilder("a");
                            PageUrlValues["GamesPage"] = i;
                            tag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                            if (PageClassesEnabled)
                            {
                                tag.AddCssClass(PageClass);
                                tag.AddCssClass(i == PageModel.CurrentPage
                                ? PageClassSelected : PageClassNormal);
                            }
                            tag.InnerHtml.Append(i.ToString());
                            result.InnerHtml.AppendHtml(tag);
                        }
                    }
                    TagBuilder Lasttag = new TagBuilder("a");
                    PageUrlValues["GamesPage"] = PageModel.TotalPages;
                    Lasttag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                    if (PageClassesEnabled)
                    {
                        Lasttag.AddCssClass(PageClass);
                        Lasttag.AddCssClass(PageModel.TotalPages == PageModel.CurrentPage
                        ? PageClassSelected : PageClassNormal);
                    }
                    Lasttag.InnerHtml.Append("Last Page");
                    result.InnerHtml.AppendHtml(Lasttag);
                }
                output.Content.AppendHtml(result.InnerHtml);
            }
            
        }
    }
}