using AspLesson.exceptions;
using AspLesson.Filters;
using AspLesson.middlewares;
using AspLesson.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Context;

Log.Logger = new LoggerConfiguration().WriteTo.Console().WriteTo
    .File("logs/log.txt", rollingInterval: RollingInterval.Day).CreateLogger();

// 输出到 Serilog 
Serilog.Debugging.SelfLog.Enable(msg => Log.Error(msg));
try
{
    var builder = WebApplication.CreateBuilder(args);
    Log.Information("Starting web host");

    builder.Services.AddSerilog((services, lc) =>
    {
        lc.ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext();
    });

// Add services to the container.
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<CacheResourceFilter>();
        // 全局注册验证过滤器，自动对所有 Action 的请求参数进行 FluentValidation 验证
        options.Filters.Add<ValidationActionFilter>();
    });

// 关闭 ApiController 的自动 ModelState 校验（由 ValidationActionFilter 接管）
    builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

// 注册 FluentValidation 验证器
    builder.Services.AddValidatorsFromAssemblyContaining<UserRegistrationRequestValidator>();

// 配置 ProblemDetails（用于异常处理）
    builder.Services.AddProblemDetails(options =>
    {
        options.CustomizeProblemDetails = ctx =>
        {
            ctx.ProblemDetails.Extensions["traceId"] = ctx.HttpContext.TraceIdentifier;
            ctx.ProblemDetails.Extensions["timestamp"] = DateTime.UtcNow;
            ctx.ProblemDetails.Instance = $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}";
        };
    });

// 注册全局异常处理器
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// 配置 OpenAPI
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
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        // app.UseHsts();
    }

    app.UseExceptionHandler();
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseMiddleware<CorrelationIdMiddleware>();
    app.UseSerilogRequestLogging();
    app.UseAuthorization();
    
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapStaticAssets();
    app.MapControllers();

    app.MapGet("/", () => "Hello World!").AddEndpointFilter<LoggingFilter>();

    app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}