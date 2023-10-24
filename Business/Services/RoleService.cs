﻿using Business.Models;
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
				Name = r.Name.Substring(0, 1).ToUpper() + r.Name.Substring(1).ToLower()
			});
		}
	}
}
