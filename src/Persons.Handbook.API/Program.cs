using System.Text.Json.Serialization;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Persons.Handbook.API.Infrastructure.AutofacModules;
using Persons.Handbook.Application;
using Serilog;
using Persons.Handbook.API.Infrastructure.Middlewares;
using Persons.Handbook.Infrastructure.Context;
using Persons.Handbook.API.Infrastructure.FileManager;
using Persons.Handbook.Infrastructure.Configs;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

const string corsPolicy = nameof(corsPolicy);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(a => a.RegisterModule(new MediatorModule()));
builder.Host.ConfigureContainer<ContainerBuilder>(a => a.RegisterModule(new ApplicationModule()));

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy,
        config => config
            .WithOrigins(builder.Configuration.GetSection("Cors:AllowOrigins").Get<string[]>() ?? Array.Empty<string>())
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Content-Description", "content-disposition")
            .AllowCredentials());
});

builder.Services.AddApplication(builder.Configuration);

builder.Services.Configure<UploadFoldersConfig>(builder.Configuration.GetSection(nameof(UploadFoldersConfig)));
builder.Services.Configure<ConnectionsStringsConfig>(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddScoped<IFileManagerService, FileManagerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseCors(corsPolicy);

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

MigrateDatabase();

app.Run();

void MigrateDatabase()
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider
        .GetRequiredService<PersonsHandbookDbContext>();

    dbContext.Database.Migrate();

    DbInitializer.Init(dbContext);
}