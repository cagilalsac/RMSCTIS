#nullable disable

using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        const string SESSIONKEY = "favoriteskey"; // the key value to reach the related session data

        private readonly IResourceService _resourceService; // we need resource service injection so we can get the resource information by its id

        public FavoritesController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        public IActionResult Add(int resourceId) // action to add a new favorite to the session,
                                                 // parameter must be named resourceId because in the Resources/Index view,
                                                 // we defined the route value as asp-route-resourceId in the "Add to Favorites" link
        {
            // first we get the resource by id from the service:
            var resource = _resourceService.GetItem(resourceId);

            // then we get the previously stored favorites session data (if any):
            var favorites = GetSession();

            // then we add a new FavoriteModel instance to the favorites list if not added before (preventing duplicates):
            if (!favorites.Any(f => f.ResourceId == resource.Id))
            {
                favorites.Add(new FavoriteModel()
                {
                    ResourceId = resource.Id,
                    ResourceTitle = resource.Title,
                    ResourceScore = resource.Score ?? 0, // if resource's score is null, set 0, otherwise use resource's score value
                    ResourceScoreOutput = (resource.Score ?? 0).ToString("N1"), // format to the 1 decimal number format
                    UserName = User.Identity.Name
                });
            }

            // finally we update the favorites session data with the favorites list with newly added favorite item:
            SetSession(favorites);

            // we set the message to show in the Resources/Index view and redirect the application user to the Index action of the Resources controller
            TempData["Message"] = $"\"{resource.Title}\" added to favorites successfully.";
            return RedirectToAction("Index", "Resources");
        }

        public IActionResult Index() // action to list the favorites stored in the session
        {
            // we get the previously stored favorites session data (if any) and send it to the Index view:
            var favorites = GetSession();
            return View(favorites);
        }

        public IActionResult Remove(int resourceId) // action to remove the selected favorite from the session,
                                                    // parameter must be named resourceId because in the Favorites/Index view,
                                                    // we defined the route value as resourceId in the "Remove from Favorites" link
        {
            // first we get the previously stored favorites session data (if any):
            var favorites = GetSession();

            // then we remove the items from the favorites collection according to the resource id predicate:
            favorites.RemoveAll(f => f.ResourceId == resourceId);

            // finally we update the favorites session data with the favorites list with the removed favorite item:
            SetSession(favorites);

            // we redirect the application user to the Index action
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Clear() // action to remove all of the favorites session data according to the user name
        {
            // first we get the previously stored favorites session data (if any):
            var favorites = GetSession();

            // then we remove the items from the favorites collection according to the user name predicate:
            favorites.RemoveAll(f => f.UserName == User.Identity.Name);

            // finally we update the favorites session data with the favorites list with the removed favorite items:
            SetSession(favorites);

            // we redirect the application user to the Index action
            return RedirectToAction(nameof(Index));
        }



        private List<FavoriteModel> GetSession() // the method that returns the favorites collection from the session
        {
            var favorites = new List<FavoriteModel>(); // we first initialize an empty list,
                                                       // if there is no favorites data in the session, we will return this empty list

            var favoritesJson = HttpContext.Session.GetString(SESSIONKEY); // from HttpContext's Session reference property we use the GetString
                                                                           // method to get the session data in JSON format.
                                                                           // Other session get methods:
                                                                           // Get: returns byte[],
                                                                           // GetInt32: returns int

            if (!string.IsNullOrWhiteSpace(favoritesJson)) // if there is data in favoritesJson
            {
                favorites = JsonConvert.DeserializeObject<List<FavoriteModel>>(favoritesJson);
                // we convert the data in JSON format to the C# FavoriteModel List by using Newtonsoft.Json Library's deserialize method,
                // JSON to C#: Deserialize

                favorites = favorites.Where(f => f.UserName == User.Identity.Name).ToList();
                // we filter the list elements by the application user's user name to get the application user's favorite resources
            }

            return favorites; // we return either the favorites collection with no elements, or favorites collection with elements from the session
        }

        private void SetSession(List<FavoriteModel> favorites) // the method that sets the session data by the favorites list parameter
        {
            var favoritesJson = JsonConvert.SerializeObject(favorites); // we convert the C# FavoriteModel List to JSON format by using
                                                                        // Newtonsoft.Json Library's serialize method,
                                                                        // C# to JSON: Serialize

            HttpContext.Session.SetString(SESSIONKEY, favoritesJson); // from HttpContext's Session reference property we use the SetString
                                                                      // method to set the session data in JSON format.
                                                                      // Other session set methods:
                                                                      // Set: uses a byte[] parameter,
                                                                      // SetInt32: uses an int parameter
        }
    }
}
