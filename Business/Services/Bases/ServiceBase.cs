using DataAccess.Contexts;

namespace Business.Services.Bases
{
    public abstract class ServiceBase // all services will inherit from this abstract class, therefore Db object injection can be easily made
    {
        protected readonly Db _db; // we set protected as the accessibility modifier, so we can use this field in the inherited service classes

        protected ServiceBase(Db db) // DbContext object constructor injection
        {
            _db = db;
        }
    }
}
