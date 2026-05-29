using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BibliothequeApp.Controllers
{
    // ─── Filtre d'authentification ────────────────────────────────────────────
    public class AuthorizeRolesAttribute : ActionFilterAttribute
    {
        private readonly string[] _roles;

        public AuthorizeRolesAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var role = session.GetString("Role");
            var username = session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            if (_roles.Length > 0 && !_roles.Contains(role))
            {
                context.Result = new RedirectToActionResult("AccesDenied", "Auth", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }

    // ─── BaseController ───────────────────────────────────────────────────────
    public class BaseController : Controller
    {
        protected string? CurrentRole => HttpContext.Session.GetString("Role");
        protected string? CurrentUsername => HttpContext.Session.GetString("Username");
        protected string? CurrentUserId => HttpContext.Session.GetString("UserId");
        protected string? CurrentMembreId => HttpContext.Session.GetString("MembreId");

        protected bool IsAdmin => CurrentRole == "Administrateur";
        protected bool IsUtilisateur => CurrentRole == "Utilisateur";
        protected bool IsMembre => CurrentRole == "Membre";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.Role = CurrentRole;
            ViewBag.Username = CurrentUsername;
            ViewBag.UserId = CurrentUserId;
            base.OnActionExecuting(context);
        }

        protected bool RequireAuth()
        {
            if (string.IsNullOrEmpty(CurrentUsername))
            {
                return false;
            }
            return true;
        }
    }
}