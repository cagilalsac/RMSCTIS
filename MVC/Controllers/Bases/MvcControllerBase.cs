using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace MVC.Controllers.Bases
{
    #region Localization
    // Way 2: Instead of managing localization in the Program.cs, managing it here is a better way
    public abstract class MvcControllerBase : Controller // all controllers will inherit from this abstract class
    {
        protected MvcControllerBase()
        {
            CultureInfo culture = new CultureInfo("en-US"); // for Turkish, "tr-TR" constructor parameter must be used,
                                                            // this will be our default culture for our MVC Web Application
                                                            // and can be changed by using session or a cookie
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
    #endregion
}
