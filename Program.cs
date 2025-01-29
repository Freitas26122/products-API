using Microsoft.EntityFrameworkCore;
using ProductsAPI.Domain.Commands;
using ProductsAPI.Domain.Utils;
using ProductsAPI.Domain.Queries;
using ProductsAPI.Domain.Listeners;
using ProductsAPI.Core.Filters;
using Dapper;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.OpenApi.Models;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Keys.SetIssuer(builder.Configuration.GetValue<string>("ProductsServer:Issuer"));
Keys.SetApiUrl(builder.Configuration.GetValue<string>("ProductsServer:ApiUrl"));

builder.Services.AddDbContext<CommandsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Commands"));
    options.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
});

builder.Services.AddDbContext<QueriesDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Queries"));
    options.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
});

builder.Services.AddDbContext<CommandsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Commands")));
builder.Services.AddDbContext<QueriesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Queries")));

builder.Services.AddScoped<ListenersHandler>(provider =>
{
    var secretKey = builder.Configuration.GetValue<string>(Keys.ACCESS_TOKEN_SIGNATURE);
    return new ListenersHandler(secretKey);
});
builder.Services.AddScoped<ValidateTokenFilter>();
builder.Services.AddScoped<CommandsHandler>();
builder.Services.AddScoped<QueriesHandler>();

DefaultTypeMap.MatchNamesWithUnderscores = true;
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.Conventions.Add(new VersionByNamespaceConvention());
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Products API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }, new List<string>()
        }
    });
    c.CustomSchemaIds(type =>
    {
        if (!type.IsGenericType || type.Name != "QueryResult`1")
            return GetSwaggerTypeName(type);
        var argumentType = type.GenericTypeArguments[0];
        var typeName = GetSwaggerTypeName(argumentType);
        return $"QueryResult<{typeName}>";
    });
});

builder.Services.AddMvc().AddJsonOptions(options => { });
builder.Services.AddAuthentication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductsAPI.v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

static string GetSwaggerTypeName(Type type)
{
    var pattern = @"^([0-9a-z_\-]+)\.Domain\.Queries\.([0-9a-z_\-]+).(?<namespace>[0-9a-z_\-]+)\.ViewModels\.(?<class>[0-9a-z_\-]+)$";
    var m = Regex.Match(type.FullName, pattern, RegexOptions.IgnoreCase);
    if (!m.Success)
        return type.Name;
    var ns = m.Groups["namespace"].Value;
    var cl = m.Groups["class"].Value;
    return ns + "." + cl;
}