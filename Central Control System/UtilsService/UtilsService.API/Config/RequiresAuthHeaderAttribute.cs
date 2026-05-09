namespace UtilsService.API.Config
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class RequiresAuthHeaderAttribute : Attribute
    {
    }
}
