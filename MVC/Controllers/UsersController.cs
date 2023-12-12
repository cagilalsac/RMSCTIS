#nullable disable
using Business;
using Business.Models;
using Business.Results.Bases;
using Business.Services;
using DataAccess.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

//Generated from Custom Template.
namespace MVC.Controllers
{
    public class UsersController : Controller
    {
        // Add service injections here
        #region User and Role Service Constructor Injections
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

		// Objects of type UserService and RoleService which are implemented from the IUserService
        // and IRoleService interfaces are injected to this class through the constructor therefore
		// user and role CRUD and other operations can be performed with these objects.
		public UsersController(IUserService userService, IRoleService roleService)
		{
			_userService = userService;
			_roleService = roleService;
		}
		#endregion

        // GET: Users/GetList
        // Way 1: GetList action will execute for only authenticated (logged in) application users 
		// with role names "admin" or "user"
        //[Authorize(Roles = "admin,user")]
        // Way 2: since we have only 2 roles and want this action to execute for both of them, 
		// we can use Authorize attribute to check application user's authentication cookie
        [Authorize]
		public IActionResult GetList()
        {
            // A query is executed and the result is stored in the collection
            // when ToList method is called.
            List<UserModel> userList = _userService.Query().ToList();

            // Way 1: 
            //return View(userList); // model will be passed to the GetList view under Views/Users folder
            // Way 2:
            return View("List", userList); // model will be passed to the List view under Views/Users folder
        }

        // Returning user list in JSON format:
        // GET: Users/GetListJson
        [Authorize(Roles = "admin")] // GetListJson action will execute for only authenticated (logged in) 
								     // application users with role name "admin" 
        public JsonResult GetListJson()
        {
            var userList = _userService.Query().ToList();
            return Json(userList);
        }

        // GET: Users/Details/5
        [Authorize(Roles = "admin")]
        public IActionResult Details(int id)
        {
            // Way 1:
            //UserModel user = _userService.Query().FirstOrDefault(u => u.Id == id);
            // Way 2:
            //UserModel user = _userService.Query().LastOrDefault(u => u.Id == id);
            // Way 3:
            UserModel user = _userService.Query().SingleOrDefault(u => u.Id == id);
            // The SingleOrDefault method, when used with a lambda expression, returns a single element (record) 
            // based on the specified condition. If the query returns multiple elements, it throws an exception, 
            // and if no elements match the condition, it returns a null reference.
            // You can use Single instead of SingleOrDefault and it throws an exception if multiple elements 
            // match the condition or if no elements are found.
            // Similarly, you can use FirstOrDefault instead of SingleOrDefault. When using a lambda expression, 
            // it returns the first element that matches the condition whether there are multiple matching elements or not. 
            // If no elements are found, it returns a null reference.
            // You can also use First instead of FirstOrDefault, and it throws an exception if no elements match the condition.
            // The LastOrDefault and Last methods perform operations on the last element found based on the specified condition, 
            // which can be considered as the reverse of FirstOrDefault and First.
            // Generally, methods ending with OrDefault that return a null result when no elements are found are used 
            // when dealing with a situation where no match is expected.

            if (user == null)
            {
                return NotFound(); // returns 404 HTTP not found status code
            }

            return View(user); // send user of type UserModel to the Views/Users/Details view
        }

        // GET: Users/Create
        // GET: Register: custom conventional route named register defined in Program.cs
        [HttpGet] // action method which is get by default when not written
        public IActionResult Create()
        {
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            
            // Creation of a SelectList object with parameters in order as role list, value member of each element
            // to be used in the background through related model property name (Id) and display member of each element
            // to be shown to the user through related model property name (Name) and assignment to the
            // ViewData through the Roles key.
            // Way 1 ViewData:
            //ViewData["Roles"] = new SelectList(_roleService.Query().ToList(), "Id", "Name");
            // Way 2: ViewBag which is the same object as ViewData
            ViewBag.Roles = new SelectList(_roleService.Query().ToList(), "Id", "Name");

			return View(); // returning Views/Users/Create view with no model data
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] // action method which is used for processing request data sent by a form or AJAX
        [ValidateAntiForgeryToken] // attribute for preventing Cross-Site Request Forgery (CSRF) 
        // Way 1: catching data with parameters through form input elements' name HTML attribute
        //public IActionResult Create(string UserName, string Password, bool IsActive, Statuses Status, int RoleId)
        // Way 2:
        public IActionResult Create(UserModel user) // since UserModel has properties for above parameters, it should be used as action parameter
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("admin")) // setting default user model values for registering new users operation
            {
                user.Status = Statuses.Junior;
                user.IsActive = true;
                user.RoleId = (int)Roles.User;

                ModelState.Remove(nameof(user.RoleId)); // if required like here, some model properties can be removed from the ModelState validation
            }
            // if user is authenticated and user is in admin role, create a new user from the values of all inputs from the Create view

