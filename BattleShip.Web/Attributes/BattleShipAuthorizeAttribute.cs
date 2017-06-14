using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BattleShip.Database.Entities;

namespace BattleShip.Web.Attributes
{
    public class BattleShipAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string _role;

        public BattleShipAuthorizeAttribute(string role = null) : base()
        {
            _role = role;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (!string.IsNullOrEmpty(_role))
            {
                var rolesList = filterContext.HttpContext.Session["Roles"] as IEnumerable<Role>;

                if (!rolesList.Any(x => x.Name.Equals(_role, StringComparison.OrdinalIgnoreCase)))
                {
                    var url = new UrlHelper(filterContext.RequestContext);
                    var errorUrl = url.Action("Index", "Errors",
                        new

                        {
                            message = $"You are not authorized to make changes " +
                                      $"kontroler {filterContext.ActionDescriptor.ControllerDescriptor.ControllerName}" +
                                      $", akcja {filterContext.ActionDescriptor.ActionName}"
                        });
                    filterContext.Result = new RedirectResult(errorUrl);
                }
            }
        }
    }
}