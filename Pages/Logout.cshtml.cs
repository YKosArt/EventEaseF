using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;




public class LogoutModel : PageModel
{
    private readonly UserSessionService _session;

    public LogoutModel(UserSessionService session)
    {
        _session = session;
    }

    public async Task OnGet()
    {
        await HttpContext.SignOutAsync("Identity.Application");
      //   await _session.ClearSessionAsync();
        Response.Redirect("/?loggedOut=true");
    }
}



