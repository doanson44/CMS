using CMS.Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.WebApi.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[AllowAnonymous]
[Route("[controller]/[action]")]
public class HomeController : Controller
{
    public IActionResult RedirectToApp()
    {
        var userAgent = Request.Headers["User-Agent"].ToString().ToLower();
        if (userAgent.Contains("iphone"))
        {
            //return Redirect("instagram://user?username=duykhuong.huynh");
            //return Redirect("fb://profile/100009403303605");
            // intent route
            // SKU=MayTinh
            return Redirect("fb://page/?id=100009403303605");
        }
        else
        {
            //return Redirect("instagram://user?username=duykhuong.huynh");
            //return Redirect("http://instagram.com/_u/duykhuong.huynh");
            return Redirect("fb://facewebmodal/f?href=https://www.facebook.com/bill.gate");
        }
    }

    //Home/TestSendErrorToSlack
    public IActionResult TestSendErrorToSlack()
    {
        throw new BusinessException("Test error message!!!!!!!!!!!!!!");
    }
}
