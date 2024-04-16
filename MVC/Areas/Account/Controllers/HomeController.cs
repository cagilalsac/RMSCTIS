using Business.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Business;
using DataAccess.Enums;

namespace MVC.Areas.Account.Controllers
{
    [Area("Account")]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        // ~/Account/Home/Login // we can invoke this action by calling https://exampledomain.com/Account/Home/Login
        // GET: ~/Login
        // Way 1: we can change the route so that we can call this action by https://exampledomain.com/Login
        //[Route("/Login")]
        // Way 2: we can use the action name by writing {action}, controller name by writing {controller}, area name by writing {area}
        // and {id} by writing {id}, for example "/{area}/{controller}/{action}/{id}"
        //[Route("/{action}")]
        // Way 3: we can also change the route template in the HttpGet action method
        [HttpGet("/{action}")]
        public IActionResult Login()
        {
            return View(); // returning the Login view to the user for entering the user name and password
        }

        // POST: ~/Login
        // Way 1: changing the route by using Route attribute
        //[Route("/{action}")]
        // Way 2: changing the route by using the HttpPost action method
        [HttpPost("/{action}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserModel user)
        {
            // checking the active user from the database table by the user name and password
            var existingUser = _userService.Query().SingleOrDefault(u => u.UserName == user.UserName && u.Password == user.Password && u.IsActive);
            if (existingUser is null) // if an active user with the entered user name and password can't be found in the database table
            {
                ModelState.AddModelError("", "Invalid user name and password!"); // send the invalid message to the view's validation summary 
                return View(); // returning the Login view
            }

            // Creating the claim list that will be hashed in the authentication cookie which will be sent with each request to the web application.
            // Only non-critical user data, which will be generally used in the web application such as user name to show in the views or user role
            // to check if the user is authorized to perform specific actions, should be put in the claim list.
            // Critical data such as password must never be put in the claim list!
            List<Claim> userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, existingUser.UserName),
                new Claim(ClaimTypes.Role, existingUser.RoleNameOutput)
            };

            // creating an identity by the claim list and default cookie authentication
            var userIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

            // creating a principal by the identity
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            // signing the user in to the MVC web application and returning the hashed authentication cookie to the client
            await HttpContext.SignInAsync(userPrincipal);
            // Methods ending with "Async" should be used with the "await" (asynchronous wait) operator therefore
            // the execution of the task run by the asynchronous method can be waited to complete and the
            // result of the method can be used. If the "await" operator is used in a method, the method definition
            // must be changed by adding "async" keyword before the return type and the return type must be written 
            // in "Task". If the method is void, only "Task" should be written.

            // redirecting user to the home page which has the controller action route /Home/Index without area
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // GET: ~/Logout
        [HttpGet("/{action}")]
        public async Task<IActionResult> Logout()
        {
            // signing out the user by removing the authentication cookie from the client
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // redirecting user to the home page which has the controller action route /Home/Index without area
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // GET: ~/AccessDenied
        [HttpGet("/{action}")]
        public IActionResult AccessDenied()
        {
            // returning the view "Error" by sending the message of type string as model
            return View("Error", "You don't have access to this operation!");
        }

        // GET: ~/Register
        [HttpGet("/{action}")]
        public IActionResult Register()
        {
            return View();
        }

        // POST: ~/Register
        [HttpPost("/{action}")]
        public IActionResult Register(UserModel user)
        {
            // setting default user model values for registering new users operation
            user.Status = Statuses.Junior;
            user.IsActive = true;
            user.RoleId = (int)Roles.User;

            ModelState.Remove(nameof(user.RoleId)); // if required like here, some model properties can be removed from the ModelState validation

            if (ModelState.IsValid)
            {
                var result = _userService.Add(user);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Login));
                ModelState.AddModelError("", result.Message);
            }
            return View(user);
        }
    }
}
