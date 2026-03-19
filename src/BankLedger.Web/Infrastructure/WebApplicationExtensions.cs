namespace BankLedger.Web.Infrastructure;

using System.Reflection; //used for runtime inspection lets the program ask: what classes exist in this assembly, what methods properties does this type have, inheritance? 

public static class WebApplicationExtensions
{
    //this is a helper method to map endpoint groups to the web app, it will be using the group name as the route prefix and the tags for swagger documentation
    public static RouteGroupBuilder MapEndpointGroup(this WebApplication app, EndpointGroupBase endpointGroup)
    {
        var groupName = endpointGroup.GroupName ?? endpointGroup.GetType().Name.ToLower();

        var group = app.MapGroup($"/api/{groupName}").WithTags(groupName);

        endpointGroup.Map(group);

        return group;
    }

    //this method will scan the assembly for all types that inherit from EndpointGroupBase and map them to the web app
    public static WebApplication MapEndpoints(this WebApplication app)
    {

        //sole purpose 
        //instead of doing app.MapGroup("/api/accounts") in Program.cs
        //we do app.MapEndpoints() and it will automatically find all endpoint groups and map them to the web app, this way we can keep our Program.cs clean and organized

        var endpointGroupType = typeof(EndpointGroupBase);

        //gets the current running compiled project/assembly
        var assembly = Assembly.GetExecutingAssembly();

        //gets the pubnlic types in that assembly
        var endpointGroupTypes = assembly.GetExportedTypes()
            .Where(t => t.IsSubclassOf(endpointGroupType));

        //instead of manually mapping the endpoint app.MapEndpointGroup(new Account()), app.MapEndpointGroup(new Users()), we use the assembly and have dynamic discovery at runtime

        foreach (var type in endpointGroupTypes)
        {
            if (Activator.CreateInstance(type) is EndpointGroupBase endpointGroup)
            {
                app.MapEndpointGroup(endpointGroup);
            }
        }

        return app;
    }
}