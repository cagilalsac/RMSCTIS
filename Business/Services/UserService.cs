using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Business;

public interface IUserService
{
    // method definitions
    IQueryable<UserModel> Query();
}

public class UserService : IUserService // UserService is a IUserService (UserService implements IUserService)
{
    #region Db Constructor Injection
    private readonly Db _db;

    // An object of type Db which inherits from DbContext class is
    // injected to this class through the constructor therefore
    // user CRUD and other operations can be performed with this object.
    public UserService(Db db)
    {
        _db = db;
    }
    #endregion

    // method implementations of the method definitions in the interface
    public IQueryable<UserModel> Query()
    {
        // Query method will be used for generating SQL queries without executing them.
        // From the Users DbSet first order records by IsActive data descending
        // then for records with same IsActive data order UserName ascending
        // then for each element in the User entity collection map user entity
        // properties to the desired user model properties (projection) and return the query.
        // In Entity Framework Core, lazy loading (loading related data automatically without the need to include it) 
        // is not active by default if projection is not used. To use eager loading (loading related data 
        // on-demand with include), you can write the desired related entity property on the DbSet retrieved from 
        // the _db using the Include method either through a lambda expression or a string. If you want to include 
        // the related entity property of the included entity, you should write it through a delegate of type
        // included entity in the ThenInclude method. However, if the ThenInclude method is to be used, 
        // a lambda expression should be used in the Include method.
        return _db.Users.Include(e => e.Role).OrderByDescending(e => e.IsActive)
            .ThenBy(e => e.UserName)
            .Select(e => new UserModel()
            {
                // model - entity property assignments
                Id = e.Id,
                IsActive = e.IsActive,

                // Way 1: replacing password characters with an asterisk character in view
                //Password = e.Password,
                // Way 2: replacing password characters with an asterisk character in service
                Password = new string('*', e.Password.Length),

                RoleId = e.RoleId,
                Status = e.Status,
                UserName = e.UserName,

                // modified model - entity property assignments for displaying in views
                IsActiveOutput = e.IsActive ? "Yes" : "No",
                RoleNameOutput = e.Role.Name
            });
    }
}
