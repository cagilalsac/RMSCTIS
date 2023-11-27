#nullable disable
using Business;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


//Generated from Custom Template.
namespace MVC.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly IResourceService _resourceService;
        private readonly IUserService _userService;

		public ResourcesController(IResourceService resourceService, IUserService userService)
		{
			_resourceService = resourceService;
			_userService = userService;
		}

		// GET: Resources
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
                // we send the message as model to the _Error partial view
                // which is created under ~/Views/Shared folder
                return View("_Error", "Resource not found!");
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
			// TODO: User list ViewBag
			return View(resource);
        }

        // GET: Resources/Edit/5
        public IActionResult Edit(int id)
        {
			// TODO: Resource service GetItem method
			ResourceModel resource = null;
            if (resource == null)
            {
				// TODO: Error partial view
			}
			// TODO: User list ViewBag
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
					// TODO: redirect to details
				}
                ModelState.AddModelError("", result.Message);
            }
			// TODO: User list ViewBag
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
