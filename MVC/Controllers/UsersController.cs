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

        // GET: Users/List
        public IActionResult List()
        {
            // A query is executed and the result is stored in the collection
            // when ToList method is called.
            List<UserModel> userList = _userService.Query().ToList();

            // Way 1: 
            //return View(userList); // model will be passed to the List view under Views/Users folder
            // Way 2:
            return View("Index", userList); // model will be passed to the Index view under Views/Users folder
        }

        // GET: Users/Details/5
        public IActionResult Details(int id)
        {
            UserModel user = null; // TODO: Add get item service logic here
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
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
