// Way 1:
// namespace DataAccess.Entities;
// Way 2:
namespace DataAccess // namespace DataAccess; can also be written
                     // therefore we don't need to use curly braces
{
    public class Role
    {
        // data member and member method usage example from Java:
        // private int id; // a class variable is called as a field in C#

        // public void setId(int id) // a class method is called as a behavior in C#
        // {
        //     this.id = id;
        // }

        // public int getId()
        // {
        //     return id;
        // }



        public int Id { get; set; } // this is called a property in C# which contains getters and setters 
    }
}
