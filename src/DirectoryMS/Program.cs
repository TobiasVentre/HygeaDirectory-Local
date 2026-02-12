using Microsoft.EntityFrameworkCore;
using DirectoryMS.Application.Interfaces;
using DirectoryMS.Application.UseCases;
using DirectoryMS.Infrastructure;
using DirectoryMS.Infrastructure.Persistence;
using DirectoryMS.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Hygea DirectoryMS API",
        Version = "v1",
        Description = "Directory microservice for user and fumigator approval management."
    });
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IUserUseCase, UserUseCase>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DirectoryDbContext>();
    dbContext.Database.Migrate();
}

app.UseMiddleware<ApiExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Hygea DirectoryMS API v1");
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
