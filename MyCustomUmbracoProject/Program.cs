using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .Build();

var _env = builder.Environment;

WebApplication app = builder.Build();

await app.BootUmbracoAsync();


app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

    app.UseDefaultFiles();
    app.UseStaticFiles(new StaticFileOptions
            {

                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                               Path.Combine(_env.ContentRootPath, "ClientApp/dist/umbraco-app")),
                RequestPath = "",
                OnPrepareResponse = ctx =>
                {
                    // Set cache control header to allow caching of static files
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000,immutable");
                }
            });

    app.UseEndpoints(endpoints =>
           {
               endpoints.MapControllers();

                /*
               endpoints.MapControllerRoute(
               name: "Default",
               pattern: "",
               defaults: new { controller = "Home", action = "Index" });
 endpoints.MapFallbackToController("Index", "Home");
 */
           });
           
            app.UseSpa(spa =>
            {

                spa.Options.SourcePath = "ClientApp";
                spa.UseReactDevelopmentServer(npmScript: "start-dev");

            });

await app.RunAsync();
