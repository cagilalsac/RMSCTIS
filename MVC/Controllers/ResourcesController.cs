#nullable disable
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;


//Generated from Custom Template.
namespace MVC.Controllers
{
    public class ResourcesController : Controller
    {
        // TODO: Add service injections here
        private readonly IResourceService _resourceService;

        public ResourcesController(IResourceService resourceService)
        {
            _resourceService = resourceService;
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
            ResourceModel resource = null; // TODO: Add get item service logic here
            if (resource == null)
            {
                return NotFound();
            }
            return View(resource);
        }

        // GET: Resources/Create
        public IActionResult Create()
        {
            // TODO: Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
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
                // TODO: Add insert service logic here
                return RedirectToAction(nameof(Index));
            }
            // TODO: Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            return View(resource);
        }

        // GET: Resources/Edit/5
        public IActionResult Edit(int id)
        {
            ResourceModel resource = null; // TODO: Add get item service logic here
            if (resource == null)
            {
                return NotFound();
            }
            // TODO: Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
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
                // TODO: Add update service logic here
                return RedirectToAction(nameof(Index));
            }
            // TODO: Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            return View(resource);
        }

        // GET: Resources/Delete/5
        public IActionResult Delete(int id)
        {
            ResourceModel resource = null; // TODO: Add get item service logic here
            if (resource == null)
            {
                return NotFound();
            }
            return View(resource);
        }

        // POST: Resources/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // TODO: Add delete service logic here
            return RedirectToAction(nameof(Index));
        }
	}
}
