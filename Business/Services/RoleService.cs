using Business.Models;
using DataAccess.Contexts;

namespace Business.Services
{
	public interface IRoleService
	{
		IQueryable<RoleModel> Query();
	}

	public class RoleService : IRoleService
	{
		private readonly Db _db;

		public RoleService(Db db)
		{
			_db = db;
		}

		public IQueryable<RoleModel> Query()
		{
			return _db.Roles.OrderBy(r => r.Name).Select(r => new RoleModel()
			{
				Id = r.Id,
				Name = r.Name // todo: first letter to upper others to lower
			});
		}
	}
}
