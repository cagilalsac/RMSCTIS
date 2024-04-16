#nullable disable
using Business;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Controllers.Bases;


//Generated from Custom Template.
namespace MVC.Controllers
{
    [Authorize] // only logged in application users with authentication cookie can call the controller's actions
    public class ResourcesController : MvcControllerBase // instead of inheriting from the Controller, we inherit from MvcControllerBase to set the culture
    {
        private readonly IResourceService _resourceService;
        private readonly IUserService _userService;

		public ResourcesController(IResourceService resourceService, IUserService userService)
		{
			_resourceService = resourceService;
			_userService = userService;
		}

        // GET: Resources
        [AllowAnonymous] // if Authorize is used for the controller but an action is wanted to be called
                         // by all application users even without authentication cookie, AllowAnonymous is used
		public IActionResult Index()
        {
            // Way 1:
            //List<ResourceModel> resourceList = _resourceService.Query().ToList();
            // Way 2: since we implemented the GetList method in the service class
            // and defined it in the service interface to return the list of type ResourceModel,
            // from now on we should use it for resource listing operations
            List<ResourceModel> resourceList = _resourceService.GetList();

            return View(resourceList);
        }

        // GET: Resources/Details/5
        public IActionResult Details(int id)
        {
            // since we implemented the GetItem method in the service class
            // and defined it in the service interface to return the item of type ResourceModel,
            // from now on we should use it for getting one resource item operations
            ResourceModel resource = _resourceService.GetItem(id);
            if (resource == null)
            {
                // instead of returning 404 Not Found HTTP Status Code,
                // we send the message as model to the Error view
                // which is created under ~/Views/Shared folder
                return View("Error", "Resource not found!");
            }
            return View(resource);
        }

        // GET: Resources/Create
        public IActionResult Create()
        {
            // SelectList must be used for drop down list (select HTML tag),
            // MultiSelectList must be used for list box (select HTML tag with multiple HTML attribute)
            ViewBag.UserId = new MultiSelectList(_userService.Query().ToList(), "Id", "UserName");
            return View();
        }

        // POST: Resources/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ResourceModel resource)
        {
            if (ModelState.IsValid)
            {
                var result = _resourceService.Add(resource);
                if (result.IsSuccessful)
                {
					TempData["Message"] = result.Message;
					return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
			ViewBag.UserId = new MultiSelectList(_userService.Query().ToList(), "Id", "UserName");
			return View(resource);
        }

        // GET: Resources/Edit/5
        public IActionResult Edit(int id)
        {
            ResourceModel resource = _resourceService.GetItem(id);
            if (resource == null)
            {
                return View("Error", "Resource not found!");
			}
			ViewBag.UserId = new MultiSelectList(_userService.Query().ToList(), "Id", "UserName");
			return View(resource);
        }

        // POST: Resources/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ResourceModel resource)
        {
            if (ModelState.IsValid)
            {
                var result = _resourceService.Update(resource);
                if (result.IsSuccessful)
                {
					TempData["Message"] = result.Message;
                    // Way 1: 
                    //return RedirectToAction(nameof(Index));
                    // Way 2: redirection with route values
                    return RedirectToAction(nameof(Details), new { id = resource.Id });
				}
                ModelState.AddModelError("", result.Message);
            }
			ViewBag.UserId = new MultiSelectList(_userService.Query().ToList(), "Id", "UserName");
			return View(resource);
        }

        // GET: Resources/Delete/5
        public IActionResult Delete(int id)
        {
            var result = _resourceService.Delete(id);
			TempData["Message"] = result.Message;
			return RedirectToAction(nameof(Index));
		}
    }
}