            if (ModelState.IsValid) // validates the user action parameter (model) through data annotations above its properties
            {
                // If model data is valid, insert service logic should be written here.
                Result result = _userService.Add(user); // result referenced object can be of type SuccessResult or ErrorResult
                if (result.IsSuccessful)
                {
                    if (!User.Identity.IsAuthenticated || !User.IsInRole("admin")) // if register operation is successful, redirect to the "Account/Login" route
                        return Redirect("Account/Login"); // custom route redirection

                    // if create operation of admin is successful, redirect to the user list
                    // Way 1:
                    //return RedirectToAction("GetList");
                    // Way 2:
                    TempData["Message"] = result.Message; // if there is a redirection, the data should be carried with TempData to the redirected action's view
                    return RedirectToAction(nameof(GetList)); // redirection to the action specified of this controller to get the updated list from database
                }

				// Way 1:  carrying data from the action with ViewData
				//ViewBag.Message = result.Message; // ViewData["Message"] = result.Message;
				// Way 2: sends data to view's validation summary
				ModelState.AddModelError("", result.Message); 

            }

            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            // Way 1: SelectList constructor last parameter is the selected value
            //ViewBag.Roles = new SelectList(_roleService.Query().ToList(), "Id", "Name", user.RoleId);
			// Way 2:
			ViewBag.Roles = new SelectList(_roleService.Query().ToList(), "Id", "Name");

			// Returning the model containing the data entered by the user to the view therefore
            // the user can correct validation errors without losing data of the form input elements.
			return View(user); 
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            UserModel user = _userService.Query().SingleOrDefault(u => u.Id == id); // getting the model from the service
            if (user == null)
            {
                return NotFound(); // 404 HTTP Status Code
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            ViewBag.RoleId = new SelectList(_roleService.Query().ToList(), "Id", "Name"); // filling the roles
            return View(user); // returning the model to the view so that user can see the data to edit
        }

        // POST: Users/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(UserModel user)
        {
            if (ModelState.IsValid) // if no validation errors through data annotations of the model
            {
                var result = _userService.Update(user); // update the user in the service
                if (result.IsSuccessful)
                {
                    // if update operation result is successful, carry successful result message to the List view through the GetList action
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(GetList));
                }
                ModelState.AddModelError("", result.Message); // if unsuccessful, carry error result message to the view's validation summary
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            ViewBag.RoleId = new SelectList(_roleService.Query().ToList(), "Id", "Name"); // filling the roles
            return View(user); // returning the model sent by application user to the view so he/she can correct the validation errors and try again
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            UserModel user = _userService.Query().SingleOrDefault(u => u.Id == id); // getting the model from the service
            if (user == null)
            {
                return NotFound();
            }
            return View(user); // sending the model to the view so application user can see the details of the user
        }

        // POST: Users/Delete
		// ActionName attribute (Delete) renames and overrides the action method name (DeleteConfirmed) 
		// for the route so that it can be requested as not Users/DeleteConfirmed but as Users/Delete. 
		[HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _userService.DeleteUser(id);
			
			// carrying the service result message to the List view through GetList action
            TempData["Message"] = result.Message;
			 
            return RedirectToAction(nameof(GetList));
        }



        #region User Authentication
        // ~/Users/Login // we could invoke this action by calling https://exampledomain.com/Users/Login like other actions above
        // Way 1: we can change the route so that we can call this action by https://exampledomain.com/Account/Login
		//[Route("Account/Login")]
		// Way 2: we can use the action name by writing {action} and controller name by writing {controller}
        //[Route("Account/{action}")]
		// Way 3: we can also change the route template in the HttpGet action method
        [HttpGet("Account/{action}")]
        public IActionResult Login()
        {
            return View(); // returning the Login view to the user for entering the user name and password
        }

		// Way 1: changing the route by using Route attribute
        //[Route("Account/{action}")]
		// Way 2: changing the route by using the HttpPost action method
        [HttpPost("Account/{action}")] 
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
            
            // redirecting user to the home page
            return RedirectToAction("Index", "Home");
        }

		// ~/Account/Logout
        [HttpGet("Account/{action}")]
        public async Task<IActionResult> Logout()
        {
			// signing out the user by removing the authentication cookie from the client
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			
			// redirecting user to the home page
            return RedirectToAction("Index", "Home");
        }

		// ~/Account/AccessDenied
        [HttpGet("Account/{action}")]
        public IActionResult AccessDenied()
        {
			// returning the partial view "_Error" by sending the message of type string as model
            return View("_Error", "You don't have access to this operation!");
        }
        #endregion
    }
}
