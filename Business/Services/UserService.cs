using DataAccess.Contexts;

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
        return _db.Users.OrderByDescending(e => e.IsActive)
            .ThenBy(e => e.UserName)
            .Select(e => new UserModel()
            {
                Id = e.Id,
                IsActive = e.IsActive,
                Password = e.Password,
                RoleId = e.RoleId,
                Status = e.Status,
                UserName = e.UserName
            });
    }
}
