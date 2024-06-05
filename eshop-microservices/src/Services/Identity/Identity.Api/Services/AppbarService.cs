using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Identity.Api.Services
{
    public class AppbarService

    {
        private readonly ILogger<AppbarService> _logger;
        private readonly IUrlHelper urlHelper;
        public List<SidebarItem> items = [];
        public AppbarService(IUrlHelperFactory factory, IActionContextAccessor action, ILogger<AppbarService> logger)
        {
            _logger = logger;
            urlHelper = factory.GetUrlHelper(action.ActionContext);
            items = new List<SidebarItem>(){
                  new SidebarItem()
                  {
                        Title = "Home",
                        FontAwesomeIcon = "fa-solid fa-house",
                        Action = "Index",
                        Controller = "Manage",
                        Type= SidebarItemType.NavItem,
                        Area = "Identity",
                  },
                  new SidebarItem()
                  {
                        Title = "Profile",
                        FontAwesomeIcon = "fa-regular fa-user",
                        Action = "Profile",
                        Controller = "Manage",
                        Type= SidebarItemType.NavItem,
                        Area = "Identity",
                  },
                  new SidebarItem()
                  {
                        Title = "Linking Account",
                        Action = "LinkLogin",
                        Controller = "Manage",
                        Type= SidebarItemType.NavItem,
                        Area="Identity",
                        FontAwesomeIcon="fa-solid fa-link"
                  },
                    new SidebarItem()
                  {
                        Title = "Change Password",
                        Action = "ChangePassword",
                        Controller = "Manage",
                        Type= SidebarItemType.NavItem,
                        Area="Identity",
                        FontAwesomeIcon="fa-solid fa-lock"
                  },



            };
        }


        public string RenderHtml()
        {
            var html = new StringBuilder();
            foreach (var item in items)
            {
                html.Append(item.RenderHtml(urlHelper));
            }
            return html.ToString();
        }
        public void SetActive(string controller, string action, string area)
        {
            foreach (var item in items)
            {
                if (controller == item.Controller && action == item.Action && area == item.Area)
                {
                    item.IsActive = true;
                    _logger.LogInformation(item.IsActive.ToString());
                    return;
                }
                else
                {
                    if (item.Items != null)
                    {
                        foreach (var subItem in item.Items)
                        {
                            if (controller == subItem.Controller && action == subItem.Action && area == subItem.Area)
                            {
                                item.IsActive = true;
                                subItem.IsActive = true;
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
    public enum SidebarItemType
    {
        Divider,
        Heading,
        NavItem
    }
    public class SidebarItem
    {
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public SidebarItemType Type { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
        public string FontAwesomeIcon { get; set; }
        public List<SidebarItem> Items { get; set; }
        public string collapseId { get; set; }
        public string CssClass { get; set; }
        public string GetUrl(IUrlHelper urlHelper)
        {
            return urlHelper.Action(Action, Controller, new { area = Area });
        }

        public string RenderHtml(IUrlHelper urlHelper)
        {
            var html = new StringBuilder();
            switch (Type)
            {
                case SidebarItemType.Divider:
                    html.Append("<hr class=\"sidebar-divider my-0\"/>");
                    break;
                case SidebarItemType.Heading:
                    html.Append($"<div class=\"sidebar-heading\">{Title}</div>");
                    break;
                case SidebarItemType.NavItem:
                    var target = Title == "File management" ? "_blank" : null;
                    if (Items == null)
                    {
                        var url = GetUrl(urlHelper);
                        var icon = (FontAwesomeIcon != null) ? $"<i class=\"{FontAwesomeIcon}\"></i>" : "";
                        CssClass ??= "nav-item";
                        if (IsActive) CssClass += " active";

                        html.Append(@$"
                    <li class=""{CssClass}"">
                        <a target=""{target}"" class=""nav-link"" href=""{url}"">
                            {icon}
                            <span>{Title}</span>
                        </a>
                    </li>
                    ");
                    }
                    else
                    {
                        var icon = (FontAwesomeIcon != null) ? $"<i class=\"{FontAwesomeIcon}\"></i>" : "";
                        CssClass = "nav-item";
                        if (IsActive) CssClass += " active";
                        var collapsedItem = "";
                        var collapse = "collapse";
                        if (IsActive) collapse += " show";
                        foreach (var item in Items)
                        {
                            var iconItem = (item.FontAwesomeIcon != null) ? $"<i class=\"{FontAwesomeIcon}\"></i>" : "";
                            var url = item.GetUrl(urlHelper);
                            var itemCssClass = "collapse-item d-flex";
                            if (item.IsActive) itemCssClass += " active";
                            collapsedItem += $"<a class=\"{itemCssClass}\" href=\"{url}\">{item.Title}</a>";
                        }
                        html.Append(@$"
                            <li class=""{CssClass}"">
                                <a class=""nav-link collapsed"" href=""#"" data-toggle=""collapse"" data-target=""#{collapseId}"" aria-expanded=""true""
                                    aria-controls=""{collapseId}"">
                                    {icon}
                                    <span>{Title}</span>
                                </a>
                                <div id=""{collapseId}"" class=""{collapse}"" aria-labelledby=""headingTwo"" data-parent=""#accordionSidebar"">
                                    <div class=""bg-white py-2 collapse-inner rounded"">
                                        {collapsedItem}
                                    </div>
                                </div>
                            </li>
                        ");
                    }
                    break;

                default: break;
            }

            return html.ToString();
        }
    }
}