namespace DataAccess.Records.Bases
{
    // Record is an abstract base class for all of the entities and models, which contains the common properties
    // such as Id, Guid, CreateDate, CreatedBy, UpdateDate, UpdatedBy, IsDeleted, etc.
    public abstract class Record
    {
        public int Id { get; set; }
    }
}
