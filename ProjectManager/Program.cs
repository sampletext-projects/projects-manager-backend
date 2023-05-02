
using BusinessLogic;
using BusinessLogic.Configs;
using DataAccess;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using ProjectManager;
using ProjectManager.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddControllers()
    .AddNewtonsoftJson(options => { options.SerializerSettings.Configure(); });

builder.SetupSwagger();

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddBLL(builder.Configuration);

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection(nameof(JwtConfig)));

builder.SetupAuthentication();

// ---

var app = builder.Build();

await app.Services.MigrateDb();

app.UseForwardedHeaders(
    new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    }
);

if (!builder.Environment.IsProduction())
{
    app.UseSwagger(
        options =>
        {
            options.RouteTemplate = "swagger/{documentName}/swagger.json";
            options.PreSerializeFilters.Add(
                (swaggerDoc, httpReq) => swaggerDoc.Servers = new List<OpenApiServer>
                {
                    new() {Url = $"https://{httpReq.Host.Value}/api"},
                }
            );
        }
    );
    app.UseSwaggerUI();
}

app.UseCors(
    policyBuilder =>
        policyBuilder.WithOrigins(
                "http://localhost",
                "http://localhost:4200",
                "https://localhost",
                "https://localhost:4200"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
);

app.UseMiddleware<ExceptionCatcherMiddleware>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(
    endpoints =>
    {
        endpoints.MapControllers();
    }
);

await app.RunAsync();