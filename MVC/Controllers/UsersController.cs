#nullable disable
using Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

//Generated from Custom Template.
namespace MVC.Controllers
{
    public class UsersController : Controller
    {
        // Add service injections here
        #region User Service Constructor Injection
        private readonly IUserService _userService;

        // An object of type UserService which is implemented from the IUserService
        // interface is injected to this class through the constructor therefore
        // user CRUD and other operations can be performed with this object.
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        // GET: Users/GetList
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
        public JsonResult GetListJson()
        {
            var userList = _userService.Query().ToList();
            return Json(userList);
        }

        // GET: Users/Details/5
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
        public IActionResult Create()
        {
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            ViewData["RoleId"] = new SelectList(null, "Id", "Id");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add insert service logic here
                return RedirectToAction(nameof(Index));
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            ViewData["RoleId"] = new SelectList(null, "Id", "Id", user.RoleId);
            return View(user);
        }

        // GET: Users/Edit/5
        public IActionResult Edit(int id)
        {
            UserModel user = null; // TODO: Add get item service logic here
            if (user == null)
            {
                return NotFound();
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            ViewData["RoleId"] = new SelectList(null, "Id", "Id", user.RoleId);
            return View(user);
        }

        // POST: Users/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserModel user)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add update service logic here
                return RedirectToAction(nameof(Index));
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            ViewData["RoleId"] = new SelectList(null, "Id", "Id", user.RoleId);
            return View(user);
        }

        // GET: Users/Delete/5
        public IActionResult Delete(int id)
        {
            UserModel user = null; // TODO: Add get item service logic here
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // TODO: Add delete service logic here
            return RedirectToAction(nameof(Index));
        }
	}
}
