using AspLesson.exceptions;
using AspLesson.Filters;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Version = "10.0";
        document.Info.Title = "Demo .NET 10 API";
        document.Info.Description = "This API demonstrates OpenAPI customization in a .NET 10 project.";
        document.Info.TermsOfService = new Uri("https://codewithmukesh.com/terms");
        document.Info.Contact = new OpenApiContact
        {
            Name = "Mukesh Murugan",
            Email = "mukesh@codewithmukesh.com",
            Url = new Uri("https://codewithmukesh.com")
        };
        document.Info.License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        };
        return Task.CompletedTask;
    });
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // app.UseHsts();
}


app.MapOpenApi();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapGet("/", () => "Hello World!");//.AddEndpointFilter<LoggingFilter>();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();