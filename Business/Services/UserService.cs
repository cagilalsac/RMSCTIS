using Business.Results;
using Business.Results.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business;

public interface IUserService
{
    // method definitions: method definitions must be created here in order to be used in the related controller
    IQueryable<UserModel> Query();
    Result Add(UserModel model);
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

    public Result Add(UserModel model)
    {
        // Way 1: Data case sensitivity can be simply eliminated by using ToUpper or ToLower methods in both sides,
		// Trim method can be used to remove the white spaces from the beginning and the end of the data.
        //User existingUser = _db.Users.SingleOrDefault(u => u.UserName.ToUpper() == model.UserName.ToUpper().Trim());
        //if (existingUser is not null)
        //    return new ErrorResult("User with the same user name already exists!");
        // Way 2:
        if (_db.Users.Any(u => u.UserName.ToUpper() == model.UserName.ToUpper().Trim()))
			return new ErrorResult("User with the same user name already exists!");

		// entity creation from the model
		User user = new User()
        {
            IsActive = model.IsActive,
            UserName = model.UserName.Trim(),
            Password = model.Password.Trim(),

			// Way 1:
			//RoleId = model.RoleId != null ? model.RoleId.Value : 0,
			// Way 2:
			//RoleId = model.RoleId is not null ? model.RoleId.Value : 0,
			// Way 3:
			//RoleId = model.RoleId.HasValue ? model.RoleId.Value : 0,
			// Way 4:
			//RoleId = model.RoleId.Value, // if we are sure that RoleId has a value
			// Way 5:
			RoleId = model.RoleId ?? 0,

            Status = model.Status
		};
		
		// adding entity to the related db set
        _db.Users.Add(user);
		
		// changes in all of the db sets are commited to the database with Unit of Work
        _db.SaveChanges(); 

        return new SuccessResult("User added successfully.");
    }
}
