namespace MVC.Settings
{
    /// <summary>
    /// This class is used for binding JSON property values of the AppSettings section 
    /// of the appsettings.json file. The section and class names must be the same,
    /// also the property names must be the same for successful binding in the Program.cs file. 
    /// </summary>
    public class AppSettings
    {
        // static is used for directly reaching the value of the properties using the class name, 
        // for example AppSettings.AppTitle or AppSettings.AppVersion without initialization.
        // We can easily reach these properties in views and controller actions.
        // static can be thought as a shared resource throughout the application, however
        // only shared resources such as application configuration which will effect all the users
        // of the application should be declared static, otherwise one change that a user performs
        // may effect other users when they are using the application.

        public static string AppTitle { get; set; }
        public static double AppVersion { get; set; }
    }
}
